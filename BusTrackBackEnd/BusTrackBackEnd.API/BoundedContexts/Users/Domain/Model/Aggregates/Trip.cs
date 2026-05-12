using System.ComponentModel.DataAnnotations.Schema;

namespace BusTrackBackEnd.API.BoundedContexts.Users.Domain.Model.Aggregates;

[Table("trips")]
public class Trip
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? RouteId { get; set; }
    public string? Origin { get; set; }
    public string? Destination { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Trip(
        int userId,
        int? routeId,
        string? origin,
        string? destination,
        DateTime? startedAt,
        DateTime? endedAt,
        string? notes)
    {
        UserId = userId;
        RouteId = routeId;
        Origin = origin;
        Destination = destination;
        StartedAt = startedAt;
        EndedAt = endedAt;
        Notes = notes;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(
        int? routeId,
        string? origin,
        string? destination,
        DateTime? startedAt,
        DateTime? endedAt,
        string? notes)
    {
        RouteId = routeId;
        Origin = origin;
        Destination = destination;
        StartedAt = startedAt;
        EndedAt = endedAt;
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
    }

    protected Trip() { }
}

