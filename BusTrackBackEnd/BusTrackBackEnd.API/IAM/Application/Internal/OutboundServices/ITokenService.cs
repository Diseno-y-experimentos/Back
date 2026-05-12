using BusTrackBackEnd.API.IAM.Domain.Model.Aggregates;
using BusTrackBackEnd.API.Companies.Domain.Model.Aggregates;

namespace BusTrackBackEnd.API.IAM.Application.Internal.OutboundServices;

public interface ITokenService
{
    string GenerateToken(User user);
    string GenerateToken(Company company);
}