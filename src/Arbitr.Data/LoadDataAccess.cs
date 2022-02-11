using Arbitr.Data.Interfaces;
using Arbitr.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Arbitr.Data
{
    public class LoadDataAccess : ILoadDataAccess
    {
        private readonly string _arbitrConnectionString;

        public LoadDataAccess(string arbitrConnectionString)
        {
            _arbitrConnectionString = arbitrConnectionString ?? throw new ArgumentNullException(nameof(arbitrConnectionString));
        }

        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(_arbitrConnectionString);
        }

        #region Public Methods

        public List<Load> GetAllLoadsFromDatabase()
        {
            using var connection = GetSqlConnection();

            var selectAllLoadsCommand = connection.CreateCommand();
            selectAllLoadsCommand.CommandText = "[dbo].[SelectAllLoads]";
            selectAllLoadsCommand.CommandType = CommandType.StoredProcedure;

            connection.Open();

            var reader = selectAllLoadsCommand.ExecuteReader();

            var loads = new List<Load>();

            while (reader.Read())
            {
                var l = new Load
                {
                    LoadKey = (Guid)reader["LoadKey"],
                    OriginLocationId = (int)reader["OriginLocationId"],
                    DestinationLocationId = (int)reader["DestinationLocationId"],
                    PickupDate = (DateTimeOffset)reader["PickupDate"]
                };

                loads.Add(l);
            }

            return loads;
        }

        public void UpsertLoad(Guid loadKey, int originLocationId, int destinationLocationId, DateTimeOffset pickupDate)
        {
            if (CheckLoadExists(loadKey))
            {
                UpdateLoad(loadKey, originLocationId, destinationLocationId, pickupDate);
            }
            else
            {
                InsertLoad(loadKey, originLocationId, destinationLocationId, pickupDate);
            }
        }

        public void RemoveLoad(Guid loadKey)
        {
            using var connection = GetSqlConnection();

            var removeLoadCommand = connection.CreateCommand();
            removeLoadCommand.CommandText = "[dbo].[RemoveLoad]";
            removeLoadCommand.CommandType = CommandType.StoredProcedure;

            removeLoadCommand.Parameters.Add(new SqlParameter("@LoadKey", loadKey));

            connection.Open();

            removeLoadCommand.ExecuteNonQuery();
        }

        #endregion

        #region Private Methods

        private bool CheckLoadExists(Guid loadKey)
        {
            using var connection = GetSqlConnection();

            var checkLoadExistsCommand = connection.CreateCommand();
            checkLoadExistsCommand.CommandText = "[dbo].[CheckLoadExists]";
            checkLoadExistsCommand.CommandType = CommandType.StoredProcedure;

            checkLoadExistsCommand.Parameters.Add(new SqlParameter("@LoadKey", loadKey));

            connection.Open();

            return Convert.ToBoolean(checkLoadExistsCommand.ExecuteScalar());
        }

        private void InsertLoad(Guid loadKey, int originLocationId, int destinationLocationId, DateTimeOffset pickupDate)
        {
            using var connection = GetSqlConnection();

            var insertLoadCommand = connection.CreateCommand();
            insertLoadCommand.CommandText = "[dbo].[InsertLoad]";
            insertLoadCommand.CommandType = CommandType.StoredProcedure;

            insertLoadCommand.Parameters.Add(new SqlParameter("@LoadKey", loadKey));
            insertLoadCommand.Parameters.Add(new SqlParameter("@OriginLocationId", originLocationId));
            insertLoadCommand.Parameters.Add(new SqlParameter("@DestinationLocationId", destinationLocationId));
            insertLoadCommand.Parameters.Add(new SqlParameter("@PickupDate", pickupDate));

            connection.Open();

            insertLoadCommand.ExecuteNonQuery();
        }

        private void UpdateLoad(Guid loadKey, int originLocationId, int destinationLocationId, DateTimeOffset pickupDate)
        {
            using var connection = GetSqlConnection();

            var updateLoadCommand = connection.CreateCommand();
            updateLoadCommand.CommandText = "[dbo].[UpdateLoad]";
            updateLoadCommand.CommandType = CommandType.StoredProcedure;

            updateLoadCommand.Parameters.Add(new SqlParameter("@LoadKey", loadKey));
            updateLoadCommand.Parameters.Add(new SqlParameter("@OriginLocationId", originLocationId));
            updateLoadCommand.Parameters.Add(new SqlParameter("@DestinationLocationId", destinationLocationId));
            updateLoadCommand.Parameters.Add(new SqlParameter("@PickupDate", pickupDate));

            connection.Open();

            updateLoadCommand.ExecuteNonQuery();
        }

        #endregion
    }
}