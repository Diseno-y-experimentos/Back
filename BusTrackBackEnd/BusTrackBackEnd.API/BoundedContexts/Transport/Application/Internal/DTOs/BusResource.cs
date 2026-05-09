namespace BusTrackBackEnd.API.BoundedContexts.Transport.Application.Internal.DTOs
{
    public class BusResource
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public int Capacity { get; set; }
        public string CurrentLocation { get; set; }
        public string Status { get; set; }
        public int? CurrentRouteId { get; set; }
    }
}
