using HackerNewsViewer;
using HackerNewsViewer.Models; 
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;

namespace HackerNewsViewer.Tests;

public class StoriesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public StoriesControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            // Mock services if needed for integration without external calls
        });
    }

    [Fact]
    public async Task Get_ReturnsPagedStories()
    {
        var client = _factory.CreateClient();
        var response = await client.GetFromJsonAsync<PagedStories>("/api/stories?page=1&pageSize=20");

        Assert.NotNull(response);
        Assert.True(response.Total > 0);
    }
}