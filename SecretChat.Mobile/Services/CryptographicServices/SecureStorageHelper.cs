namespace SecretChat.Mobile.Services.CryptographicServices
{
	public static class SecureStorageHelper
	{
		private const string KeyPrefix = "CHAT_KEY_";

		public static async Task<bool> SaveChatKeyAsync(int chatId, byte[] key, byte[] salt)
		{
			try
			{
				await SecureStorage.Default.SetAsync($"{KeyPrefix}{chatId}_KEY", Convert.ToBase64String(key));
				await SecureStorage.Default.SetAsync($"{KeyPrefix}{chatId}_SALT", Convert.ToBase64String(salt));
				return true;
			}
			catch { return false; }
		}

		public static async Task<(byte[] key, byte[] salt)?> LoadChatKeyAsync(int chatId)
		{
			try
			{
				var keyStr = await SecureStorage.Default.GetAsync($"{KeyPrefix}{chatId}_KEY");
				var saltStr = await SecureStorage.Default.GetAsync($"{KeyPrefix}{chatId}_SALT");

				if (string.IsNullOrEmpty(keyStr) || string.IsNullOrEmpty(saltStr))
					return null;

				var key = Convert.FromBase64String(keyStr);
				var salt = Convert.FromBase64String(saltStr);

				if (key.Length != 32)
					return null;

				return (key, salt);
			}
			catch { return null; }
		}

		public static async Task<bool> DeleteChatKeyAsync(int chatId)
		{
			try
			{
				SecureStorage.Default.Remove($"{KeyPrefix}{chatId}_KEY");
				SecureStorage.Default.Remove($"{KeyPrefix}{chatId}_SALT");
				return true;
			}
			catch { return false; }
		}
	}
}
