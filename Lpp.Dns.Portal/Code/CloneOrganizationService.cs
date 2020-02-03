using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Lpp.Dns.Model;
using System.ComponentModel.Composition;
using Lpp.Composition;
//using Lpp.Data;
using Lpp.Security;
using Lpp.Security.UI;
using System.Reflection;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal
{
    [Export(typeof(ICloneService<Organization>)), PartMetadata(ExportScope.Key, TransactionScope.Id)]
    internal class CloneOrganizationService : CloneService<Organization>
    {
        readonly string[] skipProperties = new[] { 
                "Id",
                "IsDeleted",
                "SID",
                "Kind",
                "Name",
                "Children",
                "SecurityGroups",
                "MemberUsers",
                "Groups",
                "DataMarts",
                "Registries",
                "Primary",
                "X509PublicKey",
                "X509PrivateKey",
                "InSearchResults",
                "Requests",
                "Users"
            };

        //[Import]
        //public IRepository<DnsDomain, Organization> Organizations { get; set; }

        public override Organization Clone(Organization existing)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //Organization organization = new Organization();
            //organization.Name = CopyName(existing.Name, (index) => Organizations.All.Count(o => o.Name.StartsWith(existing.Name.Substring(0, index))));

            //CopyProperties(existing, organization, skipProperties);

            //Organizations.Add(organization);

            

            //foreach (OrganizationSecurityGroup securityGroup in existing.SecurityGroups)
            //{
            //    organization.SecurityGroups.Add(new OrganizationSecurityGroup { Kind = securityGroup.Kind, Name = securityGroup.Name });
            //}

            //foreach (Group group in existing.Groups) 
            //{
            //    organization.Groups.Add(group);   
            //}

            //foreach (var registry in existing.Registries)
            //{
            //    organization.Registries.Add(new OrganizationRegistry
            //    {
            //        OrganizationID = registry.OrganizationID,
            //        RegistryID = registry.RegistryID,
            //        Description = registry.Description
            //    });
            //}

            //UnitOfWork.Commit();

            ////Skip DataMarts => specific to organization, talk to Bruce as to how to handle
            ////Skip Users => specific to organization and can't be copied

            

            //SetAcl(existing, organization);

            //SetDefaultRequestAcl(existing, organization);

            //SetDefaultDataMartAcl(existing, organization);

            //SetDefaultUserAcl(existing, organization);


            //SecurityHierarchy.SetObjectInheritanceParent(organization, organization.Parent ?? Lpp.Dns.Portal.VirtualSecObjects.AllOrganizations);

            //UnitOfWork.Commit();

            //return organization;
        }

        void SetAcl(Organization existing, Organization organization)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //Lpp.Security.UI.AclEditModel existingAcl = DnsAclService.AclModelFor(Sec.Target(existing));

            //var nonInheritedEntries = FilterForNonInheritedEntries(existingAcl.Entries).ReplaceObject(existing.SID, organization.SID);

            //DnsAclService.SetAclForTargetAndEvents(Sec.Target(organization), nonInheritedEntries);
        }

        void SetDefaultRequestAcl(Organization existing, Organization organization)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //Lpp.Security.UI.AclEditModel existingAcl = DnsAclService.AclModelFor( AllRequestTargets( existing ) );

            //var nonInheritedEntries = FilterForNonInheritedEntries(existingAcl.Entries).ReplaceObject(existing.SID, organization.SID);

            //DnsAclService.SetAclForTargetAndEvents(AllRequestTargets(organization), nonInheritedEntries);
        }

        void SetDefaultDataMartAcl(Organization existing, Organization organization)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //Lpp.Security.UI.AclEditModel existingAcl = DnsAclService.AclModelFor( AllDmTargets( existing ) );

            //var nonInheritedEntries = FilterForNonInheritedEntries(existingAcl.Entries).ReplaceObject(existing.SID, organization.SID);

            //DnsAclService.SetAclForTargetAndEvents(AllDmTargets(organization), nonInheritedEntries);
        }

        void SetDefaultUserAcl(Organization existing, Organization organization)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //Lpp.Security.UI.AclEditModel existingAcl = DnsAclService.AclModelFor(Sec.Target(existing, Lpp.Dns.Portal.VirtualSecObjects.AllUsers));

            //var nonInheritedEntries = FilterForNonInheritedEntries(existingAcl.Entries).ReplaceObject(existing.SID, organization.SID);

            //DnsAclService.SetAclForTargetAndEvents(Sec.Target(organization, Lpp.Dns.Portal.VirtualSecObjects.AllUsers), nonInheritedEntries);
        }

        static IEnumerable<Lpp.Security.SecurityTarget> AllRequestTargets(Organization organization, Project project = null)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //yield return Sec.Target(project ?? Lpp.Dns.Portal.VirtualSecObjects.AllProjects, organization ?? Lpp.Dns.Portal.VirtualSecObjects.AllOrganizations, Lpp.Dns.Portal.VirtualSecObjects.AllUsers);
            //yield return Sec.Target(project ?? Lpp.Dns.Portal.VirtualSecObjects.AllProjects, organization ?? Lpp.Dns.Portal.VirtualSecObjects.AllOrganizations, Lpp.Dns.Portal.VirtualSecObjects.AllRequests);
        }

        static IEnumerable<Lpp.Security.SecurityTarget> AllDmTargets(Organization organization, Project project = null)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //yield return Sec.Target(organization, Lpp.Dns.Portal.VirtualSecObjects.AllDataMarts);
            //yield return Sec.Target(project ?? Lpp.Dns.Portal.VirtualSecObjects.AllProjects, organization, Lpp.Dns.Portal.VirtualSecObjects.AllDataMarts);
            ////yield return Sec.Target(project ?? Lpp.Dns.Portal.VirtualSecObjects.AllProjects, organization, Lpp.Dns.Portal.VirtualSecObjects.AllDataMarts, Lpp.Dns.Portal.VirtualSecObjects.AllRequests);
        }
    }
}
