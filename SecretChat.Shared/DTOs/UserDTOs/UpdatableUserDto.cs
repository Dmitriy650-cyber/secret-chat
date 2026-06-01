namespace SecretChat.Shared.DTOs.UserDTOs
{
	public record UpdatableUserDto
		(
			[Required, MinLength(5), MaxLength(20)] string Name,
			[Phone] string? TelephoneNumber,
			[MaxLength(500)] string? AboutMyself
		);
}
