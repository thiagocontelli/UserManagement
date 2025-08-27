using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using UserManagement.Models;
using UserManagement.Services.UserServices;
using UserManagement.Views;

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

    [RelayCommand]
    private void GoToAddUser()
    {
        var page = new AddUserView();
        var window = new Window(page);
        var displayInfo = DeviceDisplay.Current.MainDisplayInfo;
        window.X = (displayInfo.Width / displayInfo.Density - window.Width) / 2;
        window.Y = (displayInfo.Height / displayInfo.Density - window.Height) / 2;
        window.Title = "Add user";
        Application.Current?.OpenWindow(window);
    }
}
