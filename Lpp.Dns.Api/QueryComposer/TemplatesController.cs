using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities;
using Lpp.Utilities.WebSites.Controllers;

namespace Lpp.Dns.Api.Controllers
{
    /// <summary>
    /// Manages Templates
    /// </summary>
    public class TemplatesController : LppApiDataController<Template, TemplateDTO, DataContext, PermissionDefinition>
    {
        /// <summary>
        /// Return all Templates that the user has View rights for.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public override IQueryable<TemplateDTO> List()
        {
            List<PermissionDefinition> lstPermissions = new List<PermissionDefinition>();
            lstPermissions.Add(PermissionIdentifiers.Templates.View);

            var result = (from rt in DataContext.Secure<Template>(Identity) select rt);
            return DataContext.Filter(result, Identity, lstPermissions.ToArray()).Map<Template, TemplateDTO>();
        }

        /// <summary>
        /// Returns all Criteria Group templates the user has view rights for.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<TemplateDTO> CriteriaGroups()
        {
            return DataContext.Filter((DataContext.Templates.Where(t => t.Type == DTO.Enums.TemplateTypes.CriteriaGroup)), Identity, PermissionIdentifiers.Templates.View).Map<Template, TemplateDTO>();
        }

        /// <summary>
        /// Gets the template for the specified request type.
        /// </summary>
        /// <param name="requestTypeID">The ID of the request type.</param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<TemplateDTO> GetByRequestType(Guid requestTypeID)
        {
            var result = DataContext.RequestTypes.Where(rt => rt.ID == requestTypeID && rt.TemplateID.HasValue).Select(rt => rt.Template).Map<Template, TemplateDTO>();
            return result;
        }

        /// <summary>
        /// Gets a result indicating information regarding global edit permissions for templates.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<HasGlobalSecurityForTemplateDTO> GetGlobalTemplatePermissions()
        {
            var result = (from u in DataContext.Users
                         let aclGlobals = DataContext.GlobalAcls.Where(a => a.PermissionID == PermissionIdentifiers.Templates.ManageSecurity.ID)
                         where u.ID == Identity.ID
                         select new HasGlobalSecurityForTemplateDTO
                         {
                             //see if any security groups that have users have been assigned the global manage security permission
                             SecurityGroupExistsForGlobalPermission = (aclGlobals.Any(a => a.Allowed) && aclGlobals.All(a => a.Allowed) && aclGlobals.Any(a => a.SecurityGroup.Users.Any())),
                             //see if the current user has global permission to manage security
                             CurrentUserHasGlobalPermission = (aclGlobals.Any(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == u.ID)) && aclGlobals.Any(a => a.Allowed) && aclGlobals.All(a => a.Allowed))
                         }).ToArray();

            return result;
        }

        /// <summary>
        /// Saves a new criteria group template.
        /// </summary>
        /// <param name="details">The details of the criteria group to save, and parent entity to clone security from.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<TemplateDTO>> SaveCriteriaGroup([FromBody] SaveCriteriaGroupRequestDTO details)
        {
            if (string.IsNullOrEmpty(details.Name))
                throw new ArgumentException("The template Name is required.");

            if (string.IsNullOrEmpty(details.Json))
                throw new ArgumentException("The content of the criteria group template cannot be empty.");
            
            var template = DataContext.Templates.Add(new Template {
                CreatedByID = Identity.ID,
                Name = details.Name,
                Description = details.Description,
                CreatedOn = DateTime.UtcNow,
                Type = DTO.Enums.TemplateTypes.CriteriaGroup,
                QueryType = details.AdapterDetail,
                Data = details.Json
            });

            await DataContext.SaveChangesAsync();

            //clone the parent template permissions
            AclTemplate[] parentPermissions = null;
            if (details.TemplateID.HasValue)
            {
                parentPermissions = await DataContext.TemplateAcls.Where(a => a.TemplateID == details.TemplateID.Value).ToArrayAsync();
            }
            else if (details.RequestTypeID.HasValue)
            {
                parentPermissions = await DataContext.TemplateAcls.Where(a => a.Template.RequestTypes.Any(t => t.ID == details.RequestTypeID.Value)).ToArrayAsync();
            }
            else if (details.RequestID.HasValue)
            {
                parentPermissions = await DataContext.TemplateAcls.Where(a => DataContext.Requests.Where(r => r.RequestType.TemplateID == a.TemplateID).Any()).ToArrayAsync();
            }

            if(parentPermissions != null && parentPermissions.Length > 0)
            {
                foreach (var acl in parentPermissions)
                {
                    DataContext.TemplateAcls.Add(
                        new AclTemplate { 
                            TemplateID = template.ID,
                            SecurityGroupID = acl.SecurityGroupID,
                            PermissionID = acl.PermissionID,
                            Overridden = acl.Overridden,
                            Allowed = acl.Allowed
                        }
                    );
                }

                await DataContext.SaveChangesAsync();
            }

            //load the creator for use in dto mapping.
            await DataContext.Entry(template).Reference(t => t.CreatedBy).LoadAsync();

            return new[] { template.Map<Template, TemplateDTO>() };
        }
    }
}