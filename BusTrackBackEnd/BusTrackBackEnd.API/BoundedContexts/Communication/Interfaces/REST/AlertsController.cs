using BusTrackBackEnd.API.BoundedContexts.Communication.Domain.Model.Aggregates;
using BusTrackBackEnd.API.BoundedContexts.Communication.Interfaces.REST.Resources;
using BusTrackBackEnd.API.Shared.Domain.Repositories;
using BusTrackBackEnd.API.Shared.Infrastructure.Persistence.EFC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusTrackBackEnd.API.BoundedContexts.Communication.Interfaces.REST;

[ApiController]
[Route("api/v1/alerts")]
[Route("alerts")]
public class AlertsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public AlertsController(AppDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok((await _context.Set<Alert>().OrderByDescending(x => x.CreatedAt).ToListAsync()).Select(ToResource));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var alert = await _context.Set<Alert>().FindAsync(id);
        if (alert == null) return NotFound();
        return Ok(ToResource(alert));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAlertResource resource)
    {
        var alert = new Alert(resource.Title, resource.Message, resource.IsRead);
        await _context.Set<Alert>().AddAsync(alert);
        await _unitOfWork.CompleteAsync();
        return Ok(ToResource(alert));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAlertResource resource)
    {
        var alert = await _context.Set<Alert>().FindAsync(id);
        if (alert == null) return NotFound();

        alert.Update(resource.Title, resource.Message, resource.IsRead);
        _context.Set<Alert>().Update(alert);
        await _unitOfWork.CompleteAsync();
        return Ok(ToResource(alert));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var alert = await _context.Set<Alert>().FindAsync(id);
        if (alert == null) return NotFound();

        _context.Set<Alert>().Remove(alert);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    private static AlertResource ToResource(Alert alert)
        => new(alert.Id, alert.Title, alert.Message, alert.IsRead, alert.CreatedAt, alert.UpdatedAt);
}

