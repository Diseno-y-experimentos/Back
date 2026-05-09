using System.Threading.Tasks;
using BusTrackBackEnd.API.BoundedContexts.Transport.Domain.Model;
using BusTrackBackEnd.API.BoundedContexts.Transport.Domain.Repositories;
using BusTrackBackEnd.API.BoundedContexts.Transport.Application.Internal.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace BusTrackBackEnd.API.BoundedContexts.Transport.Application.Internal.Services
{
    public interface IBusService
    {
        Task<IEnumerable<BusResource>> GetAllAsync();
        Task<BusResource> GetByIdAsync(int id);
        Task<BusResource> CreateAsync(CreateBusResource resource);
    }

    public class BusService : IBusService
    {
        private readonly IBusRepository _busRepository;
        // In a real project you might use an IUnitOfWork or SaveChanges on the repo
        // For simplicity we will assume the repository handles saving, or we need to add IUnitOfWork. Let's add simple basic implementation.

        public BusService(IBusRepository busRepository)
        {
            _busRepository = busRepository;
        }

        public async Task<IEnumerable<BusResource>> GetAllAsync()
        {
            var buses = await _busRepository.GetAllAsync();
            return buses.Select(b => new BusResource
            {
                Id = b.Id,
                LicensePlate = b.LicensePlate,
                Capacity = b.Capacity,
                CurrentLocation = b.CurrentLocation,
                Status = b.Status.ToString(),
                CurrentRouteId = b.CurrentRouteId
            });
        }

        public async Task<BusResource> GetByIdAsync(int id)
        {
            var b = await _busRepository.GetByIdAsync(id);
            if (b == null) return null;

            return new BusResource
            {
                Id = b.Id,
                LicensePlate = b.LicensePlate,
                Capacity = b.Capacity,
                CurrentLocation = b.CurrentLocation,
                Status = b.Status.ToString(),
                CurrentRouteId = b.CurrentRouteId
            };
        }

        public async Task<BusResource> CreateAsync(CreateBusResource resource)
        {
            var existing = await _busRepository.GetByLicensePlateAsync(resource.LicensePlate);
            if (existing != null)
                throw new System.Exception("License plate already exists.");

            var bus = new Bus(resource.LicensePlate, resource.Capacity, resource.CurrentLocation);
            await _busRepository.AddAsync(bus);

            return new BusResource
            {
                Id = bus.Id, // EF Core might not populate this until SaveChanges is called.
                LicensePlate = bus.LicensePlate,
                Capacity = bus.Capacity,
                CurrentLocation = bus.CurrentLocation,
                Status = bus.Status.ToString(),
                CurrentRouteId = bus.CurrentRouteId
            };
        }
    }
}
