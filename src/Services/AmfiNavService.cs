using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FundNavTracker.Services
{
    public class AmfiNavService : INavService
    {
        private readonly HttpClient _httpClient;
        public AmfiNavService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<IEnumerable<FundNavDto>> GetLatestNavsAsync(string? search = null)
        {
            // Placeholder: Replace with actual AMFI API or scraping logic
            // Example: https://www.amfiindia.com/spages/NAVAll.txt
            var navs = new List<FundNavDto>
            {
                new FundNavDto { FundName = "Sample Fund 1", FundCode = "100001", Nav = 123.45M, Date = "2026-02-10" },
                new FundNavDto { FundName = "Sample Fund 2", FundCode = "100002", Nav = 234.56M, Date = "2026-02-10" }
            };
            if (!string.IsNullOrEmpty(search))
            {
                navs = navs.FindAll(f => f.FundName.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            return Task.FromResult<IEnumerable<FundNavDto>>(navs);
        }

        public Task<FundNavDto> GetNavByFundCodeAsync(string fundCode)
        {
            // Placeholder: Replace with actual lookup logic
            var nav = new FundNavDto { FundName = "Sample Fund 1", FundCode = fundCode, Nav = 123.45M, Date = "2026-02-10" };
            return Task.FromResult(nav);
        }
    }
}
