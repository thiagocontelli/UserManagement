using UserManagement.Models;

namespace UserManagement.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> users = [];

    public async Task AddUser(User user)
    {
        await Task.Delay(100);

        User? existingUser = await GetUserById(user.Id);

        if (existingUser is not null)
        {
            throw new Exception("User already exists");
        }

        users.Add(user);
    }

    public async Task DeleteUser(Guid id)
    {
        await Task.Delay(100);
        
        User? user = await GetUserById(id) ?? throw new Exception("User not found");

        users.Remove(user);
    }

    public async Task<User?> GetUserById(Guid id)
    {
        await Task.Delay(100);
        return users.FirstOrDefault(u => u.Id == id);
    }

    public async Task<List<User>> GetUsers()
    {
        await Task.Delay(100);
        return users;
    }

    public async Task UpdateUser(User user)
    {
        await Task.Delay(100);

        User? existingUser = await GetUserById(user.Id) ?? throw new Exception("User not found");

        users.Remove(existingUser);
        users.Add(user);
    }
}
