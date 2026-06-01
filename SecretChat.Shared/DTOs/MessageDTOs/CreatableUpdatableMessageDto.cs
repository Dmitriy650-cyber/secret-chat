namespace SecretChat.Shared.DTOs.MessageDTOs
{
	public record CreatableUpdatableMessageDto
		(
			[Required, Range(1, int.MaxValue)] int ChatId,
			[MaxLength(500)] string? Content
		);
}
