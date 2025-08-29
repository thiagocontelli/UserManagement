using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UserManagement.Models;
using UserManagement.Services.UserServices;

namespace UserManagement.ViewModels;

public partial class UpsertUserViewModel : ObservableObject
{
    public event Action? RequestOnClose;
    public event Action<string>? DisplayError;

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string lastName;

    [ObservableProperty]
    private string age;

    [ObservableProperty]
    private string address;

    [ObservableProperty]
    private string title;

    [ObservableProperty]
    private string subtitle;

    public Guid? UserId { get; private set; }

    private readonly AddUserService addUserService;
    private readonly UpdateUserService updateUserService;
    private readonly GetUserService getUserService;

    public UpsertUserViewModel(AddUserService addUserService, UpdateUserService updateUserService, GetUserService getUserService)
    {
        this.addUserService = addUserService;
        this.updateUserService = updateUserService;
        this.getUserService = getUserService;
    }

    public async Task LoadUser(Guid? id = null)
    {
        UserId = id;

        if (id.HasValue)
        {
            Title = "Edit user";
            Subtitle = "Edit the user details in your system.";
        }
        else
        {
            Title = "Add user";
            Subtitle = "Add new user to your system.";
            return;
        }

        User? user = await getUserService.Execute(id.Value);
        if (user is null) return;

        Name = user.Name;
        LastName = user.LastName;
        Age = user.Age.ToString();
        Address = user.Address;
    }

    [RelayCommand]
    private async Task UpsertUser()
    {
        try
        {
            if (!int.TryParse(Age, out int age))
            {
                DisplayError?.Invoke("Age must be a number");

                return;
            }

            if (UserId is null)
            {
                var addUserInput = new AddUserInput(Name, LastName, age, Address);

                await addUserService.Execute(addUserInput);
            }
            else
            {
                var updateUserInput = new UpdateUserInput((Guid)UserId, Name, LastName, age, Address);

                await updateUserService.Execute(updateUserInput);
            }

            RequestOnClose?.Invoke();
        }
        catch (Exception ex)
        {
            DisplayError?.Invoke(ex.Message);
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        RequestOnClose?.Invoke();
    }
}
