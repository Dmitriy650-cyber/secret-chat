namespace SecretChat.Shared.DTOs.UserDTOs
{
	public record RegisterUserResponseDto
		(
			LoggedInUserDto User,
			string Token
		);
}
