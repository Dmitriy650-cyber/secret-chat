namespace SecretChat.Mobile.Services
{
	public class AuthService
	{
		public const string UserKey = "user-key";

		public LoggedInUserDto? User;
		public string? Token;

		public bool IsLoggedIn => User is not null && !string.IsNullOrWhiteSpace(Token);

		public AuthService()
		{
			Initialize();
		}

		public void Login(RegisterUserResponseDto dto)
		{
			(User, Token) = dto;

			Preferences.Default.Set(UserKey, JsonSerializer.Serialize(dto));
		}

		public void Logout()
		{
			(User, Token) = (null, null);

			Preferences.Default.Remove(UserKey);
		}

		private void Initialize()
		{
			var data = Preferences.Default.Get<string?>(UserKey, null);

			if (!string.IsNullOrWhiteSpace(data))
			{
				var dto = JsonSerializer.Deserialize<RegisterUserResponseDto>(data);

				if(dto is not null && dto.User is not null && dto.Token is not null)
				{
					(User, Token) = dto;
				}
				else
				{
					Preferences.Default.Remove(UserKey);
				}
			}
		}
	}
}
