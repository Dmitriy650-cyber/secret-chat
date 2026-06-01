namespace SecretChat.Mobile.Controls;

public partial class UserImage : Border
{
	public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
		nameof(ImageSource),
		typeof(ImageSource),
		typeof(UserImage),
		null,
		BindingMode.OneWay,
		null,
		OnImageSourceChanged
		);

	private static void OnImageSourceChanged(BindableObject bindable, object oldValue, object newValue) =>
		(bindable as UserImage)?.Image.Source = (ImageSource)newValue;

	public ImageSource ImageSource
	{
		get => (ImageSource)GetValue(ImageSourceProperty);
		set => SetValue(ImageSourceProperty, value);
	}

	public UserImage()
	{
		InitializeComponent();
	}
}