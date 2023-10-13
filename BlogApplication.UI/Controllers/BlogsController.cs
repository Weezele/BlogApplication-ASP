using BlogApplication.UI.Models.ViewModels;
using BlogApplication.UI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplication.UI.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;

        public BlogsController(IBlogPostRepository blogPostRepository, IBlogPostLikeRepository blogPostLikeRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandler)
        {
           var blogPost =  await blogPostRepository.GetByUrlHandleAsync(urlHandler);
            var blogDetailsViewModel = new BlogDetailsViewModel();



            if (blogPost != null)
            {
                var totalLikes = await blogPostLikeRepository.GetTotalLikes(blogPost.Id);

                blogDetailsViewModel = new BlogDetailsViewModel
                {
                    Id = blogPost.Id,
                    Content = blogPost.Content,
                    Author = blogPost.Author,
                    PageTitle = blogPost.PageTitle,
                    Heading = blogPost.Heading,
                    ImageUrl = blogPost.ImageUrl,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    Tags = blogPost.Tags,
                    UrlHandler = urlHandler,
                    Visible = blogPost.Visible,
                    TotalLikes = totalLikes,
                };

               
            }

            return View(blogDetailsViewModel);
        }

    }
}
