using UserManagement.Models;

namespace UserManagement.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetUsers();
    Task AddUser(User user);
    Task UpdateUser(User user);
    Task<User?> GetUserById(Guid id);
    Task DeleteUser(Guid id);
}
