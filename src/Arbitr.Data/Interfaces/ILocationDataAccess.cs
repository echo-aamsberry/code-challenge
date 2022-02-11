using Arbitr.Data.Models;
using System.Collections.Generic;

namespace Arbitr.Data.Interfaces
{
    public interface ILocationDataAccess
    {
        List<Location> GetAllLocationsFromDatabase();
        Location GetLocationByZipFromDatabase(string zip);
    }
}