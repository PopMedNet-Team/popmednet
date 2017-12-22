using System.Linq.Expressions;
using System.Linq;
using System;
using Lpp.Mvc.Application;
using System.Web.Mvc;
using Lpp.Mvc;
using System.ComponentModel.Composition;
using Lpp.Data;
using DotNetOpenAuth.OpenId.RelyingParty;
using System.Web.Security;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.Messaging;
using Lpp.Auth;
using System.Web;
using System.Diagnostics.Contracts;
using System.Web.Routing;

namespace Lpp.Auth.UI
{
    [Export, ExportController]
    class AuthController : BaseController
    {
        [Import] public IAuthUIService UI { get; set; }

        public ActionResult Login( Guid? provider, string returnTo )
        {
            return UI.Login( provider, returnTo );
        }

        public ActionResult DoLogin( string providerGuid, string username, string returnTo )
        {
            return UI.DoLogin( providerGuid, username, returnTo );
        }

        public ActionResult Logout( string returnTo )
        {
            return UI.Logout( returnTo );
        }
    }

    public static class AuthControllerExtensions
    {
        public static string Login( this UrlHelper url, bool setReturnUrl = true )
        {
            Contract.Requires( url != null );
            return url.Action( ( AuthController c ) => c.Login( null, setReturnUrl ? url.RequestContext.HttpContext.Request.Url.ToString() : null ) );
        }

        public static string Logout( this UrlHelper url, bool setReturnUrl = true )
        {
            Contract.Requires( url != null );
            return url.Action( ( AuthController c ) => c.Logout( setReturnUrl ? url.RequestContext.HttpContext.Request.Url.ToString() : null ) );
        }
    }
}