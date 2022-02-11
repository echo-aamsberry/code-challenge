using Arbitr.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using core = Arbitr.Core.Models;
using data = Arbitr.Data.Models;

namespace Arbitr.Repositories
{
    public class LoadRepository
    {
        private readonly ILoadDataAccess _loadDataAccess;
        private readonly List<core.Location> _knownLocations;

        public LoadRepository(ILoadDataAccess loadDataAccess, List<core.Location> knownLocations)
        {
            _loadDataAccess = loadDataAccess ?? throw new ArgumentNullException(nameof(loadDataAccess));
            _knownLocations = knownLocations ?? throw new ArgumentNullException(nameof(knownLocations));
        }

        public List<core.Load> GetAllLoads()
        {
            var dbLoads = _loadDataAccess.GetAllLoadsFromDatabase();

            var loads = new List<core.Load>();

            foreach (var dbLoad in dbLoads)
            {
                loads.Add(MapLoadDataToCore(dbLoad));
            }

            return loads;
        }

        private core.Load MapLoadDataToCore(data.Load dbLoad)
        {
            var origin = (from l in _knownLocations where l.LocationId == dbLoad.OriginLocationId select l).First();
            var destination = (from l in _knownLocations where l.LocationId == dbLoad.DestinationLocationId select l).First();

            return new core.Load()
            {
                LoadKey = dbLoad.LoadKey,
                Origin = origin,
                Destination = destination,
                PickupDate = dbLoad.PickupDate
            };
        }
    }
}