using Arbitr.Data.Interfaces;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Arbitr.Data
{
    public class ClutchDataAccess : IClutchDataAccess
    {
        private readonly string _arbitrConnectionString;

        public ClutchDataAccess(string arbitrConnectionString)
        {
            _arbitrConnectionString = arbitrConnectionString ?? throw new ArgumentNullException(nameof(arbitrConnectionString));
        }

        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(_arbitrConnectionString);
        }

        #region Public Methods

        public void InsertTruckMapping(int truckId, Guid truckKey)
        {
            using var connection = GetSqlConnection();

            var insertTruckMappingCommand = connection.CreateCommand();
            insertTruckMappingCommand.CommandText = "[Clutch].[InsertTruckMapping]";
            insertTruckMappingCommand.CommandType = CommandType.StoredProcedure;

            insertTruckMappingCommand.Parameters.Add(new SqlParameter("@TruckId", truckId));
            insertTruckMappingCommand.Parameters.Add(new SqlParameter("@TruckKey", truckKey));

            connection.Open();

            insertTruckMappingCommand.ExecuteNonQuery();
        }

        public void RemoveTruckMapping(int truckId, Guid truckKey)
        {
            using var connection = GetSqlConnection();

            var removeTruckMappingCommand = connection.CreateCommand();
            removeTruckMappingCommand.CommandText = "[Clutch].[RemoveTruckMapping]";
            removeTruckMappingCommand.CommandType = CommandType.StoredProcedure;

            removeTruckMappingCommand.Parameters.Add(new SqlParameter("@TruckId", truckId));
            removeTruckMappingCommand.Parameters.Add(new SqlParameter("@TruckKey", truckKey));

            connection.Open();

            removeTruckMappingCommand.ExecuteNonQuery();
        }

        public Guid? GetTruckKeyById(int truckId)
        {
            using var connection = GetSqlConnection();

            var getTruckKeyCommand = connection.CreateCommand();
            getTruckKeyCommand.CommandText = "[Clutch].[GetTruckKeyById]";
            getTruckKeyCommand.CommandType = CommandType.StoredProcedure;

            getTruckKeyCommand.Parameters.Add(new SqlParameter("@TruckId", truckId));

            connection.Open();

            return (Guid?)getTruckKeyCommand.ExecuteScalar();
        }

        public int? GetTruckIdByKey(Guid truckKey)
        {
            using var connection = GetSqlConnection();

            var getTruckIdCommand = connection.CreateCommand();
            getTruckIdCommand.CommandText = "[Clutch].[GetTruckIdByKey]";
            getTruckIdCommand.CommandType = CommandType.StoredProcedure;

            getTruckIdCommand.Parameters.Add(new SqlParameter("@TruckKey", truckKey));

            connection.Open();

            return (int?)getTruckIdCommand.ExecuteScalar();
        }

        public void InsertLoadMapping(int loadId, Guid loadKey)
        {
            using var connection = GetSqlConnection();

            var insertLoadMappingCommand = connection.CreateCommand();
            insertLoadMappingCommand.CommandText = "[Clutch].[InsertLoadMapping]";
            insertLoadMappingCommand.CommandType = CommandType.StoredProcedure;

            insertLoadMappingCommand.Parameters.Add(new SqlParameter("@LoadId", loadId));
            insertLoadMappingCommand.Parameters.Add(new SqlParameter("@LoadKey", loadKey));

            connection.Open();

            insertLoadMappingCommand.ExecuteNonQuery();
        }

        public void RemoveLoadMapping(int loadId, Guid loadKey)
        {
            using var connection = GetSqlConnection();

            var removeLoadMappingCommand = connection.CreateCommand();
            removeLoadMappingCommand.CommandText = "[Clutch].[RemoveLoadMapping]";
            removeLoadMappingCommand.CommandType = CommandType.StoredProcedure;

            removeLoadMappingCommand.Parameters.Add(new SqlParameter("@LoadId", loadId));
            removeLoadMappingCommand.Parameters.Add(new SqlParameter("@LoadKey", loadKey));

            connection.Open();

            removeLoadMappingCommand.ExecuteNonQuery();
        }

        public Guid? GetLoadKeyById(int loadId)
        {
            using var connection = GetSqlConnection();

            var getLoadKeyCommand = connection.CreateCommand();
            getLoadKeyCommand.CommandText = "[Clutch].[GetLoadKeyById]";
            getLoadKeyCommand.CommandType = CommandType.StoredProcedure;

            getLoadKeyCommand.Parameters.Add(new SqlParameter("@LoadId", loadId));

            connection.Open();

            return (Guid?)getLoadKeyCommand.ExecuteScalar();
        }

        public int? GetLoadIdByKey(Guid loadKey)
        {
            using var connection = GetSqlConnection();

            var getLoadIdCommand = connection.CreateCommand();
            getLoadIdCommand.CommandText = "[Clutch].[GetLoadIdByKey]";
            getLoadIdCommand.CommandType = CommandType.StoredProcedure;

            getLoadIdCommand.Parameters.Add(new SqlParameter("@LoadKey", loadKey));

            connection.Open();

            return (int?)getLoadIdCommand.ExecuteScalar();
        }

        #endregion

    }
}