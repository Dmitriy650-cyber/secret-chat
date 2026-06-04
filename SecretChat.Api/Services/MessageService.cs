namespace SecretChat.Api.Services
{
	public class MessageService(DataContext context, FileUploadingService fileUploadingService, IMapper mapper, IHubContext<SecretChatHub, ISecretChatHubClient> hubContext) : IMessageService
	{
		public async Task<ApiResult<IEnumerable<MessageDto>>> GetChatMessagesAsync(int chatId, int userId)
		{
			try
			{
				var messages = await context.Messages.Where(n => n.ChatId == chatId).ToArrayAsync();

				foreach(var message in messages.Where(n => n.WasReaded == false && n.UserId != userId))
				{
					message.WasReaded = true;
				}

				await context.SaveChangesAsync();

				return ApiResult<IEnumerable<MessageDto>>.Success(mapper.Map<IEnumerable<MessageDto>>(messages));
			}
			catch (Exception ex)
			{
				return ApiResult<IEnumerable<MessageDto>>.Fail(ex.Message);
			}
		}

		public async Task<ApiResult<MessageDto>> CreateMessageAsync(CreatableUpdatableMessageDto? dto, IFormFile? file, int userId)
		{
			try
			{
				Message message = new();

				if (dto is { })
				{
					message = mapper.Map<Message>(dto);
				}
				
				message.UserId = userId;

				if (file is { })
					(message.FilePath, message.FileUrl) = await fileUploadingService.SaveFileAsync(file, "uploads", "files", "messages");

				var newMessage = context.Messages.Add(message).Entity;
				await context.SaveChangesAsync();

				var chat = await context.Chats.FindAsync(message.ChatId);
				if (chat is null)
					return ApiResult<MessageDto>.Fail("Chat not found");
				chat.LastMessageId = newMessage.Id;

				await context.SaveChangesAsync();

				var messageDto = mapper.Map<MessageDto>(newMessage);

				await hubContext.Clients.All.MessageSentAsync(messageDto);

				return ApiResult<MessageDto>.Success(messageDto);
			}
			catch (Exception ex)
			{
				return ApiResult<MessageDto>.Fail(ex.Message);
			}
		}

		public async Task<ApiResult<MessageDto>> UpdateMessageAsync(CreatableUpdatableMessageDto dto, IFormFile? file, int messageId)
		{
			try
			{
				var message = await context.Messages.FindAsync(messageId);
				if (message is null)
					return ApiResult<MessageDto>.Fail("Message not found");

				message.Content = dto.Content;

				if (file is { })
				{
					if (!string.IsNullOrEmpty(message.FilePath) && File.Exists(message.FilePath))
						File.Delete(message.FilePath);

					(message.FilePath, message.FileUrl) = await fileUploadingService.SaveFileAsync(file, "uploads", "files", "messages");
				}

				await context.SaveChangesAsync();

				var messageDto = mapper.Map<MessageDto>(message);

				await hubContext.Clients.All.MessageSentAsync(messageDto);

				return ApiResult<MessageDto>.Success(messageDto);
			}
			catch (Exception ex)
			{
				return ApiResult<MessageDto>.Fail(ex.Message);
			}
		}

		public async Task<ApiResult> DeleteMessageAsync(int id)
		{
			try
			{
				var message = await context.Messages.FindAsync(id);
				if (message is null)
					return ApiResult.Fail("Message not found");

				context.Messages.Remove(message);
				await context.SaveChangesAsync();

				if (!string.IsNullOrEmpty(message.FilePath) && File.Exists(message.FilePath))
					File.Delete(message.FilePath);

				await hubContext.Clients.All.MessageDeletedAsync(id);

				return ApiResult.Success();
			}
			catch (Exception ex)
			{
				return ApiResult.Fail(ex.Message);
			}
		}
	}
}
