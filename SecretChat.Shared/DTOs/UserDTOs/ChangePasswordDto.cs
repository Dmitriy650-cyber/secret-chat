namespace SecretChat.Shared.DTOs.UserDTOs
{
	public record ChangePasswordDto
		(
			[Required, MinLength(5), MaxLength(30), PasswordComplexity] string OldPassword,
			[Required, MinLength(5), MaxLength(30), PasswordComplexity] string NewPassword
		)
	{
		[JsonIgnore, Compare(nameof(NewPassword)), Required]
		public string ConfirmedNewPassword { get; set; } = null!;
	}
}
