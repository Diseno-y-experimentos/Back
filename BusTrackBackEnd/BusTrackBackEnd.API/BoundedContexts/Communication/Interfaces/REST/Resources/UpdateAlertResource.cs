namespace BusTrackBackEnd.API.BoundedContexts.Communication.Interfaces.REST.Resources;

public record UpdateAlertResource(
    string Title,
    string Message,
    bool IsRead
);

