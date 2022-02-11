using Arbitr.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using core = Arbitr.Core.Models;
using data = Arbitr.Data.Models;

namespace Arbitr.Repositories
{
    public class MatchRepository
    {
        private IMatchDataAccess _matchDataAccess;

        public MatchRepository(IMatchDataAccess matchDataAccess)
        {
            _matchDataAccess = matchDataAccess ?? throw new ArgumentNullException(nameof(matchDataAccess));
        }

        public List<core.Match> GetAllMatches()
        {
            var dbMatches = _matchDataAccess.GetAllMatchesFromDatabase();

            var matches = new List<core.Match>();

            foreach (var dbMatch in dbMatches)
            {
                matches.Add(MapMatchDataToCore(dbMatch));
            }

            return matches;
        }

        public void SaveAllMatches(List<core.Match> matches)
        {
            var dbMatches = new List<data.Match>();
            matches.ForEach(m => dbMatches.Add(MapMatchCoreToData(m)));
            _matchDataAccess.SaveAllMatches(dbMatches);
        }

        private data.Match MapMatchCoreToData(core.Match match)
        {
            return new data.Match()
            {
                TruckKey = match.TruckKey,
                LoadKey = match.LoadKey,
                Matchiness = match.Matchiness
            };
        }

        private core.Match MapMatchDataToCore(data.Match match)
        {
            return new core.Match()
            {
                TruckKey = match.TruckKey,
                LoadKey = match.LoadKey,
                Matchiness = match.Matchiness
            };
        }

    }
}