using FundNavTracker.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FundNavTracker.Controllers
{
    public class NavController : Controller
    {
        private readonly INavService _navService;
        public NavController(INavService navService)
        {
            _navService = navService;
        }

        public async Task<IActionResult> Index(string search)
        {
            var navs = await _navService.GetLatestNavsAsync(search);
            return View(navs);
        }
    }
}
