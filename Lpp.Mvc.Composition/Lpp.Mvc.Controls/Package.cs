using System.ComponentModel.Composition;
using System.Web.Mvc;

namespace Lpp.Mvc.Controls
{
    [Export( typeof( IJsPackage ) )]
    class Package : IJsPackage
    {
        public string Name { get { return "lpp.mvc.controls"; } }

        public string Url( UrlHelper url )
        {
            return url.Resource( this.GetType().Assembly, "__name__" ).Replace( "/__name__", "" );
        }
    }
}