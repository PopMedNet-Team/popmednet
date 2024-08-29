using System.ComponentModel.Composition;
using System.Linq;
using Lpp.Dns.Data;
using Lpp.Security;
//using grp = Lpp.Dns.Model.SecurityGroupKinds;
using p = Lpp.Dns.Portal.SecPrivileges;

namespace Lpp.Dns.Portal
{
    [Export(typeof(IPermissionsTemplate<Registry>))]
    public class RegistryPermissionTemplate : IPermissionsTemplate<Registry>
    {
        public ILookup<Lpp.Security.SecurityTarget, AclEntry> GetDefaultPermissions(Registry registry)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var pp = new[]{
            //    new { t = st(registry), e = new []{
            //        new { g= grp.Everyone, p = p.Crud.Read },
            //        new { g = grp.Administrators, p = p.ManageSecurity },
            //        new { g = grp.Administrators, p = p.Crud.Delete },
            //        new { g = grp.Administrators, p = p.Crud.Edit }
            //    }}
            //};

            //var evts = new[] { 
            //    new { g = grp.Administrators, e = new []{
            //        new { e = AuditEvents.RegistryCrud, t = st( registry)}
            //    }}
            //};

            //var evts1 = from g in evts
            //            from e in g.e
            //            group new { g.g, e } by new { e.t, e.e } into es
            //            select new
            //            {
            //                t = new SecurityTarget(es.Key.t.Elements.Concat(new[] { es.Key.e.AsSecurityObject() })),
            //                e = es.Select(x => new { g = x.g, p = p.Event.Observe }).ToArray()
            //            };

            //var res = from t in pp.Concat(evts1)
            //          from e in t.e
            //          select new { t.t, e = new AclEntry { Kind = AclEntryKind.Allow, Privilege = e.p, Subject = registry } };

            //return res.ToLookup(x => x.t, x => x.e);

        }

        static Lpp.Security.SecurityTarget st(params ISecurityObject[] objs) 
        {
            return new Lpp.Security.SecurityTarget(objs); 
        }
    }
}