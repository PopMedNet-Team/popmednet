using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PopMedNet.CDM.Population.CDM;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PopMedNet.CDM.Population
{
    public class CDMUpdater : IHostedService
    {
        private readonly IConfiguration Configuration;
        private readonly ILogger Logger;
        private readonly IHostApplicationLifetime Lifetime;

        public CDMUpdater(IConfiguration configuration, ILogger logger, IHostApplicationLifetime lifetime)
        {
            this.Configuration = configuration;
            this.Logger = logger;
            this.Lifetime = lifetime;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            this.Logger.Information("Starting Up CDM Updater.");
            Setting settings = new();
            this.Configuration.Bind(settings);

            var sourceType = Type.GetType(settings.Source.ClassType);

            var sourceCdm = Activator.CreateInstance(sourceType, new object[] { settings.Source, this.Logger }) as ISourceCDM;

            await sourceCdm.InitializeAsync();
            var source = await sourceCdm.RetrieveSourceRecordsAsync(cancellationToken);

            foreach (var replicaSetting in settings.Replicas)
            {
                var replicaType = Type.GetType(replicaSetting.ClassType);

                var replicaCdm = Activator.CreateInstance(replicaType, new object[] { replicaSetting, this.Logger }) as IReplicaCDM;

                await replicaCdm.InitializeAsync();

                await replicaCdm.PopulateAsync(source);
            }

            this.Lifetime.StopApplication();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            
        }
    }
}
