using LinqKit;
using Lpp.Dns.Data;
using Lpp.Utilities.Security;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using Lpp.Dns.DTO.Enums;
using Lpp.Utilities.Objects;
using Lpp.Objects;
using Lpp.Dns.DTO.Security;

namespace Lpp.Dns.Data
{
    public abstract class DnsEntitySecurityConfiguration<TEntity> : EntitySecurityConfiguration<DataContext, TEntity, PermissionDefinition>
        where TEntity : class 
    {
        public virtual Expression<Func<AclProject, bool>> ProjectFilter(params Guid[] objID)
        {
            return null;
        }
        public virtual Expression<Func<AclProjectDataMart, bool>> ProjectDataMartFilter(params Guid[] objIDs)
        {
            return null;
        }

        public virtual Expression<Func<AclProjectOrganization, bool>> ProjectOrganzationFilter(params Guid[] objIDs)
        {
            return null;
        }

        public virtual Expression<Func<AclDataMart, bool>> DataMartFilter(params Guid[] objIDs)
        {
            return null;
        }

        public virtual Expression<Func<AclGroup, bool>> GroupFilter(params Guid[] objIDs)
        {
            return null;
        }

        public virtual Expression<Func<AclOrganizationDataMart, bool>> OrganizationDataMartFilter(Guid[] objIDs)
        {
            return null;
        }

        public virtual Expression<Func<AclOrganization, bool>> OrganizationFilter(params Guid[] objIDs)
        {
            return null;
        }

        public virtual Expression<Func<AclOrganizationUser, bool>> OrganizationUserFilter(params Guid[] objIDs)
        {
            return null;
        }

        public virtual Expression<Func<AclRegistry, bool>> RegistryFilter(params Guid[] objIDs)
        {
            return null;
        }

        public virtual Expression<Func<AclRequest, bool>> RequestFilter(params Guid[] objIDs)
        {
            return null;
        }
        public virtual Expression<Func<AclRequestType, bool>> RequestTypeFilter(params Guid[] objIDs)
        {
            return null;
        }
        public virtual Expression<Func<AclUser, bool>> UserFilter(params Guid[] objIDs)
        {
            return null;
        }
        
        public virtual Expression<Func<AclRequestSharedFolder, bool>> RequestSharedFolderFilter(params Guid[] objIDs)
        {
            return null;
        }

        public virtual Expression<Func<AclTemplate, bool>> TemplateFilter(params Guid[] objIDs)
        {
            return null;
        }

        public override Task<IEnumerable<PermissionDefinition>> HasGrantedPermissions(DataContext db, ApiIdentity identity, Guid[] objID, params PermissionDefinition[] requestedPermissions)
        {
            return HasGrantedPermissions(db, identity, objID, null, requestedPermissions);
        }

        public async Task<IEnumerable<PermissionDefinition>> HasGrantedPermissions(DataContext db, ApiIdentity identity, Guid[] objID, ExtendedQuery filters, params PermissionDefinition[] requestedPermissions)
        {
            var predicateAll = PredicateBuilder.True<Permission>();
            var predicateAny = PredicateBuilder.False<Permission>();

            if (filters == null)
                filters = new ExtendedQuery();

            var locations = requestedPermissions.SelectMany(p => p.Locations).Distinct();
            foreach (var location in locations)
            {
                var permissions = requestedPermissions.Where(p => p.Locations.Any(l => l == location)).ToArray();

                switch (location)
                {
                    case PermissionAclTypes.Global:
                        var globalAcls = db.GlobalAcls.FilterAcl(identity, permissions);

                        if (filters.Global != null)
                            globalAcls = globalAcls.Where(filters.Global);
                        
                        predicateAny = predicateAny.Or(p => globalAcls.Where(a => a.PermissionID == p.ID).Any());
                        predicateAll = predicateAll.And(p => globalAcls.Where(a => a.PermissionID == p.ID).All(a => a.Allowed));
                        break;
                    case PermissionAclTypes.DataMarts:
                        var dmFilter = this.DataMartFilter(objID);
                        if (dmFilter == null)
                            break;
                        var dataMartAcls = db.DataMartAcls.FilterAcl(identity, permissions).Where(dmFilter);

                        if (filters.DataMarts != null)
                            dataMartAcls = dataMartAcls.Where(filters.DataMarts);

                        predicateAny = predicateAny.Or(p => dataMartAcls.Where(a => a.PermissionID == p.ID).Any());
                        predicateAll = predicateAll.And(p => dataMartAcls.Where(a => a.PermissionID == p.ID).All(a => a.Allowed));
                        break;
                    case PermissionAclTypes.Groups:
                        var gFilter = this.GroupFilter(objID);
                        if (gFilter == null)
                            break;
                        var groupAcls = db.GroupAcls.FilterAcl(identity, permissions);

                        if (filters.Groups != null)
                        {
                            groupAcls = groupAcls.Where(filters.Groups);
                        }

                        predicateAny = predicateAny.Or(p => groupAcls.Where(a => a.PermissionID == p.ID).Where(gFilter).Any());
                        predicateAll = predicateAll.And(p => groupAcls.Where(a => a.PermissionID == p.ID).Where(gFilter).All(a => a.Allowed));
                        break;
                    case PermissionAclTypes.Organizations:
                        var oFilter = this.OrganizationFilter(objID);
                        if (oFilter == null)
                            break;
                        var organizationAcls = db.OrganizationAcls.FilterAcl(identity, permissions);

                        if (filters.Organizations != null)
                        {
                            organizationAcls = organizationAcls.Where(filters.Organizations);
                        }

                        predicateAny = predicateAny.Or(p => organizationAcls.Where(a => a.PermissionID == p.ID).Where(oFilter).Any());
                        predicateAll = predicateAll.And(p => organizationAcls.Where(a => a.PermissionID == p.ID).Where(oFilter).All(a => a.Allowed));
                        break;
                    case PermissionAclTypes.OrganizationUsers:
                        var ouFilter = this.OrganizationUserFilter(objID);
                        if (ouFilter == null)
                            break;
                        var organizationUserAcls = db.OrganizationUserAcls.FilterAcl(identity, permissions);

                        if (filters.OrganizationUsers != null)
                        {
                            organizationUserAcls = organizationUserAcls.Where(filters.OrganizationUsers);
                        }

                        predicateAny = predicateAny.Or(p => organizationUserAcls.Where(a => a.PermissionID == p.ID).Where(ouFilter).Any());
                        predicateAll = predicateAll.And(p => organizationUserAcls.Where(a => a.PermissionID == p.ID).Where(ouFilter).All(a => a.Allowed));
                        break;
                    case PermissionAclTypes.ProjectDataMarts:
                        var pdmFilter = this.ProjectDataMartFilter(objID);
                        if (pdmFilter == null)
                            break;

                        var projectDataMartAcls = db.ProjectDataMartAcls.FilterAcl(identity, permissions).Where(pdmFilter);

                        if (filters.ProjectDataMarts != null) 
                            projectDataMartAcls = projectDataMartAcls.Where(filters.ProjectDataMarts);

                        predicateAny = predicateAny.Or(p => projectDataMartAcls.Where(a => a.PermissionID == p.ID).Any());
                        predicateAll = predicateAll.And(p => projectDataMartAcls.Where(a => a.PermissionID == p.ID).All(a => a.Allowed));
                        break;
                    case PermissionAclTypes.ProjectOrganizations:
                        var poFilter = this.ProjectOrganzationFilter(objID);
                        if (poFilter == null)
                            break;

                        var projectOrganizationAcls = db.ProjectOrganizationAcls.FilterAcl(identity, permissions);

                        if (filters.ProjectOrganizations != null)
                        {
                            projectOrganizationAcls = projectOrganizationAcls.Where(filters.ProjectOrganizations);
                        }

                        predicateAny = predicateAny.Or(p => projectOrganizationAcls.Where(a => a.PermissionID == p.ID).Where(poFilter).Any());
                        predicateAll = predicateAll.And(p => projectOrganizationAcls.Where(a => a.PermissionID == p.ID).Where(poFilter).All(a => a.Allowed));
                        break;
                    case PermissionAclTypes.Projects:
                        var pFilter = this.ProjectFilter(objID);
                        if (pFilter == null)
                            break;

                        var projectAcls = db.ProjectAcls.FilterAcl(identity, permissions);

                        if (filters.Projects != null)
                        {
                            projectAcls = projectAcls.Where(filters.Projects);
                        }

                        predicateAny = predicateAny.Or(p => projectAcls.Where(a => a.PermissionID == p.ID).Where(pFilter).Any());
                        predicateAll = predicateAll.And(p => projectAcls.Where(a => a.PermissionID == p.ID).Where(pFilter).All(a => a.Allowed));
                        break;
                    case PermissionAclTypes.Registries:
                        var rFilter = this.RegistryFilter(objID);
                        if (rFilter == null)
                            break;
                        var registryAcls = db.RegistryAcls.FilterAcl(identity, permissions);

                        if (filters.Registries != null)
                        {
                            registryAcls = registryAcls.Where(filters.Registries);
                        }

                        predicateAny = predicateAny.Or(p => registryAcls.Where(a => a.PermissionID == p.ID).Where(rFilter).Any());
                        predicateAll = predicateAll.And(p => registryAcls.Where(a => a.PermissionID == p.ID).Where(rFilter).All(a => a.Allowed));
                        break;
                    case PermissionAclTypes.Requests:
                        var reqFilter = this.RequestFilter(objID);
                        if (reqFilter == null)
                            break;

                        var requestAcls = db.RequestAcls.FilterAcl(identity, permissions);

                        if (filters.Requests != null)
                        {
                            requestAcls = requestAcls.Where(filters.Requests);
                        }

                        predicateAny = predicateAny.Or(p => requestAcls.Where(a => a.PermissionID == p.ID).Where(reqFilter).Any());
                        predicateAll = predicateAll.And(p => requestAcls.Where(a => a.PermissionID == p.ID).Where(reqFilter).All(a => a.Allowed));
                        break;
                    case PermissionAclTypes.RequestSharedFolders:
                        var rsfFilter = this.RequestSharedFolderFilter(objID);
                        if (rsfFilter == null)
                            break;
                        var rsfAcls = db.RequestSharedFolderAcls.FilterAcl(identity, permissions);

                        if (filters.SharedFolders != null)
                        {
                            rsfAcls = rsfAcls.Where(filters.SharedFolders);
                        }

                        predicateAny = predicateAny.Or(p => rsfAcls.Where(a => a.PermissionID == p.ID).Where(rsfFilter).Any());
                        predicateAll = predicateAll.And(p => rsfAcls.Where(a => a.PermissionID == p.ID).Where(rsfFilter).All(a => a.Allowed));
                        break;
                    case PermissionAclTypes.Users:
                        var uFilter = this.UserFilter(objID);
                        if (uFilter == null)
                            break;

                        var userAcls = db.UserAcls.FilterAcl(identity, permissions);

                        if (filters.Users != null)
                        {
                            userAcls = userAcls.Where(filters.Users);
                        }

                        predicateAny = predicateAny.Or(p => userAcls.Where(a => a.PermissionID == p.ID).Where(uFilter).Any());
                        predicateAll = predicateAll.And(p => userAcls.Where(a => a.PermissionID == p.ID).Where(uFilter).All(a => a.Allowed));
                        break;
                    case PermissionAclTypes.RequestTypes:
                        var rtFilter = this.RequestTypeFilter(objID);
                        if (rtFilter == null)
                            break;
                        var requestTypeAcls = db.RequestTypeAcls.FilterAcl(identity, permissions);
                        if (filters.RequestTypes != null)
                            requestTypeAcls.Where(filters.RequestTypes);

                        predicateAny = predicateAny.Or(p => requestTypeAcls.Where(a => a.PermissionID == p.ID).Where(rtFilter).Any());
                        predicateAll = predicateAll.And(p => requestTypeAcls.Where(a => a.PermissionID == p.ID).Where(rtFilter).All(a => a.Allowed));
                        break;
                    case PermissionAclTypes.Templates:
                        var tFilter = this.TemplateFilter(objID);
                        if (tFilter == null)
                            break;

                        var templateAcls = db.TemplateAcls.FilterAcl(identity, permissions);

                        if (filters.Templates != null)
                        {
                            templateAcls = templateAcls.Where(filters.Templates);
                        }

                        predicateAny = predicateAny.Or(p => templateAcls.Where(a => a.PermissionID == p.ID).Where(tFilter).Any());
                        predicateAll = predicateAll.And(p => templateAcls.Where(a => a.PermissionID == p.ID).Where(tFilter).All(a => a.Allowed));
                        break;
                    default:
                        throw new NotSupportedException("The security location is not supported.");
                }
            }

            //Combine the two predicates and and them so that we know that we have at least one allowed, and all are allowed
            var predicate = PredicateBuilder.True<Permission>();
            predicate = predicate.And(predicateAll.Expand());
            predicate = predicate.And(predicateAny.Expand());

            var result = (from p in await db.Permissions.AsExpandable().Where(predicate).ToArrayAsync() join rp in requestedPermissions on p.ID equals rp.ID select rp);
            return result;
        }
    }
}