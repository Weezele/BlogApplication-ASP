using BlogApplication.UI.Data;
using BlogApplication.UI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogApplication.UI.Repositories
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly BlogDbContext blogDbContext;

        public BlogPostLikeRepository(BlogDbContext blogDbContext)
        {
            this.blogDbContext = blogDbContext;
        }

        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await blogDbContext.BlogPostLike.AddAsync(blogPostLike);
            await blogDbContext.SaveChangesAsync();
            return blogPostLike;
        }

        public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
        {
            return await blogDbContext.BlogPostLike.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
            return await blogDbContext.BlogPostLike.CountAsync(x => x.BlogPostId == blogPostId);
        }
    }
}
