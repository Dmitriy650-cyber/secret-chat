namespace SecretChat.Api.Data.Entities
{
	[Table("Messages")]
	public class Message
	{
		public int Id { get; set; }

		public string? Content { get; set; }
		public string? FilePath { get; set; }
		public string? FileUrl { get; set; }
		public DateTime SendOn { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public bool WasReaded { get; set; }

		public User? User { get; set; }
		public int? UserId { get; set; }

		public Chat? Chat { get; set; }
		public int? ChatId { get; set; }
	}
}
