using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnDemandCarWash.Dtos;
using OnDemandCarWash.Models;
using OnDemandCarWash.Services;
using System.Data;

namespace OnDemandCarWash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Washer")]
    public class WasherController : ControllerBase
    {
        private readonly WasherService _service;
        private readonly IMapper _mapper;
        public WasherController(WasherService service, IMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        #region View-ProfileMethod
        [HttpGet("view-profile/{id}")]
        public async Task<ActionResult<WasherProfileDto>> GetWasherById(int id)
        {
            var res = await _service.GetWasherAsync(id);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res.Value);
        }
        #endregion

        #region Edit-ProfileMethod
        [HttpPut("edit-profile/{id}")]
        [Authorize(Roles = "Washer")]
        public async Task<ActionResult<WasherProfileDto>> UpdateWasher(int id, WasherProfileDto washer)
        {

            var res = await _service.UpdateWasherAsync(id, washer);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res.Value);

        }
        #endregion

        #region GetRequestsMethod
        [HttpGet("requests")]
        [Authorize(Roles = "Washer")]
        public async Task<ActionResult<IEnumerable<WasherRequestsDto>>> GetWasherRequestsAsync()
        {
            var res = await _service.GetWasherRequestsAsync();
            if (res == null || !res.Any())
            {
                return NotFound("No requests to display.");
            }
            return Ok(res);
        }
        #endregion

        #region WashCompleteMethod
        [HttpPost("wash-complete")]
        [Authorize(Roles = "Washer")]
        public async Task<ActionResult> AddAfterWashMethod(AfterWashDto request)
        {
            var res = await _service.AddAfterWashAsync(request);
            if (res == null)
            {
                return BadRequest("Failure!");
            }
            return Ok(res);
        }
        #endregion

        #region CurrentOrdersMethod
        [HttpGet("current-orders/{id}")]
        [Authorize(Roles = "Washer")]
        public async Task<ActionResult<IEnumerable<WasherRequestsDto>>> GetCurrentOrdersAsync(int id)
        {
            var res = await _service.GetCurrentOrdersAsync(id);
            if (res == null || !res.Any())
            {
                return NotFound("No Orders to display.");
            }
            return Ok(res);
        }
        #endregion

        #region PastOrdersMethod
        [HttpGet("past-orders/{id}")]
        [Authorize(Roles = "Washer")]
        public async Task<ActionResult<IEnumerable<WasherRequestsDto>>> GetPastOrdersAsync(int id)
        {
            var res = await _service.GetPastOrdersAsync(id);
            if (res == null || !res.Any())
            {
                return NotFound("No Orders to display.");
            }
            return Ok(res);
        }
        #endregion

        #region InvoiceDetailsMethod
        [HttpGet("invoice-details/{id}")]
        [Authorize(Roles = "Washer")]
        public async Task<ActionResult<IEnumerable<SendInvoiceDto>>> GetInvoiceDetailsAsync(int id)
        {
            var res = await _service.GetInvoiceDetailsAsync(id);
            if(res == null || !res.Any())
            {
                return NotFound("No invoice to display.");
            }
            return Ok(res);
        }
        #endregion

        #region AddOrderToWasherMethod
        [HttpPost("accept-request")]
        [Authorize(Roles = "Washer")]
        public async Task<ActionResult> AcceptRequestAsync(AcceptRequestDto request)
        {
            var res = await _service.AcceptRequestAsync(request);
            if (res == null)
            {
                return BadRequest("Failure!");
            }
            return Ok(res);
        }
        #endregion

        #region ProfileImageUploadMethod
        [HttpPost("upload-profile-img")]
        [Authorize(Roles = "Washer")]
        public async Task<ActionResult> ProfileImageUploadAsync(ImageDto request)
        {
            var res = await _service.ProfileImageUploadAsync(request);
            if(res == null)
            {
                return BadRequest("Failure!");
            }
            return Ok(res);
        }
        #endregion
    }
}
