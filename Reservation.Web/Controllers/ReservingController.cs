using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reservation.Models.Common;
using Reservation.Models.Reserving;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Web.Controllers
{
    public class ReservingController:Controller
    {
        private readonly IReservingService _reserve;
        private readonly ILogger _logger;

        public ReservingController(IReservingService reserve, ILogger<ReservingController> logger)
        {
            _reserve = reserve;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> GetReservingDealsHistory(ReservingModel model)
        {
            _logger.LogWarning("Requesting Reserving/GetReservingDealsHistory");

            var result = new RequestResult();

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages();
                return Json(result);
            }

            result = await _reserve.AddReservingAsync(model);
            return Json(result);
        }
    }
}
