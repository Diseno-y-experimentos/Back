namespace BusTrackBackEnd.API.IAM.Interfaces.REST.Resources;

public record UpdateUserResource(
    string Username,
    string Email,
    string? Password
);

