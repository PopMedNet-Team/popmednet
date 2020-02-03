using System;
using Lpp.Utilities.Security;
using Lpp.Utilities.WebSites.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.IO;
using Lpp.Dns.Data;
using System.Security.Principal;


namespace Lpp.Dns.Api.Tests
{
    [TestClass]
    public class ControllerTest<C> : IDisposable where C : LppApiController<DataContext>, new()
    {
        protected C controller;
        public ControllerTest()
        {
            //Set the current context
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "https://tempuri.org", ""),
                new HttpResponse(new StringWriter())
            );

            //Set the current user
            var ident = new ApiIdentity(new Guid("96DC0001-94F1-47CC-BFE6-A22201424AD0"), "SystemAdministrator", "System Administrator");
            HttpContext.Current.User = new GenericPrincipal(ident, new string[] { });

            controller = new C();
            controller.Request = new System.Net.Http.HttpRequestMessage();
            controller.RequestContext = new System.Web.Http.Controllers.HttpRequestContext();
        }



        public void Dispose()
        {
            if (controller != null)
                controller.Dispose();
        }
    }
}
