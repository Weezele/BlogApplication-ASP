using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogApplication.UI.Models.ViewModels
{
    public class AddBlogPostRequest
    {
        public string? Heading { get; set; }

        public string? PageTitle { get; set; }

        public string? Content { get; set; }

        public string? ShortDescription { get; set; }

        public string? ImageUrl { get; set; }

        public string? UrlHandler { get; set; }

        public DateTime PublishedDate { get; set; }

        public string? Author { get; set; }

        public bool Visible { get; set; }

        // Display Tags

        public IEnumerable<SelectListItem> Tags { get; set; }

        // Collect Tag
        public string?[] SelectedTags { get; set; } = Array.Empty<string>();

    }
}
