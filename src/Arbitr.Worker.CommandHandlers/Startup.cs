using Arbitr.Data;
using Arbitr.Data.Interfaces;
using Arbitr.MassTransit;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.Common;

namespace Arbitr.Worker.CommandHandlers
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

            var truckDataAccess = new TruckDataAccess(arbitrDbConnectionString);
            var loadDataAccess = new LoadDataAccess(arbitrDbConnectionString);
            var matchDataAccess = new MatchDataAccess(arbitrDbConnectionString);

            services.AddSingleton<ITruckDataAccess>(truckDataAccess);
            services.AddSingleton<ILoadDataAccess>(loadDataAccess);
            services.AddSingleton<IMatchDataAccess>(matchDataAccess);

            var rabbitMqSettings = new RabbitMqSettings();
            Configuration.GetSection(RabbitMqSettings.KEY).Bind(rabbitMqSettings);

            services.AddMassTransit(cfg => CommandHandlerWorkerSetup.ConfigureMassTransit(cfg, rabbitMqSettings, truckDataAccess, loadDataAccess, matchDataAccess));

            services.AddMassTransitHostedService();            

            services.AddHealthChecks()
                .AddRabbitMQ($"amqp://{rabbitMqSettings.Username}:{rabbitMqSettings.Password}@{rabbitMqSettings.Host}", name: "rabbitmq-check", tags: new string[] { "ready", "rabbitmq" })
                .AddSqlServer(arbitrDbConnectionString, name: "Arbitr", tags: new[] { "ready", "sql" });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHealthChecks("/_health");
        }
    }
}