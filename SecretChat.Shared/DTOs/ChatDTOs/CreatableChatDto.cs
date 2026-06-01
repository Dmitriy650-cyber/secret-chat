namespace SecretChat.Shared.DTOs.ChatDTOs
{
	public record CreatableChatDto
		(
			[Required, Range(1, int.MaxValue)] int FirtUserId,
			[Required, Range(1, int.MaxValue)] int SecondUserId
		);
}
