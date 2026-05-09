using BusTrackBackEnd.API.Routes.Domain.Model.Commands;
using BusTrackBackEnd.API.Routes.Interfaces.REST.Resources;

namespace BusTrackBackEnd.API.Routes.Interfaces.REST.Transform;

public static class CreateRouteCommandFromResourceAssembler
{
    public static CreateRouteCommand ToCommand(CreateRouteResource resource)
    {
        var waypoints = (resource.Waypoints ?? new List<WaypointResource>())
            .Select(w => new CreateWaypointCommand(
                w.Name, w.Latitude, w.Longitude, w.Order))
            .ToList();

        return new CreateRouteCommand(
            resource.Name,
            resource.CompanyId,
            resource.EstimatedTime,
            resource.Frequency,
            waypoints
        );
    }
}