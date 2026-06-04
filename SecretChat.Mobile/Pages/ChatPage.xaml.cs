namespace SecretChat.Mobile.Pages;

public partial class ChatPage : BaseView<ChatViewModel>
{
	public ChatPage()
	{
		InitializeComponent();
		ViewModelInitializedAction += InitializeAction;
		ViewModelDisappearingAction += DisappearingAction;
	}

	private void DisappearingAction(object? sender, EventArgs e)
	{
		ViewModel.RemoveHandlers();
		ViewModel.MakeMessagesReaded();
	}

	private void InitializeAction(object? sender, EventArgs e)
	{
		ViewModel.ConfigureRealtimeUpdates();
	}
}