using Microsoft.AspNetCore.Mvc;
using HackerNewsViewer.Services;

namespace HackerNewsViewer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoriesController : ControllerBase
{
    private readonly IHackerNewsService _service;

    public StoriesController(IHackerNewsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<PagedStories>> Get([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (page < 1 || pageSize < 1)
        {
            return BadRequest("Invalid page or pageSize.");
        }
        var result = await _service.GetNewStoriesAsync(search, page, pageSize);
        return Ok(result);
    }
}