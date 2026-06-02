namespace SecretChat.Mobile.ViewModels
{
	public partial class ProfileViewModel(IConnectivity connectivity, AuthService authService, IPhotoApi photoApi, RealtimeUpdateService realtimeUpdateService) : BaseViewModel(connectivity, authService)
	{
		[ObservableProperty]
		private LoggedInUserDto _user = authService.User!;
		[ObservableProperty]
		private int _selectedIndex;

		public ObservableCollection<PhotoDto> Photos { get; set; } = [];

		public override async Task InitializeViewModel()
		{
			await MakeFirstApiRequestAsync(async () =>
			{
				var response = await photoApi.GetUserPhotosAsync();

				if (!response.IsSuccess)
				{
					await ShowErrorAlertAsync(response.Message);
					return;
				}

				Photos.AddRange(response.Data);

				if (Photos.Count == 0)
				{
					Photos.Add(new PhotoDto(0, "user.png", true));
				}

				DeletePhotoCommand.NotifyCanExecuteChanged();
				SetMainPhotoCommand.NotifyCanExecuteChanged();
			});
		}

		partial void OnSelectedIndexChanged(int value)
		{
			DeletePhotoCommand.NotifyCanExecuteChanged();
			SetMainPhotoCommand.NotifyCanExecuteChanged();
		}

		[RelayCommand]
		private async Task GoToEditPageAsync()
		{
			await NavigateAsync(nameof(EditPage));
		}

		[RelayCommand]
		private async Task AddPhotoAsync()
		{
			var photoPath = await ChoosePhotoAsync();
			if (photoPath is null)
			{
				await ShowToastAsync("Photo wasn't chosen");
				return;
			}

			await MakeApiRequestAsync(async () =>
			{
				var fileName = Path.GetFileName(photoPath);
				using var fileStream = File.OpenRead(photoPath);
				var photoStreamPart = new StreamPart(fileStream, fileName);

				var response = await photoApi.AddUserPhotoAsync(photoStreamPart);

				if (!response.IsSuccess)
				{
					await ShowErrorAlertAsync(response.Message);
					return;
				}

				await ShowToastAsync("Photo added successfully");
				Photos.Add(response.Data);
				var photo = Photos.FirstOrDefault(n => n.Id == 0);
				if (photo is { })
				{
					Photos.Remove(photo);
				}
				DeletePhotoCommand.NotifyCanExecuteChanged();
				SetMainPhotoCommand.NotifyCanExecuteChanged();
			});
		}

		[RelayCommand(CanExecute = nameof(CanExecuteSetMainPhoto))]
		private async Task SetMainPhotoAsync()
		{
			await MakeApiRequestAsync(async () =>
			{
				var response = await photoApi.SetMainPhotoAsync(Photos[SelectedIndex].Id);

				if (!response.IsSuccess)
				{
					await ShowErrorAlertAsync(response.Message);
					return;
				}

				await ShowToastAsync("Photo set main seccessfully");
				SetMainPhotoCommand.NotifyCanExecuteChanged();
			});
		}

		private bool CanExecuteSetMainPhoto()
		{
			return Photos.Count > 1 && Photos[SelectedIndex].IsMain == false;
		}

		[RelayCommand(CanExecute = nameof(CanExecuteDeletePhoto))]
		private async Task DeletePhotoAsync()
		{
			await MakeApiRequestAsync(async () =>
			{
				var response = await photoApi.DeleteUserPhotoAsync(Photos[SelectedIndex].Id);

				if (!response.IsSuccess)
				{
					await ShowErrorAlertAsync(response.Message);
					return;
				}

				await ShowToastAsync("Photo deleted seccessfully");
				Photos.Remove(Photos[SelectedIndex]);
				if (Photos.Count == 0)
				{
					Photos.Add(new PhotoDto(0, "user.png", true));
				}
				DeletePhotoCommand.NotifyCanExecuteChanged();
				SetMainPhotoCommand.NotifyCanExecuteChanged();
			});
		}

		private bool CanExecuteDeletePhoto()
		{
			return SelectedIndex >= 0
				&& SelectedIndex < Photos.Count
				&& Photos[SelectedIndex].Id != 0;
		}

		public void ConfigureRealtimeUpdates()
		{
			realtimeUpdateService.AddUserCreatedOrUpdatedHandler(nameof(ProfileViewModel), OnUserCreatedOrUpdated);
		}

		private void OnUserCreatedOrUpdated(LoggedInUserDto dto)
		{
			if (dto.Id == AuthService.User!.Id)
			{
				User = dto;
			}
		}
	}
}
