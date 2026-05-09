namespace BusTrackBackEnd.API.BoundedContexts.Transport.Application.Internal.DTOs
{
    public class CreateBusResource
    {
        public string LicensePlate { get; set; }
        public int Capacity { get; set; }
        public string CurrentLocation { get; set; }
    }
}
