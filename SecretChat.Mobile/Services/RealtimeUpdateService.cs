namespace SecretChat.Mobile.Services
{
	public class RealtimeUpdateService
	{
		private HubConnection? _hubConnection;

		private readonly Dictionary<string, Action<ChatDto>> _chatCreatedActions = [];
		public void AddChatCreatedHandler(string key, Action<ChatDto> handler) =>
			_chatCreatedActions[key] = handler;

		private readonly Dictionary<string, Action<int>> _chatDeletedActions = [];
		public void AddChatDeletedHandler(string key, Action<int> handler) =>
			_chatDeletedActions[key] = handler;

		private readonly Dictionary<string, Action<LoggedInUserDto>> _userCreatedOrUpdatedActions = [];
		public void AddUserCreatedOrUpdatedHandler(string key, Action<LoggedInUserDto> handler) =>
			_userCreatedOrUpdatedActions[key] = handler;

		private readonly Dictionary<string, Action<int>> _userDeletedActions = [];
		public void AddUserDeletedHandler(string key, Action<int> handler) =>
			_userDeletedActions[key] = handler;

		private readonly Dictionary<string, Action<int>> _contactAddedActions = [];
		public void AddContactAddedHandler(string key, Action<int> handler) =>
			_contactAddedActions[key] = handler;

		private readonly Dictionary<string, Action<int>> _contactDeletedActions = [];
		public void AddContactDeletedHandler(string key, Action<int> handler) =>
			_contactDeletedActions[key] = handler;

		private readonly Dictionary<string, Action<MessageDto>> _messageSentActions = [];
		public void AddMessageSentHandler(string key, Action<MessageDto> handler) =>
			_messageSentActions[key] = handler;

		private readonly Dictionary<string, Action<int>> _messageDeletedActions = [];
		public void AddMessageDeletedHandler(string key, Action<int> handler) =>
			_messageDeletedActions[key] = handler;

		public async Task ConfigureRealtimeUpdates()
		{
			try
			{
				_hubConnection = new HubConnectionBuilder()
					.WithUrl(AppConstants.HubFullUrl)
					.WithAutomaticReconnect()
					.Build();

				_hubConnection.On<ChatDto>(nameof(ISecretChatHubClient.ChatCreatedAsync), chat =>
				{
					MainThread.BeginInvokeOnMainThread(() =>
					{
						foreach (var (key, action) in _chatCreatedActions)
						{
							try
							{
								action.Invoke(chat);
							}
							catch (Exception)
							{

							}
						}
					});
				});

				_hubConnection.On<int>(nameof(ISecretChatHubClient.ChatDeletedAsync), id =>
				{
					MainThread.BeginInvokeOnMainThread(() =>
					{
						foreach (var (key, action) in _chatDeletedActions)
						{
							try
							{
								action.Invoke(id);
							}
							catch (Exception)
							{

							}
						}
					});
				});

				_hubConnection.On<LoggedInUserDto>(nameof(ISecretChatHubClient.UserCreatedOrUpdatedAsync), user =>
				{
					MainThread.BeginInvokeOnMainThread(() =>
					{
						foreach(var (key, action) in _userCreatedOrUpdatedActions)
						{
							try
							{
								action.Invoke(user);
							}
							catch (Exception)
							{
								
							}
						}
					});
				});

				_hubConnection.On<int>(nameof(ISecretChatHubClient.UserDeletedAsync), id =>
				{
					MainThread.BeginInvokeOnMainThread(() =>
					{
						foreach(var (key, action) in _userDeletedActions)
						{
							try
							{
								action.Invoke(id);
							}
							catch (Exception)
							{

							}
						}
					});
				});

				_hubConnection.On<int>(nameof(ISecretChatHubClient.ContactAddedAsync), id =>
				{
					MainThread.BeginInvokeOnMainThread(() =>
					{
						foreach(var (key, action) in _contactAddedActions)
						{
							try
							{
								action.Invoke(id);
							}
							catch (Exception)
							{

							}
						}
					});
				});

				_hubConnection.On<int>(nameof(ISecretChatHubClient.ContactDeletedAsync), id =>
				{
					MainThread.BeginInvokeOnMainThread(() =>
					{
						foreach(var (key, action) in _contactDeletedActions)
						{
							try
							{
								action.Invoke(id);
							}
							catch (Exception)
							{

							}
						}
					});
				});

				_hubConnection.On<MessageDto>(nameof(ISecretChatHubClient.MessageSentAsync), message =>
				{
					MainThread.BeginInvokeOnMainThread(() =>
					{
						foreach(var (key, action) in _messageSentActions)
						{
							try
							{
								action.Invoke(message);
							}
							catch (Exception)
							{

							}
						}
					});
				});

				_hubConnection.On<int>(nameof(ISecretChatHubClient.MessageDeletedAsync), id =>
				{
					MainThread.BeginInvokeOnMainThread(() =>
					{
						foreach(var (key, action) in _messageDeletedActions)
						{
							try
							{
								action.Invoke(id);
							}
							catch (Exception)
							{

							}
						}
					});
				});

				await _hubConnection.StartAsync();
			}
			catch (Exception)
			{

			}
		}

		public void RemoveHandlers(string key)
		{
			if (_chatCreatedActions.ContainsKey(key))
				_chatCreatedActions.Remove(key);

			if (_chatDeletedActions.ContainsKey(key))
				_chatDeletedActions.Remove(key);

			if (_userCreatedOrUpdatedActions.ContainsKey(key))
				_userCreatedOrUpdatedActions.Remove(key);

			if (_userDeletedActions.ContainsKey(key))
				_userDeletedActions.Remove(key);

			if (_contactAddedActions.ContainsKey(key))
				_contactAddedActions.Remove(key);

			if (_contactDeletedActions.ContainsKey(key))
				_contactDeletedActions.Remove(key);

			if (_messageSentActions.ContainsKey(key))
				_messageSentActions.Remove(key);

			if (_messageDeletedActions.ContainsKey(key))
				_messageDeletedActions.Remove(key);
		}
	}
}
