using Microsoft.AspNetCore.Mvc;
using Reservation.Models.BankCard;
using Reservation.Models.Common;
using Reservation.Models.Member;
using Reservation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Web.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _member;
        private readonly IBankCardService _bankCard;

        public MemberController(IMemberService member, IBankCardService bankCard)
        {
            _member = member;
            _bankCard = bankCard;
        }

        [HttpPost]
        public async Task<IActionResult> AddNewMember([FromBody]MemberRegistrationModel model)
        {
            var result = new RequestResult();
            if(!ModelState.IsValid)
            {
                result.Message = "WrongIncomingParameter";
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
                result.Message = "WrongIncomingParameter";
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
                result.Message = "WrongIncomingParameter";
                return Json(result);
            }

            result = await _member.UpdateMemberInfoAsync(model);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetModel model)
        {
            var result = new RequestResult();
            if (!ModelState.IsValid)
            {
                result.Message = "WrongIncomingParameter";
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
                result.Message = "WrongIncomingParameter";
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
                result.Message = "WrongIncomingParameters";
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
                result.Message = "WrongIncomingParameters";
                return Json(result);
            }

            result = await _member.DetachBankCardAsync(memberId.Value, cardId.Value);
            return Json(result);
        }
    }
}
