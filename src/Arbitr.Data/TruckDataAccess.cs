using Arbitr.Data.Interfaces;
using Arbitr.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Arbitr.Data
{
    public class TruckDataAccess : ITruckDataAccess
    {
        private readonly string _arbitrConnectionString;

        public TruckDataAccess(string arbitrConnectionString)
        {
            _arbitrConnectionString = arbitrConnectionString ?? throw new ArgumentNullException(nameof(arbitrConnectionString));
        }

        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(_arbitrConnectionString);
        }

        #region Public Methods

        public List<Truck> GetAllTrucksFromDatabase()
        {
            using var connection = GetSqlConnection();

            var selectAllTrucksCommand = connection.CreateCommand();
            selectAllTrucksCommand.CommandText = "[dbo].[SelectAllTrucks]";
            selectAllTrucksCommand.CommandType = CommandType.StoredProcedure;

            connection.Open();

            var reader = selectAllTrucksCommand.ExecuteReader();

            var trucks = new Dictionary<Guid, Truck>();

            // Trucks
            while (reader.Read())
            {
                var truckKey = (Guid)reader["TruckKey"];

                var t = new Truck
                {
                    TruckKey = truckKey,
                    OriginLocationId = (int)reader["OriginLocationId"],
                    PossibleDestinationLocationIds = new List<int>(),
                    AvailableDate = (DateTimeOffset)reader["AvailableDate"]
                };

                trucks[truckKey] = t;
            }

            // Truck destinations
            reader.NextResult();

            while (reader.Read())
            {
                trucks[(Guid)reader["TruckKey"]].PossibleDestinationLocationIds.Add((int)reader["DestinationLocationId"]);
            }

            return trucks.Values.ToList();
        }

        public void UpsertTruck(Guid truckKey, int originLocationId, List<int> possibleDestinationLocationIds, DateTimeOffset pickupDate)
        {
            if (CheckTruckExists(truckKey))
            {
                UpdateTruck(truckKey, originLocationId, possibleDestinationLocationIds, pickupDate);
            }
            else
            {
                InsertTruck(truckKey, originLocationId, possibleDestinationLocationIds, pickupDate);
            }
        }

        public void RemoveTruck(Guid truckKey)
        {
            using var connection = GetSqlConnection();

            var removeTruckCommand = connection.CreateCommand();
            removeTruckCommand.CommandText = "[dbo].[RemoveTruck]";
            removeTruckCommand.CommandType = CommandType.StoredProcedure;

            removeTruckCommand.Parameters.Add(new SqlParameter("@TruckKey", truckKey));

            connection.Open();

            removeTruckCommand.ExecuteNonQuery();
        }

        #endregion

        #region Private Methods

        private bool CheckTruckExists(Guid truckKey)
        {
            using var connection = GetSqlConnection();

            var checkTruckExistsCommand = connection.CreateCommand();
            checkTruckExistsCommand.CommandText = "[dbo].[CheckTruckExists]";
            checkTruckExistsCommand.CommandType = CommandType.StoredProcedure;

            checkTruckExistsCommand.Parameters.Add(new SqlParameter("@TruckKey", truckKey));

            connection.Open();

            return Convert.ToBoolean(checkTruckExistsCommand.ExecuteScalar());
        }

        private void InsertTruck(Guid truckKey, int originLocationId, List<int> possibleDestinationLocationIds, DateTimeOffset pickupDate)
        {
            using var connection = GetSqlConnection();

            var insertTruckCommand = connection.CreateCommand();
            insertTruckCommand.CommandText = "[dbo].[InsertTruck]";
            insertTruckCommand.CommandType = CommandType.StoredProcedure;

            insertTruckCommand.Parameters.Add(new SqlParameter("@TruckKey", truckKey));
            insertTruckCommand.Parameters.Add(new SqlParameter("@OriginLocationId", originLocationId));
            insertTruckCommand.Parameters.Add(new SqlParameter("@AvailableDate", pickupDate));

            using var destinations = new DataTable();
            destinations.Columns.Add("DestinationLocationId", typeof(int));
            possibleDestinationLocationIds.ForEach(id => destinations.Rows.Add(id));

            var possibleDestinationLocationIdsParameter = new SqlParameter("@PossibleDestinationLocationIds", SqlDbType.Structured)
            {
                TypeName = "[dbo].[TruckDestinationType]",
                Value = destinations
            };

            insertTruckCommand.Parameters.Add(possibleDestinationLocationIdsParameter);

            connection.Open();

            insertTruckCommand.ExecuteNonQuery();
        }

        private void UpdateTruck(Guid truckKey, int originLocationId, List<int> possibleDestinationLocationIds, DateTimeOffset pickupDate)
        {
            using var connection = GetSqlConnection();

            var updateTruckCommand = connection.CreateCommand();
            updateTruckCommand.CommandText = "[dbo].[UpdateTruck]";
            updateTruckCommand.CommandType = CommandType.StoredProcedure;

            updateTruckCommand.Parameters.Add(new SqlParameter("@TruckKey", truckKey));
            updateTruckCommand.Parameters.Add(new SqlParameter("@OriginLocationId", originLocationId));
            updateTruckCommand.Parameters.Add(new SqlParameter("@AvailableDate", pickupDate));

            using var destinations = new DataTable();
            destinations.Columns.Add("DestinationLocationId", typeof(int));
            possibleDestinationLocationIds.ForEach(id => destinations.Rows.Add(id));

            var possibleDestinationLocationIdsParameter = new SqlParameter("@PossibleDestinationLocationIds", SqlDbType.Structured)
            {
                TypeName = "[dbo].[TruckDestinationType]",
                Value = destinations
            };

            updateTruckCommand.Parameters.Add(possibleDestinationLocationIdsParameter);

            connection.Open();

            updateTruckCommand.ExecuteNonQuery();
        }

        #endregion
    }
}