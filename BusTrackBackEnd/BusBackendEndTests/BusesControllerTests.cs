using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using BusTrackBackEnd.API.BoundedContexts.Transport.Interfaces.REST;
using BusTrackBackEnd.API.BoundedContexts.Transport.Application.Internal.DTOs;
using BusTrackBackEnd.API.BoundedContexts.Transport.Application.Internal.Services;

namespace BusTrackBackEnd.Tests
{
    public class BusesControllerTests
    {
        private readonly Mock<IBusService> _busServiceMock;
        private readonly BusesController _controller;

        public BusesControllerTests()
        {
            _busServiceMock = new Mock<IBusService>();
            _controller = new BusesController(_busServiceMock.Object);
        }

        [Fact]
        public async Task GetAllBuses_ShouldReturnOkWithList()
        {
            // Arrange
            // Usamos BusResource directamente. (Si tu BusResource te marca error de "0 parameters", 
            // simplemente ponle los datos que te pida entre los paréntesis).
            var expectedBuses = new List<BusResource>
            {
                new BusResource(),
                new BusResource()
            };

            _busServiceMock.Setup(s => s.GetAllAsync())
                           .ReturnsAsync(expectedBuses);

            // Act
            var result = await _controller.GetAllBuses();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedBuses, okResult.Value);
        }

        [Fact]
        public async Task GetBusById_WhenBusExists_ShouldReturnOk()
        {
            // Arrange
            int busId = 1;
            var expectedBus = new BusResource();

            _busServiceMock.Setup(s => s.GetByIdAsync(busId))
                           .ReturnsAsync(expectedBus);

            // Act
            var result = await _controller.GetBusById(busId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedBus, okResult.Value);
        }

        [Fact]
        public async Task GetBusById_WhenBusDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            int nonExistentId = 999;
            
            _busServiceMock.Setup(s => s.GetByIdAsync(nonExistentId))
                           .ReturnsAsync((BusResource)null);

            // Act
            var result = await _controller.GetBusById(nonExistentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateBus_WithValidResource_ShouldReturnCreatedAtAction()
        {
            // Arrange
            // El error indicó que CreateBusResource se instancia con paréntesis vacíos
            var createResource = new CreateBusResource(); 
            var createdBus = new BusResource();

            _busServiceMock.Setup(s => s.CreateAsync(createResource))
                           .ReturnsAsync(createdBus);

            // Act
            var result = await _controller.CreateBus(createResource);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(BusesController.GetBusById), createdResult.ActionName);
            Assert.Equal(createdBus, createdResult.Value);
        }

        [Fact]
        public async Task CreateBus_WhenServiceThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            var createResource = new CreateBusResource();
            string expectedErrorMessage = "Capacity must be greater than 0";

            _busServiceMock.Setup(s => s.CreateAsync(createResource))
                           .ThrowsAsync(new ArgumentException(expectedErrorMessage));

            // Act
            var result = await _controller.CreateBus(createResource);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var responseValue = badRequestResult.Value?.ToString();
            Assert.Contains(expectedErrorMessage, responseValue ?? string.Empty);
        }
    }
}