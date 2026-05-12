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
using BusTrackBackEnd.API.Routes.Domain.Repositories;
using BusTrackBackEnd.API.Shared.Domain.Repositories;
using BusTrackBackEnd.API.Shared.Infrastructure.Persistence.EFC;
using Route = BusTrackBackEnd.API.Routes.Domain.Model.Aggregates.Route;

namespace BusTrackBackEnd.Tests
{
    public class SavedRoutesControllerTests
    {
        private readonly AppDbContext _context;
        private readonly Mock<IRouteRepository> _routeRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly SavedRoutesController _controller;

        public SavedRoutesControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _routeRepositoryMock = new Mock<IRouteRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            
            _unitOfWorkMock.Setup(u => u.CompleteAsync())
                           .Returns(async () => await _context.SaveChangesAsync());

            _controller = new SavedRoutesController(_context, _routeRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithResources()
        {
            int userId = 1;
            var savedRoute = new SavedRoute(userId, 10);
            _context.Set<SavedRoute>().Add(savedRoute);
            await _context.SaveChangesAsync();
            
            _routeRepositoryMock.Setup(r => r.FindByIdAsync(10))
                                .ReturnsAsync(new Route("Ruta de Prueba", 1, 60, 15));
            
            var result = await _controller.GetAll(userId);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resources = Assert.IsAssignableFrom<IEnumerable<SavedRouteResource>>(okResult.Value);
            Assert.NotEmpty(resources);
        }

        [Fact]
        public async Task Create_WhenRouteNotFound_ShouldReturnNotFound()
        {
            int userId = 1;
            var resource = new CreateSavedRouteResource(99);
            
            _routeRepositoryMock.Setup(r => r.FindByIdAsync(99))
                                .ReturnsAsync((Route)null);
            
            var result = await _controller.Create(userId, resource);
            
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Create_WhenRouteExists_ShouldReturnOkAndSave()
        {
            int userId = 1;
            int routeId = 5;
            var resource = new CreateSavedRouteResource(routeId);

            _routeRepositoryMock.Setup(r => r.FindByIdAsync(routeId))
                                .ReturnsAsync(new Route("Troncal 1", 1, 45, 10));
            
            var result = await _controller.Create(userId, resource);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
            
            var saved = await _context.Set<SavedRoute>().FirstOrDefaultAsync(sr => sr.RouteId == routeId);
            Assert.NotNull(saved);
        }

        [Fact]
        public async Task Delete_WhenExists_ShouldReturnNoContent()
        {
            int userId = 1;
            var savedRoute = new SavedRoute(userId, 20);
            _context.Set<SavedRoute>().Add(savedRoute);
            await _context.SaveChangesAsync();
            
            var result = await _controller.Delete(userId, savedRoute.Id);
            
            Assert.IsType<NoContentResult>(result);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
            
            var exists = await _context.Set<SavedRoute>().AnyAsync(sr => sr.Id == savedRoute.Id);
            Assert.False(exists);
        }
    }
}