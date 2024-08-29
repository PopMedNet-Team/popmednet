using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Utilities.WebSites.Controllers;
using Lpp.Utilities;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lpp.Dns.DTO.Enums;
using System.Threading.Tasks;
using Lpp.Dns.DTO.Security;

namespace Lpp.Dns.Api.Security
{
    /// <summary>
    /// Controller that supports the Security
    /// </summary>
    public class SecurityController : LppApiController<DataContext>
    {
        /// <summary>
        /// Returns a list of Users and Security Groups using OData for Filtering and sorting
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<SecurityEntityDTO> ListSecurityEntities()
        {
            var users = from u in DataContext.Secure<User>(Identity)
                        where u.Deleted == false
                        select new SecurityEntityDTO
                        {
                            ID = u.ID,
                            Name = u.FirstName + " " + u.LastName,
                            Timestamp = u.Timestamp,
                            Type = SecurityEntityTypes.User
                        };

            var securityGroups = from sg in DataContext.Secure<SecurityGroup>(Identity)
                                 where DataContext.Organizations.Any(org => org.Deleted == false && org.ID == sg.OwnerID) || DataContext.Projects.Any(p => p.Deleted == false && p.ID == sg.OwnerID)
                                 select new SecurityEntityDTO
                                 {
                                     ID = sg.ID,
                                     Name = sg.Path,
                                     Timestamp = sg.Timestamp,
                                     Type = SecurityEntityTypes.SecurityGroup
                                 };

            return users.Concat(securityGroups);
        }
        /// <summary>
        /// Returns all Permissions based on locations
        /// </summary>
        /// <param name="locations"></param>
        /// <returns></returns>
        [HttpPost]
        public IQueryable<PermissionDTO> GetPermissionsByLocation(IEnumerable<PermissionAclTypes> locations)
        {
            var result = (from p in DataContext.Secure<Permission>(Identity) where p.Locations.Any(a => locations.Contains(a.Type)) orderby p.Name select p).Map<Permission, PermissionDTO>();

            return result;
        }

        /// <summary>
        /// Returns all of the permissions for the specified DataMart
        /// </summary>
        /// <param name="dataMartID"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<AclDataMartDTO> GetDataMartPermissions(Guid dataMartID)
        {
            var result = (from a in DataContext.Secure<AclDataMart>(Identity) where a.DataMartID == dataMartID && a.Overridden && (DataContext.Organizations.Any(org => org.Deleted == false && org.ID == a.SecurityGroup.OwnerID) || DataContext.Projects.Any(p => p.Deleted == false && p.ID == a.SecurityGroup.OwnerID)) select a).Map<AclDataMart, AclDataMartDTO>();

            return result;
        }

        /// <summary>
        /// Returns all of the permissions for the specified Organization
        /// </summary>
        /// <param name="organizationID"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<AclOrganizationDTO> GetOrganizationPermissions(Guid organizationID)
        {
            var result = (from a in DataContext.Secure<AclOrganization>(Identity) where a.OrganizationID == organizationID && a.Overridden && (DataContext.Organizations.Any(org => org.Deleted == false && org.ID == a.SecurityGroup.OwnerID) || DataContext.Projects.Any(p => p.Deleted == false && p.ID == a.SecurityGroup.OwnerID)) select a).Map<AclOrganization, AclOrganizationDTO>();

            return result;
        }

        /// <summary>
        /// Returns all of the permissions for the specified user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<AclUserDTO> GetUserPermissions(Guid userID)
        {
            var result = (from a in DataContext.Secure<AclUser>(Identity) where a.UserID == userID && a.Overridden && (DataContext.Organizations.Any(org => org.Deleted == false && org.ID == a.SecurityGroup.OwnerID) || DataContext.Projects.Any(p => p.Deleted == false && p.ID == a.SecurityGroup.OwnerID)) select a).Map<AclUser, AclUserDTO>();

            return result;
        }

        /// <summary>
        /// Returns all of the permissions for the specified group
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<AclGroupDTO> GetGroupPermissions(Guid groupID)
        {
            var result = (from a in DataContext.Secure<AclGroup>(Identity) where a.GroupID == groupID && a.Overridden && (DataContext.Organizations.Any(org => org.Deleted == false && org.ID == a.SecurityGroup.OwnerID) || DataContext.Projects.Any(p => p.Deleted == false && p.ID == a.SecurityGroup.OwnerID)) select a).Map<AclGroup, AclGroupDTO>();

            return result;
        }

        /// <summary>
        /// Returns all of the permissions for the specified registry
        /// </summary>
        /// <param name="registryID"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<AclRegistryDTO> GetRegistryPermissions(Guid registryID)
        {
            var result = (from a in DataContext.Secure<AclRegistry>(Identity) where a.RegistryID == registryID && a.Overridden && (DataContext.Organizations.Any(org => org.Deleted == false && org.ID == a.SecurityGroup.OwnerID) || DataContext.Projects.Any(p => p.Deleted == false && p.ID == a.SecurityGroup.OwnerID)) select a).Map<AclRegistry, AclRegistryDTO>();

            return result;
        }

        /// <summary>
        /// Returns all of the permissions for the specified project
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<AclProjectDTO> GetProjectPermissions(Guid projectID)
        {
            var result = (from a in DataContext.Secure<AclProject>(Identity) where a.ProjectID == projectID && a.Overridden && (DataContext.Organizations.Any(org => org.Deleted == false && org.ID == a.SecurityGroup.OwnerID) || DataContext.Projects.Any(p => p.Deleted == false && p.ID == a.SecurityGroup.OwnerID)) select a).Map<AclProject, AclProjectDTO>();

            return result;
        }

        /// <summary>
        /// Gets all Global Acls
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<AclDTO> GetGlobalPermissions()
        {
            var result = (from a in DataContext.Secure<AclGlobal>(Identity) where a.Overridden && (DataContext.Organizations.Any(org => org.Deleted == false && org.ID == a.SecurityGroup.OwnerID) || DataContext.Projects.Any(p => p.Deleted == false && p.ID == a.SecurityGroup.OwnerID)) select a).Map<AclGlobal, AclDTO>();

            return result;
        }

        /// <summary>
        /// Returns all of the permissions for the specified project and optionally the specified organization
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="organizationID"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<AclProjectOrganizationDTO> GetProjectOrganizationPermissions(Guid projectID, Guid? organizationID = null)
        {
            var result = (from a in DataContext.Secure<AclProjectOrganization>(Identity) where a.ProjectID == projectID && (organizationID == null || a.OrganizationID == organizationID.Value) && a.Overridden && (DataContext.Organizations.Any(org => org.Deleted == false && org.ID == a.SecurityGroup.OwnerID) || DataContext.Projects.Any(p => p.Deleted == false && p.ID == a.SecurityGroup.OwnerID)) select a).Map<AclProjectOrganization, AclProjectOrganizationDTO>();

            return result;
        }

        /// <summary>
        /// Returns all of the permissions for the specified project nad opitonal the specified request type and workflow activity
        /// </summary>
        /// <param name="projectID">The ID of the project to get the permissions for.</param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<AclProjectRequestTypeWorkflowActivityDTO> GetProjectRequestTypeWorkflowActivityPermissions(Guid projectID)
        {
            var result = (from a in DataContext.Secure<AclProjectRequestTypeWorkflowActivity>(Identity) where a.ProjectID == projectID && a.Overridden && (DataContext.Organizations.Any(org => org.Deleted == false && org.ID == a.SecurityGroup.OwnerID) || DataContext.Projects.Any(p => p.Deleted == false && p.ID == a.SecurityGroup.OwnerID)) select a).Map<AclProjectRequestTypeWorkflowActivity, AclProjectRequestTypeWorkflowActivityDTO>();

            return result;
        }

        /// <summary>
        /// Gets the workflow activity permissions for the specified project and Identity.
        /// </summary>
        /// <param name="projectID">The ID of the project.</param>
        /// <param name="identityID">The ID of the identity to get the permissions for.</param>
        /// <returns></returns>
        public IQueryable<AclProjectRequestTypeWorkflowActivityDTO> GetProjectRequestTypeWorkflowActivityPermissionForIdentity(Guid projectID, Guid identityID)
        {
            var result = (from a in DataContext.Secure<AclProjectRequestTypeWorkflowActivity>(Identity)
                          where a.ProjectID == projectID && a.Overridden
                              && DataContext.Users.Any(u => u.ID == identityID && u.SecurityGroups.Any(s => s.SecurityGroupID == a.SecurityGroupID))
                          select a).Map<AclProjectRequestTypeWorkflowActivity, AclProjectRequestTypeWorkflowActivityDTO>();

            return result;
        }

        /// <summary>
        /// Gets the granted permissions for the current user based on the specified project, workflow activity, request type, and requested permissions.
        /// </summary>
        /// <param name="projectID">The ID of the project.</param>
        /// <param name="workflowActivityID">The ID of the workflow activity.</param>
        /// <param name="requestTypeID">The request type ID.</param>
        /// <param name="permissionID">A collection of permissionID's to confirm permission for.</param>
        /// <returns>A collection of the permission ID's the current Identity has authorization for.</returns>
        [HttpGet]
        public async Task<Guid[]> GetWorkflowActivityPermissionsForIdentity(Guid projectID, Guid workflowActivityID, Guid requestTypeID, [FromUri]IEnumerable<Guid> permissionID)
        {
            var result = await DataContext.GetGrantedPermissionsForWorkflowActivityAsync(Identity, projectID, workflowActivityID, requestTypeID, (permissionID ?? new Guid[0]).ToArray());
            return result.Select(p => p.ID).ToArray();
        }

        /// <summary>
        /// Returns all of the permissions for the specified Project and DataMart
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="dataMartID"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<AclProjectDataMartDTO> GetProjectDataMartPermissions(Guid projectID, Guid? dataMartID = null)
        {
            var result = (from a in DataContext.Secure<AclProjectDataMart>(Identity) where a.ProjectID == projectID && (dataMartID == null || a.DataMartID == dataMartID.Value) && !a.DataMart.Deleted && a.Overridden select a).Map<AclProjectDataMart, AclProjectDataMartDTO>();
            return result;
        }

        /// <summary>
        /// Returns all of the permissions for the specified Project and DataMart
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="dataMartID"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<AclProjectDataMartRequestTypeDTO> GetProjectDataMartRequestTypePermissions(Guid projectID, Guid? dataMartID = null)
        {
            var result = (from a in DataContext.Secure<AclProjectDataMartRequestType>(Identity)
                          where a.ProjectID == projectID &&
                          (dataMartID == null || a.DataMartID == dataMartID.Value) &&
                          a.DataMart.Deleted == false &&
                          a.Overridden
                          select a)
                          .Map<AclProjectDataMartRequestType, AclProjectDataMartRequestTypeDTO>();
            return result;
        }

        /// <summary>
        /// Returns all of the permissions for the specified Project and DataMart
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<AclProjectRequestTypeDTO> GetProjectRequestTypePermissions(Guid projectID)
        {
            var result = (from a in DataContext.Secure<AclProjectRequestType>(Identity) where a.ProjectID == projectID && a.Overridden && (DataContext.Organizations.Any(org => org.Deleted == false && org.ID == a.SecurityGroup.OwnerID) || DataContext.Projects.Any(p => p.Deleted == false && p.ID == a.SecurityGroup.OwnerID)) select a).Map<AclProjectRequestType, AclProjectRequestTypeDTO>();
            return result;
        }

        /// <summary>
        /// Returns all of the request type permissions for the specified DataMart
        /// </summary>
        /// <param name="dataMartID"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<AclDataMartRequestTypeDTO> GetDataMartRequestTypePermissions(Guid dataMartID)
        {
            var result = (from a in DataContext.Secure<AclDataMartRequestType>(Identity) where a.DataMartID == dataMartID && a.Overridden && (DataContext.Organizations.Any(org => org.Deleted == false && org.ID == a.SecurityGroup.OwnerID) || DataContext.Projects.Any(p => p.Deleted == false && p.ID == a.SecurityGroup.OwnerID)) select a).Map<AclDataMartRequestType, AclDataMartRequestTypeDTO>();
            return result;
        }

        /// <summary>
        /// Returns all of the permissions for the specified template.
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<AclTemplateDTO> GetTemplatePermissions(Guid templateID)
        {
            var result = (from a in DataContext.Secure<AclTemplate>(Identity) where a.TemplateID == templateID && a.Overridden && (DataContext.Organizations.Any(org => org.Deleted == false && org.ID == a.SecurityGroup.OwnerID) || DataContext.Projects.Any(p => p.Deleted == false && p.ID == a.SecurityGroup.OwnerID)) select a).Map<AclTemplate, AclTemplateDTO>();
            return result;
        }

        /// <summary>
        /// Returns all of the permissions for the specified template.
        /// </summary>
        /// <param name="requestTypeID"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<AclRequestTypeDTO> GetRequestTypePermissions(Guid requestTypeID)
        {
            var result = (from a in DataContext.Secure<AclRequestType>(Identity) where a.RequestTypeID == requestTypeID && a.Overridden && (DataContext.Organizations.Any(org => org.Deleted == false && org.ID == a.SecurityGroup.OwnerID) || DataContext.Projects.Any(p => p.Deleted == false && p.ID == a.SecurityGroup.OwnerID)) select a).Map<AclRequestType, AclRequestTypeDTO>();
            return result;
        }

        /// <summary>
        /// Updates ProjectDataMartPermissions subject to security
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateProjectRequestTypeWorkflowActivityPermissions(IEnumerable<AclProjectRequestTypeWorkflowActivityDTO> permissions)
        {

            if (!permissions.Any())
                return Request.CreateResponse(HttpStatusCode.Accepted);

            //Non-optimal but no way else to do it in t-sql
            var projectIds = permissions.Select(p => p.ProjectID).ToArray();
            var requestTypeIds = permissions.Select(p => p.RequestTypeID).ToArray();
            var activityIds = permissions.Select(p => p.WorkflowActivityID).ToArray();

            //Has Permissions
            //if the user does not have has manage security permission allowed at global or project level, return errormessage
            if (
                (!await (DataContext.GlobalAcls.FilterAcl(Identity, PermissionIdentifiers.Project.ManageSecurity)).AnyAsync() &&
                !await (DataContext.GlobalAcls.FilterAcl(Identity, PermissionIdentifiers.Project.ManageSecurity)).AllAsync(a => a.Allowed)) ||
                (!await (from a in DataContext.ProjectAcls.FilterAcl(Identity, PermissionIdentifiers.Project.ManageSecurity) where projectIds.Contains(a.ProjectID) select a).AnyAsync() &&
                !await (from a in DataContext.ProjectAcls.FilterAcl(Identity, PermissionIdentifiers.Project.ManageSecurity) where projectIds.Contains(a.ProjectID) select a).AllAsync(a => a.Allowed)))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to manage one or more projects associated with the permissions passed.");
            }
            //if the user has manage security permission allowed at global level but denied at project level
            if (!await (from a in DataContext.ProjectAcls.FilterAcl(Identity, PermissionIdentifiers.Project.ManageSecurity) where projectIds.Contains(a.ProjectID) select a).AllAsync(a => a.Allowed))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to manage one or more projects associated with the permissions passed.");
            }

            var _dbAcls = await (from a in DataContext.ProjectRequestTypeWorkflowActivities where projectIds.Contains(a.ProjectID) && requestTypeIds.Contains(a.RequestTypeID) && activityIds.Contains(a.WorkflowActivityID) select a).ToArrayAsync();

            var acls = from a in _dbAcls join p in permissions on new { a.ProjectID, a.RequestTypeID, a.WorkflowActivityID, a.SecurityGroupID, a.PermissionID } equals new { p.ProjectID, p.RequestTypeID, p.WorkflowActivityID, p.SecurityGroupID, p.PermissionID } select new { dbAcl = a, permissions = p };

            foreach (var acl in acls)
            {
                if (acl.permissions.Allowed == null)
                {
                    DataContext.ProjectRequestTypeWorkflowActivities.Remove(acl.dbAcl);
                }
                else
                {
                    acl.dbAcl.Allowed = acl.permissions.Allowed.Value;
                    acl.dbAcl.Overridden = true;
                }
            }

            //Now add all of the ones that aren't in there.

            var newAcls = permissions.Where(p => p.Allowed.HasValue).Except(acls.Select(a => a.permissions));
            foreach (var acl in newAcls)
            {
                DataContext.ProjectRequestTypeWorkflowActivities.Add(new AclProjectRequestTypeWorkflowActivity
                {
                    Allowed = acl.Allowed.Value,
                    RequestTypeID = acl.RequestTypeID,
                    Overridden = true,
                    PermissionID = acl.PermissionID,
                    ProjectID = acl.ProjectID,
                    SecurityGroupID = acl.SecurityGroupID,
                    WorkflowActivityID = acl.WorkflowActivityID
                });
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Update list of request type permissions associated with Datamart
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateDataMartRequestTypePermissions(IEnumerable<AclDataMartRequestTypeDTO> permissions)
        {

            //Non-optimal but no way else to do it in t-sql
            var dataMartIds = permissions.Select(p => p.DataMartID).ToArray();
            var requestTypeIds = permissions.Select(p => p.RequestTypeID).ToArray();

            //Has Permissions
            if (!await (from a in DataContext.DataMartAcls.FilterAcl(Identity, PermissionIdentifiers.DataMart.ManageSecurity) where dataMartIds.Contains(a.DataMartID) select a).AllAsync(a => a.Allowed))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to manage one or more datamarts associated with the permissions passed.");

            var _dbAcls = await (from a in DataContext.DataMartRequestTypeAcls where dataMartIds.Contains(a.DataMartID) && requestTypeIds.Contains(a.RequestTypeID) select a).ToArrayAsync();

            var acls = from a in _dbAcls join p in permissions on new { a.DataMartID, a.RequestTypeID, a.SecurityGroupID } equals new { p.DataMartID, p.RequestTypeID, p.SecurityGroupID } select new { dbAcl = a, permissions = p };

            foreach (var acl in acls)
            {
                if (acl.permissions.Permission == null)
                {
                    DataContext.DataMartRequestTypeAcls.Remove(acl.dbAcl);
                }
                else
                {
                    if (acl.dbAcl.Permission != acl.permissions.Permission.Value)
                    {
                        DataContext.DataMartRequestTypeAcls.Remove(acl.dbAcl);
                        DataContext.DataMartRequestTypeAcls.Add(new AclDataMartRequestType
                        {
                            Permission = acl.permissions.Permission.Value,
                            DataMartID = acl.permissions.DataMartID,
                            Overridden = true,
                            RequestTypeID = acl.permissions.RequestTypeID,
                            SecurityGroupID = acl.permissions.SecurityGroupID
                        });
                    }
                }
            }

            //Now add all of the ones that aren't in there.

            var newAcls = permissions.Where(p => p.Permission.HasValue).Except(acls.Select(a => a.permissions));
            foreach (var acl in newAcls)
            {
                DataContext.DataMartRequestTypeAcls.Add(new AclDataMartRequestType
                {
                    Permission = acl.Permission.Value,
                    DataMartID = acl.DataMartID,
                    Overridden = true,
                    RequestTypeID = acl.RequestTypeID,
                    SecurityGroupID = acl.SecurityGroupID
                });
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Updates ProjectDataMartPermissions subject to security
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateProjectDataMartPermissions(IEnumerable<AclProjectDataMartDTO> permissions)
        {

            //Non-optimal but no way else to do it in t-sql
            var projectIds = permissions.Select(p => p.ProjectID).ToArray();
            var dataMartIds = permissions.Select(p => p.DataMartID).ToArray();

            //Has Permissions
            if (!await (from a in DataContext.ProjectAcls.FilterAcl(Identity, PermissionIdentifiers.Project.ManageSecurity) where projectIds.Contains(a.ProjectID) select a).AllAsync(a => a.Allowed))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to manage one or more projects associated with the permissions passed.");

            var _dbAcls = await (from a in DataContext.ProjectDataMartAcls where projectIds.Contains(a.ProjectID) && dataMartIds.Contains(a.DataMartID) select a).ToArrayAsync();

            var acls = from a in _dbAcls join p in permissions on new { a.ProjectID, a.DataMartID, a.SecurityGroupID, a.PermissionID } equals new { p.ProjectID, p.DataMartID, p.SecurityGroupID, p.PermissionID } select new { dbAcl = a, permissions = p };

            foreach (var acl in acls)
            {
                if (acl.permissions.Allowed == null)
                {
                    DataContext.ProjectDataMartAcls.Remove(acl.dbAcl);
                }
                else
                {
                    acl.dbAcl.Allowed = acl.permissions.Allowed.Value;
                    acl.dbAcl.Overridden = true;
                }
            }

            //Now add all of the ones that aren't in there.

            var newAcls = permissions.Where(p => p.Allowed.HasValue).Except(acls.Select(a => a.permissions));
            foreach (var acl in newAcls)
            {
                DataContext.ProjectDataMartAcls.Add(new AclProjectDataMart
                {
                    Allowed = acl.Allowed.Value,
                    DataMartID = acl.DataMartID,
                    Overridden = true,
                    PermissionID = acl.PermissionID,
                    ProjectID = acl.ProjectID,
                    SecurityGroupID = acl.SecurityGroupID
                });
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }
        /// <summary>
        /// update Project Datamart requesttye permissions subject to security
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateProjectDataMartRequestTypePermissions(IEnumerable<AclProjectDataMartRequestTypeDTO> permissions)
        {

            //Non-optimal but no way else to do it in t-sql
            var projectIds = permissions.Select(p => p.ProjectID).ToArray();
            var dataMartIds = permissions.Select(p => p.DataMartID).ToArray();
            var requestTypeIds = permissions.Select(p => p.RequestTypeID).ToArray();

            //Has Permissions
            if (!await (from a in DataContext.ProjectAcls.FilterAcl(Identity, PermissionIdentifiers.Project.ManageSecurity) where projectIds.Contains(a.ProjectID) select a).AllAsync(a => a.Allowed))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to manage one or more projects associated with the permissions passed.");

            var _dbAcls = await (from a in DataContext.ProjectDataMartRequestTypeAcls where projectIds.Contains(a.ProjectID) && dataMartIds.Contains(a.DataMartID) && requestTypeIds.Contains(a.RequestTypeID) select a).ToArrayAsync();

            var acls = from a in _dbAcls join p in permissions on new { a.ProjectID, a.DataMartID, a.RequestTypeID, a.SecurityGroupID } equals new { p.ProjectID, p.DataMartID, p.RequestTypeID, p.SecurityGroupID } select new { dbAcl = a, permissions = p };

            foreach (var acl in acls)
            {
                if (acl.permissions.Permission == null)
                {
                    DataContext.ProjectDataMartRequestTypeAcls.Remove(acl.dbAcl);
                }
                else
                {
                    if (acl.dbAcl.Permission != acl.permissions.Permission.Value)
                    {
                        DataContext.ProjectDataMartRequestTypeAcls.Remove(acl.dbAcl);
                        DataContext.ProjectDataMartRequestTypeAcls.Add(new AclProjectDataMartRequestType
                        {
                            Permission = acl.permissions.Permission.Value,
                            Overridden = true,
                            ProjectID = acl.permissions.ProjectID,
                            DataMartID = acl.permissions.DataMartID,
                            RequestTypeID = acl.permissions.RequestTypeID,
                            SecurityGroupID = acl.permissions.SecurityGroupID
                        });
                    }
                }
            }

            //Now add all of the ones that aren't in there.

            var newAcls = permissions.Where(p => p.Permission.HasValue).Except(acls.Select(a => a.permissions));
            foreach (var acl in newAcls)
            {
                DataContext.ProjectDataMartRequestTypeAcls.Add(new AclProjectDataMartRequestType
                {
                    Permission = acl.Permission.Value,
                    DataMartID = acl.DataMartID,
                    Overridden = true,
                    ProjectID = acl.ProjectID,
                    RequestTypeID = acl.RequestTypeID,
                    SecurityGroupID = acl.SecurityGroupID
                });
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }


        /// <summary>
        /// Updates ProjectOrganizationPermissions subject to security
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateProjectOrganizationPermissions(IEnumerable<AclProjectOrganizationDTO> permissions)
        {

            //Non-optimal but no way else to do it in t-sql
            var projectIds = permissions.Select(p => p.ProjectID).ToArray();
            var organizationIDs = permissions.Select(p => p.OrganizationID).ToArray();

            //Has Permissions
            if (!await (from a in DataContext.ProjectAcls.FilterAcl(Identity, PermissionIdentifiers.Project.ManageSecurity) where projectIds.Contains(a.ProjectID) select a).AllAsync(a => a.Allowed))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to manage one or more projects associated with the permissions passed.");

            var _dbAcls = await (from a in DataContext.ProjectOrganizationAcls where projectIds.Contains(a.ProjectID) && organizationIDs.Contains(a.OrganizationID) select a).ToArrayAsync();

            var acls = from a in _dbAcls join p in permissions on new { a.ProjectID, a.OrganizationID, a.SecurityGroupID, a.PermissionID } equals new { p.ProjectID, p.OrganizationID, p.SecurityGroupID, p.PermissionID } select new { dbAcl = a, permissions = p };

            foreach (var acl in acls)
            {
                if (acl.permissions.Allowed == null)
                {
                    DataContext.ProjectOrganizationAcls.Remove(acl.dbAcl);
                }
                else
                {
                    acl.dbAcl.Allowed = acl.permissions.Allowed.Value;
                    acl.dbAcl.Overridden = true;
                }
            }

            //Now add all of the ones that aren't in there.

            var newAcls = permissions.Where(p => p.Allowed.HasValue).Except(acls.Select(a => a.permissions));
            foreach (var acl in newAcls)
            {
                DataContext.ProjectOrganizationAcls.Add(new AclProjectOrganization
                {
                    Allowed = acl.Allowed.Value,
                    OrganizationID = acl.OrganizationID,
                    Overridden = true,
                    PermissionID = acl.PermissionID,
                    ProjectID = acl.ProjectID,
                    SecurityGroupID = acl.SecurityGroupID
                });
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Updates the global permissions specified
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UpdatePermissions(IEnumerable<AclDTO> permissions)
        {

            var securityGroupIds = permissions.Select(p => p.SecurityGroupID).ToArray();
            var PermissionIdentifierss = permissions.Select(p => p.PermissionID).ToArray();

            //Has Permissions
            if (!await (from a in DataContext.GlobalAcls.FilterAcl(Identity, PermissionIdentifiers.Portal.ManageSecurity) select a).AllAsync(a => a.Allowed))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to manage these permissions.");

            var _dbAcls = await (from a in DataContext.GlobalAcls where securityGroupIds.Contains(a.SecurityGroupID) && PermissionIdentifierss.Contains(a.PermissionID) select a).ToArrayAsync();

            var acls = from a in _dbAcls join p in permissions on new { a.SecurityGroupID, a.PermissionID } equals new { p.SecurityGroupID, p.PermissionID } select new { dbAcl = a, permissions = p };

            foreach (var acl in acls)
            {
                if (acl.permissions.Allowed == null)
                {
                    DataContext.GlobalAcls.Remove(acl.dbAcl);
                }
                else
                {
                    acl.dbAcl.Allowed = acl.permissions.Allowed.Value;
                    acl.dbAcl.Overridden = true;
                }
            }

            //Now add all of the ones that aren't in there.

            var newAcls = permissions.Where(p => p.Allowed.HasValue).Except(acls.Select(a => a.permissions));
            foreach (var acl in newAcls)
            {
                DataContext.GlobalAcls.Add(new AclGlobal
                {
                    Allowed = acl.Allowed.Value,
                    Overridden = true,
                    PermissionID = acl.PermissionID,
                    SecurityGroupID = acl.SecurityGroupID
                });
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }


        /// <summary>
        /// Updates Group Permissions subject to security
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateGroupPermissions(IEnumerable<AclGroupDTO> permissions)
        {

            //Non-optimal but no way else to do it in t-sql
            var groupIds = permissions.Select(g => g.GroupID).ToArray();

            //Has Permissions
            if (!await (from a in DataContext.GroupAcls.FilterAcl(Identity, PermissionIdentifiers.Group.ManageSecurity) where groupIds.Contains(a.GroupID) select a).AllAsync(a => a.Allowed))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to manage one or more groups associated with the permissions passed.");

            var _dbAcls = await (from a in DataContext.GroupAcls where groupIds.Contains(a.GroupID) select a).ToArrayAsync();

            var acls = from a in _dbAcls join p in permissions on new { a.GroupID, a.SecurityGroupID, a.PermissionID } equals new { p.GroupID, p.SecurityGroupID, p.PermissionID } select new { dbAcl = a, permissions = p };

            foreach (var acl in acls)
            {
                if (acl.permissions.Allowed == null)
                {
                    DataContext.GroupAcls.Remove(acl.dbAcl);
                }
                else
                {
                    acl.dbAcl.Allowed = acl.permissions.Allowed.Value;
                    acl.dbAcl.Overridden = true;
                }
            }

            //Now add all of the ones that aren't in there.

            var newAcls = permissions.Where(p => p.Allowed.HasValue).Except(acls.Select(a => a.permissions));
            foreach (var acl in newAcls)
            {
                DataContext.GroupAcls.Add(new AclGroup
                {
                    Allowed = acl.Allowed.Value,
                    Overridden = true,
                    PermissionID = acl.PermissionID,
                    GroupID = acl.GroupID,
                    SecurityGroupID = acl.SecurityGroupID
                });
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Updates Registry Permissions subject to security
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateRegistryPermissions(IEnumerable<AclRegistryDTO> permissions)
        {

            //Non-optimal but no way else to do it in t-sql
            var registryIds = permissions.Select(r => r.RegistryID).ToArray();

            //Has Permissions
            if (!await (from a in DataContext.RegistryAcls.FilterAcl(Identity, PermissionIdentifiers.Registry.ManageSecurity) where registryIds.Contains(a.RegistryID) select a).AllAsync(a => a.Allowed))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to manage one or more registries associated with the permissions passed.");

            var _dbAcls = await (from a in DataContext.RegistryAcls where registryIds.Contains(a.RegistryID) select a).ToArrayAsync();

            var acls = from a in _dbAcls join p in permissions on new { a.RegistryID, a.SecurityGroupID, a.PermissionID } equals new { p.RegistryID, p.SecurityGroupID, p.PermissionID } select new { dbAcl = a, permissions = p };

            foreach (var acl in acls)
            {
                if (acl.permissions.Allowed == null)
                {
                    DataContext.RegistryAcls.Remove(acl.dbAcl);
                }
                else
                {
                    acl.dbAcl.Allowed = acl.permissions.Allowed.Value;
                    acl.dbAcl.Overridden = true;
                }
            }

            //Now add all of the ones that aren't in there.

            var newAcls = permissions.Where(p => p.Allowed.HasValue).Except(acls.Select(a => a.permissions));
            foreach (var acl in newAcls)
            {
                DataContext.RegistryAcls.Add(new AclRegistry
                {
                    Allowed = acl.Allowed.Value,
                    Overridden = true,
                    PermissionID = acl.PermissionID,
                    RegistryID = acl.RegistryID,
                    SecurityGroupID = acl.SecurityGroupID
                });
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Updates Project Permissions subject to security
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateProjectPermissions(IEnumerable<AclProjectDTO> permissions)
        {

            //Non-optimal but no way else to do it in t-sql
            var projectIds = permissions.Select(p => p.ProjectID).ToArray();

            //Has Permissions
            if (!await (from a in DataContext.ProjectAcls.FilterAcl(Identity, PermissionIdentifiers.Project.ManageSecurity) where projectIds.Contains(a.ProjectID) select a).AllAsync(a => a.Allowed))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to manage one or more projects associated with the permissions passed.");

            var _dbAcls = await (from a in DataContext.ProjectAcls where projectIds.Contains(a.ProjectID) select a).ToArrayAsync();

            var acls = from a in _dbAcls join p in permissions on new { a.ProjectID, a.SecurityGroupID, a.PermissionID } equals new { p.ProjectID, p.SecurityGroupID, p.PermissionID } select new { dbAcl = a, permissions = p };

            foreach (var acl in acls)
            {
                if (acl.permissions.Allowed == null)
                {
                    DataContext.ProjectAcls.Remove(acl.dbAcl);
                }
                else
                {
                    acl.dbAcl.Allowed = acl.permissions.Allowed.Value;
                    acl.dbAcl.Overridden = true;
                }
            }

            await DataContext.SaveChangesAsync();

            //Now add all of the ones that aren't in there.

            var newAcls = permissions.Where(p => p.Allowed.HasValue).Except(acls.Select(a => a.permissions));
            foreach (var acl in newAcls)
            {
                DataContext.ProjectAcls.Add(new AclProject
                {
                    Allowed = acl.Allowed.Value,
                    Overridden = true,
                    PermissionID = acl.PermissionID,
                    ProjectID = acl.ProjectID,
                    SecurityGroupID = acl.SecurityGroupID
                });
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Updates Project Permissions subject to security
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateProjectRequestTypePermissions(IEnumerable<AclProjectRequestTypeDTO> permissions)
        {

            //Non-optimal but no way else to do it in t-sql
            var projectIds = permissions.Select(p => p.ProjectID).Distinct().ToArray();
            var requestTypeIDs = permissions.Select(p => p.RequestTypeID).Distinct().ToArray();

            //Has Permissions
            if (!await (from a in DataContext.ProjectAcls.FilterAcl(Identity, PermissionIdentifiers.Project.ManageSecurity) where projectIds.Contains(a.ProjectID) select a).AllAsync(a => a.Allowed))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to manage one or more projects associated with the permissions passed.");

            var _dbAcls = await (from a in DataContext.ProjectRequestTypeAcls where projectIds.Contains(a.ProjectID) && requestTypeIDs.Contains(a.RequestTypeID) select a).ToArrayAsync();

            var acls = from a in _dbAcls join p in permissions on new { a.ProjectID, a.RequestTypeID, a.SecurityGroupID } equals new { p.ProjectID, p.RequestTypeID, p.SecurityGroupID } select new { dbAcl = a, permissions = p };

            foreach (var acl in acls)
            {
                if (acl.permissions.Permission == null)
                {
                    DataContext.ProjectRequestTypeAcls.Remove(acl.dbAcl);
                }
                else
                {
                    if (acl.dbAcl.Permission != acl.permissions.Permission.Value)
                    {
                        DataContext.ProjectRequestTypeAcls.Remove(acl.dbAcl);
                        DataContext.ProjectRequestTypeAcls.Add(new AclProjectRequestType
                        {
                            Permission = acl.permissions.Permission.Value,
                            Overridden = true,
                            ProjectID = acl.permissions.ProjectID,
                            RequestTypeID = acl.permissions.RequestTypeID,
                            SecurityGroupID = acl.permissions.SecurityGroupID
                        });
                    }
                }
            }

            //Now add all of the ones that aren't in there.

            var newAcls = permissions.Where(p => p.Permission.HasValue).Except(acls.Select(a => a.permissions));
            foreach (var acl in newAcls)
            {
                DataContext.ProjectRequestTypeAcls.Add(new AclProjectRequestType
                {
                    Permission = acl.Permission.Value,
                    Overridden = true,
                    ProjectID = acl.ProjectID,
                    RequestTypeID = acl.RequestTypeID,
                    SecurityGroupID = acl.SecurityGroupID
                });
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }


        /// <summary>
        /// Updates DataMart Permissions subject to security
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateDataMartPermissions(IEnumerable<AclDataMartDTO> permissions)
        {

            //Non-optimal but no way else to do it in t-sql
            var dataMartIDs = permissions.Select(p => p.DataMartID).ToArray();

            //Has Permissions
            if (!await (from a in DataContext.DataMartAcls.FilterAcl(Identity, PermissionIdentifiers.DataMart.ManageSecurity) where dataMartIDs.Contains(a.DataMartID) select a).AllAsync(a => a.Allowed))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to manage one or more DataMarts associated with the permissions passed.");

            var _dbAcls = await (from a in DataContext.DataMartAcls where dataMartIDs.Contains(a.DataMartID) select a).ToArrayAsync();

            var acls = from a in _dbAcls join p in permissions on new { a.DataMartID, a.SecurityGroupID, a.PermissionID } equals new { p.DataMartID, p.SecurityGroupID, p.PermissionID } select new { dbAcl = a, permissions = p };

            foreach (var acl in acls)
            {
                if (acl.permissions.Allowed == null)
                {
                    DataContext.DataMartAcls.Remove(acl.dbAcl);
                }
                else
                {
                    acl.dbAcl.Allowed = acl.permissions.Allowed.Value;
                    acl.dbAcl.Overridden = true;
                }
            }

            //Now add all of the ones that aren't in there.

            var newAcls = permissions.Where(p => p.Allowed.HasValue).Except(acls.Select(a => a.permissions));
            foreach (var acl in newAcls)
            {
                DataContext.DataMartAcls.Add(new AclDataMart
                {
                    Allowed = acl.Allowed.Value,
                    Overridden = true,
                    PermissionID = acl.PermissionID,
                    DataMartID = acl.DataMartID,
                    SecurityGroupID = acl.SecurityGroupID
                });
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Updates Organization Permissions subject to security
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateOrganizationPermissions(IEnumerable<AclOrganizationDTO> permissions)
        {

            //Non-optimal but no way else to do it in t-sql
            var organizationIDs = permissions.Select(p => p.OrganizationID).ToArray();

            //Has Permissions
            if (!await (from a in DataContext.OrganizationAcls.FilterAcl(Identity, PermissionIdentifiers.Organization.ManageSecurity) where organizationIDs.Contains(a.OrganizationID) select a).AllAsync(a => a.Allowed))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to manage one or more Organizations associated with the permissions passed.");

            var _dbAcls = await (from a in DataContext.OrganizationAcls where organizationIDs.Contains(a.OrganizationID) select a).ToArrayAsync();

            var acls = from a in _dbAcls join p in permissions on new { a.OrganizationID, a.SecurityGroupID, a.PermissionID } equals new { p.OrganizationID, p.SecurityGroupID, p.PermissionID } select new { dbAcl = a, permissions = p };

            foreach (var acl in acls)
            {
                if (acl.permissions.Allowed == null)
                {
                    DataContext.OrganizationAcls.Remove(acl.dbAcl);
                }
                else
                {
                    acl.dbAcl.Allowed = acl.permissions.Allowed.Value;
                    acl.dbAcl.Overridden = true;
                }
            }

            //Now add all of the ones that aren't in there.

            var newAcls = permissions.Where(p => p.Allowed.HasValue).Except(acls.Select(a => a.permissions));
            foreach (var acl in newAcls)
            {
                DataContext.OrganizationAcls.Add(new AclOrganization
                {
                    Allowed = acl.Allowed.Value,
                    Overridden = true,
                    PermissionID = acl.PermissionID,
                    OrganizationID = acl.OrganizationID,
                    SecurityGroupID = acl.SecurityGroupID
                });
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Updates user permissions subject to security
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateUserPermissions(IEnumerable<AclUserDTO> permissions)
        {

            //Non-optimal but no way else to do it in t-sql
            var userIDs = permissions.Select(p => p.UserID).ToArray();

            //Has Permissions
            if (!await (from a in DataContext.UserAcls.FilterAcl(Identity, PermissionIdentifiers.User.ManageSecurity) where userIDs.Contains(a.UserID) select a).AllAsync(a => a.Allowed))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to manage one or more users associated with the permissions passed.");

            var _dbAcls = await (from a in DataContext.UserAcls where userIDs.Contains(a.UserID) select a).ToArrayAsync();

            var acls = from a in _dbAcls join p in permissions on new { a.UserID, a.SecurityGroupID, a.PermissionID } equals new { p.UserID, p.SecurityGroupID, p.PermissionID } select new { dbAcl = a, permissions = p };

            foreach (var acl in acls)
            {
                if (acl.permissions.Allowed == null)
                {
                    DataContext.UserAcls.Remove(acl.dbAcl);
                }
                else
                {
                    acl.dbAcl.Allowed = acl.permissions.Allowed.Value;
                    acl.dbAcl.Overridden = true;
                }
            }

            //Now add all of the ones that aren't in there.

            var newAcls = permissions.Where(p => p.Allowed.HasValue).Except(acls.Select(a => a.permissions));
            foreach (var acl in newAcls)
            {
                DataContext.UserAcls.Add(new AclUser
                {
                    Allowed = acl.Allowed.Value,
                    Overridden = true,
                    PermissionID = acl.PermissionID,
                    UserID = acl.UserID,
                    SecurityGroupID = acl.SecurityGroupID
                });
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }


        /// <summary>
        /// Returns a grouped tree of available security groups that can be added for ACL purposes.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<TreeItemDTO>> GetAvailableSecurityGroupTree()
        {
            var tree = new List<TreeItemDTO>();

            var groups = await (from sg in DataContext.Secure<SecurityGroup>(Identity)
                                where (sg.Type == SecurityGroupTypes.Organization && !DataContext.Organizations.Where(o => o.ID == sg.OwnerID).FirstOrDefault().Deleted) || !DataContext.Projects.Where(p => p.ID == sg.OwnerID).FirstOrDefault().Deleted
                                group sg by sg.Type into g
                                select new
                                {
                                    Type = g.Key,
                                    Parents = (from p in g
                                               group p by new { p.OwnerID, p.Owner } into parent
                                               select new
                                               {
                                                   OwnerID = parent.Key.OwnerID,
                                                   Owner = parent.Key.Owner,
                                                   Groups = (from s in parent
                                                             orderby s.Name
                                                             select s)
                                               })
                                }
                          ).ToArrayAsync();


            if (groups.Any(g => g.Type == SecurityGroupTypes.Project))
            {
                var projects = new TreeItemDTO
                {
                    ID = null,
                    Name = "Projects",
                    Type = 0, //Main Group   
                    HasChildren = true
                };

                var projectItems = new List<TreeItemDTO>();

                foreach (var proj in groups.First(g => g.Type == SecurityGroupTypes.Project).Parents)
                {

                    var project = new TreeItemDTO
                    {
                        ID = null, //Intentionally set to null because we don't want it to link to anything.
                        Name = proj.Owner,
                        Type = 1, //Project
                        SubItems = proj.Groups.Select(t => new TreeItemDTO
                        {
                            HasChildren = false,
                            ID = t.ID,
                            Name = t.Name,
                            Path = t.Path,
                            SubItems = null,
                            Type = 3
                        }),
                        HasChildren = true
                    };


                    projectItems.Add(project);
                }

                projects.SubItems = projectItems;
                tree.Add(projects);
            }

            if (groups.Any(g => g.Type == SecurityGroupTypes.Organization))
            {
                var orgs = new TreeItemDTO
                {
                    ID = null,
                    Name = "Organizations",
                    Type = 0, //Main Group              
                    HasChildren = true
                };

                var orgItems = new List<TreeItemDTO>();

                foreach (var org in groups.First(g => g.Type == SecurityGroupTypes.Organization).Parents)
                {

                    var organization = new TreeItemDTO
                    {
                        ID = null, //Intentionally set to null because we don't want it to link to anything.
                        Name = org.Owner,
                        Type = 2, //Organization
                        SubItems = org.Groups.Select(t => new TreeItemDTO
                        {
                            HasChildren = false,
                            ID = t.ID,
                            Name = t.Name,
                            Path = t.Path,
                            SubItems = null,
                            Type = 3
                        }),
                        HasChildren = true
                    };


                    orgItems.Add(organization);
                }

                orgs.SubItems = orgItems;
                tree.Add(orgs);

            }

            return tree;
        }

        /// <summary>
        /// Updates Template Permissions subject to security
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateTemplatePermissions(IEnumerable<AclTemplateDTO> permissions)
        {

            //Non-optimal but no way else to do it in t-sql
            var templateIDs = permissions.Select(r => r.TemplateID).ToArray();

            //Has Permissions
            if (!await (from a in DataContext.TemplateAcls.FilterAcl(Identity, PermissionIdentifiers.Templates.ManageSecurity) where templateIDs.Contains(a.TemplateID) select a).AllAsync(a => a.Allowed))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to manage one or more templates associated with the permissions passed.");

            var _dbAcls = await (from a in DataContext.TemplateAcls where templateIDs.Contains(a.TemplateID) select a).ToArrayAsync();

            var acls = from a in _dbAcls join p in permissions on new { a.TemplateID, a.SecurityGroupID, a.PermissionID } equals new { p.TemplateID, p.SecurityGroupID, p.PermissionID } select new { dbAcl = a, permissions = p };

            foreach (var acl in acls)
            {
                if (acl.permissions.Allowed == null)
                {
                    DataContext.TemplateAcls.Remove(acl.dbAcl);
                }
                else
                {
                    acl.dbAcl.Allowed = acl.permissions.Allowed.Value;
                    acl.dbAcl.Overridden = true;
                }
            }

            //Now add all of the ones that aren't in there.
            var newAcls = permissions.Where(p => p.Allowed.HasValue).Except(acls.Select(a => a.permissions));
            foreach (var acl in newAcls)
            {
                DataContext.TemplateAcls.Add(new AclTemplate
                {
                    Allowed = acl.Allowed.Value,
                    Overridden = true,
                    PermissionID = acl.PermissionID,
                    TemplateID = acl.TemplateID,
                    SecurityGroupID = acl.SecurityGroupID
                });
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Updates Template Permissions subject to security
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateRequestTypePermissions(IEnumerable<AclRequestTypeDTO> permissions)
        {
            var requestTypeIDs = permissions.Select(r => r.RequestTypeID).Distinct().ToArray();

            if (!(await DataContext.HasGrantedPermissions<RequestType>(Identity, requestTypeIDs, new[] { PermissionIdentifiers.RequestTypes.ManageSecurity.ID })).Any())
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to manage one or more Request Types associated with the permissions passed.");
            }

            List<AclRequestType> updatedAcls = new List<AclRequestType>();

            var dbAcls = await (from a in DataContext.RequestTypeAcls where requestTypeIDs.Contains(a.RequestTypeID) select a).ToArrayAsync();

            foreach (var acl in dbAcls)
            {
                var perm = permissions.FirstOrDefault(p => p.RequestTypeID == acl.RequestTypeID && p.SecurityGroupID == acl.SecurityGroupID && p.PermissionID == acl.PermissionID);
                if (perm == null || perm.Allowed == null)
                {
                    DataContext.RequestTypeAcls.Remove(acl);
                }
                else if(acl.Allowed != perm.Allowed.Value)
                {
                    acl.Allowed = perm.Allowed.Value;
                    acl.Overridden = true;
                    updatedAcls.Add(acl);
                }
            }

            //Now add all of the ones that aren't in there.
            var newAcls = permissions.Where(p => p.Allowed.HasValue && dbAcls.Any(a => a.RequestTypeID == p.RequestTypeID && p.SecurityGroupID == a.SecurityGroupID && p.PermissionID == a.PermissionID) == false).ToArray();
            foreach (var acl in newAcls)
            {
                updatedAcls.Add(DataContext.RequestTypeAcls.Add(new AclRequestType
                {
                    Allowed = acl.Allowed.Value,
                    Overridden = true,
                    PermissionID = acl.PermissionID,
                    RequestTypeID = acl.RequestTypeID,
                    SecurityGroupID = acl.SecurityGroupID
                }));
            }

            await DataContext.SaveChangesAsync();

            //For each Request Type permission being updated, the associated template permission should also be updated

            var requestTypes = await DataContext.RequestTypes.Where(rt => requestTypeIDs.Contains(rt.ID) && rt.Queries.Any()).SelectMany(rt => rt.Queries.Select(q => new { rt.ID, TemplateID = q.ID })).ToDictionaryAsync(rt => rt.ID);
            
            var permissionIDMap = new Dictionary<Guid, Guid> {
                { PermissionIdentifiers.RequestTypes.View.ID, PermissionIdentifiers.Templates.View.ID },
                { PermissionIdentifiers.RequestTypes.ManageSecurity.ID, PermissionIdentifiers.Templates.ManageSecurity.ID },
                { PermissionIdentifiers.RequestTypes.Edit.ID, PermissionIdentifiers.Templates.Edit.ID },
                { PermissionIdentifiers.RequestTypes.Delete.ID, PermissionIdentifiers.Templates.Delete.ID },
            };

            //Transpose the requestType permissions to template permissions
            var templatePermissionsRequest = permissions.Select(p => new
            {
                p.Allowed,
                p.SecurityGroupID,
                TemplateID = requestTypes[p.RequestTypeID].TemplateID,
                PermissionID = permissionIDMap[p.PermissionID]

            });

            var templatePermissionIDs = requestTypes.Select(rt => rt.Value.TemplateID).Distinct();
            var templatePermissions = await DataContext.TemplateAcls.Where(t => templatePermissionIDs.Contains(t.TemplateID)).ToArrayAsync();

            foreach (var tmpACL in templatePermissions)
            {
                var perm = templatePermissionsRequest.FirstOrDefault(a => a.TemplateID == tmpACL.TemplateID && a.SecurityGroupID == tmpACL.SecurityGroupID && a.PermissionID == tmpACL.PermissionID);
                if (perm == null || perm.Allowed == null)
                {
                    //Cannot do traditional Remove Due to Trigger
                    //DataContext.TemplateAcls.Remove(tmpACL);
                    await DataContext.Database.ExecuteSqlCommandAsync("Delete from AclTemplates Where TemplateID = {0} AND SecurityGroupID = {1} AND PermissionID = {2}", tmpACL.TemplateID, tmpACL.SecurityGroupID, tmpACL.PermissionID);

                }
                else if (tmpACL.Allowed != perm.Allowed.Value)
                {
                    tmpACL.Allowed = perm.Allowed.Value;
                    tmpACL.Overridden = true;
                }
            }

            var newTemplateACLs = templatePermissionsRequest.Where(p => p.Allowed.HasValue && templatePermissions.Any(a => a.PermissionID == p.PermissionID && a.SecurityGroupID == p.SecurityGroupID && a.TemplateID == p.TemplateID) == false).ToArray();
            foreach (var acl in newTemplateACLs)
            {
                DataContext.TemplateAcls.Add(
                    new AclTemplate
                    {
                        Allowed = acl.Allowed.Value,
                        Overridden = true,
                        PermissionID = acl.PermissionID,
                        TemplateID = acl.TemplateID,
                        SecurityGroupID = acl.SecurityGroupID
                    }
                    );
            }

            await DataContext.SaveChangesAsync();


            return Request.CreateResponse(HttpStatusCode.Accepted);
        }


        /// <summary>
        /// Gets all Global Field Option Acls
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<AclGlobalFieldOptionDTO> GetGlobalFieldOptionPermissions()
        {
            var result = (from a in DataContext.GlobalFieldOptionAcls where a.Overridden select a).Map<AclGlobalFieldOption, AclGlobalFieldOptionDTO>();

            return result;
        }

        /// <summary>
        /// Updates Global Field Option Permissions subject to security
        /// </summary>
        /// <param name="fieldOptionUpdates"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateFieldOptionPermissions(IEnumerable<AclGlobalFieldOptionDTO> fieldOptionUpdates)
        {
            if (fieldOptionUpdates == null)
                fieldOptionUpdates = Enumerable.Empty<AclGlobalFieldOptionDTO>();

            //Has Permissions
            if (!await (from a in DataContext.RequestTypeAcls.FilterAcl(Identity, PermissionIdentifiers.Portal.ManageSecurity) select a).AllAsync(a => a.Allowed))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to manage these permissions.");

            var dbAcls = await (from a in DataContext.GlobalFieldOptionAcls select a).ToListAsync();

            //for global try to always have a field option existing, default will be optional - this may need to change for certain fields in future like Request.Name
            foreach (var key in FieldOptionIdentifiers.AllFieldOptionKeys)
            {
                AclGlobalFieldOption existing = dbAcls.FirstOrDefault(k => k.FieldIdentifier == key);
                if (existing == null)
                {
                    existing = DataContext.GlobalFieldOptionAcls.Add(new AclGlobalFieldOption { FieldIdentifier = key, Overridden = true, Permission = FieldOptionPermissions.Optional });
                }

                AclGlobalFieldOptionDTO update = fieldOptionUpdates.FirstOrDefault(k => key.Equals(k.FieldIdentifier, StringComparison.OrdinalIgnoreCase));
                if (update != null && update.Permission != existing.Permission)
                {
                    //since the permission setting is part of the key, need to remove existing and re-add with updated value
                    DataContext.GlobalFieldOptionAcls.Remove(existing);
                    dbAcls.Remove(existing);

                    dbAcls.Add(DataContext.GlobalFieldOptionAcls.Add(new AclGlobalFieldOption { FieldIdentifier = key, Overridden = true, Permission = update.Permission }));
                }
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Returns all of the Field option permissions for the specified Project
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<AclProjectFieldOptionDTO> GetProjectFieldOptionPermissions(Guid projectID)
        {
            var result = (from a in DataContext.ProjectFieldOptionAcls where a.ProjectID == projectID && a.Overridden select a).Map<AclProjectFieldOption, AclProjectFieldOptionDTO>();
            return result;
        }

        /// <summary>
        /// Updates Project Field Option Permissions subject to security
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateProjectFieldOptionPermissions(IEnumerable<AclProjectFieldOptionDTO> permissions)
        {
            if (permissions == null)
                permissions = Enumerable.Empty<AclProjectFieldOptionDTO>();

            var projectIds = permissions.Select(p => p.ProjectID).Distinct().ToArray();

            if (!projectIds.Any())
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "A project must be specified to update the field options for.");

            //Has Permissions
            if (!await (from a in DataContext.ProjectAcls.FilterAcl(Identity, PermissionIdentifiers.Project.ManageSecurity) where projectIds.Contains(a.ProjectID) select a).AllAsync(a => a.Allowed))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to manage one or more projects associated with the permissions passed.");

            var securityGroupIds = permissions.Select(s => s.SecurityGroupID).Distinct().ToArray();

            foreach (var sg in securityGroupIds)
            {
                var existingFieldOptions = await DataContext.ProjectFieldOptionAcls.Where(a => (projectIds.Contains(a.ProjectID)) && (a.SecurityGroupID == sg)).ToArrayAsync();
                foreach (var key in FieldOptionIdentifiers.AllFieldOptionKeys)
                {
                    var updateValue = permissions.FirstOrDefault(a => key.Equals(a.FieldIdentifier, StringComparison.OrdinalIgnoreCase) && (a.SecurityGroupID == sg));
                    if (updateValue == null)
                        continue;

                    var existingValue = existingFieldOptions.FirstOrDefault(a => (a.FieldIdentifier == key));

                    //no existing value which defaults to global setting, and the update value is for Inherit -> nothing to do, move on
                    if ((existingValue == null || existingValue.Overridden == false) && updateValue.Permission == FieldOptionPermissions.Inherit)
                        continue;

                    //update value is the same as the current value -> nothing to do, move on
                    if (existingValue != null && existingValue.Permission == updateValue.Permission && updateValue.Permission != FieldOptionPermissions.Inherit)
                        continue;

                    if (existingValue != null)
                    {
                        //since the permission setting is part of the key, need to remove existing and re-add with updated value
                        DataContext.ProjectFieldOptionAcls.Remove(existingValue);
                    }

                    if (updateValue.Permission != FieldOptionPermissions.Inherit && (existingValue == null || existingValue.Permission != updateValue.Permission))
                    {
                        DataContext.ProjectFieldOptionAcls.Add(new AclProjectFieldOption { FieldIdentifier = key, ProjectID = updateValue.ProjectID, Permission = updateValue.Permission, Overridden = true, SecurityGroupID = updateValue.SecurityGroupID });
                    }
                }
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }


    }


}
