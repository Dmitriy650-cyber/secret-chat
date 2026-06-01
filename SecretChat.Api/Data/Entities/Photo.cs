namespace SecretChat.Api.Data.Entities
{
	[Table("Photos")]
	public class Photo
	{
		public int Id { get; set; }

		public string PhotoPath { get; set; } = null!;
		public string PhotoUrl { get; set; } = null!;
		public bool IsMain { get; set; }

		public User? User { get; set; }
		public int UserId { get; set; }
	}
}
