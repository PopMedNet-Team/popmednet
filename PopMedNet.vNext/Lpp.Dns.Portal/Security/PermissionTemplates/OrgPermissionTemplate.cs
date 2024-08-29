using System.ComponentModel.Composition;
using System.Linq;
using Lpp.Dns.Data;
using Lpp.Security;
//using grp = Lpp.Dns.Model.SecurityGroupKinds;
using p = Lpp.Dns.Portal.SecPrivileges;

namespace Lpp.Dns.Portal
{
    [Export(typeof(IPermissionsTemplate<Organization>))]
    public class OrgPermissionTemplate : IPermissionsTemplate<Organization>
    {
        public ILookup<Lpp.Security.SecurityTarget, AclEntry> GetDefaultPermissions(Organization org)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var pp = new[]
            //{
            //    new { t = st(org), e = new[]
            //    {
            //        new { g = grp.Everyone, p = p.Crud.Read },
            //        new { g = grp.Administrators, p = p.ManageSecurity },
            //        new { g = grp.Administrators, p = p.Crud.Delete },
            //        new { g = grp.Administrators, p = p.Crud.Edit },
            //        new { g = grp.Administrators, p = p.Organization.CreateDataMarts },
            //        new { g = grp.Administrators, p = p.Organization.CreateUsers }
            //    } },
            //    new { t = st( org, v.AllDataMarts ), e = new[]
            //    {
            //        new { g = grp.Everyone, p = p.Crud.Read },
            //        new { g = grp.Administrators, p = p.ManageSecurity },
            //        new { g = grp.Administrators, p = p.Crud.Delete },
            //        new { g = grp.Administrators, p = p.Crud.Edit },
            //        new { g = grp.Administrators, p = p.DataMart.RunAuditReport },
            //        new { g = grp.DataMartAdministrators, p = p.DataMart.InstallModels },
            //        new { g = grp.DataMartAdministrators, p = p.DataMart.UninstallModels },
            //    } },
            //    new { t = st( org, v.AllUsers ), e = new[]
            //    {
            //        new { g = grp.Everyone, p = p.Crud.Read },
            //        new { g = grp.Administrators, p = p.ManageSecurity },
            //        new { g = grp.Administrators, p = p.Crud.Delete },
            //        new { g = grp.Administrators, p = p.Crud.Edit },
            //        new { g = grp.Administrators, p = p.User.ChangeLogin },
            //        new { g = grp.Administrators, p = p.User.ChangePassword },
            //        new { g = grp.Administrators, p = p.User.ManageNotifications },
            //    } }
            //};
            //var evts = new[]
            //{
            //    new { g = grp.Administrators, e = new[]
            //    {
            //        new { e = AuditEvents.DataMartCrud, t = st( org, v.AllDataMarts ) },
            //        new { e = AuditEvents.UserCrud, t = st( org, v.AllUsers ) },
            //        new { e = AuditEvents.OrganizationCrud, t = st(org) },
            //    } }
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
            //          join g in org.SecurityGroups on e.g equals g.Kind
            //          select new { t.t, e = new AclEntry { Kind = AclEntryKind.Allow, Privilege = e.p, Subject = g } };
            //return res.ToLookup(x => x.t, x => x.e);
        }

        static Lpp.Security.SecurityTarget st(params ISecurityObject[] objs) 
        { 
            return new Lpp.Security.SecurityTarget(objs); 
        }
    }
}