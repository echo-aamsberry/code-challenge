using Arbitr.Commands;
using Arbitr.Data.Interfaces;
using MassTransit;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Arbitr.CommandHandlers
{
    public class RemoveTruckCommandHandler : IConsumer<RemoveTruckCommand>
    {
        private readonly ITruckDataAccess _truckDataAccess;
        private readonly IMatchDataAccess _matchDataAccess;

        public RemoveTruckCommandHandler(ITruckDataAccess truckDataAccess, IMatchDataAccess matchDataAccess)
        {
            _truckDataAccess = truckDataAccess ?? throw new ArgumentNullException(nameof(truckDataAccess));
            _matchDataAccess = matchDataAccess ?? throw new ArgumentNullException(nameof(matchDataAccess));
        }

        public Task Consume(ConsumeContext<RemoveTruckCommand> context)
        {
            var command = context.Message;
            _matchDataAccess.RemoveMatchesForTruck(command.TruckKey);
            _truckDataAccess.RemoveTruck(command.TruckKey);
            Log.Debug($"Processed RemoveTruckCommand: {command.TruckKey}");
            return Task.CompletedTask;
        }
    }
}