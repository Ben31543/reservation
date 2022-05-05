using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Reservation.Resources.Enumerations;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace Reservation.Resources.Controllers
{
    public class CommonController: Controller
    {
        private readonly IStringLocalizer<ResourcesController> _localizer;

        public CommonController(IStringLocalizer<ResourcesController> localizer)
        {
            _localizer = localizer;
        }
        
        [HttpGet]
        public async Task<List<SelectListItem>> GetPersonsEnumValues()
        {
            var statuses = Enum.GetValues(typeof(TableSchemas)).Cast<TableSchemas>();
            return statuses.Select(x => new SelectListItem
            {
                Text = _localizer[x.ToString()].Value,
                Value = ((int)x).ToString()
            }).ToList();
        }
    }
}