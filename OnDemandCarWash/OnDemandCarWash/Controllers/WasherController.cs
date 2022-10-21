﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnDemandCarWash.Dtos;
using OnDemandCarWash.Models;
using OnDemandCarWash.Services;

namespace OnDemandCarWash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            return Ok(res);
        }
        #endregion

        #region Edit-ProfileMethod
        [HttpPut("edit-profile/{id}")]
        public async Task<ActionResult> UpdateWasher(int id, WasherProfileDto washer)
        {

            var res = await _service.UpdateWasherAsync(id, washer);
            if (res == null)
            {
                return NotFound();
            }
            return Ok("Success!");

        }
        #endregion

        #region GetRequestsMethod
        [HttpGet("requests")]
        public async Task<ActionResult<IEnumerable<WasherRequestsDto>>> GetWasherRequestsAsync()
        {
            var res = await _service.GetWasherRequestsAsync();
            if (res == null || !res.Any())
            {
                return NotFound("No requests to display.");
            }
            return Ok(_mapper.Map<IEnumerable<WasherRequestsDto>>(res));
        }
        #endregion

        #region WashCompleteMethod
        [HttpPost("wash-complete")]
        public async Task<ActionResult> AddAfterWashMethod(AfterWashDto request)
        {
            var res = await _service.AddAfterWashAsync(request);
            if (res == null)
            {
                return BadRequest("Failure!");
            }
            return Ok("Success!");
        }
        #endregion

        #region CurrentOrdersMethod
        [HttpGet("current-orders")]
        public async Task<ActionResult<IEnumerable<WasherRequestsDto>>> GetCurrentOrdersAsync()
        {
            var res = await _service.GetCurrentOrdersAsync();
            if (res == null || !res.Any())
            {
                return NotFound("No Orders to display.");
            }
            return Ok(_mapper.Map<WasherRequestsDto>(res));
        }
        #endregion

        #region PastOrdersMethod
        [HttpGet("past-orders")]
        public async Task<ActionResult<IEnumerable<WasherRequestsDto>>> GetPastOrdersAsync()
        {
            var res = await _service.GetPastOrdersAsync();
            if (res == null || !res.Any())
            {
                return NotFound("No Orders to display.");
            }
            return Ok(_mapper.Map<WasherRequestsDto>(res));
        }
        #endregion
    }
}
