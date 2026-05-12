using BusTrackBackEnd.API.IAM.Application.Internal.OutboundServices;
using BusTrackBackEnd.API.IAM.Domain.Model.Aggregates;
using BusTrackBackEnd.API.IAM.Domain.Repositories;
using BusTrackBackEnd.API.IAM.Domain.Model.Commands;
using BusTrackBackEnd.API.IAM.Domain.Services;
using BusTrackBackEnd.API.Shared.Domain.Repositories;

namespace BusTrackBackEnd.API.IAM.Application.Internal.CommandServices;

public class UserCommandService : IUserCommandService
{
    private readonly IUserRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHashingService _hashingService;
    private readonly ITokenService _tokenService;

    public UserCommandService(IUserRepository repository, IUnitOfWork unitOfWork,
        IHashingService hashingService, ITokenService tokenService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _hashingService = hashingService;
        _tokenService = tokenService;
    }

    public async Task RegisterAsync(SignUpCommand command)
    {
        if (_repository.ExistsByUsername(command.Username))
            throw new InvalidOperationException("Username already exists");

        var passwordHash = _hashingService.HashPassword(command.Password);
        var user = new User(command.Username, command.Email, passwordHash);

        await _repository.AddAsync(user);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<string> SignInAsync(SignInCommand command)
    {
        var user = await _repository.FindByUsernameOrEmailAsync(command.Username);
        if (user == null) throw new UnauthorizedAccessException("Invalid username or password");

        var passwordValid = _hashingService.VerifyPassword(command.Password, user.PasswordHash);
        if (!passwordValid) throw new UnauthorizedAccessException("Invalid username or password");

        return _tokenService.GenerateToken(user);
    }
}