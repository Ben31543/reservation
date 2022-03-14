using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reservation.Models.Common;
using Reservation.Models.Reserving;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;
using System.Threading.Tasks;

namespace Reservation.Web.Controllers
{
	public class ReservingController : Controller
	{
		private readonly IReservingService _reserve;
		private readonly ILogger _logger;

		public ReservingController(
			IReservingService reserve,
			ILogger<ReservingController> logger)
		{
			_reserve = reserve;
			_logger = logger;
		}

		[HttpPost]
		public async Task<IActionResult> GetReservingDealsHistory(ReservingModel model)
		{
			_logger.LogRequest("Reserving/GetReservingDealsHistory", model);
			var result = new RequestResult();

			if (!ModelState.IsValid)
			{
				result.Message = ModelState.GetErrorMessages();
				return Json(result);
			}

			result = await _reserve.AddReservingAsync(model);
			_logger.LogResponse("Reserving/GetReservingDealsHistory", result);
			return Json(result);
		}
	}
}
