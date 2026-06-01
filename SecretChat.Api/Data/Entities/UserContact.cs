namespace SecretChat.Api.Data.Entities
{
	[Table("Contacts")]
	public class UserContact
	{
		public User? User { get; set; }
		public int? UserId { get; set; }

		public User? Contact { get; set; }
		public int? ContactId { get; set; }
	}
}
