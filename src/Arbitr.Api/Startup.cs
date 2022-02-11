using Arbitr.Data;
using Arbitr.Data.Interfaces;
using Arbitr.MassTransit;
using Arbitr.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Arbitr.Api
{
    public class Startup
    {
        public readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var arbitrDbConnectionString = Configuration.GetSection("ConnectionStrings").GetValue<string>("Arbitr");

            var locationDataAccess = new LocationDataAccess(arbitrDbConnectionString);
            services.AddSingleton<ILocationDataAccess>(locationDataAccess);
            services.AddSingleton<ITruckDataAccess>(new TruckDataAccess(arbitrDbConnectionString));
            services.AddSingleton<ILoadDataAccess>(new LoadDataAccess(arbitrDbConnectionString));
            services.AddSingleton(new LocationRepository(locationDataAccess));

            ConfigureMassTransit(services);

            services.AddControllers();

            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
			
			app.UseHealthChecks("/_health");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureMassTransit(IServiceCollection services)
        {
            services.Configure<RabbitMqSettings>(Configuration.GetSection(RabbitMqSettings.KEY));

            services.AddMassTransit(cfg =>
            {
                cfg.AddBus(ApiSetup.ConfigureBus);
            });
        }
    }
}
