using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PopMedNet.DMCS.Data.Identity;
using PopMedNet.DMCS.Data.Model;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PopMedNet.DMCS.Utils
{
    public class DbMigrator : BackgroundService
    {
        readonly IServiceProvider services;
        readonly ILogger logger;
        readonly ApplicationParameters _appParams;

        public DbMigrator(IServiceProvider services, ILogger logger, ApplicationParameters appParams)
        {
            this.services = services;
            //this.logger = logger.ForContext("SourceContext", typeof(DbMigrator));
            this.logger = logger.ForContext<DbMigrator>();
            this._appParams = appParams;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.Debug("Checking the database to see if migrations need to be applied");

            using (var scope = this.services.CreateScope())
            {
                var identDb = scope.ServiceProvider.GetRequiredService<IdentityContext>();
                var modelDb = scope.ServiceProvider.GetRequiredService<ModelContext>();

                var identMigs = await identDb.Database.GetPendingMigrationsAsync();
                var modelMigs = await modelDb.Database.GetPendingMigrationsAsync();

                if ((identMigs).Count() > 0)
                {
                    this.logger.Information($"Migrating the database to apply the following migrations for the Identity Context: {string.Join(',', identMigs)}");
                    try
                    {
                        await identDb.Database.MigrateAsync();
                    }
                    catch (Exception ex)
                    {
                        this.logger.Error(ex, "An error occured while migrating the database for the Identity Context");
                        throw;
                    }
                }

                if ((modelMigs).Count() > 0)
                {
                    this.logger.Information($"Migrating the database to apply the following migrations for the Model Context: {string.Join(',', modelMigs)}");
                    try
                    {
                        await modelDb.Database.MigrateAsync();
                    }
                    catch (Exception ex)
                    {
                        this.logger.Error(ex, "An error occured while migrating the database for the Model Context");
                        throw;
                    }
                }
            }

            _appParams.DbMigrationComplete = true;
            logger.Information("Database migration check complete");
        }
    }
}
