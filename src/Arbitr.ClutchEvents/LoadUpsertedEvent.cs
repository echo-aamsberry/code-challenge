using System;

namespace Arbitr.ClutchEvents
{
    public class LoadUpsertedEvent
    {
        public int LoadId{ get; set; }
        public int OriginLocationId { get; set; }
        public int DestinationLocationId { get; set; }
        public DateTimeOffset AvailableDate { get; set; }
    }
}