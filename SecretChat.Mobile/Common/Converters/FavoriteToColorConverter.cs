namespace SecretChat.Mobile.Common.Converters
{
	public class FavoriteToColorConverter : IValueConverter
	{
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
			value is true ? Color.FromArgb("#512BD4") : Colors.White;

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
