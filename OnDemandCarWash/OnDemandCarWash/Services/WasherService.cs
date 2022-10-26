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

        public async Task<ActionResult<WasherProfileDto>> UpdateWasherAsync(int id, WasherProfileDto washer)
        {
            if (!await _repo.WasherExistsAsync(id))
            {
                return null;
            }
            return await _repo.UpdateWasherAsync(id, washer);
        }

        public async Task<IEnumerable<WasherRequestsDto>> GetWasherRequestsAsync()
        {
            return await _repo.GetWasherRequestsAsync();
        }

        public async Task<ActionResult<AfterWash>> AddAfterWashAsync(AfterWashDto request)
        {
            return await _repo.AddAfterWashAsync(request);
        }

        public async Task<IEnumerable<WasherRequestsDto>> GetCurrentOrdersAsync()
        {
            return await _repo.GetCurrentOrdersAsync();
        }

        public async Task<IEnumerable<WasherRequestsDto>> GetPastOrdersAsync()
        {
            return await _repo.GetPastOrdersAsync();
        }

        public async Task<IEnumerable<SendInvoiceDto>> GetInvoiceDetailsAsync()
        {
            return await _repo.GetInvoiceDetailsAsync();
        }

        public async Task<ActionResult<Order>> AcceptRequestAsync(AcceptRequestDto request)
        {
            return await _repo.AcceptRequestAsync(request);
        }
    }
}
