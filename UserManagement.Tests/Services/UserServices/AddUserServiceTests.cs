using Moq;
using UserManagement.Models;
using UserManagement.Repositories;
using UserManagement.Services.UserServices;

namespace UserManagement.Tests.Services.UserServices
{
    public class AddUserServiceTests
    {
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly AddUserService addUserService;

        public AddUserServiceTests()
        {
            mockUserRepository = new Mock<IUserRepository>();
            addUserService = new AddUserService(mockUserRepository.Object);
        }

        [Fact]
        public async Task Execute_ValidInput_ShouldAddUserAndReturnUser()
        {
            var input = new AddUserInput("João", "Silva", 30, "Rua Tal");
            mockUserRepository.Setup(repo => repo.AddUser(It.IsAny<User>())).Returns(Task.CompletedTask);

            var result = await addUserService.Execute(input);

            Assert.NotNull(result);
            Assert.Equal(input.Name, result.Name);
            Assert.Equal(input.LastName, result.LastName);
            Assert.Equal(input.Age, result.Age);
            Assert.Equal(input.Address, result.Address);
            mockUserRepository.Verify(repo => repo.AddUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task Execute_UserAlreadyExists_ShouldThrowException()
        {
            var input = new AddUserInput("João", "Silva", 30, "Rua Tal");
            mockUserRepository.Setup(repo => repo.AddUser(It.IsAny<User>()))
                .ThrowsAsync(new Exception("User already exists"));

            var exception = await Assert.ThrowsAsync<Exception>(() => addUserService.Execute(input));
            Assert.Equal("User already exists", exception.Message);
        }

        [Theory]
        [InlineData("", "Silva", 30, "Rua Tal", "Name is required")]
        [InlineData("João", "", 30, "Rua Tal", "Last Name is required")]
        [InlineData("João", "Silva", 30, "", "Address is required")]
        [InlineData("João", "Silva", -1, "", "Age must be an integer number")]
        public async Task Execute_InvalidInput_ShouldThrowException(string name, string lastName, int age, string address, string expectedErrorMessage)
        {
            var input = new AddUserInput(name, lastName, age, address);

            var exception = await Assert.ThrowsAsync<Exception>(() => addUserService.Execute(input));
            Assert.Equal(expectedErrorMessage, exception.Message);
        }
    }
}