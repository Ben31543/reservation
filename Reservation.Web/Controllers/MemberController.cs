using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reservation.Data.Enumerations;
using Reservation.Models.BankCard;
using Reservation.Models.Common;
using Reservation.Models.Member;
using Reservation.Resources.Contents;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;
using System;
using System.Threading.Tasks;

namespace Reservation.Web.Controllers
{
	public class MemberController : Controller
	{
		private readonly IMemberService _member;
		private readonly ILogger _logger;

		public MemberController(
			IMemberService member,
			ILogger<MemberController> logger)
		{
			_member = member;
			_logger = logger;
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
				result.Message = LocalizationKeys.ErrorMessages.InvalidPhoneNumber;
				return Json(result);
			}

			if (!model.Email.IsValidEmail())
			{
				result.Message = LocalizationKeys.ErrorMessages.InvalidEmail;
				return Json(result);
			}

			if (!model.Password.Equals(model.ConfirmPassword, StringComparison.Ordinal))
			{
				result.Message = LocalizationKeys.ErrorMessages.PasswordDoNotMatch;
				return Json(result);
			}

			result = await _member.AddNewMemberAsync(model);
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
				result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
				return Json(result);
			}

			result.Value = await _member.GetMemberByIdAsync(id.Value);
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
				result.Message = LocalizationKeys.ErrorMessages.InvalidPhoneNumber;
				return Json(result);
			}

			if (!model.Email.IsValidEmail())
			{
				result.Message = LocalizationKeys.ErrorMessages.InvalidEmail;
				return Json(result);
			}

			result = await _member.UpdateMemberInfoAsync(model);
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
				result.Message = LocalizationKeys.ErrorMessages.PasswordDoNotMatch;
				return Json(result);
			}

			result = await _member.ResetPasswordAsync(model);
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

			result = await _member.VerifyMemberAsync(model);
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

			result = await _member.AddBankCardAsync(model);
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
				result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
				return Json(result);
			}

			result = await _member.DetachBankCardAsync(memberId.Value, cardId.Value);
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
				result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
				return Json(result);
			}

			result.Value = await _member.GetMemberDealsHistoryAsync(memberId.Value);
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
				result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
				return Json(result);
			}

			if (!model.ResourceType.HasValue)
			{
				model.ResourceType = ResourceTypes.MemberImage;
			}

			result = await _member.SaveMemberProfileImageAsync(model);
			_logger.LogResponse("Member/SaveMemberProfilePicture", result);
			return Json(result);
		}
	}
}
