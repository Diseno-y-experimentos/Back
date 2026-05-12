using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusTrackBackEnd.API.BoundedContexts.Transport.Domain.Model;
using BusTrackBackEnd.API.BoundedContexts.Transport.Domain.Repositories;
using BusTrackBackEnd.API.Shared.Infrastructure.Persistence.EFC;

namespace BusTrackBackEnd.API.BoundedContexts.Transport.Infrastructure.Persistence
{
    public class BusRepository : IBusRepository
    {
        private readonly AppDbContext _context;

        public BusRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bus>> GetAllAsync()
        {
            return await _context.Set<Bus>().ToListAsync();
        }

        public async Task<Bus> GetByIdAsync(int id)
        {
            return await _context.Set<Bus>().FindAsync(id);
        }

        public async Task<Bus> GetByLicensePlateAsync(string licensePlate)
        {
            return await _context.Set<Bus>().FirstOrDefaultAsync(b => b.Plate == licensePlate);
        }

        public async Task AddAsync(Bus bus)
        {
            await _context.Set<Bus>().AddAsync(bus);
            await _context.SaveChangesAsync();
        }

        public void Update(Bus bus)
        {
            _context.Set<Bus>().Update(bus);
            _context.SaveChanges();
        }

        public void Remove(Bus bus)
        {
            _context.Set<Bus>().Remove(bus);
            _context.SaveChanges();
        }
    }
}
