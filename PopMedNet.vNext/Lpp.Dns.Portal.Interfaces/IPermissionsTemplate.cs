using System.Linq;
using Lpp.Security;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Lpp.Dns.Portal
{
    public interface IPermissionsTemplate<TObject>
    {
        ILookup<SecurityTarget, AclEntry> GetDefaultPermissions( TObject obj );
    }

    public static class PermissionTemplateExtensions
    {
        public static ILookup<SecurityTarget, AclEntry> GetDefaultPermissions<TObject>(
            this IEnumerable<IPermissionsTemplate<TObject>> templates, TObject obj )
        {
            var res = from t in templates
                      from k in t.GetDefaultPermissions( obj )
                      from e in k
                      select new { k.Key, e };
            return res.ToLookup( x => x.Key, x => x.e );
        }
    }
}