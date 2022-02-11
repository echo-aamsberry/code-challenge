using Arbitr.ClutchEvents;
using MassTransit;
using System.Threading.Tasks;

namespace Arbitr.EventHandlers
{
    public class TruckUpsertedEventHandler : IConsumer<TruckUpsertedEvent>
    {
        public Task Consume(ConsumeContext<TruckUpsertedEvent> context)
        {
            return Task.CompletedTask;
        }
    }
}