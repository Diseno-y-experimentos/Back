namespace BusTrackBackEnd.API.Shared.Domain.Repositories;

public interface IBaseRepository<T>
{
    Task<IEnumerable<T>> ListAsync();
    Task<T?> FindByIdAsync(int id);
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}