using System;
using System.Collections.Generic;

namespace Arbitr.Commands
{
    public class UpsertTruckCommand
    {
        public Guid TruckKey { get; set; }
        public int OriginLocationId { get; set; }
        public List<int> PossibleDestinationLocationIds { get; set; }
        public DateTimeOffset AvailableDate { get; set; }

        public UpsertTruckCommand(Guid truckKey, int originLocationId, List<int> possibleDestinationLocationIds, DateTimeOffset availableDate)
        {
            TruckKey = truckKey;
            OriginLocationId = originLocationId;
            PossibleDestinationLocationIds = possibleDestinationLocationIds;
            AvailableDate = availableDate;
        }
    }
}