namespace SecretChat.Mobile
{
	public partial class App : Application
	{
		private readonly AuthService _authService;
		private readonly IServiceScopeFactory _serviceScopeFactory;
		private RealtimeUpdateService? _realtimeUpdateService;

		public App(AuthService authService, IServiceScopeFactory serviceScopeFactory)
		{
			InitializeComponent();

			_authService = authService;
			_serviceScopeFactory = serviceScopeFactory;
		}

		protected override Window CreateWindow(IActivationState? activationState)
		{
			return new Window(new AppShell(_authService));
		}

		protected override void OnStart()
		{
			base.OnStart();

			using var scope = _serviceScopeFactory.CreateScope();
			_realtimeUpdateService = scope.ServiceProvider.GetRequiredService<RealtimeUpdateService>();

			_ = Task.Run(async () =>
			{
				try
				{
					await _realtimeUpdateService.ConfigureRealtimeUpdates();
				}
				catch (Exception)
				{

				}
			});
		}
	}
}