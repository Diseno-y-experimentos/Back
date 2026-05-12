namespace BusTrackBackEnd.API.BoundedContexts.Communication.Interfaces.REST.Resources;

public record CreateAlertResource(
    string Title,
    string Message,
    bool IsRead = false
);

