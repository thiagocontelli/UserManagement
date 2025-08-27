using UserManagement.ViewModels;

namespace UserManagement.Views;

public partial class UsersView : ContentPage
{
	public UsersView(UsersViewModel usersViewModel)
	{
		InitializeComponent();
		BindingContext = usersViewModel;
	}
}