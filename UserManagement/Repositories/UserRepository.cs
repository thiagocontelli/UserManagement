using SQLite;
using UserManagement.Models;

namespace UserManagement.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SQLiteAsyncConnection _database;

    public UserRepository()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "users.db3");
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<UserSQLite>().Wait();
    }

    private class UserSQLite
    {
        [PrimaryKey]
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int Age { get; set; }
        public string Address { get; set; } = null!;
    }

    private static UserSQLite ToSQLite(User user) => new UserSQLite
    {
        Id = user.Id.ToString(),
        Name = user.Name,
        LastName = user.LastName,
        Age = user.Age,
        Address = user.Address
    };

    private static User FromSQLite(UserSQLite user) => new User(
        Guid.Parse(user.Id),
        user.Name,
        user.LastName,
        user.Age,
        user.Address
    );

    public async Task AddUser(User user)
    {
        var existing = await GetUserById(user.Id);
        if (existing is not null)
            throw new Exception("User already exists");

        await _database.InsertAsync(ToSQLite(user));
    }

    public async Task DeleteUser(Guid id)
    {
        var user = await GetUserById(id) ?? throw new Exception("User not found");
        await _database.DeleteAsync<UserSQLite>(id.ToString());
    }

    public async Task<User?> GetUserById(Guid id)
    {
        var result = await _database.FindAsync<UserSQLite>(id.ToString());
        return result == null ? null : FromSQLite(result);
    }

    public async Task<List<User>> GetUsers()
    {
        var list = await _database.Table<UserSQLite>().ToListAsync();
        return list.Select(FromSQLite).ToList();
    }

    public async Task UpdateUser(User user)
    {
        user.Validate();

        var existing = await GetUserById(user.Id) ?? throw new Exception("User not found");

        await _database.UpdateAsync(ToSQLite(user));
    }
}
