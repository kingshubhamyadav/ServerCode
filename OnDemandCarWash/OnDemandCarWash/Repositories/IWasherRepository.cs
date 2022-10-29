using Microsoft.AspNetCore.Mvc;
using OnDemandCarWash.Dtos;
using OnDemandCarWash.Models;

namespace OnDemandCarWash.Repositories
{
    public interface IWasherRepository
    {
        Task<bool> WasherExistsAsync(int id);
        Task<ActionResult<WasherProfileDto>> GetWasherAsync(int id);
        Task<ActionResult<WasherProfileDto>> UpdateWasherAsync(int id, WasherProfileDto washer);
        Task<IEnumerable<WasherRequestsDto>> GetWasherRequestsAsync();
        Task<ActionResult<AfterWash>> AddAfterWashAsync(AfterWashDto request);
        Task<IEnumerable<WasherRequestsDto>> GetCurrentOrdersAsync(int id);
        Task<IEnumerable<WasherRequestsDto>> GetPastOrdersAsync(int id);
        Task<IEnumerable<SendInvoiceDto>> GetInvoiceDetailsAsync(int id);
        Task<ActionResult<Order>> AcceptRequestAsync(AcceptRequestDto request);
        Task<ActionResult<User>> ProfileImageUploadAsync(ImageDto request);
    }
}
