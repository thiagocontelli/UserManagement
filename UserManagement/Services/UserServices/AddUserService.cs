using UserManagement.Models;
using UserManagement.Repositories;

namespace UserManagement.Services.UserServices;

public class AddUserService(IUserRepository userRepository) : IService<AddUserInput, User>
{
    private readonly IUserRepository userRepository = userRepository;

    public async Task<User> Execute(AddUserInput input)
    {
        var newUser = new User(Guid.NewGuid(), input.Name, input.LastName, input.Age, input.Address);

        newUser.Validate();

        await userRepository.AddUser(newUser);

        return newUser;
    }
}
