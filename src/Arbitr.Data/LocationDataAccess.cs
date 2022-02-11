using Arbitr.Data.Interfaces;
using Arbitr.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Arbitr.Data
{
    public class LocationDataAccess : ILocationDataAccess
    {
        private readonly string _arbitrConnectionString;

        public LocationDataAccess(string arbitrConnectionString)
        {
            _arbitrConnectionString = arbitrConnectionString ?? throw new ArgumentNullException(nameof(arbitrConnectionString));
        }

        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(_arbitrConnectionString);
        }

        #region Public Methods

        public List<Location> GetAllLocationsFromDatabase()
        {
            using var connection = GetSqlConnection();

            var selectAllLocationsCommand = connection.CreateCommand();
            selectAllLocationsCommand.CommandText = "[dbo].[SelectAllLocations]";
            selectAllLocationsCommand.CommandType = CommandType.StoredProcedure;

            connection.Open();

            var reader = selectAllLocationsCommand.ExecuteReader();

            var locations = new List<Location>();

            while (reader.Read())
            {
                var l = new Location
                {
                    LocationId = (int)reader["LocationId"],
                    City = (string)reader["City"],
                    State = (string)reader["State"],
                    Zip = (string)reader["Zip"],
                    Country = (string)reader["Country"],
                    Latitude = (decimal)reader["Latitude"],
                    Longitude = (decimal)reader["Longitude"]
                };

                locations.Add(l);
            }

            return locations;
        }

        public Location GetLocationByZipFromDatabase(string zip)
        {
            using var connection = GetSqlConnection();

            var selectLocationByZipCommand = connection.CreateCommand();
            selectLocationByZipCommand.CommandText = "[dbo].[SelectLocationByZip]";
            selectLocationByZipCommand.CommandType = CommandType.StoredProcedure;

            selectLocationByZipCommand.Parameters.Add(new SqlParameter("@Zip", zip));

            connection.Open();

            var reader = selectLocationByZipCommand.ExecuteReader();

            reader.Read();

            var l = new Location
            {
                LocationId = (int)reader["LocationId"],
                City = (string)reader["City"],
                State = (string)reader["State"],
                Country = (string)reader["Country"],
                Latitude = (decimal)reader["Latitude"],
                Longitude = (decimal)reader["Longitude"]
            };

            return l;
        }

        #endregion

    }
}