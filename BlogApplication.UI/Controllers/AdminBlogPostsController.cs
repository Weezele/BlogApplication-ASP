using BlogApplication.UI.Models.Domain;
using BlogApplication.UI.Models.ViewModels;
using BlogApplication.UI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogApplication.UI.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await tagRepository.GetAllTagsAsync();
            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            // Map view model to domain model
            var blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                Author = addBlogPostRequest.Author,
                Content = addBlogPostRequest.Content,
                ImageUrl = addBlogPostRequest.ImageUrl,
                PageTitle = addBlogPostRequest.PageTitle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                ShortDescription = addBlogPostRequest.ShortDescription,
                UrlHandler = addBlogPostRequest.UrlHandler,
                Visible = addBlogPostRequest.Visible,

            };

            // Map Tags from selected tags 
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);
                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }
            // Mapping tags back to domain model
            blogPost.Tags = selectedTags;

            await blogPostRepository.AddAsync(blogPost);

            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            // Call the repository
            var blogPost = await blogPostRepository.GetAllAsync();

            return View(blogPost);
        }
    }
}
