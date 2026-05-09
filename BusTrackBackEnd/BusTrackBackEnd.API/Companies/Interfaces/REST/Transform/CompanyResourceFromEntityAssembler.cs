using BusTrackBackEnd.API.Companies.Domain.Model.Aggregates;
using BusTrackBackEnd.API.Companies.Interfaces.REST.Resources;

namespace BusTrackBackEnd.API.Companies.Interfaces.REST.Transform;

public static class CompanyResourceFromEntityAssembler
{
    public static CompanyResource ToResource(Company entity)
        => new(entity.Id, entity.Name, entity.Email, entity.Ruc, entity.Phone, entity.Address);
}