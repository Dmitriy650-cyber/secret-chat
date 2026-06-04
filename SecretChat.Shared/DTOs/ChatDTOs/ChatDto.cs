namespace SecretChat.Shared.DTOs.ChatDTOs
{
	public class ChatDto
	{
		public int Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public LoggedInUserDto User { get; set; } = null!;
		public LoggedInUserDto WithUser { get; set; } = null!;
		public bool IsFavorite { get; set; }
		public MessageDto? LastMessage { get; set; }
		public int CountMessagesThatHaveNotBeenRead { get; set; }

		public ChatDto(int id, DateTime createdAt, LoggedInUserDto user, LoggedInUserDto withUser, bool isFavorite)
		{
			Id = id;
			CreatedAt = createdAt;
			User = user;
			WithUser = withUser;
			IsFavorite = isFavorite;
		}

		public ChatDto() { }
	}
}
