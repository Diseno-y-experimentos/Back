namespace BusTrackBackEnd.API.Routes.Domain.Model.Commands;

public record CreateRouteCommand(
    string Name,
    int CompanyId,
    int EstimatedTime,
    int Frequency,
    List<CreateWaypointCommand> Waypoints
);

public record CreateWaypointCommand(
    string Name,
    double Latitude,
    double Longitude,
    int Order
);