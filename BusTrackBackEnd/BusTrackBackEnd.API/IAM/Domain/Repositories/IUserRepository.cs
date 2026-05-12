using BusTrackBackEnd.API.IAM.Domain.Model.Aggregates;
using BusTrackBackEnd.API.Shared.Domain.Repositories;

namespace BusTrackBackEnd.API.IAM.Domain.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> FindByUsernameAsync(string username);
    Task<User?> FindByUsernameOrEmailAsync(string usernameOrEmail);
    
    bool ExistsByUsername(string username);
}