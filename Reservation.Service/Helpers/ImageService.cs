using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Reservation.Service.Helpers
{
	public static class ImageService
	{
		public async static Task<string> SaveAsync(IFormFile file, string resourcePath, string path)
		{
			string filePath = $"{resourcePath}{path}";
			if (!Directory.Exists(filePath))
			{
				Directory.CreateDirectory(filePath);
			}

			var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
			string imagePath = $"{path}{uniqueFileName}";
			string fullFilefilePath = $@"{filePath}{uniqueFileName}";

			using (var fileStream = new FileStream(fullFilefilePath, FileMode.Create))
			{
				await file.CopyToAsync(fileStream);
				return imagePath;
			}
		}
	}
}
