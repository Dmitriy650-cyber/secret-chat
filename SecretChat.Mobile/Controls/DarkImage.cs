namespace SecretChat.Mobile.Controls
{
    public class DarkImage : Image
    {
		public DarkImage()
		{
			if (App.Current!.Resources.TryGetValue("PrimaryDark", out object color) && color is Color primaryDark)
			{
				var tintColorBehavior = Behaviors.FirstOrDefault(n => n is IconTintColorBehavior);
				if (tintColorBehavior == null)
				{
					tintColorBehavior = new IconTintColorBehavior
					{
						TintColor = primaryDark
					};
					Behaviors.Add(tintColorBehavior);
				}
			}
		}
	}
}
