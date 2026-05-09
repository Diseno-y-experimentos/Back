namespace BusTrackBackEnd.API.Companies.Domain.Model.Commands;

public record CreateCompanyCommand(
    string Name,
    string Email,
    string Ruc,
    string Phone,
    string Address
    );