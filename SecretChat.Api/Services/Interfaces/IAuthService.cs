namespace SecretChat.Api.Services.Interfaces
{
	public interface IAuthService
	{
		Task<ApiResult<RegisterUserResponseDto>> LoginUserAsync(LoginUserDto dto);
		Task<ApiResult> RegisterUserAsync(RegisterUserDto dto);
	}
}