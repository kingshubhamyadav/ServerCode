﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnDemandCarWash.Dtos;
using OnDemandCarWash.Models;
using OnDemandCarWash.Services;

namespace OnDemandCarWash.Context
{
    [Route("api/[controller]")]
    [ApiController]
    public class WasherController : ControllerBase
    {
        private readonly WasherService _service;
        public WasherController(WasherService service, IMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WasherProfileDto>> GetWasherById(int id)
        {
            var res = await _service.GetWasherAsync(id);
            if(res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateWasher(int id,WasherProfileDto washer)
        {

            var res = await _service.UpdateWasherAsync(id, washer);
            if(res == null)
            {
                return NotFound();
            }
            return Ok("Success!");

        }
    }
}
