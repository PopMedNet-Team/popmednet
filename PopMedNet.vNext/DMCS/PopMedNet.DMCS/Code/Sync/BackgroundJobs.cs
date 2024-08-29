using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PopMedNet.DMCS.Code
{
    public class BackgroundJobs : BackgroundService
    {
        private readonly IServiceProvider services;
        private readonly ILogger logger;
        readonly ApplicationParameters _appParams;

        public BackgroundJobs(IServiceProvider services, ILogger logger, ApplicationParameters appParams)
        {
            this.services = services;
            this.logger = logger.ForContext<BackgroundJobs>();
            _appParams = appParams;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                while(_appParams.DbMigrationComplete == false)
                {
                    logger.Debug("Waiting for DbMigrator to update database");
                    await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                }

                try
                {                    
                    using (var scope = this.services.CreateScope())
                    {
                        var userSync = scope.ServiceProvider.GetRequiredService<UserSync>();
                        await userSync.Execute();

                        this.logger.Information("Starting to execute the Background Sync Service");
                        var syncService = scope.ServiceProvider.GetRequiredService<SyncService>();
                        await syncService.UpdateDatamarts();
                    }
                }
                catch (Exception ex)
                {
                    this.logger.Error(ex, "An error occurred when executing the sync service");
                }

                await Task.Delay(_appParams.SyncServiceInterval, stoppingToken);
            }
           
        }
    }
}
