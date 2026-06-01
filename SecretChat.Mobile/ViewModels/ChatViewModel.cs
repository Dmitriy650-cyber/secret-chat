namespace SecretChat.Mobile.ViewModels
{
    public partial class ChatViewModel(IConnectivity connectivity, IMessageApi messageApi, IUserApi userApi, AuthService authService, RealtimeUpdateService realtimeUpdateService) : BaseViewModel(connectivity, authService)
    {
		private byte[] _chatKey = null!;

		public ObservableCollection<CollectionViewItemMessage> Messages { get; set; } = [];
		private ObservableCollection<MessageDto> _messages { get; set; } = [];

		[ObservableProperty]
		private LoggedInUserDto? _user;

		[ObservableProperty]
		private ChatDto? _chat;

		[ObservableProperty]
		private string? _content;
		[ObservableProperty]
		private string? _filePath;
		[ObservableProperty]
		private string? _mediaFilePath;

		[ObservableProperty]
		private bool _isShowMic = true;

		partial void OnContentChanged(string? value)
		{
			if (!string.IsNullOrEmpty(value))
				IsShowMic = false;
			else
				IsShowMic = true;
		}

		public override async Task InitializeViewModel()
		{
			WeakReferenceMessenger.Default.Register<FromHomeToChatMessage>(this, (r, m) => this.Receive(m));

			GoBackAction += GoBackHandler;

			await MakeFirstApiRequestAsync(async () =>
			{
		
			});
		}

		[RelayCommand]
		private async Task SendMessageAsync()
		{
			await MakeApiRequestAsync(async () =>
			{
				if (Chat is null)
					return;

				if (MediaFilePath is not null)
				{
					// todo
				}
				else if (FilePath is not null)
				{
					// todo
				}
				else
				{
					if (Content is null)
						return;

					// todo
					//var secretContent = AesHelper.Encrypt(Content, _chatKey);
					//CreatableUpdatableMessageDto message = new(Chat.Id, secretContent);

					var message = new CreatableUpdatableMessageDto(Chat.Id, Content);

					var serializedCreatableUpdatableMessageDto = JsonSerializer.Serialize(message);

					var response = await messageApi.CreateMessageWithoutFileAsync(serializedCreatableUpdatableMessageDto);

					if (!response.IsSuccess)
					{
						await ShowErrorAlertAsync(response.Message);
						return;
					}

					Content = null;
				}
			});
		}

		[RelayCommand]
		private async Task GoToDetailsPageAsync()
		{
			bool isUserContact = false;

			await MakeApiRequestAsync(async () =>
			{
				var response = await userApi.CheckContactAsync(User!.Id);

				if (response.IsSuccess)
					isUserContact = true;
				else
					isUserContact = false;
			});

			await NavigateAsync(nameof(DetailsPage));

			WeakReferenceMessenger.Default.Send(new FromContactToDetailsMessage
			{
				User = User!,
				IsUserContact = isUserContact,
				IsHaveChat = true,
				IsFromChat = true
			});
		}

		private void GoBackHandler(object? sender, EventArgs e)
		{
			realtimeUpdateService.RemoveHandlers(nameof(ChatViewModel));
			GoBackAction -= GoBackHandler;
		}

		private async void Receive(FromHomeToChatMessage message)
		{
			try
			{
				User = message.Chat.WithUser;
				Chat = message.Chat;

				var keyData = await SecureStorageHelper.LoadChatKeyAsync(Chat.Id);
				if (keyData == null)
				{
					await ShowErrorAlertAsync("No key for chat");
				}
				else
				{
					var (key, _) = keyData.Value;

					_chatKey = key;
				}
					//throw new InvalidOperationException($"No key found for chat {Chat.Id}");

				await MakeApiRequestAsync(async () =>
				{
					var response = await messageApi.GetChatMessagesAsync(Chat.Id);

					if (!response.IsSuccess)
					{
						await ShowErrorAlertAsync(response.Message);
						return;
					}

					_messages.AddRange(response.Data);
					RefreshMessages();
				});
			}
			catch (Exception)
			{
				await ShowErrorAlertAsync("No key for chat");
				//await NavigateBackAsync();
			}
		}

		private void RefreshMessages()
		{
			Messages.Clear();

			var messages = _messages.OrderBy(n => n.SendOn);
			var count = messages.Count();

			Messages.AddRange(messages.Select((item, index) => new CollectionViewItemMessage
			{
				Item = item,
				ItemIndex = index,
				ItemCount = count,
				IsCurrentUser = AuthService.User!.Id == item.UserId
			}));
		}

		public void ConfigureRealtimeUpdates()
		{
			realtimeUpdateService.AddMessageSentHandler(nameof(ChatViewModel), OnMessageSent);
			realtimeUpdateService.AddMessageDeletedHandler(nameof(ChatViewModel), OnMessageDeleted);
		}

		private void OnMessageDeleted(int id)
		{
			var message = _messages.FirstOrDefault(n => n.Id == id);
			if (message is null)
				return;

			_messages.Remove(message);
			RefreshMessages();
		}

		private async void OnMessageSent(MessageDto dto)
		{
			try
			{
				if (dto.ChatId != Chat!.Id)
					return;

				var message = _messages.FirstOrDefault(n => n.Id == dto.Id);
				if (message is not null)
				{
					_messages.Remove(message);
				}

				//if (dto.Content is { })
				//{
				//	var decryptContent = AesHelper.Decrypt(dto.Content, _chatKey);
				//	var decryptMessage = new MessageDto(
				//		dto.Id,
				//		decryptContent,
				//		dto.FileUrl,
				//		dto.SendOn,
				//		dto.ModifiedOn,
				//		dto.UserId,
				//		dto.ChatId);
				//	_messages.Add(decryptMessage);
				//	RefreshMessages();
				//	return;
				//}

				_messages.Add(dto);
				RefreshMessages();
			}
			catch (Exception ex)
			{
				await ShowErrorAlertAsync(ex.Message);
			}
		}
	}
}
