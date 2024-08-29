using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using Lpp.Composition;
//using Xunit;
using Lpp.Dns.Model;
using System.Web;
using Lpp.Dns.Portal;
using System.ServiceModel.Web;
using System.ServiceModel;
using Lpp.Mvc;
using Lpp.Tests;

namespace Lpp.Dns.Tests
{
    //public class Wcf
    //{
    //    public static void TestService<TServiceImpl, TServiceIntf>( 
    //        CompositionFixture comp, bool isRestService, 
    //        Func<MockScopeBuilder, MockScopeBuilder> prepareScope,
    //        Action<ICompositionService, Uri, TServiceIntf> act )
    //        where TServiceImpl : class, TServiceIntf, new()
    //        where TServiceIntf : class
    //    {
    //        using ( var scope = prepareScope( comp.MockScope() ).Build() )
    //        {
    //            var address = new Uri( "http://localhost:12345/theservice" );
    //            using ( var host = new SingleContractHttpServiceHost<TServiceImpl, TServiceIntf>(
    //                _ => scope.Compose( new TServiceImpl() ), new[] { address }, isRestService ) )
    //            {
    //                host.Open();
    //                //address = new Uri( "http://localhost:8888/TT" ); // <== this is for testing with Fiddler
    //                using ( var chf = isRestService ? 
    //                    new WebChannelFactory<TServiceIntf>( address ) : 
    //                    new ChannelFactory<TServiceIntf>( new BasicHttpBinding(), new EndpointAddress( address.ToString() ) ) )
    //                {
    //                    act( scope, address, chf.CreateChannel() );
    //                }
    //            }
    //        }
    //    }
    //}
}