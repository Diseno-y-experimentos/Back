using BusTrackBackEnd.API.Companies.Domain.Model.Aggregates;
using BusTrackBackEnd.API.Companies.Domain.Model.Queries;

namespace BusTrackBackEnd.API.Companies.Domain.Services;

public interface ICompanyQueryService
{
    Task<IEnumerable<Company>> Handle(GetAllCompaniesQuery query);
    Task<Company?> Handle(GetCompanyByIdQuery query);
    Task<Company?> Handle(GetCompanyByEmailQuery query);
}