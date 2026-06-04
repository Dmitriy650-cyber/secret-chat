namespace SecretChat.Api.Services.Interfaces
{
	public interface IChatService
	{
		Task<ApiResult<ChatDto>> CreateChatAsync(CreatableChatDto dto);
		Task<ApiResult> MakeMessagesReadedAsync(int chatId, int userId);
		Task<ApiResult> DeleteChatAsync(int chatId);
		Task<ApiResult<IEnumerable<ChatDto>>> GetUserChatsAsync(int userId);
		Task<ApiResult<ChatDto>> GetChatByUserIdAsync(int id, int userId);
		Task<ApiResult> MakeChatFavoriteAsync(int chatId, int userId);
		Task<ApiResult> RemoveChatFromFavoritesAsync(int chatId, int userId);
	}
}