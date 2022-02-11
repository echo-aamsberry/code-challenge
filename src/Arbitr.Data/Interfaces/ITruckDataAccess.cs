using Arbitr.Data.Models;
using System;
using System.Collections.Generic;

namespace Arbitr.Data.Interfaces
{
    public interface ITruckDataAccess
    {
        List<Truck> GetAllTrucksFromDatabase();
        void UpsertTruck(Guid truckKey, int originLocationId, List<int> possibleDestinationLocationIds, DateTimeOffset pickupDate);
        void RemoveTruck(Guid truckKey);
    }
}