using Lpp.Dns.Data;
using Lpp.Dns.Data.Query;
using Lpp.Dns.DTO.Security;
using Lpp.Objects;
using Lpp.Utilities;
using Lpp.Utilities.WebSites.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Http;
using System.Xml.Linq;
using dmc = Lpp.Dns.DTO.DataMartClient;

namespace Lpp.Dns.Api.DataMartClient
{
    /// <summary>
    /// Actions for supporting DMC client.
    /// </summary>
    /// <remarks>This controller is intentionally not included in the Lpp.Dns.NetClient generated file so that a dependency is not needed on Lpp.Dns.DTO.DataMartClient.</remarks>
    [ClientEntityIgnore]
    [OverrideAuthentication]
    [DMCAuthenticationLogging]
    public class DMCController : LppApiController<Lpp.Dns.Data.DataContext>
    {
        static DMCController()
        {
            lock (_lock)
            {
                var type = typeof(IPostProcessDocumentContent);
                PostProcessorTypes = ObjectEx.GetNonSystemAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);
            }
            
        }

        static readonly Guid LegacyModularProcessorID = new Guid("C8BC0BD9-A50D-4B9C-9A25-472827C8640A");
        static readonly Guid ModularProgramTermID = new Guid("A1AE0001-E5B4-46D2-9FAD-A3D8014FFFD8");
        static readonly Guid ModularModelID = new Guid("1B0FFD4C-3EEF-479D-A5C4-69D8BA0D0154");
        static readonly Guid DefaultQEAdapterProcessorID = new Guid("AE0DA7B0-0F73-4D06-B70B-922032B7F0EB");
        static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(DMCController));
        static readonly IEnumerable<Type> PostProcessorTypes;
        static object _lock = new object();
        /// <summary>
        /// Gets the current Utilities.Security.ApiIdentity.
        /// </summary>
        /// <returns></returns>
        protected virtual Utilities.Security.ApiIdentity GetCurrentIdentity()
        {
            return Identity;
        }

        /// <summary>
        /// Gets the profile for the authenticated user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<dmc.Profile> GetProfile()
        {
            //authorize the user and then return details about the currently logged in user
            var identity = GetCurrentIdentity();
            var user = await (from u in DataContext.Users where u.ID == identity.ID && u.Active && !u.Deleted select new dmc.Profile { Email = u.Email, FullName = u.FirstName + " " + u.LastName, OrganizationName = u.Organization.Name, Username = u.UserName }).SingleOrDefaultAsync();

            if(user == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Invalid username or password, please check credentials."));

            user.SupportsUploadV2 = WebConfigurationManager.AppSettings["AllowDMCConcurrency"] == null ? true : WebConfigurationManager.AppSettings["AllowDMCConcurrency"].ToBool();

            return user;
        }        

        /// <summary>
        /// Gets the datamarts the authenticated user is authorized to see requests for.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<dmc.DataMart> GetDataMarts()
        {
            var result = GetGrantedDataMarts().Select(d => new dmc.DataMart
            {
                ID = d.ID,
                Name = d.Name,
                UnattendedMode = (DTO.DataMartClient.Enums.UnattendedModes)(int)(d.UnattendedMode ?? DTO.Enums.UnattendedModes.NoUnattendedOperation),
                OrganizationID = d.OrganizationID,
                OrganizationName = d.Organization.Name,
                Models = d.Models.Where(m => d.AdapterID.HasValue || m.Model.RequestTypes.Select(rt => rt.RequestType.ProcessorID).Any()).Select(m => new dmc.Model
                {
                    ID = d.AdapterID.HasValue ? d.AdapterID.Value : m.ModelID,
                    ProcessorID = d.ProcessorID.HasValue ? d.ProcessorID.Value : (d.AdapterID.HasValue ? DefaultQEAdapterProcessorID : m.Model.RequestTypes.Where(rt => rt.RequestType.ProcessorID.HasValue).Select(rt => rt.RequestType.ProcessorID).FirstOrDefault() ?? Guid.Empty),
                    PackageIdentifier = (d.AdapterID.HasValue ? DataContext.RequestTypes.Where(rt => (d.ProcessorID.HasValue && d.ProcessorID.Value == rt.ProcessorID.Value) || rt.ProcessorID.Value == DefaultQEAdapterProcessorID).Select(rt => rt.PackageIdentifier).FirstOrDefault() : m.Model.RequestTypes.Select(rt => rt.RequestType.PackageIdentifier).FirstOrDefault()) ?? string.Empty,
                    Name = d.AdapterID.HasValue ? d.Adapter.Name : m.Model.Name,
                    Properties = d.Models.Where(im => im.ModelID == m.ModelID).Select(im => im.Properties).FirstOrDefault()
                })
            });

            return result;
        }

        

        /// <summary>
        /// Gets the current request list.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        /// <remarks>Note: To and From dates should be specified in UTC.</remarks>
        [HttpPost]
        public async Task<dmc.RequestList> GetRequestList(DTO.DataMartClient.Criteria.RequestListCriteria criteria)
        {
            try
            {
                if (criteria == null)
                {
                    criteria = new dmc.Criteria.RequestListCriteria
                    {
                        StartIndex = 0
                    };
                }
                var rd = from r in DataContext.Requests
                         where r.SubmittedOn != null && (criteria.FromDate == null || r.SubmittedOn >= criteria.FromDate) && (criteria.ToDate == null || r.SubmittedOn <= criteria.ToDate)
                         && r.RequestType.ProcessorID.HasValue
                         from d in r.DataMarts
                         from i in d.Responses
                         let isCurrent = i.Count == d.Responses.Max(rr => rr.Count)
                         where isCurrent && d.Status != DTO.Enums.RoutingStatus.Draft && d.Status != DTO.Enums.RoutingStatus.RequestRejected && d.Status != DTO.Enums.RoutingStatus.AwaitingRequestApproval && d.Status != DTO.Enums.RoutingStatus.Canceled && d.Status > 0
                         select new { r, d, i, OrganizationID = d.DataMart.OrganizationID };

                if (criteria.FilterByDataMartIDs != null && criteria.FilterByDataMartIDs.Any())
                {
                    rd = rd.Where(x => criteria.FilterByDataMartIDs.Contains(x.d.DataMartID));
                }

                if (criteria.FilterByStatus != null && criteria.FilterByStatus.Any())
                {
                    rd = rd.Where(x => criteria.FilterByStatus.Any(s => (int)s == (int)x.d.Status));
                }

                var res = from gd in DataContext.DataMarts.SelectMany(x => x.Projects.Select(p => new { DataMartID = x.ID, p.ProjectID })).GroupBy(k => new { ProjectID = k.ProjectID, DataMartID = k.DataMartID })
                          join dm in rd on gd.Key equals new { ProjectID = dm.r.ProjectID, DataMartID = dm.d.DataMartID }
                          let r = dm.r
                          let d = dm.d
                          let i = dm.i
                          let permissionID = PermissionIdentifiers.DataMartInProject.SeeRequests.ID
                          let userID = Identity.ID
                          let globalAcls = DataContext.GlobalAcls.Where(x => x.SecurityGroup.Users.Any(y => y.UserID == userID) && x.PermissionID == permissionID)
                          let projectAcls = DataContext.ProjectAcls.Where(x => x.SecurityGroup.Users.Any(y => y.UserID == userID) && x.PermissionID == permissionID && x.ProjectID == gd.Key.ProjectID)
                          let datamartAcls = DataContext.DataMartAcls.Where(x => x.SecurityGroup.Users.Any(y => y.UserID == userID) && x.PermissionID == permissionID && x.DataMartID == gd.Key.DataMartID)
                          let projectDataMartAcls = DataContext.ProjectDataMartAcls.Where(x => x.SecurityGroup.Users.Any(y => y.UserID == userID) && x.PermissionID == permissionID && x.ProjectID == gd.Key.ProjectID && x.DataMartID == gd.Key.DataMartID)
                          let organizationAcls = DataContext.OrganizationAcls.Where(org => org.SecurityGroup.Users.Any(sg => sg.UserID == userID) && org.PermissionID == permissionID && org.OrganizationID == dm.OrganizationID)
                          where (globalAcls.Any(x => x.Allowed) || projectAcls.Any(x => x.Allowed) || datamartAcls.Any(x => x.Allowed) || projectDataMartAcls.Any(x => x.Allowed) || organizationAcls.Any(x => x.Allowed)) &&
                          (globalAcls.All(x => x.Allowed) && projectAcls.All(x => x.Allowed) && datamartAcls.All(x => x.Allowed) && projectDataMartAcls.All(x => x.Allowed) && organizationAcls.All(x => x.Allowed))
                          select new
                          {
                              r.ID,
                              r.Identifier,
                              r.MSRequestID,
                              r.Name,
                              r.RequestTypeID,
                              Priority = d.Priority,
                              DueDate = d.DueDate ?? r.DueDate,
                              RequestTypeName = r.RequestType.Name,
                              RequestTypeModelName = d.DataMart.AdapterID.HasValue ? d.DataMart.Adapter.Name : r.RequestType.Models.Select(m => m.DataModel.Name).FirstOrDefault(),
                              RequestTypePackageIdentifier = r.RequestType.PackageIdentifier,
                              r.AdapterPackageVersion,
                              d.Status,
                              r.SubmittedOn,
                              d.DataMartID,
                              DataMartName = d.DataMart.Name,
                              CreatedByUsername = r.CreatedBy.UserName,
                              RespondedByUsername = i.RespondedBy.UserName,
                              i.ResponseTime,
                              //PMNDEV-987: Submitted time is based on the time the routing is submitted, falling back to request submitted time if not available, field is non-nullable so it defaults to '0001-01-01 00:00:00.0000000'
                              RoutingSubmittedOn = i.SubmittedOn < new DateTime(1900, 1, 1) ? (r.SubmittedOn ?? DateTime.UtcNow) : i.SubmittedOn,
                              ProjectName = r.Project.Name,
                              AllowUnattendedProcessing = (from rt in DataContext.RequestTypes
                                                           let datamartRequestTypesFilter = DataContext.DataMartRequestTypeAcls.Where(a => a.RequestTypeID == rt.ID && a.SecurityGroup.Users.Any(u => u.UserID == r.CreatedByID) && a.Permission >= DTO.Enums.RequestTypePermissions.Automatic && a.DataMartID == d.DataMartID)
                                                           let projectDataMartRequestTypesFilter = DataContext.ProjectDataMartRequestTypeAcls.Where(a => a.RequestTypeID == rt.ID && a.SecurityGroup.Users.Any(u => u.UserID == r.CreatedByID) && a.Permission >= DTO.Enums.RequestTypePermissions.Automatic && a.DataMartID == d.DataMartID && a.ProjectID == r.ProjectID)
                                                           let projectRequestTypesFilter = DataContext.ProjectRequestTypeAcls.Where(a => a.RequestTypeID == rt.ID && a.SecurityGroup.Users.Any(u => u.UserID == r.CreatedByID) && a.Permission >= DTO.Enums.RequestTypePermissions.Automatic && a.ProjectID == r.ProjectID)
                                                           where rt.ID == r.RequestTypeID
                                                           && (datamartRequestTypesFilter.Any() || projectDataMartRequestTypesFilter.Any() || projectRequestTypesFilter.Any())
                                                           select rt).Any(),
                              ResponseID = i.ID
                          };

                var totalCount = await res.CountAsync();

                var effectiveSortColumn = criteria.SortColumn ?? dmc.RequestSortColumn.RequestTime;

                var sortHelper = SortIf(res, effectiveSortColumn, criteria.SortAscending);

                res =
                    sortHelper.sort(dmc.RequestSortColumn.RequestId, r => r.ID) ??
                    sortHelper.sort(dmc.RequestSortColumn.RequestName, r => r.Name) ??
                    sortHelper.sort(dmc.RequestSortColumn.RequestPriority, r => r.Priority) ??
                    sortHelper.sort(dmc.RequestSortColumn.RequestType, r => r.RequestTypeID) ??
                    sortHelper.sort(dmc.RequestSortColumn.RequestModelType, r => r.RequestTypeID) ??
                    sortHelper.sort(dmc.RequestSortColumn.RequestDueDate, r => r.DueDate) ??
                    sortHelper.sort(dmc.RequestSortColumn.RequestStatus, r => r.Status) ??
                    sortHelper.sort(dmc.RequestSortColumn.RequestTime, r => r.RoutingSubmittedOn) ??
                    sortHelper.sort(dmc.RequestSortColumn.MSRequestID, r => r.MSRequestID) ??
                    sortHelper.sort(dmc.RequestSortColumn.DataMartName, r => r.DataMartName) ??
                    sortHelper.sort(dmc.RequestSortColumn.CreatedByUsername, r => r.CreatedByUsername) ??
                    sortHelper.sort(dmc.RequestSortColumn.ResponseTime, r => r.ResponseTime) ??
                    sortHelper.sort(dmc.RequestSortColumn.RespondedByUsername, r => r.RespondedByUsername) ??
                    sortHelper.sort(dmc.RequestSortColumn.ProjectName, r => r.ProjectName) ??
                    res;

                if (criteria.StartIndex.HasValue && criteria.StartIndex.Value >= 0)
                    res = res.Skip(criteria.StartIndex.Value);

                if (criteria.MaxCount.HasValue && criteria.MaxCount.Value > 0)
                    res = res.Take(criteria.MaxCount.Value);

                var result = new dmc.RequestList
                {
                    Segment = Enumerable.Empty<dmc.RequestListRow>(),
                    StartIndex = criteria.StartIndex ?? 0,
                    TotalCount = totalCount,
                    SortedByColumn = effectiveSortColumn,
                    SortedAscending = !DefaultDescendingSort.Contains(effectiveSortColumn)
                };

                if (totalCount > 0)
                {
                    result.Segment = await res
                        .Select(r => new dmc.RequestListRow
                        {
                            ID = r.ID,
                            Identifier = r.Identifier,
                            MSRequestID = r.MSRequestID,
                            AllowUnattendedProcessing = r.AllowUnattendedProcessing,
                            Name = r.Name,
                            Type = r.RequestTypeName,
                            RequestTypePackageIdentifier = r.RequestTypePackageIdentifier,
                            AdapterPackageVersion = r.AdapterPackageVersion,
                            ModelName = r.RequestTypeModelName,
                            Priority = (DTO.DataMartClient.Enums.Priorities)(int?)r.Priority,
                            DueDate = r.DueDate,
                            RoutingStatus = (int)r.Status,
                            //PMNDEV-987: Submitted time is based on the time the routing is submitted
                            Time = r.RoutingSubmittedOn,
                            DataMartID = r.DataMartID,
                            DataMartName = r.DataMartName,
                            CreatedBy = r.CreatedByUsername,
                            RespondedBy = r.RespondedByUsername,
                            ResponseTime = r.ResponseTime,
                            ProjectName = r.ProjectName,
                            Status = (DTO.DataMartClient.Enums.DMCRoutingStatus)((int)r.Status),
                            ResponseID = r.ResponseID
                        })
                        .ToListAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }       

        /// <summary>
        /// Gets the details for the specified requests.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<dmc.Request>> GetRequests(DTO.DataMartClient.Criteria.RequestCriteria criteria)
        {

            if (criteria.ID == null || !criteria.ID.Any())
                return Enumerable.Empty<dmc.Request>();

            var query = (from r in DataContext.Secure<Request>(Identity)
                         let datamartID = criteria.DatamartID
                         where criteria.ID.Contains(r.ID)
                         select new
                         {
                             CreatedOn = r.CreatedOn,
                             Activity = r.Activity != null && r.Activity.TaskLevel == 3 && r.Activity.ParentActivityID.HasValue ? r.Activity.ParentActivity.Name : r.Activity != null && r.Activity.TaskLevel == 2 ? r.Activity.Name : "Not Selected",
                             ActivityProject = r.Activity != null && r.Activity.TaskLevel == 3 ? r.Activity.Name : "Not Selected",
                             TaskOrder = r.Activity.TaskLevel == 3 && r.Activity.ParentActivityID.HasValue && r.Activity.ParentActivity.ParentActivityID.HasValue ? r.Activity.ParentActivity.ParentActivity.Name : r.Activity != null && r.Activity.TaskLevel == 2 && r.Activity.ParentActivityID.HasValue ? r.Activity.ParentActivity.Name : r.Activity != null ? r.Activity.Name : "Not Selected",
                             SourceActivity = r.SourceActivity != null ? r.SourceActivity.Name : "Not Selected",
                             SourceActivityProject = r.SourceActivityProject != null ? r.SourceActivityProject.Name : "Not Selected",
                             SourceTaskOrder = r.SourceTaskOrder != null ? r.SourceTaskOrder.Name : "Not Selected",
                             ActivityDescription = r.ActivityDescription,
                             AdapterPackageVersion = r.AdapterPackageVersion,
                             AdditionalInstructions = r.AdditionalInstructions,
                             Author = new
                             {
                                 FullName = r.CreatedBy.FirstName + " " + r.CreatedBy.LastName,
                                 OrganizationName = r.CreatedBy.Organization.Name,
                                 Username = r.CreatedBy.UserName,
                                 Email = r.CreatedBy.Email
                             },
                             Description = r.Description,
                             DueDate = r.DataMarts.Where(rdm => rdm.DueDate.HasValue && datamartID.HasValue && rdm.DataMartID == datamartID.Value).Select(rdm => rdm.DueDate).FirstOrDefault() ?? r.DueDate,
                             ID = r.ID,
                             Identifier = r.Identifier,
                             MSRequestID = r.MSRequestID,
                             IsMetadataRequest = r.RequestType.MetaData,
                             //need to use the adapter ID for the specified datamart first, then fallback to first available adapter ID
                             ModelID = r.DataMarts.Where(rdm => datamartID.HasValue && rdm.DataMartID == datamartID.Value && rdm.DataMart.AdapterID.HasValue).Select(rdm => rdm.DataMart.AdapterID).FirstOrDefault() ?? r.DataMarts.Where(rdm => rdm.DataMart.AdapterID.HasValue).Select(rdm => rdm.DataMart.AdapterID).FirstOrDefault() ?? r.RequestType.Models.Select(model => model.DataModelID).FirstOrDefault(),
                             Name = r.Name,
                             PhiDisclosureLevel = r.PhiDisclosureLevel,
                             Priority = (dmc.Enums.Priorities)(int?)r.Priority,
                             Project = new dmc.Project
                             {
                                 Name = r.Project.Name,
                                 Description = r.Project.Description,
                                 StartDate = r.Project.StartDate,
                                 EndDate = r.Project.EndDate
                             },
                             PurposeOfUse = r.PurposeOfUse,
                             RequestorCenter = r.RequesterCenter.Name,
                             RequestTypeID = r.RequestTypeID,
                             RequestTypeName = r.RequestType.Name,
                             RequestTypePackageIdentifier = r.RequestType.PackageIdentifier,
                             //legacy requests do not have a workflowID
                             //to check this, we get the value
                             WorkFlowActivityID = r.WorkFlowActivityID,
                             WorkPlanType = r.WorkplanType.Name,
                             ReportAggregationLevel = r.ReportAggregationLevel.Name,
                             Routings = (from rdm in r.DataMarts
                                         let uploadResultsPermissionID = PermissionIdentifiers.DataMartInProject.UploadResults.ID
                                         let holdRequestPermissionID = PermissionIdentifiers.DataMartInProject.HoldRequest.ID
                                         let rejectRequestPermissionID = PermissionIdentifiers.DataMartInProject.RejectRequest.ID
                                         let modifyResultsPermissionID = PermissionIdentifiers.DataMartInProject.ModifyResults.ID
                                         let viewAttachmentsPermissionID = PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ViewAttachments.ID
                                         let modifyAttachmentsPermissionID = PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ModifyAttachments.ID
                                         let seeRequestPermissionID = PermissionIdentifiers.DataMartInProject.SeeRequests.ID
                                         let userID = Identity.ID
                                         let globalAcls = DataContext.GlobalAcls.Where(x => x.SecurityGroup.Users.Any(y => y.UserID == userID) && x.PermissionID == seeRequestPermissionID)
                                         let projectAcls = DataContext.ProjectAcls.Where(x => x.SecurityGroup.Users.Any(y => y.UserID == userID) && x.PermissionID == seeRequestPermissionID && x.ProjectID == rdm.Request.ProjectID)
                                         let datamartAcls = DataContext.DataMartAcls.Where(x => x.SecurityGroup.Users.Any(y => y.UserID == userID) && x.PermissionID == seeRequestPermissionID && x.DataMartID == rdm.DataMartID)
                                         let projectDataMartAcls = DataContext.ProjectDataMartAcls.Where(x => x.SecurityGroup.Users.Any(y => y.UserID == userID) && x.PermissionID == seeRequestPermissionID && x.ProjectID == rdm.Request.ProjectID && x.DataMartID == rdm.DataMartID)
                                         let organizationAcls = DataContext.OrganizationAcls.Where(org => org.SecurityGroup.Users.Any(sg => sg.UserID == userID) && org.PermissionID == seeRequestPermissionID && org.OrganizationID == rdm.DataMart.OrganizationID)
                                         let acls = DataContext.DataMartRights(userID, r.ID, rdm.DataMartID)
                                         where (criteria.DatamartID.HasValue ? rdm.DataMartID == criteria.DatamartID : true) &&
                                         (globalAcls.Any(x => x.Allowed) || projectAcls.Any(x => x.Allowed) || datamartAcls.Any(x => x.Allowed) || projectDataMartAcls.Any(x => x.Allowed) || organizationAcls.Any(x => x.Allowed)) &&
                                         (globalAcls.All(x => x.Allowed) && projectAcls.All(x => x.Allowed) && datamartAcls.All(x => x.Allowed) && projectDataMartAcls.All(x => x.Allowed) && organizationAcls.All(x => x.Allowed))
                                         select new
                                         {
                                             DataMartID = rdm.DataMartID,
                                             AllowUnattendedProcessing = DataContext.DataMartAllowUnattendedProcessing(Identity.ID, r.RequestTypeID, r.ProjectID, rdm.DataMartID).Any() && DataContext.DataMartAllowUnattendedProcessing(Identity.ID, r.RequestTypeID, r.ProjectID, rdm.DataMartID).All(a => a.Permission == 2),
                                             Properties = rdm.Properties,
                                             Status = (dmc.Enums.DMCRoutingStatus)(int?)rdm.Status,
                                             canRun = acls.Where(a => a.PermissionID == uploadResultsPermissionID).Any() && acls.Where(a => a.PermissionID == uploadResultsPermissionID).All(a => a.Allowed) ? dmc.RequestRights.Run : 0,

                                             canHold = acls.Where(a => a.PermissionID == holdRequestPermissionID).Any() && acls.Where(a => a.PermissionID == holdRequestPermissionID).All(a => a.Allowed) ? dmc.RequestRights.Hold : 0,

                                             canReject = acls.Where(a => a.PermissionID == rejectRequestPermissionID).Any() && acls.Where(a => a.PermissionID == rejectRequestPermissionID).All(a => a.Allowed) ? dmc.RequestRights.Reject : 0,

                                             canModifyResults = acls.Where(a => a.PermissionID == modifyResultsPermissionID).Any() && acls.Where(a => a.PermissionID == modifyResultsPermissionID).All(a => a.Allowed) ? dmc.RequestRights.ModifyResults : 0,

                                             canViewAttachments = acls.Where(a => a.PermissionID == viewAttachmentsPermissionID).Any() && acls.Where(a => a.PermissionID == viewAttachmentsPermissionID).All(a => a.Allowed) ? dmc.RequestRights.ViewAttachments : 0,

                                             canModifyAttachments = acls.Where(a => a.PermissionID == modifyAttachmentsPermissionID).Any() && acls.Where(a => a.PermissionID == modifyAttachmentsPermissionID).All(a => a.Allowed) ? dmc.RequestRights.ModifyAttachments : 0
                                         }),
                             Responses = r.DataMarts.Where(rdm => rdm.Responses.Any()).Select(rdm => rdm.Responses.FirstOrDefault(response => !DataContext.Responses.Any(oReponse => oReponse.RequestDataMartID == response.RequestDataMartID && oReponse.Count > response.Count))).Select(response => new
                             {
                                 ResponseID = response.ID,
                                 CreatedOn = response.ResponseTime ?? response.SubmittedOn,
                                 DataMartID = response.RequestDataMart.DataMartID,
                                 Email = response.RespondedBy.Email,
                                 FullName = response.RespondedBy.FirstName + " " + response.RespondedBy.LastName,
                                 OrganizationName = response.RespondedBy.Organization.Name,
                                 Username = response.RespondedBy.UserName
                             })
                         });


            var requests = await query.ToArrayAsync();

            if (requests.Length == 0)
            {
                return Enumerable.Empty<dmc.Request>();
            }

            Guid[] requestIDs = requests.Select(r => r.ID).ToArray();
            Guid[] dataMartIDs = requests.SelectMany(x => x.Responses).Select(x => x.DataMartID).ToArray();
            var docs = await DataContext.Documents.Where(dx => requestIDs.Contains(dx.ItemID)).Select(d => new DocumentDetailsDTO { RequestID = d.ItemID, ID = d.ID, IsViewable = d.Viewable, Kind = d.Kind, MimeType = d.MimeType, Name = d.FileName, Size = d.Length, DocumentType = DTO.Enums.RequestDocumentType.Input })
                .Union(
                from rd in DataContext.RequestDocuments
                join rsp in DataContext.Responses on rd.ResponseID equals rsp.ID
                join rdm in DataContext.RequestDataMarts on rsp.RequestDataMartID equals rdm.ID
                where requestIDs.Contains(rdm.RequestID)
                && dataMartIDs.Contains(rdm.DataMartID)
                && rsp.Count == rdm.Responses.Max(rr => rr.Count)
                && (rd.DocumentType == DTO.Enums.RequestDocumentType.Input || rd.DocumentType == DTO.Enums.RequestDocumentType.AttachmentInput)
                from doc in DataContext.Documents
                where doc == DataContext.Documents.Where(d => d.RevisionSetID == rd.RevisionSetID).OrderByDescending(o => o.MajorVersion).ThenByDescending(o => o.MinorVersion).ThenByDescending(o => o.BuildVersion).ThenByDescending(o => o.RevisionVersion).FirstOrDefault()
                select new DocumentDetailsDTO {
                    RequestID = rdm.RequestID,
                    ID = doc.ID,
                    IsViewable = doc.Viewable,
                    Kind = doc.Kind,
                    MimeType = doc.MimeType,
                    Name = doc.FileName,
                    Size = doc.Length,
                    DocumentType = rd.DocumentType
                }
                ).ToArrayAsync();

            var results = (from r in requests
                           select new dmc.Request
                           {
                              Activity = r.Activity,
                              ActivityDescription = r.ActivityDescription,
                              ActivityProject = r.ActivityProject,
                              AdapterPackageVersion = r.AdapterPackageVersion,
                              AdditionalInstructions = r.AdditionalInstructions,
                              Author = new dmc.Profile
                              {
                                  Email = r.Author.Email,
                                  FullName = r.Author.FullName,
                                  OrganizationName = r.Author.OrganizationName,
                                  Username = r.Author.Username
                              },
                              CreatedOn = r.CreatedOn,
                              Description = r.WorkFlowActivityID.HasValue ? r.Description : FormatRequestDescription(r.Description),    //edit in line breaks for legacy requests
                              Documents = docs.Where(d => d.DocumentType == DTO.Enums.RequestDocumentType.Input && d.RequestID == r.ID).Select(d => new dmc.DocumentWithID
                              {
                                  ID = d.ID,
                                  Document = new dmc.Document
                                  {
                                      IsViewable = d.IsViewable,
                                      Kind = d.Kind,
                                      MimeType = d.MimeType,
                                      Name = d.Name,
                                      Size = d.Size
                                  }
                              }).OrderBy(d => d.Document.Name).ToArray(),
                              Attachments = docs.Where(d => d.DocumentType == DTO.Enums.RequestDocumentType.AttachmentInput && d.RequestID == r.ID).Select(d => new dmc.DocumentWithID
                              {
                                  ID = d.ID,
                                  Document = new dmc.Document
                                  {
                                      IsViewable = d.IsViewable,
                                      Kind = d.Kind,
                                      MimeType = d.MimeType,
                                      Name = d.Name,
                                      Size = d.Size
                                  }
                              }).OrderBy(d => d.Document.Name).ToArray(),
                              DueDate = r.DueDate,
                              ID = r.ID,
                              Identifier = r.Identifier,
                              MSRequestID = r.MSRequestID,
                              IsMetadataRequest = r.IsMetadataRequest,
                              ModelID = r.ModelID,
                              Name = r.Name,
                              PhiDisclosureLevel = r.PhiDisclosureLevel,
                              Priority = r.Priority,
                              Project = r.Project,
                              PurposeOfUse = TranslatePurposeOfUse(r.PurposeOfUse),
                              RequestorCenter = r.RequestorCenter,
                              RequestTypeID = r.RequestTypeID,
                              RequestTypeName = r.RequestTypeName,
                              RequestTypePackageIdentifier = r.RequestTypePackageIdentifier,
                              Responses = r.Responses.Select(response => new dmc.Response
                              {
                                  ResponseID = response.ResponseID,
                                  Author = new dmc.Profile
                                  {
                                      Email = response.Email,
                                      FullName = response.FullName,
                                      OrganizationName = response.OrganizationName,
                                      Username = response.Username
                                  },
                                  CreatedOn = response.CreatedOn,
                                  DataMartID = response.DataMartID
                              }).ToArray(),
                              Routings = r.Routings.Select(routing => new dmc.RequestRouting
                              {
                                  AllowUnattendedProcessing = routing.AllowUnattendedProcessing,
                                  DataMartID = routing.DataMartID,
                                  Rights = routing.canHold | routing.canReject | routing.canRun | routing.canModifyResults | routing.canViewAttachments | routing.canModifyAttachments,
                                  Status = routing.Status,
                                  Properties = routing.Properties == null ? null : (
                                            from root in ParseXml(routing.Properties)
                                            from e in root.Elements()
                                            let key = (string)e.Attribute("Key")
                                            where !string.IsNullOrEmpty(key)
                                            select new dmc.RoutingProperty { Name = key, Value = e.Value }
                                        ).ToArray()
                              }).ToArray(),
                              //KT changes
                              SourceActivity = r.SourceActivity,
                              SourceActivityProject = r.SourceActivityProject,
                              SourceTaskOrder = r.SourceTaskOrder,
                              //end KT changes
                              TaskOrder = r.TaskOrder,
                              WorkPlanType = r.WorkPlanType,
                              ReportAggregationLevel = r.ReportAggregationLevel,
                          }).ToArray();

            return results;
        }

        private string FormatRequestDescription(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            return input.Replace(Environment.NewLine, "<br/>");
        }

        string TranslatePurposeOfUse(string value)
        {
            switch (value)
            {
                case "CLINTRCH":
                    return "Clinical Trial Research";
                case "HMARKT":
                    return "Healthcare Marketing";
                case "HOPERAT":
                    return "Healthcare Operations";
                case "HPAYMT":
                    return "Healthcare Payment";
                case "HRESCH":
                    return "Healthcare Research";
                case "OBSRCH":
                    return "Observational Research";
                case "PATRQT":
                    return "Patient Requested";
                case "PTR":
                    return "Prep-to-Research";
                case "PUBHLTH":
                    return "Public Health";
                case "QA":
                    return "Quality Assurance";
                case "TREAT":
                    return "Treatment";
            }
            return value;
        }

        /// <summary>
        /// Gets a chunck of the specified document.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetDocumentChunk(Guid ID, int offset, int size)
        {
            var document = await DataContext.Documents.Where(d => d.ID == ID).Select(d => new { d.ID, d.Length }).FirstOrDefaultAsync();
            if (document == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Document not found for the specified ID.");
            }

            HttpResponseMessage response = this.Request != null ? this.Request.CreateResponse(HttpStatusCode.OK) : new HttpResponseMessage(HttpStatusCode.OK);
            if (offset < document.Length)
            {
                byte[] buffer = await LocalDiskCache.Instance.ReadChunk(DataContext, document.ID, offset, Math.Min(size, Convert.ToInt32(document.Length - offset))).ConfigureAwait(false);

                response.Content = new ByteArrayContent(buffer);
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentRange = new System.Net.Http.Headers.ContentRangeHeaderValue(offset, offset + buffer.Length);
                response.Content.Headers.ContentLength = buffer.Length;
            }
            else
            {
                Logger.Warn($"Offset is greater than or equal to the length of the document, no content returned. DocumentID:{ ID }, offset:{ offset }, requested size: { size }, document length:{ document.Length }");
            }

            return response;
        }

        /// <summary>
        /// Creates/saves new response documents, document content is posted in PostResponseDocumentChunk().
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Guid[]> PostResponseDocuments(dmc.Criteria.PostResponseDocumentsData data)
        {
            bool canUpload = await CheckUploadPermission(data.RequestID, data.DataMartID);
            if (canUpload == false)
            {
                throw new HttpResponseException(this.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Not authorizad to upload results."));
            }

            Response response = await DataContext.Responses
                                                 .Where(rsp => rsp.RequestDataMart.DataMartID == data.DataMartID && rsp.RequestDataMart.RequestID == data.RequestID)
                                                 .OrderByDescending(rsp => rsp.Count)
                                                 .FirstOrDefaultAsync();

            if (response == null)
            {
                RequestDataMart datamart = await DataContext.RequestDataMarts.FirstOrDefaultAsync(dm => dm.DataMartID == data.DataMartID && dm.RequestID == data.RequestID);
                if (datamart == null)
                {
                    throw new HttpResponseException(this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Unable to determine the routing for the response."));
                }

                //create the response
                response = datamart.AddResponse(GetCurrentIdentity().ID);
                response.SubmittedOn = DateTime.UtcNow;
            }

            //As Per PMNDEV-4303: Previous documents are not wiped but versioned.
            //The DMC is responsible for restricting re-upload of results for Request Types other than Modular Program and File Distribution.
            //DataContext.Documents.RemoveRange(DataContext.Documents.Where(d => d.ItemID == response.ID));
            var requestWorkflowID = await DataContext.Requests.Where(r => r.ID == data.RequestID).Select(x => x.WorkFlowActivityID).FirstOrDefaultAsync();

            Guid[] documentIDs = new Guid[0];
            string[] documentNames = (data.Documents ?? Enumerable.Empty<dmc.Document>()).Select(d => d.Name).ToArray();

            if (DataContext.Documents.Any(d => d.ItemID == response.ID && documentNames.Contains(d.FileName)))
            {
                //Get existing documents with revision 
                var existingDocuments = await DataContext.Documents.Where(d => d.ItemID == response.ID && documentNames.Contains(d.FileName)).Select(d => new { d.FileName, d.MajorVersion, d.MinorVersion, d.BuildVersion, d.RevisionVersion, d.ID, d.RevisionSetID }).ToArrayAsync();
                var existingRequestDocuments = await DataContext.RequestDocuments.AsNoTracking().Where(rd => rd.ResponseID == response.ID).ToListAsync();

                //As Per PMNDEV-4303: We need to version the documents that already exist in the database.

                //Add documents that don't need to be versioned here first.
                var newDocuments = (data.Documents ?? Enumerable.Empty<dmc.Document>()).Where(d => existingDocuments.Any(ed => ed.FileName == d.Name) == false).Select(d => {
                    
                    var doc = new Document
                    {
                        ItemID = response.ID,
                        FileName = d.Name,
                        Name = d.Name,
                        ParentDocumentID = null,
                        MimeType = d.MimeType,
                        Length = d.Size,
                        Viewable = d.IsViewable,
                        Kind = d.Kind,
                        Description = string.Empty,
                        UploadedByID = Identity.ID
                    };

                    doc.RevisionSetID = doc.ID;

                    return doc;
                  }).ToArray();

                List<RequestDocument> newRequestDocuments = new List<RequestDocument>();

                if (requestWorkflowID.HasValue)
                {
                    foreach (var doc in newDocuments)
                    {
                        if (!existingRequestDocuments.Where(x => x.RevisionSetID == doc.RevisionSetID.Value).Any() && !newRequestDocuments.Where(x => x.RevisionSetID == doc.RevisionSetID.Value).Any())
                        {
                            newRequestDocuments.Add(new RequestDocument { ResponseID = doc.ItemID, RevisionSetID = doc.RevisionSetID.Value, DocumentType = DTO.Enums.RequestDocumentType.Output });
                            
                        }
                    }
                }

                //Now add the ones that need to be versioned.
                var revisionedDocuments = (data.Documents ?? Enumerable.Empty<dmc.Document>()).Where(d => existingDocuments.Any(ed => ed.FileName == d.Name)).Select(d => {
                    //get the most recent parent document
                    var pDoc = existingDocuments.Where(ed => ed.FileName == d.Name).OrderByDescending(ed => ed.MajorVersion).ThenByDescending(ed => ed.MinorVersion).ThenByDescending(ed => ed.BuildVersion).ThenByDescending(ed => ed.RevisionVersion).First();

                    var doc = new Document
                    {
                        ItemID = response.ID,
                        FileName = d.Name,
                        Name = d.Name,
                        Description = string.Empty,
                        UploadedByID = Identity.ID,
                        MimeType = d.MimeType,
                        Length = d.Size,
                        Viewable = d.IsViewable,
                        Kind = d.Kind,
                        //Set the RevisionSetID to the first version's ID.
                        RevisionSetID = pDoc.RevisionSetID,
                        ParentDocumentID = pDoc.ID,
                        MajorVersion = pDoc.MajorVersion,
                        MinorVersion = pDoc.MinorVersion,
                        BuildVersion = pDoc.BuildVersion,
                        RevisionVersion = pDoc.RevisionVersion + 1
                    };

                    return doc;
                }).ToArray();

                if (requestWorkflowID.HasValue)
                {
                    foreach (var doc in revisionedDocuments)
                    {
                        if (!existingRequestDocuments.Where(x => x.RevisionSetID == doc.RevisionSetID.Value).Any() && !newRequestDocuments.Where(x => x.RevisionSetID == doc.RevisionSetID.Value).Any())
                        {
                            newRequestDocuments.Add(new RequestDocument { ResponseID = doc.ItemID, RevisionSetID = doc.RevisionSetID.Value, DocumentType = DTO.Enums.RequestDocumentType.Output });
                        }
                    }
                }

                DataContext.Documents.AddRange(newDocuments);
                DataContext.Documents.AddRange(revisionedDocuments);

                if (requestWorkflowID.HasValue)
                {
                    DataContext.RequestDocuments.AddRange(newRequestDocuments); 
                }
                
                documentIDs = newDocuments.Select(d => d.ID).Union(revisionedDocuments.Select(d => d.ID)).ToArray();
            }
            else
            {
                //save add the new documents
                var newDocuments = (data.Documents ?? Enumerable.Empty<dmc.Document>()).Select(d =>
                {
                    var doc = new Document
                    {
                        ItemID = response.ID,
                        FileName = d.Name,
                        Name = d.Name,
                        ParentDocumentID = null,
                        MimeType = d.MimeType,
                        Length = d.Size,
                        Viewable = d.IsViewable,
                        Kind = d.Kind,
                        Description = string.Empty,
                        UploadedByID = Identity.ID
                    };

                    doc.RevisionSetID = doc.ID;
                    return doc;
                }).ToArray();

                DataContext.Documents.AddRange(newDocuments);
                

                if (requestWorkflowID.HasValue)
                {
                    DataContext.RequestDocuments.AddRange(newDocuments.Select(nd => new RequestDocument { ResponseID = response.ID, RevisionSetID = nd.RevisionSetID.Value, DocumentType = DTO.Enums.RequestDocumentType.Output }).ToArray());
                }
                
                documentIDs = newDocuments.Select(d => d.ID).ToArray();
            }

            DataContext.SaveChanges();

            return documentIDs;
        }

        

        /// <summary>
        /// Saves the actual document content for the document specified.
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<HttpResponseMessage> PostResponseDocumentChunk([FromUri]Guid documentID, [Utilities.WebSites.Attributes.RawBody]IEnumerable<byte> data)
        {            
            
            var details = await (from d in DataContext.Documents
                                 let requestDataMart = DataContext.Responses.Where(r => r.ID == d.ItemID).Select(r => r.RequestDataMart).FirstOrDefault()
                                 where d.ID == documentID 
                                 select new
                                 {
                                     Document = d,
                                     RequestID = requestDataMart.RequestID,
                                     DataMartID = requestDataMart.DataMartID
                                 }
                                    ).FirstOrDefaultAsync();

            if (details == null)
            {
                Logger.Debug($"Document metadata not found for document ID:{ documentID }");
                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Document not found.");
            }

            Action<string> logDebug = (string message) => {
                Logger.Debug($"[RequestID: { details.RequestID }, DataMartID: { details.DataMartID }, UserID: { Identity.ID }] {message}");
            };

            Action<string, Exception> logError = (string message, Exception ex) => {
                Logger.Error($"[RequestID: { details.RequestID }, DataMartID: { details.DataMartID }, UserID: { Identity.ID }] {message}", ex);
            };

            bool canUpload = await CheckUploadPermission(details.RequestID, details.DataMartID);
            if (!canUpload)
            {
                logDebug("User not authorized to upload results.");
                return this.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Not authorized to upload results.");
            }

            string uploadPath = System.Web.Configuration.WebConfigurationManager.AppSettings["DocumentsUploadFolder"] ?? string.Empty;
            if (string.IsNullOrEmpty(uploadPath))
                uploadPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Uploads/");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            string cacheDocumentName = $"{details.Document.ID.ToString("D")}.part";
            string cacheDocumentPath = Path.Combine(uploadPath, cacheDocumentName);

            logDebug($"Begin caching document \"{ cacheDocumentName }\".");


            using (var fs = File.Open(cacheDocumentPath, FileMode.Append))
            {
                foreach (var b in data)
                {
                    fs.WriteByte(b);
                }

                fs.Flush();

                if (fs.Length < details.Document.Length)
                {
                    logDebug($"Finished appending to cached document \"{ cacheDocumentName }\", {fs.Length}/{details.Document.Length} written to cache.");

                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
            }

            logDebug($"Begin writing document \"{ cacheDocumentName }\" to the database.");
            using (var fs = new FileStream(cacheDocumentPath, FileMode.Open, FileAccess.Read))
            {
                if (DataContext.Database.Connection.State != ConnectionState.Open)
                    DataContext.Database.Connection.Open();

                using (var conn = (SqlConnection)DataContext.Database.Connection)
                {
                    int chunkSize = 524288000;
                    int bytesRead;
                    byte[] buffer = fs.Length < chunkSize ? new byte[fs.Length] : new byte[chunkSize];
                    while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                            using (var cmd = new SqlCommand("UPDATE Documents SET Data = CASE WHEN Data IS NULL THEN @newData ELSE Data + @newData END, [Length] = DATALENGTH(CASE WHEN Data IS NULL THEN @newData ELSE Data + @newData END), ContentModifiedOn = GETUTCDATE(), ContentCreatedOn = CASE WHEN ContentCreatedOn IS NULL THEN GETUTCDATE() ELSE ContentCreatedOn END WHERE ID = @ID", conn))
                            {
                            try
                            {
                                cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = details.Document.ID;
                                cmd.Parameters.Add("@newData", SqlDbType.VarBinary, bytesRead).Value = buffer;

                                cmd.CommandType = CommandType.Text;
                                cmd.CommandTimeout = 900;
                                await cmd.ExecuteNonQueryAsync();
                                bytesRead -= chunkSize;                               

                            }
                            catch (Exception ex)
                            {
                                logError($"Error saving document ID: { details.Document.ID } to the database.", ex);
                                throw;
                            }
                        }

                    }
                    conn.Close();
                    conn.Dispose();
                }

                fs.Close();
                fs.Dispose();
            }

            var docMetadata = new dmc.Criteria.DocumentMetadata { 
                ID = details.Document.ID, 
                DataMartID = details.DataMartID, 
                CurrentChunkIndex = 0,
                IsViewable = details.Document.Viewable,
                Kind = details.Document.Kind,
                MimeType = details.Document.MimeType,
                Name = details.Document.Name,
                RequestID = details.RequestID,
                Size = details.Document.Length
            };
            Hangfire.BackgroundJob.Enqueue(() => PostProcessDocument(Identity.ID, docMetadata, cacheDocumentPath));

            return this.Request != null ? this.Request.CreateResponse(HttpStatusCode.OK) :  new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> PostDocumentChunk()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Content must be mime multipart.");
            }

            string uploadPath = System.Web.Configuration.WebConfigurationManager.AppSettings["DocumentsUploadFolder"] ?? string.Empty;

            if (string.IsNullOrEmpty(uploadPath))
                uploadPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Uploads/");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            ChunkedMultipartFormDMCProvider provider;
            try
            {
                provider = new ChunkedMultipartFormDMCProvider(uploadPath, HttpContext.Current.Request, DataContext, Identity);
            }
            catch (Exception ex)
            {
                Logger.Error("Error processing document.", ex);
                throw new HttpResponseException(this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error processing document chunk."));
            }

            bool canUpload = await CheckUploadPermission(provider.DocumentMetadata.RequestID, provider.DocumentMetadata.DataMartID);

            if (canUpload == false)
            {
                Logger.Debug($"[RequestID: { provider.DocumentMetadata.RequestID }, DataMartID: { provider.DocumentMetadata.DataMartID }, UserID: { Identity.ID }] User not authorized to upload results.");

                throw new HttpResponseException(this.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Not authorized to upload results."));
            }

            try
            {
                var o = await Request.Content.ReadAsMultipartAsync(provider);

                if (o.IsFinalChunk == false)
                {
                    return Request.CreateResponse(HttpStatusCode.Created, o.DocumentMetadata.ID);
                }

                await o.CombineChunks();

                await o.SetUpDocumentInDatabase();

                await o.StreamDocumentToDatabase();

                Hangfire.BackgroundJob.Enqueue(() => PostProcessDocument(Identity.ID, o.DocumentMetadata, o.CombindedTempDocumentFileName));

                return Request.CreateResponse(HttpStatusCode.Created, o.DocumentMetadata.ID);
            }
            catch (Exception ex)
            {
                Logger.Error($"[RequestID: { provider.DocumentMetadata.RequestID }, DataMartID: { provider.DocumentMetadata.DataMartID }, UserID: { Identity.ID }] Error uploading document: { Newtonsoft.Json.JsonConvert.SerializeObject(provider.DocumentMetadata) } ", ex);

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occured while trying to upload the document content."));
            }
        }

        /// <summary>
        /// Executes document post processing for all the registered post processors.
        /// </summary>
        /// <param name="identityID">The ID of the user that initiated the document upload.</param>
        /// <param name="documentMetadata">The document metadata.</param>
        /// <param name="cachedDocumentFileName">The full path and name of the temp cached file.</param>
        /// <returns></returns>
        public async Task PostProcessDocument(Guid identityID, dmc.Criteria.DocumentMetadata documentMetadata, string cachedDocumentFileName)
        {
            using (var db = new DataContext())
            {
                Data.Document postProcessDocument = await db.Documents.FindAsync(documentMetadata.ID);
                
                if (!File.Exists(cachedDocumentFileName))
                {
                    using(var writer = File.OpenWrite(cachedDocumentFileName))
                    using(var documentStream = postProcessDocument.GetStream(db))
                    {
                        documentStream.CopyTo(writer);
                        writer.Flush();
                    }
                }
                
                foreach(var item in PostProcessorTypes)
                {
                    try
                    {
                        IPostProcessDocumentContent postProcess = Activator.CreateInstance(item) as IPostProcessDocumentContent;
                        postProcess.Initialize(db, Path.GetDirectoryName(cachedDocumentFileName));
                        await postProcess.ExecuteAsync(postProcessDocument, Path.GetFileName(cachedDocumentFileName));
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"[RequestID: { documentMetadata.RequestID }, DataMartID: { documentMetadata.DataMartID }, UserID: { identityID }] Error post-processing document: { Newtonsoft.Json.JsonConvert.SerializeObject(documentMetadata) } ", ex);
                    }
                }
            }

            try
            {
                File.Delete(cachedDocumentFileName);
            }
            catch { }
        }



        /// <summary>
        /// Sets the status of the specified request.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<HttpResponseMessage> SetRequestStatus(dmc.Criteria.SetRequestStatusData data)
        {
            var processor = new DMCRoutingStatusProcessor(DataContext, Identity);
            var result = await processor.UpdateStatusAsync(data);

            if(result.StatusCode == HttpStatusCode.Forbidden || result.StatusCode == HttpStatusCode.NotFound)
            {
                return Request.CreateErrorResponse(result.StatusCode, result.Message);
            }

            return Request.CreateResponse(result.StatusCode, result.Message);
        }

        IQueryable<DataMart> GetGrantedDataMarts()
        {
            var identity = GetCurrentIdentity();
            var query = new GetDataMartsQuery(DataContext);
            return query.Execute(identity).Where(dm => dm.Deleted == false && DataContext.Users.Any(u => u.ID == identity.ID && u.Active && !u.Deleted));
        }

        async Task<bool> CheckUploadPermission(Guid requestID, Guid datamartID)
        {
            return await CheckPermission(requestID, datamartID, PermissionIdentifiers.DataMartInProject.UploadResults, GetCurrentIdentity());
        }

        async Task<bool> CheckHasSkipApprovalPermission(Guid requestID, Guid datamartID)
        {
            var i = DataContext.Requests.Where(r => r.ID == requestID).Select(r => new { ID = r.CreatedBy.ID, r.CreatedBy.UserName, Name = (r.CreatedBy.FirstName + " " + r.CreatedBy.LastName).Trim(), r.CreatedBy.OrganizationID }).Single();

            return await CheckPermission(requestID, datamartID, PermissionIdentifiers.DataMartInProject.SkipResponseApproval, new Utilities.Security.ApiIdentity(i.ID, i.UserName, i.Name, i.OrganizationID));
        }

        async Task<bool> CheckPermission(Guid requestID, Guid datamartID, PermissionDefinition permission, Utilities.Security.ApiIdentity identity)
        {
            var filter = new ExtendedQuery
            {
                DataMarts = a => a.DataMartID == datamartID && a.DataMart.Requests.Any(r => r.RequestID == requestID),
                Projects = a => a.Project.Requests.Any(r => r.ID == requestID) && a.Project.DataMarts.Any(dm => dm.DataMartID == datamartID && dm.DataMart.Requests.Any(r => r.RequestID == requestID)),
                ProjectDataMarts = a => a.DataMartID == datamartID && a.Project.Requests.Any(r => r.ID == requestID) && a.DataMart.Requests.Any(r => r.RequestID == requestID),
                Organizations = a => a.Organization.DataMarts.Any(dm => dm.ID == datamartID && dm.Requests.Any(dr => dr.RequestID == requestID))
            };

            var permissions = await DataContext.HasGrantedPermissions<Request>(identity, requestID, filter, permission);

            return permissions.Any();
        }

        static readonly ISet<dmc.RequestSortColumn> DefaultDescendingSort = new HashSet<dmc.RequestSortColumn>(new[] { 
            dmc.RequestSortColumn.RequestTime, 
            dmc.RequestSortColumn.RequestPriority 
        });

        class SortHelper<T>
        {
            public dmc.RequestSortColumn Actual { get; set; }
            public IQueryable<T> Source { get; set; }
            public bool EffectiveAscending { get; set; }

            public IOrderedQueryable<T> sort<U>(dmc.RequestSortColumn expected, Expression<Func<T, U>> sortExpression)
            {
                if (Actual != expected) return null;
                return EffectiveAscending ? Source.OrderBy(sortExpression) : Source.OrderByDescending(sortExpression);
            }
        }

        static SortHelper<T> SortIf<T>(IQueryable<T> source, dmc.RequestSortColumn actual, bool? ascending)
        {
            return new SortHelper<T> { Actual = actual, EffectiveAscending = ascending ?? !DefaultDescendingSort.Contains(actual), Source = source };
        }

        static IEnumerable<XElement> ParseXml(string value)
        {
            try
            {
                return new[] { XElement.Parse(value) };
            }
            catch
            {
                return Enumerable.Empty<XElement>();
            }
        }
        
        class DocumentDetailsDTO
        {

            public DocumentDetailsDTO()
            {
                DocumentType = DTO.Enums.RequestDocumentType.Input;
            }

            public Guid RequestID { get; set; }

            public Guid ID { get; set; }

            public bool IsViewable { get; set; }

            public string Kind { get; set; }

            public string MimeType { get; set; }

            public string Name { get; set; }

            public long Size { get; set; }

            public DTO.Enums.RequestDocumentType DocumentType { get; set; }
        }
    }
}
