using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PopMedNet.CDM.Population.CDM;
using Serilog;
using System;
using System.IO;

namespace PopMedNet.CDM.Population
{
    class Program
    {
        public static void Main(string[] args)
        {
            //Console.WriteLine(typeof(MSSQLSourceCDM).AssemblyQualifiedName);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("appsettings.json", optional: true);
                    configHost.AddCommandLine(args);
                })
                .UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext()
                 )
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<CDMUpdater>();
                });
    }
}
