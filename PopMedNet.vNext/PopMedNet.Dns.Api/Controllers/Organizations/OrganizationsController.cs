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
using System.Net;
using Microsoft.Data.SqlClient;

namespace PopMedNet.Dns.Api.Organizations
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = PopMedNet.Utilities.WebSites.Security.AuthSchemeConstants.Scheme)]
    public class OrganizationsController : ApiDataControllerBase<Organization, OrganizationDTO, DataContext, PermissionDefinition>
    {
        public OrganizationsController(IConfiguration config, DataContext db, IMapper mapper)
            : base(db, mapper, config)
        {
        }

        /// <summary>
        /// Returns a list of organizations the user has access to that are filterable using OData
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public override IActionResult List(ODataQueryOptions<OrganizationDTO> options)
        {
            IQueryable<OrganizationDTO> q = (from u in DataContext.Secure<Organization>(Identity) where u.Deleted == false select u).ProjectTo<OrganizationDTO>(_mapper.ConfigurationProvider);
            var queryHelper = new Utilities.WebSites.ODataQueryHandler<OrganizationDTO>(q, options);
            return Ok(queryHelper.Result());
        }

        /// <summary>
        /// Returns a secure list of organizations by Group
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        [HttpGet("listbygroupmembership"), EnableQuery]
        public IQueryable<OrganizationDTO> ListByGroupMembership(Guid groupID)
        {
            var result = (from o in DataContext.Secure<Organization>(Identity) where o.Groups.Any(g => g.GroupID == groupID) select o).ProjectTo<OrganizationDTO>(_mapper.ConfigurationProvider);
            return result;
        }

        /// <summary>
        /// List Orgs in EHRS System.
        /// </summary>
        /// <returns></returns>
        [HttpGet("listehrs"), EnableQuery]
        public IQueryable<OrganizationEHRSDTO> ListEHRS()
        {
            return (from eh in DataContext.OrganizationEHRSes
                    select new OrganizationEHRSDTO
                    {
                        ID = eh.ID,
                        OrganizationID = eh.OrganizationID,
                        Type = eh.Type,
                        System = eh.System,
                        Other = eh.Other,
                        StartYear = eh.StartYear,
                        EndYear = eh.EndYear
                    }).AsQueryable();
        }

        /// <summary>
        /// Inserts or updates a list of organizations associated with a EHR System.
        /// </summary>
        /// <param name="updates"></param>
        /// <returns></returns>
        [HttpPost("EHRSInsertOrUpdate")]
        public async Task<IActionResult> EHRSInsertOrUpdate(OrganizationUpdateEHRsesDTO updates)
        {
            if (updates.EHRS.Any(e => e.OrganizationID != updates.OrganizationID))
                return StatusCode(StatusCodes.Status403Forbidden, "You may not update ehrses that are not from the specified organization.");

            if (!await DataContext.HasPermissions<Organization>(Identity, updates.OrganizationID, PermissionIdentifiers.Organization.Edit))
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to alter this Organization referenced");

            List<OrganizationEHRS> modified = new List<OrganizationEHRS>();

            var existingOrgEHRS = await (from orgEHRS in DataContext.OrganizationEHRSes where orgEHRS.OrganizationID == updates.OrganizationID select orgEHRS).ToArrayAsync();

            // Newly added.
            foreach (var ehrs in updates.EHRS.Where(o => !existingOrgEHRS.Any(eh => eh.ID == o.ID)))
            {

                var newEHRS = DataContext.OrganizationEHRSes.Add(new OrganizationEHRS
                {
                    Type = ehrs.Type,
                    System = ehrs.System,
                    Other = ehrs.Other,
                    StartYear = ehrs.StartYear,
                    EndYear = ehrs.EndYear,
                    OrganizationID = ehrs.OrganizationID
                });

                modified.Add(newEHRS.Entity);
            }

            // Existing.
            foreach (var ehrs in existingOrgEHRS)
            {
                var orgEHRS = updates.EHRS.Where(o => o.ID == ehrs.ID).Select(o => o).FirstOrDefault();
                if (orgEHRS == null)
                    continue;

                ehrs.EndYear = orgEHRS.EndYear;
                ehrs.StartYear = orgEHRS.StartYear;
                ehrs.Other = orgEHRS.Other;
                ehrs.Type = orgEHRS.Type;
                ehrs.System = orgEHRS.System;

                modified.Add(ehrs);
            }

            await DataContext.SaveChangesAsync();

            return Accepted();
        }

        /// <summary>
        /// Delete EHR System.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("deleteehrs")]
        public async Task<IActionResult> DeleteEHRS([FromQuery] IEnumerable<Guid> id)
        {

            var ids = id.ToArray();

            var ehrs = await DataContext.OrganizationEHRSes.Where(eh => ids.Contains(eh.ID)).ToArrayAsync();

            DataContext.OrganizationEHRSes.RemoveRange(ehrs);

            await DataContext.SaveChangesAsync();

            return Accepted();
        }

        /// <summary>
        /// Returns a specified copied organization
        /// </summary>
        /// <param name="organizationID"></param>
        /// <returns></returns>
        [HttpGet("copy")]
        public async Task<Guid> Copy(Guid organizationID)
        {
            var existing = await (from o in DataContext.Organizations where o.ID == organizationID select o).FirstOrDefaultAsync();

            if (existing == null)
                throw new HttpResponseException(StatusCodes.Status404NotFound, "The Organization could not be found.");

            if (!await DataContext.HasPermissions<Organization>(Identity, existing.ID, PermissionIdentifiers.Organization.Copy))
                throw new HttpResponseException(StatusCodes.Status403Forbidden, "You do not have permission to copy the specified organization.");

            string newAcronym = "New " + existing.Acronym;
            string newName = "New " + existing.Name;

            while (await (from p in DataContext.Organizations where !p.Deleted && (p.Name == newName && p.Acronym == newAcronym) select p).AnyAsync())
            {
                newAcronym = "New " + newAcronym;
                newName = "New " + newName;
            }

            var organization = new Organization
            {
                Acronym = newAcronym,
                Name = newName,
                ParentOrganizationID = existing.ParentOrganizationID,
                ApprovalRequired = existing.ApprovalRequired,
                ContactEmail = existing.ContactEmail,
                ContactFirstName = existing.ContactFirstName,
                ContactLastName = existing.ContactLastName,
                ContactPhone = existing.ContactPhone,
                SpecialRequirements = existing.SpecialRequirements,
                UsageRestrictions = existing.UsageRestrictions,
                HealthPlanDescription = existing.HealthPlanDescription,
                EnableClaimsAndBilling = existing.EnableClaimsAndBilling,
                EnableEHRA = existing.EnableEHRA,
                EnableRegistries = existing.EnableRegistries,
                DataModelESP = existing.DataModelESP,
                DataModelHMORNVDW = existing.DataModelHMORNVDW,
                DataModelI2B2 = existing.DataModelI2B2,
                DataModelMSCDM = existing.DataModelMSCDM,
                DataModelOMOP = existing.DataModelOMOP,
                DataModelOther = existing.DataModelOther,
                DataModelOtherText = existing.DataModelOtherText,
                PragmaticClinicalTrials = existing.PragmaticClinicalTrials,
                Biorepositories = existing.Biorepositories,
                PatientReportedBehaviors = existing.PatientReportedBehaviors,
                PatientReportedOutcomes = existing.PatientReportedOutcomes,
                PrescriptionOrders = existing.PrescriptionOrders,
                InpatientEHRApplication = existing.InpatientEHRApplication,
                OutpatientEHRApplication = existing.OutpatientEHRApplication,
                OtherInpatientEHRApplication = existing.OtherInpatientEHRApplication,
                OtherOutpatientEHRApplication = existing.OtherOutpatientEHRApplication,
                InpatientClaims = existing.InpatientClaims,
                OutpatientClaims = existing.OutpatientClaims,
                ObservationalParticipation = existing.ObservationalParticipation,
                ProspectiveTrials = existing.ProspectiveTrials,
                EnrollmentClaims = existing.EnrollmentClaims,
                DemographicsClaims = existing.DemographicsClaims,
                LaboratoryResultsClaims = existing.LaboratoryResultsClaims,
                VitalSignsClaims = existing.LaboratoryResultsClaims,
                OtherClaims = existing.OtherClaims,
                OtherClaimsText = existing.OtherClaimsText,
                ObservationClinicalExperience = existing.ObservationClinicalExperience
            };

            DataContext.Organizations.Add(organization);

            //Registries
            var existingRegistries = await (from reg in DataContext.OrganizationRegistries where reg.OrganizationID == existing.ID select reg).ToArrayAsync();
            foreach (var existingRegistry in existingRegistries)
            {

                var reg = new OrganizationRegistry
                {
                    RegistryID = existingRegistry.RegistryID,
                    OrganizationID = organization.ID,
                    Description = existingRegistry.Description
                };
                DataContext.OrganizationRegistries.Add(reg);
            }

            //EHRS
            foreach (var existingEHR in await DataContext.OrganizationEHRSes.Where(e => e.OrganizationID == existing.ID).ToArrayAsync())
            {
                DataContext.OrganizationEHRSes.Add(new OrganizationEHRS
                {
                    EndYear = existingEHR.EndYear,
                    OrganizationID = organization.ID,
                    Other = existingEHR.Other,
                    StartYear = existingEHR.StartYear,
                    System = existingEHR.System,
                    Type = existingEHR.Type
                });
            }

            //Security Groups
            var existingSecurityGroups = await (from sg in DataContext.SecurityGroups.Include(x => x.Users) where sg.OwnerID == existing.ID orderby sg.ParentSecurityGroupID select sg).ToArrayAsync();
            var SecurityGroupMap = new Dictionary<Guid, Guid>();

            CopySecurityGroups(existingSecurityGroups, ref SecurityGroupMap, null, organization);

            await DataContext.SaveChangesAsync();

            //All of these are done this way with a conditional if because the triggers cause inserts that entity framework is not aware of. Note that they are parameterized to ensure no sql injections.

            foreach (var user in existingSecurityGroups.SelectMany(u => u.Users).DistinctBy(u => new { u.SecurityGroupID, u.UserID }))
            {
                await DataContext.Database.ExecuteSqlRawAsync(@"IF NOT EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE UserID = @UserID AND SecurityGroupID = @SecurityGroupID)
	INSERT INTO SecurityGroupUsers (UserID, SecurityGroupID, Overridden) VALUES (@UserID, @SecurityGroupID, 0)", new SqlParameter("UserID", user.UserID), new SqlParameter("SecurityGroupID", SecurityGroupMap[user.SecurityGroupID]));

            }


            //Org Acls
            var existingSecurityGroupIDs = SecurityGroupMap.Select(gm => gm.Key).ToArray();
            var existingOrganizationAcls = await (from a in DataContext.OrganizationAcls where a.OrganizationID == existing.ID && existingSecurityGroupIDs.Contains(a.SecurityGroupID) select a).Distinct().ToArrayAsync();

            foreach (var existingOrganizationAcl in existingOrganizationAcls)
            {
                if (!SecurityGroupMap.ContainsKey(existingOrganizationAcl.SecurityGroupID))
                    SecurityGroupMap.Add(existingOrganizationAcl.SecurityGroupID, existingOrganizationAcl.SecurityGroupID);

                var count = await DataContext.Database.ExecuteSqlRawAsync(@"IF NOT EXISTS(SELECT NULL FROM AclOrganizations WHERE OrganizationID = @OrganizationID AND SecurityGroupID = @SecurityGroupID AND PermissionID = @PermissionID)
	INSERT INTO AclOrganizations (OrganizationID, SecurityGroupID, PermissionID, Allowed, Overridden) VALUES (@OrganizationID, @SecurityGroupID, @PermissionID, @Allowed, 1)", new SqlParameter("OrganizationID", organization.ID), new SqlParameter("SecurityGroupID", SecurityGroupMap[existingOrganizationAcl.SecurityGroupID]), new SqlParameter("PermissionID", existingOrganizationAcl.PermissionID), new SqlParameter("Allowed", existingOrganizationAcl.Allowed));

            }

            //Org Event Acls
            var existingOrganizationEventAcls = await (from a in DataContext.OrganizationEvents where a.OrganizationID == existing.ID select a).ToArrayAsync();
            foreach (var existingOrganizationEventAcl in existingOrganizationEventAcls)
            {
                if (!SecurityGroupMap.ContainsKey(existingOrganizationEventAcl.SecurityGroupID))
                    SecurityGroupMap.Add(existingOrganizationEventAcl.SecurityGroupID, existingOrganizationEventAcl.SecurityGroupID);

                await DataContext.Database.ExecuteSqlRawAsync(@"IF NOT EXISTS(SELECT NULL FROM OrganizationEvents WHERE OrganizationID = @OrganizationID AND SecurityGroupID = @SecurityGroupID AND EventID = @EventID)
	INSERT INTO OrganizationEvents (OrganizationID, SecurityGroupID, EventID, Allowed, Overridden) VALUES (@OrganizationID, @SecurityGroupID, @EventID, @Allowed, 0)", new SqlParameter("OrganizationID", organization.ID), new SqlParameter("SecurityGroupID", SecurityGroupMap[existingOrganizationEventAcl.SecurityGroupID]), new SqlParameter("EventID", existingOrganizationEventAcl.EventID), new SqlParameter("Allowed", existingOrganizationEventAcl.Allowed));
            }

            return organization.ID;
        }

        private void CopySecurityGroups(SecurityGroup[] existingSecurityGroups, ref Dictionary<Guid, Guid> securityGroupMap, Guid? parentSecurityGroupID, Organization organization)
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
                    OwnerID = organization.ID,
                    Owner = organization.Acronym,
                    Type = DTO.Enums.SecurityGroupTypes.Organization,
                    Path = organization.Acronym + @"\" + existingSecurityGroup.Name,
                    ParentSecurityGroupID = existingSecurityGroup.ParentSecurityGroupID.HasValue && existingSecurityGroup.ParentSecurityGroupID.Value != existingSecurityGroup.ID ? securityGroupMap[existingSecurityGroup.ParentSecurityGroupID.Value] : (Guid?)null
                };
                DataContext.SecurityGroups.Add(sg);

                securityGroupMap.Add(existingSecurityGroup.ID, sg.ID);

                CopySecurityGroups(existingSecurityGroups, ref securityGroupMap, existingSecurityGroup.ID, organization);
            }
        }

        /// <summary>
        /// Flags the organization as deleted.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task Delete([FromQuery] IEnumerable<Guid> ID)
        {
            if (!await DataContext.CanDelete<Organization>(Identity, ID.ToArray()))
                throw new HttpResponseException(StatusCodes.Status403Forbidden, "You do not have permission to delete this Organization.");

            var orgs = await (from o in DataContext.Organizations where ID.Contains(o.ID) select o).ToArrayAsync();
            foreach (var org in orgs)
            {
                org.Deleted = true;
            }

            var users = await (from u in DataContext.Users where ID.Contains(u.OrganizationID.Value) select u).ToArrayAsync();
            foreach (var user in users)
            {
                user.Deleted = true;
            }

            var dms = await (from d in DataContext.DataMarts where ID.Contains(d.OrganizationID) select d).ToArrayAsync();
            foreach (var dm in dms)
            {
                dm.Deleted = true;
            }

            await DataContext.SaveChangesAsync();

            var securityGroups = await (from sg in DataContext.SecurityGroups where ID.Contains(sg.OwnerID) select sg.ID).ToArrayAsync();
            if (securityGroups.Any())
            {
                var securityGroupIDs = String.Join(",", securityGroups.Select(x => String.Format("'{0}'", x)));
                await DataContext.Database.ExecuteSqlRawAsync(string.Format("delete from AclGlobal where SecurityGroupID IN ({0})", securityGroupIDs));
                await DataContext.Database.ExecuteSqlRawAsync(string.Format("delete from AclDataMarts where SecurityGroupID IN ({0})", securityGroupIDs));
                await DataContext.Database.ExecuteSqlRawAsync(string.Format("delete from AclDataMartRequestTypes where SecurityGroupID IN ({0})", securityGroupIDs));
                await DataContext.Database.ExecuteSqlRawAsync(string.Format("delete from AclOrganizations where SecurityGroupID IN ({0})", securityGroupIDs));
                await DataContext.Database.ExecuteSqlRawAsync(string.Format("delete from AclOrganizationDataMarts where SecurityGroupID IN ({0})", securityGroupIDs));
                await DataContext.Database.ExecuteSqlRawAsync(string.Format("delete from AclOrganizationUsers where SecurityGroupID IN ({0})", securityGroupIDs));
                await DataContext.Database.ExecuteSqlRawAsync(string.Format("delete from AclProjects where SecurityGroupID IN ({0})", securityGroupIDs));
                await DataContext.Database.ExecuteSqlRawAsync(string.Format("delete from AclProjectOrganizations where SecurityGroupID IN ({0})", securityGroupIDs));
                await DataContext.Database.ExecuteSqlRawAsync(string.Format("delete from AclProjectDataMarts where SecurityGroupID IN ({0})", securityGroupIDs));
                await DataContext.Database.ExecuteSqlRawAsync(string.Format("delete from AclProjectDataMartRequestTypes where SecurityGroupID IN ({0})", securityGroupIDs));
                await DataContext.Database.ExecuteSqlRawAsync(string.Format("delete from AclRegistries where SecurityGroupID IN ({0})", securityGroupIDs));
                await DataContext.Database.ExecuteSqlRawAsync(string.Format("delete from AclRequests where SecurityGroupID IN ({0})", securityGroupIDs));
                await DataContext.Database.ExecuteSqlRawAsync(string.Format("delete from AclRequestSharedFolders where SecurityGroupID IN ({0})", securityGroupIDs));
                await DataContext.Database.ExecuteSqlRawAsync(string.Format("delete from AclRequestTypes where SecurityGroupID IN ({0})", securityGroupIDs));
                await DataContext.Database.ExecuteSqlRawAsync(string.Format("delete from AclUsers where SecurityGroupID IN ({0})", securityGroupIDs));
                await DataContext.Database.ExecuteSqlRawAsync(string.Format("delete from AclRequestTypes where SecurityGroupID IN ({0})", securityGroupIDs));
                await DataContext.Database.ExecuteSqlRawAsync(string.Format("delete from GlobalEvents where SecurityGroupID IN ({0})", securityGroupIDs));
                await DataContext.Database.ExecuteSqlRawAsync(string.Format("delete from DataMartEvents where SecurityGroupID IN ({0})", securityGroupIDs));
                await DataContext.Database.ExecuteSqlRawAsync(string.Format("delete from GroupEvents where SecurityGroupID IN ({0})", securityGroupIDs));
                await DataContext.Database.ExecuteSqlRawAsync(string.Format("delete from OrganizationEvents where SecurityGroupID IN ({0})", securityGroupIDs));
                await DataContext.Database.ExecuteSqlRawAsync(string.Format("delete from ProjectEvents where SecurityGroupID IN ({0})", securityGroupIDs));
                await DataContext.Database.ExecuteSqlRawAsync(string.Format("delete from ProjectOrganizationEvents where SecurityGroupID IN ({0})", securityGroupIDs));
                await DataContext.Database.ExecuteSqlRawAsync(string.Format("delete from ProjectDataMartEvents where SecurityGroupID IN ({0})", securityGroupIDs));
            }
        }
    }
}
