using Moq;
using UserManagement.Models;
using UserManagement.Repositories;
using UserManagement.Services.UserServices;

namespace UserManagement.Tests.Services.UserServices
{
    public class UpdateUserServiceTests
    {
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly UpdateUserService updateUserService;

        public UpdateUserServiceTests()
        {
            mockUserRepository = new Mock<IUserRepository>();
            updateUserService = new UpdateUserService(mockUserRepository.Object);
        }

        [Fact]
        public async Task Execute_ValidInput_ShouldUpdateUserAndReturnUnit()
        {
            var userId = Guid.NewGuid();
            var existingUser = new User(userId, "João", "Silva", 30, "Rua Tal");
            var input = new UpdateUserInput(userId, "Maria", "Almeida", 25, "Avenida Aquela");
            
            mockUserRepository.Setup(repo => repo.GetUserById(userId))
                .ReturnsAsync(existingUser);
            mockUserRepository.Setup(repo => repo.UpdateUser(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            var result = await updateUserService.Execute(input);

            Assert.Equal(Unit.Value, result);
            mockUserRepository.Verify(repo => repo.GetUserById(userId), Times.Once);
            mockUserRepository.Verify(repo => repo.UpdateUser(It.Is<User>(u => 
                u.Id == userId && 
                u.Name == input.Name && 
                u.LastName == input.LastName && 
                u.Age == input.Age && 
                u.Address == input.Address)), Times.Once);
        }

        [Fact]
        public async Task Execute_UserNotFound_ShouldThrowException()
        {
            var userId = Guid.NewGuid();
            var input = new UpdateUserInput(userId, "Maria", "Almeida", 25, "Avenida Aquela");
            
            mockUserRepository.Setup(repo => repo.GetUserById(userId))
                .ReturnsAsync((User?)null);

            var exception = await Assert.ThrowsAsync<Exception>(() => updateUserService.Execute(input));
            Assert.Equal("User not found", exception.Message);
            mockUserRepository.Verify(repo => repo.GetUserById(userId), Times.Once);
            mockUserRepository.Verify(repo => repo.UpdateUser(It.IsAny<User>()), Times.Never);
        }

        [Theory]
        [InlineData("", "Almeida", 25, "Avenida Aquela", "Name is required")]
        [InlineData("Maria", "", 25, "Avenida Aquela", "Last Name is required")]
        [InlineData("Maria", "Almeida", 25, "", "Address is required")]
        public async Task Execute_InvalidInput_ShouldThrowException(string name, string lastName, int age, string address, string expectedErrorMessage)
        {
            var userId = Guid.NewGuid();
            var existingUser = new User(userId, "João", "Silva", 30, "Rua Tal");
            var input = new UpdateUserInput(userId, name, lastName, age, address);
            
            mockUserRepository.Setup(repo => repo.GetUserById(userId))
                .ReturnsAsync(existingUser);

            var exception = await Assert.ThrowsAsync<Exception>(() => updateUserService.Execute(input));
            Assert.Equal(expectedErrorMessage, exception.Message);
            mockUserRepository.Verify(repo => repo.GetUserById(userId), Times.Once);
            mockUserRepository.Verify(repo => repo.UpdateUser(It.IsAny<User>()), Times.Never);
        }
    }
}