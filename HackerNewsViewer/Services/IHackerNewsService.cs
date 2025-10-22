namespace HackerNewsViewer.Services;

public interface IHackerNewsService
{
    Task<PagedStories> GetNewStoriesAsync(string? search, int page, int pageSize);
}

public class PagedStories
{
    public List<StoryDto> Stories { get; set; } = new();
    public int Total { get; set; }
}

public class StoryDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}