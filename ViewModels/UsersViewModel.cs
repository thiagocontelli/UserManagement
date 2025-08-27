using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using UserManagement.Models;

namespace UserManagement.ViewModels;

public class UsersViewModel : ObservableObject
{
    public ObservableCollection<User> Items { get; } = [];
    public UsersViewModel()
    {
        Items = [new User(1, "John", "Doe", 21, "Street Something"), new User(2, "Mary", "Parker", 19, "Street Other")];
    }
}
