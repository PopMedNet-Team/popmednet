using System;
using System.Linq;
using System.ServiceModel;
using Common.Logging;
using Quartz;

namespace Lpp.Dns.Portal.Notifications
{
    public abstract class PokeJob<TService> : IJob
    {
        private readonly string _serviceUrls;
        protected PokeJob( string serviceUrls ) { _serviceUrls = serviceUrls; }

        protected abstract void Poke( TService s );

        private static readonly ILog _logger = LogManager.GetLogger(typeof(TService));
        public void Execute(IJobExecutionContext context)
        {
            foreach ( var url in 
                (_serviceUrls??"")
                .Split( ';' )
                .Where( s => !string.IsNullOrEmpty( s ) )
                .Select( s => s.Trim() ) )
            {
                try
                {
                    _logger.Debug("Executing Poke Job at " + url);
                    var binding = new BasicHttpBinding();
                    if ( string.Equals( new Uri( url ).Scheme, "https" ) ) binding.Security = new BasicHttpSecurity { Mode = BasicHttpSecurityMode.Transport };

                    using ( var factory = new ChannelFactory<TService>( binding, url ) )
                    {
                        Poke( factory.CreateChannel() );
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