using UserManagement.Models;
using UserManagement.Repositories;

namespace UserManagement.Services.UserServices;

public class GetUserService(IUserRepository userRepository) : IService<Guid, User?>
{

    private readonly IUserRepository userRepository = userRepository;

    public async Task<User?> Execute(Guid id) => await userRepository.GetUserById(id);
}
