using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Reservation.Models.Common;
using Reservation.Models.ServiceMemberBranch;
using Reservation.Resources.Contents;
using Reservation.Resources.Controllers;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;
using System;
using System.Threading.Tasks;
using Reservation.ServiceMember;

namespace Reservation.ServiceMember.Controllers
{
    public class ServiceMemberBranchController : ApplicationUser
    {
        private readonly IServiceMemberBranchService _branchService;
        private readonly ILogger _logger;
        private readonly IStringLocalizer<ResourcesController> _localizer;

        public ServiceMemberBranchController(IServiceMemberBranchService branch,
            ILogger<ServiceMemberBranchController> logger,
            IStringLocalizer<ResourcesController> localizer)
        {
            _branchService = branch;
            _logger = logger;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> GetBranchById(long? branchId)
        {
            _logger.LogRequest("ServiceMemberBranch/GetBranchById", branchId);

            RequestResult result = new RequestResult();
            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("ServiceMemberBranch/GetBranchById", result);
                return Json(result);
            }
            
            if (branchId == null)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.WrongIncomingParameters);
                return Json(result);
            }

            result.Value = await _branchService.GetBranchByIdAsync(branchId.Value);
            result.Succeeded = true;
            _logger.LogResponse("ServiceMemberBranch/GetBranchById", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveServiceMemberBranch([FromBody] ServiceMemberBranchEditModel model)
        {
            _logger.LogRequest("ServiceMemberBranch/SaveServiceMemberBranch", model);
            RequestResult result = new RequestResult();
            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("ServiceMemberBranch/SaveServiceMemberBranch", result);
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
            
            if (!Time.IsValidWorkingSchedule(model.OpenTime, model.CloseTime))
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.OpenTimeMustBeEarlierThanCloseTime);
                return Json(result);
            }

            if (model.Id.HasValue && model.Id.Value != default)
            {
                result = await _branchService.EditBranchInfoAsync(model);
            }
            else
            {
                result = await _branchService.AddBranchAsync(model);
            }

            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer.GetLocalizationOf(result.Message);
            }

            _logger.LogResponse("ServiceMemberBranch/SaveServiceMemberBranch", result);
            return Json(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBranch(long? branchId)
        {
            _logger.LogRequest("ServiceMemberBranch/DeleteBranch", branchId);

            RequestResult result = new RequestResult();
            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("ServiceMemberBranch/DeleteBranch", result);
                return Json(result);
            }
            
            if (branchId == null)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.WrongIncomingParameters);
                return Json(result);
            }

            result = await _branchService.DeleteBranchAsync(branchId.Value);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer.GetLocalizationOf(result.Message);
            }

            _logger.LogResponse("ServiceMemberBranch/DeleteBranch", result);
            return Json(result);
        }
    }
}
