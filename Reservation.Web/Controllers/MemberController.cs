﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reservation.Data.Enumerations;
using Reservation.Models.BankCard;
using Reservation.Models.Common;
using Reservation.Models.Member;
using Reservation.Resources.Contents;
using Reservation.Service.Interfaces;
using System.Threading.Tasks;
using static Reservation.Resources.Contents.LocalizationKeys;

namespace Reservation.Web.Controllers
{
    public class MemberController : Controller
    {
        private readonly ILogger _logger;
        private readonly IMemberService _member;
        private readonly IHostingEnvironment _environment;

        public MemberController(
            IMemberService member,
            IHostingEnvironment environment,
            ILogger<MemberController> logger)
        {
            _member = member;
            _logger = logger;
            _environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> AddNewMember([FromBody] MemberRegistrationModel model)
        {
            _logger.LogWarning("Requesting Member/AddNewMember");

            var result = new RequestResult();
            if (!ModelState.IsValid)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result = await _member.AddNewMemberAsync(model);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMemberById(long? id)
        {
            _logger.LogWarning("Requesting Member/GetMemberById");

            var result = new RequestResult();

            if (!id.HasValue)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            var member = await _member.GetMemberByIdAsync(id.Value);
            if (!string.IsNullOrEmpty(member.ProfilePictureUrl))
            {
                member.ProfilePictureUrl = $"{_environment.WebRootPath}{member.ProfilePictureUrl}";
            }

            result.Value = member;
            result.Succeeded = true;
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMemberInfo([FromBody] MemberEditModel model)
        {
            _logger.LogWarning("Requesting Member/UpdateMemberInfo");

            var result = new RequestResult();
            if (!ModelState.IsValid)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result = await _member.UpdateMemberInfoAsync(model);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            _logger.LogWarning("Requesting Member/ResetPassword");

            var result = new RequestResult();
            if (!ModelState.IsValid)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result = await _member.ResetPasswordAsync(model);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyMember([FromBody] SignInModel model)
        {
            _logger.LogWarning("Requesting Member/VerifyMember");

            var result = new RequestResult();
            if (!ModelState.IsValid)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result = await _member.VerifyMemberAsync(model);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> AttachCardToMember([FromBody] AttachCardToMemberModel model)
        {
            _logger.LogWarning("Requesting Member/AttachCardToMember");

            var result = new RequestResult();

            if (!ModelState.IsValid)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result = await _member.AddBankCardAsync(model);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> DetachCardFromMember([FromQuery] long? memberId, [FromQuery] long? cardId)
        {
            _logger.LogWarning("Requesting Member/DetachCardFromMember");

            var result = new RequestResult();

            if (!cardId.HasValue || !memberId.HasValue)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result = await _member.DetachBankCardAsync(memberId.Value, cardId.Value);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveMemberProfileImage([FromForm] SaveImageModel model)
        {
            var result = new RequestResult();

            if (model == null || model.Image == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            if (!model.ResourceType.HasValue)
            {
                model.ResourceType = ResourceTypes.MemberImage;
            }

            result = await _member.SaveMemberProfileImageAsync(model);
            return Json(result);
        }
    }
}
