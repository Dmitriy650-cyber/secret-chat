namespace SecretChat.Mobile.Services.CryptographicServices
{
	public static class AesHelper
	{
		/// <summary>
		/// Шифрует текст с помощью AES‑256 и случайного IV.
		/// IV вшит в результат в виде [IV][ciphertext].
		/// </summary>
		/// <param name="plainText">Открытый текст</param>
		/// <param name="key">32‑байтовый ключ (AES‑256)</param>
		/// <returns>base64‑строка IV + шифротекста</returns>
		public static string Encrypt(string plainText, byte[] key)
		{
			using var aes = Aes.Create();
			aes.Key = key;
			aes.GenerateIV();

			using var encryptor = aes.CreateEncryptor();
			using var ms = new MemoryStream();
			using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
			using var sw = new StreamWriter(cs, Encoding.UTF8);

			sw.Write(plainText);
			sw.Close();

			var result = ms.ToArray();
			var ivAndCiphertext = new byte[aes.IV.Length + result.Length];
			Buffer.BlockCopy(aes.IV, 0, ivAndCiphertext, 0, aes.IV.Length);
			Buffer.BlockCopy(result, 0, ivAndCiphertext, aes.IV.Length, result.Length);

			return Convert.ToBase64String(ivAndCiphertext);
		}

		/// <summary>
		/// Расшифровывает текст, зашифрованный Encrypt.
		/// </summary>
		/// <param name="cipherText">base64‑строка IV + шифротекста</param>
		/// <param name="key">32‑байтовый ключ (AES‑256)</param>
		/// <returns>Открытый текст</returns>
		public static string Decrypt(string cipherText, byte[] key)
		{
			var data = Convert.FromBase64String(cipherText);

			using var aes = Aes.Create();
			aes.Key = key;

			var iv = new byte[aes.IV.Length];
			var ciphertext = new byte[data.Length - iv.Length];

			Buffer.BlockCopy(data, 0, iv, 0, iv.Length);
			Buffer.BlockCopy(data, iv.Length, ciphertext, 0, ciphertext.Length);

			aes.IV = iv;

			using var decryptor = aes.CreateDecryptor();
			using var ms = new MemoryStream(ciphertext);
			using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
			using var sr = new StreamReader(cs, Encoding.UTF8);

			return sr.ReadToEnd();
		}
	}
}
