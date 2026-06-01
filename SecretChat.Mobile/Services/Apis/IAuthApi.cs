namespace SecretChat.Mobile.Services.Apis
{
	public interface IAuthApi
	{
		[Post("/api/auth/login")]
		Task<ApiResult<RegisterUserResponseDto>> LoginUserAsync(LoginUserDto dto);

		[Post("/api/auth/register")]
		Task<ApiResult> RegisterUserAsync(RegisterUserDto dto);
	}
}
