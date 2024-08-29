using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Utilities.WebSites.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lpp.Utilities;
using System.Threading.Tasks;
using System.Data.Entity;
using Lpp.Dns.DTO.Security;
using Lpp.Dns.DTO.QueryComposer;

namespace Lpp.Dns.Api.Requests
{
    /// <summary>
    /// Controller that services Request Type related actions.
    /// </summary>
    public class RequestTypesController : LppApiDataController<RequestType, RequestTypeDTO, DataContext, PermissionDefinition>
    {
        static readonly Guid QueryComposerModelProcessorID = new Guid("AE0DA7B0-0F73-4D06-B70B-922032B7F0EB");
        const string QueryComposerPackageIdentifier = "Lpp.Dns.DataMart.Model.QueryComposer";
        /// <summary>
        /// Inserts request types.
        /// </summary>
        /// <param name="values">Enumerable RequestTypeDTOs</param>
        /// <returns>Inserted RequestTypeDTOs</returns>
        [HttpPost]
        public override async Task<IEnumerable<RequestTypeDTO>> Insert(IEnumerable<RequestTypeDTO> values)
        {
            //throw error because this Insert should not be used. Save method should be used instead.
            throw new ArgumentException("Action not supported. Saves should be completed using Save method ");

        }

        /// <summary>
        /// Return all RequestTypes that the user has View rights for.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public override IQueryable<RequestTypeDTO> List()
        {
            List<PermissionDefinition> lstPermissions = new List<PermissionDefinition>();
            lstPermissions.Add(PermissionIdentifiers.RequestTypes.View);

            var result = (from rt in DataContext.Secure<RequestType>(Identity) select rt);
            return DataContext.Filter(result, Identity, lstPermissions.ToArray()).Map<RequestType, RequestTypeDTO>();
        }

        /// <summary>
        /// Return all RequestTypes 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<RequestTypeDTO> ListAvailableRequestTypes()
        {
            return DataContext.RequestTypes.AsQueryable().Map<RequestType, RequestTypeDTO>();
        }

        /// <summary>
        /// Inserts or Updates if already exists request types.
        /// </summary>
        /// <param name="values">Enumerable RequestTypeDTOs</param>
        /// <returns>Inserted or updated RequestTypeDTOs</returns>
        [HttpPost]
        public override async System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.RequestTypeDTO>> InsertOrUpdate(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.RequestTypeDTO> values)
        {
            //throw error because this Insert should not be used.  Save method should be used instead.
            throw new ArgumentException("Action not supported. Saves should be completed using Save method ");

        }

        /// <summary>
        /// Updates request types.
        /// </summary>
        /// <param name="values">Enumerable RequestTypeDTOs</param>
        /// <returns>Updated RequestTypeDTOs</returns>
        [HttpPost]
        public override async Task<IEnumerable<RequestTypeDTO>> Update(IEnumerable<RequestTypeDTO> values)
        {

            //throw error because this Insert should not be used.  Save method should be used instead.
            throw new ArgumentException("Action not supported. Saves should be completed using Save method ");

        }

        /// <summary>
        /// Deletes request types.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        public async override Task Delete([FromUri]IEnumerable<Guid> ID)
        {
            if (!await DataContext.CanDelete<RequestType>(Identity, ID.ToArray()))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to delete this RequestType."));

            //Cannot delete a request type if it is being used by a request
            var requests = await (from r in DataContext.Requests where ID.Contains(r.RequestTypeID) select r).ToArrayAsync();
            if (requests.Any())
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Cannot delete a request type that is used by an existing request."));


            var requestTypes = await (from rt in DataContext.RequestTypes where ID.Contains(rt.ID) select rt).ToArrayAsync();
            //Deleting a request type should delete any associated model relationships, the query template, and all associated acls.
            foreach (var requestType in requestTypes)
            {
                //Delete associated model relationships
                var models = await DataContext.RequestTypeDataModels.Where(rtm => rtm.RequestTypeID == requestType.ID).ToArrayAsync();
                DataContext.RequestTypeDataModels.RemoveRange(models);

                //Delete associated term relationships
                var terms = await DataContext.RequestTypeTerms.Where(tm => tm.RequestTypeID == requestType.ID).ToArrayAsync();
                DataContext.RequestTypeTerms.RemoveRange(terms);

                //Delete the associated Acls: Request Type, DataMart Request Type, Project DataMart Request Type, Project Request Type, Project Request Type Workflow Activity
                var rtAcls = await DataContext.RequestTypeAcls.Where(rt => rt.RequestTypeID == requestType.ID).ToArrayAsync();
                DataContext.RequestTypeAcls.RemoveRange(rtAcls);
                var drtAcls = await DataContext.DataMartRequestTypeAcls.Where(rt => rt.RequestTypeID == requestType.ID).ToArrayAsync();
                DataContext.DataMartRequestTypeAcls.RemoveRange(drtAcls);
                var pdrtAcls = await DataContext.ProjectDataMartRequestTypeAcls.Where(rt => rt.RequestTypeID == requestType.ID).ToArrayAsync();
                DataContext.ProjectDataMartRequestTypeAcls.RemoveRange(pdrtAcls);
                var prtAcls = await DataContext.ProjectRequestTypeAcls.Where(rt => rt.RequestTypeID == requestType.ID).ToArrayAsync();
                DataContext.ProjectRequestTypeAcls.RemoveRange(prtAcls);
                var prtWFAcls = await DataContext.ProjectRequestTypeWorkflowActivities.Where(rt => rt.RequestTypeID == requestType.ID).ToArrayAsync();
                DataContext.ProjectRequestTypeWorkflowActivities.RemoveRange(prtWFAcls);

                //Delete the query templates
                var templates = await DataContext.Templates.Where(t => t.RequestTypeID == requestType.ID).ToArrayAsync();
                DataContext.Templates.RemoveRange(templates);

                //Delete the request type
                DataContext.RequestTypes.Remove(requestType);

            }

            await DataContext.SaveChangesAsync();
        }

        /// <summary>
        /// Inserts or updates the requesttype, template, and associated models
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> Save(UpdateRequestTypeRequestDTO details)
        {
            if (details.Queries == null || !details.Queries.Any())
            {
                throw new ArgumentException("Templates cannot be null for the RequestType, at least one template is required.", "details.Templates");
            }

            if (details.RequestType == null)
            {
                throw new ArgumentException("RequestType cannot be null.", "details.RequestType");
            }

            RequestType requestType = null;            

            if (details.RequestType.ID.HasValue)
            {
                //check for edit permission
                if ((await DataContext.HasGrantedPermissions<RequestType>(Identity, details.RequestType.ID.Value, PermissionIdentifiers.RequestTypes.Edit)).Any() == false)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Permission required to update the specified request type.");
                }

                requestType = await DataContext.RequestTypes.FindAsync(details.RequestType.ID.Value);
                await DataContext.LoadCollection(requestType, rt => rt.Queries);
                await DataContext.LoadCollection(requestType, rt => rt.Terms);

            } else
            {
                if (!(await DataContext.HasPermissions<RequestType>(Identity, PermissionIdentifiers.Portal.CreateRequestTypes)))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Permission required to create request types.");
                }

                requestType = DataContext.RequestTypes.Add(new RequestType { ProcessorID = QueryComposerModelProcessorID, PackageIdentifier = QueryComposerPackageIdentifier, Models = new HashSet<RequestTypeModel>(), Terms = new HashSet<RequestTypeTerm>() });
            }

            //update request type
            details.RequestType.Apply(DataContext.Entry(requestType));

            //update the request type permissions
            var updatedAcls = new List<AclRequestType>();
            var dbAcls = await DataContext.RequestTypeAcls.Where(a => a.RequestTypeID == requestType.ID).ToArrayAsync();

            foreach(var acl in dbAcls)
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
                updatedAcls.Add(DataContext.RequestTypeAcls.Add(new AclRequestType
                {
                    Allowed = acl.Allowed.Value,
                    Overridden = true,
                    PermissionID = acl.PermissionID,
                    RequestTypeID = requestType.ID,
                    SecurityGroupID = acl.SecurityGroupID
                }));
            }            
            
            //update the queries for the requesttype 
            foreach(var templateDTO in details.Queries)
            {
                Template template = null;
                if (templateDTO.ID.HasValue)
                {
                    template = requestType.Queries.FirstOrDefault(t => t.ID == templateDTO.ID.Value);
                }
                
                if(template == null)
                {
                    template = DataContext.Templates.Add(new Template
                    {
                        CreatedByID = Identity.ID,
                        CreatedOn = DateTime.UtcNow,
                        Type = DTO.Enums.TemplateTypes.Request,
                        RequestType = requestType,
                        RequestTypeID = requestType.ID
                    });
                    requestType.Queries.Add(template);
                    templateDTO.ID = template.ID;
                }

                //these properties should never change after creation
                templateDTO.CreatedByID = template.CreatedByID;
                templateDTO.CreatedOn = template.CreatedOn;
                templateDTO.Type = DTO.Enums.TemplateTypes.Request;
                templateDTO.RequestTypeID = requestType.ID;

                templateDTO.Apply(DataContext.Entry(template));

                if (string.IsNullOrEmpty(template.Name))
                {
                    template.Name = "Cohort " + (template.Order + 1);
                }
            }

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
                foreach(var template in details.Queries)
                {
                    //clear out the existing hidden terms for the template
                    DataContext.TemplateTerms.RemoveRange(DataContext.TemplateTerms.Where(t => t.TemplateID == template.ID.Value));

                    //add any specified term to the hidden terms collection
                    foreach(var ht in details.NotAllowedTerms.Where(nt => nt.TemplateID == template.ID.Value).ToArray())
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
                                      Queries = rt.Queries.OrderBy(q => q.Order).Select(q => new TemplateDTO {
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


            var response = Request.CreateResponse(HttpStatusCode.Accepted, responseDetail);
            return response;
        }

        /// <summary>
        /// Updates Models for Request Types
        /// </summary>
        /// <param name="details">Enumerable RequestTypeModelDTOs</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateModels(UpdateRequestTypeModelsDTO details)
        {
            if (details.DataModels != null && details.DataModels.Any())
            {
                var datamodels = await DataContext.RequestTypeDataModels.Where(rtm => rtm.RequestTypeID == details.RequestTypeID).ToArrayAsync();

                //models to add
                foreach (var modelID in details.DataModels.Where(id => datamodels.Any(mm => mm.DataModelID == id) == false))
                {
                    DataContext.RequestTypeDataModels.Add(new RequestTypeModel { RequestTypeID = details.RequestTypeID, DataModelID = modelID });
                }

                //models to delete
                foreach (var rtm in datamodels.Where(m => details.DataModels.Any(id => m.DataModelID == id) == false))
                {
                    DataContext.RequestTypeDataModels.Remove(rtm);
                }

                await DataContext.SaveChangesAsync();
            }
            else
            {
                //remove all model associations - the requesttype is model agnostic       
                await DataContext.Database.ExecuteSqlCommandAsync("DELETE FROM RequestTypeModels WHERE RequestTypeID = @requestTypeID", new System.Data.SqlClient.SqlParameter("@requestTypeID", details.RequestTypeID));
            }

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Returns the terms for a specific request type.
        /// </summary>
        /// <param name="requestTypeID"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<RequestTypeModelDTO> GetRequestTypeModels(Guid requestTypeID)
        {
            var requestTypeModels = DataContext.Secure<RequestType>(Identity).Where(rt => rt.ID == requestTypeID).SelectMany(rt => rt.Models).Map<RequestTypeModel, RequestTypeModelDTO>();
            return requestTypeModels;
        }

        /// <summary>
        /// Returns the terms for a specific request type.
        /// </summary>
        /// <param name="requestTypeID"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<RequestTypeTermDTO> GetRequestTypeTerms(Guid requestTypeID)
        {
            var results = DataContext.RequestTypeTerms.Where(t => t.RequestTypeID == requestTypeID).Map<RequestTypeTerm, RequestTypeTermDTO>();
            return results;
        }

        /// <summary>
        /// Gets all of the applicable terms for the specified RequestType. This will be either the explictly limited terms, terms limited by specified model, or no terms indicating the composer should show all terms.
        /// </summary>
        /// <param name="id">The ID of the request type.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IQueryable<RequestTypeTermDTO>> GetFilteredTerms(Guid id)
        {
            RequestTypeTermDTO[] requestTypeTerms = DataContext.RequestTypes.Where(rt => rt.ID == id).SelectMany(rt => rt.Terms).Map<RequestTypeTerm, RequestTypeTermDTO>().ToArray();
            if (requestTypeTerms.Length == 0)
            {
                var q = (from rtm in DataContext.RequestTypeDataModels
                         where rtm.RequestTypeID == id
                         from dmst in DataContext.DataModelSupportedTerms
                         where dmst.DataModelID == rtm.DataModelID
                         select new RequestTypeTermDTO {
                             TermID = dmst.Term.ID,
                             Description = dmst.Term.Description,
                             OID = dmst.Term.OID,
                             ReferenceUrl = dmst.Term.ReferenceUrl,
                             Term = dmst.Term.Name,
                             RequestTypeID = rtm.RequestTypeID
                         }).DistinctBy(t => t.TermID);


                requestTypeTerms = q.ToArray();

                //filter even further if the query template has a sub-query type specified
                var adapterDetailTerms = await (from t in DataContext.Templates
                                                join da in DataContext.DataAdapterDetailTerms on t.QueryType.Value equals da.QueryType
                                                where t.RequestTypeID == id && t.QueryType.HasValue
                                                select da.TermID).ToArrayAsync();

                if (adapterDetailTerms.Length > 0)
                {
                    if (requestTypeTerms.Length > 0)
                    {
                        requestTypeTerms = requestTypeTerms.Where(t => adapterDetailTerms.Contains(t.TermID)).ToArray();
                    }
                    else {
                        requestTypeTerms = await DataContext.Terms
                            .Where(t => adapterDetailTerms.Contains(t.ID))
                            .Select(t => new RequestTypeTermDTO {
                                TermID = t.ID,
                                Description = t.Description,
                                OID = t.OID,
                                ReferenceUrl = t.ReferenceUrl,
                                Term = t.Name,
                                RequestTypeID = id
                            }).ToArrayAsync();
                    }
                }
            }

            return requestTypeTerms.AsQueryable();
        }

        /// <summary>
        /// Gets all the valid terms based on the models, adapterdetail specified. Not specific to a particular requesttype or template.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<RequestTypeTermDTO> GetTermsFilteredBy()
        {
            /**
             * Due to this action needing to be able to accept and IEnumerable query parameter it cannot support OData specifications for client side query.
             * OData does not support specifing more than one query parameter with the same key.
             * Changing the return result to an IEnumerable indicates that it should not go through the OData action filters, and will not error if the key value exists more than once.
             * */
            var queryString = Request.GetQueryNameValuePairs();

            if (queryString.Any(k => string.Equals(k.Key, "termID", StringComparison.OrdinalIgnoreCase)))
            {
                List<Guid> termID = new List<Guid>();
                foreach (var m in queryString.Where(k => string.Equals(k.Key, "termID", StringComparison.OrdinalIgnoreCase)))
                {
                    Guid id;
                    if (Guid.TryParse(m.Value, out id))
                    {
                        termID.Add(id);
                    }
                }

                if (termID.Any())
                {
                    return DataContext.Terms.Where(t => termID.Contains(t.ID)).Select(t => new RequestTypeTermDTO
                    {
                        TermID = t.ID,
                        Description = t.Description,
                        OID = t.OID,
                        ReferenceUrl = t.ReferenceUrl,
                        Term = t.Name,
                        RequestTypeID = Guid.Empty
                    }).AsQueryable();
                }
                    
            }

            var q = DataContext.DataModelSupportedTerms.Where(dmst => dmst.DataModel.QueryComposer);

            if (queryString.Any(k => string.Equals(k.Key, "modelID", StringComparison.OrdinalIgnoreCase)))
            {
                List<Guid> modelID = new List<Guid>();
                foreach (var m in queryString.Where(k => string.Equals(k.Key, "modelID", StringComparison.OrdinalIgnoreCase))) {
                    Guid id;
                    if(Guid.TryParse(m.Value, out id))
                    {
                        modelID.Add(id);
                    }
                }

                if(modelID.Any())
                    q = q.Where(dmst => modelID.Contains(dmst.DataModelID));
            }

            if (queryString.Any(k => string.Equals(k.Key, "adapterDetail", StringComparison.OrdinalIgnoreCase)))
            {
                DTO.Enums.QueryComposerQueryTypes adapterDetail;
                if (Enum.TryParse<DTO.Enums.QueryComposerQueryTypes>(queryString.Where(k => string.Equals(k.Key, "adapterDetail", StringComparison.OrdinalIgnoreCase)).Select(k => k.Value).First(), out adapterDetail)){
                    q = q.Where(dmst => DataContext.DataAdapterDetailTerms.Where(dat => dat.QueryType == adapterDetail).Any(dat => dat.TermID == dmst.TermID));
                }
            }  
            
            if(queryString.Any(k => string.Equals(k.Key, "templateID", StringComparison.OrdinalIgnoreCase)))
            {
                Guid templateID;
                if(Guid.TryParse(queryString.Where(k => string.Equals("templateID", k.Key, StringComparison.OrdinalIgnoreCase)).Select(k => k.Value).FirstOrDefault(), out templateID))
                {
                    q = q.Where(dmst => (from t in DataContext.Templates join ad in DataContext.DataAdapterDetailTerms on t.QueryType.Value equals ad.QueryType where t.QueryType.HasValue && t.ID == templateID && dmst.TermID == ad.TermID select ad.TermID).Any());
                }
            }          

            var r = q.Select(dmst =>
                                    new RequestTypeTermDTO
                                    {
                                        TermID = dmst.Term.ID,
                                        Description = dmst.Term.Description,
                                        OID = dmst.Term.OID,
                                        ReferenceUrl = dmst.Term.ReferenceUrl,
                                        Term = dmst.Term.Name,
                                        RequestTypeID = Guid.Empty
                                    }).DistinctBy(t => t.TermID);

            return r;
        }

        /// <summary>
        /// Gets the available terms based on the specified RestrictToTerms, Adapter, AdapterDetail, and/or TemplateID
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> TermsByAdapterAndDetail([FromBody] AvailableTermsRequestDTO details)
        {
            //initially filter only terms that are registered to Adapters that are supported by QueryComposer.
            var query = DataContext.Terms.Where(t => t.DataModels.Any(m => m.DataModel.QueryComposer));
            if(details.Adapters != null && details.Adapters.Any())
            {
                //filter by specified Adapters
                query = query.Where(t => t.DataModels.Any(m => details.Adapters.Contains(m.DataModelID)));
            }
            if (details.QueryType.HasValue)
            {
                //filter by the data adapter detail
                query = query.Where(t => DataContext.DataAdapterDetailTerms.Where(d => d.QueryType == details.QueryType.Value).Any(d => d.TermID == t.ID));
            }

            var result = await query.Select(t => t.ID).ToArrayAsync();

            var response = Request.CreateResponse<Guid[]>(HttpStatusCode.OK, result);

            return response;
        }

        /// <summary>
        /// Updates terms associated with a request type.
        /// </summary>
        /// <param name="updateInfo">Update information defining the request type to update, and the terms that should be associated with the request type.</param>
        /// <returns>Http Accepted or Bad Request</returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateRequestTypeTerms(UpdateRequestTypeTermsDTO updateInfo)
        {
            if (updateInfo == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Update information is required.");
            }

            var requestType = DataContext.RequestTypes.Include(rt => rt.Terms).Where(rt => rt.ID == updateInfo.RequestTypeID).FirstOrDefault();

            if (requestType == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to determine the RequestType to update.");
            }

            if (!(await DataContext.HasPermissions<RequestType>(Identity, updateInfo.RequestTypeID, PermissionIdentifiers.RequestTypes.Edit))) 
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to manage the specified RequestType.");
            }

            if (updateInfo.Terms == null || !updateInfo.Terms.Any())
            {
                requestType.Terms.Clear();
            }
            else
            {
                var termsToDelete = requestType.Terms.Where(t => !updateInfo.Terms.Any(x => x == t.TermID)).ToArray();
                foreach (var term in termsToDelete)
                {
                    requestType.Terms.Remove(term);
                }

                foreach (var termID in updateInfo.Terms.Where(i => !requestType.Terms.Any(t => t.TermID == i)))
                {
                    requestType.Terms.Add(new RequestTypeTerm { RequestTypeID = requestType.ID, TermID = termID });
                }
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }
    }
}
