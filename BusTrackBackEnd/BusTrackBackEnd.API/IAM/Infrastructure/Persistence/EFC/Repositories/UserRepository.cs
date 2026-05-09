using BusTrackBackEnd.API.IAM.Domain.Model.Aggregates;
using BusTrackBackEnd.API.Shared.Infrastructure.Persistence.EFC;
using BusTrackBackEnd.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;
using BusTrackBackEnd.API.IAM.Domain.Repositories;

namespace BusTrackBackEnd.API.IAM.Infrastructure.Persistence.EFC.Repositories;

// 1. CORRECCIÓN: Borramos la referencia repetida ", Domain.Repositories.IUserRepository"
public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) {}

    public async Task<User?> FindByUsernameAsync(string username)
        => await Context.Set<User>().FirstOrDefaultAsync(x => x.Username == username);

    // 2. CORRECCIÓN: Agregamos este método que pide tu nueva interfaz
    public bool ExistsByUsername(string username)
    {
        return Context.Set<User>().Any(x => x.Username == username);
    }
}