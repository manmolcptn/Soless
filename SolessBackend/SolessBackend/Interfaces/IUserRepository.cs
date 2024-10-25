using SolessBackend.Models;

namespace SolessBackend.Interfaces
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
    }
}
