using Arbitr.Core;
using Arbitr.Data;
using Arbitr.Repositories;
using System;
using System.Linq;

namespace Arbitr.Worker.Matcher
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            var arbitrDbConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Arbitr");
            var locationRepository = new LocationRepository(new LocationDataAccess(arbitrDbConnectionString));
            locationRepository.InitializeAllKnownLocations();
            var knownLocations = locationRepository.KnownLocations.Values.ToList();
            var truckRepository = new TruckRepository(new TruckDataAccess(arbitrDbConnectionString), knownLocations);
            var loadRepository = new LoadRepository(new LoadDataAccess(arbitrDbConnectionString), knownLocations);
            var matchRepository = new MatchRepository(new MatchDataAccess(arbitrDbConnectionString));
            var magicMaker = new MagicMaker();

            var trucks = truckRepository.GetAllTrucks();
            var loads = loadRepository.GetAllLoads();
            var matches = magicMaker.MakeMagic(trucks, loads);

            matches.ForEach(m => Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(m)));

            matchRepository.SaveAllMatches(matches);

            sw.Stop();
            Console.WriteLine("*************************************************************");
            Console.WriteLine("**                       YEP IT RAN                        **");
            Console.WriteLine("**                       THIS FAST:                        **");
            Console.WriteLine($"**                       {sw.Elapsed}                  **");
            Console.WriteLine("*************************************************************");
        }
    }
}