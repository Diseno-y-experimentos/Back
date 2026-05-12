namespace BusTrackBackEnd.API.BoundedContexts.Transport.Application.Internal.DTOs
{
    public class CreateBusResource
    {
        public string Plate { get; set; }
        public string Route { get; set; }
        public string Status { get; set; }
        public string Driver { get; set; }
    }
}
