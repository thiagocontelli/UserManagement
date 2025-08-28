using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services.UserServices;
using UserManagement.Views;

namespace UserManagement.ViewModels;

public partial class UsersViewModel : ObservableObject
{
    public ObservableCollection<User> Items { get; } = [];
    private readonly GetUsersService getUsersService;
    private readonly DeleteUserService deleteUserService;
    public UsersViewModel(GetUsersService getUsersService, DeleteUserService deleteUserService)
    {
        this.getUsersService = getUsersService;
        this.deleteUserService = deleteUserService;
        LoadUsers();
    }

    public async void LoadUsers()
    {
        List<User> users = await getUsersService.Execute(Unit.Value);
        Items.Clear();
        foreach (var user in users)
        {
            Items.Add(user);
        }
    }

    [RelayCommand]
    private async Task DeleteUser(User user)
    {
        await deleteUserService.Execute(user.Id);
        LoadUsers();
    }
}
