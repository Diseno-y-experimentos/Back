using BusTrackBackEnd.API.Routes.Domain.Model.Queries;
using Route = BusTrackBackEnd.API.Routes.Domain.Model.Aggregates.Route;
namespace BusTrackBackEnd.API.Routes.Domain.Services;

public interface IRouteQueryService
{
    Task<IEnumerable<Route>> Handle(GetAllRoutesQuery query);
    Task<Route?> Handle(GetRouteByIdQuery query);
    Task<IEnumerable<Route>> Handle(GetRoutesByCompanyIdQuery query);
}