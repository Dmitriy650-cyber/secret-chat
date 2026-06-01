namespace SecretChat.Shared.DTOs.UserDTOs
{
	public record LoginUserDto
		(
			[Required, EmailAddress] string Email,
			[Required, MinLength(5), MaxLength(30), PasswordComplexity] string Password
		);
}
