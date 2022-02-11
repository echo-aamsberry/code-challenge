using Arbitr.ClutchEvents;
using MassTransit;
using System.Threading.Tasks;

namespace Arbitr.EventHandlers
{
    public class TruckRemovedEventHandler : IConsumer<TruckRemovedEvent>
    {
        public Task Consume(ConsumeContext<TruckRemovedEvent> context)
        {
            return Task.CompletedTask;
        }
    }
}