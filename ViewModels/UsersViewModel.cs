using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using UserManagement.Models;
using UserManagement.Services.UserServices;

namespace UserManagement.ViewModels;

public partial class UsersViewModel : ObservableObject
{
    public ObservableCollection<User> Items { get; } = [];
    private readonly GetUsersService getUsersService;
    public UsersViewModel(GetUsersService getUsersService)
    {
        this.getUsersService = getUsersService;
        LoadUsers();
    }

    private async void LoadUsers()
    {
        List<User> users = await getUsersService.Execute();
        foreach (var user in users)
        {
            Items.Add(user);
        }
    }
}
