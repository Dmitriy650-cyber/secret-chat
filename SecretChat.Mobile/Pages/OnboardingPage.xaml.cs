namespace SecretChat.Mobile.Pages;

public partial class OnboardingPage : ContentPage
{
	public OnboardingPage()
	{
		InitializeComponent();
	}

	private async void SimpleButton_Clicked(object sender, EventArgs e)
	{
		Preferences.Default.Set(MobileConstants.FirstRunKey, true);

		await Shell.Current.GoToAsync(nameof(AuthPage), animate: true);
    }
}