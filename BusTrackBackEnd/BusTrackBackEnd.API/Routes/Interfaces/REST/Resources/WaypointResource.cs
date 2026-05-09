namespace BusTrackBackEnd.API.Routes.Interfaces.REST.Resources;

public record WaypointResource(
    string Name,
    double Latitude,
    double Longitude,
    int Order
    );