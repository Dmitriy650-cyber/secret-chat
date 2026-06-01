namespace SecretChat.Mobile.ViewModels
{
	public partial class HelpViewModel(IConnectivity connectivity, AuthService authService) : BaseViewModel(connectivity, authService)
	{
		public override async Task InitializeViewModel()
		{
			IsLoadedPage = true;
		}
	}
}
