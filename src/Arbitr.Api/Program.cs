using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Json;
using System;

namespace Arbitr.Api
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddCommandLine(args)
                    .AddEnvironmentVariables()
                    .Build();

                Log.Logger = new LoggerConfiguration().WriteTo.Console(new JsonFormatter()).CreateLogger();

                var host = Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseConfiguration(config);
                        webBuilder.UseStartup<Startup>();
                    })
                    .UseSerilog()
                    .Build();

                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"Host terminated - {ex.Message}");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
