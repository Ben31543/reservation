﻿using Microsoft.AspNetCore.Mvc;
using Reservation.Models.BankAccount;
using Reservation.Models.Common;
using Reservation.Models.Member;
using Reservation.Models.ServiceMember;
using Reservation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Web.Controllers
{
    public class ServiceMemberController : Controller
    {
        private readonly IServiceMemberService _serviceMember;

        public ServiceMemberController(IServiceMemberService serviceMember)
        {
            _serviceMember = serviceMember;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterServiceMember([FromBody] ServiceMemberRegistrationModel model)
        {
            RequestResult result = new RequestResult();
            if (!ModelState.IsValid)
            {
                result.Message = "WrongIncomingParameters";
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
                result.Message = "WrongIncomingParameters";
                return Json(result);
            }

            var serviceMember = await _serviceMember.GetServiceMemberByIdAsync(id.Value);
            if (serviceMember == null)
            {
                result.Message = "ServiceMemberDoesNotExist";
                result.Value = id;
                return Json(result);
            }

            result.Succeeded = true;
            result.Value = serviceMember;
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyServiceMember(MemberSignInModel model)
        {
            RequestResult result = new RequestResult();

            if (!ModelState.IsValid)
            {
                result.Message = "WrongIncomingParameters";
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
        public async Task<IActionResult> ResetPassword([FromBody] MemberResetPasswordModel model)
        {
            RequestResult result = new RequestResult();
            if (!ModelState.IsValid)
            {
                result.Message = "WrongIncomingParameters";
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
                result.Message = "WrongIncomingParameters";
                return Json(result);
            }

            result = await _serviceMember.UpdateServiceMemberInfoAsync(model);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> AttachBankAccount([FromBody]BankAccountAttachModel model)
        {
            RequestResult result = new RequestResult();
            if (!model.ServiceMemberId.HasValue)
            {
                result.Message = "WrongIncomingParameters";
                return Json(result);
            }

            result = await _serviceMember.AddBankAccountAsync(model);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> DetachFromBankAccount([FromQuery]long? serviceMemberId, [FromQuery]long? bankAccountId)
        {
            var result = new RequestResult();
            if (!serviceMemberId.HasValue || !bankAccountId.HasValue)
            {
                result.Message = "WrongIncomingParameters";
                return Json(result);
            }

            result = await _serviceMember.DetachBankAccountAsync(serviceMemberId.Value, bankAccountId.Value);
            return Json(result);
        }
    }
}
