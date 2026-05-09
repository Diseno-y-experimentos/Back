using BusTrackBackEnd.API.Routes.Domain.Model.Commands;
using BusTrackBackEnd.API.Routes.Domain.Model.Entities;
using BusTrackBackEnd.API.Routes.Domain.Repositories;
using BusTrackBackEnd.API.Routes.Domain.Services;
using BusTrackBackEnd.API.Shared.Domain.Repositories;
using Route = BusTrackBackEnd.API.Routes.Domain.Model.Aggregates.Route;

namespace BusTrackBackEnd.API.Routes.Application.Internal.CommandServices;

public class RouteCommandService : IRouteCommandService
{
    private readonly IRouteRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RouteCommandService(IRouteRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateRouteCommand command)
    {
        var route = new Route(
            command.Name, 
            command.CompanyId, 
            command.EstimatedTime, 
            command.Frequency
        );

        foreach (var w in command.Waypoints)
        {
            var waypoint = new Waypoint(w.Name, w.Latitude, w.Longitude, w.Order);
            route.AddWaypoint(waypoint);
        }

        await _repository.AddAsync(route);
        await _unitOfWork.CompleteAsync();

        return route.Id;
    }
}