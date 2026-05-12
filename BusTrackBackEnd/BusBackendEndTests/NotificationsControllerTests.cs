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
    public class NotificationsControllerTests
    {
        private readonly AppDbContext _context;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly NotificationsController _controller;

        public NotificationsControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _context = new AppDbContext(options);
            
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            
            _unitOfWorkMock.Setup(u => u.CompleteAsync())
                           .Returns(async () => await _context.SaveChangesAsync());
            
            _controller = new NotificationsController(_context, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetById_WhenNotificationExists_ShouldReturnOk()
        {
            var newNotification = new Notification("Notificación de Prueba", "Mensaje de prueba", false);
            _context.Set<Notification>().Add(newNotification);
            await _context.SaveChangesAsync();
            
            var result = await _controller.GetById(newNotification.Id);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task GetById_WhenNotificationDoesNotExist_ShouldReturnNotFound()
        {
            int nonExistentId = 999;
            
            var result = await _controller.GetById(nonExistentId);
            
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_WithValidResource_ShouldReturnOkAndCompleteUnitOfWork()
        {
            var createResource = new CreateNotificationResource("Nueva Notificación", "Detalle de la notificación", false);
            
            var result = await _controller.Create(createResource);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
            
            var savedNotification = await _context.Set<Notification>().FirstOrDefaultAsync(n => n.Title == "Nueva Notificación");
            Assert.NotNull(savedNotification);
        }

        [Fact]
        public async Task Delete_WhenNotificationExists_ShouldReturnNoContent()
        {
            var notificationToDelete = new Notification("Notificación para borrar", "Detalle", true);
            _context.Set<Notification>().Add(notificationToDelete);
            await _context.SaveChangesAsync();
            
            var result = await _controller.Delete(notificationToDelete.Id);
            
            Assert.IsType<NoContentResult>(result);
            
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
            
            var deletedNotification = await _context.Set<Notification>().FindAsync(notificationToDelete.Id);
            Assert.Null(deletedNotification);
        }
    }
}