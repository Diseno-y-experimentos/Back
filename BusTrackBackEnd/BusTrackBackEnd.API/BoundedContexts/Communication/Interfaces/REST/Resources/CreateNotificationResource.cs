namespace BusTrackBackEnd.API.BoundedContexts.Communication.Interfaces.REST.Resources;

public record CreateNotificationResource(
    string Title,
    string Message,
    bool IsRead = false
);

