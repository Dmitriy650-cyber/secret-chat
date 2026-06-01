namespace SecretChat.Shared.DTOs.ChatDTOs
{
	public record ChatDto
		(
			int Id,
			DateTime CreatedAt,
			LoggedInUserDto User,
			LoggedInUserDto WithUser,
			bool IsFavorite
		)
	{
		public MessageDto? LastMessage { get; set; }
	}
}
