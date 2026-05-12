using BusTrackBackEnd.API.IAM.Domain.Services;
using BusTrackBackEnd.API.IAM.Interfaces.REST.Resources;
using BusTrackBackEnd.API.IAM.Domain.Repositories;
using BusTrackBackEnd.API.IAM.Application.Internal.OutboundServices;
using BusTrackBackEnd.API.Companies.Domain.Model.Aggregates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusTrackBackEnd.API.IAM.Interfaces.REST;

[ApiController]
[Route("api/v1/auth")]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserCommandService _commandService;
    private readonly IUserRepository _userRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly ITokenService _tokenService;

    public AuthenticationController(
        IUserCommandService commandService,
        IUserRepository userRepository,
        ICompanyRepository companyRepository,
        ITokenService tokenService)
    {
        _commandService = commandService;
        _userRepository = userRepository;
        _companyRepository = companyRepository;
        _tokenService = tokenService;
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> Register([FromBody] SignUpResource body)
    {
        try
        {
            await _commandService.RegisterAsync(new(body.Username, body.Email, body.Password));
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInResource body)
    {
        try
        {
            var token = await _commandService.SignInAsync(new(body.Username, body.Password));
            return Ok(new { token });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        if (!TryGetCurrentUserId(out var userId))
            return Unauthorized(new { message = "Invalid token" });

        var accountType = User.Claims.FirstOrDefault(c => c.Type == "account_type")?.Value ?? "user";

        if (accountType == "company")
        {
            var company = await _companyRepository.FindByIdAsync(userId);
            if (company == null)
                return NotFound(new { message = "Company not found" });

            return Ok(new { token = _tokenService.GenerateToken(company) });
        }

        var user = await _userRepository.FindByIdAsync(userId);
        if (user == null)
            return NotFound(new { message = "User not found" });

        return Ok(new { token = _tokenService.GenerateToken(user) });
    }

    private bool TryGetCurrentUserId(out int userId)
    {
        userId = 0;
        var claim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        return int.TryParse(claim, out userId);
    }
}