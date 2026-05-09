using BusTrackBackEnd.API.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BusTrackBackEnd.API.Shared.Infrastructure.Persistence.EFC.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly AppDbContext Context;

    protected BaseRepository(AppDbContext context)
    {
        Context = context;
    }

    public async Task<IEnumerable<T>> ListAsync()
        => await Context.Set<T>().ToListAsync();

    public async Task<T?> FindByIdAsync(int id)
        => await Context.Set<T>().FindAsync(id);

    public async Task AddAsync(T entity)
        => await Context.Set<T>().AddAsync(entity);

    public void Update(T entity)
        => Context.Set<T>().Update(entity);

    public void Remove(T entity)
        => Context.Set<T>().Remove(entity);
}