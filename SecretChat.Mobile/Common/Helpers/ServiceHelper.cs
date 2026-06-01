namespace SecretChat.Mobile.Common.Helpers
{
	public static class ServiceHelper
	{
		public static MauiApp? App { get; set; }

		public static TService? GetService<TService>() where TService : class =>
			App?.Services.GetService<TService>();

		public static IServiceProvider? Services => App?.Services;
	}
}
