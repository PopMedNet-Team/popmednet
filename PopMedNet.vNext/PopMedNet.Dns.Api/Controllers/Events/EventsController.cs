using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopMedNet.Dns.DTO;
using PopMedNet.Dns.Data;
using Microsoft.EntityFrameworkCore;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Dns.DTO.Enums;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.OData.Query;
using PopMedNet.Utilities;
using System.Net;

namespace PopMedNet.Dns.Api.Events
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = PopMedNet.Utilities.WebSites.Security.AuthSchemeConstants.Scheme)]
    public class EventsController : ApiDataControllerBase<Event, EventDTO, DataContext, PermissionDefinition>
    {
        public EventsController(IConfiguration config, DataContext db, IMapper mapper)
            : base(db, mapper, config)
        {
        }

        /// <summary>
        /// Returns all of the event permissions for the specified Project and DataMart
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="dataMartID"></param>
        /// <returns></returns>
        [HttpGet("GetProjectDataMartEventPermissions"), EnableQuery]
        public IQueryable<ProjectDataMartEventDTO> GetProjectDataMartEventPermissions(Guid projectID, Guid? dataMartID = null)
        {
            var result = (from a in DataContext.Secure<ProjectDataMartEvent>(Identity) where a.ProjectID == projectID && (dataMartID == null || a.DataMartID == dataMartID.Value) && !a.DataMart.Deleted select a).ProjectTo<ProjectDataMartEventDTO>(_mapper.ConfigurationProvider);

            return result;
        }

        /// <summary>
        /// Returns all of the events for the specified project
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        [HttpGet("GetProjectEventPermissions"), EnableQuery]
        public IQueryable<ProjectEventDTO> GetProjectEventPermissions(Guid projectID)
        {
            var result = (from a in DataContext.Secure<ProjectEvent>(Identity) where a.ProjectID == projectID select a).ProjectTo<ProjectEventDTO>(_mapper.ConfigurationProvider);

            return result;
        }

        /// <summary>
        /// Returns all of the events for the specified project
        /// </summary>
        /// <param name="organizationID"></param>
        /// <returns></returns>
        [HttpGet("getorganizationeventpermissions"), EnableQuery]
        public IQueryable<OrganizationEventDTO> GetOrganizationEventPermissions(Guid organizationID)
        {
            var result = (from a in DataContext.Secure<OrganizationEvent>(Identity) where a.OrganizationID == organizationID select a).ProjectTo<OrganizationEventDTO>(_mapper.ConfigurationProvider);

            return result;
        }

        /// <summary>
        /// Returns all of the Organization events for the specified project
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="organizationID"></param>
        /// <returns></returns>
        [HttpGet("GetProjectOrganizationEventPermissions"), EnableQuery]
        public IQueryable<ProjectOrganizationEventDTO> GetProjectOrganizationEventPermissions(Guid projectID, Guid? organizationID = null)
        {
            var result = (from a in DataContext.Secure<ProjectOrganizationEvent>(Identity)
                          where a.ProjectID == projectID && (organizationID == null || a.OrganizationID == organizationID.Value)
                          select a).ProjectTo<ProjectOrganizationEventDTO>(_mapper.ConfigurationProvider);

            return result;
        }
        /// <summary>
        /// Updates ProjectOrganizationPermissions subject to security
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost("UpdateProjectOrganizationEventPermissions")]
        public async Task<IActionResult> UpdateProjectOrganizationEventPermissions(IEnumerable<ProjectOrganizationEventDTO> permissions)
        {
            //Non-optimal but no way else to do it in t-sql
            var projectIds = permissions.Select(p => p.ProjectID).ToArray();
            var organizationIDs = permissions.Select(p => p.OrganizationID).ToArray();

            //Has Permissions
            if (!await (from a in DataContext.ProjectAcls.FilterAcl(Identity, PermissionIdentifiers.Project.ManageSecurity) where projectIds.Contains(a.ProjectID) select a).AllAsync(a => a.Allowed))
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to manage one or more projects associated with the permissions passed.");

            var _dbAcls = await (from a in DataContext.ProjectOrganizationEvents where projectIds.Contains(a.ProjectID) && organizationIDs.Contains(a.OrganizationID) select a).ToArrayAsync();

            var acls = from a in _dbAcls join p in permissions on new { a.ProjectID, a.OrganizationID, a.EventID, a.SecurityGroupID } equals new { p.ProjectID, p.OrganizationID, p.EventID, p.SecurityGroupID } select new { dbAcl = a, permissions = p };

            foreach (var acl in acls)
            {
                if (acl.permissions.Allowed == null)
                {
                    DataContext.ProjectOrganizationEvents.Remove(acl.dbAcl);
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
                DataContext.ProjectOrganizationEvents.Add(new ProjectOrganizationEvent
                {
                    Allowed = acl.Allowed!.Value,
                    OrganizationID = acl.OrganizationID,
                    Overridden = true,
                    ProjectID = acl.ProjectID,
                    SecurityGroupID = acl.SecurityGroupID,
                    EventID = acl.EventID
                });
            }

            await DataContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status202Accepted);
        }

        [HttpGet("geteventsbylocation"), EnableQuery]
        public IQueryable<EventDTO> GetEventsByLocation([FromQuery]IEnumerable<PermissionAclTypes> locations)
        {
            var result = (from e in DataContext.Secure<Event>(Identity) where e.Locations.Any(a => locations.Contains(a.Location)) orderby e.Name select e).ProjectTo<EventDTO>(_mapper.ConfigurationProvider);
            return result;
        }

        /// <summary>
        /// Updates ProjectDataMartPermissions subject to security
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost("UpdateProjectDataMartEventPermissions")]
        public async Task<IActionResult> UpdateProjectDataMartEventPermissions(IEnumerable<ProjectDataMartEventDTO> permissions)
        {

            //Non-optimal but no way else to do it in t-sql
            var projectIds = permissions.Select(p => p.ProjectID).ToArray();
            var dataMartIds = permissions.Select(p => p.DataMartID).ToArray();

            //Has Permissions
            if (!await (from a in DataContext.ProjectAcls.FilterAcl(Identity, PermissionIdentifiers.Project.ManageSecurity) where projectIds.Contains(a.ProjectID) select a).AllAsync(a => a.Allowed))
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to manage one or more projects associated with the permissions passed.");

            var _dbAcls = await (from a in DataContext.ProjectDataMartEvents where projectIds.Contains(a.ProjectID) && dataMartIds.Contains(a.DataMartID) select a).ToArrayAsync();

            var acls = from a in _dbAcls join p in permissions on new { a.ProjectID, a.DataMartID, a.EventID, a.SecurityGroupID } equals new { p.ProjectID, p.DataMartID, p.EventID, p.SecurityGroupID } select new { dbAcl = a, permissions = p };

            foreach (var acl in acls)
            {
                if (acl.permissions.Allowed == null)
                {
                    DataContext.ProjectDataMartEvents.Remove(acl.dbAcl);
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
                DataContext.ProjectDataMartEvents.Add(new ProjectDataMartEvent
                {
                    Allowed = acl.Allowed!.Value,
                    DataMartID = acl.DataMartID,
                    Overridden = true,
                    ProjectID = acl.ProjectID,
                    SecurityGroupID = acl.SecurityGroupID,
                    EventID = acl.EventID
                });
            }

            await DataContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status202Accepted);
        }

        /// <summary>
        /// Updates Project Event Permissions subject to security
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost("UpdateProjectEventPermissions")]
        public async Task<IActionResult> UpdateProjectEventPermissions(IEnumerable<ProjectEventDTO> permissions)
        {

            //Non-optimal but no way else to do it in t-sql
            var projectIds = permissions.Select(p => p.ProjectID).ToArray();

            //Has Permissions
            if (!await (from a in DataContext.ProjectAcls.FilterAcl(Identity, PermissionIdentifiers.Project.ManageSecurity) where projectIds.Contains(a.ProjectID) select a).AllAsync(a => a.Allowed))
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to manage one or more projects associated with the permissions passed.");

            var _dbAcls = await (from a in DataContext.ProjectEvents where projectIds.Contains(a.ProjectID) select a).ToArrayAsync();

            var acls = from a in _dbAcls join p in permissions on new { a.ProjectID, a.EventID, a.SecurityGroupID } equals new { p.ProjectID, p.EventID, p.SecurityGroupID } select new { dbAcl = a, permissions = p };

            foreach (var acl in acls)
            {
                if (acl.permissions.Allowed == null)
                {
                    DataContext.ProjectEvents.Remove(acl.dbAcl);
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
                DataContext.ProjectEvents.Add(new ProjectEvent
                {
                    Allowed = acl.Allowed.Value,
                    Overridden = true,
                    ProjectID = acl.ProjectID,
                    SecurityGroupID = acl.SecurityGroupID,
                    EventID = acl.EventID
                });
            }

            await DataContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status202Accepted);
        }

        /// <summary>
        /// Updates Organization Event Permissions subject to security
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost("updateorganizationeventpermissions")]
        public async Task<IActionResult> UpdateOrganizationEventPermissions(IEnumerable<OrganizationEventDTO> permissions)
        {
            //Non-optimal but no way else to do it in t-sql
            var organizationIds = permissions.Select(p => p.OrganizationID).ToArray();

            //Has Permissions
            if (!await (from a in DataContext.OrganizationAcls.FilterAcl(Identity, PermissionIdentifiers.Organization.ManageSecurity) where organizationIds.Contains(a.OrganizationID) select a).AllAsync(a => a.Allowed))
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to manage one or more organization associated with the permissions passed.");

            var _dbAcls = await (from a in DataContext.OrganizationEvents where organizationIds.Contains(a.OrganizationID) select a).ToArrayAsync();

            var acls = from a in _dbAcls join p in permissions on new { a.OrganizationID, a.EventID, a.SecurityGroupID } equals new { p.OrganizationID, p.EventID, p.SecurityGroupID } select new { dbAcl = a, permissions = p };

            foreach (var acl in acls)
            {
                if (acl.permissions.Allowed == null)
                {
                    DataContext.OrganizationEvents.Remove(acl.dbAcl);
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
                DataContext.OrganizationEvents.Add(new OrganizationEvent
                {
                    Allowed = acl.Allowed!.Value,
                    Overridden = true,
                    OrganizationID = acl.OrganizationID,
                    SecurityGroupID = acl.SecurityGroupID,
                    EventID = acl.EventID
                });
            }

            await DataContext.SaveChangesAsync();

            return Accepted();
        }

        /// <summary>
        /// Returns all of the events for the specified group
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        [HttpGet("getgroupeventpermissions")]
        public IQueryable<GroupEventDTO> GetGroupEventPermissions(Guid groupID)
        {
            var result = (from a in DataContext.Secure<GroupEvent>(Identity) where a.GroupID == groupID select a).ProjectTo<GroupEventDTO>(_mapper.ConfigurationProvider);

            return result;
        }

        /// <summary>
        /// Updates Group Event Permissions subject to security
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost("updategroupeventpermissions")]
        public async Task<IActionResult> UpdateGroupEventPermissions(IEnumerable<GroupEventDTO> permissions)
        {

            //Non-optimal but no way else to do it in t-sql
            var groupIds = permissions.Select(g => g.GroupID).ToArray();

            //Has Permissions
            if (!await (from a in DataContext.GroupAcls.FilterAcl(Identity, PermissionIdentifiers.Group.ManageSecurity) where groupIds.Contains(a.GroupID) select a).AllAsync(a => a.Allowed))
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to manage one or more groups associated with the permissions passed.");

            var _dbAcls = await (from a in DataContext.GroupEvents where groupIds.Contains(a.GroupID) select a).ToArrayAsync();

            var acls = from a in _dbAcls join p in permissions on new { a.GroupID, a.EventID, a.SecurityGroupID } equals new { p.GroupID, p.EventID, p.SecurityGroupID } select new { dbAcl = a, permissions = p };

            foreach (var acl in acls)
            {
                if (acl.permissions.Allowed == null)
                {
                    DataContext.GroupEvents.Remove(acl.dbAcl);
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
                DataContext.GroupEvents.Add(new GroupEvent
                {
                    Allowed = acl.Allowed!.Value,
                    Overridden = true,
                    GroupID = acl.GroupID,
                    SecurityGroupID = acl.SecurityGroupID,
                    EventID = acl.EventID
                });
            }

            await DataContext.SaveChangesAsync();

            return Accepted();
        }

        /// <summary>
        /// Gets all acls for global events
        /// </summary>
        /// <returns></returns>
        [HttpGet("getglobaleventpermissions"), EnableQuery]
        public IQueryable<BaseEventPermissionDTO> GetGlobalEventPermissions()
        {
            var result = (from a in DataContext.Secure<GlobalEvent>(Identity) select a).ProjectTo<BaseEventPermissionDTO>(_mapper.ConfigurationProvider);

            return result;
        }

        [HttpPost("updateglobaleventpermissions")]
        public async Task<IActionResult> UpdateGlobalEventPermissions(IEnumerable<BaseEventPermissionDTO> permissions)
        {

            //Non-optimal but no way else to do it in t-sql
            var securityGroupIds = permissions.Select(p => p.SecurityGroupID).ToArray();
            var eventIds = permissions.Select(p => p.EventID).ToArray();

            //Has Permissions

            if (!await DataContext.HasPermission(Identity, PermissionIdentifiers.Portal.ManageSecurity))
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to manage the global security settings for this site.");

            var _dbAcls = await (from a in DataContext.GlobalEvents where securityGroupIds.Contains(a.SecurityGroupID) && eventIds.Contains(a.EventID) select a).ToArrayAsync();

            var acls = from a in _dbAcls join p in permissions on new { a.EventID, a.SecurityGroupID } equals new { p.EventID, p.SecurityGroupID } select new { dbAcl = a, permissions = p };

            foreach (var acl in acls)
            {
                if (acl.permissions.Allowed == null)
                {
                    DataContext.GlobalEvents.Remove(acl.dbAcl);
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
                DataContext.GlobalEvents.Add(new GlobalEvent
                {
                    Allowed = acl.Allowed!.Value,
                    Overridden = true,
                    SecurityGroupID = acl.SecurityGroupID,
                    EventID = acl.EventID
                });
            }

            await DataContext.SaveChangesAsync();

            return Accepted();
        }

        /// <summary>
        /// Returns all of the events for the specified user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet("getusereventpermissions"), EnableQuery]
        public IQueryable<UserEventDTO> GetUserEventPermissions(Guid userID)
        {
            var result = (from a in DataContext.Secure<UserEvent>(Identity) where a.UserID == userID select a).ProjectTo<UserEventDTO>(_mapper.ConfigurationProvider);

            return result;
        }

        /// <summary>
        /// Updates or adds all of the user event permissions passed.
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost("updateusereventpermissions")]
        public async Task<IActionResult> UpdateUserEventPermissions([FromBody]IEnumerable<UserEventDTO> permissions)
        {

            //Non-optimal but no way else to do it in t-sql
            var UserIDs = permissions.Select(p => p.UserID).ToArray();

            //Has Permissions
            if (!await (from a in DataContext.UserAcls.FilterAcl(Identity, PermissionIdentifiers.User.ManageSecurity) where UserIDs.Contains(a.UserID) select a).AllAsync(a => a.Allowed))
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to manage one or more users associated with the permissions passed.");

            var _dbAcls = await (from a in DataContext.UserEvents where UserIDs.Contains(a.UserID) select a).ToArrayAsync();

            var acls = from a in _dbAcls join p in permissions on new { a.UserID, a.EventID, a.SecurityGroupID } equals new { p.UserID, p.EventID, p.SecurityGroupID } select new { dbAcl = a, permissions = p };

            foreach (var acl in acls)
            {
                if (acl.permissions.Allowed == null)
                {
                    DataContext.UserEvents.Remove(acl.dbAcl);
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
                DataContext.UserEvents.Add(new UserEvent
                {
                    Allowed = acl.Allowed!.Value,
                    Overridden = true,
                    UserID = acl.UserID,
                    SecurityGroupID = acl.SecurityGroupID,
                    EventID = acl.EventID
                });
            }

            await DataContext.SaveChangesAsync();

            return Accepted();
        }
    }
}
