using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using BusTrackBackEnd.API.Routes.Interfaces.REST;
using BusTrackBackEnd.API.Routes.Interfaces.REST.Resources;
using BusTrackBackEnd.API.Routes.Domain.Services;
using BusTrackBackEnd.API.Routes.Domain.Repositories;
using BusTrackBackEnd.API.Routes.Domain.Model.Aggregates;
using BusTrackBackEnd.API.Routes.Domain.Model.Entities;
using BusTrackBackEnd.API.Routes.Domain.Model.Queries;
using BusTrackBackEnd.API.Routes.Domain.Model.Commands;
using BusTrackBackEnd.API.Shared.Domain.Repositories;

namespace BusTrackBackEnd.Tests
{
    public class RoutesControllerTests
    {
        private readonly Mock<IRouteCommandService> _commandServiceMock;
        private readonly Mock<IRouteQueryService> _queryServiceMock;
        private readonly Mock<IRouteRepository> _routeRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly RoutesController _controller;

        public RoutesControllerTests()
        {
            _commandServiceMock = new Mock<IRouteCommandService>();
            _queryServiceMock = new Mock<IRouteQueryService>();
            _routeRepositoryMock = new Mock<IRouteRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _controller = new RoutesController(
                _commandServiceMock.Object,
                _queryServiceMock.Object,
                _routeRepositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Create_WithValidResource_ShouldReturnOk()
        {
            var resource = new CreateRouteResource("Ruta A", 1, 30, 10, new List<WaypointResource>());
            var routeId = 100;
            var createdRoute = new Route("Ruta A", 1, 30, 10);

            _commandServiceMock.Setup(s => s.Handle(It.IsAny<CreateRouteCommand>()))
                .ReturnsAsync(routeId);
            
            _queryServiceMock.Setup(s => s.Handle(It.Is<GetRouteByIdQuery>(q => q.Id == routeId)))
                .ReturnsAsync(createdRoute);
            
            var result = await _controller.Create(resource);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task GetAll_WithFilter_ShouldReturnFilteredList()
        {
            var routes = new List<Route>
            {
                new Route("Troncal Sur", 1, 40, 5),
                new Route("Expreso Norte", 1, 20, 15)
            };

            _queryServiceMock.Setup(s => s.Handle(It.IsAny<GetAllRoutesQuery>()))
                .ReturnsAsync(routes);
            
            var result = await _controller.GetAll("Troncal");
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResources = Assert.IsAssignableFrom<IEnumerable<RouteResource>>(okResult.Value);
            Assert.Single(returnedResources);
            Assert.Equal("Troncal Sur", returnedResources.First().Name);
        }

        [Fact]
        public async Task GetById_WhenExists_ShouldReturnOk()
        {
            int routeId = 5;
            var route = new Route("Línea 1", 2, 60, 20);

            _queryServiceMock.Setup(s => s.Handle(It.Is<GetRouteByIdQuery>(q => q.Id == routeId)))
                .ReturnsAsync(route);
            
            var result = await _controller.GetById(routeId);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resource = Assert.IsType<RouteResource>(okResult.Value);
            Assert.Equal("Línea 1", resource.Name);
        }

        [Fact]
        public async Task Update_WhenExists_ShouldUpdateDetailsAndWaypoints()
        {
            int routeId = 1;
            var existingRoute = new Route("Ruta Vieja", 1, 30, 10);
            var updateResource = new CreateRouteResource("Ruta Nueva", 2, 45, 5, new List<WaypointResource>
            {
                new WaypointResource("Paradero 1", -12.0, -77.0, 1)
            });

            _routeRepositoryMock.Setup(r => r.FindByIdAsync(routeId))
                .ReturnsAsync(existingRoute);
            
            var result = await _controller.Update(routeId, updateResource);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
            
            var updatedResource = Assert.IsType<RouteResource>(okResult.Value);
            Assert.Equal("Ruta Nueva", updatedResource.Name);
        }

        [Fact]
        public async Task Delete_WhenExists_ShouldRemoveAndReturnNoContent()
        {
            int routeId = 10;
            var route = new Route("Ruta a Borrar", 1, 10, 10);

            _routeRepositoryMock.Setup(r => r.FindByIdAsync(routeId))
                .ReturnsAsync(route);
            
            var result = await _controller.Delete(routeId);
            
            Assert.IsType<NoContentResult>(result);
            _routeRepositoryMock.Verify(r => r.Remove(route), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByName_WhenNotFound_ShouldReturnNotFound()
        {
            string name = "Ruta Fantasma";
            _routeRepositoryMock.Setup(r => r.FindByNameAsync(name))
                .ReturnsAsync((Route)null);
            
            var result = await _controller.GetByName(name);
            
            Assert.IsType<NotFoundResult>(result);
        }
    }
}