namespace SecretChat.Mobile.ViewModels
{
	public partial class EditViewModel(IConnectivity connectivity, AuthService authService, IUserApi userApi) : BaseViewModel(connectivity, authService)
	{
		[ObservableProperty]
		private string? _name = authService.User!.Name;
		[ObservableProperty]
		private string? _telephoneNumber = authService.User!.TelephoneNumber;
		[ObservableProperty]
		private string? _aboutMyself = authService.User!.AboutMyself;
		[ObservableProperty]
		private string? _oldPassword;
		[ObservableProperty]
		private string? _newPassword;
		[ObservableProperty]
		private string? _confirmedNewPassword;

		public override async Task InitializeViewModel()
		{
			IsLoadedPage = true;
		}

		[RelayCommand]
		private async Task UpdateUserDataAsync()
		{
			if (string.IsNullOrWhiteSpace(Name) || Name.Length < 5 || Name.Length > 20)
			{
				await ShowErrorAlertAsync("The name must be longer than 5 and shorter than 20 characters");
				return;
			}

			await MakeApiRequestAsync(async () =>
			{
				var response = await userApi.UpdateUserAsync(new UpdatableUserDto(Name, TelephoneNumber, AboutMyself));

				if (!response.IsSuccess)
				{
					await ShowErrorAlertAsync(response.Message);
					return;
				}

				AuthService.Login(response.Data);
				await ShowToastAsync("Data changed successfully");
			});
		}

		[RelayCommand]
		private async Task ChangePasswordAsync()
		{
			if(string.IsNullOrWhiteSpace(OldPassword) || 
			   string.IsNullOrWhiteSpace(NewPassword) || 
			   string.IsNullOrWhiteSpace(ConfirmedNewPassword))
			{
				await ShowErrorAlertAsync("All fields are requared");
				return;
			}

			await MakeApiRequestAsync(async () =>
			{
				var response = await userApi.ChangePasswordAsync(
					new ChangePasswordDto(OldPassword, NewPassword) { ConfirmedNewPassword = ConfirmedNewPassword});

				if (!response.IsSuccess)
				{
					await ShowErrorAlertAsync(response.Message);
					return;
				}

				OldPassword = string.Empty;
				NewPassword = string.Empty;
				ConfirmedNewPassword = string.Empty;

				await ShowToastAsync("Password changed successfully");
			});
		}
	}
}
