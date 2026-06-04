namespace SecretChat.Shared.DTOs.MessageDTOs
{
	public record MessageDto
		(
			int Id,
			string? Content,
			string? FileUrl,
			DateTime SendOn,
			DateTime? ModifiedOn,
			int UserId,
			int ChatId,
			bool WasReaded
		);
}
