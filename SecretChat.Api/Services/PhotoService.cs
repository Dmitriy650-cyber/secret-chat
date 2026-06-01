namespace SecretChat.Api.Services
{
	public class PhotoService(DataContext context, FileUploadingService fileUploadingService, IMapper mapper) : IPhotoService
	{
		public async Task<ApiResult<IEnumerable<PhotoDto>>> GetUserPhotosAsync(int userId)
		{
			try
			{
				var userPhotos = await context.Photos.Where(n => n.UserId == userId).ToArrayAsync();

				return ApiResult<IEnumerable<PhotoDto>>.Success(mapper.Map<IEnumerable<PhotoDto>>(userPhotos));
			}
			catch (Exception ex)
			{
				return ApiResult<IEnumerable<PhotoDto>>.Fail(ex.Message);
			}
		}

		public async Task<ApiResult<IEnumerable<PhotoDto>>> GetUserPhotosByIdAsync(int id)
		{
			try
			{
				var userPhotos = await context.Photos.Where(n => n.UserId == id).ToArrayAsync();

				return ApiResult<IEnumerable<PhotoDto>>.Success(mapper.Map<IEnumerable<PhotoDto>>(userPhotos));
			}
			catch(Exception ex)
			{
				return ApiResult<IEnumerable<PhotoDto>>.Fail(ex.Message);
			}
		}

		public async Task<ApiResult<PhotoDto>> AddUserPhotoAsync(IFormFile photo, int userId)
		{
			try
			{
				var user = await context.Users.Include(n => n.Photos).FirstOrDefaultAsync(n => n.Id == userId);
				if (user is null)
					return ApiResult<PhotoDto>.Fail("User not found");

				var newPhoto = new Photo();

				(newPhoto.PhotoPath, newPhoto.PhotoUrl) = await fileUploadingService.SaveFileAsync(photo, "uploads", "images", "users");
				newPhoto.UserId = userId;

				if (user.Photos.Count == 0)
					newPhoto.IsMain = true;

				await context.Photos.AddAsync(newPhoto);
				await context.SaveChangesAsync();

				return ApiResult<PhotoDto>.Success(mapper.Map<PhotoDto>(newPhoto));
			}
			catch (Exception ex)
			{
				return ApiResult<PhotoDto>.Fail(ex.Message);
			}
		}

		public async Task<ApiResult<PhotoDto?>> DeleteUserPhotoAsync(int photoId)
		{
			try
			{
				var photo = await context.Photos.FindAsync(photoId);
				if (photo is null)
					return ApiResult<PhotoDto?>.Fail("Photo not found");

				var user = await context.Users.Include(n => n.Photos).FirstOrDefaultAsync(n => n.Id == photo.UserId);
				if (user is null)
					return ApiResult<PhotoDto?>.Fail("User not found");

				if (photo.IsMain && user.Photos.Count > 1)
				{
					user.Photos.Last().IsMain = true;
				}

				context.Photos.Remove(photo);
				await context.SaveChangesAsync();

				if (File.Exists(photo.PhotoPath))
					File.Delete(photo.PhotoPath);

				var photoResponse = user.Photos.FirstOrDefault(n => n.IsMain);

				return ApiResult<PhotoDto?>.Success(photoResponse is { } ? mapper.Map<PhotoDto>(photoResponse) : null);
			}
			catch (Exception ex)
			{
				return ApiResult<PhotoDto?>.Fail(ex.Message);
			}
		}

		public async Task<ApiResult> SetMainPhotoAsync(int photoId)
		{
			try
			{
				var photo = await context.Photos.FindAsync(photoId);
				if (photo is null)
					return ApiResult.Fail("Photo not found");

				var user = await context.Users
					.Include(n => n.Photos)
					.FirstOrDefaultAsync(n => n.Id == photo.UserId);
				if (user is null)
					return ApiResult.Fail("User not found");

				foreach (var p in user.Photos)
				{
					p.IsMain = false;
				}

				photo.IsMain = true;

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
