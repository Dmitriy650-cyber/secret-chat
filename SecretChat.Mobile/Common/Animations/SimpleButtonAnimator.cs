namespace SecretChat.Mobile.Common.Animations
{
	public static class SimpleButtonAnimator
	{
		public static async Task AnimateToColor(SimpleButton button, Color toColor, int animataionDuration)
		{
			var fromColor = button.BackgroundColor ?? Colors.White;

			var animation = new Animation
			{
				{
					0, 1,
					new Animation(
						v =>
						{
							var r = (float)(fromColor.Red   + (toColor.Red   - fromColor.Red)   * v);
							var g = (float)(fromColor.Green + (toColor.Green - fromColor.Green) * v);
							var b = (float)(fromColor.Blue  + (toColor.Blue  - fromColor.Blue)  * v);
							var a = (float)(fromColor.Alpha + (toColor.Alpha - fromColor.Alpha) * v);

							button.Background = Color.FromRgb(r, g, b).WithAlpha(a);
						},
						0, 1, Easing.Linear)
				}
			};

			await Task.Run(() =>
			{
				animation.Commit(
					owner: button,
					name: "ColorAnimation",
					length: (uint)animataionDuration,
					finished: null);
			});

			button.Background = toColor;
		}
	}
}
