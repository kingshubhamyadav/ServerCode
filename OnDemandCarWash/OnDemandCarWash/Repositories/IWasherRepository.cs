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
    }
}
