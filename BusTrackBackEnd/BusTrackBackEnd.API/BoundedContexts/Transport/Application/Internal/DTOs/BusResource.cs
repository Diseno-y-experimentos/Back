using System;

namespace BusTrackBackEnd.API.BoundedContexts.Transport.Application.Internal.DTOs
{
    public class BusResource
    {
        public int Id { get; set; }
        public string Plate { get; set; }
        public string Route { get; set; }
        public string Status { get; set; }
        public string Driver { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
