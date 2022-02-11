using Arbitr.EventHandlers;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;

namespace Arbitr.MassTransit
{
    public class ClutchTranslatorWorkerSetup
    {
        public static void ConfigureMassTransit(
            IServiceCollectionBusConfigurator configurator,
            RabbitMqSettings rabbitMqSettings)
        {
            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqSettings.Host, h =>
                {
                    h.Username(rabbitMqSettings.Username);
                    h.Password(rabbitMqSettings.Password);
                });

                cfg.ReceiveEndpoint("Arbitr.TruckUpsertedEventHandler", e =>
                {
                    e.Consumer(() => new TruckUpsertedEventHandler());
                });

                cfg.ReceiveEndpoint("Arbitr.TruckRemovedEventHandler", e =>
                {
                    e.Consumer(() => new TruckRemovedEventHandler());
                });

                cfg.ReceiveEndpoint("Arbitr.LoadUpsertedEventHandler", e =>
                {
                    e.Consumer(() => new LoadUpsertedEventHandler());
                });

                cfg.ReceiveEndpoint("Arbitr.LoadRemovedEventHandler", e =>
                {
                    e.Consumer(() => new LoadRemovedEventHandler());
                });
            });
        }
    }
}