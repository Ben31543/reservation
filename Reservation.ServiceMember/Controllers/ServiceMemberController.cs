using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reservation.Data.Enumerations;
using Reservation.Models.BankAccount;
using Reservation.Models.Common;
using Reservation.Models.Criterias;
using Reservation.Models.Dish;
using Reservation.Models.ServiceMember;
using Reservation.Resources.Contents;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;
using System.Threading.Tasks;

namespace Reservation.Web.Controllers
{
    public class ServiceMemberController : Controller
    {
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _environment;
        private readonly IServiceMemberService _serviceMember;
        private readonly IServiceMemberBranchService _branch;
        private readonly IDishService _dish;

        public ServiceMemberController(
            IServiceMemberService serviceMember,
            ILogger<ServiceMemberController> logger,
            IHostingEnvironment environment,
            IServiceMemberBranchService branch,
            IDishService dish)
        {
            _serviceMember = serviceMember;
            _logger = logger;
            _environment = environment;
            _branch = branch;
            _dish = dish;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterServiceMember([FromBody] ServiceMemberRegistrationModel model)
        {
            _logger.LogRequest("ServiceMember/RegisterServiceMember", model);

            RequestResult result = new RequestResult();
            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages();
                return Json(result);
            }

            result = await _serviceMember.RegisterServiceMemberAsync(model);
            _logger.LogResponse("ServiceMember/RegisterServiceMember", result);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetServiceMember(long? id)
        {
            _logger.LogRequest("ServiceMember/GetServiceMember", id);

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
                serviceMember.LogoUrl = $"{_environment.WebRootPath}{serviceMember.LogoUrl}";
            }

            if (!string.IsNullOrEmpty(serviceMember.ImageUrl))
            {
                serviceMember.ImageUrl = $"{_environment.WebRootPath}{serviceMember.ImageUrl}";
            }

            result.Succeeded = true;
            result.Value = serviceMember;
            _logger.LogResponse("ServiceMember/GetServiceMember", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyServiceMember([FromBody] SignInModel model)
        {
            _logger.LogRequest("ServiceMember/VerifyServiceMember", model);

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

            _logger.LogResponse("ServiceMember/VerifyServiceMember", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            _logger.LogRequest("ServiceMember/ResetPassword", model);

            RequestResult result = new RequestResult();
            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages();
                return Json(result);
            }

            result = await _serviceMember.ResetPasswordAsync(model);
            _logger.LogResponse("ServiceMember/ResetPassword", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateServiceMember([FromBody] ServiceMemberEditModel model)
        {
            _logger.LogRequest("ServiceMember/UpdateServiceMember", model);

            RequestResult result = new RequestResult();
            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages();
                return Json(result);
            }

            result = await _serviceMember.UpdateServiceMemberInfoAsync(model);
            _logger.LogResponse("ServiceMember/UpdateServiceMember", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> AttachBankAccount([FromBody] BankAccountAttachModel model)
        {
            _logger.LogRequest("ServiceMember/AttachBankAccount", model);

            RequestResult result = new RequestResult();
            if (!model.ServiceMemberId.HasValue)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result = await _serviceMember.AddBankAccountAsync(model);
            _logger.LogResponse("ServiceMember/AttachBankAccount", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> DetachFromBankAccount([FromQuery] long? serviceMemberId, [FromQuery] long? bankAccountId)
        {
            _logger.LogRequest("ServiceMember/DetachFromBankAccount", new { ServiceMemberId = serviceMemberId, BankAccountId = bankAccountId });

            var result = new RequestResult();
            if (!serviceMemberId.HasValue || !bankAccountId.HasValue)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result = await _serviceMember.DetachBankAccountAsync(serviceMemberId.Value, bankAccountId.Value);
            _logger.LogResponse("ServiceMember/DetachFromBankAccount", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetServiceMemberDealsHistory(long? serviceMemberId)
        {
            _logger.LogRequest("ServiceMember/GetServiceMemberDealsHistory", serviceMemberId);

            RequestResult result = new RequestResult();
            if (!serviceMemberId.HasValue)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result.Value = await _serviceMember.GetServiceMemberDealsHistoryAsync(serviceMemberId.Value);
            _logger.LogResponse("ServiceMember/GetServiceMemberDealsHistory", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveServiceMemberImage([FromForm] SaveImageModel model)
        {
            _logger.LogRequest("ServiceMember/SaveServiceMemberImage", model);
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
            _logger.LogResponse("ServiceMember/SaveServiceMemberImage", result);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetBranches(long? serviceMemberId)
        {
            _logger.LogRequest("ServiceMember/GetBranches", serviceMemberId);

            RequestResult result = new RequestResult();
            if (serviceMemberId == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result.Value = await _branch.GetBranchesAsync(serviceMemberId.Value);
            result.Succeeded = true;
            _logger.LogResponse("ServiceMember/GetBranches", result);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetSelectedDish(long? dishId)
        {
            _logger.LogRequest("ServiceMember/GetSelectedDish", dishId);

            RequestResult result = new RequestResult();
            if (dishId == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result.Value = await _dish.GetDishById(dishId.Value);
            result.Succeeded = true;
            _logger.LogResponse("ServiceMember/GetSelectedDish", result);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetDishes(DishSearchCriteria dishSearch)
        {
            _logger.LogRequest("ServiceMember/GetDishes", dishSearch);

            RequestResult result = new RequestResult();
            if (dishSearch.ServiceMemberId == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result.Value = await _dish.GetDishesAsync(dishSearch);
            result.Succeeded = true;
            _logger.LogResponse("ServiceMember/GetDishes", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewDish([FromForm] DishModel model)
        {
            _logger.LogRequest("ServiceMember/AddNewDish", model);

            RequestResult result = new RequestResult();
            if (!ModelState.IsValid)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result.Value = await _dish.AddNewDishAsync(model);
            _logger.LogResponse("ServiceMember/AddNewDish", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> EditDish([FromForm] DishModel model)
        {
            _logger.LogRequest("ServiceMember/EditDish", model);

            RequestResult result = new RequestResult();
            if (!ModelState.IsValid)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result.Value = await _dish.EditDishAsync(model);
            _logger.LogResponse("ServiceMember/EditDish", result);
            return Json(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDish(long? dishId)
        {
            _logger.LogRequest("ServiceMember/DeleteDish", dishId);

            RequestResult result = new RequestResult();
            if (dishId == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result.Value = await _dish.DeleteDishAsync(dishId.Value);
            _logger.LogResponse("ServiceMember/DeleteDish", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveDishImage([FromForm] SaveImageModel model)
        {
            _logger.LogRequest("ServiceMember/SaveDishImage", model);

            RequestResult result = new RequestResult();
            if (model == null || model.Image == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            if (!model.ResourceType.HasValue)
            {
                model.ResourceType = ResourceTypes.MemberImage;
            }

            result.Value = await _dish.SaveDishImageAsync(model);
            _logger.LogResponse("ServiceMember/SaveDishImage", result);
            return Json(result);
        }
    }
}
