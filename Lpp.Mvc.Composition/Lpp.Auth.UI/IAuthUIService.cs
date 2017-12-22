using System;
using System.Web.Mvc;

namespace Lpp.Auth.UI
{
    interface IAuthUIService
    {
        ActionResult Login( Guid? provider, string returnTo );
        ActionResult DoLogin( string providerGuid, string username, string returnTo );
        ActionResult Logout( string returnTo );
    }
}