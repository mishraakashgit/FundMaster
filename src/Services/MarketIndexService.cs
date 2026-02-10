using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FundNavTracker.Models;

namespace FundNavTracker.Services
{
    public class MarketIndexService : IIndexService
    {
        public Task<IEnumerable<NiftySensexDto>> GetHeaderIndicesAsync()
        {
            // Placeholder: Replace with a real market index API integration.
            var updatedAt = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm");
            var indices = new List<NiftySensexDto>
            {
                new NiftySensexDto { IndexName = "NIFTY 50", CurrentValue = 22000.50M, Change = 100.25M, PercentChange = 0.46M, LastUpdated = updatedAt },
                new NiftySensexDto { IndexName = "SENSEX", CurrentValue = 73000.75M, Change = 250.10M, PercentChange = 0.34M, LastUpdated = updatedAt }
            };
            return Task.FromResult<IEnumerable<NiftySensexDto>>(indices);
        }
    }
}
