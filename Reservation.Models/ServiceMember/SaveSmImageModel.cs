using Microsoft.AspNetCore.Http;
using Reservation.Data.Enumerations;

namespace Reservation.Models.ServiceMember
{
	public class SaveSmImageModel
	{
		public long ServiceMemberId { get; set; }

		public ResourceTypes ResourceType { get; set; }

		public IFormFile Image { get; set; }
	}
}
