using Arbitr.ClutchEvents;
using MassTransit;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Arbitr.IntegrationTests
{
    public class DevHelperTests
    {
        private IBusControl _bus;

        private const int TRUCK_ID = 7;
        private const int LOAD_ID = 23;

        [SetUp]
        public void Setup()
        {
            _bus = Bus.Factory.CreateUsingRabbitMq(cfg => cfg.Host("localhost", h =>
            {
                h.Username("guest");
                h.Password("guest");
            }));
        }

        [Test]
        public void PublishTruckUpsertedEvent_Test()
        {
            var publishTask = _bus.Publish(new TruckUpsertedEvent() { TruckId = TRUCK_ID, OriginLocationId = 1, AvailableDate = DateTimeOffset.Now, PossibleDestinationLocationIds = new List<int>() });
            publishTask.Wait();

            Assert.Pass();
        }

        [Test]
        public void PublishTruckRemovedEvent_Test()
        {
            var publishTask = _bus.Publish(new TruckRemovedEvent() { TruckId = TRUCK_ID });
            publishTask.Wait();

            Assert.Pass();
        }

        [Test]
        public void PublishLoadUpsertedEvent_Test()
        {
            var publishTask = _bus.Publish(new LoadUpsertedEvent() { LoadId = LOAD_ID, OriginLocationId = 1, AvailableDate = DateTimeOffset.Now, DestinationLocationId = 2 });
            publishTask.Wait();

            Assert.Pass();
        }

        [Test]
        public void PublishLoadRemovedEvent_Test()
        {
            var publishTask = _bus.Publish(new LoadRemovedEvent() { LoadId = LOAD_ID });
            publishTask.Wait();

            Assert.Pass();
        }
    }
}