namespace BusTrackBackEnd.API.IAM.Domain.Model.Commands;

public record SignUpCommand(string Username, string Email, string Password);