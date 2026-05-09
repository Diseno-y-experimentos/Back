namespace BusTrackBackEnd.API.Companies.Interfaces.REST.Resources;

public record CreateCompanyResource(
    string Name,
    string Email,
    string Ruc,
    string Phone,
    string Address
    );