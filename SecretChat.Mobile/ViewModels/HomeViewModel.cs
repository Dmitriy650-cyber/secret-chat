namespace SecretChat.Mobile.ViewModels
{
	public partial class HomeViewModel(IConnectivity connectivity, IChatApi chatApi, RealtimeUpdateService realtimeUpdateService, AuthService authService) : BaseViewModel(connectivity, authService)
	{
		public ObservableCollection<CollectionViewItemChat> Chats { get; set; } = [];
		private ObservableCollection<ChatDto> _allChats { get; set; } = [];

		[ObservableProperty]
		private bool _isFavoriteChats = false;

		public override async Task InitializeViewModel()
		{
			await MakeFirstApiRequestAsync(async () =>
			{
				var response = await chatApi.GetUserChatsAsync();

				if (!response.IsSuccess)
				{
					await ShowErrorAlertAsync(response.Message);
					return;
				}

				if (!response.Data.Any())
					return;

				_allChats.AddRange(response.Data);
				RefreshChats();
			});
		}

		[RelayCommand]
		private async Task GoToChatPageAsync(ChatDto dto)
		{
			await NavigateAsync(nameof(ChatPage));

			WeakReferenceMessenger.Default.Send(new FromHomeToChatMessage
			{
				Chat = dto
			});
		}

		private void RefreshChats()
		{
			Chats.Clear();

			var chats = _allChats.OrderByDescending(n => n.LastMessage?.SendOn);
			var count = chats.Count();

			Chats.AddRange(chats.Select((item, index) => new CollectionViewItemChat
			{
				Item = item,
				ItemIndex = index,
				ItemCount = count
			}));
		}

		public bool IsItInTheChat(LoggedInUserDto dto) =>
			Chats.Any(n => n.Item.WithUser.Id == dto.Id);

		public void SetFavoriteMode() => IsFavoriteChats = true;
		public void RemoveFavoriteMode() => IsFavoriteChats = false;

		public void ConfigureRealtimeUpdates()
		{
			realtimeUpdateService.AddChatCreatedHandler(nameof(HomeViewModel), OnChatCreated);
			realtimeUpdateService.AddChatDeletedHandler(nameof(HomeViewModel), OnChatDeleted);
			realtimeUpdateService.AddMessageSentHandler(nameof(HomeViewModel), OnMessageSent);
			realtimeUpdateService.AddUserCreatedOrUpdatedHandler(nameof(HomeViewModel), OnUserCreatedOrUpdated);
		}

		private void OnUserCreatedOrUpdated(LoggedInUserDto dto)
		{
			var chat = _allChats.FirstOrDefault(n => n.WithUser.Id == dto.Id);
			if (chat is not null)
			{
				chat.WithUser = dto;
				RefreshChats();
			}
		}

		private void OnMessageSent(MessageDto dto)
		{
			var chat = _allChats.FirstOrDefault(n => n.Id == dto.ChatId);
			if (chat is null)
				return;

			chat.LastMessage = dto;

			RefreshChats();
		}

		private void OnChatDeleted(int id)
		{
			var chat = _allChats.FirstOrDefault(n => n.Id == id);
			if (chat is null)
				return;

			_allChats.Remove(chat);
			RefreshChats();
		}

		private void OnChatCreated(ChatDto dto)
		{
			if (dto.User.Id != AuthService.User!.Id)
				return;

			_allChats.Add(dto);
			RefreshChats();
		}
	}
}
