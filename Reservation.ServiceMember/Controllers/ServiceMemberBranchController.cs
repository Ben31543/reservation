using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reservation.Models.Common;
using Reservation.Models.ServiceMemberBranch;
using Reservation.Resources.Contents;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;
using System;
using System.Threading.Tasks;

namespace Reservation.Web.Controllers
{
    public class ServiceMemberBranchController : Controller
    {
        private readonly IServiceMemberBranchService _branch;
        private readonly ILogger _logger;

        public ServiceMemberBranchController(IServiceMemberBranchService branch, ILogger<ServiceMemberBranchController> logger)
        {
            _branch = branch;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetBranchById(long? branchId)
        {
            _logger.LogRequest("ServiceMemberBranch/GetBranchById", branchId);

            RequestResult result = new RequestResult();
            if (branchId == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result.Value = await _branch.GetBranchByIdAsync(branchId.Value);
            result.Succeeded = true;
            _logger.LogResponse("ServiceMemberBranch/GetBranchById", result);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetBranches(long? serviceMemberId)
        {
            _logger.LogRequest("ServiceMemberBranch/GetBranches", serviceMemberId);

            RequestResult result = new RequestResult();
            if (serviceMemberId == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result.Value = await _branch.GetBranchesAsync(serviceMemberId.Value);
            result.Succeeded = true;
            _logger.LogResponse("ServiceMemberBranch/GetBranches", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveServiceMemberBranch([FromBody] ServiceMemberBranchEditModel model)
        {
            _logger.LogRequest("ServiceMemberBranch/SaveServiceMemberBranch", model);
            RequestResult result = new RequestResult();
            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages();
                return Json(result);
            }

            if (model.OpenTime.Hour >= model.CloseTime.Hour)
            {
                result.Message = LocalizationKeys.ErrorMessages.OpenTimeMustBeEarlierThanCloseTime;
                return Json(result);
            }

            if (model.Id.HasValue && model.Id.Value != default)
            {
                result = await _branch.EditBranchInfoAsync(model);
            }
            else
            {
                result = await _branch.AddBranchAsync(model);
            }

            _logger.LogResponse("ServiceMemberBranch/SaveServiceMemberBranch", result);
            return Json(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBranch(long? branchId)
        {
            _logger.LogRequest("ServiceMemberBranch/DeleteBranch", branchId);

            RequestResult result = new RequestResult();
            if (branchId == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result = await _branch.DeleteBranchAsync(branchId.Value);
            _logger.LogResponse("ServiceMemberBranch/DeleteBranch", result);
            return Json(result);
        }
    }
}
