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
using System.Web.Routing;
using Lpp.Dns.Portal;
using Lpp.Audit;
//using Moq;

namespace Lpp.Dns.Tests
{
    //public class DnsMock
    //{
    //    public static HttpContextBase HttpContext()
    //    {
    //        var ctx = new Moq.Mock<HttpContextBase>();
    //        var req = new Moq.Mock<HttpRequestBase>();
    //        var reqCtx = new RequestContext( ctx.Object, new RouteData() );
    //        ctx.Setup( c => c.Request ).Returns( req.Object );
    //        req.Setup( r => r.RequestContext ).Returns( reqCtx );
    //        req.Setup( r => r.Url ).Returns( new Uri( "http://lincolnpeak.com/a/b/c" ) );
    //        return ctx.Object;
    //    }

    //    public static IAuthenticationService Auth( User user = null )
    //    {
    //        var a = new Moq.Mock<IAuthenticationService>();
    //        a.Setup( au => au.CurrentUser ).Returns( user );
    //        return a.Object;
    //    }

    //    class MockEventBuilder : IAuditEventBuilder
    //    {
    //        public void Log() { }
    //        public IAuditEventBuilder With( Lpp.Audit.Data.AuditPropertyValue pv ) { return this; }
    //    }

    //    public static IAuditService<DnsDomain> Audit()
    //    {
    //        var a = new Moq.Mock<IAuditService<DnsDomain>>();
    //        a.Setup( x => x.CreateEvent( It.IsAny<AuditEventKind>(), It.IsAny<Security.SecurityTarget>() ) ).Returns( new MockEventBuilder() );
    //        return a.Object;
    //    }
    //}
}