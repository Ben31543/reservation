using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Reservation.Models.BankCard;
using Reservation.Models.Common;
using Reservation.Models.Member;
using Reservation.Resources.Contents;
using Reservation.Resources.Controllers;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Reservation.Resources.Enumerations;

namespace Reservation.Web.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _environment;
        private readonly IStringLocalizer<ResourcesController> _localizer;

        public MemberController(
            IMemberService member,
            ILogger<MemberController> logger,
            IHostingEnvironment environment,
            IStringLocalizer<ResourcesController> localizer)
        {
            _memberService = member;
            _logger = logger;
            _environment = environment;
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddNewMember([FromBody] MemberRegistrationModel model)
        {
            _logger.LogRequest("Member/AddNewMember", model);

            var result = new RequestResult();
            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages();
                return Json(result);
            }

            if (!model.Phone.IsValidArmPhoneNumber())
            {
                result.Message = _localizer[LocalizationKeys.ErrorMessages.InvalidPhoneNumber].Value;
                return Json(result);
            }

            if (!model.Email.IsValidEmail())
            {
                result.Message = _localizer[LocalizationKeys.ErrorMessages.InvalidEmail].Value;
                return Json(result);
            }

            if (!model.Password.Equals(model.ConfirmPassword, StringComparison.Ordinal))
            {
                result.Message = _localizer[LocalizationKeys.ErrorMessages.PasswordDoNotMatch].Value;
                return Json(result);
            }

            result = await _memberService.AddNewMemberAsync(model);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer[result.Message].Value;
            }

            _logger.LogResponse("Member/AddNewMember", result);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMemberById(long? id)
        {
            _logger.LogRequest("Member/GetMemberById", id);
            var result = new RequestResult();

            if (!id.HasValue)
            {
                result.Message = _localizer[LocalizationKeys.ErrorMessages.WrongIncomingParameters].Value;
                return Json(result);
            }

            var member = await _memberService.GetMemberByIdAsync(id.Value);
            if (member != null && !string.IsNullOrEmpty(member.ProfilePictureUrl))
            {
                member.ProfilePictureUrl = $"{_environment.WebRootPath}{member.ProfilePictureUrl}";
            }

            result.Value = member;
            result.Succeeded = true;
            _logger.LogResponse("Member/GetMemberById", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMemberInfo([FromBody] MemberEditModel model)
        {
            _logger.LogRequest("Member/UpdateMemberInfo", model);
            var result = new RequestResult();
            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages();
                return Json(result);
            }

            if (!model.Phone.IsValidArmPhoneNumber())
            {
                result.Message = _localizer[LocalizationKeys.ErrorMessages.InvalidPhoneNumber].Value;
                return Json(result);
            }

            if (!model.Email.IsValidEmail())
            {
                result.Message = _localizer[LocalizationKeys.ErrorMessages.InvalidEmail].Value;
                return Json(result);
            }

            result = await _memberService.UpdateMemberInfoAsync(model);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer[result.Message].Value;
            }

            _logger.LogResponse("Member/UpdateMemberInfo", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            _logger.LogRequest("Member/ResetPassword", model);
            var result = new RequestResult();
            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages();
                return Json(result);
            }

            if (!model.NewPassword.Equals(model.ConfirmNewPassword, StringComparison.Ordinal))
            {
                result.Message = _localizer[LocalizationKeys.ErrorMessages.PasswordDoNotMatch].Value;
                return Json(result);
            }

            result = await _memberService.ResetPasswordAsync(model);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer[result.Message].Value;
            }

            _logger.LogResponse("Member/ResetPassword", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyMember([FromBody] SignInModel model)
        {
            _logger.LogRequest("Member/VerifyMember", model);
            var result = new RequestResult();
            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages();
                return Json(result);
            }

            result = await _memberService.VerifyMemberAsync(model);
            if (result.Succeeded)
            {
                await Authenticate(model.Login);
            }

            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer[result.Message].Value;
            }

            _logger.LogResponse("Member/VerifyMember", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> AttachCardToMember([FromBody] AttachCardToMemberModel model)
        {
            _logger.LogRequest("Member/AttachCardToMember", model);
            var result = new RequestResult();

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages();
                return Json(result);
            }

            result = await _memberService.AddBankCardAsync(model);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer[result.Message].Value;
            }

            _logger.LogResponse("Member/AttachCardToMember", result);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> DetachCardFromMember([FromQuery] long? memberId, [FromQuery] long? cardId)
        {
            _logger.LogRequest("Member/DetachCardFromMember", new { MemberId = memberId, CardId = cardId });
            var result = new RequestResult();

            if (!cardId.HasValue || !memberId.HasValue)
            {
                result.Message = _localizer[LocalizationKeys.ErrorMessages.WrongIncomingParameters].Value;
                return Json(result);
            }

            result = await _memberService.DetachBankCardAsync(memberId.Value, cardId.Value);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer[result.Message].Value;
            }

            _logger.LogResponse("Member/DetachCardFromMember", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetMemberDealsHistory(long? memberId)
        {
            _logger.LogRequest("Member/GetMemberDealsHistory", memberId);
            RequestResult result = new RequestResult();
            if (!memberId.HasValue)
            {
                result.Message = _localizer[LocalizationKeys.ErrorMessages.WrongIncomingParameters].Value;
                return Json(result);
            }

            result.Value = await _memberService.GetMemberDealsHistoryAsync(memberId.Value);
            _logger.LogResponse("Member/GetMemberDealsHistory", result);
            return (Json(result));
        }

        [HttpPost]
        public async Task<IActionResult> SaveMemberProfilePicture([FromForm] SaveImageModel model)
        {
            _logger.LogRequest("Member/SaveMemberProfilePicture", model);
            var result = new RequestResult();

            if (model == null || model.Image == null)
            {
                result.Message = _localizer[LocalizationKeys.ErrorMessages.WrongIncomingParameters].Value;
                return Json(result);
            }

            if (!model.ResourceType.HasValue)
            {
                model.ResourceType = ResourceTypes.MemberImage;
            }

            result = await _memberService.SaveMemberProfileImageAsync(model);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer[result.Message].Value;
            }

            _logger.LogResponse("Member/SaveMemberProfilePicture", result);
            return Json(result);
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
