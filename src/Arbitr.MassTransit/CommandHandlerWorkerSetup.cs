using Arbitr.CommandHandlers;
using Arbitr.Data.Interfaces;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;

namespace Arbitr.MassTransit
{
    public class CommandHandlerWorkerSetup
    {
        public static void ConfigureMassTransit(
            IServiceCollectionBusConfigurator configurator,
            RabbitMqSettings rabbitMqSettings,
            ITruckDataAccess truckDataAccess,
            ILoadDataAccess loadDataAccess,
            IMatchDataAccess matchDataAccess)
        {
            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqSettings.Host, h =>
                {
                    h.Username(rabbitMqSettings.Username);
                    h.Password(rabbitMqSettings.Password);
                });

                cfg.ReceiveEndpoint("Arbitr.UpsertTruckCommandHandler", e =>
                {
                    e.Consumer(() => new UpsertTruckCommandHandler(truckDataAccess));
                });

                cfg.ReceiveEndpoint("Arbitr.RemoveTruckCommandHandler", e =>
                {
                    e.Consumer(() => new RemoveTruckCommandHandler(truckDataAccess, matchDataAccess));
                });

                cfg.ReceiveEndpoint("Arbitr.UpsertLoadCommandHandler", e =>
                {
                    e.Consumer(() => new UpsertLoadCommandHandler(loadDataAccess));
                });

                cfg.ReceiveEndpoint("Arbitr.RemoveLoadCommandHandler", e =>
                {
                    e.Consumer(() => new RemoveLoadCommandHandler(loadDataAccess, matchDataAccess));
                });
            });
        }
    }
}