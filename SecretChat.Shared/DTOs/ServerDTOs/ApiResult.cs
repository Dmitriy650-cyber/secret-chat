namespace SecretChat.Shared.DTOs.ServerDTOs
{
	public record ApiResult(bool IsSuccess, string Message)
	{
		public static ApiResult Fail(string message) => new(false, message);
		public static ApiResult Success() => new(true, string.Empty);
	}

	public record ApiResult<TData>(bool IsSuccess, TData Data, string Message)
	{
		public static ApiResult<TData> Fail(string message) => new(false, default!, message);
		public static ApiResult<TData> Success(TData data) => new(true, data, string.Empty);
	}
}
