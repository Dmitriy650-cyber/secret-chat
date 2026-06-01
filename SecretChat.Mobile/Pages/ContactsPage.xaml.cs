namespace SecretChat.Mobile.Pages;

public partial class ContactsPage : BaseView<ContactsViewModel>
{
	public ContactsPage()
	{
		InitializeComponent();
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

	private void NoBorderEntry_TextChanged(object sender, TextChangedEventArgs e)
	{
		if ((e.OldTextValue is null && e.NewTextValue is not null) || (e.OldTextValue is "" && e.NewTextValue is not null))
		{
			ViewModel.IsSearchMode = true;
		}

		if (e.NewTextValue is null || e.NewTextValue == "")
		{
			ViewModel.IsSearchMode = false;
			return;
		}

		ViewModel.UpdateSortUsers(e.NewTextValue);
	}
}