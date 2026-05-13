using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using BusTrackBackEnd.API.BoundedContexts.Users.Interfaces.REST;
using BusTrackBackEnd.API.BoundedContexts.Users.Interfaces.REST.Resources;
using BusTrackBackEnd.API.BoundedContexts.Users.Domain.Model.Aggregates;
using BusTrackBackEnd.API.Shared.Domain.Repositories;
using BusTrackBackEnd.API.Shared.Infrastructure.Persistence.EFC;

namespace BusTrackBackEnd.Tests
{
    public class TripsControllerTests
    {
        private readonly AppDbContext _context;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly TripsController _controller;

        public TripsControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _context = new AppDbContext(options);

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(u => u.CompleteAsync())
                           .Returns(async () => await _context.SaveChangesAsync());
            
            _controller = new TripsController(_context, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithTrips()
        {
            int userId = 1;
            var newTrip = new Trip(userId, 10, "Terminal Sur", "Miraflores", DateTime.UtcNow, null, "Sin novedades");
            _context.Set<Trip>().Add(newTrip);
            await _context.SaveChangesAsync();
            
            var result = await _controller.GetAll(userId);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task Create_WithValidResource_ShouldReturnOkAndCompleteUnitOfWork()
        {
            int userId = 1;
            var createResource = new CreateTripResource(15, "Origen", "Destino", DateTime.UtcNow, null, "Iniciando ruta");
            
            var result = await _controller.Create(userId, createResource);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
            
            var savedInDb = await _context.Set<Trip>().FirstOrDefaultAsync(x => x.Origin == "Origen" && x.UserId == userId);
            Assert.NotNull(savedInDb);
        }

        [Fact]
        public async Task Delete_WhenTripExists_ShouldReturnNoContent()
        {
            int userId = 1;
            var tripToDelete = new Trip(userId, 10, "A", "B", DateTime.UtcNow, DateTime.UtcNow, "Finalizado");
            _context.Set<Trip>().Add(tripToDelete);
            await _context.SaveChangesAsync();
            
            var result = await _controller.Delete(userId, tripToDelete.Id);
            
            Assert.IsType<NoContentResult>(result);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);

            var deletedTrip = await _context.Set<Trip>().FindAsync(tripToDelete.Id);
            Assert.Null(deletedTrip);
        }
    }
}