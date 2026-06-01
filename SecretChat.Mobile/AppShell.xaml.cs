namespace SecretChat.Mobile
{
	public partial class AppShell : SimpleShell
	{
		private readonly AuthService _authService;

		public AppShell(AuthService authService)
		{
			InitializeComponent();

			AddTab(typeof(HomePage), nameof(HomePage));
			AddTab(typeof(ProfilePage), nameof(ProfilePage));
			AddTab(typeof(ContactsPage), nameof(ContactsPage));

			Routing.RegisterRoute(nameof(ChatPage), typeof(ChatPage));
			Routing.RegisterRoute(nameof(AuthPage), typeof(AuthPage));
			Routing.RegisterRoute(nameof(OnboardingPage), typeof(OnboardingPage));
			Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
			Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
			Routing.RegisterRoute(nameof(HelpPage), typeof(HelpPage));
			Routing.RegisterRoute(nameof(EditPage), typeof(EditPage));

			_authService = authService;
		}

		protected override async void OnAppearing()
		{
			if (Preferences.ContainsKey(MobileConstants.FirstRunKey))
			{
				if (_authService.IsLoggedIn)
				{
					await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
				}
				else
				{
					await Shell.Current.GoToAsync(nameof(AuthPage));
				}
			}
			else
			{
				await Shell.Current.GoToAsync(nameof(OnboardingPage));
			}
		}

		private void AddTab(Type page, string route)
		{
			var tab = new Tab { Route = route, Title  = route };
			tab.Items.Add(new ShellContent { ContentTemplate = new DataTemplate(page) });
			tabBar.Items.Add(tab);
		}

		private void floatingBottomBar_CurrentPageSelectionChanged(object arg1, object arg2)
		{
			var tabBarEventArgs = arg2 as TabBarEventArgs;
			Shell.Current.GoToAsync("///" + tabBarEventArgs!.PageKey);
		}
	}

	public class TabBarEventArgs : EventArgs
	{
		public string PageKey { get; set; }

		public TabBarEventArgs(string pageKey)
		{
			PageKey = pageKey;
		}
	}
}
