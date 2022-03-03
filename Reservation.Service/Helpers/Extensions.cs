using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Reservation.Service.Helpers
{
	public static class Extensions
	{
		public static string ToHashedPassword(this string password)
		{
			using (SHA256 sha256Hash = SHA256.Create())
			{
				byte[] sourceBytes = Encoding.UTF8.GetBytes(password);
				byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
				string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

				return hashedPassword;
			}
		}

		public static string GetErrorMessages(this ModelStateDictionary model)
		{
			StringBuilder result = new StringBuilder();
			IEnumerable<string> errorMessages = model.Values
										   .SelectMany(v => v.Errors)
										   .Select(i => i.ErrorMessage);

			result.AppendJoin(", ", errorMessages);
			return result.ToString();
		}
	}
}
