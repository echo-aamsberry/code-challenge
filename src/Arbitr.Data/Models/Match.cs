using System;

namespace Arbitr.Data.Models
{
    public class Match
    {
        public Guid TruckKey { get; set; }
        public Guid LoadKey { get; set; }
        public byte Matchiness { get; set; }
    }
}