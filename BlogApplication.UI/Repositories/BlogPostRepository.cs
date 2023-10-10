using BlogApplication.UI.Data;
using BlogApplication.UI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogApplication.UI.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BlogDbContext dbContext;

        public BlogPostRepository(BlogDbContext blogDbContext)
        {
            this.dbContext = blogDbContext;

        }

        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await dbContext.AddAsync(blogPost);
            await dbContext.SaveChangesAsync();
            return blogPost;
        }

        public Task<BlogPost?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await dbContext.BlogPosts.Include(x=>x.Tags).ToListAsync();
        }

        public Task<BlogPost?> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            throw new NotImplementedException();
        }
    }
}
