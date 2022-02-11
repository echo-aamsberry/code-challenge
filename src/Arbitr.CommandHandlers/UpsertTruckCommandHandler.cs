using Arbitr.Commands;
using Arbitr.Data.Interfaces;
using MassTransit;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Arbitr.CommandHandlers
{
    public class UpsertTruckCommandHandler : IConsumer<UpsertTruckCommand>
    {
        private readonly ITruckDataAccess _truckDataAccess;

        public UpsertTruckCommandHandler(ITruckDataAccess truckDataAccess)
        {
            _truckDataAccess = truckDataAccess ?? throw new ArgumentNullException(nameof(truckDataAccess));
        }

        public Task Consume(ConsumeContext<UpsertTruckCommand> context)
        {
            var command = context.Message;
            _truckDataAccess.UpsertTruck(command.TruckKey, command.OriginLocationId, command.PossibleDestinationLocationIds, command.AvailableDate);
            Log.Information($"Processed UpsertTruckCommand: {command.TruckKey}");
            return Task.CompletedTask;
        }
    }
}