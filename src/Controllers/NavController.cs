using FundNavTracker.Models;
using FundNavTracker.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FundNavTracker.Controllers
{
    public class NavController : Controller
    {
        private readonly INavService _navService;
        private readonly IAlertRepository _alertRepository;

        public NavController(INavService navService, IAlertRepository alertRepository)
        {
            _navService = navService;
            _alertRepository = alertRepository;
        }

        public async Task<IActionResult> Index(string search)
        {
            var navs = await _navService.GetLatestNavsAsync(search);
            return View(navs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAlert(AlertCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["AlertError"] = "Please fill all fields correctly.";
                return RedirectToAction("Index");
            }

            var alert = new NavAlert
            {
                UserEmail = request.UserEmail.Trim(),
                FundCode = request.FundCode.Trim(),
                TargetNav = request.TargetNav,
                Direction = request.Direction
            };

            await _alertRepository.AddAsync(alert);
            TempData["AlertSuccess"] = "Alert created. You will receive an email when it triggers.";
            return RedirectToAction("Index");
        }
    }
}
