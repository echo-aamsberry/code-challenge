using Arbitr.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using core = Arbitr.Core.Models;
using data = Arbitr.Data.Models;

namespace Arbitr.Repositories
{
    public class TruckRepository
    {
        private readonly ITruckDataAccess _truckDataAccess;
        private readonly List<core.Location> _knownLocations;

        public TruckRepository(ITruckDataAccess truckDataAccess, List<core.Location> knownLocations)
        {
            _truckDataAccess = truckDataAccess ?? throw new ArgumentNullException(nameof(truckDataAccess));
            _knownLocations = knownLocations ?? throw new ArgumentNullException(nameof(knownLocations));
        }

        public List<core.Truck> GetAllTrucks()
        {
            var dbTrucks = _truckDataAccess.GetAllTrucksFromDatabase();

            var trucks = new List<core.Truck>();

            foreach (var dbTruck in dbTrucks)
            {
                trucks.Add(MapTruckDataToCore(dbTruck));
            }

            return trucks;
        }

        private core.Truck MapTruckDataToCore(data.Truck dbTruck)
        {
            var origin = (from l in _knownLocations where l.LocationId == dbTruck.OriginLocationId select l).First();
            var possibleDestinations = new List<core.Location>();

            foreach (var destinationId in dbTruck.PossibleDestinationLocationIds)
            {
                possibleDestinations.Add((from l in _knownLocations where l.LocationId == destinationId select l).First());
            }

            return new core.Truck()
            {
                TruckKey = dbTruck.TruckKey,
                Origin = origin,
                PossibleDestinations = possibleDestinations,
                AvailableDate = dbTruck.AvailableDate
            };
        }
    }
}