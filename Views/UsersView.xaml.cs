using System.Threading.Tasks;
using UserManagement.Services;
using UserManagement.ViewModels;

namespace UserManagement.Views;

public partial class UsersView : ContentPage
{
    private readonly UsersViewModel usersViewModel;

	public UsersView(UsersViewModel usersViewModel)
	{
		InitializeComponent();
        this.usersViewModel = usersViewModel;
		BindingContext = usersViewModel;
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button) return;
        Guid? userId = button?.CommandParameter as Guid?;

        var page = ServiceHelper.GetService<UpsertUserView>();
        var vm = page.BindingContext as UpsertUserViewModel;
        var window = new Window(page);

        if (userId.HasValue && userId != Guid.Empty)
        {
            vm?.LoadUser(userId.Value);
            window.Title = "Edit user";
        }
        else
        {
            window.Title = "Add user";
        }

        var displayInfo = DeviceDisplay.Current.MainDisplayInfo;
        window.X = (displayInfo.Width / displayInfo.Density - window.Width) / 2;
        window.Y = (displayInfo.Height / displayInfo.Density - window.Height) / 2;

        window.Destroying += (_, _) =>
        {
            usersViewModel.LoadUsers();
        };

        Application.Current?.OpenWindow(window);
    }
}