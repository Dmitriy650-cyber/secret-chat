namespace SecretChat.Mobile.Common.Converters
{
	public class FavoriteModeVisibilityConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.Length != 2)
				return false;

			if (values[0] is bool isFavoriteMode && values[1] is bool isFavorite)
			{
				if (isFavoriteMode == false)
				{
					return true;
				}
				if (isFavoriteMode == true && isFavorite == true)
				{
					return true;
				}
				else
				{
					return false;
				}
			}

			return false;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
