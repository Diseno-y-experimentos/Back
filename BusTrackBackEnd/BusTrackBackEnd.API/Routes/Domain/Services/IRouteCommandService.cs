using BusTrackBackEnd.API.Routes.Domain.Model.Commands;

namespace BusTrackBackEnd.API.Routes.Domain.Services;

public interface IRouteCommandService
{
    Task<int> Handle(CreateRouteCommand command);
}