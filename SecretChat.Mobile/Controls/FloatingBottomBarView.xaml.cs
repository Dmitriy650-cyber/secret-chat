namespace SecretChat.Mobile.Controls;

public partial class FloatingBottomBarView : ContentView
{
	private SimpleButton? _currentButton;
	private string? _currentPageKey = $"{nameof(HomePage)}";

	public FloatingBottomBarView()
	{
		InitializeComponent();

		SetActiveButton(_currentPageKey);
	}

	public event Action<object, TabBarEventArgs>? CurrentPageSelectionChanged;

	private ICommand CreateClickCommand()
	{
		return new Command<string>(pageKey => 
		{
			SetActiveButton(pageKey);

			CurrentPageSelectionChanged?.Invoke(this, new TabBarEventArgs(pageKey));
		});
	}

	public ICommand ClickCommand => CreateClickCommand();

	private async void SetActiveButton(string pageKey)
	{
		// Сбросить предыдущую кнопку
		if (_currentButton != null)
		{
			await SimpleButtonAnimator.AnimateToColor(_currentButton, Colors.White, MobileConstants.AnimationDuration);

			RestoreNormalState(_currentButton);
		}

		// Найти новую кнопку
		SimpleButton? newButton = pageKey switch
		{
			$"{nameof(HomePage)}" => this.FindByName<SimpleButton>("HomePageButton"),
			$"{nameof(ProfilePage)}" => this.FindByName<SimpleButton>("ProfilePageButton"),
			$"{nameof(ContactsPage)}" => this.FindByName<SimpleButton>("ContactsPageButton"),
			_ => null
		};

		if (newButton != null)
		{
			RestoreNormalState(newButton);
			await SimpleButtonAnimator.AnimateToColor(newButton, Color.FromArgb("#DFD8F7"), MobileConstants.AnimationDuration);

			MakeButtonActive(newButton);
			_currentButton = newButton;
			_currentPageKey = pageKey;
		}
	}

	private void MakeButtonActive(SimpleButton button)
	{
		//button.Background = Color.FromArgb("#DFD8F7");
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
	}

	private void RestoreNormalState(SimpleButton button)
	{
		//button.Background = Colors.White;
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
}