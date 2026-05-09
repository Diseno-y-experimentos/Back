namespace BusTrackBackEnd.API.Shared.Domain.Model.Events;

public interface IEvent
{
    DateTime OccurredAt { get; } 
}