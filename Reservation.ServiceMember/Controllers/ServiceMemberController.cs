using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Reservation.Data.Enumerations;
using Reservation.Models.BankAccount;
using Reservation.Models.Common;
using Reservation.Models.ServiceMember;
using Reservation.Resources.Contents;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;
using System.Threading.Tasks;

namespace Reservation.Web.Controllers
{
    public class ServiceMemberController : Controller
    {
        private readonly IServiceMemberService _serviceMember;
        private readonly IHostingEnvironment environment;

        public ServiceMemberController(IServiceMemberService serviceMember, IHostingEnvironment environment)
        {
            _serviceMember = serviceMember;
            this.environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterServiceMember([FromBody] ServiceMemberRegistrationModel model)
        {
            RequestResult result = new RequestResult();
            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages();
                return Json(result);
            }

            result = await _serviceMember.RegisterServiceMemberAsync(model);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetServiceMember(long? id)
        {
            RequestResult result = new RequestResult();

            if (id == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            var serviceMember = await _serviceMember.GetServiceMemberByIdAsync(id.Value);
            if (serviceMember == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                result.Value = id;
                return Json(result);
            }

			if (!string.IsNullOrEmpty(serviceMember.LogoUrl))
			{
                serviceMember.LogoUrl = $"{environment.WebRootPath}{serviceMember.LogoUrl}";
			}

            if (!string.IsNullOrEmpty(serviceMember.ImageUrl))
            {
                serviceMember.ImageUrl = $"{environment.WebRootPath}{serviceMember.ImageUrl}";
            }

            result.Succeeded = true;
            result.Value = serviceMember;
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyServiceMember([FromBody] SignInModel model)
        {
            RequestResult result = new RequestResult();

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages();
                return Json(result);
            }

            result = await _serviceMember.VerifyServiceMemberAsync(model);

            if (!result.Succeeded)
            {
                return NotFound(result);
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            RequestResult result = new RequestResult();
            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages();
                return Json(result);
            }

            result = await _serviceMember.ResetPasswordAsync(model);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateServiceMember([FromBody] ServiceMemberEditModel model)
        {
            RequestResult result = new RequestResult();
            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages();
                return Json(result);
            }

            result = await _serviceMember.UpdateServiceMemberInfoAsync(model);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> AttachBankAccount([FromBody] BankAccountAttachModel model)
        {
            RequestResult result = new RequestResult();
            if (!model.ServiceMemberId.HasValue)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result = await _serviceMember.AddBankAccountAsync(model);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> DetachFromBankAccount([FromQuery] long? serviceMemberId, [FromQuery] long? bankAccountId)
        {
            var result = new RequestResult();
            if (!serviceMemberId.HasValue || !bankAccountId.HasValue)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result = await _serviceMember.DetachBankAccountAsync(serviceMemberId.Value, bankAccountId.Value);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetServiceMemberDealsHistory(long? serviceMemberId)
        {
            RequestResult result = new RequestResult();
            if (!serviceMemberId.HasValue)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result.Value = await _serviceMember.GetServiceMemberDealsHistoryAsync(serviceMemberId.Value);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveServiceMemberImage([FromForm] SaveImageModel model)
		{
            var result = new RequestResult();

			if (model == null || model.Image == null)
			{
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
			}

			if (!model.ResourceType.HasValue)
			{
                model.ResourceType = ResourceTypes.ServiceMemberImage;
			}

            result = await _serviceMember.SaveServiceMemberImageAsync(model);
            return Json(result);
		}
    }
}
