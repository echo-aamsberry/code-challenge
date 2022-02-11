using Arbitr.MassTransit;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Arbitr.Worker.ClutchTranslator
{
    public class Startup
    {
        public readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var arbitrDbConnectionString = Configuration.GetSection("ConnectionStrings").GetValue<string>("Arbitr");

            var rabbitMqSettings = new RabbitMqSettings();
            Configuration.GetSection(RabbitMqSettings.KEY).Bind(rabbitMqSettings);

            var bus = Bus.Factory.CreateUsingRabbitMq(cfg => cfg.Host(rabbitMqSettings.Host, h =>
            {
                h.Username(rabbitMqSettings.Username);
                h.Password(rabbitMqSettings.Password);
            }));

            services.AddMassTransit(cfg => ClutchTranslatorWorkerSetup.ConfigureMassTransit(cfg, rabbitMqSettings));

            services.AddMassTransitHostedService();

            services.AddHealthChecks().AddSqlServer(arbitrDbConnectionString);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHealthChecks("/_health");
        }
    }
}