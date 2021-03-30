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
            //return all the criteria templates the user has permission to see or created
            return DataContext.Filter((DataContext.Templates.Where(t => t.Type == DTO.Enums.TemplateTypes.CriteriaGroup)), Identity, PermissionIdentifiers.Templates.View).Union(DataContext.Templates.Where(t => t.Type == DTO.Enums.TemplateTypes.CriteriaGroup && t.CreatedByID == Identity.ID)).Map<Template, TemplateDTO>();
        }

        /// <summary>
        /// Gets the templates for the specified request type.
        /// </summary>
        /// <param name="requestTypeID">The ID of the request type.</param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<TemplateDTO> GetByRequestType(Guid requestTypeID)
        {
            var result = DataContext.Templates.Where(t => t.RequestTypeID == requestTypeID && t.Type == DTO.Enums.TemplateTypes.Request).Map<Template, TemplateDTO>();
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
        public async Task<IEnumerable<TemplateDTO>> SaveCriteriaGroup([FromBody] CreateCriteriaGroupTemplateDTO details)
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
                ComposerInterface = DTO.Enums.QueryComposerInterface.PresetQuery,
                Data = details.Json
            });

            await DataContext.SaveChangesAsync();

            var templateDTO = new TemplateDTO {
                ID = template.ID,
                ComposerInterface = template.ComposerInterface,
                CreatedByID = Identity.ID,
                CreatedOn = template.CreatedOn,
                Data = template.Data,
                Description = template.Description,
                Name = template.Name,
                Order = template.Order,
                QueryType = template.QueryType,
                Type = template.Type
            };

            return new[] { templateDTO };
        }

        /// <summary>
        /// Gets the collection of template terms for the specified template.
        /// </summary>
        /// <param name="ID">The ID of the template</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<DTO.QueryComposer.TemplateTermDTO>> ListHiddenTerms(Guid ID)
        {
            var result = from tt in DataContext.TemplateTerms
                         join template in DataContext.Templates on tt.TemplateID equals template.ID
                         where template.ID == ID
                         select new DTO.QueryComposer.TemplateTermDTO {
                             Allowed = tt.Allowed,
                             Section = tt.Section,
                             TemplateID = tt.TemplateID,
                             TermID = tt.TermID
                         };

            return await result.ToArrayAsync();
        }

        /// <summary>
        /// Returns all the hidden terms associated to templates for the specified request type.
        /// </summary>
        /// <param name="id">The id of the request type.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<DTO.QueryComposer.TemplateTermDTO>> ListHiddenTermsByRequestType(Guid id)
        {
            var result = from tt in DataContext.TemplateTerms
                         join template in DataContext.Templates on tt.TemplateID equals template.ID
                         where template.RequestTypeID == id
                         select new DTO.QueryComposer.TemplateTermDTO
                         {
                             Allowed = tt.Allowed,
                             Section = tt.Section,
                             TemplateID = tt.TemplateID,
                             TermID = tt.TermID
                         };

            return await result.ToArrayAsync();
        }
    }
}