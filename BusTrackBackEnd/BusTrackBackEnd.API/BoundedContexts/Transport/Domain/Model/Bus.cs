using System;

namespace BusTrackBackEnd.API.BoundedContexts.Transport.Domain.Model
{
    public class Bus
    {
        public int Id { get; private set; }
        public string LicensePlate { get; private set; }
        public int Capacity { get; private set; }
        public string CurrentLocation { get; private set; }
        public BusStatus Status { get; private set; }
        public int? CurrentRouteId { get; private set; }

        protected Bus() { } // Para EF Core

        public Bus(string licensePlate, int capacity, string currentLocation)
        {
            LicensePlate = licensePlate ?? throw new ArgumentNullException(nameof(licensePlate));
            Capacity = capacity > 0 ? capacity : throw new ArgumentException("Capacity must be greater than 0");
            CurrentLocation = currentLocation ?? throw new ArgumentNullException(nameof(currentLocation));
            Status = BusStatus.INACTIVE;
        }

        public void UpdateLocation(string location)
        {
            CurrentLocation = location ?? throw new ArgumentNullException(nameof(location));
        }

        public void ChangeStatus(BusStatus status)
        {
            Status = status;
        }

        public void AssignRoute(int routeId)
        {
            CurrentRouteId = routeId;
            Status = BusStatus.ACTIVE;
        }
    }
}
