namespace SecretChat.Api.Endpoints
{
	public static class UserEndpoints
	{
		public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder builder)
		{
			var userGroup = builder
				.MapGroup("/api/user")
				.RequireAuthorization()
				.WithTags("User");

			userGroup.MapPut("/change-password", async ([FromBody] ChangePasswordDto dto, IUserService serivice, ClaimsPrincipal principal) =>
			{
				return await serivice.ChangePasswordAsync(dto, principal.GetUserId());
			})
				.Produces<ApiResult>()
				.WithName("ChangePassword");

			userGroup.MapPut("/", async ([FromBody] UpdatableUserDto dto, IUserService service, ClaimsPrincipal principal) =>
			{
				return await service.UpdateUserAsync(dto, principal.GetUserId());
			})
				.Produces<ApiResult<RegisterUserResponseDto>>()
				.WithName("UpdateUser");

			userGroup.MapGet("/", async (IUserService service, ClaimsPrincipal principal) =>
			{
				return await service.GetUserContactsAsync(principal.GetUserId());
			})
				.Produces<ApiResult<IEnumerable<LoggedInUserDto>>>()
				.WithName("GetUserContacts");

			userGroup.MapGet("/all", async (IUserService service) =>
			{
				return await service.GetAllUsersAsync();
			})
				.Produces<ApiResult<IEnumerable<LoggedInUserDto>>>()
				.WithName("GetAllUsers");

			userGroup.MapPost("/contact/{id:int}", async (int id, IUserService service, ClaimsPrincipal principal) =>
			{
				return await service.AddContactAsync(id, principal.GetUserId());
			})
				.Produces<ApiResult>()
				.WithName("AddContact");

			userGroup.MapDelete("/contact/{id:int}", async (int id, IUserService service, ClaimsPrincipal principal) =>
			{
				return await service.DeleteContactAsync(id, principal.GetUserId());
			})
				.Produces<ApiResult>()
				.WithName("DeleteContact");

			userGroup.MapGet("/check-contact/{id:int}", async (int id, IUserService service, ClaimsPrincipal principal) =>
			{
				return await service.CheckContactAsync(id, principal.GetUserId());
			})
				.Produces<ApiResult>()
				.WithName("CheckContact");

			return builder;
		}
	}
}
