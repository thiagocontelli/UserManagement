using UserManagement.ViewModels;

namespace UserManagement.Views;

public partial class UpsertUserView : ContentPage
{
	public UpsertUserView(UpsertUserViewModel upsertUserViewModel)
	{
		InitializeComponent();
		BindingContext = upsertUserViewModel;
		upsertUserViewModel.RequestOnClose += () =>
		{
			Application.Current?.CloseWindow(Window);
		};
		upsertUserViewModel.DisplayError += async (string errorMessage) =>
		{
			await DisplayAlert("Error", errorMessage, "OK");
		};
	}
}