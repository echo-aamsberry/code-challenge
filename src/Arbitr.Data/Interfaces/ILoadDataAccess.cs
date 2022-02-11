using Arbitr.Data.Models;
using System;
using System.Collections.Generic;

namespace Arbitr.Data.Interfaces
{
    public interface ILoadDataAccess
    {
        List<Load> GetAllLoadsFromDatabase();
        void UpsertLoad(Guid loadKey, int originLocationId, int destinationLocationId, DateTimeOffset pickupDate);
        void RemoveLoad(Guid loadKey);
    }
}