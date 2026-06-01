namespace SecretChat.Shared.DTOs.UserDTOs
{
	public record LoggedInUserDto
		(
			int Id,
			string Name,
			string Email,
			string? TelephoneNumber,
			string? AboutMyself
		)
	{
		public string? PhotoUrl { get; set; }

		[JsonIgnore]
		public string UserPhoto => PhotoUrl ?? "user.png";

		[JsonIgnore]
		public string Phone => TelephoneNumber ?? "No telephone number";
	}
}
