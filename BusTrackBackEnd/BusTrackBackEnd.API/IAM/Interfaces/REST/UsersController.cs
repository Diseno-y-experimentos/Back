using BusTrackBackEnd.API.IAM.Application.Internal.OutboundServices;
using BusTrackBackEnd.API.IAM.Domain.Repositories;
using BusTrackBackEnd.API.IAM.Interfaces.REST.Resources;
using BusTrackBackEnd.API.Shared.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusTrackBackEnd.API.IAM.Interfaces.REST;

[ApiController]
[Authorize]
[Route("api/v1/user")]
[Route("user")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IHashingService _hashingService;
    private readonly IUnitOfWork _unitOfWork;

    public UsersController(
        IUserRepository userRepository,
        IHashingService hashingService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _hashingService = hashingService;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var user = await ResolveCurrentUserAsync();
        if (user == null) return NotFound(new { message = "User not found" });

        return Ok(new UserResource(user.Id, user.Username, user.Email, user.CreatedAt, user.UpdatedAt));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserResource resource)
    {
        var user = await ResolveCurrentUserAsync();
        if (user == null) return NotFound(new { message = "User not found" });

        var usernameExists = await _userRepository.FindByUsernameAsync(resource.Username);
        if (usernameExists != null && usernameExists.Id != user.Id)
            return Conflict(new { message = "Username already exists" });

        var passwordHash = string.IsNullOrWhiteSpace(resource.Password)
            ? null
            : _hashingService.HashPassword(resource.Password);

        user.UpdateProfile(resource.Username, resource.Email, passwordHash);
        _userRepository.Update(user);
        await _unitOfWork.CompleteAsync();

        return Ok(new UserResource(user.Id, user.Username, user.Email, user.CreatedAt, user.UpdatedAt));
    }

    private async Task<BusTrackBackEnd.API.IAM.Domain.Model.Aggregates.User?> ResolveCurrentUserAsync()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        if (!int.TryParse(claim, out var userId)) return null;

        return await _userRepository.FindByIdAsync(userId);
    }
}
