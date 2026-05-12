using System.ComponentModel.DataAnnotations.Schema;

namespace BusTrackBackEnd.API.BoundedContexts.Users.Domain.Model.Aggregates;

[Table("saved_routes")]
public class SavedRoute
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RouteId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public SavedRoute(int userId, int routeId)
    {
        UserId = userId;
        RouteId = routeId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Touch() => UpdatedAt = DateTime.UtcNow;

    protected SavedRoute() { }
}

