using System;
using System.Collections.Generic;

namespace Arbitr.Core.Models
{
    public class Truck
    {
        public Guid TruckKey { get; set; }
        public Location Origin { get; set; }
        public List<Location> PossibleDestinations { get; set; }
        public DateTimeOffset AvailableDate { get; set; }
    }
}