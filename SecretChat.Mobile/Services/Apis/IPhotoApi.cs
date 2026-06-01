namespace SecretChat.Mobile.Services.Apis
{
	[Headers("Authorization: Bearer")]
	public interface IPhotoApi
	{
		[Multipart]
		[Post("/api/photo")]
		Task<ApiResult<PhotoDto>> AddUserPhotoAsync(StreamPart photo);

		[Delete("/api/photo/{photoId}")]
		Task<ApiResult<PhotoDto?>> DeleteUserPhotoAsync(int photoId);

		[Get("/api/photo")]
		Task<ApiResult<IEnumerable<PhotoDto>>> GetUserPhotosAsync();

		[Get("/api/photo/{id}")]
		Task<ApiResult<IEnumerable<PhotoDto>>> GetUserPhotosByIdAsync(int id);

		[Put("/api/photo/{photoId}")]
		Task<ApiResult> SetMainPhotoAsync(int photoId);
	}
}
