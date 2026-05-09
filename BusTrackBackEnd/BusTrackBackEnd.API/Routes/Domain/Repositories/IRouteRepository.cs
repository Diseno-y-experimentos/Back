using System.Collections.Generic; // Necesario para IEnumerable
using System.Threading.Tasks;     // Necesario para Task
using Route = BusTrackBackEnd.API.Routes.Domain.Model.Aggregates.Route;

namespace BusTrackBackEnd.API.Routes.Domain.Repositories;

public interface IRouteRepository
{
    Task AddAsync(Route route);
    
    Task<Route?> FindByIdAsync(int id);
    
    Task<Route?> FindByNameAsync(string name);
    
    Task<IEnumerable<Route>> ListAsync();
    
    Task<IEnumerable<Route>> ListByCompanyIdAsync(int companyId);
    
    void Remove(Route route);
}