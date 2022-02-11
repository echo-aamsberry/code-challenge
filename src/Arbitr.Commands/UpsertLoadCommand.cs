using System;

namespace Arbitr.Commands
{
    public class UpsertLoadCommand
    {
        public Guid LoadKey { get; set; }
        public int OriginLocationId { get; set; }
        public int DestinationLocationId { get; set; }
        public DateTimeOffset PickupDate { get; set; }

        public UpsertLoadCommand(Guid loadKey, int originLocationId, int destinationLocationId, DateTimeOffset pickupDate)
        {
            LoadKey = loadKey;
            OriginLocationId = originLocationId;
            DestinationLocationId = destinationLocationId;
            PickupDate = pickupDate;
        }
    }
}