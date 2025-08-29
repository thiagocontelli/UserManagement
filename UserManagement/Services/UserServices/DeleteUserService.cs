using UserManagement.Models;
using UserManagement.Repositories;

namespace UserManagement.Services.UserServices;

public class DeleteUserService(IUserRepository userRepository) : IService<Guid, Unit>
{
    private readonly IUserRepository userRepository = userRepository;

    public async Task<Unit> Execute(Guid id)
    {
        await userRepository.DeleteUser(id);

        return Unit.Value;
    }
}
