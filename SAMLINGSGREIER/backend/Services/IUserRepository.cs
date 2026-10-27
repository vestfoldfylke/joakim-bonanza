using Backend.Models;

namespace Backend.Services;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> AddUserAsync(AddUserRequest request);
    Task<bool> RemoveUserAsync(Guid id);
    Task<User?> GetUserByIdAsync(Guid id);
    Task<User?> VerifyPasswordAsync(string username, string password);

}