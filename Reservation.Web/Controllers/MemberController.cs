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
        private readonly IServiceMemberService _serviceMemberService;
        private readonly IStringLocalizer<ResourcesController> _localizer;
        private readonly IServiceMemberBranchService _serviceMemberBranchService;

        public MemberController(
            IMemberService member,
            ILogger<MemberController> logger,
            IServiceMemberService serviceMemberService,
            IStringLocalizer<ResourcesController> localizer,
            IServiceMemberBranchService serviceMemberBranchService)
        {
            _logger = logger;
            _localizer = localizer;
            _memberService = member;
            _serviceMemberService = serviceMemberService;
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
                _logger.LogResponse("Member/AddNewMember", result);
                return Json(result);
            }

            if (!model.Phone.IsValidArmPhoneNumber())
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.InvalidPhoneNumber);
                _logger.LogResponse("Member/AddNewMember", result);
                return Json(result);
            }

            if (!model.Email.IsValidEmail())
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.InvalidEmail);
                _logger.LogResponse("Member/AddNewMember", result);
                return Json(result);
            }

            if (!model.Password.Equals(model.ConfirmPassword, StringComparison.Ordinal))
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.PasswordDoNotMatch);
                _logger.LogResponse("Member/AddNewMember", result);
                return Json(result);
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

            id ??= CurrentMemberId;
            result.Value = await _memberService.GetMemberForViewAsync(id.Value);
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

            if (model.Id == default)
            {
                model.Id = CurrentMemberId.Value;
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
                Authenticate(model.Login, (long)result.Value);
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

            if (!ModelState.IsValid)
            {
                result.Message = _localizer.GetModelsLocalizedErrors(ModelState);
                return Json(result);
            }

            if (model.MemberId == default)
            {
                model.MemberId = CurrentMemberId.Value;
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
        public async Task<IActionResult> DetachCardFromMember([FromQuery] long? memberId, [FromQuery] string cardNumber)
        {
            _logger.LogRequest("Member/DetachCardFromMember", new {MemberId = memberId, CardId = cardNumber});
            var result = new RequestResult();

            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.WrongIncomingParameters);
                return Json(result);
            }

            memberId ??= CurrentMemberId;

            result = await _memberService.DetachBankCardAsync(memberId.Value, cardNumber);
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

            memberId ??= CurrentMemberId;
            
            result.Value = await _memberService.GetMemberDealsHistoryAsync(memberId.Value);
            result.Succeeded = true;
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

        public async Task<IActionResult> GetAttachedCardsNumber([FromQuery] long? memberId)
        {
            _logger.LogRequest("Member/GetAttachedCardsNumber", new {MemberId = memberId});
            var result = new RequestResult();

            memberId ??= CurrentMemberId;

            result.Succeeded = true;
            result.Value = await _memberService.GetBankCardNumberAsync(memberId.Value);
            _logger.LogResponse("Member/GetAttachedCardsNumber", result);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetServiceMember([FromQuery] long? smId)
        {
            _logger.LogRequest("Member/GetServiceMember", new {ServiceMemberId = smId});
            var result = new RequestResult();

            if (!smId.HasValue)
            {
                result.Message = LocalizationKeys.Errors.WrongIncomingParameters;
                _logger.LogResponse("Member/GetServiceMember", result);
                return Json(result);
            }

            var serviceMember = await _serviceMemberService.GetServiceMemberByIdAsync(smId.Value, isMember: true);
            if (serviceMember == null)
            {
                result.Message = LocalizationKeys.Errors.ServiceMemberDoesNotExist;
                _logger.LogResponse("Member/GetServiceMember", result);
                return Json(result);
            }

            result.Succeeded = true;
            result.Value = serviceMember;
            _logger.LogResponse("Member/GetServiceMember", result);
            return Json(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            RequestResult result = new RequestResult();
            LogoutUser();
            result.Succeeded = true;
            return Json(result);
        }

        [HttpGet]
        public async Task<long> GetCurrentMemberId()
        {
            return CurrentMemberId ?? 0;
        }
    }
}