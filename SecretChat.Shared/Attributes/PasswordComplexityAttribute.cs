namespace SecretChat.Shared.Attributes
{
	public partial class PasswordComplexityAttribute : ValidationAttribute
	{
		public override bool IsValid(object? value)
		{
			var password = value as string;

			if (string.IsNullOrEmpty(password))
				return false;

			var hasLowerChar = HasLowerChar().IsMatch(password);
			var hasUpperChar = HasUpperChar().IsMatch(password);
			var hasDigit = HasDigit().IsMatch(password);
			var hasSpecialChar = HasSpecialChar().IsMatch(password);

			return hasLowerChar && hasUpperChar && hasDigit && hasSpecialChar;
		}

		public override string FormatErrorMessage(string name)
		{
			return $"{name} must contain lowercase and uppercase letters, numbers, and special characters.";
		}

		[GeneratedRegex("[a-z]")]
		private static partial Regex HasLowerChar();

		[GeneratedRegex("[A-Z]")]
		private static partial Regex HasUpperChar();

		[GeneratedRegex("[0-9]")]
		private static partial Regex HasDigit();

		[GeneratedRegex("[^a-zA-Z0-9]")]
		private static partial Regex HasSpecialChar();
	}
}
