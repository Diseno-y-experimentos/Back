using BusTrackBackEnd.API.IAM.Domain.Model.Commands;

namespace BusTrackBackEnd.API.IAM.Domain.Services;

public interface IUserCommandService
{
    Task RegisterAsync(SignUpCommand command);
    Task<string> SignInAsync(SignInCommand command);
}