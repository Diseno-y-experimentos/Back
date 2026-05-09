using BusTrackBackEnd.API.Routes.Interfaces.REST.Resources;
using Route = BusTrackBackEnd.API.Routes.Domain.Model.Aggregates.Route;

namespace BusTrackBackEnd.API.Routes.Interfaces.REST.Transform;

public static class RouteResourceFromEntityAssembler
{
    public static RouteResource ToResource(Route entity)
    {
        var waypoints = entity.Waypoints
            .Select(w => new WaypointResource(w.Name, w.Latitude, w.Longitude, w.Order));

        return new RouteResource(entity.Id, entity.Name, entity.CompanyId, entity.EstimatedTime, entity.Frequency, waypoints);
    }
}