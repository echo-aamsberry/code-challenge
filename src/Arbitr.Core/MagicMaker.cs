using Arbitr.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Arbitr.Core
{
    public class MagicMaker
    {
        public List<Match> MakeMagic(List<Truck> trucks, List<Load> loads)
        {
            var matches = new List<Match>();

            foreach (var l in loads)
            {
                var potentialTrucks = (from t in trucks where t.Origin.LocationId == l.Origin.LocationId select t).ToList();
                potentialTrucks.ForEach(t => matches.Add(new Match() { LoadKey = l.LoadKey, TruckKey = t.TruckKey, Matchiness = 50 }));
            }

            return matches;
        }
    }
}