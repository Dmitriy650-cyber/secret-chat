namespace SecretChat.Mobile.Common.Converters
{
	public class InverseBooleanConverter : IValueConverter
	{
		public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
			value is bool ? !((bool)value) : false;

		public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
			value is bool ? !((bool)value) : false;
	}
}
