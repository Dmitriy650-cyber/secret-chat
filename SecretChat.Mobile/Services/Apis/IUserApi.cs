namespace SecretChat.Mobile.Services.Apis
{
	[Headers("Authorization: Bearer")]
	public interface IUserApi
	{
		[Put("/api/user/change-password")]
		Task<ApiResult> ChangePasswordAsync(ChangePasswordDto dto);

		[Get("/api/user")]
		Task<ApiResult<IEnumerable<LoggedInUserDto>>> GetUserContactsAsync();

		[Get("/api/user/all")]
		Task<ApiResult<IEnumerable<LoggedInUserDto>>> GetAllUsersAsync();

		[Put("/api/user")]
		Task<ApiResult<RegisterUserResponseDto>> UpdateUserAsync(UpdatableUserDto dto);

		[Post("/api/user/contact/{id}")]
		Task<ApiResult> AddContactAsync(int id);

		[Delete("/api/user/contact/{id}")]
		Task<ApiResult> DeleteContactAsync(int id);

		[Get("/api/user/check-contact/{id}")]
		Task<ApiResult> CheckContactAsync(int id);
	}
}
