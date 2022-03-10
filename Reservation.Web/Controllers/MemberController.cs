using Microsoft.AspNetCore.Mvc;
using Reservation.Models.BankCard;
using Reservation.Models.Common;
using Reservation.Models.Member;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;
using System.Threading.Tasks;

namespace Reservation.Web.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _member;
        public MemberController(IMemberService member)
        {
            _member = member;
        }

        [HttpPost]
        public async Task<IActionResult> AddNewMember([FromBody]MemberRegistrationModel model)
        {
            var result = new RequestResult();
            if(!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages();
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
                result.Message = ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result.Value = await _member.GetMemberByIdAsync(id.Value);
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

            result = await _member.ResetPasswordAsync(model);
            return Json(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> VerifyMember([FromBody]SignInModel model)
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
        public async Task<IActionResult> AttachCardToMember([FromBody]AttachCardToMemberModel model)
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
        public async Task<IActionResult> DetachCardFromMember([FromQuery] long? memberId, [FromQuery]long? cardId)
        {
            var result = new RequestResult();

            if (!cardId.HasValue || !memberId.HasValue)
            {
                result.Message = ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result = await _member.DetachBankCardAsync(memberId.Value, cardId.Value);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> MembersDealsHistory(long? memberId)
        {
            RequestResult result = new RequestResult();
            if (!memberId.HasValue)
            {
                result.Message = ErrorMessages.WrongIncomingParameters;
                return Json(result);
            }

            result.Value = await _member.GetMemberDealsHistoryAsync(memberId.Value);
            return (Json(result));
        }
    }
}
