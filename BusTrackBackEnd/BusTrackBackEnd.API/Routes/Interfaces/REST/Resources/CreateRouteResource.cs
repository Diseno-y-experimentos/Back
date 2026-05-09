namespace BusTrackBackEnd.API.Routes.Interfaces.REST.Resources;

public record CreateRouteResource(
    string Name,
    int CompanyId,
    int EstimatedTime,
    int Frequency,
    List<WaypointResource>? Waypoints
);
