using System;

namespace Arbitr.Commands
{
    public class RemoveTruckCommand
    {
        public Guid TruckKey { get; set; }

        public RemoveTruckCommand(Guid truckKey)
        {
            TruckKey = truckKey;
        }
    }
}