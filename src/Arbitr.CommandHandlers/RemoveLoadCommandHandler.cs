using Arbitr.Commands;
using Arbitr.Data.Interfaces;
using MassTransit;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Arbitr.CommandHandlers
{
    public class RemoveLoadCommandHandler : IConsumer<RemoveLoadCommand>
    {
        private readonly ILoadDataAccess _loadDataAccess;
        private readonly IMatchDataAccess _matchDataAccess;

        public RemoveLoadCommandHandler(ILoadDataAccess loadDataAccess, IMatchDataAccess matchDataAccess)
        {
            _loadDataAccess = loadDataAccess ?? throw new ArgumentNullException(nameof(loadDataAccess));
            _matchDataAccess = matchDataAccess ?? throw new ArgumentNullException(nameof(matchDataAccess));
        }

        public Task Consume(ConsumeContext<RemoveLoadCommand> context)
        {
            var command = context.Message;
            _matchDataAccess.RemoveMatchesForLoad(command.LoadKey);
            _loadDataAccess.RemoveLoad(command.LoadKey);
            Log.Debug($"Processed RemoveLoadCommand: {command.LoadKey}");
            return Task.CompletedTask;
        }
    }
}