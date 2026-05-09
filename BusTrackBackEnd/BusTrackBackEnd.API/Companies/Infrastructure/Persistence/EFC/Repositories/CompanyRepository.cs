using BusTrackBackEnd.API.Companies.Domain.Model.Aggregates;
using BusTrackBackEnd.API.Shared.Infrastructure.Persistence.EFC;
using BusTrackBackEnd.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BusTrackBackEnd.API.Companies.Infrastructure.Persistence.EFC.Repositories;

public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
{
    public CompanyRepository(AppDbContext context) : base(context) {}

    public async Task<Company?> FindByEmailAsync(string email)
        => await Context.Set<Company>().FirstOrDefaultAsync(c => c.Email == email);
}