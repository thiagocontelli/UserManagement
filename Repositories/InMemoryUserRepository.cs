using UserManagement.Models;

namespace UserManagement.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> users = [];

    public async Task<List<User>> GetUsers()
    {
        await Task.Delay(1500);
        return users;
    }
}
