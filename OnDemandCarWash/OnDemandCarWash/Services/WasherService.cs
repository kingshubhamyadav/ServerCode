using Microsoft.AspNetCore.Mvc;
using OnDemandCarWash.Dtos;
using OnDemandCarWash.Models;
using OnDemandCarWash.Repositories;

namespace OnDemandCarWash.Services
{
    public class WasherService
    {
        private readonly IWasherRepository _repo;
        public WasherService(IWasherRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<ActionResult<WasherProfileDto>> GetWasherAsync(int id)
        {
            return await _repo.GetWasherAsync(id);
        }

        public async Task<ActionResult<User>> UpdateWasherAsync(int id, WasherProfileDto washer)
        {
            if (!await _repo.WasherExistsAsync(id))
            {
                return null;
            }
            return await _repo.UpdateWasherAsync(id, washer);
        }

        public async Task<IEnumerable<Order>> GetWasherRequestsAsync()
        {
            return await _repo.GetWasherRequestsAsync();
        }
    }
}
