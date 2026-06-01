namespace SecretChat.Api.Services.Interfaces
{
	public interface IMessageService
	{
		Task<ApiResult<MessageDto>> CreateMessageAsync(CreatableUpdatableMessageDto dto, IFormFile? file, int userId);
		Task<ApiResult> DeleteMessageAsync(int id);
		Task<ApiResult<IEnumerable<MessageDto>>> GetChatMessagesAsync(int chatId);
		Task<ApiResult<MessageDto>> UpdateMessageAsync(CreatableUpdatableMessageDto dto, IFormFile? file, int messageId);
	}
}