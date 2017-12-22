using System;
using System.IO;
using System.Web.WebPages;
using Lpp.Utilities.Legacy;

namespace Lpp.Mvc
{
    public class FullyQualifiedTypeVirtualPathFactory : IVirtualPathFactory
    {
        public object CreateInstance( string virtualPath )
        {
            return
                GetType( virtualPath )
                .Select( Activator.CreateInstance )
                .Catch()
                .ValueOrNull();
        }

        public bool Exists( string virtualPath )
        {
            return GetType( virtualPath ).Kind == MaybeKind.Value;
        }

        public MaybeNotNull<Type> GetType( string virtualPath )
        {
            var res = from vp in Maybe.Value( virtualPath )
                      where vp != ""
                      from last in Path.GetFileName( vp )
                      where last.Contains( "," )
                      from type in Type.GetType( last, false )
                      select type;

            return res.Catch();
        }
    }
}