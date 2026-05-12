using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using BusTrackBackEnd.API.IAM.Interfaces.REST;
using BusTrackBackEnd.API.IAM.Interfaces.REST.Resources;
using BusTrackBackEnd.API.IAM.Domain.Repositories;
using BusTrackBackEnd.API.IAM.Application.Internal.OutboundServices;
using BusTrackBackEnd.API.IAM.Domain.Model.Aggregates;
using BusTrackBackEnd.API.Shared.Domain.Repositories;

namespace BusTrackBackEnd.Tests
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IHashingService> _hashingServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _hashingServiceMock = new Mock<IHashingService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _controller = new UsersController(
                _userRepositoryMock.Object,
                _hashingServiceMock.Object,
                _unitOfWorkMock.Object);
        }
        
        private void MockUserContext(int userId)
        {
            var claims = new[] { new Claim("id", userId.ToString()) };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
        }

        [Fact]
        public async Task GetProfile_WhenUserExists_ShouldReturnOk()
        {
            int userId = 1;
            MockUserContext(userId);
            
            var user = new User("walter_dev", "walter@example.com", "hash");
            _userRepositoryMock.Setup(r => r.FindByIdAsync(userId)).ReturnsAsync(user);
            
            var result = await _controller.GetProfile();
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resource = Assert.IsType<UserResource>(okResult.Value);
            Assert.Equal("walter_dev", resource.Username);
        }

        [Fact]
        public async Task GetProfile_WhenUserNotFound_ShouldReturnNotFound()
        {
            int userId = 99;
            MockUserContext(userId);
            _userRepositoryMock.Setup(r => r.FindByIdAsync(userId)).ReturnsAsync((User)null);
            
            var result = await _controller.GetProfile();
            
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UpdateProfile_WhenUsernameConflict_ShouldReturnConflict()
        {
            int currentUserId = 1;
            int otherUserId = 2; 
            MockUserContext(currentUserId);

            var currentUser = new User("walter_old", "walter@example.com", "hash");
            var otherUser = new User("walter_new", "other@example.com", "hash");
            
            typeof(User).GetProperty("Id")?.SetValue(currentUser, currentUserId);
            typeof(User).GetProperty("Id")?.SetValue(otherUser, otherUserId);
            
            _userRepositoryMock.Setup(r => r.FindByIdAsync(currentUserId)).ReturnsAsync(currentUser);
            _userRepositoryMock.Setup(r => r.FindByUsernameAsync("walter_new")).ReturnsAsync(otherUser);

            var resource = new UpdateUserResource("walter_new", "walter@example.com", "new_password");
            
            var result = await _controller.UpdateProfile(resource);
            
            Assert.IsType<ConflictObjectResult>(result);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Never);
        }
        [Fact]
        public async Task UpdateProfile_WhenValid_ShouldUpdateAndReturnOk()
        {
            int userId = 1;
            MockUserContext(userId);

            var user = new User("walter_dev", "walter@example.com", "old_hash");
            _userRepositoryMock.Setup(r => r.FindByIdAsync(userId)).ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.FindByUsernameAsync("walter_updated")).ReturnsAsync((User)null);
            _hashingServiceMock.Setup(s => s.HashPassword("new_pass")).Returns("new_hash");

            var resource = new UpdateUserResource("walter_updated", "new@email.com", "new_pass");
            
            var result = await _controller.UpdateProfile(resource);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
            
            var updatedResource = Assert.IsType<UserResource>(okResult.Value);
            Assert.Equal("walter_updated", updatedResource.Username);
            Assert.Equal("new@email.com", updatedResource.Email);
        }
    }
}