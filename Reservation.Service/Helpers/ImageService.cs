using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Reservation.Service.Helpers
{
	public static class ImageService
	{
		public async static Task<string> SaveAsync(IFormFile file, string path)
		{
			var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
			string filePath = $@"{path}{uniqueFileName}";

			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(fileStream);
				return path;
			}
		}
	}
}
