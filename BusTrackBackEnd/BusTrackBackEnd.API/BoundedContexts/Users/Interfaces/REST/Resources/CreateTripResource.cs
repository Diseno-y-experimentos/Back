namespace BusTrackBackEnd.API.BoundedContexts.Users.Interfaces.REST.Resources;

public record CreateTripResource(
    int? RouteId,
    string? Origin,
    string? Destination,
    DateTime? StartedAt,
    DateTime? EndedAt,
    string? Notes
);

