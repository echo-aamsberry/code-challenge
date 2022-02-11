using Arbitr.Commands;
using Arbitr.Data.Interfaces;
using MassTransit;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Arbitr.CommandHandlers
{
    public class UpsertLoadCommandHandler : IConsumer<UpsertLoadCommand>
    {
        private readonly ILoadDataAccess _loadDataAccess;

        public UpsertLoadCommandHandler(ILoadDataAccess loadDataAccess)
        {
            _loadDataAccess = loadDataAccess ?? throw new ArgumentNullException(nameof(loadDataAccess));
        }

        public Task Consume(ConsumeContext<UpsertLoadCommand> context)
        {
            var command = context.Message;
            _loadDataAccess.UpsertLoad(command.LoadKey, command.OriginLocationId, command.DestinationLocationId, command.PickupDate);
            Log.Debug($"Processed UpsertLoadCommand: {command.LoadKey}");
            return Task.CompletedTask;
        }
    }
}