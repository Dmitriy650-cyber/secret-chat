namespace SecretChat.Api.Data.Entities
{
	[Table("Chats")]
	public class Chat
	{
		public int Id { get; set; }
		public int? LastMessageId { get; set; }
		public DateTime CreatedAt { get; set; }

		public User? FirstUser { get; set; }
		public int? FirstUserId { get; set; }
		public bool IsFavoriteForFirstUser { get; set; }

		public User? SecondUser { get; set; }
		public int? SecondUserId { get; set; }
		public bool IsFavoriteForSecondUser { get; set; }

		public List<Message> Messages { get; set; } = [];
	}
}
