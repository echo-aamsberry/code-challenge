using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Arbitr.MassTransit
{
    public class ApiSetup
    {
        public static IBusControl ConfigureBus(IServiceProvider provider)
        {
            var rabbitMqSettings = provider.GetRequiredService<Microsoft.Extensions.Options.IOptions<RabbitMqSettings>>().Value;

            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(rabbitMqSettings.Host, h =>
                {
                    h.Username(rabbitMqSettings.Username);
                    h.Password(rabbitMqSettings.Password);
                });
            });
        }
    }
}