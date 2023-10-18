using Microsoft.AspNetCore.Identity;

namespace BlogApplication.UI.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}
