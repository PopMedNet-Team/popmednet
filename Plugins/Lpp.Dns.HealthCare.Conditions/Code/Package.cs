using System.ComponentModel.Composition;
using System.Web.Mvc;
using Lpp.Mvc;
using Lpp.Mvc.Controls;

namespace Lpp.Dns.HealthCare.Conditions
{
    [Export( typeof( IJsPackage ) )]
    class Package : IJsPackage
    {
        public string Name { get { return "lpp.mvc.healthcare.conditions"; } }

        public string Url( UrlHelper url )
        {
            return url.Resource( this.GetType().Assembly, "__name__" ).Replace( "/__name__", "" );
        }
    }
}