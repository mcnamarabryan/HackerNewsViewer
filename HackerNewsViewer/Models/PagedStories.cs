namespace HackerNewsViewer.Models
{
    public class PagedStories
    {
        public List<StoryDto> Stories { get; set; } = new();
        public int Total { get; set; }
    }
}
