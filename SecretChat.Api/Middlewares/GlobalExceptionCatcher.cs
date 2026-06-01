namespace SecretChat.Api.Middlewares
{
	public class GlobalExceptionCatcher(RequestDelegate next)
	{
		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await next(context);
				await HandlerStatusCodes(context);
			}
			catch (Exception ex)
			{
				await HandleExceptions(context, ex);
			}
		}

		private async Task HandlerStatusCodes(HttpContext context)
		{
			if (context.Response.HasStarted) return;

			var problemDetails = new ProblemDetails();
			var statusCode = context.Response.StatusCode;

			switch (statusCode)
			{
				case StatusCodes.Status401Unauthorized:
					problemDetails.Title = "Unauthorized";
					problemDetails.Detail = "You are unauthorized to access this resource";
					break;
				case StatusCodes.Status403Forbidden:
					problemDetails.Title = "Forbidden";
					problemDetails.Detail = "You don't have permission to access this resource";
					break;
				case StatusCodes.Status429TooManyRequests:
					problemDetails.Title = "Too many requests";
					problemDetails.Detail = "Too many requests made. Please try it later";
					break;
			}

			context.Response.StatusCode = statusCode;
			await WhiteProblemDetailsAsync(context, problemDetails);
		}

		private async Task HandleExceptions(HttpContext context, Exception ex)
		{
			if (context.Response.HasStarted) return;

			var problemDetails = new ProblemDetails
			{
				Title = "Internal Server Error",
				Detail = "An undexpected error occurred. Please try again later",
				Status = StatusCodes.Status500InternalServerError
			};

			if (ex is TaskCanceledException || ex is TimeoutException)
			{
				problemDetails.Title = "Timout Exception";
				problemDetails.Detail = "The request is timout. Please try again later";
				problemDetails.Status = StatusCodes.Status408RequestTimeout;
			}

			await WhiteProblemDetailsAsync(context, problemDetails);
		}

		private async Task WhiteProblemDetailsAsync(HttpContext context, ProblemDetails problemDetails)
		{
			context.Response.ContentType = "application/problem+json";

			var options = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

			var json = JsonSerializer.Serialize(problemDetails, options);
			await context.Response.WriteAsync(json);
		}
	}
}
