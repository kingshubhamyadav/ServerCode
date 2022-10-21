using Microsoft.AspNetCore.Mvc;
using OnDemandCarWash.Dtos;
using OnDemandCarWash.Models;

namespace OnDemandCarWash.Repositories
{
    public interface IWasherRepository
    {
        Task<bool> WasherExistsAsync(int id);
        Task<ActionResult<WasherProfileDto>> GetWasherAsync(int id);
        Task<ActionResult<User>> UpdateWasherAsync(int id, WasherProfileDto washer);
        Task<IEnumerable<Order>> GetWasherRequestsAsync();
        Task<ActionResult<AfterWash>> AddAfterWashAsync(AfterWashDto request);
        Task<IEnumerable<Order>> GetCurrentOrdersAsync();
        Task<IEnumerable<Order>> GetPastOrdersAsync();
    }
}
