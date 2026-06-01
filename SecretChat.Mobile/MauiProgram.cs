namespace SecretChat.Mobile
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.UseMauiCommunityToolkit()
				.UseSimpleButton()
				.UseSimpleShell()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("Ubuntu-Regular.ttf", "UbuntuRegular");
					fonts.AddFont("Ubuntu-Bold.ttf", "UbuntuBold");
					fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
				});

#if DEBUG
			builder.Logging.AddDebug();
#endif
			builder.Services.ConfigureRefitServices();
			builder.Services.ConfigureServices();

			ServiceHelper.App = builder.Build();

			return ServiceHelper.App;
		}

		public static IServiceCollection ConfigureServices(this IServiceCollection services)
		{
			services.AddSingleton<IConnectivity>(Connectivity.Current);
			services.AddSingleton<AuthService>();
			services.AddSingleton<RealtimeUpdateService>();

			services.AddSingleton<HomeViewModel>().AddTransient<HomePage>();
			services.AddSingleton<ContactsViewModel>().AddTransient<ContactsPage>();
			services.AddSingleton<ProfileViewModel>().AddTransient<ProfilePage>();
			services.AddSingleton<SettingsViewModel>().AddTransient<SettingsPage>();
			services.AddSingleton<HelpViewModel>().AddTransient<HelpPage>();

			services.AddTransient<ChatViewModel>().AddTransient<ChatPage>();
			services.AddTransient<AuthViewModel>().AddTransient<AuthPage>();
			services.AddTransient<DetailsViewModel>().AddTransient<DetailsPage>();
			services.AddTransient<EditViewModel>().AddTransient<EditPage>();

			return services;
		}

		public static IServiceCollection ConfigureRefitServices(this IServiceCollection services)
		{
			services
				.AddRefitClient<IAuthApi>(GetRefitSettings)
				.ConfigureHttpClient(SetHttpClient);
			services
				.AddRefitClient<IUserApi>(GetRefitSettings)
				.ConfigureHttpClient(SetHttpClient);
			services
				.AddRefitClient<IChatApi>(GetRefitSettings)
				.ConfigureHttpClient(SetHttpClient);
			services
				.AddRefitClient<IMessageApi>(GetRefitSettings)
				.ConfigureHttpClient(SetHttpClient);
			services
				.AddRefitClient<IPhotoApi>(GetRefitSettings)
				.ConfigureHttpClient(SetHttpClient);

			void SetHttpClient(HttpClient httpClient) => httpClient.BaseAddress = new Uri(AppConstants.ApiBaseUrl);

			RefitSettings GetRefitSettings(IServiceProvider sp)
			{
				var authService = sp.GetRequiredService<AuthService>();

				return new RefitSettings
				{
					AuthorizationHeaderValueGetter = (_, _) => Task.FromResult(authService.Token ?? "")
				};
			}

			return services;
		}
	}
}
