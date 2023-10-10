using BlogApplication.UI.Data;
using BlogApplication.UI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogApplication.UI.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BlogDbContext dbContext;
        public TagRepository(BlogDbContext blogDbContext)
        {
            this.dbContext = blogDbContext;
        }
        public async Task<Tag> AddAsync(Tag tag)
        {
            await dbContext.Tags.AddAsync(tag);
            await dbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var existingTag = await dbContext.Tags.FindAsync(id);
            if (existingTag != null)
            {
                dbContext.Tags.Remove(existingTag);
                await dbContext.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await dbContext.Tags.ToListAsync();
        }

        public  Task<Tag?> GetAsync(Guid id)
        {
            return  dbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existTag = await dbContext.Tags.FindAsync(tag.Id);
            if (existTag != null)
            {
                existTag.Name = tag.Name;
                existTag.DisplayName = tag.DisplayName;
                await dbContext.SaveChangesAsync();
                return existTag;
            }
            return null;
        }
    }
}
