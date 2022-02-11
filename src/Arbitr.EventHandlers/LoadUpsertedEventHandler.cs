using Arbitr.ClutchEvents;
using MassTransit;
using System.Threading.Tasks;

namespace Arbitr.EventHandlers
{
    public class LoadUpsertedEventHandler : IConsumer<LoadUpsertedEvent>
    {
        public Task Consume(ConsumeContext<LoadUpsertedEvent> context)
        {
            return Task.CompletedTask;
        }
    }
}