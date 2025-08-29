using Moq;
using UserManagement.Models;
using UserManagement.Repositories;
using UserManagement.Services.UserServices;

namespace UserManagement.Tests.Services.UserServices
{
    public class DeleteUserServiceTests
    {
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly DeleteUserService deleteUserService;

        public DeleteUserServiceTests()
        {
            mockUserRepository = new Mock<IUserRepository>();
            deleteUserService = new DeleteUserService(mockUserRepository.Object);
        }

        [Fact]
        public async Task Execute_ExistingUserId_ShouldDeleteUserAndReturnUnit()
        {
            var userId = Guid.NewGuid();
            
            mockUserRepository.Setup(repo => repo.DeleteUser(userId))
                .Returns(Task.CompletedTask);

            var result = await deleteUserService.Execute(userId);

            Assert.Equal(Unit.Value, result);
            mockUserRepository.Verify(repo => repo.DeleteUser(userId), Times.Once);
        }

        [Fact]
        public async Task Execute_UserNotFound_ShouldThrowException()
        {
            var userId = Guid.NewGuid();
            
            mockUserRepository.Setup(repo => repo.DeleteUser(userId))
                .ThrowsAsync(new Exception("User not found"));

            var exception = await Assert.ThrowsAsync<Exception>(() => deleteUserService.Execute(userId));
            Assert.Equal("User not found", exception.Message);
            mockUserRepository.Verify(repo => repo.DeleteUser(userId), Times.Once);
        }
    }
}