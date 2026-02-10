using System.Collections.Generic;
using System.Threading.Tasks;
using FundNavTracker.Models;

namespace FundNavTracker.Services
{
    public interface IIndexService
    {
        Task<IEnumerable<NiftySensexDto>> GetHeaderIndicesAsync();
    }
}
