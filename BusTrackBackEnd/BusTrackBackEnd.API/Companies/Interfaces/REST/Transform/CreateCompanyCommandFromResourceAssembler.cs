using BusTrackBackEnd.API.Companies.Domain.Model.Commands;
using BusTrackBackEnd.API.Companies.Interfaces.REST.Resources;

namespace BusTrackBackEnd.API.Companies.Interfaces.REST.Transform;

public static class CreateCompanyCommandFromResourceAssembler
{
    public static CreateCompanyCommand ToCommand(CreateCompanyResource resource)
        => new(resource.Name, resource.Email, resource.Ruc, resource.Phone, resource.Address);

}