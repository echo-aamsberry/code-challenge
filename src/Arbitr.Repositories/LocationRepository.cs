using Arbitr.Data.Interfaces;
using System;
using System.Collections.Generic;
using core = Arbitr.Core.Models;
using data = Arbitr.Data.Models;

namespace Arbitr.Repositories
{
    public class LocationRepository
    {
        private readonly ILocationDataAccess _locationDataAccess;
        public Dictionary<string, core.Location> KnownLocations { get; }

        public LocationRepository(ILocationDataAccess locationDataAccess)
        {
            _locationDataAccess = locationDataAccess ?? throw new ArgumentNullException(nameof(locationDataAccess));
            KnownLocations = new Dictionary<string, core.Location>();
        }

        public void InitializeAllKnownLocations()
        {
            var dbLocations = _locationDataAccess.GetAllLocationsFromDatabase();

            foreach (var dbLocation in dbLocations)
            {
                KnownLocations[dbLocation.Zip] = MapLocationDataToCore(dbLocation);
            }
        }

        public int GetLocationId(string zip)
        {
            if (KnownLocations.ContainsKey(zip) == false)
            {
                KnownLocations[zip] = GetLocationByZipFromDatabase(zip);
            }

            return KnownLocations[zip].LocationId;
        }

        private core.Location GetLocationByZipFromDatabase(string zip)
        {
            var dbLocation = _locationDataAccess.GetLocationByZipFromDatabase(zip);
            return MapLocationDataToCore(dbLocation);
        }

        private core.Location MapLocationDataToCore(data.Location dbLocation)
        {
            return new core.Location()
            {
                LocationId = dbLocation.LocationId,
                City = dbLocation.City,
                State = dbLocation.State,
                Zip = dbLocation.Zip,
                Latitude = dbLocation.Latitude,
                Longitude = dbLocation.Longitude
            };
        }
    }
}