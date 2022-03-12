using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Reservation.Data.Enumerations;
using Reservation.Models.BankCard;
using Reservation.Models.Common;
using Reservation.Models.Member;
using Reservation.Models.ServiceMember;
using Reservation.Resources.Contents;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Reservation.Web.Controllers
{
	public class MemberController : Controller
	{
		private readonly IMemberService _member;
		private readonly IHostingEnvironment environment;
		public MemberController(IMemberService member, IHostingEnvironment environment)
		{
			_member = member;
			this.environment = environment;
		}

		[HttpPost]
		public async Task<IActionResult> AddNewMember([FromBody] MemberRegistrationModel model)
		{
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
			return Json(result);
		}

		[HttpGet]
		public async Task<IActionResult> GetMemberById(long? id)
		{
			var result = new RequestResult();

			if (!id.HasValue)
			{
				result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
				return Json(result);
			}

			var member = await _member.GetMemberByIdAsync(id.Value);
			if (!string.IsNullOrEmpty(member.ProfilePictureUrl))
			{
				member.ProfilePictureUrl = $"{environment.WebRootPath}{member.ProfilePictureUrl}";
			}

			result.Succeeded = true;
			return Json(result);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateMemberInfo([FromBody] MemberEditModel model)
		{
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
			return Json(result);
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
		{
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
			return Json(result);
		}

		[HttpPost]
		public async Task<IActionResult> VerifyMember([FromBody] SignInModel model)
		{
			var result = new RequestResult();
			if (!ModelState.IsValid)
			{
				result.Message = ModelState.GetErrorMessages();
				return Json(result);
			}

			result = await _member.VerifyMemberAsync(model);
			return Json(result);
		}

		[HttpPost]
		public async Task<IActionResult> AttachCardToMember([FromBody] AttachCardToMemberModel model)
		{
			var result = new RequestResult();

			if (!ModelState.IsValid)
			{
				result.Message = ModelState.GetErrorMessages();
				return Json(result);
			}

			result = await _member.AddBankCardAsync(model);
			return Json(result);
		}

		[HttpGet]
		public async Task<IActionResult> DetachCardFromMember([FromQuery] long? memberId, [FromQuery] long? cardId)
		{
			var result = new RequestResult();

			if (!cardId.HasValue || !memberId.HasValue)
			{
				result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
				return Json(result);
			}

			result = await _member.DetachBankCardAsync(memberId.Value, cardId.Value);
			return Json(result);
		}

		[HttpPost]
		public async Task<IActionResult> GetMemberDealsHistory(long? memberId)
		{
			RequestResult result = new RequestResult();
			if (!memberId.HasValue)
			{
				result.Message = LocalizationKeys.ErrorMessages.WrongIncomingParameters;
				return Json(result);
			}

			result.Value = await _member.GetMemberDealsHistoryAsync(memberId.Value);
			return Json(result);
		}

		[HttpPost]
		public async Task<IActionResult> SaveMemberProfileImage([FromForm] SaveImageModel model)
		{
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
			return Json(result);
		}
	}
}
