using UserManagement.Models;
using UserManagement.Repositories;

namespace UserManagement.Services.UserServices;

public class UpdateUserService(IUserRepository userRepository) : IService<UpdateUserInput, Unit>
{
    private readonly IUserRepository userRepository = userRepository;

    public async Task<Unit> Execute(UpdateUserInput input)
    {
        User? user = await userRepository.GetUserById(input.Id) ?? throw new Exception("User not found");

        var newUser = new User(user.Id, input.Name, input.LastName, input.Age, input.Address);

        newUser.Validate();

        await userRepository.UpdateUser(newUser);

        return Unit.Value;
    }
}
