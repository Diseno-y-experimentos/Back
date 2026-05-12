using BusTrackBackEnd.API.BoundedContexts.Communication.Domain.Model.Aggregates;
using BusTrackBackEnd.API.BoundedContexts.Communication.Interfaces.REST.Resources;
using BusTrackBackEnd.API.Shared.Domain.Repositories;
using BusTrackBackEnd.API.Shared.Infrastructure.Persistence.EFC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusTrackBackEnd.API.BoundedContexts.Communication.Interfaces.REST;

[ApiController]
[Route("api/v1/notifications")]
[Route("notifications")]
public class NotificationsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public NotificationsController(AppDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok((await _context.Set<Notification>().OrderByDescending(x => x.CreatedAt).ToListAsync()).Select(ToResource));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var notification = await _context.Set<Notification>().FindAsync(id);
        if (notification == null) return NotFound();
        return Ok(ToResource(notification));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNotificationResource resource)
    {
        var notification = new Notification(resource.Title, resource.Message, resource.IsRead);
        await _context.Set<Notification>().AddAsync(notification);
        await _unitOfWork.CompleteAsync();
        return Ok(ToResource(notification));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateNotificationResource resource)
    {
        var notification = await _context.Set<Notification>().FindAsync(id);
        if (notification == null) return NotFound();

        notification.Update(resource.Title, resource.Message, resource.IsRead);
        _context.Set<Notification>().Update(notification);
        await _unitOfWork.CompleteAsync();
        return Ok(ToResource(notification));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var notification = await _context.Set<Notification>().FindAsync(id);
        if (notification == null) return NotFound();

        _context.Set<Notification>().Remove(notification);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    private static NotificationResource ToResource(Notification notification)
        => new(notification.Id, notification.Title, notification.Message, notification.IsRead, notification.CreatedAt, notification.UpdatedAt);
}

