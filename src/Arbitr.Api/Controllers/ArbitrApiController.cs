using Arbitr.Api.Models;
using Arbitr.Commands;
using Arbitr.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arbitr.Api
{
    [ApiController]
    public class ArbitrApiController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly LocationRepository _locationRepository;

        public ArbitrApiController(IBus bus, LocationRepository locationRepository)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _locationRepository = locationRepository ?? throw new ArgumentNullException(nameof(locationRepository));
        }

        [HttpPut]
        [Route("match/truck")]
        public void MatchTruck([FromBody] Truck truck)
        {
            var originLocationId = _locationRepository.GetLocationId(truck.OriginZip);

            var possibleDestinationIds = new List<int>();

            foreach (var z in truck.PossibleDestinationZips)
            {
                var destId = _locationRepository.GetLocationId(z);
                possibleDestinationIds.Add(destId);
            }

            var matchTruckCommand = new UpsertTruckCommand(truck.TruckKey, originLocationId, possibleDestinationIds, truck.AvailableDate);
            _bus.Publish(matchTruckCommand);
        }

        [HttpDelete]
        [Route("match/truck/{TruckKey}")]
        public void RemoveTruck(Guid truckKey)
        {
            var removeTruckCommand = new RemoveTruckCommand(truckKey);
            _bus.Publish(removeTruckCommand);
        }

        [HttpPut]
        [Route("match/load")]
        public void MatchLoad([FromBody] Load load)
        {
            var originLocationId = _locationRepository.GetLocationId(load.OriginZip);
            var destinationLocationId = _locationRepository.GetLocationId(load.DestinationZip);
            var matchLoadCommand = new UpsertLoadCommand(load.LoadKey, originLocationId, destinationLocationId, load.PickupDate);
            _bus.Publish(matchLoadCommand);
        }

        [HttpDelete]
        [Route("match/load/{LoadKey}")]
        public void RemoveLoad(Guid loadKey)
        {
            var removeLoadCommand = new RemoveLoadCommand(loadKey);
            _bus.Publish(removeLoadCommand);
        }

        #region Testing Stuff

        [HttpPost]
        [Route("test/setup")]
        public void SetupTestData(SetupTestDataArgs args)
        {
            for (int i = 0; i < args.NumberOfTrucks; i++)
            {
                MatchTruck(GenerateRandomTruck());
            }

            for (int i = 0; i < args.NumberOfLoads; i++)
            {
                MatchLoad(GenerateRandomLoad());
            }
        }

        private Truck GenerateRandomTruck()
        {
            var possibleDestinationZips = new List<string>();

            for (int i = 0; i < new Random().Next(4); i++)
            {
                var randomZip = GetRandomZip();
                if (!possibleDestinationZips.Contains(randomZip))
                {
                    possibleDestinationZips.Add(randomZip);
                }
            }

            var truck = new Truck
            {
                TruckKey = Guid.NewGuid(),
                OriginZip = GetRandomZip(),
                PossibleDestinationZips = possibleDestinationZips,
                AvailableDate = DateTimeOffset.Now.AddDays(new Random().Next(7))
            };

            return truck;
        }

        private Load GenerateRandomLoad()
        {
            var load = new Load
            {
                LoadKey = Guid.NewGuid(),
                OriginZip = GetRandomZip(),
                DestinationZip = GetRandomZip(),
                PickupDate = DateTimeOffset.Now.AddDays(new Random().Next(7))
            };

            return load;
        }

        private string GetRandomZip()
        {
            var locations = new List<string>
            {
                "60654", // Chicago, IL
                "89109", // Las Vegas, NV
                "90007", // Los Angeles, CA
                "10001", // New York, NY
                "76011", // Dallas, TX
                "78205", // San Antonio, TX
                "33140"  // Miami, FL
            };

            return locations[new Random().Next(locations.Count())];
        }

        public class SetupTestDataArgs
        {
            public int NumberOfTrucks { get; set; }
            public int NumberOfLoads { get; set; }
        }

        #endregion

    }
}