using System.ComponentModel.Composition;
using System.Linq;
//using Lpp.Dns.Model;
using Lpp.Security;
//using grp = Lpp.Dns.Model.SecurityGroupKinds;
using p = Lpp.Dns.Portal.SecPrivileges;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal
{
    [Export( typeof( IPermissionsTemplate<User> ) )]
    public class UserPermissionTemplate : IPermissionsTemplate<User>
    {
        public ILookup<Lpp.Security.SecurityTarget, AclEntry> GetDefaultPermissions( User user )
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var pp = new[]
            //{
            //    p.Crud.Read, p.Crud.Edit, RequestPrivileges.ViewStatus, RequestPrivileges.ViewResults, RequestPrivileges.ChangeRoutings
            //};
            //var evts = new[]
            //{
            //    Sec.Target( v.AllProjects, v.AllOrganizations, user, AuditEvents.RequestStatusChange.AsSecurityObject() ),
            //                    Sec.Target( v.AllProjects, v.AllOrganizations, user, v.AllRequests, AuditEvents.UnexaminedResultsReminder.AsSecurityObject() )
            //};

            //var pevtsTarget = new SecurityTarget( user, v.AllPersonalEvents );

            //return pp
            //                .Select( pr => new { t = Sec.Target( v.AllProjects, v.AllOrganizations, user ) as SecurityTarget, pr } )
            //  .Concat( evts.Select( e => new { t = e, pr = p.Event.Observe } ) )
            //  .Concat( new[] { new { t = pevtsTarget, pr = p.Event.Observe } } )

            //  .ToLookup( x => x.t, x => new AclEntry { Kind = AclEntryKind.Allow, Privilege = x.pr, Subject = user } );
        }
    }
}