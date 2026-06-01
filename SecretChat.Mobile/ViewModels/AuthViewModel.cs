namespace SecretChat.Mobile.ViewModels
{
	public partial class AuthViewModel(IConnectivity connectivity, IAuthApi authApi, AuthService authService) : BaseViewModel(connectivity, authService)
	{
		[ObservableProperty]
		private bool _isLoginMode = true;
		[ObservableProperty]
		private string? _name;
		[ObservableProperty]
		private string? _password;
		[ObservableProperty]
		private string? _email;
		[ObservableProperty]
		private string? _telephoneNumber;

		[RelayCommand]
		private void ChangeMode()
		{
			IsLoginMode = !IsLoginMode;
		}

		[RelayCommand]
		private async Task LoginRegisterAsync()
		{
			await MakeApiRequestAsync(async () =>
			{
				if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Email))
				{
					await ShowErrorAlertAsync("Email and password are requared");
					return;
				}

				if (IsLoginMode)
				{
					var response = await authApi.LoginUserAsync(new LoginUserDto(Email, Password));

					if (!response.IsSuccess)
					{
						await ShowErrorAlertAsync(response.Message);
						return;
					}

					await ShowToastAsync("User logged successfully");

					AuthService.Login(response.Data);

					await NavigateAsync($"//{nameof(HomePage)}");
				}
				else
				{
					if (string.IsNullOrWhiteSpace(Name))
					{
						await ShowErrorAlertAsync("Name is requared");
						return;
					}

					var response = await authApi.RegisterUserAsync(new RegisterUserDto(Name, Email, Password, TelephoneNumber, null));

					if (!response.IsSuccess)
					{
						await ShowErrorAlertAsync(response.Message);
						return;
					}

					await ShowToastAsync("User registered successfully");

					ChangeMode();
				}
			});
		}
	}
}
