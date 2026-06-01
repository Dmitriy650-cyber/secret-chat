using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using System.Runtime.CompilerServices;

namespace SecretChat.Mobile.Controls
{
	public class NoBorderEditor : Editor
	{
		protected override void OnHandlerChanged()
		{
			base.OnHandlerChanged();
			SetBorderlessBackground();
		}

		protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
#if ANDROID
			if (propertyName == nameof(BackgroundColor))
				SetBorderlessBackground();
#endif
		}

		private void SetBorderlessBackground()
		{
#if ANDROID
			if (Handler is IEditorHandler entryHandler)
			{
				if (BackgroundColor is null)
					entryHandler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
				else
					entryHandler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(BackgroundColor.ToPlatform());
			}
#endif
		}
	}
}
