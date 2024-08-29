using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using PopMedNet.DMCS.Code;
using Serilog;

namespace PopMedNet.DMCS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext()
                    .WriteTo.DMCSSerilogSink(context.Configuration, null, services)
                    
                 )
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
