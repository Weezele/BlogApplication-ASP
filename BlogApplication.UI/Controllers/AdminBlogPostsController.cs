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

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            // Retrieve the Result from the repositort
            var blogPost = await blogPostRepository.GetAsync(id);
            var tagsDomainModel = await tagRepository.GetAllTagsAsync();


            if (blogPost != null)
            {
                // map the domain model into the view model
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    Author = blogPost.Author,
                    ImageUrl = blogPost.ImageUrl,
                    UrlHandler = blogPost.UrlHandler,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    Visible = blogPost.Visible,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString(),
                    }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray(),
                };
                return View(model);
            }

            // pass data to view
            return View(null);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            // map view model back to domain model
            var blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                ImageUrl = editBlogPostRequest.ImageUrl,
                ShortDescription = editBlogPostRequest.ShortDescription,
                PublishedDate = editBlogPostRequest.PublishedDate,
                Visible = editBlogPostRequest.Visible,
                UrlHandler = editBlogPostRequest.UrlHandler,
            };

            // Map tags into domain model
            var selectedTags = new List<Tag>();
            foreach (var selectedTag in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);

                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }

            blogPostDomainModel.Tags = selectedTags;

            // Submit Information to repository to update
            var updatedBlod = await blogPostRepository.UpdateAsync(blogPostDomainModel);
            if (updatedBlod != null)
            {
                return RedirectToAction("Edit");
            }
            return RedirectToAction("Edit");




        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            var deletedBlogPost = await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);

            if (deletedBlogPost != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new {id = editBlogPostRequest.Id});
        }
    }
}
