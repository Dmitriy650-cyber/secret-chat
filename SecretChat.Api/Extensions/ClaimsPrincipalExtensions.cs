namespace SecretChat.Api.Extensions
{
	public static class ClaimsPrincipalExtensions
	{
		public static int GetUserId(this ClaimsPrincipal principal) =>
			int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
	}
}
