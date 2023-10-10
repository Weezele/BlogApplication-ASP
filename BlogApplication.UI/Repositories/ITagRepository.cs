using BlogApplication.UI.Models.Domain;

namespace BlogApplication.UI.Repositories
{
    public interface ITagRepository
    {
       Task<IEnumerable<Tag>> GetAllTagsAsync();

        Task<Tag?> GetAsync(Guid id);

       Task<Tag> AddAsync(Tag tag);

       Task<Tag?> UpdateAsync(Tag tag);

       Task<Tag?> DeleteAsync(Guid id);
    }
}
