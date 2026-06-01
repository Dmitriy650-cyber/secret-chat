namespace SecretChat.Shared.DTOs.UserDTOs
{
	public record RegisterUserDto
		(
			[Required, MinLength(5), MaxLength(20)] string Name,
			[Required, EmailAddress] string Email,
			[Required, MinLength(5), MaxLength(30), PasswordComplexity] string Password,
			[Phone] string? TelephoneNumber,
			[MaxLength(500)] string? AboutMyself
		);
}
