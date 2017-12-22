using System;
using System.Linq;
using System.ServiceModel;
using Common.Logging;
using Quartz;

namespace Lpp.Dns.Portal.RequestMetadataCollection
{
    public class RequestMetadataCollectionJob : IJob
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(RequestMetadataCollectionJob));
        public void Execute(IJobExecutionContext context)
        {
            _logger.Info("Invoking Request Metadata Job at " +  Properties.Settings.Default.RequestMetadataCollectionWcfServiceUrl );
            foreach ( var url in
                (Properties.Settings.Default.RequestMetadataCollectionWcfServiceUrl ?? "")
                .Split( ';' )
                .Where( s => !string.IsNullOrEmpty( s ) )
                .Select( s => s.Trim() ) )
            {
                try
                {
                    var binding = new BasicHttpBinding();
                    if ( string.Equals( new Uri( url ).Scheme, "https" ) ) binding.Security = new BasicHttpSecurity { Mode = BasicHttpSecurityMode.Transport };

                    using ( var factory = new ChannelFactory<IRequestMetadataCollectionWcfService>( binding, url ) )
                    {
                        factory.CreateChannel().RequestMetadata();
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