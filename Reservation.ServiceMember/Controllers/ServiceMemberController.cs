using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Reservation.Models.BankAccount;
using Reservation.Models.Common;
using Reservation.Models.Criterias;
using Reservation.Models.Dish;
using Reservation.Models.ServiceMember;
using Reservation.Resources;
using Reservation.Resources.Constants;
using Reservation.Resources.Contents;
using Reservation.Resources.Controllers;
using Reservation.Resources.Enumerations;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;

namespace Reservation.ServiceMember.Controllers
{
    public class ServiceMemberController : ApplicationUser
    {
        private static object _locker = new object();
        
        private readonly ILogger _logger;
        private readonly IServiceMemberService _serviceMemberService;
        private readonly IServiceMemberBranchService _branchService;
        private readonly IDishService _dishService;
        private readonly IStringLocalizer<ResourcesController> _localizer;

        public ServiceMemberController(
            IServiceMemberService serviceMember,
            ILogger<ServiceMemberController> logger,
            IServiceMemberBranchService branch,
            IDishService dish,
            IStringLocalizer<ResourcesController> localizer)
        {
            _serviceMemberService = serviceMember;
            _logger = logger;
            _branchService = branch;
            _dishService = dish;
            _localizer = localizer;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterServiceMember([FromBody] ServiceMemberRegistrationModel model)
        {
            _logger.LogRequest("ServiceMember/RegisterServiceMember", model);

            RequestResult result = new RequestResult();
            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("ServiceMember/RegisterServiceMember", result);
                return Json(result);
            }
            
            if (!ModelState.IsValid)
            {
                result.Message = _localizer.GetModelsLocalizedErrors(ModelState);
                return Json(result);
            }

            if (!model.Email.IsValidEmail())
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.InvalidEmail);
                return Json(result);
            }

            result = await _serviceMemberService.RegisterServiceMemberAsync(model);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer.GetLocalizationOf(result.Message);
            }

            _logger.LogResponse("ServiceMember/RegisterServiceMember", result);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetServiceMember(long? id)
        {
            _logger.LogRequest("ServiceMember/GetServiceMember", id);

            RequestResult result = new RequestResult();
            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("ServiceMember/GetServiceMember", result);
                return Json(result);
            }
            
            if (id == null)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.WrongIncomingParameters);
                return Json(result);
            }

            var serviceMember = await _serviceMemberService.GetServiceMemberByIdAsync(id.Value);
            if (serviceMember == null)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.WrongIncomingParameters);
                result.Value = id;
                return Json(result);
            }

            if (!string.IsNullOrEmpty(serviceMember.LogoUrl))
            {
                serviceMember.LogoUrl = $"{ImageSaverConstants.ImagesHostingPath}{serviceMember.LogoUrl}";
            }

            result.Succeeded = true;
            result.Value = serviceMember;
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer[result.Message].Value;
            }

            _logger.LogResponse("ServiceMember/GetServiceMember", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyServiceMember([FromBody] SignInModel model)
        {
            _logger.LogRequest("ServiceMember/VerifyServiceMember", model);

            RequestResult result = new RequestResult();
            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("ServiceMember/VerifyServiceMember", result);
                return Json(result);
            }

            if (!ModelState.IsValid)
            {
                result.Message = _localizer.GetModelsLocalizedErrors(ModelState);
                return Json(result);
            }

            result = await _serviceMemberService.VerifyServiceMemberAsync(model);
            if (result.Succeeded)
            {
                Authenticate(model.Login);
            }

            if (!result.Succeeded)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.ServiceMemberDoesNotExist);
                return Json(result);
            }

            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer.GetLocalizationOf(result.Message);
            }

            _logger.LogResponse("ServiceMember/VerifyServiceMember", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            _logger.LogRequest("ServiceMember/ResetPassword", model);

            RequestResult result = new RequestResult();
            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("ServiceMember/ResetPassword", result);
                return Json(result);
            }

            if (!ModelState.IsValid)
            {
                result.Message = _localizer.GetModelsLocalizedErrors(ModelState);
                return Json(result);
            }

            result = await _serviceMemberService.ResetPasswordAsync(model);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer.GetLocalizationOf(result.Message);
            }

            _logger.LogResponse("ServiceMember/ResetPassword", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateServiceMember([FromBody] ServiceMemberEditModel model)
        {
            _logger.LogRequest("ServiceMember/UpdateServiceMember", model);

            RequestResult result = new RequestResult();
            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("ServiceMember/UpdateServiceMember", result);
                return Json(result);
            }

            if (!ModelState.IsValid)
            {
                result.Message = _localizer.GetModelsLocalizedErrors(ModelState);
                return Json(result);
            }

            result = await _serviceMemberService.UpdateServiceMemberInfoAsync(model);
            _logger.LogResponse("ServiceMember/UpdateServiceMember", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> AttachBankAccount([FromBody] BankAccountAttachModel model)
        {
            _logger.LogRequest("ServiceMember/AttachBankAccount", model);

            RequestResult result = new RequestResult();
            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("ServiceMember/AttachBankAccount", result);
                return Json(result);
            }

            if (!ModelState.IsValid)
            {
                result.Message = _localizer.GetModelsLocalizedErrors(ModelState);
                return Json(result);
            }

            if (!model.ServiceMemberId.HasValue)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.WrongIncomingParameters);
                return Json(result);
            }

            result = await _serviceMemberService.AddBankAccountAsync(model);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer.GetLocalizationOf(result.Message);
            }

            _logger.LogResponse("ServiceMember/AttachBankAccount", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> DetachFromBankAccount([FromQuery] long? serviceMemberId, [FromQuery] long? bankAccountId)
        {
            _logger.LogRequest("ServiceMember/DetachFromBankAccount", new { ServiceMemberId = serviceMemberId, BankAccountId = bankAccountId });

            var result = new RequestResult();
            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("ServiceMember/DetachFromBankAccount", result);
                return Json(result);
            }

            if (!serviceMemberId.HasValue || !bankAccountId.HasValue)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.WrongIncomingParameters);
                return Json(result);
            }

            result = await _serviceMemberService.DetachBankAccountAsync(serviceMemberId.Value, bankAccountId.Value);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer.GetLocalizationOf(result.Message);
            }

            _logger.LogResponse("ServiceMember/DetachFromBankAccount", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetServiceMemberDealsHistory(long? serviceMemberId)
        {
            _logger.LogRequest("ServiceMember/GetServiceMemberDealsHistory", serviceMemberId);

            RequestResult result = new RequestResult();
            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("ServiceMember/GetServiceMemberDealsHistory", result);
                return Json(result);
            }

            if (!serviceMemberId.HasValue)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.WrongIncomingParameters);
                return Json(result);
            }

            result.Value = await _serviceMemberService.GetServiceMemberDealsHistoryAsync(serviceMemberId.Value);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer.GetLocalizationOf(result.Message);
            }

            _logger.LogResponse("ServiceMember/GetServiceMemberDealsHistory", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveServiceMemberImage([FromForm] SaveImageModel model)
        {
            _logger.LogRequest("ServiceMember/SaveServiceMemberImage", model);
            var result = new RequestResult();
            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("ServiceMember/SaveServiceMemberImage", result);
                return Json(result);
            }

            if (model == null || model.Image == null)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.WrongIncomingParameters);
                return Json(result);
            }

            if (!model.ResourceType.HasValue)
            {
                model.ResourceType = ResourceTypes.ServiceMemberImage;
            }

            result = await _serviceMemberService.SaveServiceMemberImageAsync(model);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer.GetLocalizationOf(result.Message);
            }

            _logger.LogResponse("ServiceMember/SaveServiceMemberImage", result);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetBranches(long? serviceMemberId)
        {
            _logger.LogRequest("ServiceMember/GetBranches", serviceMemberId);

            RequestResult result = new RequestResult();
            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("ServiceMember/GetBranches", result);
                return Json(result);
            }

            if (serviceMemberId == null)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.WrongIncomingParameters);
                return Json(result);
            }

            result.Value = await _branchService.GetBranchesAsync(serviceMemberId.Value);
            result.Succeeded = true;
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer.GetLocalizationOf(result.Message);
            }

            _logger.LogResponse("ServiceMember/GetBranches", result);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetDish(long? dishId)
        {
            _logger.LogRequest("ServiceMember/GetDish", dishId);

            RequestResult result = new RequestResult();
            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("ServiceMember/GetDish", result);
                return Json(result);
            }

            if (dishId == null)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.WrongIncomingParameters);
                return Json(result);
            }

            result.Value = await _dishService.GetDishById(dishId.Value);
            result.Succeeded = true;
            _logger.LogResponse("ServiceMember/GetDish", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetDishes([FromBody] DishSearchCriteria criteria)
        {
            _logger.LogRequest("ServiceMember/GetDishes", criteria);

            RequestResult result = new RequestResult();
            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("ServiceMember/GetDishes", result);
                return Json(result);
            }

            if (criteria.ServiceMemberId == null)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.WrongIncomingParameters);
                return Json(result);
            }

            result.Value = await _dishService.GetDishesAsync(criteria);
            result.Succeeded = true;
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer.GetLocalizationOf(result.Message);
            }

            _logger.LogResponse("ServiceMember/GetDishes", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveDish([FromBody] DishModel model)
        {
            _logger.LogRequest("ServiceMember/SaveDish", model);

            RequestResult result = new RequestResult();
            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("ServiceMember/SaveDish", result);
                return Json(result);
            }

            if (!ModelState.IsValid)
            {
                result.Message = _localizer.GetModelsLocalizedErrors(ModelState);
                return Json(result);
            }

            result = await _dishService.AddNewDishAsync(model);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer.GetLocalizationOf(result.Message);
            }

            _logger.LogResponse("ServiceMember/SaveDish", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> EditDish([FromBody] DishModel model)
        {
            _logger.LogRequest("ServiceMember/EditDish", model);

            RequestResult result = new RequestResult();
            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("ServiceMember/EditDish", result);
                return Json(result);
            }

            if (!ModelState.IsValid)
            {
                result.Message = _localizer.GetModelsLocalizedErrors(ModelState);
                return Json(result);
            }

            result = await _dishService.EditDishAsync(model);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer.GetLocalizationOf(result.Message);
            }

            _logger.LogResponse("ServiceMember/EditDish", result);
            return Json(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDish(long? dishId)
        {
            _logger.LogRequest("ServiceMember/DeleteDish", dishId);

            RequestResult result = new RequestResult();
            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("ServiceMember/DeleteDish", result);
                return Json(result);
            }

            if (dishId == null)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.WrongIncomingParameters);
                return Json(result);
            }

            result = await _dishService.DeleteDishAsync(dishId.Value);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer.GetLocalizationOf(result.Message);
            }

            _logger.LogResponse("ServiceMember/DeleteDish", result);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveDishImage([FromForm] SaveImageModel model)
        {
            _logger.LogRequest("ServiceMember/SaveDishImage", model);

            RequestResult result = new RequestResult();
            if (!IsAuthorized)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberIsNotLoggedIn);
                _logger.LogResponse("ServiceMember/SaveDishImage", result);
                return Json(result);
            }

            if (model == null || model.Image == null)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.WrongIncomingParameters);
                return Json(result);
            }

            if (!model.ResourceType.HasValue)
            {
                model.ResourceType = ResourceTypes.DishImage;
            }

            result = await _dishService.SaveDishImageAsync(model);
            if (!string.IsNullOrEmpty(result.Message))
            {
                result.Message = _localizer.GetLocalizationOf(result.Message);
            }

            _logger.LogResponse("ServiceMember/SaveDishImage", result);
            return Json(result);
        }

        public async Task<IActionResult> Logout()
        {
            LogoutServiceMember();
            return Json(new RequestResult
            {
                Succeeded = true
            });
        }
    }
}
