namespace SecretChat.Api.Endpoints
{
	public static class MessageEndpoints
	{
		public static IEndpointRouteBuilder MapMessageEndpoints(this IEndpointRouteBuilder builder)
		{
			var messageGroup = builder
				.MapGroup("/api/message")
				.RequireAuthorization()
				.WithTags("Message");

			messageGroup.MapGet("/{chatId:int}", async (int chatId, IMessageService service) =>
			{
				return await service.GetChatMessagesAsync(chatId);
			})
				.Produces<ApiResult<IEnumerable<MessageDto>>>()
				.WithName("GetChatMessages");

			messageGroup.MapPost("/", async (
				[FromForm] IFormFile? file, 
				[FromForm] string? serializedCreatableUpdatableMessageDto, 
				IMessageService service, 
				ClaimsPrincipal principal, 
				ValidationService validation) =>
			{
				CreatableUpdatableMessageDto? dto = new(0, null);

				if (serializedCreatableUpdatableMessageDto is not null)
				{
					dto = JsonSerializer.Deserialize<CreatableUpdatableMessageDto>(serializedCreatableUpdatableMessageDto);
					if (dto is null || !validation.IsValid(dto))
						return ApiResult<MessageDto>.Fail("Invalid data");
				}

				return await service.CreateMessageAsync(dto, file, principal.GetUserId());
			})
				.Produces<ApiResult<MessageDto>>()
				.WithName("CreateMessage")
				.DisableAntiforgery();

			messageGroup.MapPut("/{messageId:int}", async (
				[FromForm] IFormFile? file, 
				[FromForm] string serializedCreatableUpdatableMessageDto, 
				int messageId,
				IMessageService service, 
				ValidationService validation) =>
			{
				var dto = JsonSerializer.Deserialize<CreatableUpdatableMessageDto>(serializedCreatableUpdatableMessageDto);
				if (dto is null || !validation.IsValid(dto))
					return ApiResult<MessageDto>.Fail("Invalid data");

				return await service.UpdateMessageAsync(dto, file, messageId);
			})
				.Produces<ApiResult<MessageDto>>()
				.WithName("UpdateMessage");

			messageGroup.MapDelete("/{id:int}", async (int id, IMessageService service) =>
			{
				return await service.DeleteMessageAsync(id);
			})
				.Produces<ApiResult>()
				.WithName("DeleteMessage");

			return builder;
		}
	}
}
