using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Lpp.Dns;
using System.ServiceModel.Web;
using System.ServiceModel;

namespace Lpp.Dns.Models.Sample.Controllers
{
    public class SampleModelController : Controller
    {
        public ActionResult CreateRequest( Models.PluginCallModel req )
        {
            return View( req );
        }

        [HttpPost]
        public ActionResult CreateRequest( Models.PluginCallModel req, string requestText )
        {
            string returnUrl = null;
            using ( var f = new WebChannelFactory<IRequestService>( new Uri( req.Service ) ) )
            {
                var ws = f.CreateChannel();
                var md = ws.GetSessionMetadata( req.SessionToken );
                returnUrl = md.ReturnUrl;
                ws.PostDocument( req.SessionToken, "Text", "text/plain", true, Encoding.UTF8.GetBytes( requestText ) );
                ws.RequestCreated( req.SessionToken, new RequestHeader { Name = "Some wierd name", Priority = RequestPriority.High }, null );
            }

            return Redirect( returnUrl );
        }

        public ActionResult RetrieveResponse( Models.PluginCallModel req )
        {
            return View( req );
        }
    }
}
