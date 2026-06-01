namespace SecretChat.Mobile.Services.CryptographicServices
{
	public static class KeyGenerator
	{
		public static (byte[] key, byte[] salt) CreateChatKey()
		{
			var key = new byte[32];
			RandomNumberGenerator.Fill(key);

			var salt = new byte[32];
			RandomNumberGenerator.Fill(salt);

			return (key, salt);
		}
	}
}
