using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusTrackBackEnd.API.BoundedContexts.Transport.Application.Internal.DTOs;
using BusTrackBackEnd.API.BoundedContexts.Transport.Application.Internal.Services;

namespace BusTrackBackEnd.API.BoundedContexts.Transport.Interfaces.REST
{
    [ApiController]
    [Route("api/v1/vehicles")]
    public class BusesController : ControllerBase
    {
        private readonly IBusService _busService;

        public BusesController(IBusService busService)
        {
            _busService = busService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBuses()
        {
            var buses = await _busService.GetAllAsync();
            return Ok(buses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBusById(int id)
        {
            var bus = await _busService.GetByIdAsync(id);
            if (bus == null)
            {
                return NotFound();
            }
            return Ok(bus);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBus([FromBody] CreateBusResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var bus = await _busService.CreateAsync(resource);
                // Return 201 Created and the location header
                return CreatedAtAction(nameof(GetBusById), new { id = bus.Id }, bus);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
