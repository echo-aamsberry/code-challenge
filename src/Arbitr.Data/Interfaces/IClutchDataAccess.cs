using System;

namespace Arbitr.Data.Interfaces
{
    public interface IClutchDataAccess
    {
        void InsertTruckMapping(int truckId, Guid truckKey);
        void RemoveTruckMapping(int truckId, Guid truckKey);
        Guid? GetTruckKeyById(int truckId);
        int? GetTruckIdByKey(Guid truckKey);
        void InsertLoadMapping(int loadId, Guid loadKey);
        void RemoveLoadMapping(int truckId, Guid truckKey);
        Guid? GetLoadKeyById(int loadId);
        int? GetLoadIdByKey(Guid loadKey);
    }
}