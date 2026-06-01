namespace SecretChat.Mobile.Services.Apis
{
	[Headers("Authorization: Bearer")]
	public interface IChatApi
	{
		[Post("/api/chat")]
		Task<ApiResult<ChatDto>> CreateChatAsync(CreatableChatDto dto);

		[Delete("/api/chat/{chatId}")]
		Task<ApiResult> DeleteChatAsync(int chatId);

		[Get("/api/chat")]
		Task<ApiResult<IEnumerable<ChatDto>>> GetUserChatsAsync();

		[Get("/api/chat/{id}")]
		Task<ApiResult<ChatDto>> GetChatByUserIdAsync(int id);

		[Put("/api/chat/make-favorite-chat/{chatId}")]
		Task<ApiResult> MakeChatFavoriteAsync(int chatId);

		[Put("/api/chat/remove-chat-from-favorites/{chatId}")]
		Task<ApiResult> RemoveChatFromFavoritesAsync(int chatId);
	}
}
