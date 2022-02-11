using System;
using System.Collections.Generic;

namespace Arbitr.Data.Models
{
    public class Truck
    {
        public Guid TruckKey { get; set; }
        public int OriginLocationId { get; set; }
        public List<int> PossibleDestinationLocationIds { get; set; }
        public DateTimeOffset AvailableDate { get; set; }
    }
}