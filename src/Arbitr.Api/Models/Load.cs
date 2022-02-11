using System;

namespace Arbitr.Api.Models
{
    public class Load
    {
        public Guid LoadKey { get; set; }
        public string OriginZip { get; set; }
        public string DestinationZip { get; set; }
        public DateTimeOffset PickupDate { get; set; }
    }
}
