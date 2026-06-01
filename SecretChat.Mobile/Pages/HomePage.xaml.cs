namespace SecretChat.Mobile.Pages;

public partial class HomePage : BaseView<HomeViewModel>
{
	private SimpleButton? _currentButton;

	public HomePage() 
	{
		InitializeComponent();

		AllChatsButton_Clicked(this.FindByName("AllChatsButton"), EventArgs.Empty);
		ViewModelInitializedAction += InitializedAction;
	}

	private void InitializedAction(object? sender, EventArgs e)
	{
		ViewModel.ConfigureRealtimeUpdates();
	}

	protected override bool OnBackButtonPressed()
	{
		return true;
	}

	private async void AllChatsButton_Clicked(object sender, EventArgs e)
	{
		if (_currentButton != null)
		{
			await SimpleButtonAnimator.AnimateToColor(_currentButton, Colors.White, MobileConstants.AnimationDuration);

			RestoreNormalState(_currentButton);
		}

		if (sender is SimpleButton newButton)
		{
			RestoreNormalState(newButton);
			await SimpleButtonAnimator.AnimateToColor(newButton, Color.FromArgb("#DFD8F7"), MobileConstants.AnimationDuration);

			MakeButtonActive(newButton);
			_currentButton = newButton;
		}
	}

	private void MakeButtonActive(SimpleButton button)
	{
		if (button.Content is View content)
		{
			if (content is Label label)
			{
				label.TextColor = Color.FromArgb("#512BD4");
			}
			else if (content is StackLayout sl && sl.Children[0] is Label nestedLabel)
			{
				nestedLabel.TextColor = Color.FromArgb("#512BD4");
			}
		}

		if (ViewModel is null)
			return;

		if (AllChatsButton == button)
		{
			ViewModel.RemoveFavoriteMode();
		}
		else
		{
			ViewModel.SetFavoriteMode();
		}
	}

	private void RestoreNormalState(SimpleButton button)
	{
		if (button.Content is View content)
		{
			if (content is Label label)
			{
				label.TextColor = Colors.Black;
			}
			else if (content is StackLayout sl && sl.Children[0] is Label nestedLabel)
			{
				nestedLabel.TextColor = Colors.Black;
			}
		}
	}

	private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(ChatPage), animate: true);
	}
}