using BusTrackBackEnd.API.Routes.Domain.Model.Entities;

namespace BusTrackBackEnd.API.Routes.Domain.Model.Aggregates;

public class Route
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    
    // Propiedades nuevas para el Frontend
    public int EstimatedTime { get; private set; } // En minutos
    public int Frequency { get; private set; }     // En minutos

    // FK company
    public int CompanyId { get; private set; }

    public IReadOnlyCollection<Waypoint> Waypoints => _waypoints.AsReadOnly();
    private readonly List<Waypoint> _waypoints = new();

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Actualizamos el constructor para pedir los nuevos datos
    public Route(string name, int companyId, int estimatedTime, int frequency)
    {
        Name = name;
        CompanyId = companyId;
        EstimatedTime = estimatedTime;
        Frequency = frequency;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddWaypoint(Waypoint waypoint)
    {
        _waypoints.Add(waypoint);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, int companyId, int estimatedTime, int frequency)
    {
        Name = name;
        CompanyId = companyId;
        EstimatedTime = estimatedTime;
        Frequency = frequency;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ReplaceWaypoints(IEnumerable<Waypoint> waypoints)
    {
        _waypoints.Clear();
        _waypoints.AddRange(waypoints);
        UpdatedAt = DateTime.UtcNow;
    }

    // Constructor vacío para EF Core
    protected Route() {} 
}