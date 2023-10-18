using BlogApplication.UI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogApplication.UI.Data
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) 
        {
            
        }
        public DbSet<BlogPost>? BlogPosts { get; set; }
        public DbSet<Tag>? Tags { get; set; }
        public DbSet<BlogPostLike>? BlogPostLike { get; set; }
        public DbSet<BlogPostComment>? BlogPostComments { get; set; }



    }
}
