namespace BusTrackBackEnd.API.Routes.Interfaces.REST.Resources;

public record RouteResource(
    int Id,
    string Name,
    int CompanyId,
    int EstimatedTime,
    int Frequency,
    IEnumerable<WaypointResource> Waypoints
    );