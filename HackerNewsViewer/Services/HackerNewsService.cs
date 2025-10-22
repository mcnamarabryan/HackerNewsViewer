using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNewsViewer.Services;

public class HackerNewsService : IHackerNewsService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private const string BaseUrl = "https://hacker-news.firebaseio.com/v0/";
    private const string CacheKey = "new-stories";

    public HackerNewsService(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<PagedStories> GetNewStoriesAsync(string? search, int page = 1, int pageSize = 20)
    {
        var stories = await GetOrFetchStories();
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLowerInvariant();
            stories = stories.Where(s => s.Title.ToLowerInvariant().Contains(search)).ToList();
        }

        var total = stories.Count;
        var pagedStories = stories.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return new PagedStories { Stories = pagedStories, Total = total };
    }

    private async Task<List<StoryDto>> GetOrFetchStories()
    {
        if (_cache.TryGetValue(CacheKey, out List<StoryDto>? cachedStories) && cachedStories != null)
        {
            return cachedStories;
        }

        var stories = await FetchNewStories();
        var cacheOptions = new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) };
        _cache.Set(CacheKey, stories, cacheOptions);
        return stories;
    }

    private async Task<List<StoryDto>> FetchNewStories()
    {
        var idsResponse = await _httpClient.GetStringAsync($"{BaseUrl}newstories.json");
        var ids = JsonSerializer.Deserialize<List<long>>(idsResponse) ?? new List<long>();

        // Limit to first 200 for performance; HN allows up to 500 but fetching all in parallel is acceptable
        ids = ids.Take(200).ToList();

        var fetchTasks = ids.Select(async id =>
        {
            var itemResponse = await _httpClient.GetStringAsync($"{BaseUrl}item/{id}.json");
            using var itemDoc = JsonDocument.Parse(itemResponse);
            var root = itemDoc.RootElement;

            if (root.TryGetProperty("type", out var typeProp) &&
                (typeProp.GetString() == "story" || typeProp.GetString() == "job" || typeProp.GetString() == "poll"))
            {
                var title = root.TryGetProperty("title", out var titleProp) ? titleProp.GetString() ?? "" : "";
                var url = root.TryGetProperty("url", out var urlProp) ? urlProp.GetString() ?? "" : "";
                if (string.IsNullOrEmpty(url))
                {
                    url = $"https://news.ycombinator.com/item?id={id}";
                }
                return new StoryDto { Id = id, Title = title, Url = url };
            }
            return null;
        });

        var results = await Task.WhenAll(fetchTasks);
        return results.Where(s => s != null).Cast<StoryDto>().ToList();
    }
}