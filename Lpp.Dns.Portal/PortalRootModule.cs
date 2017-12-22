using System.Collections.Generic;
using System.ComponentModel.Composition;
using Lpp.Audit;
using Lpp.Audit.UI;
using Lpp.Composition.Modules;
using Lpp.Dns.Model;
using Lpp.Mvc;
using Lpp.Mvc.Controls;
using Lpp.Security;
using Lpp.Security.UI;

namespace Lpp.Dns.Portal
{
    [Export( typeof( IRootModule ) ), Export( typeof( IJsPackage ) )]
    class PortalRootModule : IRootModule, IJsPackage
    {
        public IEnumerable<IModule> GetModules()
        {
            yield return Sec.Module<DnsDomain>();
            yield return SecurityUI.Module<DnsDomain>();
            yield return Aud.Module<DnsDomain>();
            yield return AuditUI.Module<DnsDomain>();
            yield return Mvc.Boilerplate.JsBootstrap();
        }

        public string Name 
        {
            get
            {
                return "lpp.dns.portal"; 
            }
        }

        public string Url( System.Web.Mvc.UrlHelper url )
        {
            return url.Resource( GetType().Assembly, "__name__" ).Replace( "/__name__", "" );
        }
    }
}