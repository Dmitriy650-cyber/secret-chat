namespace SecretChat.Mobile.Pages;

public partial class ChatPage : BaseView<ChatViewModel>
{
	public ChatPage()
	{
		InitializeComponent();
		ViewModelInitializedAction += InitializeAction;
	}

	private void InitializeAction(object? sender, EventArgs e)
	{
		ViewModel.ConfigureRealtimeUpdates();
	}
}