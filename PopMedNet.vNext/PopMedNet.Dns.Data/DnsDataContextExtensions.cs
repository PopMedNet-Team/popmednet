using PopMedNet.Utilities.Security;
using PopMedNet.Dns.DTO.Security;
using LinqKit;

namespace PopMedNet.Dns.Data
{
    /// <summary>
    /// These are overridden extention methods to save having to specify generic parameters on every call. The root is DataContextExtensions, however you're not allowed to inherit or override so these have to be disconnected like this. 
    /// Add using Lpp.Network.Data; to a file to use these. 
    /// </summary>
    public static class DnsDataContextExtensions
    {

        public static IQueryable<T> FilterAcl<T>(this IQueryable<T> query, ApiIdentity identity, params PermissionDefinition[] permissions)
            where T : Acl
        {
            return FilterAcl(query, identity.ID, permissions);
        }

        public static IQueryable<T> FilterAcl<T>(this IQueryable<T> query, Guid? identityID, params PermissionDefinition[] permissions)
            where T : Acl
        {
            query = query.Where(a => a.SecurityGroup.Users.Any(u => u.UserID == identityID)).AsExpandableEFCore();

            if (permissions != null && permissions.Length > 0)
            {
                var or = PredicateBuilder.New<T>(false);
                foreach (var permission in permissions)
                {
                    or = or.Or(p => p.PermissionID == permission.ID);
                }
                query = query.AsExpandableEFCore().Where(or);
            }

            return query;
        }

        public static IQueryable<T> FilterAcl<T>(this IQueryable<T> query, Guid? identityID, params Guid[] permissions)
            where T : Acl
        {
            query = query.Where(a => a.SecurityGroup!.Users.Any(u => u.UserID == identityID));

            if (permissions != null && permissions.Length > 0)
            {
                query = query.Where(a => permissions.Contains(a.PermissionID));
                //var or = PredicateBuilder.New<T>(false);
                //foreach (var permission in permissions)
                //{
                //    or = or.Or(p => p.PermissionID == permission);
                //}
                //query = query.AsExpandableEFCore().Where(or);
            }

            return query.AsExpandableEFCore();
        }

        public static IQueryable<T> FilterRequestTypeAcl<T>(this IQueryable<T> query, Guid? identityID)
            where T : RequestTypeAcl
        {
            query = query.Where(a => a.SecurityGroup.Users.Any(u => u.UserID == identityID));
            return query.AsExpandableEFCore();
        }

        public static IQueryable<T> FilterEvent<T>(this IQueryable<T> query, Guid? identityID, params DTO.Events.EventIdentifiers.EventDefinition[] events)
            where T : BaseEventPermission
        {
            query = query.Where(e => e.SecurityGroup.Users.Any(u => u.UserID == identityID));

            var or = PredicateBuilder.New<T>(false);
            foreach (var evt in events)
            {
                or = or.Or(e => e.EventID == evt.ID);
            }

            query = query.AsExpandableEFCore().Where(or);

            return query;
        }

        public static IQueryable<TEntity> Secure<TEntity>(this DataContext dataContext, ApiIdentity identity)
            where TEntity : class
        {
            return DataContextExtensions.Secure<DataContext, TEntity, PermissionDefinition>(dataContext, identity);
        }

        public static IQueryable<TEntity> Secure<TEntity>(this IQueryable<TEntity> query, DataContext dataContext, ApiIdentity identity)
    where TEntity : class
        {
            return DataContextExtensions.Secure<DataContext, TEntity, PermissionDefinition>(query, dataContext, identity);
        }

        public static IQueryable<TEntity> Secure<TEntity>(this DataContext dataContext, ApiIdentity identity, params PermissionDefinition[] permissions)
    where TEntity : class
        {
            return DataContextExtensions.Secure<DataContext, TEntity, PermissionDefinition>(dataContext, identity, permissions);
        }


        public static async Task<bool> CanInsert<TEntity>(this DataContext dataContext, ApiIdentity identity, params TEntity[] objs)
            where TEntity : class
        {
            return await DataContextExtensions.CanInsert<DataContext, TEntity, PermissionDefinition>(dataContext, identity);
        }

        public static async Task<bool> CanUpdate<TEntity>(this DataContext dataContext, ApiIdentity identity, params Guid[] keys)
            where TEntity : class
        {
            return await DataContextExtensions.CanUpdate<DataContext, TEntity, PermissionDefinition>(dataContext, identity, keys);
        }

        public static async Task<bool> CanDelete<TEntity>(this DataContext dataContext, ApiIdentity identity, params Guid[] keys)
            where TEntity : class
        {
            return await DataContextExtensions.CanDelete<DataContext, TEntity, PermissionDefinition>(dataContext, identity, keys);
        }

        public static async Task<bool> HasPermissions<TEntity>(this DataContext dataContext, ApiIdentity identity, Guid[] objIDs, params PermissionDefinition[] permissions)
            where TEntity : class
        {
            return await DataContextExtensions.HasPermissions<DataContext, TEntity, PermissionDefinition>(dataContext, identity, objIDs, permissions);
        }

        public static async Task<bool> HasPermissions<TEntity>(this DataContext dataContext, ApiIdentity identity, Guid objID, params PermissionDefinition[] permissions)
            where TEntity : class, new()
        {
            return await DataContextExtensions.HasPermissions<DataContext, TEntity, PermissionDefinition>(dataContext, identity, objID, permissions);
        }

        public static async Task<bool> HasPermissions<TEntity>(this DataContext dataContext, ApiIdentity identity, params PermissionDefinition[] permissions)
    where TEntity : class, new()
        {
            return await DataContextExtensions.HasPermissions<DataContext, TEntity, PermissionDefinition>(dataContext, identity, permissions);
        }        

        public static bool CanViewIndividualResults(this DataContext db, ApiIdentity apiIdentity, Guid currentUserID, Guid requestID)
        {
            var organizationsAcls = db.OrganizationAcls.FilterAcl(apiIdentity, PermissionIdentifiers.Request.ViewIndividualResults, PermissionIdentifiers.DataMartInProject.SeeRequests);
            var projectsAcls = db.ProjectAcls.FilterAcl(apiIdentity, PermissionIdentifiers.Request.ViewIndividualResults);
            var usersAcls = db.UserAcls.FilterAcl(apiIdentity, PermissionIdentifiers.Request.ViewIndividualResults);
            var projectOrganizationsAcls = db.ProjectOrganizationAcls.FilterAcl(apiIdentity, PermissionIdentifiers.Request.ViewIndividualResults);
            var projectDataMartAcls = db.ProjectDataMartAcls.FilterAcl(apiIdentity, PermissionIdentifiers.DataMartInProject.SeeRequests);
            var dataMartAcls = db.DataMartAcls.FilterAcl(apiIdentity, PermissionIdentifiers.DataMartInProject.SeeRequests);

            var canViewIndividualResults = (from r in db.Requests
                                            let organization = organizationsAcls.Where(o => o.OrganizationID == r.OrganizationID)
                                            let project = projectsAcls.Where(p => p.ProjectID == r.ProjectID)
                                            let user = usersAcls.Where(u => u.UserID == currentUserID)
                                            let projectOrganization = projectOrganizationsAcls.Where(p => p.ProjectID == r.ProjectID && p.OrganizationID == r.OrganizationID)
                                            let projectDataMart = projectDataMartAcls.Where(a => a.ProjectID == r.ProjectID && r.DataMarts.Any(dm => dm.DataMartID == a.DataMartID))
                                            let dataMart = dataMartAcls.Where(a => r.DataMarts.Any(dm => dm.DataMartID == a.DataMartID))
                                            where r.ID == requestID                                            
                                            && (organization.Any() || project.Any() || user.Any() || projectOrganization.Any() || projectDataMart.Any() || dataMart.Any())
                                            && organization.All(a => a.Allowed) && project.All(a => a.Allowed) && user.All(a => a.Allowed) && projectOrganization.All(a => a.Allowed) && projectDataMart.All(a => a.Allowed) && dataMart.All(a => a.Allowed)
                                            select r).Any();

            return canViewIndividualResults;
        }

        /// <summary>
        /// Returns the permissions the specified Identity is authorized for based on the specified permissions identifiers. If no permissions are specified it will check all ProjectRequestTypeWorkflowActivity permission definitions.
        /// </summary>
        /// <param name="db">The DataContext.</param>
        /// <param name="apiIdentity">The Identity to check permission for.</param>
        /// <param name="projectID">The project ID.</param>
        /// <param name="workflowActivityID">The workflow activity ID.</param>
        /// <param name="requestTypeID">The request type ID.</param>
        /// <param name="permissionID">The permissions to check, or null for all.</param>
        /// <returns></returns>
        public static async Task<IEnumerable<PermissionDefinition>> GetGrantedPermissionsForWorkflowActivityAsync(this DataContext db, ApiIdentity apiIdentity, Guid projectID, Guid workflowActivityID, Guid requestTypeID, params Guid[] permissionID)
        {
            ExtendedQuery query = new ExtendedQuery
            {
                ProjectRequestTypeWorkflowActivity = p => p.ProjectID == projectID && p.RequestTypeID == requestTypeID && p.WorkflowActivityID == workflowActivityID
            };

            PermissionDefinition[] permissions = null;
            if (permissionID != null && permissionID.Any())
            {
                permissions = PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.Permissions().Where(p => permissionID.Contains(p.ID)).ToArray();

                if (permissions != null && permissions.Length == 0)
                {
                    //invalid permissions were specified, return no permission
                    return Enumerable.Empty<PermissionDefinition>();
                }
            }

            if (permissions == null || permissions.Length == 0)
                permissions = PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.Permissions().ToArray();

            var grantedPermissions = await db.HasGrantedPermissions(apiIdentity, query, permissions);

            return grantedPermissions;
        }

        /// <summary>
        /// Returns the permissions the specified Identity is authorized for based on the specified permissions identifiers. If no permissions are specified it will check all ProjectRequestTypeWorkflowActivity permission definitions.
        /// </summary>
        /// <param name="db">The DataContext.</param>
        /// <param name="apiIdentity">The Identity to check permission for.</param>
        /// <param name="projectID">The project ID.</param>
        /// <param name="workflowActivityID">The workflow activity ID.</param>
        /// <param name="requestTypeID">The request type ID.</param>
        /// <param name="permissionID">The permissions to check, or null for all.</param>
        /// <returns></returns>
        public static IEnumerable<PermissionDefinition> GetGrantedPermissionsForWorkflowActivity(this DataContext db, ApiIdentity apiIdentity, Guid projectID, Guid workflowActivityID, Guid requestTypeID, params Guid[] permissionID)
        {
            IEnumerable<PermissionDefinition> permissions = Enumerable.Empty<PermissionDefinition>();

            Task.WaitAll(Task.Run(async () => { permissions = await GetGrantedPermissionsForWorkflowActivityAsync(db, apiIdentity, projectID, workflowActivityID, requestTypeID, permissionID); }));

            return permissions;
        }

        /// <summary>
        /// Gets the permissions specified for the current workflow activity for the specified request and Identity.
        /// </summary>
        /// <param name="db">The DataContext.</param>
        /// <param name="apiIdentity">The Identity.</param>
        /// <param name="request">The request indicating the project, workflow activity, and request type.</param>
        /// <param name="permissionID">The permissions to check, or if null - all.</param>
        /// <returns></returns>
        public static IEnumerable<PermissionDefinition> GetGrantedWorkflowActivityPermissionsForRequest(this DataContext db, ApiIdentity apiIdentity, Request request, params Guid[] permissionID)
        {
            return GetGrantedPermissionsForWorkflowActivity(db, apiIdentity, request.ProjectID, request.WorkFlowActivityID.Value, request.RequestTypeID, permissionID);
        }

        /// <summary>
        /// Gets the permissions specified for the current workflow activity for the specified request and Identity.
        /// </summary>
        /// <param name="db">The DataContext.</param>
        /// <param name="apiIdentity">The Identity.</param>
        /// <param name="request">The request indicating the project, workflow activity, and request type.</param>
        /// <param name="permissionID">The permissions to check, or if null - all.</param>
        /// <returns></returns>
        public static async Task<IEnumerable<PermissionDefinition>> GetGrantedWorkflowActivityPermissionsForRequestAsync(this DataContext db, ApiIdentity apiIdentity, Request request, params Guid[] permissionID)
        {
            return await GetGrantedPermissionsForWorkflowActivityAsync(db, apiIdentity, request.ProjectID, request.WorkFlowActivityID.Value, request.RequestTypeID, permissionID);
        }

    }

}
