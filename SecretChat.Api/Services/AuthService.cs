namespace SecretChat.Api.Services
{
	public class AuthService(DataContext context, JwtTokenService jwtTokenService, IMapper mapper, IHubContext<SecretChatHub, ISecretChatHubClient> hubContext) : IAuthService
	{
		public async Task<ApiResult> RegisterUserAsync(RegisterUserDto dto)
		{
			try
			{
				var user = mapper.Map<User>(dto);
				user.HashPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

				var hubMessage = context.Users.Add(user).Entity;
				await context.SaveChangesAsync();

				await hubContext.Clients.All.UserCreatedOrUpdatedAsync(mapper.Map<LoggedInUserDto>(hubMessage));

				return ApiResult.Success();
			}
			catch (Exception ex)
			{
				return ApiResult.Fail(ex.Message);
			}
		}

		public async Task<ApiResult<RegisterUserResponseDto>> LoginUserAsync(LoginUserDto dto)
		{
			try
			{
				var user = await context.Users
					.AsNoTracking()
					.Include(n => n.Photos)
					.FirstOrDefaultAsync(n => n.Email == dto.Email);
				if (user is null)
					return ApiResult<RegisterUserResponseDto>.Fail("User not found");
				if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.HashPassword))
					return ApiResult<RegisterUserResponseDto>.Fail("Invalid password");

				var loggedInUser = mapper.Map<LoggedInUserDto>(user);
				loggedInUser.PhotoUrl = user.Photos.FirstOrDefault(n => n.IsMain)?.PhotoUrl;

				var token = jwtTokenService.GenerateJwtToken(user);

				return ApiResult<RegisterUserResponseDto>.Success(new RegisterUserResponseDto(loggedInUser, token));
			}
			catch (Exception ex)
			{
				return ApiResult<RegisterUserResponseDto>.Fail(ex.Message);
			}
		}
	}
}
