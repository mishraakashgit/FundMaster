using System.Collections.Generic;
using System.Threading.Tasks;
using FundNavTracker.Models;

namespace FundNavTracker.Services
{
    public interface IAlertRepository
    {
        Task AddAsync(NavAlert alert);
        Task<List<NavAlert>> GetActiveAsync();
        Task UpdateAsync(NavAlert alert);
    }
}
