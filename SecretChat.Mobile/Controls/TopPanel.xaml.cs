namespace SecretChat.Mobile.Controls;

public partial class TopPanel : Grid
{
	public static readonly BindableProperty LeftTextProperty = BindableProperty.Create(
		nameof(LeftText),
		typeof(string),
		typeof(TopPanel),
		string.Empty,
		BindingMode.OneWay,
		null,
		OnLeftTextChanged
		);

	private static void OnLeftTextChanged(BindableObject bindable, object oldValue, object newValue) =>
		(bindable as TopPanel)?.LeftTextLabel.Text = (string)newValue;

	public string LeftText
	{
		get => (string)GetValue(LeftTextProperty); 
		set => SetValue(LeftTextProperty, value);
	}

	public static readonly BindableProperty MiddleTextProperty = BindableProperty.Create(
		nameof(MiddleText),
		typeof(string),
		typeof(TopPanel),
		string.Empty,
		BindingMode.OneWay,
		null,
		OnMiddleTextChanged
		);

	private static void OnMiddleTextChanged(BindableObject bindable, object oldValue, object newValue) =>
		(bindable as TopPanel)?.MiddleTextLabel.Text = (string)newValue;

	public string MiddleText
	{
		get => (string)GetValue(MiddleTextProperty);
		set => SetValue(MiddleTextProperty, value);
	}

	public static readonly BindableProperty RightTextProperty = BindableProperty.Create(
		nameof(RightText),
		typeof(string),
		typeof(TopPanel),
		string.Empty,
		BindingMode.OneWay,
		null,
		OnRightTextChanged
		);

	private static void OnRightTextChanged(BindableObject bindable, object oldValue, object newValue) =>
		(bindable as TopPanel)?.RightTextLabel.Text = (string)newValue;

	public string RightText
	{
		get => (string)GetValue(RightTextProperty);
		set => SetValue(RightTextProperty, value);
	}

	public static readonly BindableProperty LeftTextCommandProperty = BindableProperty.Create(
		nameof(LeftTextCommand),
		typeof(ICommand),
		typeof(TopPanel),
		propertyChanged: OnLeftTextCommandChanged
		);

	private static void OnLeftTextCommandChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var panel = (TopPanel)bindable;
		if (panel.LeftTextLabel.GestureRecognizers.Count == 0)
		{
			var tap = new TapGestureRecognizer();
			panel.LeftTextLabel.GestureRecognizers.Add(tap);
		}
		(panel.LeftTextLabel.GestureRecognizers[0] as TapGestureRecognizer)?.Command = (ICommand)newValue;
	}

	public ICommand LeftTextCommand
	{
		get => (ICommand)GetValue(LeftTextCommandProperty);
		set => SetValue(LeftTextCommandProperty, value);
	}

	public static readonly BindableProperty RightTextCommandProperty = BindableProperty.Create(
		nameof(RightTextCommand),
		typeof(ICommand),
		typeof(TopPanel),
		propertyChanged: OnRightTextCommandChanged
		);

	private static void OnRightTextCommandChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var panel = (TopPanel)bindable;
		if (panel.RightTextLabel.GestureRecognizers.Count == 0)
		{
			var tap = new TapGestureRecognizer();
			panel.RightTextLabel.GestureRecognizers.Add(tap);
		}
		(panel.RightTextLabel.GestureRecognizers[0] as TapGestureRecognizer)?.Command = (ICommand)newValue;
	}

	public ICommand RightTextCommand
	{
		get => (ICommand)GetValue(RightTextCommandProperty);
		set => SetValue(RightTextCommandProperty, value);
	}

	public static readonly BindableProperty LeftTextColorProperty = BindableProperty.Create(
		nameof(LeftTextColor),
		typeof(Color),
		typeof(TopPanel),
		Colors.Black,
		BindingMode.OneWay,
		null,
		OnLeftTextColorChanged
		);

	private static void OnLeftTextColorChanged(BindableObject bindable, object oldValue, object newValue) =>
		(bindable as TopPanel)?.LeftTextLabel.TextColor = (Color)newValue;

	public Color LeftTextColor
	{
		get => (Color)GetValue(LeftTextColorProperty);
		set => SetValue(LeftTextColorProperty, value);
	}

	public static readonly BindableProperty RightTextColorProperty = BindableProperty.Create(
		nameof(RightTextColor),
		typeof(Color),
		typeof(TopPanel),
		Colors.Black,
		BindingMode.OneWay,
		null,
		OnRightTextColorChanged
		);

	private static void OnRightTextColorChanged(BindableObject bindable, object oldValue, object newValue) =>
		(bindable as TopPanel)?.RightTextLabel.TextColor = (Color)newValue;

	public Color RightTextColor
	{
		get => (Color)GetValue(RightTextColorProperty);
		set => SetValue(RightTextColorProperty, value);
	}

	public static readonly BindableProperty UserImageProperty = BindableProperty.Create(
		nameof(UserImage),
		typeof(ImageSource),
		typeof(TopPanel),
		null,
		BindingMode.OneWay,
		null,
		OnUserImageChanged
		);

	private static void OnUserImageChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var topPanel = bindable as TopPanel;

		topPanel?.UserImageControl.ImageSource = (ImageSource)newValue;
		topPanel?.UserImageControl.IsVisible = true;
	}

	public ImageSource UserImage
	{
		get => (ImageSource)GetValue(UserImageProperty);
		set => SetValue(UserImageProperty, value);
	}

	public static readonly BindableProperty UserImageCommandProperty = BindableProperty.Create(
		nameof(UserImageCommand),
		typeof(ICommand),
		typeof(TopPanel),
		null,
		BindingMode.OneWay,
		null,
		OnUserImageCommandChanged
		);

	private static void OnUserImageCommandChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var panel = (TopPanel)bindable;
		if (panel.UserImageControl.GestureRecognizers.Count == 0)
		{
			var tap = new TapGestureRecognizer();
			panel.UserImageControl.GestureRecognizers.Add(tap);
		}
		(panel.UserImageControl.GestureRecognizers[0] as TapGestureRecognizer)?.Command = (ICommand)newValue;
	}

	public ICommand UserImageCommand
	{
		get => (ICommand)GetValue(UserImageCommandProperty);
		set => SetValue(UserImageCommandProperty, value);
	}

	public static readonly BindableProperty UserImageCommandParameterProperty = BindableProperty.Create(
		nameof(UserImageCommandParameter),
		typeof(object),
		typeof(TopPanel),
		null,
		BindingMode.OneWay,
		null,
		OnUserImageCommandParameterChanged
		);

	private static void OnUserImageCommandParameterChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var panel = (TopPanel)bindable;
		if (panel.UserImageControl.GestureRecognizers.Count == 0)
		{
			return;
		}
		(panel.UserImageControl.GestureRecognizers[0] as TapGestureRecognizer)?.CommandParameter = newValue;
	}

	public object UserImageCommandParameter
	{
		get => GetValue(UserImageCommandParameterProperty);
		set => SetValue(UserImageCommandParameterProperty, value);
	}

	public TopPanel()
	{
		InitializeComponent();
	}
}