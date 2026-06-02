namespace SecretChat.Mobile.Pages;

public partial class DetailsPage : BaseView<DetailsViewModel>
{
	public DetailsPage()
	{
		InitializeComponent();
		ViewModelInitializedAction += InitiazeAction;
		ViewModelDisappearingAction += DisappearingAction;
	}

	private void DisappearingAction(object? sender, EventArgs e)
	{
		ViewModel.RemoveHandlers();
	}

	private void InitiazeAction(object? sender, EventArgs e)
	{
		ViewModel.ConfigureRealtimeUpdates();
	}
}