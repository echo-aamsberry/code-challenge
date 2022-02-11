using System;

namespace Arbitr.Data.Models
{
    public class Load
    {
        public Guid LoadKey { get; set; }
        public int OriginLocationId { get; set; }
        public int DestinationLocationId { get; set; }
        public DateTimeOffset PickupDate { get; set; }
    }
}