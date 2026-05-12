using BusTrackBackEnd.API.BoundedContexts.Users.Domain.Model.Aggregates;
using BusTrackBackEnd.API.BoundedContexts.Users.Interfaces.REST.Resources;
using BusTrackBackEnd.API.Routes.Domain.Repositories;
using BusTrackBackEnd.API.Routes.Interfaces.REST.Transform;
using BusTrackBackEnd.API.Shared.Domain.Repositories;
using BusTrackBackEnd.API.Shared.Infrastructure.Persistence.EFC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Route = BusTrackBackEnd.API.Routes.Domain.Model.Aggregates.Route;

namespace BusTrackBackEnd.API.BoundedContexts.Users.Interfaces.REST;

[ApiController]
[Route("api/v1/users/{userId:int}/saved-routes")]
[Route("users/{userId:int}/saved-routes")]
public class SavedRoutesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IRouteRepository _routeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SavedRoutesController(AppDbContext context, IRouteRepository routeRepository, IUnitOfWork unitOfWork)
    {
        _context = context;
        _routeRepository = routeRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int userId)
    {
        var entries = await _context.Set<SavedRoute>()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        var resources = new List<SavedRouteResource>();
        foreach (var entry in entries)
        {
            resources.Add(await ToResourceAsync(entry));
        }

        return Ok(resources);
    }

    [HttpPost]
    public async Task<IActionResult> Create(int userId, [FromBody] CreateSavedRouteResource resource)
    {
        var route = await _routeRepository.FindByIdAsync(resource.RouteId);
        if (route == null) return NotFound(new { message = "Route not found" });

        var existing = await _context.Set<SavedRoute>()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.RouteId == resource.RouteId);

        if (existing != null)
        {
            return Ok(await ToResourceAsync(existing));
        }

        var savedRoute = new SavedRoute(userId, resource.RouteId);
        await _context.Set<SavedRoute>().AddAsync(savedRoute);
        await _unitOfWork.CompleteAsync();

        return Ok(await ToResourceAsync(savedRoute));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int userId, int id)
    {
        var entry = await _context.Set<SavedRoute>()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (entry == null) return NotFound();

        _context.Set<SavedRoute>().Remove(entry);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    private async Task<SavedRouteResource> ToResourceAsync(SavedRoute entry)
    {
        Route? route = await _routeRepository.FindByIdAsync(entry.RouteId);
        return new SavedRouteResource(
            entry.Id,
            entry.UserId,
            entry.RouteId,
            entry.CreatedAt,
            entry.UpdatedAt,
            route == null ? null : RouteResourceFromEntityAssembler.ToResource(route));
    }
}

