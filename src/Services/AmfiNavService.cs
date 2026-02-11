using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace FundNavTracker.Services
{
    public class AmfiNavService : INavService
    {
        private readonly HttpClient _httpClient;
        private readonly string _navUrl;

        public AmfiNavService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _navUrl = configuration["DataSources:AmfiNavUrl"] ?? "https://www.amfiindia.com/spages/NAVAll.txt";
        }

        public async Task<IEnumerable<FundNavDto>> GetLatestNavsAsync(string? search = null)
        {
            var navs = await FetchNavsAsync();
            if (!string.IsNullOrWhiteSpace(search))
            {
                navs = navs
                    .Where(f => f.FundName.Contains(search, StringComparison.OrdinalIgnoreCase)
                        || f.FundCode.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            return navs;
        }

        public async Task<FundNavDto?> GetNavByFundCodeAsync(string fundCode)
        {
            var navs = await FetchNavsAsync();
            return navs.FirstOrDefault(f => string.Equals(f.FundCode, fundCode, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<List<FundNavDto>> FetchNavsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_navUrl);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return ParseAmfiNavData(content);
            }
            catch
            {
                return new List<FundNavDto>();
            }
        }

        private static List<FundNavDto> ParseAmfiNavData(string content)
        {
            var navs = new List<FundNavDto>();
            var lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                if (!line.Contains(';'))
                {
                    continue;
                }

                var parts = line.Split(';');
                if (parts.Length < 6)
                {
                    continue;
                }

                var schemeCode = parts[0].Trim();
                var schemeName = parts[3].Trim();
                var navText = parts[4].Trim();
                var dateText = parts[5].Trim();

                if (!int.TryParse(schemeCode, out _))
                {
                    continue;
                }

                if (!decimal.TryParse(navText, NumberStyles.Any, CultureInfo.InvariantCulture, out var nav))
                {
                    continue;
                }

                var date = dateText;
                if (DateTime.TryParseExact(dateText, "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                {
                    date = parsedDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                }

                navs.Add(new FundNavDto
                {
                    FundCode = schemeCode,
                    FundName = schemeName,
                    Nav = nav,
                    Date = date
                });
            }

            return navs;
        }
    }
}
