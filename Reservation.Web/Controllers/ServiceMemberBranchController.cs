using Microsoft.AspNetCore.Mvc;
using Reservation.Models.Common;
using Reservation.Models.ServiceMemberBranch;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;
using System.Threading.Tasks;

namespace Reservation.Web.Controllers
{
    public class ServiceMemberBranchController : Controller
    {
        private readonly IServiceMemberBranchService _branch;
        public ServiceMemberBranchController(IServiceMemberBranchService branch)
        {
            _branch = branch;
        }

        [HttpGet]
        public async Task<IActionResult> GetBranchById(long? branchId)
        {
            RequestResult result = new RequestResult();
            if (branchId == null)
            {
                result.Message = ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result.Value = await _branch.GetBranchByIdAsync(branchId.Value);
            result.Succeeded = true;
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetBranches(long? serviceMemberId)
        {
            RequestResult result = new RequestResult();
            if (serviceMemberId == null)
            {
                result.Message = ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result.Value = await _branch.GetBranchesAsync(serviceMemberId.Value);
            result.Succeeded = true;
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveServiceMemberBranch([FromBody]ServiceMemberBranchEditModel model)
        {
            RequestResult result = new RequestResult();
            if (!ModelState.IsValid)
            {
                result.Message = ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            if (model.OpenTime >= model.CloseTime)
            {
                result.Message = "OpenTimeMustBeEarlierThanCloseTime";
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

            return Json(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBranch(long? branchId)
        {
            RequestResult result = new RequestResult();
            if (branchId == null)
            {
                result.Message = ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result = await _branch.DeleteBranchAsync(branchId.Value);
            return Json(result);
        }
    }
}
