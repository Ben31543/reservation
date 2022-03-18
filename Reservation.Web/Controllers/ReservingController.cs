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
		private readonly IReservingService _reservingService;
		private readonly ILogger _logger;

		public ReservingController(
			IReservingService reservingService,
			ILogger<ReservingController> logger)
		{
			_reservingService = reservingService;
			_logger = logger;
		}

		[HttpPost]
		public async Task<IActionResult> AddReserving([FromBody]ReservingModel model)
		{
			_logger.LogRequest("Reserving/AddReserving", model);
			var result = new RequestResult();

			if (!ModelState.IsValid)
			{
				result.Message = ModelState.GetErrorMessages();
				return Json(result);
			}

			result = await _reservingService.AddReservingAsync(model);
			_logger.LogResponse("Reserving/AddReserving", result);
			return Json(result);
		} 

		[HttpGet]
		public async Task<IActionResult> CancelReserving(long? id)
		{
			_logger.LogRequest("Reserving/CancelReserving", id);
			var result = new RequestResult();

			if (id == null)
			{
				result.Message = ModelState.GetErrorMessages();
				return Json(result);
			}

			result = await _reservingService.CancelReservingAsync(id.Value);
			_logger.LogResponse("Reserving/CancelReserving", result);
			return Json(result);
		}
	}
}
