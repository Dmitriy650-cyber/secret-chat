namespace SecretChat.Api.Endpoints
{
	public static class ChatEndpoints
	{
		public static IEndpointRouteBuilder MapChatEndpoints(this IEndpointRouteBuilder builder)
		{
			var chatGroup = builder
				.MapGroup("/api/chat")
				.RequireAuthorization()
				.WithTags("Chat");

			chatGroup.MapGet("/", async (IChatService service, ClaimsPrincipal principal) =>
			{
				return await service.GetUserChatsAsync(principal.GetUserId());
			})
				.Produces<ApiResult<IEnumerable<ChatDto>>>()
				.WithName("GetUserChats");

			chatGroup.MapGet("/{id:int}", async (int id, IChatService service, ClaimsPrincipal principal) =>
			{
				return await service.GetChatByUserIdAsync(id, principal.GetUserId());
			})
				.Produces<ApiResult<ChatDto>>()
				.WithName("GetChatByUserId");

			chatGroup.MapPost("/", async ([FromBody] CreatableChatDto dto, IChatService service) =>
			{
				return await service.CreateChatAsync(dto);
			})
				.Produces<ApiResult<ChatDto>>()
				.WithName("CreateChat");

			chatGroup.MapDelete("/{chatId:int}", async (int chatId, IChatService service) =>
			{
				return await service.DeleteChatAsync(chatId);
			})
				.Produces<ApiResult>()
				.WithName("DeleteChat");

			chatGroup.MapPut("/make-favorite-chat/{chatId:int}", async (int chatId, IChatService service, ClaimsPrincipal principal) =>
			{
				return await service.MakeChatFavoriteAsync(chatId, principal.GetUserId());
			})
				.Produces<ApiResult>()
				.WithName("MakeChatFavorite");

			chatGroup.MapPut("/remove-chat-from-favorites/{chatId:int}", async (int chatId, IChatService service, ClaimsPrincipal principal) =>
			{
				return await service.RemoveChatFromFavoritesAsync(chatId, principal.GetUserId());
			})
				.Produces<ApiResult>()
				.WithName("RemoveChatFromFavorites");

			return builder;
		}
	}
}
