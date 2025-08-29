using UserManagement.Models;
using UserManagement.Repositories;

namespace UserManagement.Services.UserServices;

public class GetUsersService(IUserRepository userRepository) : IService<Unit, List<User>>
{

    private readonly IUserRepository userRepository = userRepository;

    public async Task<List<User>> Execute(Unit input) => await userRepository.GetUsers();
}
