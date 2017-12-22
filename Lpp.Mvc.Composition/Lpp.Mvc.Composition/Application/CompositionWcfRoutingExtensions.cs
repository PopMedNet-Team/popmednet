using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using Lpp.Composition;
using System.Web.Routing;
using System.ServiceModel.Activation;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ComponentModel.Composition.Hosting;
using System.Web;
using System.ServiceModel.Channels;

namespace Lpp.Mvc
{
    public static class CompositionWcfRoutingExtensions
    {
        public static void MapSoapService<TServiceImplementation, TServiceContract>( 
            this RouteCollection routes, string uriPrefix, Action<IServiceConfiguration> configureServiceHost = null )
            where TServiceImplementation : class, TServiceContract, new()
        {
            //Contract.Requires( routes != null );
            //Contract.Requires( !String.IsNullOrEmpty( uriPrefix ) );
            routes.MapService<TServiceImplementation, TServiceContract>( uriPrefix, configureServiceHost, false );
        }

        public static void MapRestService<TServiceImplementation, TServiceContract>(
            this RouteCollection routes, string uriPrefix, Action<IServiceConfiguration> configureServiceHost = null )
            where TServiceImplementation : class, TServiceContract, new()
        {
            //Contract.Requires( routes != null );
            //Contract.Requires( !String.IsNullOrEmpty( uriPrefix ) );
            routes.MapService<TServiceImplementation, TServiceContract>( uriPrefix, configureServiceHost, true );
        }

        static void MapService<TServiceImplementation, TServiceContract>(
            this RouteCollection routes, string uriPrefix, Action<IServiceConfiguration> configureServiceHost, bool isRest )
            where TServiceImplementation : class, TServiceContract, new()
        {
            //Contract.Requires( routes != null );
            //Contract.Requires( !String.IsNullOrEmpty( uriPrefix ) );

            routes.Add( new ServiceRoute( uriPrefix,
                new AnonymousServiceHostFactory( (serviceType, baseAddresses) =>
                {
                    if ( serviceType != typeof( TServiceImplementation ) ) throw new NotSupportedException();
                    var host = new SingleContractHttpServiceHost<TServiceImplementation, TServiceContract>( 
                        _ => HttpContext.Current.Composition().Compose( new TServiceImplementation() ),
                        baseAddresses, isRest );
                    if ( configureServiceHost != null ) configureServiceHost( host );
                    return host;
                } ), 
                typeof( TServiceImplementation ) ) );
        }
    }

    public class AnonymousServiceHostFactory : ServiceHostFactory
    {
        private readonly Func<Type, Uri[], ServiceHost> _create;

        public AnonymousServiceHostFactory( Func<Type, Uri[], ServiceHost> create )
        {
            //Contract.Requires( create != null );
            _create = create;
        }

        protected override ServiceHost CreateServiceHost( Type serviceType, Uri[] baseAddresses )
        {
            return _create( serviceType, baseAddresses );
        }
    }

    public class SingleContractHttpServiceHost<TServiceImplementation, TServiceContract> : ServiceHost, IServiceConfiguration
        where TServiceImplementation : class, TServiceContract
    {
        private readonly bool _isRest;

        public ServiceHost Host { get { return this; } }
        public Action<ServiceEndpoint> ConfigureEndpoint { get; set; }

        public SingleContractHttpServiceHost( Func<InstanceContext, object> createInstance, Uri[] baseAddresses, bool isRest )
            : base( typeof( TServiceImplementation ), baseAddresses )
        {
            //Contract.Requires( createInstance != null );
            //Contract.Requires( baseAddresses != null );
            //Contract.Requires( baseAddresses.Any() );

            _isRest = isRest;
            this.Description.Behaviors.Add( new AnonymousInstanceProvider( createInstance ) );

            if ( !_isRest )
            {
                var md = Description.Behaviors.Find<ServiceMetadataBehavior>();
                if ( md == null ) Description.Behaviors.Add( md = new ServiceMetadataBehavior() );
                md.HttpsGetEnabled = baseAddresses.Any( a => string.Equals( a.Scheme, "https", StringComparison.InvariantCultureIgnoreCase ) );
                md.HttpGetEnabled = baseAddresses.Any( a => string.Equals( a.Scheme, "http", StringComparison.InvariantCultureIgnoreCase ) );
            }

            var aspnet = Description.Behaviors.Find<AspNetCompatibilityRequirementsAttribute>();
            if ( aspnet == null ) Description.Behaviors.Add( aspnet = new AspNetCompatibilityRequirementsAttribute() );
            aspnet.RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed;
        }

        protected override ServiceDescription CreateDescription( out IDictionary<string, ContractDescription> implementedContracts )
        {
            var result = base.CreateDescription( out implementedContracts );
            implementedContracts = 
                implementedContracts
                .Where( kv => kv.Value.ContractType == typeof( TServiceContract ) )
                .ToDictionary( kv => kv.Key, kv => kv.Value );
            return result;
        }

        public override System.Collections.ObjectModel.ReadOnlyCollection<ServiceEndpoint> AddDefaultEndpoints()
        {
            var res = from contract in this.ImplementedContracts.Values
                      from url in this.BaseAddresses
                      let secure = string.Equals( url.Scheme, "https", StringComparison.InvariantCultureIgnoreCase )
                      let unsecure = string.Equals( url.Scheme, "http", StringComparison.InvariantCultureIgnoreCase )
                      where secure || unsecure
                      group 0 by new { contract.ConfigurationName, 
                          secure } into x
                      select base.AddServiceEndpoint( x.Key.ConfigurationName, CreateBinding( x.Key.secure ), "" );

            if ( _isRest )
            {
                res = res.Do( ep => ep.Behaviors.Add( new WebHttpBehavior { HelpEnabled = true } ) );
            }
            if ( ConfigureEndpoint != null ) res = res.Select( e => { ConfigureEndpoint( e ); return e; } );

            return res.ToList().AsReadOnly();
        }

        private Binding CreateBinding( bool secure )
        {
            var quotas = new System.Xml.XmlDictionaryReaderQuotas { MaxArrayLength = int.MaxValue, MaxStringContentLength = int.MaxValue };

            return _isRest ? (Binding)
                new WebHttpBinding
                {
                    ReaderQuotas = quotas,
                    Security = new WebHttpSecurity { Mode = secure ? WebHttpSecurityMode.Transport : WebHttpSecurityMode.None }
                }
                :
                new BasicHttpBinding
                {
                    ReaderQuotas = quotas,
                    Security = new BasicHttpSecurity { Mode = secure ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.None }
                };
        }
    }

    class AnonymousInstanceProvider : IInstanceProvider, IServiceBehavior
    {
        private readonly Func<InstanceContext, object> _getInstance;
        private readonly Action<InstanceContext, object> _releaseInstance;

        public AnonymousInstanceProvider( Func<InstanceContext, object> getInstance, Action<InstanceContext, object> releaseInstance = null )
        {
            //Contract.Requires( getInstance != null );
            _getInstance = getInstance;
            _releaseInstance = releaseInstance;
        }

        public object GetInstance( InstanceContext instanceContext, System.ServiceModel.Channels.Message message )
        {
            return _getInstance( instanceContext );
        }

        public object GetInstance( InstanceContext instanceContext )
        {
            return _getInstance( instanceContext );
        }

        public void ReleaseInstance( InstanceContext instanceContext, object instance )
        {
            if ( _releaseInstance != null ) _releaseInstance( instanceContext, instance );
        }

        public void AddBindingParameters( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters )
        {
        }

        public void ApplyDispatchBehavior( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase )
        {
            serviceHostBase.ChannelDispatchers
                .OfType<ChannelDispatcher>()
                .SelectMany( cd => cd.Endpoints )
                .Where( e => !e.IsSystemEndpoint )
                .ForEach( ep => ep.DispatchRuntime.InstanceProvider = this );
        }
        public void Validate( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase )
        {
        }
    }

    public interface IServiceConfiguration
    {
        ServiceHost Host { get; }
        Action<ServiceEndpoint> ConfigureEndpoint { get; set; }
    }
}