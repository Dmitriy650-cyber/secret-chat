namespace SecretChat.Mobile.Controls
{
	public class WhiteImage : Image
	{
		public WhiteImage()
		{
			if (App.Current!.Resources.TryGetValue("White", out object color) && color is Color white)
			{
				var tintColorBehavior = Behaviors.FirstOrDefault(n => n is IconTintColorBehavior);
				if (tintColorBehavior == null)
				{
					tintColorBehavior = new IconTintColorBehavior
					{
						TintColor = white
					};
					Behaviors.Add(tintColorBehavior);
				}
			}
		}
	}
}
