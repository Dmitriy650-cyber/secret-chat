namespace SecretChat.Mobile.Pages;

public partial class ProfilePage : BaseView<ProfileViewModel>
{
	public ProfilePage()
	{
		InitializeComponent();
	}

	protected override bool OnBackButtonPressed()
	{
		return true;
	}
}