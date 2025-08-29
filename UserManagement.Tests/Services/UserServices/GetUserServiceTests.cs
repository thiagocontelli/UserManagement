using Moq;
using UserManagement.Models;
using UserManagement.Repositories;
using UserManagement.Services.UserServices;

namespace UserManagement.Tests.Services.UserServices
{
    public class GetUserServiceTests
    {
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly GetUserService getUserService;

        public GetUserServiceTests()
        {
            mockUserRepository = new Mock<IUserRepository>();
            getUserService = new GetUserService(mockUserRepository.Object);
        }

        [Fact]
        public async Task Execute_ExistingUserId_ShouldReturnUser()
        {
            var userId = Guid.NewGuid();
            var expectedUser = new User(userId, "João", "Silva", 30, "Rua Tal");
            
            mockUserRepository.Setup(repo => repo.GetUserById(userId))
                .ReturnsAsync(expectedUser);

            var result = await getUserService.Execute(userId);

            Assert.NotNull(result);
            Assert.Equal(expectedUser.Id, result.Id);
            Assert.Equal(expectedUser.Name, result.Name);
            Assert.Equal(expectedUser.LastName, result.LastName);
            Assert.Equal(expectedUser.Age, result.Age);
            Assert.Equal(expectedUser.Address, result.Address);
            mockUserRepository.Verify(repo => repo.GetUserById(userId), Times.Once);
        }

        [Fact]
        public async Task Execute_NonExistingUserId_ShouldReturnNull()
        {
            var userId = Guid.NewGuid();
            
            mockUserRepository.Setup(repo => repo.GetUserById(userId))
                .ReturnsAsync((User?)null);

            var result = await getUserService.Execute(userId);

            Assert.Null(result);
            mockUserRepository.Verify(repo => repo.GetUserById(userId), Times.Once);
        }
    }
}