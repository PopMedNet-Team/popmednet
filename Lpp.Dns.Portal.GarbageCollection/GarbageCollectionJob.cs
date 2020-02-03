using System;
using System.Linq;
using System.ServiceModel;
using Common.Logging;
using Quartz;

namespace Lpp.Dns.Portal.GarbageCollection
{
    public class GarbageCollectionJob : IJob
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(GarbageCollectionJob));
        public void Execute(IJobExecutionContext context)
        {
            _logger.Info("Executing Garbage Collection Job at: " +  Properties.Settings.Default.GarbageCollectionWcfServiceUrl );
            foreach ( var url in 
                (Properties.Settings.Default.GarbageCollectionWcfServiceUrl??"")
                .Split( ';' )
                .Where( s => !string.IsNullOrEmpty( s ) )
                .Select( s => s.Trim() ) )
            {
                try
                {
                    var binding = new BasicHttpBinding();
                    if ( string.Equals( new Uri( url ).Scheme, "https" ) ) binding.Security = new BasicHttpSecurity { Mode = BasicHttpSecurityMode.Transport };

                    using ( var factory = new ChannelFactory<IGarbageCollectionWcfService>( binding, url ) )
                    {
                        factory.CreateChannel().CollectGarbage();
                    }
                }
                catch ( Exception ex )
                {
                    _logger.Error( ex.Message );
                }
            }
        }
    }
}