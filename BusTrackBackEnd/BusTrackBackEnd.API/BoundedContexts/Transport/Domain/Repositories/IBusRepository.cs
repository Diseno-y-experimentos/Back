using System.Collections.Generic;
using System.Threading.Tasks;
using BusTrackBackEnd.API.BoundedContexts.Transport.Domain.Model;

namespace BusTrackBackEnd.API.BoundedContexts.Transport.Domain.Repositories
{
    public interface IBusRepository
    {
        Task<IEnumerable<Bus>> GetAllAsync();
        Task<Bus> GetByIdAsync(int id);
        Task<Bus> GetByLicensePlateAsync(string licensePlate);
        Task AddAsync(Bus bus);
        void Update(Bus bus);
        void Remove(Bus bus);
    }
}
