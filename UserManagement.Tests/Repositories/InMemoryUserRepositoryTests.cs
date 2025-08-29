using UserManagement.Models;
using UserManagement.Repositories;

namespace UserManagement.Tests.Repositories
{
    public class InMemoryUserRepositoryTests
    {
        private readonly InMemoryUserRepository repository;

        public InMemoryUserRepositoryTests()
        {
            repository = new InMemoryUserRepository();
        }

        [Fact]
        public async Task GetUsers_EmptyRepository_ShouldReturnEmptyList()
        {
            var result = await repository.GetUsers();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetUsers_WithUsers_ShouldReturnAllUsers()
        {
            var user1 = new User(Guid.NewGuid(), "João", "Silva", 30, "Rua Tal");
            var user2 = new User(Guid.NewGuid(), "Maria", "Almeida", 25, "Avenida Aquela");
            await repository.AddUser(user1);
            await repository.AddUser(user2);

            var result = await repository.GetUsers();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, u => u.Id == user1.Id);
            Assert.Contains(result, u => u.Id == user2.Id);
        }

        [Fact]
        public async Task AddUser_NewUser_ShouldAddUserToRepository()
        {
            var user = new User(Guid.NewGuid(), "João", "Silva", 30, "Rua Tal");

            await repository.AddUser(user);

            var users = await repository.GetUsers();
            Assert.Single(users);
            Assert.Equal(user.Id, users[0].Id);
            Assert.Equal(user.Name, users[0].Name);
            Assert.Equal(user.LastName, users[0].LastName);
            Assert.Equal(user.Age, users[0].Age);
            Assert.Equal(user.Address, users[0].Address);
        }

        [Fact]
        public async Task AddUser_ExistingUser_ShouldThrowException()
        {
            var userId = Guid.NewGuid();
            var user = new User(userId, "João", "Silva", 30, "Rua Tal");
            await repository.AddUser(user);

            var duplicateUser = new User(userId, "Maria", "Almeida", 25, "Avenida Aquela");

            var exception = await Assert.ThrowsAsync<Exception>(() => repository.AddUser(duplicateUser));
            Assert.Equal("User already exists", exception.Message);
        }

        [Fact]
        public async Task GetUserById_ExistingUser_ShouldReturnUser()
        {
            var user = new User(Guid.NewGuid(), "João", "Silva", 30, "Rua Tal");
            await repository.AddUser(user);

            var result = await repository.GetUserById(user.Id);

            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.Name, result.Name);
            Assert.Equal(user.LastName, result.LastName);
            Assert.Equal(user.Age, result.Age);
            Assert.Equal(user.Address, result.Address);
        }

        [Fact]
        public async Task GetUserById_NonExistingUser_ShouldReturnNull()
        {
            var result = await repository.GetUserById(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateUser_ExistingUser_ShouldUpdateUser()
        {
            var userId = Guid.NewGuid();
            var user = new User(userId, "João", "Silva", 30, "Rua Tal");
            await repository.AddUser(user);

            var updatedUser = new User(userId, "Maria", "Almeida", 25, "Avenida Aquela");
            await repository.UpdateUser(updatedUser);

            var result = await repository.GetUserById(userId);
            Assert.NotNull(result);
            Assert.Equal(updatedUser.Name, result.Name);
            Assert.Equal(updatedUser.LastName, result.LastName);
            Assert.Equal(updatedUser.Age, result.Age);
            Assert.Equal(updatedUser.Address, result.Address);
        }

        [Fact]
        public async Task UpdateUser_NonExistingUser_ShouldThrowException()
        {
            var user = new User(Guid.NewGuid(), "João", "Silva", 30, "Rua Tal");

            var exception = await Assert.ThrowsAsync<Exception>(() => repository.UpdateUser(user));
            Assert.Equal("User not found", exception.Message);
        }

        [Fact]
        public async Task DeleteUser_ExistingUser_ShouldRemoveUser()
        {
            var user = new User(Guid.NewGuid(), "João", "Silva", 30, "Rua Tal");
            await repository.AddUser(user);

            await repository.DeleteUser(user.Id);

            var result = await repository.GetUserById(user.Id);
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteUser_NonExistingUser_ShouldThrowException()
        {
            var exception = await Assert.ThrowsAsync<Exception>(() => repository.DeleteUser(Guid.NewGuid()));
            Assert.Equal("User not found", exception.Message);
        }
    }
}