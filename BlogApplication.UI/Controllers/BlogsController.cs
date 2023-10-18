using BlogApplication.UI.Models.Domain;
using BlogApplication.UI.Models.ViewModels;
using BlogApplication.UI.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplication.UI.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IBlogPostCommentRepository blogPostCommentRepository;

        public BlogsController
            (
            IBlogPostRepository blogPostRepository,
            IBlogPostLikeRepository blogPostLikeRepository,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IBlogPostCommentRepository blogPostCommentRepository
            )
        {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.blogPostCommentRepository = blogPostCommentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandler)
        {
            var liked = false;
            var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandler);
            var blogDetailsViewModel = new BlogDetailsViewModel();
            if (blogPost != null)
            {
                var totalLikes = await blogPostLikeRepository.GetTotalLikes(blogPost.Id);

                if (signInManager.IsSignedIn(User))
                {
                    var likeForBlog = await blogPostLikeRepository.GetLikesForBlog(blogPost.Id);

                    var userId = userManager.GetUserId(User);

                    if (userId != null)
                    {
                        var likeFromUser = likeForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                        liked = likeFromUser != null;

                    }
                }

                var blogCommentsDomainModel = await blogPostCommentRepository.GetCommentsByBlogIdAsync(blogPost.Id);

                var blogCommentsForView = new List<BlogComment>();
                foreach (var blogComment in blogCommentsDomainModel)
                {
                    blogCommentsForView.Add(new BlogComment
                    {
                        Description = blogComment.Description,
                        DateAdded = blogComment.DateAdded,
                        Username = (await userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName,
                    });
                }


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
                    liked = liked,
                    Comments = blogCommentsForView
                };

            }

            return View(blogDetailsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
        {
            if (signInManager.IsSignedIn(User))
            {
                var domainModel = new BlogPostComment
                {
                    BlogPostId = blogDetailsViewModel.Id,
                    Description = blogDetailsViewModel.CommentDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                    DateAdded = DateTime.Now,
                };
                await blogPostCommentRepository.AddAsync(domainModel);
                return RedirectToAction("Index", "Blogs",
                    new {urlHandler = blogDetailsViewModel.UrlHandler} );
            }
            return View();
  
        }

    }
}
