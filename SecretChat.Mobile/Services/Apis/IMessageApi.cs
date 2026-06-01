namespace SecretChat.Mobile.Services.Apis
{
	[Headers("Authorization: Bearer")]
	public interface IMessageApi
	{
		[Multipart]
		[Post("/api/message")]
		Task<ApiResult<MessageDto>> CreateMessageAsync(StreamPart file, string serializedCreatableUpdatableMessageDto);

		[Multipart]
		[Post("/api/message")]
		Task<ApiResult<MessageDto>> CreateMessageWithoutFileAsync([AliasAs("serializedCreatableUpdatableMessageDto")] string serializedCreatableUpdatableMessageDto);

		[Delete("/api/message/{id}")]
		Task<ApiResult> DeleteMessageAsync(int id);

		[Get("/api/message/{chatId}")]
		Task<ApiResult<IEnumerable<MessageDto>>> GetChatMessagesAsync(int chatId);

		[Multipart]
		[Put("/api/message/{messageId}")]
		Task<ApiResult<MessageDto>> UpdateMessageAsync(StreamPart? file, string serializedCreatableUpdatableMessageDto, int messageId);
	}
}
