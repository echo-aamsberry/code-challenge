using Arbitr.Data.Interfaces;
using Arbitr.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Arbitr.Data
{
    public class MatchDataAccess : IMatchDataAccess
    {
        private readonly string _arbitrConnectionString;

        public MatchDataAccess(string arbitrConnectionString)
        {
            _arbitrConnectionString = arbitrConnectionString ?? throw new ArgumentNullException(nameof(arbitrConnectionString));
        }

        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(_arbitrConnectionString);
        }

        #region Public Methods

        public List<Match> GetAllMatchesFromDatabase()
        {
            using var connection = GetSqlConnection();

            var selectAllMatchesCommand = connection.CreateCommand();
            selectAllMatchesCommand.CommandText = "[dbo].[SelectAllMatches]";
            selectAllMatchesCommand.CommandType = CommandType.StoredProcedure;

            connection.Open();

            var reader = selectAllMatchesCommand.ExecuteReader();

            var matches = new List<Match>();

            while (reader.Read())
            {
                var l = new Match
                {
                    TruckKey = (Guid)reader["TruckKey"],
                    LoadKey = (Guid)reader["LoadKey"],
                    Matchiness = (byte)reader["Matchiness"]
                };

                matches.Add(l);
            }

            return matches;
        }

        public void SaveAllMatches(List<Match> matches)
        {
            using var connection = GetSqlConnection();

            var saveAllMatchesCommand = connection.CreateCommand();
            saveAllMatchesCommand.CommandText = "[dbo].[SaveAllMatches]";
            saveAllMatchesCommand.CommandType = CommandType.StoredProcedure;

            var matchTypeTable = GetMatchTypeTableFromDataModelCollection(matches);

            var matchesParameter = new SqlParameter("@Matches", SqlDbType.Structured)
            {
                TypeName = "[dbo].[MatchType]",
                Value = matchTypeTable
            };

            saveAllMatchesCommand.Parameters.Add(matchesParameter);

            connection.Open();

            saveAllMatchesCommand.ExecuteNonQuery();
        }

        public void RemoveMatchesForLoad(Guid loadKey)
        {
            using var connection = GetSqlConnection();

            var removeMatchesForLoadCommand = connection.CreateCommand();
            removeMatchesForLoadCommand.CommandText = "[dbo].[RemoveMatchesForLoad]";
            removeMatchesForLoadCommand.CommandType = CommandType.StoredProcedure;
            
            removeMatchesForLoadCommand.Parameters.Add(new SqlParameter("@LoadKey", loadKey));

            connection.Open();

            removeMatchesForLoadCommand.ExecuteNonQuery();
        }

        public void RemoveMatchesForTruck(Guid truckKey)
        {
            using var connection = GetSqlConnection();

            var removeMatchesForTruckCommand = connection.CreateCommand();
            removeMatchesForTruckCommand.CommandText = "[dbo].[RemoveMatchesForTruck]";
            removeMatchesForTruckCommand.CommandType = CommandType.StoredProcedure;

            removeMatchesForTruckCommand.Parameters.Add(new SqlParameter("@TruckKey", truckKey));

            connection.Open();

            removeMatchesForTruckCommand.ExecuteNonQuery();
        }

        #endregion

        #region Private Methods

        private DataTable GetMatchTypeTableFromDataModelCollection(List<Match> matches)
        {
            using var typeMatches = new DataTable();

            typeMatches.Columns.Add("TruckKey", typeof(Guid));
            typeMatches.Columns.Add("LoadKey", typeof(Guid));
            typeMatches.Columns.Add("Matchiness", typeof(byte));

            matches.ForEach(m => typeMatches.Rows.Add(new object[] { m.TruckKey, m.LoadKey, m.Matchiness }));

            return typeMatches;
        }

        #endregion
    }
}