using BusTrackBackEnd.API.BoundedContexts.Users.Domain.Model.Aggregates;
using BusTrackBackEnd.API.BoundedContexts.Users.Interfaces.REST.Resources;
using BusTrackBackEnd.API.Shared.Domain.Repositories;
using BusTrackBackEnd.API.Shared.Infrastructure.Persistence.EFC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusTrackBackEnd.API.BoundedContexts.Users.Interfaces.REST;

[ApiController]
[Route("api/v1/users/{userId:int}/trips")]
[Route("users/{userId:int}/trips")]
public class TripsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public TripsController(AppDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int userId)
    {
        var trips = await _context.Set<Trip>()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return Ok(trips.Select(ToResource));
    }

    [HttpPost]
    public async Task<IActionResult> Create(int userId, [FromBody] CreateTripResource resource)
    {
        var trip = new Trip(userId, resource.RouteId, resource.Origin, resource.Destination, resource.StartedAt, resource.EndedAt, resource.Notes);
        await _context.Set<Trip>().AddAsync(trip);
        await _unitOfWork.CompleteAsync();

        return Ok(ToResource(trip));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int userId, int id)
    {
        var trip = await _context.Set<Trip>().FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        if (trip == null) return NotFound();

        _context.Set<Trip>().Remove(trip);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    private static TripResource ToResource(Trip trip)
        => new(trip.Id, trip.UserId, trip.RouteId, trip.Origin, trip.Destination, trip.StartedAt, trip.EndedAt, trip.Notes, trip.CreatedAt, trip.UpdatedAt);
}

