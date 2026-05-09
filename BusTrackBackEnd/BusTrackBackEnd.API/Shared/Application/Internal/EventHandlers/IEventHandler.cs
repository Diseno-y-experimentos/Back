using BusTrackBackEnd.API.Shared.Domain.Model.Events;

namespace BusTrackBackEnd.API.Shared.Application.Internal.EventHandlers;

public interface IEventHandler<in TEvent> where TEvent : IEvent
{
    Task HandleAsync(TEvent eventMessage);
}