namespace SecretChat.Api.Services
{
	public class UserService(DataContext context, JwtTokenService jwtTokenService, IMapper mapper, IHubContext<SecretChatHub, ISecretChatHubClient> hubContext) : IUserService
	{
		public async Task<ApiResult> ChangePasswordAsync(ChangePasswordDto dto, int userId)
		{
			try
			{
				var user = await context.Users.FindAsync(userId);
				if (user is null)
					return ApiResult.Fail("User not found");

				if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.HashPassword))
					return ApiResult.Fail("Password is invalid");

				user.HashPassword = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

				await context.SaveChangesAsync();

				return ApiResult.Success();
			}
			catch (Exception ex)
			{
				return ApiResult.Fail(ex.Message);
			}
		}

		public async Task<ApiResult<RegisterUserResponseDto>> UpdateUserAsync(UpdatableUserDto dto, int userId)
		{
			try
			{
				var user = await context.Users.FindAsync(userId);
				if (user is null)
					return ApiResult<RegisterUserResponseDto>.Fail("User not found");

				mapper.Map(dto, user);

				await context.SaveChangesAsync();

				var loggedInUser = mapper.Map<LoggedInUserDto>(user);
				loggedInUser.PhotoUrl = context.Photos
					.AsNoTracking()
					.FirstOrDefault(n => n.UserId == userId && n.IsMain)?.PhotoUrl;

				var token = jwtTokenService.GenerateJwtToken(user);

				await hubContext.Clients.All.UserCreatedOrUpdatedAsync(loggedInUser);

				return ApiResult<RegisterUserResponseDto>.Success(new RegisterUserResponseDto(loggedInUser, token));
			}
			catch (Exception ex)
			{
				return ApiResult<RegisterUserResponseDto>.Fail(ex.Message);
			}
		}

		public async Task<ApiResult<IEnumerable<LoggedInUserDto>>> GetUserContactsAsync(int userId)
		{
			try
			{
				var user = await context.Users
					.AsNoTracking()
					.Include(n => n.Contacts)
					.FirstOrDefaultAsync(n => n.Id == userId);
				if (user is null)
					return ApiResult<IEnumerable<LoggedInUserDto>>.Fail("User not found");

				var contactIds = user.Contacts.Select(n => (int)n.ContactId!);

				var contacts = mapper.Map<IEnumerable<LoggedInUserDto>>(
					context.Users.Where(n => contactIds.Any(k => k == n.Id)).ToList());

				foreach (var contact in contacts)
				{
					contact.PhotoUrl = context.Photos
						.AsNoTracking()
						.FirstOrDefault(n => n.UserId == contact.Id && n.IsMain)?.PhotoUrl;
				}

				return ApiResult<IEnumerable<LoggedInUserDto>>.Success(contacts);
			}
			catch (Exception ex)
			{
				return ApiResult<IEnumerable<LoggedInUserDto>>.Fail(ex.Message);
			}
		}

		public async Task<ApiResult<IEnumerable<LoggedInUserDto>>> GetAllUsersAsync()
		{
			try
			{
				var users = mapper.Map<IEnumerable<LoggedInUserDto>>(await context.Users
					.AsNoTracking()
					.ToListAsync());

				foreach (var user in users)
				{
					user.PhotoUrl = context.Photos
						.AsNoTracking()
						.FirstOrDefault(n => n.UserId == user.Id && n.IsMain)?.PhotoUrl;
				}

				return ApiResult<IEnumerable<LoggedInUserDto>>.Success(users);
			}
			catch (Exception ex)
			{
				return ApiResult<IEnumerable<LoggedInUserDto>>.Fail(ex.Message);
			}
		}

		public async Task<ApiResult> AddContactAsync(int id, int userId)
		{
			try
			{
				var contact = new UserContact { UserId = userId, ContactId = id };

				var hubMessage = context.Contacts.Add(contact).Entity;
				await context.SaveChangesAsync();

				await hubContext.Clients.All.ContactAddedAsync((int)hubMessage.ContactId!);

				return ApiResult.Success();
			}
			catch (Exception ex)
			{
				return ApiResult.Fail(ex.Message);
			}
		}

		public async Task<ApiResult> DeleteContactAsync(int id, int userId)
		{
			try
			{
				var contact = await context.Contacts.AsNoTracking().FirstOrDefaultAsync(n => n.ContactId == id && n.UserId == userId);
				if (contact is null)
					return ApiResult.Fail("Contact not found");

				context.Contacts.Remove(contact);
				await context.SaveChangesAsync();

				await hubContext.Clients.All.ContactDeletedAsync((int)contact.ContactId!);

				return ApiResult.Success();
			}
			catch (Exception ex)
			{
				return ApiResult.Fail(ex.Message);
			}
		}

		public async Task<ApiResult> CheckContactAsync(int id, int userId)
		{
			try
			{
				var user = await context.Users
					.AsNoTracking()
					.Include(n => n.Contacts)
					.FirstOrDefaultAsync(n => n.Id == userId);
				if (user is null)
					return ApiResult.Fail("User not found");

				var contact = user.Contacts.FirstOrDefault(n => n.ContactId == id);
				if (contact is null)
					return ApiResult.Fail("Contact not found");
				else
					return ApiResult.Success();
			}
			catch (Exception ex)
			{
				return ApiResult.Fail(ex.Message);
			}
		}
	}
}
