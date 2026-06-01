namespace SecretChat.Api.Data.Entities
{
	[Table("Users")]
	public class User
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string HashPassword { get; set; } = null!;
		public string? TelephoneNumber { get; set; }
		public string? AboutMyself { get; set; }

		public List<Chat> Chats { get; set; } = [];
		public List<Message> Messages { get; set; } = [];
		public List<UserContact> Contacts { get; set; } = [];
		public List<Photo> Photos { get; set; } = [];
	}
}
