using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace UserManagement.ViewModels;

public class UsersViewModel : ObservableObject
{
    static Random random = new();
    public ObservableCollection<User> Items { get; } = new();
    public UsersViewModel()
    {
        for (int i = 0; i < 10; i++)
        {
            Items.Add(new User
            {
                Id = i,
                Name = "Person " + i,
                Age = random.Next(14, 85),
                Address = "Street Something 123 São Paulo SP"
            });
        }
    }
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }

    }
}
