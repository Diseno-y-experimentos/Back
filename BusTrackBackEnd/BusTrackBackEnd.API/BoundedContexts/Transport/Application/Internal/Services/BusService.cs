using System.Threading.Tasks;
using BusTrackBackEnd.API.BoundedContexts.Transport.Domain.Model;
using BusTrackBackEnd.API.BoundedContexts.Transport.Domain.Repositories;
using BusTrackBackEnd.API.BoundedContexts.Transport.Application.Internal.DTOs;
using System.Collections.Generic;
using System.Linq;
using BusTrackBackEnd.API.Shared.Domain.Repositories;

namespace BusTrackBackEnd.API.BoundedContexts.Transport.Application.Internal.Services
{
    public interface IBusService
    {
        Task<IEnumerable<BusResource>> GetAllAsync();
        Task<BusResource> GetByIdAsync(int id);
        Task<BusResource> CreateAsync(CreateBusResource resource);
        Task<BusResource> UpdateAsync(int id, CreateBusResource resource);
        Task<bool> DeleteAsync(int id);
    }

    public class BusService : IBusService
    {
        private readonly IBusRepository _busRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BusService(IBusRepository busRepository, IUnitOfWork unitOfWork)
        {
            _busRepository = busRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<BusResource>> GetAllAsync()
        {
            var buses = await _busRepository.GetAllAsync();
            return buses.Select(b => new BusResource
            {
                Id = b.Id,
                Plate = b.Plate,
                Route = b.Route,
                Status = b.Status,
                Driver = b.Driver,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            });
        }

        public async Task<BusResource> GetByIdAsync(int id)
        {
            var b = await _busRepository.GetByIdAsync(id);
            if (b == null) return null;

            return new BusResource
            {
                Id = b.Id,
                Plate = b.Plate,
                Route = b.Route,
                Status = b.Status,
                Driver = b.Driver,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            };
        }

        public async Task<BusResource> CreateAsync(CreateBusResource resource)
        {
            var bus = new Bus(resource.Plate, resource.Route, resource.Status, resource.Driver);
            await _busRepository.AddAsync(bus);
            await _unitOfWork.CompleteAsync();

            return new BusResource
            {
                Id = bus.Id,
                Plate = bus.Plate,
                Route = bus.Route,
                Status = bus.Status,
                Driver = bus.Driver,
                CreatedAt = bus.CreatedAt,
                UpdatedAt = bus.UpdatedAt
            };
        }

        public async Task<BusResource> UpdateAsync(int id, CreateBusResource resource)
        {
            var bus = await _busRepository.GetByIdAsync(id);
            if (bus == null)
                throw new System.Exception("Bus not found");

            bus.Update(resource.Plate, resource.Route, resource.Status, resource.Driver);
            _busRepository.Update(bus);
            await _unitOfWork.CompleteAsync();

            return new BusResource
            {
                Id = bus.Id,
                Plate = bus.Plate,
                Route = bus.Route,
                Status = bus.Status,
                Driver = bus.Driver,
                CreatedAt = bus.CreatedAt,
                UpdatedAt = bus.UpdatedAt
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var bus = await _busRepository.GetByIdAsync(id);
            if (bus == null)
                throw new System.Exception("Bus not found");

            _busRepository.Remove(bus);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
