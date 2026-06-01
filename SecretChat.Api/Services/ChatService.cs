namespace SecretChat.Api.Services
{
	public class ChatService(DataContext context, IMapper mapper, IHubContext<SecretChatHub, ISecretChatHubClient> hubContext) : IChatService
	{
		public async Task<ApiResult<IEnumerable<ChatDto>>> GetUserChatsAsync(int userId)
		{
			try
			{
				var chats = await context.Chats
					.Where(n => n.FirstUserId == userId || n.SecondUserId == userId)
					.Include(n => n.FirstUser)
					.Include(n => n.SecondUser)
					.ToListAsync();

				List<ChatDto> chatsDtos = [];

				foreach (var chat in chats)
				{
					var withUser = chat.FirstUserId == userId
						? mapper.Map<LoggedInUserDto>(chat.SecondUser)
						: mapper.Map<LoggedInUserDto>(chat.FirstUser);
					var user = chat.FirstUserId == userId
						? mapper.Map<LoggedInUserDto>(chat.FirstUser)
						: mapper.Map<LoggedInUserDto>(chat.SecondUser);
					var isFavorite = chat.FirstUserId == userId
						? chat.IsFavoriteForFirstUser
						: chat.IsFavoriteForSecondUser;
					withUser.PhotoUrl = context.Photos
						.AsNoTracking()
						.FirstOrDefault(n => n.UserId == withUser.Id && n.IsMain)?.PhotoUrl;
					user.PhotoUrl = context.Photos
						.AsNoTracking()
						.FirstOrDefault(n => n.UserId == user.Id && n.IsMain)?.PhotoUrl;
					var lastMessage = chat.LastMessageId is { } ? await context.Messages
						.AsNoTracking()
						.FirstOrDefaultAsync(n => n.Id == chat.LastMessageId) : null;
					var lastMessageDto = mapper.Map<MessageDto>(lastMessage);
					var chatDto = new ChatDto(chat.Id, chat.CreatedAt, user, withUser, isFavorite) { LastMessage = lastMessageDto };
					chatsDtos.Add(chatDto);
				}

				return ApiResult<IEnumerable<ChatDto>>.Success(chatsDtos);
			}
			catch (Exception ex)
			{
				return ApiResult<IEnumerable<ChatDto>>.Fail(ex.Message);
			}
		}

		public async Task<ApiResult<ChatDto>> GetChatByUserIdAsync(int id, int userId)
		{
			try
			{
				var chat = await context.Chats
					.AsNoTracking()
					.Include(n => n.FirstUser)
					.Include(n => n.SecondUser)
					.FirstOrDefaultAsync(n => 
					(n.FirstUserId == id && n.SecondUserId == userId) || 
					(n.SecondUserId == id && n.FirstUserId == userId));

				if (chat is null)
					return ApiResult<ChatDto>.Fail("Chat not found");

				var withUser = chat.FirstUserId == userId
						? mapper.Map<LoggedInUserDto>(chat.SecondUser)
						: mapper.Map<LoggedInUserDto>(chat.FirstUser);
				var user = chat.FirstUserId == userId
						? mapper.Map<LoggedInUserDto>(chat.FirstUser)
						: mapper.Map<LoggedInUserDto>(chat.SecondUser);
				var isFavorite = chat.FirstUserId == userId
						? chat.IsFavoriteForFirstUser
						: chat.IsFavoriteForSecondUser;
				withUser.PhotoUrl = context.Photos
						.AsNoTracking()
						.FirstOrDefault(n => n.UserId == withUser.Id && n.IsMain)?.PhotoUrl;
				user.PhotoUrl = context.Photos
						.AsNoTracking()
						.FirstOrDefault(n => n.UserId == user.Id && n.IsMain)?.PhotoUrl;
				var lastMessage = chat.LastMessageId is { } ? await context.Messages
						.AsNoTracking()
						.FirstOrDefaultAsync(n => n.Id == chat.LastMessageId) : null;
				var lastMessageDto = mapper.Map<MessageDto>(lastMessage);
				var chatDto = new ChatDto(chat.Id, chat.CreatedAt, user, withUser, isFavorite) { LastMessage = lastMessageDto };

				return ApiResult<ChatDto>.Success(chatDto);
			}
			catch (Exception ex)
			{
				return ApiResult<ChatDto>.Fail(ex.Message);
			}
		}

		public async Task<ApiResult<ChatDto>> CreateChatAsync(CreatableChatDto dto)
		{
			try
			{
				var chat = new Chat { FirstUserId = dto.FirtUserId, SecondUserId = dto.SecondUserId };

				var newChat = context.Chats.Add(chat).Entity;
				await context.SaveChangesAsync();

				var withUser = mapper.Map<LoggedInUserDto>(await context.Users.FindAsync(newChat.SecondUserId));
				withUser.PhotoUrl = context.Photos
						.AsNoTracking()
						.FirstOrDefault(n => n.UserId == withUser.Id && n.IsMain)?.PhotoUrl;
				var user = mapper.Map<LoggedInUserDto>(await context.Users.FindAsync(newChat.FirstUserId));
				user.PhotoUrl = context.Photos
					.AsNoTracking()
					.FirstOrDefault(n => n.UserId == user.Id && n.IsMain)?.PhotoUrl;
				var chatDto = new ChatDto(newChat.Id, newChat.CreatedAt, user, withUser, false) { LastMessage = null };

				await hubContext.Clients.All.ChatCreatedAsync(chatDto);

				return ApiResult<ChatDto>.Success(chatDto);
			}
			catch (Exception ex)
			{
				return ApiResult<ChatDto>.Fail(ex.Message);
			}
		}

		public async Task<ApiResult> DeleteChatAsync(int chatId)
		{
			try
			{
				var chat = await context.Chats.FindAsync(chatId);
				if (chat is null)
					return ApiResult.Fail("Chat not found");

				context.Chats.Remove(chat);
				await context.SaveChangesAsync();

				await hubContext.Clients.All.ChatDeletedAsync(chatId);

				return ApiResult.Success();
			}
			catch (Exception ex)
			{
				return ApiResult.Fail(ex.Message);
			}
		}

		public async Task<ApiResult> MakeChatFavoriteAsync(int chatId, int userId)
		{
			try
			{
				var chat = await context.Chats.FindAsync(chatId);
				if (chat is null)
					return ApiResult.Fail("Chat not found");
				if ((chat.FirstUserId == userId && chat.IsFavoriteForFirstUser) ||
					(chat.SecondUserId == userId && chat.IsFavoriteForSecondUser))
					return ApiResult.Fail("Chat is already favorite");

				if (chat.FirstUserId == userId)
				{
					chat.IsFavoriteForFirstUser = true;
				}
				else
				{
					chat.IsFavoriteForSecondUser = true;
				}

				await context.SaveChangesAsync();

				return ApiResult.Success();
			}
			catch (Exception ex)
			{
				return ApiResult.Fail(ex.Message);
			}
		}

		public async Task<ApiResult> RemoveChatFromFavoritesAsync(int chatId, int userId)
		{
			try
			{
				var chat = await context.Chats.FindAsync(chatId);
				if (chat is null)
					return ApiResult.Fail("Chat not found");
				if ((chat.FirstUserId == userId && !chat.IsFavoriteForFirstUser) ||
					(chat.SecondUserId == userId && !chat.IsFavoriteForSecondUser))
					return ApiResult.Fail("Chat is not in favorites");

				if (chat.FirstUserId == userId)
				{
					chat.IsFavoriteForFirstUser = false;
				}
				else
				{
					chat.IsFavoriteForSecondUser = false;
				}

				await context.SaveChangesAsync();

				return ApiResult.Success();
			}
			catch (Exception ex)
			{
				return ApiResult.Fail(ex.Message);
			}
		}
	}
}
