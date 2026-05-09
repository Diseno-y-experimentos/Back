namespace BusTrackBackEnd.API.Routes.Domain.Model.Entities;

public class Waypoint
{
    public int Id { get; private set; }

    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public string Name { get; private set; }

    public int Order { get; private set; }

    // FK to Route
    public int RouteId { get; private set; }

    public Waypoint(string name, double lat, double lng, int order)
    {
        Name = name;
        Latitude = lat;
        Longitude = lng;
        Order = order;
    }

    protected Waypoint() {} 
}