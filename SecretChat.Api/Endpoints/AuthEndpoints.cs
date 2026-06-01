namespace SecretChat.Api.Endpoints
{
	public static class AuthEndpoints
	{
		public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder builder)
		{
			var authGroup = builder
				.MapGroup("/api/auth")
				.WithTags("Auth");

			authGroup.MapPost("/register", async ([FromBody] RegisterUserDto dto, IAuthService service) =>
			{
				return await service.RegisterUserAsync(dto);
			})
				.Produces<ApiResult>()
				.WithName("RegisterUser");

			authGroup.MapPost("/login", async ([FromBody] LoginUserDto dto, IAuthService service) =>
			{
				return await service.LoginUserAsync(dto);
			})
				.Produces<ApiResult<RegisterUserResponseDto>>()
				.WithName("LoginUser");

			return builder;
		}
	}
}
