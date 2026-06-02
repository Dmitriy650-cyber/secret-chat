namespace SecretChat.Api.Services.Base
{
	public class FileUploadingService(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
	{
		public async Task<(string filePath, string fileUrl)> SaveFileAsync(IFormFile file, params string[] paths)
		{
			var targerPath = Path.Combine([webHostEnvironment.WebRootPath, .. paths]);

			if (!Directory.Exists(targerPath))
			{
				Directory.CreateDirectory(targerPath);
			}

			var extension = Path.GetExtension(file.FileName);
			var newFileName = $"{Guid.NewGuid()}_{DateTime.UtcNow.Ticks}{extension}";
			var filePath = Path.Combine(targerPath, newFileName);

			using FileStream fs = File.Create(filePath);
			await file.CopyToAsync(fs);

			var domainUrl = configuration.GetValue<string>("Domain")!.TrimEnd('/');
			var fileUrl = $"{domainUrl}/{string.Join('/', paths)}/{newFileName}";

			return (filePath, fileUrl);
		}
	}
}
