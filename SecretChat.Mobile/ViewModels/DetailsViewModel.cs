namespace SecretChat.Mobile.ViewModels
{
    public partial class DetailsViewModel(IConnectivity connectivity, IPhotoApi photoApi, IUserApi userApi, IChatApi chatApi, AuthService authService, RealtimeUpdateService realtimeUpdateService) : BaseViewModel(connectivity, authService)
    {
		[ObservableProperty]
		private LoggedInUserDto? _user;
		[ObservableProperty]
		private bool _isUserConstact;
		[ObservableProperty]
		private bool _isHaveChat;

		private bool _isFromChat = false;

		public ObservableCollection<PhotoDto> Photos { get; set; } = [];

		public override async Task InitializeViewModel()
		{
			WeakReferenceMessenger.Default.Register<FromContactToDetailsMessage>(this, (r, m) => this.Receive(m));

			await MakeFirstApiRequestAsync(async () =>
			{

			});
		}

		public async void Receive(FromContactToDetailsMessage message)
		{
			User = message.User;
			IsUserConstact = message.IsUserContact;
			IsHaveChat = message.IsHaveChat;
			_isFromChat = message.IsFromChat;

			await MakeApiRequestAsync(async () =>
			{
				var response = await photoApi.GetUserPhotosByIdAsync(User.Id);

				if (!response.IsSuccess || !response.Data.Any())
				{
					Photos.Add(new PhotoDto(0, "user.png", false));
					return;
				}

				Photos.AddRange(response.Data);
			});
		}

		[RelayCommand]
		private async Task NewGoBackAsync()
		{
			if (_isFromChat && !IsHaveChat)
				await NavigateAsync($"//{nameof(HomePage)}");
			else
				await NavigateBackAsync();
		}

		[RelayCommand]
		private async Task AddOrDeleteContactAsync()
		{
			await MakeApiRequestAsync(async () =>
			{
				if (IsUserConstact)
				{
					var response = await userApi.DeleteContactAsync(User!.Id);

					if (!response.IsSuccess)
					{
						await ShowErrorAlertAsync(response.Message);
						return;
					}

					await ShowToastAsync("Contact deleted successfully");
				}
				else
				{
					var response = await userApi.AddContactAsync(User!.Id);

					if (!response.IsSuccess)
					{
						await ShowErrorAlertAsync(response.Message);
						return;
					}

					await ShowToastAsync("Contact added successfully");
				}

				IsUserConstact = !IsUserConstact;
			});
		}

		[RelayCommand]
		private async Task CreateOrDeleteChatAsync()
		{
			await MakeApiRequestAsync(async () =>
			{
				if (IsHaveChat)
				{
					if (!await ShowDialogAlertAsync("Are you sure?", "If you delete the chat it can't be recovered. Delete chat?"))
					{
						return;
					}

					var chatResponse = await chatApi.GetChatByUserIdAsync(User!.Id);

					if (!chatResponse.IsSuccess)
					{
						await ShowErrorAlertAsync(chatResponse.Message);
						return;
					}

					var response = await chatApi.DeleteChatAsync(chatResponse.Data.Id);

					if (!response.IsSuccess)
					{
						await ShowErrorAlertAsync(response.Message);
						return;
					}

					await SecureStorageHelper.DeleteChatKeyAsync(chatResponse.Data.Id);

					await ShowToastAsync("Chat deleted successfully");
				}
				else
				{
					var response = await chatApi.CreateChatAsync(new CreatableChatDto(AuthService.User!.Id, User!.Id));

					if (!response.IsSuccess)
					{
						await ShowErrorAlertAsync(response.Message);
						return;
					}

					var (key, salt) = KeyGenerator.CreateChatKey();
					await SecureStorageHelper.SaveChatKeyAsync(response.Data.Id, key, salt);

					await ShowToastAsync("Chat created successfully");
				}

				IsHaveChat = !IsHaveChat;
			});
		}

		public void ConfigureRealtimeUpdates()
		{
			realtimeUpdateService.AddUserCreatedOrUpdatedHandler(nameof(DetailsViewModel), OnUserCreatedOrUpdated);
		}

		private void OnUserCreatedOrUpdated(LoggedInUserDto dto)
		{
			if (User!.Id == dto.Id)
			{
				User = dto;
			}
		}

		public void RemoveHandlers()
		{
			realtimeUpdateService.RemoveHandlers(nameof(DetailsViewModel));
		}
	}
}
