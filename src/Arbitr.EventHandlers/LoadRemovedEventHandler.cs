using Arbitr.ClutchEvents;
using MassTransit;
using System.Threading.Tasks;

namespace Arbitr.EventHandlers
{
    public class LoadRemovedEventHandler : IConsumer<LoadRemovedEvent>
    {
        public Task Consume(ConsumeContext<LoadRemovedEvent> context)
        {
            return Task.CompletedTask;
        }
    }
}