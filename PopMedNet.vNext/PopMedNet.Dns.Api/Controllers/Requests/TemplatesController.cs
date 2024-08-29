using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopMedNet.Dns.Data;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Dns.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.OData.Query;
using PopMedNet.Utilities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace PopMedNet.Dns.Api.Requests
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = PopMedNet.Utilities.WebSites.Security.AuthSchemeConstants.Scheme)]
    public class TemplatesController : ApiDataControllerBase<Template, TemplateDTO, DataContext, PermissionDefinition>
    {
        public TemplatesController(IConfiguration config, DataContext db, IMapper mapper)
            : base(db, mapper, config)
        {
        }

        /// <summary>
        /// Returns all Criteria Group templates the user has view rights for.
        /// </summary>
        /// <returns></returns>
        [HttpGet("criteriagroups")]
        public IActionResult CriteriaGroups(ODataQueryOptions<TemplateDTO> options)
        {
            //return all the criteria templates the user has permission to see or created
            var query = DataContext.Filter((DataContext.Templates.Where(t => t.Type == DTO.Enums.TemplateTypes.CriteriaGroup)), Identity, PermissionIdentifiers.Templates.View).Union(DataContext.Templates.Where(t => t.Type == DTO.Enums.TemplateTypes.CriteriaGroup && t.CreatedByID == Identity.ID)).ProjectTo<TemplateDTO>(_mapper.ConfigurationProvider);
            
            var queryHelper = new Utilities.WebSites.ODataQueryHandler<TemplateDTO>(query, options);
            return Ok(queryHelper.Result());
        }

        /// <summary>
        /// Gets the templates for the specified request type.
        /// </summary>
        /// <param name="requestTypeID">The ID of the request type.</param>
        /// <returns></returns>
        [HttpGet("getbyrequesttype")]
        public IActionResult GetByRequestType(ODataQueryOptions<TemplateDTO> options, Guid requestTypeID)
        {
            var query = DataContext.Templates.Where(t => t.RequestTypeID == requestTypeID && t.Type == DTO.Enums.TemplateTypes.Request).ProjectTo<TemplateDTO>(_mapper.ConfigurationProvider);

            var queryHelper = new Utilities.WebSites.ODataQueryHandler<TemplateDTO>(query, options);
            return Ok(queryHelper.Result());
        }

        /// <summary>
        /// Gets the collection of template terms for the specified template.
        /// </summary>
        /// <param name="ID">The ID of the template</param>
        /// <returns></returns>
        [HttpGet("listhiddenterms")]
        public async Task<IEnumerable<DTO.QueryComposer.TemplateTermDTO>> ListHiddenTerms(Guid ID)
        {
            var result = from tt in DataContext.TemplateTerms
                         join template in DataContext.Templates on tt.TemplateID equals template.ID
                         where template.ID == ID
                         select new DTO.QueryComposer.TemplateTermDTO
                         {
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
        [HttpGet("listhiddentermsbyrequesttype")]
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

        /// <summary>
        /// Gets a result indicating information regarding global edit permissions for templates.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getglobaltemplatepermissions")]
        public IEnumerable<HasGlobalSecurityForTemplateDTO> GetGlobalTemplatePermissions()
        {
            var result = (from u in DataContext.Users
                          let aclGlobals = DataContext.GlobalAcls.Where(a => a.PermissionID == PermissionIdentifiers.Templates.ManageSecurity.ID).AsEnumerable()
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
        [HttpPost("savecriteriagroup")]
        public async Task<IEnumerable<TemplateDTO>> SaveCriteriaGroup([FromBody] CreateCriteriaGroupTemplateDTO details)
        {
            if (string.IsNullOrEmpty(details.Name))
                throw new ArgumentException("The template Name is required.");

            if (string.IsNullOrEmpty(details.Json))
                throw new ArgumentException("The content of the criteria group template cannot be empty.");

            var template = new Template
            {
                CreatedByID = Identity.ID,
                Name = details.Name,
                Description = details.Description,
                CreatedOn = DateTime.UtcNow,
                Type = DTO.Enums.TemplateTypes.CriteriaGroup,
                QueryType = details.AdapterDetail,
                ComposerInterface = DTO.Enums.QueryComposerInterface.PresetQuery,
                Data = details.Json
            };
                DataContext.Templates.Add(template);

            await DataContext.SaveChangesAsync();

            var templateDTO = new TemplateDTO
            {
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

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] IEnumerable<Guid> ID)
        {
            if (!await DataContext.CanDelete<Template>(Identity, ID.ToArray()))
                throw new HttpResponseException(StatusCodes.Status403Forbidden, "You do not have permission to delete this RequestType.");

            var templates = await DataContext.Templates.Where(t => ID.Contains(t.ID)).ToArrayAsync();
            if (templates.Length == 0)
                return NotFound();

            foreach(var template in templates)
            {
                DataContext.TemplateAcls.RemoveRange(DataContext.TemplateAcls.Where(a => a.TemplateID == template.ID).AsEnumerable());
                DataContext.TemplateTerms.RemoveRange(DataContext.TemplateTerms.Where(a => a.TemplateID == template.ID).AsEnumerable());

                DataContext.Templates.Remove(template);
            }

            await DataContext.SaveChangesAsync();

            return Accepted();
        }
    }
}
