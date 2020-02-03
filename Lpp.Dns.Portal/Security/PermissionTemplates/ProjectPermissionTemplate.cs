using System.ComponentModel.Composition;
using System.Linq;
using Lpp.Dns.Data;
using Lpp.Security;
//using grp = Lpp.Dns.Model.SecurityGroupKinds;
using p = Lpp.Dns.Portal.SecPrivileges;

namespace Lpp.Dns.Portal
{
    [Export( typeof( IPermissionsTemplate<Project> ) )]
    public class ProjectPermissionTemplate : IPermissionsTemplate<Project>
    {
        public ILookup<Lpp.Security.SecurityTarget, AclEntry> GetDefaultPermissions( Project proj )
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var pp = new[]
            //{
            //    new { t = st( proj, v.AllOrganizations, v.AllUsers ), e = new[]
            //    {
            //        new { g = grp.Observers, p = p.Crud.Read },
            //        new { g = grp.Observers, p = RequestPrivileges.ViewStatus },
            //        new { g = grp.QueryAdministrators, p = RequestPrivileges.ApproveSubmission },
            //        new { g = grp.QueryAdministrators, p = RequestPrivileges.ChangeRoutings },
            //        new { g = grp.EnhancedInvestigators, p = RequestPrivileges.ViewIndividualResults },
            //        new { g = grp.EnhancedInvestigators, p = RequestPrivileges.ViewRequest }
            //    } }
            //};
            //var evts = new[]
            //{
            //    new { g = grp.Administrators, e = new[]
            //    {
            //        new { e = AuditEvents.NewRequest, t = st(proj) },
            //        new { e = AuditEvents.ResultsViewed, t = st( proj, v.AllOrganizations, v.AllUsers ) }
            //    } },
            //    new { g = grp.Observers, e = new[]
            //    {
            //        new { e = AuditEvents.NewRequest, t = st(proj) },
            //        new { e = AuditEvents.RequestStatusChange, t = st(proj, v.AllOrganizations, v.AllUsers) }
            //    } },
            //    new { g = grp.DataMartAdministrators, e = new[]
            //    {
            //        new { e = AuditEvents.UnapprovedResultsReminder, t = st(proj, v.AllOrganizations, v.AllDataMarts) },
            //        new { e = AuditEvents.UnresponsedRequestReminder, t = st(proj, v.AllOrganizations, v.AllDataMarts) },
            //        new { e = AuditEvents.ResultsViewed, t = st(proj, v.AllOrganizations, v.AllUsers) },
            //    } }
            //};
            //var evts1 = from g in evts
            //            from e in g.e
            //            group new { g.g, e } by new { e.t, e.e } into es
            //            select new
            //            {
            //                t = new SecurityTarget( es.Key.t.Elements.Concat( new[] { es.Key.e.AsSecurityObject() } ) ),
            //                e = es.Select( x => new { g = x.g, p = p.Event.Observe } ).ToArray()
            //            };

            //var res = from t in pp.Concat( evts1 )
            //          from e in t.e
            //          join g in proj.SecurityGroups on e.g equals g.Kind
            //          select new { t.t, e = new AclEntry { Kind = AclEntryKind.Allow, Privilege = e.p, Subject = g } };
            //return res.ToLookup( x => x.t, x => x.e );
        }

        static Lpp.Security.SecurityTarget st( params ISecurityObject[] objs ) 
        { 
            return new Lpp.Security.SecurityTarget( objs ); 
        }
    }
}