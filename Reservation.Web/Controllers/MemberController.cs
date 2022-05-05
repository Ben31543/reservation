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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Reservation.Web.Controllers
{
    public class MemberController : ApplicationUser.ApplicationUser
    {
        private static object locker = new object();
        
        private readonly ILogger _logger;
        private readonly IMemberService _memberService;
        private readonly IStringLocalizer<ResourcesController> _localizer;
        private readonly IServiceMemberBranchService _serviceMemberBranchService;

        public MemberController(
            IMemberService member,
            ILogger<MemberController> logger,
            IStringLocalizer<ResourcesController> localizer,
            IServiceMemberBranchService serviceMemberBranchService)
        {
            _logger = logger;
            _localizer = localizer;
            _memberService = member;
            _serviceMemberBranchService = serviceMemberBranchService;
        }

        [HttpPost]
        public async Task<IActionResult> AddNewMember([FromBody] MemberRegistrationModel model)
        {
            _logger.LogRequest("Member/AddNewMember", model);

            var result = new RequestResult();
            if (!ModelState.IsValid)
            {
                result.Message = _localizer.GetModelsLocalizedErrors(ModelState);
            }

            if (!model.Phone.IsValidArmPhoneNumber())
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.InvalidPhoneNumber);
            }

            if (!model.Email.IsValidEmail())
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.InvalidEmail);
            }

            if (!model.Password.Equals(model.ConfirmPassword, StringComparison.Ordinal))
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.PasswordDoNotMatch);
            }

            result = await _memberService.AddNewMemberAsync(model);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer.GetLocalizationOf(result.Message);
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
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.WrongIncomingParameters);
                return Json(result);
            }

            result.Value = await _memberService.GetMemberByIdAsync(id.Value);
            result.Succeeded = true;
            _logger.LogResponse("Member/GetMemberById", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMemberInfo([FromBody] MemberEditModel model)
        {
            _logger.LogRequest("Member/UpdateMemberInfo", model);
            var result = new RequestResult();

            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("Member/UpdateMemberInfo", result);
                return Json(result);
            }
            
            if (!ModelState.IsValid)
            {
                result.Message = _localizer.GetModelsLocalizedErrors(ModelState);
                return Json(result);
            }

            if (!model.Phone.IsValidArmPhoneNumber())
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.InvalidPhoneNumber);
                return Json(result);
            }

            if (!model.Email.IsValidEmail())
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.InvalidEmail);
                return Json(result);
            }

            result = await _memberService.UpdateMemberInfoAsync(model);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer.GetLocalizationOf(result.Message);
            }

            _logger.LogResponse("Member/UpdateMemberInfo", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            _logger.LogRequest("Member/ResetPassword", model);
            var result = new RequestResult();
            
            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("Member/ResetPassword", result);
                return Json(result);
            }
            
            if (!ModelState.IsValid)
            {
                result.Message = _localizer.GetModelsLocalizedErrors(ModelState);
                return Json(result);
            }

            if (!model.NewPassword.Equals(model.ConfirmNewPassword, StringComparison.Ordinal))
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.PasswordDoNotMatch);
                return Json(result);
            }

            result = await _memberService.ResetPasswordAsync(model);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.PasswordDoNotMatch);
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
                result.Message = _localizer.GetModelsLocalizedErrors(ModelState);
                return Json(result);
            }

            result = await _memberService.VerifyMemberAsync(model);
            if (result.Succeeded)
            {
                Authenticate(model.Login);
            }

            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer.GetLocalizationOf(result.Message);
                return Json(result);
            }

            _logger.LogResponse("Member/VerifyMember", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> AttachCardToMember([FromBody] AttachCardToMemberModel model)
        {
            _logger.LogRequest("Member/AttachCardToMember", model);
            var result = new RequestResult();

            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("Member/AttachCardToMember", result);
                return Json(result);
            }
            
            if (!ModelState.IsValid)
            {
                result.Message = _localizer.GetModelsLocalizedErrors(ModelState);
                return Json(result);
            }

            result = await _memberService.AddBankCardAsync(model);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer.GetLocalizationOf(result.Message);
            }

            _logger.LogResponse("Member/AttachCardToMember", result);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> DetachCardFromMember([FromQuery] long? memberId, [FromQuery] long? cardId)
        {
            _logger.LogRequest("Member/DetachCardFromMember", new {MemberId = memberId, CardId = cardId});
            var result = new RequestResult();

            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("Member/DetachCardFromMember", result);
                return Json(result);
            }
            
            if (!cardId.HasValue || !memberId.HasValue)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.WrongIncomingParameters);
                return Json(result);
            }

            result = await _memberService.DetachBankCardAsync(memberId.Value, cardId.Value);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer.GetLocalizationOf(result.Message);
            }

            _logger.LogResponse("Member/DetachCardFromMember", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetMemberDealsHistory(long? memberId)
        {
            _logger.LogRequest("Member/GetMemberDealsHistory", memberId);
            RequestResult result = new RequestResult();
            
            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("Member/GetMemberDealsHistory", result);
                return Json(result);
            }
            
            if (!memberId.HasValue)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.WrongIncomingParameters);
                return Json(result);
            }

            result.Value = await _memberService.GetMemberDealsHistoryAsync(memberId.Value);
            _logger.LogResponse("Member/GetMemberDealsHistory", result);
            return Json(result);
        }

        [HttpGet]
        public async Task<List<SelectListItem>> GetServiceMemberBranchesForSelectize([FromQuery] long? smId)
        {
            if (smId == null)
            {
                return null;
            }

            var branches = await _serviceMemberBranchService.GetBranchesAsync(smId.Value);
            return branches.Select(branch => new SelectListItem
            {
                Text = $"{branch.Name} {branch.Address}",
                Value = branch.Id.ToString()
            }).ToList();
        }

        public async Task<IActionResult> Logout()
        {
            RequestResult result = new RequestResult();
            LogoutUser();
            result.Succeeded = true;
            return Json(result);
        }
    }
}