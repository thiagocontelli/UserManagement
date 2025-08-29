using Moq;
using UserManagement.Models;
using UserManagement.Repositories;
using UserManagement.Services.UserServices;

namespace UserManagement.Tests.Services.UserServices
{
    public class GetUsersServiceTests
    {
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly GetUsersService getUsersService;

        public GetUsersServiceTests()
        {
            mockUserRepository = new Mock<IUserRepository>();
            getUsersService = new GetUsersService(mockUserRepository.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnAllUsers()
        {
            var expectedUsers = new List<User>
            {
                new User(Guid.NewGuid(), "João", "Silva", 30, "Rua Tal"),
                new User(Guid.NewGuid(), "Maria", "Almeida", 25, "Avenida Aquela")
            };
            
            mockUserRepository.Setup(repo => repo.GetUsers())
                .ReturnsAsync(expectedUsers);

            var result = await getUsersService.Execute(Unit.Value);

            Assert.NotNull(result);
            Assert.Equal(expectedUsers.Count, result.Count);
            Assert.Equal(expectedUsers[0].Id, result[0].Id);
            Assert.Equal(expectedUsers[0].Name, result[0].Name);
            Assert.Equal(expectedUsers[1].Id, result[1].Id);
            Assert.Equal(expectedUsers[1].Name, result[1].Name);
            mockUserRepository.Verify(repo => repo.GetUsers(), Times.Once);
        }

        [Fact]
        public async Task Execute_EmptyUserList_ShouldReturnEmptyList()
        {
            var emptyList = new List<User>();
            
            mockUserRepository.Setup(repo => repo.GetUsers())
                .ReturnsAsync(emptyList);

            var result = await getUsersService.Execute(Unit.Value);

            Assert.NotNull(result);
            Assert.Empty(result);
            mockUserRepository.Verify(repo => repo.GetUsers(), Times.Once);
        }
    }
}