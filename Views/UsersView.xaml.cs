using UserManagement.Models;
using UserManagement.Services;
using UserManagement.Services.WindowServices;
using UserManagement.ViewModels;

namespace UserManagement.Views;

public partial class UsersView : ContentPage
{
    private readonly UsersViewModel usersViewModel;
    private readonly OpenWindowService openWindowService;

	public UsersView(UsersViewModel usersViewModel, OpenWindowService openWindowService)
	{
		InitializeComponent();
        this.openWindowService = openWindowService;
        this.usersViewModel = usersViewModel;
		BindingContext = usersViewModel;
        this.usersViewModel.DisplayDeleteAlert = async (user, confirmedCallback) =>
        {
            bool result = await DisplayAlert(
                "Confirm Delete",
                $"Do you really want to delete {user.FullName}?",
                "Yes",
                "No"
            );

            if (result)
            {
                await confirmedCallback();
            }
        };
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button) return;
        Guid? userId = button?.CommandParameter as Guid?;

        var page = ServiceHelper.GetService<UpsertUserView>();
        var vm = page.BindingContext as UpsertUserViewModel;

        if (userId.HasValue && userId != Guid.Empty)
        {
            vm?.LoadUser(userId.Value);
        }
        else
        {
            vm?.LoadUser();
        }

        var openWindowInput = new OpenWindowInput(
            page, 
            userId.HasValue ? "Edit user" : "Add user", 
            usersViewModel.LoadUsers, 
            null, 
            500
        );

        openWindowService.Execute(openWindowInput);
    }
}