using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.Diagnostics;
using log4net;
using log4net.Appender;
using log4net.Config;
using Lpp.Composition;

namespace Lpp.Tests
{
    public sealed class CompositionFixture : IDisposable
    {
        private readonly DirectoryCatalog _rootCatalog;
        public CompositionContainer RootContainer { get { return ( Scoping as CompositionScopingService ).RootScope as CompositionContainer; } }
        public ICompositionScopingService Scoping { get; private set; }

        public CompositionFixture() : this( null ) { }

        public CompositionFixture( Action<CompositionBatch> explicitExports )
        {
            _rootCatalog = new DirectoryCatalog( ".", new RegistrationBuilder() );
            Scoping = new CompositionScopingService( _rootCatalog, explicitExports ?? (_ => {}), new[] { TransactionScope.Id } );
            RootContainer.ComposeExportedValue<ICompositionScopingService>( Scoping );

            BasicConfigurator.Configure( new Appender() );
            RootContainer.ComposeExportedValue<ILog>( LogManager.GetLogger( "TESTLOG" ) );
        }

        class Appender : IAppender
        {
            public void Close()
            {
            }

            public void DoAppend( log4net.Core.LoggingEvent loggingEvent )
            {
                Debug.WriteLine( loggingEvent.RenderedMessage );
            }

            public string Name
            {
                get { return "A"; }
                set { }
            }
        }

        public void Dispose()
        {
            RootContainer.Dispose();
            _rootCatalog.Dispose();
        }
    }
}