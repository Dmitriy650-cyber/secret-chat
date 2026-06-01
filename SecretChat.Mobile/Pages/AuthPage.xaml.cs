namespace SecretChat.Mobile.Pages;

public partial class AuthPage : BaseView<AuthViewModel>
{
	public AuthPage()
	{
		InitializeComponent();
	}

	protected override bool OnBackButtonPressed()
	{
		return true;
	}
}