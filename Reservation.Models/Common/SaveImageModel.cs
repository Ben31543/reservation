﻿using Microsoft.AspNetCore.Http;
using Reservation.Resources.Enumerations;

namespace Reservation.Models.Common
{
	public class SaveImageModel
	{
		public long? Id { get; set; }

		public ResourceTypes? ResourceType { get; set; }

		public IFormFile Image { get; set; }
	}
}
