using Arbitr.Api.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Arbitr.UnitTests
{
    public class UnitTest1
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var x = new Truck();
            x.AvailableDate = DateTimeOffset.UtcNow.AddDays(2);
            x.OriginZip = "60654";
            x.PossibleDestinationZips = new List<string>();
            x.PossibleDestinationZips.Add("89109");
            x.PossibleDestinationZips.Add("90007");
            x.PossibleDestinationZips.Add("10001");
            x.TruckKey = Guid.NewGuid();
            var z = JsonSerializer.Serialize(x).ToString();

            Assert.Pass();
        }
    }
}