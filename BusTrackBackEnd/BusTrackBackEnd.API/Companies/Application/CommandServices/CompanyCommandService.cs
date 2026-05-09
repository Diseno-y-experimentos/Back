using BusTrackBackEnd.API.Companies.Domain.Model.Aggregates;
using BusTrackBackEnd.API.Companies.Domain.Model.Commands;
using BusTrackBackEnd.API.Companies.Domain.Services;
using BusTrackBackEnd.API.Shared.Domain.Repositories;

namespace BusTrackBackEnd.API.Companies.Application.CommandServices;

public class CompanyCommandService : ICompanyCommandService
{
    private readonly ICompanyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CompanyCommandService(ICompanyRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateCompanyCommand command)
    {
        var company = new Company(
            command.Name, command.Email, command.Ruc,
            command.Phone, command.Address);

        await _repository.AddAsync(company);
        await _unitOfWork.CompleteAsync();
    }
}