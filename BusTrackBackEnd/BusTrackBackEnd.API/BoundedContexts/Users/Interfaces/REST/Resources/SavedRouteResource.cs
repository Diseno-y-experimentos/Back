using BusTrackBackEnd.API.Routes.Interfaces.REST.Resources;

namespace BusTrackBackEnd.API.BoundedContexts.Users.Interfaces.REST.Resources;

public record SavedRouteResource(
    int Id,
    int UserId,
    int RouteId,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    RouteResource? Route
);

