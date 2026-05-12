namespace BusTrackBackEnd.API.BoundedContexts.Communication.Interfaces.REST.Resources;

public record NotificationResource(
    int Id,
    string Title,
    string Message,
    bool IsRead,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

