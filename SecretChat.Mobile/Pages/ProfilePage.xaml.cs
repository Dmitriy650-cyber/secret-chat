namespace SecretChat.Mobile.Pages;

public partial class ProfilePage : BaseView<ProfileViewModel>
{
	public ProfilePage()
	{
		InitializeComponent();
		ViewModelInitializedAction += InitializeAction;
	}

	private void InitializeAction(object? sender, EventArgs e)
	{
		ViewModel.ConfigureRealtimeUpdates();
	}

	protected override bool OnBackButtonPressed()
	{
		return true;
	}
}