using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reservation.Models.Common;
using Reservation.Models.Criterias;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;
using System.Threading.Tasks;

namespace Reservation.AdminPanel
{
    public class AdminController : Controller
    {
        private static object _locker = new object();
        private static bool IsAuthorized = false;

        private readonly IAdminService _adminService;
        private readonly IMemberService _memberService;
        private readonly ILogger<AdminController> _logger;
        private readonly IBankCardService _bankCardService;
        private readonly IBankAccountService _bankAccountService;
        private readonly IServiceMemberService _serviceMemberService;
        private readonly IServiceMemberBranchService _serviceMemberBranchService;

        public AdminController(
            IAdminService adminService,
            IMemberService memberService,
            ILogger<AdminController> logger,
            IBankCardService bankCardService,
            IBankAccountService bankAccountService,
            IServiceMemberService serviceMemberService,
            IServiceMemberBranchService serviceMemberBranchService)
        {
            _logger = logger;
            _adminService = adminService;
            _memberService = memberService;
            _bankCardService = bankCardService;
            _bankAccountService = bankAccountService;
            _serviceMemberService = serviceMemberService;
            _serviceMemberBranchService = serviceMemberBranchService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            _logger.LogRequest("Admin/Logout", null);

            lock (_locker)
            {
                IsAuthorized = false;
            }

            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> VerifyAdmin([FromForm]SignInModel model)
        {
            _logger.LogRequest("Admin/VerifyAdmin", null);
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = ModelState.GetErrorMessages();
                _logger.LogResponse("Admin/VerifyAdmin", new RequestResult
                {
                    Message = ModelState.GetErrorMessages()
                });
                return View("Login");
            }

            var verifyResult = await _adminService.VerifyAdminAsync(model.Login, model.Password);
            if (!verifyResult.Succeeded)
            {
                ViewBag.ErrorMessage = verifyResult.Message;
                _logger.LogResponse("Admin/VerifyAdmin", verifyResult);
                return View("Login");
            }

            lock (_locker)
            {
                IsAuthorized = true;
            }

            _logger.LogResponse("Admin/VerifyAdmin", verifyResult);
            return RedirectToAction("ServiceMembers");
        }

        [HttpGet]
        public async Task<IActionResult> ServiceMembers([FromQuery]ServiceMemberSearchCriteria criteria)
        {
            _logger.LogRequest("Admin/ServiceMembers", criteria);

            if (!IsAuthorized)
            {
                return RedirectToAction("Login");
            }

            var data = await _serviceMemberService.GetServiceMembersForAdminAsync(criteria ?? new ServiceMemberSearchCriteria());
            _logger.LogResponse("Admin/ServiceMembers", data);
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> ServiceMemberBranches(long? smId)
        {
            _logger.LogRequest("Admin/ServiceMemberBranches", new { ServiceMemberId = smId});
            
            if (!IsAuthorized)
            {
                return RedirectToAction("Login");
            }

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
