using BusTrackBackEnd.API.IAM.Domain.Model.Aggregates;

namespace BusTrackBackEnd.API.IAM.Application.Internal.OutboundServices;

public interface ITokenService
{
    string GenerateToken(User user);
}