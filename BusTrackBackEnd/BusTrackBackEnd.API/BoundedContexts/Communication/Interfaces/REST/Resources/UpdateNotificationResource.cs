namespace BusTrackBackEnd.API.BoundedContexts.Communication.Interfaces.REST.Resources;

public record UpdateNotificationResource(
    string Title,
    string Message,
    bool IsRead
);

