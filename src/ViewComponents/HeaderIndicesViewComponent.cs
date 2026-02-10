using System.Threading.Tasks;
using FundNavTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace FundNavTracker.ViewComponents
{
    public class HeaderIndicesViewComponent : ViewComponent
    {
        private readonly IIndexService _indexService;

        public HeaderIndicesViewComponent(IIndexService indexService)
        {
            _indexService = indexService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var indices = await _indexService.GetHeaderIndicesAsync();
            return View(indices);
        }
    }
}
