using Arbitr.Data.Models;
using System;
using System.Collections.Generic;

namespace Arbitr.Data.Interfaces
{
    public interface IMatchDataAccess
    {
        List<Match> GetAllMatchesFromDatabase();
        void SaveAllMatches(List<Match> matches);
        void RemoveMatchesForLoad(Guid loadKey);
        void RemoveMatchesForTruck(Guid truckKey);
    }
}