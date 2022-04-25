using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reservation.Models.Criterias;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;
using System.Threading.Tasks;

namespace Reservation.AdminPanel
{
    public class AdminController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly ILogger<AdminController> _logger;
        private readonly IBankCardService _bankCardService;
        private readonly IBankAccountService _bankAccountService;
        private readonly IServiceMemberService _serviceMemberService;
        private readonly IServiceMemberBranchService _serviceMemberBranchService;

        public AdminController(
            IMemberService memberService,
            ILogger<AdminController> logger,
            IBankCardService bankCardService,
            IBankAccountService bankAccountService,
            IServiceMemberService serviceMemberService,
            IServiceMemberBranchService serviceMemberBranchService)
        {
            _logger = logger;
            _memberService = memberService;
            _bankCardService = bankCardService;
            _bankAccountService = bankAccountService;
            _serviceMemberService = serviceMemberService;
            _serviceMemberBranchService = serviceMemberBranchService;
        }

        [HttpGet]
        public async Task<IActionResult> ServiceMembers([FromQuery]ServiceMemberSearchCriteria criteria)
        {
            _logger.LogRequest("Admin/ServiceMembers", criteria);
            var data = await _serviceMemberService.GetServiceMembersForAdminAsync(criteria ?? new ServiceMemberSearchCriteria());
            _logger.LogResponse("Admin/ServiceMembers", data);
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> ServiceMemberBranches(long? smId)
        {
            _logger.LogRequest("Admin/ServiceMemberBranches", new { ServiceMemberId = smId});
            if (!smId.HasValue)
            {
                return BadRequest();
            }

            var data = await _serviceMemberBranchService.GetServiceMemberBranchesForAdminAsync(smId.Value);
            _logger.LogResponse("Admin/ServiceMemberBranches", data);
            return View(data);
        }
    }
}
