using System.Web.Mvc;

namespace Lpp.Mvc.Controls
{
    public interface IJsPackage
    {
        string Name { get; }
        string Url( UrlHelper url );
    }
}