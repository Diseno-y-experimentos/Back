using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using BusTrackBackEnd.API.IAM.Interfaces.REST;
using BusTrackBackEnd.API.IAM.Interfaces.REST.Resources;
using BusTrackBackEnd.API.IAM.Domain.Services;
using BusTrackBackEnd.API.IAM.Domain.Repositories;
using BusTrackBackEnd.API.IAM.Application.Internal.OutboundServices;
using BusTrackBackEnd.API.IAM.Domain.Model.Aggregates;
using BusTrackBackEnd.API.IAM.Domain.Model.Commands;
using BusTrackBackEnd.API.Companies.Domain.Model.Aggregates;

namespace BusTrackBackEnd.Tests
{
    public class AuthenticationControllerTests
    {
        private readonly Mock<IUserCommandService> _commandServiceMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ICompanyRepository> _companyRepositoryMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly AuthenticationController _controller;

        public AuthenticationControllerTests()
        {
            _commandServiceMock = new Mock<IUserCommandService>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _companyRepositoryMock = new Mock<ICompanyRepository>();
            _tokenServiceMock = new Mock<ITokenService>();

            _controller = new AuthenticationController(
                _commandServiceMock.Object,
                _userRepositoryMock.Object,
                _companyRepositoryMock.Object,
                _tokenServiceMock.Object);
        }

        private void MockUserContext(string id, string accountType)
        {
            var claims = new List<Claim>
            {
                new Claim("id", id),
                new Claim("account_type", accountType)
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        [Fact]
        public async Task Register_WhenSuccessful_ShouldReturnOk()
        {
            var resource = new SignUpResource("walter_dev", "walter@example.com", "Password123!");
            var result = await _controller.Register(resource);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Register_WhenUserExists_ShouldReturnConflict()
        {
            var resource = new SignUpResource("existing_user", "test@test.com", "pass");
            _commandServiceMock.Setup(s => s.RegisterAsync(It.IsAny<SignUpCommand>()))
                               .ThrowsAsync(new InvalidOperationException("User already exists"));

            var result = await _controller.Register(resource);

            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Contains("already exists", conflictResult.Value?.ToString() ?? string.Empty);
        }

        [Fact]
        public async Task SignIn_WithValidCredentials_ShouldReturnToken()
        {
            var resource = new SignInResource("walter_dev", "Password123!");
            string expectedToken = "fake-jwt-token";
            
            _commandServiceMock.Setup(s => s.SignInAsync(It.IsAny<SignInCommand>()))
                               .ReturnsAsync(expectedToken);

            var result = await _controller.SignIn(resource);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains(expectedToken, okResult.Value?.ToString() ?? string.Empty);
        }

        [Fact]
        public async Task Refresh_ForUserAccount_ShouldReturnOk()
        {
            int userId = 10;
            MockUserContext(userId.ToString(), "user");
            
            var user = new User("walter_dev", "walter@example.com", "hash");
            _userRepositoryMock.Setup(r => r.FindByIdAsync(userId)).ReturnsAsync(user);
            
            _tokenServiceMock.Setup(t => t.GenerateToken(user)).Returns("new-token");

            var result = await _controller.Refresh();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("new-token", okResult.Value?.ToString() ?? string.Empty);
        }

        [Fact]
        public async Task Refresh_ForCompanyAccount_ShouldReturnOk()
        {
            int companyId = 5;
            MockUserContext(companyId.ToString(), "company");
            
            var company = new Company("Bus SAC", "info@bus.com", "20123", "999", "Lima");
            _companyRepositoryMock.Setup(r => r.FindByIdAsync(companyId)).ReturnsAsync(company);
            
            _tokenServiceMock.Setup(t => t.GenerateToken(company)).Returns("new-token");

            var result = await _controller.Refresh();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("new-token", okResult.Value?.ToString() ?? string.Empty);
        }
    }
}