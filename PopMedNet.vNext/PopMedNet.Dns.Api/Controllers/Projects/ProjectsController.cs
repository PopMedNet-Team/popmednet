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
using Microsoft.Data.SqlClient;
using PopMedNet.Utilities;

namespace PopMedNet.Dns.Api.Projects
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = PopMedNet.Utilities.WebSites.Security.AuthSchemeConstants.Scheme)]
    public class ProjectsController : ApiDataControllerBase<Project, ProjectDTO, DataContext, PermissionDefinition>
    {

        public ProjectsController(IConfiguration config, DataContext db, IMapper mapper)
            : base(db, mapper, config)
        {
        }

        /// <summary>
        /// Returns a list of projects the user has access to that are filterable using OData
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public override IActionResult List(ODataQueryOptions<ProjectDTO> options)
        {
            IQueryable<ProjectDTO> q = (from proj in DataContext.Projects
                                        join grp in DataContext.Groups on proj.GroupID equals grp.ID
                                        let pViewPermissionID = PermissionIdentifiers.Project.View.ID
                                        let pListPermissionID = PermissionIdentifiers.Group.ListProjects.ID
                                        let identityID = Identity.ID
                                        let gAcls = DataContext.GlobalAcls.Where(acl => acl.SecurityGroup.Users.Where(x => x.UserID == identityID).Any() && (acl.PermissionID == pListPermissionID || acl.PermissionID == pViewPermissionID)).AsEnumerable()
                                        let pAcls = DataContext.ProjectAcls.Where(acl => acl.SecurityGroup.Users.Where(x => x.UserID == identityID).Any() && acl.PermissionID == pViewPermissionID && proj.ID == acl.ProjectID).AsEnumerable()
                                        let groupAcls = DataContext.GroupAcls.Where(acl => acl.SecurityGroup.Users.Where(x => x.UserID == identityID).Any() && acl.PermissionID == pListPermissionID && grp.ID == acl.GroupID).AsEnumerable()
                                        where !proj.Deleted &&
                                        (
                                            (gAcls.Any(a => a.Allowed) || pAcls.Any(a => a.Allowed) || groupAcls.Any(a => a.Allowed))
                                            &&
                                            (gAcls.All(a => a.Allowed) && pAcls.All(a => a.Allowed) && groupAcls.All(a => a.Allowed))
                                        )
                                        select new ProjectDTO
                                        {
                                            Acronym = proj.Acronym,
                                            Active = proj.Active,
                                            Deleted = proj.Deleted,
                                            Description = proj.Description,
                                            EndDate = proj.EndDate,
                                            ID = proj.ID,
                                            Name = proj.Name,
                                            StartDate = proj.StartDate,
                                            Timestamp = proj.Timestamp,
                                            GroupID = proj.GroupID,
                                            Group = grp.Name
                                        });

            var queryHelper = new Utilities.WebSites.ODataQueryHandler<ProjectDTO>(q, options);
            return Ok(queryHelper.Result());
        }

        /// <summary>
        /// Gets the available request types for the project that the user has permission to manage.
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        [HttpGet("GetProjectRequestTypes"), EnableQuery]
        public async Task<IQueryable<ProjectRequestTypeDTO>> GetProjectRequestTypes(Guid projectID)
        {
            if (!await DataContext.HasPermissions<Project>(Identity, projectID, PermissionIdentifiers.Project.ManageRequestTypes))
                throw new HttpResponseException((int)System.Net.HttpStatusCode.Forbidden, "You do not have permission to manage request types.");


            var result = from prt in DataContext.ProjectRequestTypes
                         join p in DataContext.Secure<Project>(Identity, PermissionIdentifiers.Project.ManageRequestTypes) on prt.ProjectID equals p.ID
                         where p.ID == projectID
                         select prt;

            return result.ProjectTo<ProjectRequestTypeDTO>(_mapper.ConfigurationProvider);
        }

        /// <summary>
        /// Returns the projects that the current user can create a request against
        /// </summary>
        /// <returns></returns>
        [HttpGet("requestableprojects")]
        public IActionResult RequestableProjects(ODataQueryOptions<ProjectDTO> options)
        {
            var results = (from p in DataContext.Secure<Project>(Identity, PermissionIdentifiers.Request.Edit)
                          where
                              p.Active && !p.Deleted && (!p.EndDate.HasValue || p.EndDate.Value > DateTime.UtcNow) && (p.StartDate <= DateTime.UtcNow)
                          select p).ProjectTo<ProjectDTO>(_mapper.ConfigurationProvider);

            var queryHelper = new Utilities.WebSites.ODataQueryHandler<ProjectDTO>(results, options);
            return Ok(queryHelper.Result());
        }

        /// <summary>
        /// Returns request types associated with project that the user has permission to manage.
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        [HttpGet("GetRequestTypes"), EnableQuery]
        public async Task<IQueryable<RequestTypeDTO>> GetRequestTypes(Guid projectID)
        {
            if (!await DataContext.HasPermissions<Project>(Identity, projectID, PermissionIdentifiers.Project.ManageRequestTypes))
                throw new HttpResponseException((int)System.Net.HttpStatusCode.Forbidden, "You do not have permission to manage request types.");


            var result = from prt in DataContext.ProjectRequestTypes
                         join p in DataContext.Secure<Project>(Identity, PermissionIdentifiers.Project.ManageRequestTypes) on prt.ProjectID equals p.ID
                         where p.ID == projectID
                         select prt.RequestType;

            return result.ProjectTo<RequestTypeDTO>(_mapper.ConfigurationProvider);
        }

        /// <summary>
        /// Gets all the request types that are available for the given project, regardless of supported datamodel.
        /// </summary>
        /// <param name="projectID">The project the request type is associated with.</param>
        /// <returns></returns>
        [HttpGet("getavailablerequesttypefornewrequest")]
        public async Task<IActionResult> GetAvailableRequestTypeForNewRequest(Guid projectID)
        {
            if (!await DataContext.HasPermissions<Project>(Identity, projectID, PermissionIdentifiers.Request.Edit))
                return Ok(new Utilities.WebSites.Models.ODataQueryResult(Enumerable.Empty<RequestTypeDTO>().AsQueryable()));

            return Ok(new Utilities.WebSites.Models.ODataQueryResult(DataContext.GetProjectAvailableRequestTypes(projectID, Identity.ID).ProjectTo<RequestTypeDTO>(_mapper.ConfigurationProvider)));
        }

        /// <summary>
        /// Updates the available request types for the project
        /// </summary>
        /// <param name="requestTypes"></param>
        /// <returns></returns>
        [HttpPost("UpdateProjectRequestTypes")]
        public async Task<IActionResult> UpdateProjectRequestTypes(UpdateProjectRequestTypesDTO requestTypes)
        {
            if (requestTypes == null)
                return StatusCode(StatusCodes.Status202Accepted);

            //var projectAclFilter = DataContext.ProjectAcls.FilterAcl(Identity, PermissionIdentifiers.Project.ManageRequestTypes);

            //var globalAclFilter = DataContext.GlobalAcls.FilterAcl(Identity, PermissionIdentifiers.Project.ManageRequestTypes);

            var hasPermission = await (from p in DataContext.Secure<Project>(Identity)
                                       let identityID = Identity.ID
                                       let manageRequestTypesPermissionID = PermissionIdentifiers.Project.ManageRequestTypes.ID
                                       let pacl = DataContext.ProjectAcls.Where(a => a.ProjectID == p.ID && a.SecurityGroup!.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == manageRequestTypesPermissionID).ToArray()
                                       let gacl = DataContext.GlobalAcls.Where(a => a.PermissionID == manageRequestTypesPermissionID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID)).ToArray()
                                       where p.ID == requestTypes.ProjectID
                                       && (pacl.Any(a => a.Allowed) || gacl.Any(a => a.Allowed)) && (pacl.All(a => a.Allowed) && gacl.All(a => a.Allowed))
                                       select p).AnyAsync();

            if (!hasPermission)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to manage one or more Request Types associated with the permissions passed.");
            }

            var _dbRequestTypes = await (from prt in DataContext.ProjectRequestTypes.Include(i => i.RequestType) where prt.ProjectID == requestTypes.ProjectID select prt).ToArrayAsync();

            var requestTypeIDs = requestTypes.RequestTypes.Select(rt => rt.RequestTypeID).Distinct().ToArray();
            //Remove including security permissions
            var removeRequestTypes = _dbRequestTypes.Where(rt => !requestTypeIDs.Contains(rt.RequestTypeID)).ToArray();
            var removeRequestTypeIDs = removeRequestTypes.Select(rt => rt.RequestTypeID).ToArray();

            var aclProjectRequestTypes = await (from a in DataContext.ProjectRequestTypeAcls where a.ProjectID == requestTypes.ProjectID && removeRequestTypeIDs.Contains(a.RequestTypeID) select a).ToArrayAsync();
            DataContext.ProjectRequestTypeAcls.RemoveRange(aclProjectRequestTypes);

            var aclProjectDataMartRequestTypes = await (from a in DataContext.ProjectDataMartRequestTypeAcls where a.ProjectID == requestTypes.ProjectID && removeRequestTypeIDs.Contains(a.RequestTypeID) select a).ToArrayAsync();
            DataContext.ProjectDataMartRequestTypeAcls.RemoveRange(aclProjectDataMartRequestTypes);

            DataContext.ProjectRequestTypes.RemoveRange(removeRequestTypes);

            //Now add all of the ones that aren't in there.
            foreach (var prt in requestTypes.RequestTypes.Where(rt => !_dbRequestTypes.Any(prt => prt.RequestTypeID == rt.RequestTypeID)))
            {

                DataContext.ProjectRequestTypes.Add(new ProjectRequestType
                {
                    ProjectID = prt.ProjectID,
                    RequestTypeID = prt.RequestTypeID
                });
            }

            await DataContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status202Accepted);
        }

        [HttpPost("InsertOrUpdate")]
        public override async Task<IActionResult> InsertOrUpdate(IEnumerable<ProjectDTO> values)
        {
            await CheckForDuplicates(values);
            return await base.InsertOrUpdate(values);
        }

        async Task CheckForDuplicates(IEnumerable<ProjectDTO> updates)
        {
            var ids = updates.Where(u => u.ID.HasValue).Select(u => u.ID!.Value).ToArray();
            var names = updates.Select(u => u.Name).ToArray();
            var acronyms = updates.Where(u => u.Acronym != null && u.Acronym != "").Select(u => u.Acronym).ToArray();

            if (updates.GroupBy(u => u.Acronym).Any(u => u.Count() > 1))
                throw new HttpResponseException(StatusCodes.Status400BadRequest, "The Acronym of Projects must be unique.");

            if (updates.GroupBy(u => u.Name).Any(u => u.Count() > 1))
                throw new HttpResponseException(StatusCodes.Status400BadRequest, "The Name of Projects must be unique.");

            if (await (from p in DataContext.Projects where !p.Deleted && !ids.Contains(p.ID) && (names.Contains(p.Name) || acronyms.Contains(p.Acronym)) select p).AnyAsync())
                throw new HttpResponseException(StatusCodes.Status400BadRequest, "The Name and Acronym of Projects must be unique.");
        }

        /// <summary>
        /// Copies the specified project and returns the ID of the new project.
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        [HttpGet("Copy")]
        public async Task<Guid> Copy(Guid projectID)
        {
            var existing = await (from p in DataContext.Projects.Include(x => x.DataMarts) where p.ID == projectID select p).SingleOrDefaultAsync();
            
            if (existing == null)
                throw new HttpResponseException(StatusCodes.Status404NotFound, "The Project could not be found.");

            if (!await DataContext.HasPermissions<Project>(Identity, existing.ID, PermissionIdentifiers.Project.Copy))
                throw new HttpResponseException(StatusCodes.Status403Forbidden, "You do not have permission to copy the specified project.");

            string newAcronym = "New " + existing.Acronym;
            string newName = "New " + existing.Name;

            while (await (from p in DataContext.Projects where !p.Deleted && (p.Name == newName && p.Acronym == newAcronym) select p).AnyAsync())
            {
                newAcronym = "New " + newAcronym;
                newName = "New " + newName;
            }

            //TODO: why was group ID null here explicitly? without a group the project will not be loaded or listed
            var project = new Project
            {
                Acronym = newAcronym,
                StartDate = DateTime.Today,
                Name = newName,
                GroupID = existing.GroupID,
                Description = existing.Description,
                Active = true
            };

            DataContext.Projects.Add(project);

            var existingSecurityGroups = await (from sg in DataContext.SecurityGroups.Include(x => x.Users) where sg.OwnerID == existing.ID orderby sg.ParentSecurityGroupID select sg).ToArrayAsync();
            var SecurityGroupMap = new Dictionary<Guid, Guid>();

            CopySecurityGroups(existingSecurityGroups, ref SecurityGroupMap, null, project);

            await DataContext.SaveChangesAsync();

            //All of these are done this way with a conditional if because the triggers cause inserts that entity framework is not aware of. Note that they are parameterized to ensure no sql injections.

            foreach (var user in existingSecurityGroups.SelectMany(u => u.Users).DistinctBy(u => new { u.SecurityGroupID, u.UserID }))
            {
                await DataContext.Database.ExecuteSqlRawAsync(@"IF NOT EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE UserID = @UserID AND SecurityGroupID = @SecurityGroupID)
	INSERT INTO SecurityGroupUsers (UserID, SecurityGroupID, Overridden) VALUES (@UserID, @SecurityGroupID, 0)", new SqlParameter("UserID", user.UserID), new SqlParameter("SecurityGroupID", SecurityGroupMap[user.SecurityGroupID]));

            }

            //Data Marts
            foreach (var existingDataMart in existing.DataMarts)
            {
                var dm = new ProjectDataMart
                {
                    DataMartID = existingDataMart.DataMartID,
                    ProjectID = project.ID
                };

                DataContext.ProjectDataMarts.Add(dm);
            }

            //RequestTypes
            var projRequestTypes = await (from rt in DataContext.ProjectRequestTypes where rt.ProjectID == existing.ID select rt).ToArrayAsync();

            foreach (var exisitingRequestType in projRequestTypes)
            {
                await DataContext.Database.ExecuteSqlRawAsync(@"IF NOT EXISTS(SELECT NULL FROM ProjectRequestTypes WHERE ProjectID = @ProjectID AND RequestTypeID = @RequestTypeID)
	INSERT INTO ProjectRequestTypes (ProjectID, RequestTypeID) VALUES (@ProjectID, @RequestTypeID)", new SqlParameter("ProjectID", project.ID), new SqlParameter("RequestTypeID", exisitingRequestType.RequestTypeID));
            }

            var existingSecurityGroupIDs = SecurityGroupMap.Select(gm => gm.Key).ToArray();

            //Project Acls
            var existingProjectAcls = await (from a in DataContext.ProjectAcls where a.ProjectID == existing.ID select a).ToArrayAsync();
            foreach (var existingProjectAcl in existingProjectAcls)
            {
                if (!SecurityGroupMap.ContainsKey(existingProjectAcl.SecurityGroupID))
                    SecurityGroupMap.Add(existingProjectAcl.SecurityGroupID, existingProjectAcl.SecurityGroupID);

                var count = await DataContext.Database.ExecuteSqlRawAsync(@"IF NOT EXISTS(SELECT NULL FROM AclProjects WHERE ProjectID = @ProjectID AND SecurityGroupID = @SecurityGroupID AND PermissionID = @PermissionID)
	INSERT INTO AclProjects (ProjectID, SecurityGroupID, PermissionID, Allowed, Overridden) VALUES (@ProjectID, @SecurityGroupID, @PermissionID, @Allowed, 1)", new SqlParameter("ProjectID", project.ID), new SqlParameter("SecurityGroupID", SecurityGroupMap[existingProjectAcl.SecurityGroupID]), new SqlParameter("PermissionID", existingProjectAcl.PermissionID), new SqlParameter("Allowed", existingProjectAcl.Allowed));

            }



            //Project Event Acls
            var existingProjectEventAcls = await (from a in DataContext.ProjectEvents where a.ProjectID == existing.ID select a).ToArrayAsync();
            foreach (var existingProjectEventAcl in existingProjectEventAcls)
            {
                if (!SecurityGroupMap.ContainsKey(existingProjectEventAcl.SecurityGroupID))
                    SecurityGroupMap.Add(existingProjectEventAcl.SecurityGroupID, existingProjectEventAcl.SecurityGroupID);

                await DataContext.Database.ExecuteSqlRawAsync(@"IF NOT EXISTS(SELECT NULL FROM ProjectEvents WHERE ProjectID = @ProjectID AND SecurityGroupID = @SecurityGroupID AND EventID = @EventID)
	INSERT INTO ProjectEvents (ProjectID, SecurityGroupID, EventID, Allowed, Overridden) VALUES (@ProjectID, @SecurityGroupID, @EventID, @Allowed, 0)", new SqlParameter("ProjectID", project.ID), new SqlParameter("SecurityGroupID", SecurityGroupMap[existingProjectEventAcl.SecurityGroupID]), new SqlParameter("EventID", existingProjectEventAcl.EventID), new SqlParameter("Allowed", existingProjectEventAcl.Allowed));
            }

            //Project AclProjectRequestTypes Acls
            var existingProjectRequestTypesAcls = await (from a in DataContext.ProjectRequestTypeAcls where a.ProjectID == existing.ID select a).ToArrayAsync();
            foreach (var existingProjectRequestTypesAcl in existingProjectRequestTypesAcls)
            {
                if (!SecurityGroupMap.ContainsKey(existingProjectRequestTypesAcl.SecurityGroupID))
                    SecurityGroupMap.Add(existingProjectRequestTypesAcl.SecurityGroupID, existingProjectRequestTypesAcl.SecurityGroupID);

                var count = await DataContext.Database.ExecuteSqlRawAsync(@"IF NOT EXISTS(SELECT NULL FROM AclProjectRequestTypes WHERE ProjectID = @ProjectID AND SecurityGroupID = @SecurityGroupID AND Permission = @Permission AND RequestTypeID = @RequestTypeID)
	INSERT INTO AclProjectRequestTypes (ProjectID, SecurityGroupID, Permission, RequestTypeID, Overridden) VALUES (@ProjectID, @SecurityGroupID, @Permission, @RequestTypeID, 1)", new SqlParameter("ProjectID", project.ID), new SqlParameter("SecurityGroupID", SecurityGroupMap[existingProjectRequestTypesAcl.SecurityGroupID]), new SqlParameter("Permission", existingProjectRequestTypesAcl.Permission), new SqlParameter("RequestTypeID", existingProjectRequestTypesAcl.RequestTypeID));

            }

            //Project ProjectRequestTypeWorkflowActivities Acls
            var existingProjectRequestTypeWorkflowActivitiesAcls = await (from a in DataContext.ProjectRequestTypeWorkflowActivities where a.ProjectID == existing.ID select a).ToArrayAsync();
            foreach (var existingProjectRequestTypeWorkflowActivitiesAcl in existingProjectRequestTypeWorkflowActivitiesAcls)
            {
                if (!SecurityGroupMap.ContainsKey(existingProjectRequestTypeWorkflowActivitiesAcl.SecurityGroupID))
                    SecurityGroupMap.Add(existingProjectRequestTypeWorkflowActivitiesAcl.SecurityGroupID, existingProjectRequestTypeWorkflowActivitiesAcl.SecurityGroupID);

                var count = await DataContext.Database.ExecuteSqlRawAsync(@"IF NOT EXISTS(SELECT NULL FROM AclProjectRequestTypeWorkflowActivities WHERE ProjectID = @ProjectID AND SecurityGroupID = @SecurityGroupID AND PermissionID = @PermissionID AND RequestTypeID = @RequestTypeID AND WorkflowActivityID = @WorkflowActivityID)
	INSERT INTO AclProjectRequestTypeWorkflowActivities (ProjectID, SecurityGroupID, PermissionID, RequestTypeID, WorkflowActivityID, Allowed, Overridden) VALUES (@ProjectID, @SecurityGroupID, @PermissionID, @RequestTypeID, @WorkflowActivityID, @Allowed, 1)", new SqlParameter("ProjectID", project.ID), new SqlParameter("SecurityGroupID", SecurityGroupMap[existingProjectRequestTypeWorkflowActivitiesAcl.SecurityGroupID]), new SqlParameter("PermissionID", existingProjectRequestTypeWorkflowActivitiesAcl.PermissionID), new SqlParameter("RequestTypeID", existingProjectRequestTypeWorkflowActivitiesAcl.RequestTypeID), new SqlParameter("WorkflowActivityID", existingProjectRequestTypeWorkflowActivitiesAcl.WorkflowActivityID), new SqlParameter("Allowed", existingProjectRequestTypeWorkflowActivitiesAcl.Allowed));

            }


            //Project DataMart Acls
            var existingProjectDataMartAcls = await (from a in DataContext.ProjectDataMartAcls where a.ProjectID == existing.ID select a).ToArrayAsync();
            foreach (var existingProjectDataMartAcl in existingProjectDataMartAcls)
            {
                if (!SecurityGroupMap.ContainsKey(existingProjectDataMartAcl.SecurityGroupID))
                    SecurityGroupMap.Add(existingProjectDataMartAcl.SecurityGroupID, existingProjectDataMartAcl.SecurityGroupID);

                var count = await DataContext.Database.ExecuteSqlRawAsync(@"IF NOT EXISTS(SELECT NULL FROM AclProjectDataMarts WHERE ProjectID = @ProjectID AND SecurityGroupID = @SecurityGroupID AND PermissionID = @PermissionID AND DataMartID = @DataMartID)
	INSERT INTO AclProjectDataMarts (ProjectID, SecurityGroupID, PermissionID, DataMartID, Allowed, Overridden) VALUES (@ProjectID, @SecurityGroupID, @PermissionID, @DataMartID, @Allowed, 1)", new SqlParameter("ProjectID", project.ID), new SqlParameter("SecurityGroupID", SecurityGroupMap[existingProjectDataMartAcl.SecurityGroupID]), new SqlParameter("PermissionID", existingProjectDataMartAcl.PermissionID), new SqlParameter("DataMartID", existingProjectDataMartAcl.DataMartID), new SqlParameter("Allowed", existingProjectDataMartAcl.Allowed));

            }


            //Project DataMart Request Type Acls

            var existingProjectDataMartRequestTypeAcls = await (from a in DataContext.ProjectDataMartRequestTypeAcls where a.ProjectID == existing.ID select a).ToArrayAsync();
            foreach (var existingProjectDataMartRequestTypeAcl in existingProjectDataMartRequestTypeAcls)
            {
                if (!SecurityGroupMap.ContainsKey(existingProjectDataMartRequestTypeAcl.SecurityGroupID))
                    SecurityGroupMap.Add(existingProjectDataMartRequestTypeAcl.SecurityGroupID, existingProjectDataMartRequestTypeAcl.SecurityGroupID);

                var count = await DataContext.Database.ExecuteSqlRawAsync(@"IF NOT EXISTS(SELECT NULL FROM AclProjectDataMartRequestTypes WHERE ProjectID = @ProjectID AND SecurityGroupID = @SecurityGroupID AND Permission = @Permission AND RequestTypeID = @RequestTypeID AND DataMartID = @DataMartID)
	INSERT INTO AclProjectDataMartRequestTypes (ProjectID, SecurityGroupID, Permission, RequestTypeID, DataMartID, Overridden) VALUES (@ProjectID, @SecurityGroupID, @Permission, @RequestTypeID, @DataMartID, 1)", new SqlParameter("ProjectID", project.ID), new SqlParameter("SecurityGroupID", SecurityGroupMap[existingProjectDataMartRequestTypeAcl.SecurityGroupID]), new SqlParameter("Permission", existingProjectDataMartRequestTypeAcl.Permission), new SqlParameter("RequestTypeID", existingProjectDataMartRequestTypeAcl.RequestTypeID), new SqlParameter("DataMartID", existingProjectDataMartRequestTypeAcl.DataMartID));

            }

            await DataContext.SaveChangesAsync();

            return project.ID;
        }

        private void CopySecurityGroups(SecurityGroup[] existingSecurityGroups, ref Dictionary<Guid, Guid> securityGroupMap, Guid? parentSecurityGroupID, Project project)
        {
            foreach (var existingSecurityGroup in existingSecurityGroups.Where(sg => (sg.ParentSecurityGroupID == null && parentSecurityGroupID == null) || (parentSecurityGroupID == null && sg.ParentSecurityGroupID.HasValue && !existingSecurityGroups.Any(esg => esg.ID == sg.ParentSecurityGroupID.Value)) || sg.ParentSecurityGroupID == parentSecurityGroupID))
            {

                //If there is a parent, and the parent isn't a group from the organization itself and it isn't in the map yet, then it's external and not changing, so the map is the same.
                if (existingSecurityGroup.ParentSecurityGroupID.HasValue && !securityGroupMap.ContainsKey(existingSecurityGroup.ParentSecurityGroupID.Value) && !existingSecurityGroups.Any(group => group.ID == existingSecurityGroup.ParentSecurityGroupID.Value))
                    securityGroupMap.Add(existingSecurityGroup.ParentSecurityGroupID.Value, existingSecurityGroup.ParentSecurityGroupID.Value);

                var sg = new SecurityGroup
                {
                    Kind = existingSecurityGroup.Kind,
                    Name = "New " + existingSecurityGroup.Name,
                    OwnerID = project.ID,
                    Owner = project.Acronym,
                    Type = DTO.Enums.SecurityGroupTypes.Organization,
                    Path = project.Acronym + @"\" + existingSecurityGroup.Name,
                    ParentSecurityGroupID = existingSecurityGroup.ParentSecurityGroupID.HasValue && existingSecurityGroup.ParentSecurityGroupID.Value != existingSecurityGroup.ID ? securityGroupMap[existingSecurityGroup.ParentSecurityGroupID.Value] : (Guid?)null
                };
                DataContext.SecurityGroups.Add(sg);

                securityGroupMap.Add(existingSecurityGroup.ID, sg.ID);

                CopySecurityGroups(existingSecurityGroups, ref securityGroupMap, existingSecurityGroup.ID, project);
            }
        }

        /// <summary>
        /// Flags the project as deleted.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery]IEnumerable<Guid> ID)
        {
            if (!await DataContext.CanDelete<Project>(Identity, ID.ToArray()))
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to delete this project.");

            var projects = await (from p in DataContext.Projects where ID.Contains(p.ID) select p).ToArrayAsync();
            foreach (var project in projects)
                project.Deleted = true;

            await DataContext.SaveChangesAsync();

            return Ok();
        }


    }
}
