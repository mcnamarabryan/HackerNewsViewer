using HackerNewsViewer;
using HackerNewsViewer.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Moq.Protected;
using System.Net;
using Xunit;

namespace HackerNewsViewer.Tests;

public class HackerNewsServiceTests
{
    [Fact]
    public async Task GetNewStoriesAsync_FetchesAndCachesStories()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage req, CancellationToken ct) =>
            {
                var resp = new HttpResponseMessage(HttpStatusCode.OK);
                if (req.RequestUri!.ToString().EndsWith("newstories.json"))
                {
                    resp.Content = new StringContent("[1,2]");
                }
                else if (req.RequestUri.ToString().EndsWith("1.json"))
                {
                    resp.Content = new StringContent(@"{""id"":1,""type"":""story"",""title"":""Test1"",""url"":""http://test1""}");
                }
                else if (req.RequestUri.ToString().EndsWith("2.json"))
                {
                    resp.Content = new StringContent(@"{""id"":2,""type"":""story"",""title"":""Test2""}"); // No URL
                }
                return resp;
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var cache = new MemoryCache(new MemoryCacheOptions());
        var service = new HackerNewsService(httpClient, cache);

        // Act
        var result1 = await service.GetNewStoriesAsync(null, 1, 10);
        var result2 = await service.GetNewStoriesAsync(null, 1, 10); // Should use cache

        // Assert
        Assert.Equal(2, result1.Total);
        Assert.Equal("https://news.ycombinator.com/item?id=2", result1.Stories[1].Url); // Handled missing URL
        mockHandler.Protected().Verify("SendAsync", Times.Exactly(3), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()); // Added parameter matchers
    }

    [Fact]
    public async Task GetNewStoriesAsync_FiltersBySearch()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage req, CancellationToken ct) =>
            {
                var resp = new HttpResponseMessage(HttpStatusCode.OK);
                if (req.RequestUri!.ToString().EndsWith("newstories.json"))
                {
                    resp.Content = new StringContent("[1,2]");
                }
                else if (req.RequestUri.ToString().EndsWith("1.json"))
                {
                    resp.Content = new StringContent(@"{""id"":1,""type"":""story"",""title"":""Test1"",""url"":""http://test1""}");
                }
                else if (req.RequestUri.ToString().EndsWith("2.json"))
                {
                    resp.Content = new StringContent(@"{""id"":2,""type"":""story"",""title"":""Test2""}"); // No URL
                }
                return resp;
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var cache = new MemoryCache(new MemoryCacheOptions());
        var service = new HackerNewsService(httpClient, cache);

        // Act
        var result = await service.GetNewStoriesAsync("test1", 1, 10);

        // Assert
        Assert.Equal(1, result.Total);
        Assert.Equal("Test1", result.Stories[0].Title);
        mockHandler.Protected().Verify("SendAsync", Times.Exactly(3), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()); // Added parameter matchers
    }
}