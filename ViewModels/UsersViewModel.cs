using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using UserManagement.Models;
using UserManagement.Repositories;

namespace UserManagement.ViewModels;

public partial class UsersViewModel : ObservableObject
{
    public ObservableCollection<User> Items { get; } = [];
    private readonly IUserRepository userRepository;
    public UsersViewModel(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
        LoadUsers();
    }

    private async void LoadUsers()
    {
        List<User> users = await userRepository.GetUsers();
        foreach (var user in users)
        {
            Items.Add(user);
        }
    }
}
