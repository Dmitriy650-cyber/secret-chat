namespace SecretChat.Api.Services.Interfaces
{
	public interface IPhotoService
	{
		Task<ApiResult<PhotoDto>> AddUserPhotoAsync(IFormFile photo, int userId);
		Task<ApiResult<IEnumerable<PhotoDto>>> GetUserPhotosByIdAsync(int id);
		Task<ApiResult<PhotoDto?>> DeleteUserPhotoAsync(int photoId);
		Task<ApiResult<IEnumerable<PhotoDto>>> GetUserPhotosAsync(int userId);
		Task<ApiResult> SetMainPhotoAsync(int photoId);
	}
}