using System;
using System.Collections.Generic;

namespace Arbitr.ClutchEvents
{
    public class TruckUpsertedEvent
    {
        public int TruckId { get; set; }
        public int OriginLocationId { get; set; }
        public List<int> PossibleDestinationLocationIds { get; set; }
        public DateTimeOffset AvailableDate { get; set; }
    }
}