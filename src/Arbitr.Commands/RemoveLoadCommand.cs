using System;

namespace Arbitr.Commands
{
    public class RemoveLoadCommand
    {
        public Guid LoadKey { get; set; }

        public RemoveLoadCommand(Guid loadKey)
        {
            LoadKey = loadKey;
        }
    }
}