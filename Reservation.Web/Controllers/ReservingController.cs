//using Microsoft.AspNetCore.Mvc;
//using Reservation.Models.Common;
//using Reservation.Models.Reserving;
//using Reservation.Service.Helpers;
//using Reservation.Service.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Reservation.Web.Controllers
//{
//    public class ReservingController:Controller
//    {
//        private readonly IReservingService _reserve;

//        public ReservingController(IReservingService reserve)
//        {
//            _reserve = reserve;
//        }

//        [HttpPost]
//        public async Task<IActionResult> GetReservingDealsHistory(ReservingModel model)
//        {
//            var result = new RequestResult();

//            if (!ModelState.IsValid)
//            {
//                result.Message = ModelState.GetErrorMessages();
//                return Json(result);
//            }

//            result = await _reserve.AddReservingAsync(model);
//            return Json(result);
//        }
//    }
//}
