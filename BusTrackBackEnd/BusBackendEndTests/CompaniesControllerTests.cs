using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using BusTrackBackEnd.API.Companies.Interfaces.REST;
using BusTrackBackEnd.API.Companies.Interfaces.REST.Resources;
using BusTrackBackEnd.API.Companies.Domain.Services;
using BusTrackBackEnd.API.Companies.Domain.Model.Aggregates;
using BusTrackBackEnd.API.Companies.Domain.Model.Queries;
using BusTrackBackEnd.API.Companies.Domain.Model.Commands;
using BusTrackBackEnd.API.Shared.Domain.Repositories;

namespace BusTrackBackEnd.Tests
{
    public class CompaniesControllerTests
    {
        private readonly Mock<ICompanyCommandService> _commandServiceMock;
        private readonly Mock<ICompanyQueryService> _queryServiceMock;
        private readonly Mock<ICompanyRepository> _companyRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CompaniesController _controller;

        public CompaniesControllerTests()
        {
            _commandServiceMock = new Mock<ICompanyCommandService>();
            _queryServiceMock = new Mock<ICompanyQueryService>();
            _companyRepositoryMock = new Mock<ICompanyRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _controller = new CompaniesController(
                _commandServiceMock.Object,
                _queryServiceMock.Object,
                _companyRepositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task CreateCompany_WithValidResource_ShouldReturnOk()
        {
            // Arrange
            var resource = new CreateCompanyResource("Bus SAC", "bus@sac.com", "20123456789", "999888777", "Av. Lima 123");

            // Act
            var result = await _controller.CreateCompany(resource);

            // Assert
            Assert.IsType<OkResult>(result);
            _commandServiceMock.Verify(s => s.Handle(It.IsAny<CreateCompanyCommand>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithList()
        {
            // Arrange
            var companies = new List<Company>
            {
                new Company("C1", "c1@test.com", "20111", "999", "Direccion 1"),
                new Company("C2", "c2@test.com", "20222", "888", "Direccion 2")
            };

            _queryServiceMock.Setup(s => s.Handle(It.IsAny<GetAllCompaniesQuery>()))
                             .ReturnsAsync(companies);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResources = Assert.IsAssignableFrom<IEnumerable<CompanyResource>>(okResult.Value);
            Assert.Equal(2, returnedResources.Count());
        }

        [Fact]
        public async Task GetById_WhenCompanyExists_ShouldReturnOk()
        {
            // Arrange
            int companyId = 1;
            var company = new Company("Bus SAC", "bus@sac.com", "20123456789", "999888777", "Av. Lima 123");

            _queryServiceMock.Setup(s => s.Handle(It.Is<GetCompanyByIdQuery>(q => q.Id == companyId)))
                             .ReturnsAsync(company);

            // Act
            var result = await _controller.GetById(companyId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resource = Assert.IsType<CompanyResource>(okResult.Value);
            Assert.Equal("Bus SAC", resource.Name);
        }

        [Fact]
        public async Task UpdateCompany_WhenExists_ShouldUpdateAndReturnOk()
        {
            // Arrange
            int companyId = 1;
            var existingCompany = new Company("Nombre Viejo", "viej@mail.com", "101", "111", "Dir vieja");
            var resource = new CreateCompanyResource("Nombre Nuevo", "nuevo@mail.com", "202", "222", "Dir nueva");

            _companyRepositoryMock.Setup(r => r.FindByIdAsync(companyId))
                                  .ReturnsAsync(existingCompany);

            // Act
            var result = await _controller.UpdateCompany(companyId, resource);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
            
            var updatedResource = Assert.IsType<CompanyResource>(okResult.Value);
            Assert.Equal("Nombre Nuevo", updatedResource.Name);
        }

        [Fact]
        public async Task UpdateCompany_WhenDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            int companyId = 99;
            var resource = new CreateCompanyResource("X", "X", "X", "X", "X");

            _companyRepositoryMock.Setup(r => r.FindByIdAsync(companyId))
                                  .ReturnsAsync((Company)null);

            // Act
            var result = await _controller.UpdateCompany(companyId, resource);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}