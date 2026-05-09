using BusTrackBackEnd.API.Routes.Domain.Model.Queries;
using BusTrackBackEnd.API.Routes.Domain.Repositories;
using BusTrackBackEnd.API.Routes.Domain.Services;
using Route = BusTrackBackEnd.API.Routes.Domain.Model.Aggregates.Route;
namespace BusTrackBackEnd.API.Routes.Application.Internal.QueryServices;

public class RouteQueryService : IRouteQueryService
{
    private readonly IRouteRepository _repository;

    public RouteQueryService(IRouteRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Route>> Handle(GetAllRoutesQuery query)
        => _repository.ListAsync();

    public Task<Route?> Handle(GetRouteByIdQuery query)
        => _repository.FindByIdAsync(query.Id);

    public Task<IEnumerable<Route>> Handle(GetRoutesByCompanyIdQuery query)
        => _repository.ListByCompanyIdAsync(query.CompanyId);
}