namespace SecretChat.Shared.DTOs.PhotoDTOs
{
	public record PhotoDto
		(
			int Id,
			string PhotoUrl,
			bool IsMain
		);
}
