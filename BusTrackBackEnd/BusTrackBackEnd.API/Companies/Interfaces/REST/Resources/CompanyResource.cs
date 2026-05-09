namespace BusTrackBackEnd.API.Companies.Interfaces.REST.Resources;

public record CompanyResource(
    int Id,
    string Name,
    string Email,
    string Ruc,
    string Phone,
    string Address
    );