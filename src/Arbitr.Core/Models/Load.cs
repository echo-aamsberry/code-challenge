using System;

namespace Arbitr.Core.Models
{
    public class Load
    {
        public Guid LoadKey { get; set; }
        public Location Origin { get; set; }
        public Location Destination { get; set; }
        public DateTimeOffset PickupDate { get; set; }
    }
}