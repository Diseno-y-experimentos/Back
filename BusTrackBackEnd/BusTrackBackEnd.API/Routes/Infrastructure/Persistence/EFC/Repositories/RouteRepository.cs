using BusTrackBackEnd.API.Routes.Domain.Repositories;
using BusTrackBackEnd.API.Shared.Infrastructure.Persistence.EFC;
using Microsoft.EntityFrameworkCore;
using Route = BusTrackBackEnd.API.Routes.Domain.Model.Aggregates.Route;
namespace BusTrackBackEnd.API.Routes.Infrastructure.Persistence.EFC.Repositories;

public class RouteRepository : IRouteRepository
{
    private readonly AppDbContext _context;

    public RouteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Route route)
    {
        await _context.Set<Route>().AddAsync(route);
    }

    public async Task<Route?> FindByIdAsync(int id)
    {
        return await _context.Set<Route>()
            .Include(r => r.Waypoints)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Route?> FindByNameAsync(string name)
    {
        return await _context.Set<Route>()
            .Include(r => r.Waypoints)
            .FirstOrDefaultAsync(r => r.Name.ToLower() == name.ToLower());
    }

    public async Task<IEnumerable<Route>> ListAsync()
    {
        return await _context.Set<Route>()
            .Include(r => r.Waypoints)
            .ToListAsync();
    }

    public async Task<IEnumerable<Route>> ListByCompanyIdAsync(int companyId)
    {
        return await _context.Set<Route>()
            .Include(r => r.Waypoints)
            .Where(r => r.CompanyId == companyId)
            .ToListAsync();
    }

    public void Remove(Route route)
    {
        _context.Set<Route>().Remove(route);
    }
}