namespace SecretChat.Mobile.ViewModels
{
	public partial class SettingsViewModel(IConnectivity connectivity, AuthService authService) : BaseViewModel(connectivity, authService)
	{
		public override async Task InitializeViewModel()
		{
			IsLoadedPage = true;
		}
	}
}
