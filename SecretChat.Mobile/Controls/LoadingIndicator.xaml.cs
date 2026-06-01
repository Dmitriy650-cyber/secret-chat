namespace SecretChat.Mobile.Controls;

public partial class LoadingIndicator : VerticalStackLayout
{
	public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(
		nameof(IsBusy),
		typeof(bool),
		typeof(LoadingIndicator),
		false,
		BindingMode.OneWay,
		null,
		SetIsBusy
		);

	private static void SetIsBusy(BindableObject bindable, object oldValue, object newValue)
	{
		LoadingIndicator? control = bindable as LoadingIndicator;

		control?.IsVisible = (bool)newValue;
		control?.actIndicator.IsRunning = (bool)newValue;
	}

	public bool IsBusy
	{
		get => (bool)GetValue(IsBusyProperty);
		set => SetValue(IsBusyProperty, value);
	}

	public static readonly BindableProperty LoadingTextProperty = BindableProperty.Create(
		nameof(LoadingText),
		typeof(string),
		typeof(LoadingIndicator),
		string.Empty,
		BindingMode.OneWay,
		null,
		SetLoadingText
		);

	private static void SetLoadingText(BindableObject bindable, object oldValue, object newValue) =>
		(bindable as LoadingIndicator)?.lblLoadingText.Text = (string)newValue;

	public string LoadingText
	{
		get => (string)GetValue(LoadingTextProperty);
		set => SetValue(LoadingTextProperty, value);
	}

	public LoadingIndicator()
	{
		InitializeComponent();
	}
}