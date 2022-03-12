using Microsoft.AspNetCore.Http;
using Reservation.Data.Enumerations;

namespace Reservation.Models.ServiceMember
{
	public class SaveImageModel
	{
		public long Id { get; set; }

		public ResourceTypes? ResourceType { get; set; }

		public IFormFile Image { get; set; }
	}
}
