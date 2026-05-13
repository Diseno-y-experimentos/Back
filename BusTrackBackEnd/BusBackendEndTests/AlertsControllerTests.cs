using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using BusTrackBackEnd.API.BoundedContexts.Communication.Interfaces.REST;
using BusTrackBackEnd.API.BoundedContexts.Communication.Interfaces.REST.Resources;
using BusTrackBackEnd.API.BoundedContexts.Communication.Domain.Model.Aggregates;
using BusTrackBackEnd.API.Shared.Domain.Repositories;
using BusTrackBackEnd.API.Shared.Infrastructure.Persistence.EFC;

namespace BusTrackBackEnd.Tests
{
    public class AlertsControllerTests
    {
        private readonly AppDbContext _context;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly AlertsController _controller;

        public AlertsControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _context = new AppDbContext(options);
            
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            
            _unitOfWorkMock.Setup(u => u.CompleteAsync())
                           .Returns(async () => await _context.SaveChangesAsync());
            
            _controller = new AlertsController(_context, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetById_WhenAlertExists_ShouldReturnOk()
        {
            var newAlert = new Alert("Alerta de Prueba", "Mensaje de prueba", false);
            _context.Set<Alert>().Add(newAlert);
            await _context.SaveChangesAsync();
            
            var result = await _controller.GetById(newAlert.Id);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task GetById_WhenAlertDoesNotExist_ShouldReturnNotFound()
        {
            int nonExistentId = 999;
            
            var result = await _controller.GetById(nonExistentId);
            
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_WithValidResource_ShouldReturnOkAndCompleteUnitOfWork()
        {
            var createResource = new CreateAlertResource("Nueva Alerta", "Detalle", false);
            
            var result = await _controller.Create(createResource);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
            
            var savedAlert = await _context.Set<Alert>().FirstOrDefaultAsync(a => a.Title == "Nueva Alerta");
            Assert.NotNull(savedAlert);
        }

        [Fact]
        public async Task Delete_WhenAlertExists_ShouldReturnNoContent()
        {
            var alertToDelete = new Alert("Alerta para borrar", "Detalle", true);
            _context.Set<Alert>().Add(alertToDelete);
            await _context.SaveChangesAsync();
            
            var result = await _controller.Delete(alertToDelete.Id);
            
            Assert.IsType<NoContentResult>(result);
            
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
            
            var deletedAlert = await _context.Set<Alert>().FindAsync(alertToDelete.Id);
            Assert.Null(deletedAlert);
        }
    }
}