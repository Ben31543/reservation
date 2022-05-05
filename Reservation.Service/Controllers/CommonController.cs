using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Reservation.Models;
using Reservation.Models.Common;
using Reservation.Resources.Constants;
using Reservation.Resources.Contents;
using Reservation.Resources.Controllers;
using Reservation.Resources.Enumerations;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace Reservation.Service.Controllers
{
    public class CommonController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IStringLocalizer<ResourcesController> _localizer;

        public CommonController(IMemberService memberService, IStringLocalizer<ResourcesController> localizer)
        {
            _memberService = memberService;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<List<SelectListItem>> GetPersonsEnumValues()
        {
            var statuses = Enum.GetValues(typeof(TableSchemas)).Cast<TableSchemas>();
            return statuses.Select(x => new SelectListItem
            {
                Text = _localizer[x.ToString()].Value,
                Value = ((int) x).ToString()
            }).ToList();
        }

        [HttpGet]
        public async Task ChangeLanguage([FromQuery] string language)
        {
            if (string.IsNullOrEmpty(language))
            {
                return;
            }

            if (!CommonConstants.SupportedLanguages.Contains(language))
            {
                return;
            }

            Response.Cookies.Delete(CookieRequestCultureProvider.DefaultCookieName);
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(language)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1)
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> SendEmailToReservationService([FromBody] SendEmailModel model)
        {
            var result = new RequestResult();

            if (!ModelState.IsValid)
            {
                result.Message = _localizer.GetModelsLocalizedErrors(ModelState);
                return Json(result);
            }

            if (!model.EmailFrom.IsValidEmail())
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.InvalidEmail);
                return Json(result);
            }

            var member = await _memberService.GetMemberByEmailAsync(model.EmailFrom);
            if (member == null)
            {
                result.Message = _localizer.GetLocalizationOf(LocalizationKeys.Errors.MemberDoesNotExist);
                return Json(result);
            }
            
            await EmailSenderService.SendEmailFromUserAsync(model.EmailFrom, member.Id.ToString(), model.Message);
            result.Succeeded = true;
            return Json(result);
        }
    }
}