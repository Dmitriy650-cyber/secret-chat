namespace SecretChat.Api.Endpoints
{
	public static class PhotoEndpoints
	{
		public static IEndpointRouteBuilder MapPhotoEndpoints(this IEndpointRouteBuilder builder)
		{
			var photoGroup = builder
				.MapGroup("/api/photo")
				.RequireAuthorization()
				.WithTags("Photo");

			photoGroup.MapGet("/", async (IPhotoService service, ClaimsPrincipal principal) =>
			{
				return await service.GetUserPhotosAsync(principal.GetUserId());
			})
				.Produces<ApiResult<IEnumerable<PhotoDto>>>()
				.WithName("GetUserPhotos");

			photoGroup.MapGet("/{id:int}", async (int id, IPhotoService service) =>
			{
				return await service.GetUserPhotosByIdAsync(id);
			})
				.Produces<ApiResult<IEnumerable<PhotoDto>>>()
				.WithName("GetUserPhotosById");

			photoGroup.MapPost("/", async ([FromForm] IFormFile photo, IPhotoService service, ClaimsPrincipal principal) =>
			{
				return await service.AddUserPhotoAsync(photo, principal.GetUserId());
			})
				.Produces<ApiResult<PhotoDto>>()
				.DisableAntiforgery()
				.WithName("AddUserPhoto");

			photoGroup.MapDelete("/{photoId:int}", async (int photoId, IPhotoService service) =>
			{
				return await service.DeleteUserPhotoAsync(photoId);
			})
				.Produces<ApiResult<PhotoDto?>>()
				.WithName("DeleteUserPhoto");

			photoGroup.MapPut("/{photoId:int}", async (int photoId, IPhotoService service) =>
			{
				return await service.SetMainPhotoAsync(photoId);
			})
				.Produces<ApiResult>()
				.WithName("SetMainPhoto");
					
			return builder;
		}
	}
}
