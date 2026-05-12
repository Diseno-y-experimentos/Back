using System;

namespace BusTrackBackEnd.API.BoundedContexts.Transport.Domain.Model
{
    public class Bus
    {
        public int Id { get; private set; }
        public string Plate { get; private set; }
        public string Route { get; private set; }
        public string Status { get; private set; }
        public string Driver { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        protected Bus() { }

        public Bus(string plate, string route, string status, string driver)
        {
            Plate = plate ?? throw new ArgumentNullException(nameof(plate));
            Route = route ?? throw new ArgumentNullException(nameof(route));
            Status = status ?? "inactive";
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Update(string plate, string route, string status, string driver)
        {
            Plate = plate ?? Plate;
            Route = route ?? Route;
            Status = status ?? Status;
            Driver = driver ?? Driver;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
