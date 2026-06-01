namespace SecretChat.Api.Services.Interfaces
{
	public interface IUserService
	{
		Task<ApiResult> ChangePasswordAsync(ChangePasswordDto dto, int userId);
		Task<ApiResult<IEnumerable<LoggedInUserDto>>> GetUserContactsAsync(int userId);
		Task<ApiResult<RegisterUserResponseDto>> UpdateUserAsync(UpdatableUserDto dto, int userId);
		Task<ApiResult<IEnumerable<LoggedInUserDto>>> GetAllUsersAsync();
		Task<ApiResult> AddContactAsync(int id, int userId);
		Task<ApiResult> DeleteContactAsync(int id, int userId);
		Task<ApiResult> CheckContactAsync(int id, int userId);
	}
}