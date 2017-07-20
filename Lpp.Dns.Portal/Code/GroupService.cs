using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using Lpp.Composition;
using Lpp.Dns.Data;
using Lpp.Security;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns.Portal
{
    [Export, PartMetadata(ExportScope.Key, TransactionScope.Id)]
    public class GroupService
    {
        [Import]
        internal ISecurityObjectHierarchyService<Lpp.Dns.Model.DnsDomain> SecurityHierarchy { get; set; }
        //[Import]
        //internal IRepository<DnsDomain, Organization> Organizations { get; set; }
        //[Import]
        //internal IRepository<DnsDomain, Group> Groups { get; set; }

        public IQueryable<Organization> GetEffectiveMembersOf(Group grp)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var orgChildren = SecurityHierarchy.GetObjectTransitiveChildren(includeSelf: true);

            //if (grp == null)
            //{
            //    return (from g in Groups.All
            //            from o in g.Organizations
            //            from childSID in orgChildren.Invoke(o.SID)
            //            join child in Organizations.All on childSID equals child.SID
            //            select child)
            //           .Expand();
            //}
            //else
            //{
            //    return (from g in Groups.All
            //            where g.Id == grp.Id
            //            from o in g.Organizations
            //            from childSID in orgChildren.Invoke(o.SID)
            //            join child in Organizations.All on childSID equals child.SID
            //            select child)
            //           .Expand();
            //}
        }
    }
}