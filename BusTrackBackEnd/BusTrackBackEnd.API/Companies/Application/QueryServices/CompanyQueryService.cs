using BusTrackBackEnd.API.Companies.Domain.Model.Aggregates;
using BusTrackBackEnd.API.Companies.Domain.Model.Queries;
using BusTrackBackEnd.API.Companies.Domain.Services;

namespace BusTrackBackEnd.API.Companies.Application.QueryServices;

public class CompanyQueryService : ICompanyQueryService
{
    private readonly ICompanyRepository _repository;

    public CompanyQueryService(ICompanyRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Company>> Handle(GetAllCompaniesQuery query)
        => _repository.ListAsync();

    public Task<Company?> Handle(GetCompanyByIdQuery query)
        => _repository.FindByIdAsync(query.Id);

    public Task<Company?> Handle(GetCompanyByEmailQuery query)
        => _repository.FindByEmailAsync(query.Email);
}