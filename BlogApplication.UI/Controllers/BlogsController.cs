using BlogApplication.UI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplication.UI.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;

        public BlogsController(IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandler)
        {
           var blogPost =  await blogPostRepository.GetByUrlHandleAsync(urlHandler);
            return View(blogPost);
        }
    }
}
