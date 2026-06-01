namespace SecretChat.Api.Services.Base
{
	public class ValidationService
	{
		public bool IsValid<T>(T dto) where T : class
		{
			var validationResult = new List<ValidationResult>();
			var validationContext = new ValidationContext(dto);

			return Validator.TryValidateObject(dto, validationContext, validationResult, validateAllProperties: true);
		}
	}
}
