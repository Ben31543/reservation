﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Reservation.Models.Common;
using Reservation.Models.Reserving;
using Reservation.Resources.Contents;
using Reservation.Resources.Controllers;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;
using System;
using System.Threading.Tasks;
using Reservation.Resources.Enumerations;

namespace Reservation.Web.Controllers
{
    public class ReservingController : ApplicationUser.ApplicationUser
    {
        private readonly IReservingService _reservingService;
        private readonly ILogger _logger;
        private readonly IStringLocalizer<ResourcesController> _localizer;

        public ReservingController(
            IReservingService reservingService,
            ILogger<ReservingController> logger,
            IStringLocalizer<ResourcesController> localizer)
        {
            _reservingService = reservingService;
            _logger = logger;
            _localizer = localizer;
        }

        [HttpPost]
        public async Task<IActionResult> AddReserving([FromBody] ReservingModel model)
        {
            _logger.LogRequest("Reserving/AddReserving", model);
            var result = new RequestResult();

            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("Reserving/AddReserving", result);
                return Json(result);
            }

            if (!ModelState.IsValid)
            {
                result.Message = _localizer.GetModelsLocalizedErrors(ModelState);
                return Json(result);
            }

            result = await _reservingService.AddReservingAsync(model);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer.GetLocalizationOf(result.Message);
            }

            _logger.LogResponse("Reserving/AddReserving", result);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> CancelReserving(long? id)
        {
            _logger.LogRequest("Reserving/CancelReserving", id);
            var result = new RequestResult();

            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("Reserving/CancelReserving", result);
                return Json(result);
            }

            if (id == null)
            {
                result.Message = _localizer.GetModelsLocalizedErrors(ModelState);
                return Json(result);
            }

            result = await _reservingService.CancelReservingAsync(id.Value);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer.GetLocalizationOf(result.Message);
            }

            _logger.LogResponse("Reserving/CancelReserving", result);
            return Json(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> GetReservablePlaces([FromBody] SearchForReservingModel model, [FromQuery] bool? needFreeTimes)
        {
            _logger.LogRequest("Reserving/GetReservablePlaces", model);
            RequestResult result = new RequestResult();

            if (model == null)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.WrongIncomingParameters);
                return Json(result);
            }
            
            if (!model.PersonsCount.HasValue)
            {
                model.PersonsCount = TableSchemas.OneToTwoPersons;
            }

            if (!model.ReservingDate.HasValue)
            {
                model.ReservingDate = DateTime.Now.AddDays(1);
            }

            if (model.ReservingDate.Value.Date < DateTime.Now.Date)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.InvalidDate);
                return Json(result);
            }

            result.Value = await _reservingService.GetReservableBranchesAsync(model);
            result.Succeeded = true;
            _logger.LogResponse("Reserving/GetReservablePlaces", result);
            return Json(result);
        }
    }
}
