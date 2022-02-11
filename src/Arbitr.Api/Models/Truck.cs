using System;
using System.Collections.Generic;

namespace Arbitr.Api.Models
{
    public class Truck
    {
        public Guid TruckKey { get; set; }
        public string OriginZip { get; set; }
        public List<string> PossibleDestinationZips { get; set; }
        public DateTimeOffset AvailableDate { get; set; }
    }
}