using System.Collections.Generic;
using System.Threading.Tasks;

namespace FundNavTracker.Services
{
    public interface INavService
    {
        Task<IEnumerable<FundNavDto>> GetLatestNavsAsync(string? search = null);
        Task<FundNavDto?> GetNavByFundCodeAsync(string fundCode);
    }

    public class FundNavDto
    {
        public required string FundName { get; set; }
        public required string FundCode { get; set; }
        public decimal Nav { get; set; }
        public required string Date { get; set; }
    }
}
