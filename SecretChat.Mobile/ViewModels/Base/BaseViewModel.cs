namespace SecretChat.Mobile.ViewModels.Base
{
	public partial class BaseViewModel(IConnectivity connectivity, AuthService authService) : ObservableObject
	{
		[ObservableProperty]
		private bool _isBusy = false;
		[ObservableProperty]
		private bool _isLoadedPage = false;
		[ObservableProperty]
		private bool _isErrorState = false;
		[ObservableProperty]
		private bool _isShowContent = false;
		[ObservableProperty]
		private string _loadingText = string.Empty;
		[ObservableProperty]
		private string _errorText = string.Empty;
		[ObservableProperty]
		private string _errorImage = string.Empty;
		[ObservableProperty]
		private bool _isShowMenu = false;

		protected AuthService AuthService = authService;

		protected async Task ShowErrorAlertAsync(string message) =>
			await Shell.Current.DisplayAlertAsync("Error", message, "Ok");
		protected async Task<bool> ShowDialogAlertAsync(string title, string question) =>
			await Shell.Current.DisplayAlertAsync(title, question, "Yes", "No");
		protected async Task ShowToastAsync(string message) =>
			await Toast.Make(message).Show();
		protected async Task NavigateAsync(string url) =>
			await Shell.Current.GoToAsync(url, animate: true);
		protected async Task NavigateAsync(string url, Dictionary<string, object> parameters) =>
			await Shell.Current.GoToAsync(url, animate: true, parameters);
		protected async Task NavigateBackAsync() =>
			await NavigateAsync("..");

		partial void OnIsLoadedPageChanged(bool oldValue, bool newValue)
		{
			IsShowContent = IsLoadedPage && !IsErrorState;
		}

		partial void OnIsErrorStateChanged(bool oldValue, bool newValue)
		{
			IsShowContent = IsLoadedPage && !IsErrorState;
		}

		protected event EventHandler GoBackAction = null!;

		[RelayCommand]
		private async Task GoBackAsync()
		{
			GoBackAction?.Invoke(this, EventArgs.Empty);
			IsShowMenu = false;

			await NavigateBackAsync();
		}

		[RelayCommand]
		private async Task GoToSettingsPageAsync()
		{
			IsShowMenu = false;

			await NavigateAsync(nameof(SettingsPage));
		}

		[RelayCommand]
		private async Task GoToHelpPageAsync()
		{
			IsShowMenu = false;

			await NavigateAsync(nameof(HelpPage));
		}

		[RelayCommand]
		private async Task ShowMenuAsync()
		{
			IsShowMenu = true;
		}

		[RelayCommand]
		private async Task CloseMenuAsync()
		{
			IsShowMenu = false;
		}

		[RelayCommand]
		private async Task LogoutAsync()
		{
			IsShowMenu = false;

			AuthService.Logout();

			await NavigateAsync(nameof(AuthPage));
		}

		public virtual async Task InitializeViewModel()
		{
			IsLoadedPage = true;

			await Task.CompletedTask;
		}

		protected async Task MakeApiRequestAsync(Func<Task> request)
		{
			IsBusy = true;
			LoadingText = "";

			try
			{
				if (connectivity.NetworkAccess != NetworkAccess.Internet)
					throw new InternetConnectionException("No internet connection");

				await request.Invoke();
			}
			catch (InternetConnectionException ex)
			{
				await ShowErrorAlertAsync(ex.Message);
			}
			catch (ApiException ex)
			{
				await ShowErrorAlertAsync(ex.Content ?? "Server is unavailable");
			}
			catch (Exception ex)
			{
				await ShowErrorAlertAsync(ex.Message);
			}
			finally
			{
				IsBusy = false;
			}
		}

		protected async Task MakeFirstApiRequestAsync(Func<Task> request, string loadingText = "Loading content...")
		{
			IsBusy = true;
			LoadingText = loadingText;

			try
			{
				if (connectivity.NetworkAccess != NetworkAccess.Internet)
					throw new InternetConnectionException("No internet connection");

				await request.Invoke();
			}
			catch (InternetConnectionException ex)
			{
				SetErrorState(ex.Message, "nointernet.png");
			}
			catch (ApiException)
			{
				SetErrorState("Server is unavailable", "nointernet2.png");
			}
			catch (Exception)
			{
				SetErrorState("An unexpected error has occurred", "error.png");
			}
			finally
			{
				IsBusy = false;
				IsLoadedPage = true;
			}
		}

		protected void SetErrorState(string message, string imageUrl)
		{
			ErrorText = message;
			ErrorImage = imageUrl;
			IsErrorState = true;
		}

		protected async Task<string?> ChoosePhotoAsync()
		{
			try
			{
				if (MediaPicker.Default.IsCaptureSupported)
				{
					const string PickFromDevice = "Pick from Device";
					const string CapturePhoto = "Capture Photo";

					var result = await Shell.Current
						.DisplayActionSheetAsync("Choose photo", "Cancel", null, PickFromDevice, CapturePhoto);
					if (string.IsNullOrWhiteSpace(result))
						return null;

					switch (result)
					{
						case PickFromDevice:
							return await PickFromDeviceAsync();
						case CapturePhoto:
							return await CapturePhotoAsync();
					}

					async Task<string?> PickFromDeviceAsync()
					{
						var photoResults = await MediaPicker.Default.PickPhotosAsync(new MediaPickerOptions
						{
							Title = "Select Photo"
						});
						if (photoResults?.FirstOrDefault() is not FileResult fileResult)
						{
							await ShowToastAsync("No photo selected");
							return null;
						}
						return fileResult.FullPath;
					}
					async Task<string?> CapturePhotoAsync()
					{
						FileResult? fileResult = await MediaPicker.Default.CapturePhotoAsync(new MediaPickerOptions
						{
							Title = "Take Photo"
						});
						if (fileResult is null)
						{
							await ShowToastAsync("No photo captured");
							return null;
						}
						return fileResult.FullPath;
					}
				}

				await ShowToastAsync("Capture is not supported");

				return null;
			}
			catch (Exception e)
			{
				await ShowErrorAlertAsync(e.Message);

				return null;
			}
		}
	}
}
