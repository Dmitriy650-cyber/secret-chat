using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace SecretChat.Mobile
{
	[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
	public class MainActivity : MauiAppCompatActivity
	{
		protected override void OnCreate(Bundle? savedInstanceState)
		{
			MakeStatusBarTransparent(this);

			base.OnCreate(savedInstanceState);
		}

		public static void MakeStatusBarTransparent(Android.App.Activity activity)
		{
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				activity.Window!.SetFlags(
					WindowManagerFlags.LayoutNoLimits,
					WindowManagerFlags.LayoutNoLimits);

				activity.Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
				activity.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
			}
		}
	}
}
