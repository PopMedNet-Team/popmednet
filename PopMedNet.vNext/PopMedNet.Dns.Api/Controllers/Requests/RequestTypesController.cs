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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.Extensions.Options;
using System.Net;

namespace PopMedNet.Dns.Api.Requests
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = PopMedNet.Utilities.WebSites.Security.AuthSchemeConstants.Scheme)]
    public class RequestTypesController : ApiDataControllerBase<RequestType, RequestTypeDTO, DataContext, PermissionDefinition>
    {
        public RequestTypesController(IConfiguration config, DataContext db, IMapper mapper)
            : base(db, mapper, config)
        {
        }

        static readonly Guid QueryComposerModelProcessorID = new Guid("AE0DA7B0-0F73-4D06-B70B-922032B7F0EB");
        const string QueryComposerPackageIdentifier = "Lpp.Dns.DataMart.Model.QueryComposer";

        /// <summary>
        /// Inserts request types.
        /// </summary>
        /// <param name="values">Enumerable RequestTypeDTOs</param>
        /// <returns>Inserted RequestTypeDTOs</returns>
        [HttpPost]
        public override async Task<IActionResult> InsertOrUpdate(IEnumerable<RequestTypeDTO> values)
        {
            return await Task.FromResult((IActionResult)StatusCode(StatusCodes.Status405MethodNotAllowed, "Action not supported. Saves should be completed using Save method "));

        }

        /// <summary>
        /// Return all RequestTypes that the user has View rights for.
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public override IActionResult List(ODataQueryOptions<RequestTypeDTO> options)
        {
            var result = (from rt in DataContext.Secure<RequestType>(Identity) select rt);
            var query = DataContext.Filter(result, Identity, new[] { PermissionIdentifiers.RequestTypes.View }).ProjectTo<RequestTypeDTO>(_mapper.ConfigurationProvider);

            var queryHelper = new Utilities.WebSites.ODataQueryHandler<RequestTypeDTO>(query, options);
            return Ok(queryHelper.Result());
        }

        /// <summary>
        /// Return all RequestTypes 
        /// </summary>
        /// <returns></returns>
        [HttpGet("ListAvailableRequestTypes"), EnableQuery]
        public IQueryable<RequestTypeDTO> ListAvailableRequestTypes()
        {
            return DataContext.RequestTypes.AsQueryable().ProjectTo<RequestTypeDTO>(_mapper.ConfigurationProvider);
        }

        /// <summary>
        /// Returns the terms for a specific request type.
        /// </summary>
        /// <param name="requestTypeID"></param>
        /// <returns></returns>
        [HttpGet("getrequesttypemodels")]
        public IActionResult GetRequestTypeModels(ODataQueryOptions<RequestTypeModelDTO> options, Guid requestTypeID)
        {
            var requestTypeModels = DataContext.Secure<RequestType>(Identity).Where(rt => rt.ID == requestTypeID).SelectMany(rt => rt.Models).ProjectTo<RequestTypeModelDTO>(_mapper.ConfigurationProvider);

            var queryHelper = new Utilities.WebSites.ODataQueryHandler<RequestTypeModelDTO>(requestTypeModels, options);
            return Ok(queryHelper.Result());
        }

        /// <summary>
        /// Returns the terms for a specific request type.
        /// </summary>
        /// <param name="requestTypeID"></param>
        /// <returns></returns>
        [HttpGet("getrequesttypeterms")]
        public IActionResult GetRequestTypeTerms(ODataQueryOptions<RequestTypeTermDTO> options, Guid requestTypeID)
        {
            var query = DataContext.RequestTypeTerms.Where(t => t.RequestTypeID == requestTypeID).ProjectTo<RequestTypeTermDTO>(_mapper.ConfigurationProvider);

            var queryHelper = new Utilities.WebSites.ODataQueryHandler<RequestTypeTermDTO>(query, options);
            return Ok(queryHelper.Result());
        }

        /// <summary>
        /// Inserts or updates the requesttype, template, and associated models
        /// </summary>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<IActionResult> Save(UpdateRequestTypeRequestDTO details)
        {
            //if (details.Queries == null || !details.Queries.Any())
            //{
            //    throw new ArgumentException("Templates cannot be null for the RequestType, at least one template is required.", "details.Templates");
            //}

            if (details.RequestType == null)
            {
                throw new ArgumentException("RequestType cannot be null.", "details.RequestType");
            }

            RequestType? requestType = null;

            if (details.RequestType.ID.HasValue)
            {
                //check for edit permission
                if ((await DataContext.HasGrantedPermissions<RequestType>(Identity, details.RequestType.ID.Value, PermissionIdentifiers.RequestTypes.Edit)).Any() == false)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "Permission required to update the specified request type.");
                }

                requestType = await DataContext.RequestTypes.FirstOrDefaultAsync(rt => rt.ID == details.RequestType.ID.Value);

                if(requestType == null)
                {
                    return NotFound();
                }

                await DataContext.Entry(requestType).Collection(r => r.Queries).LoadAsync();
                await DataContext.Entry(requestType).Collection(r => r.Terms).LoadAsync();

            }
            else
            {
                if (!(await DataContext.HasPermissions<RequestType>(Identity, PermissionIdentifiers.Portal.CreateRequestTypes)))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "Permission required to create request types.");
                }

                requestType = new RequestType { ProcessorID = QueryComposerModelProcessorID, PackageIdentifier = QueryComposerPackageIdentifier, Models = new HashSet<RequestTypeModel>(), Terms = new HashSet<RequestTypeTerm>() };
                DataContext.RequestTypes.Add(requestType);
            }

            //update request type
            _mapper.Map(details.RequestType, requestType);
            details.RequestType.ID = requestType.ID;

            //update the request type permissions
            var updatedAcls = new List<AclRequestType>();
            var dbAcls = await DataContext.RequestTypeAcls.Where(a => a.RequestTypeID == requestType.ID).ToArrayAsync();

            foreach (var acl in dbAcls)
            {
                var perm = details.Permissions.FirstOrDefault(p => requestType.ID == p.RequestTypeID && p.SecurityGroupID == acl.SecurityGroupID && p.PermissionID == acl.PermissionID);
                if (perm == null || perm.Allowed == null)
                {
                    DataContext.RequestTypeAcls.Remove(acl);
                }
                else if (acl.Allowed != perm.Allowed.Value)
                {
                    acl.Allowed = perm.Allowed.Value;
                    acl.Overridden = true;
                    updatedAcls.Add(acl);
                }
            }

            var newAcls = details.Permissions.Where(p => (p.RequestTypeID == requestType.ID || p.RequestTypeID == Guid.Empty) && p.Allowed.HasValue && dbAcls.Any(a => p.SecurityGroupID == a.SecurityGroupID && p.PermissionID == a.PermissionID) == false).ToArray();
            foreach (var acl in newAcls)
            {
                var a = new AclRequestType
                {
                    Allowed = acl.Allowed!.Value,
                    Overridden = true,
                    PermissionID = acl.PermissionID,
                    RequestTypeID = requestType.ID,
                    SecurityGroupID = acl.SecurityGroupID
                };
                DataContext.RequestTypeAcls.Add(a);
                updatedAcls.Add(a);
            }

            ////update the queries for the requesttype 
            //foreach (var templateDTO in details.Queries)
            //{
            //    Template template = null;
            //    if (templateDTO.ID.HasValue)
            //    {
            //        template = requestType.Queries.FirstOrDefault(t => t.ID == templateDTO.ID.Value);
            //    }

            //    if (template == null)
            //    {
            //        template = new Template
            //        {
            //            CreatedByID = Identity.ID,
            //            CreatedOn = DateTime.UtcNow,
            //            Type = TemplateTypes.Request,
            //            RequestType = requestType,
            //            RequestTypeID = requestType.ID
            //        };

            //        DataContext.Templates.Add(template);
            //        requestType.Queries.Add(template);
            //        templateDTO.ID = template.ID;
            //    }

            //    //these properties should never change after creation
            //    templateDTO.CreatedByID = template.CreatedByID;
            //    templateDTO.CreatedOn = template.CreatedOn;
            //    templateDTO.Type = TemplateTypes.Request;
            //    templateDTO.RequestTypeID = requestType.ID;

            //    _mapper.Map(templateDTO, template);

            //    if (string.IsNullOrEmpty(template.Name))
            //    {
            //        template.Name = "Cohort " + (template.Order + 1);
            //    }
            //}

            //for requesttype templates remove all permissions - permission will be assumbed by the requesttype permissions
            DataContext.TemplateAcls.RemoveRange(DataContext.TemplateAcls.Where(a => a.Template.RequestTypeID == requestType.ID));

            //update associated models
            if (details.RequestType.ID.HasValue)
            {
                await DataContext.Entry(requestType).Collection(m => m.Models).LoadAsync();
            }

            if (details.Models != null && details.Models.Any())
            {
                //models to delete
                List<RequestTypeModel> modelToDelete = new List<RequestTypeModel>();
                modelToDelete.AddRange(requestType.Models.Where(m => details.Models.Any(id => m.DataModelID == id) == false).ToArray());
                foreach (var rtm in modelToDelete)
                {
                    requestType.Models.Remove(rtm);
                }

                //models to add
                var modelsToAdd = details.Models.Where(id => requestType.Models.Any(mm => mm.DataModelID == id) == false).Select(m => new RequestTypeModel { RequestTypeID = requestType.ID, DataModelID = m });
                foreach (var newModel in modelsToAdd)
                {
                    requestType.Models.Add(newModel);
                }
            }
            else
            {
                requestType.Models.Clear();
            }

            //update associated terms
            if (details.Terms != null && details.Terms.Any())
            {
                //terms to delete
                List<RequestTypeTerm> termsToDelete = new List<RequestTypeTerm>();
                termsToDelete.AddRange(requestType.Terms.Where(t => details.Terms.Any(id => t.TermID == id) == false).ToArray());
                foreach (var term in termsToDelete)
                {
                    requestType.Terms.Remove(term);
                }

                //terms to add
                var termsToAdd = details.Terms.Where(id => requestType.Terms.Any(t => t.TermID == id) == false).Select(t => new RequestTypeTerm { RequestTypeID = requestType.ID, TermID = t });
                foreach (var newTerm in termsToAdd)
                {
                    requestType.Terms.Add(newTerm);
                }
            }
            else
            {
                requestType.Terms.Clear();
            }

            /* NotAllowed Terms are the terms that are hidden from the end user when not in template edit mode - ie creating a new request. */
            if (details.NotAllowedTerms.Any())
            {
                foreach (var template in details.Queries)
                {
                    //clear out the existing hidden terms for the template
                    DataContext.TemplateTerms.RemoveRange(DataContext.TemplateTerms.Where(t => t.TemplateID == template.ID.Value));

                    //add any specified term to the hidden terms collection
                    foreach (var ht in details.NotAllowedTerms.Where(nt => nt.TemplateID == template.ID.Value).ToArray())
                    {
                        DataContext.TemplateTerms.Add(new TemplateTerm { TermID = ht.TermID, TemplateID = ht.TemplateID, Section = ht.Section, Allowed = false });
                    }

                }
            }


            await DataContext.SaveChangesAsync();

            var responseDetail = await (from rt in DataContext.RequestTypes
                                        where rt.ID == requestType.ID
                                        select new UpdateRequestTypeResponseDTO
                                        {
                                            RequestType = new RequestTypeDTO
                                            {
                                                AddFiles = rt.AddFiles,
                                                Description = rt.Description,
                                                ID = rt.ID,
                                                Metadata = rt.MetaData,
                                                Name = rt.Name,
                                                Notes = rt.Notes,
                                                PostProcess = rt.PostProcess,
                                                RequiresProcessing = rt.RequiresProcessing,
                                                SupportMultiQuery = rt.SupportMultiQuery,
                                                Timestamp = rt.Timestamp,
                                                Workflow = rt.Workflow.Name,
                                                WorkflowID = rt.WorkflowID
                                            },
                                            Queries = rt.Queries.OrderBy(q => q.Order).Select(q => new TemplateDTO
                                            {
                                                ID = q.ID,
                                                ComposerInterface = q.ComposerInterface,
                                                CreatedBy = q.CreatedBy.UserName,
                                                CreatedByID = q.CreatedByID,
                                                CreatedOn = q.CreatedOn,
                                                Data = q.Data,
                                                Description = q.Description,
                                                Name = q.Name,
                                                Notes = q.Notes,
                                                Order = q.Order,
                                                QueryType = q.QueryType,
                                                RequestType = rt.Name,
                                                RequestTypeID = rt.ID,
                                                Timestamp = q.Timestamp,
                                                Type = q.Type
                                            })
                                        }).FirstOrDefaultAsync();


            return Accepted(responseDetail);
        }

        /// <summary>
        /// Deletes request types.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] IEnumerable<Guid> ID)
        {
            if (!await DataContext.CanDelete<RequestType>(Identity, ID.ToArray()))
                throw new HttpResponseException(StatusCodes.Status403Forbidden, "You do not have permission to delete this RequestType.");

            //Cannot delete a request type if it is being used by a request
            var requests = (from r in DataContext.Requests where ID.Contains(r.RequestTypeID) select r);
            if (await requests.AnyAsync())
                throw new HttpResponseException(StatusCodes.Status403Forbidden, "Cannot delete a request type that is used by an existing request.");


            var requestTypes = await (from rt in DataContext.RequestTypes where ID.Contains(rt.ID) select rt).ToArrayAsync();
            //Deleting a request type should delete any associated model relationships, the query template, and all associated acls.
            foreach (var requestType in requestTypes)
            {
                //Delete associated model relationships
                var models = DataContext.RequestTypeDataModels.Where(rtm => rtm.RequestTypeID == requestType.ID).AsEnumerable();
                DataContext.RequestTypeDataModels.RemoveRange(models);

                //Delete associated term relationships
                var terms = DataContext.RequestTypeTerms.Where(tm => tm.RequestTypeID == requestType.ID).AsEnumerable();
                DataContext.RequestTypeTerms.RemoveRange(terms);

                //Delete the associated Acls: Request Type, DataMart Request Type, Project DataMart Request Type, Project Request Type, Project Request Type Workflow Activity
                var rtAcls = DataContext.RequestTypeAcls.Where(rt => rt.RequestTypeID == requestType.ID).AsEnumerable();
                DataContext.RequestTypeAcls.RemoveRange(rtAcls);

                var drtAcls = DataContext.DataMartRequestTypeAcls.Where(rt => rt.RequestTypeID == requestType.ID).AsEnumerable();
                DataContext.DataMartRequestTypeAcls.RemoveRange(drtAcls);
                var pdrtAcls = DataContext.ProjectDataMartRequestTypeAcls.Where(rt => rt.RequestTypeID == requestType.ID).AsEnumerable();
                DataContext.ProjectDataMartRequestTypeAcls.RemoveRange(pdrtAcls);
                var prtAcls = DataContext.ProjectRequestTypeAcls.Where(rt => rt.RequestTypeID == requestType.ID).AsEnumerable();
                DataContext.ProjectRequestTypeAcls.RemoveRange(prtAcls);
                var prtWFAcls = DataContext.ProjectRequestTypeWorkflowActivities.Where(rt => rt.RequestTypeID == requestType.ID).AsEnumerable();
                DataContext.ProjectRequestTypeWorkflowActivities.RemoveRange(prtWFAcls);

                //Delete the query templates
                var templates = DataContext.Templates.Where(t => t.RequestTypeID == requestType.ID).AsEnumerable();
                DataContext.Templates.RemoveRange(templates);

                //Delete the request type
                DataContext.RequestTypes.Remove(requestType);

            }

            await DataContext.SaveChangesAsync();

            return Ok();
        }
    }
}
