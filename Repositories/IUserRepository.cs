using UserManagement.Models;

namespace UserManagement.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetUsers();
}
