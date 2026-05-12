namespace BusTrackBackEnd.API.IAM.Interfaces.REST.Resources;

public record UserResource(
    int Id,
    string Username,
    string Email,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

