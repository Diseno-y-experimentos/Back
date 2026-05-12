namespace BusTrackBackEnd.API.BoundedContexts.Users.Interfaces.REST.Resources;

public record TripResource(
    int Id,
    int UserId,
    int? RouteId,
    string? Origin,
    string? Destination,
    DateTime? StartedAt,
    DateTime? EndedAt,
    string? Notes,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

