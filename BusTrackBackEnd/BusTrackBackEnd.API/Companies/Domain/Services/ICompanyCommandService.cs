using BusTrackBackEnd.API.Companies.Domain.Model.Commands;

namespace BusTrackBackEnd.API.Companies.Domain.Services;

public interface ICompanyCommandService
{
    Task Handle(CreateCompanyCommand command);
}