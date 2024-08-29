using Serilog;
using PopMedNet.DMCS.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.SignalR;

namespace PopMedNet.DMCS.Code.Sync
{
    public class RouteSyncPersister : IDisposable
    {
        readonly ModelContext db;
        readonly PMNApi.PMNApiClient _pmn;
        readonly ILogger _logger;
        readonly IOptions<DMCSConfiguration> _config;
        readonly IHubContext<RequestHub> _hub;
        private bool disposedValue;

        public RouteSyncPersister(ModelContext db, IOptions<DMCSConfiguration> config, ILogger logger, IHubContext<RequestHub> hub)
        {
            this.db = db;
            this._config = config;
            this._pmn = new PMNApi.PMNApiClient(config.Value.PopMedNet);
            this._logger = logger;
            this._hub = hub;
        }

        /// <summary>
        /// Adds or updates an existing request and all it's routes and support objects.
        /// </summary>
        /// <param name="dto">The request to add or update.</param>
        /// <returns></returns>
        public async Task Sync(PMNApi.PMNDto.RoutesForRequestsDTO dto)
        {
            List<Models.Notifications.ChangeNotification<Models.RoutingDTO>> routingChangeNotifications = new List<Models.Notifications.ChangeNotification<Models.RoutingDTO>>();

            var pmnRequest = dto.Requests.FirstOrDefault();

            _logger.Debug("For request: {MSRequestID} (ID: {ID}) beginning sync", pmnRequest);

            using (var trx = await db.Database.BeginTransactionAsync())
            {

                var requestComparer = new RequestMetadataEqualityComparer();

                var dmcsRequest = await db.Requests.FindAsync(pmnRequest.ID);
                if (dmcsRequest != null && !requestComparer.Equals(pmnRequest, dmcsRequest))
                {
                    _logger.Debug("For request: {MSRequestID} (ID: {ID}), request found - updating metadata", dmcsRequest);
                    //update the dmcs request
                    dmcsRequest.ActivityDescription = pmnRequest.ActivityDescription;
                    dmcsRequest.AdditionalInstructions = pmnRequest.AdditionalInstructions;
                    dmcsRequest.ReportAggregationLevel = pmnRequest.ReportAggregationLevel;
                    dmcsRequest.Description = pmnRequest.Description;
                    dmcsRequest.Identifier = pmnRequest.Identifier;
                    dmcsRequest.MSRequestID = pmnRequest.MSRequestID;
                    dmcsRequest.Name = pmnRequest.Name;
                    dmcsRequest.PhiDisclosureLevel = pmnRequest.PhiDisclosureLevel;
                    dmcsRequest.Project = pmnRequest.Project;
                    dmcsRequest.PurposeOfUse = pmnRequest.PurposeOfUse;
                    dmcsRequest.RequestorCenter = pmnRequest.RequestorCenter;
                    dmcsRequest.RequestType = pmnRequest.RequestType;
                    dmcsRequest.SubmittedOn = pmnRequest.SubmittedOn;
                    dmcsRequest.SubmittedBy = pmnRequest.SubmittedBy;
                    dmcsRequest.PmnTimestamp = pmnRequest.Timestamp;
                    dmcsRequest.WorkPlanType = pmnRequest.WorkPlanType;

                    dmcsRequest.TaskOrder = pmnRequest.TaskOrder;
                    dmcsRequest.Activity = pmnRequest.Activity;
                    dmcsRequest.ActivityProject = pmnRequest.ActivityProject;

                    dmcsRequest.SourceTaskOrder = pmnRequest.SourceTaskOrder;
                    dmcsRequest.SourceActivity = pmnRequest.SourceActivity;
                    dmcsRequest.SourceActivityProject = pmnRequest.SourceActivityProject;

                    await db.SaveChangesAsync();

                }
                else if (dmcsRequest == null)
                {
                    _logger.Debug("For request: {MSRequestID} (ID: {ID}), request not found - adding to local DMCS", pmnRequest);
                    //add the pmn request to the local DMCS
                    dmcsRequest = (await db.Requests.AddAsync(new Request
                    {
                        ID = pmnRequest.ID,
                        ActivityDescription = pmnRequest.ActivityDescription,
                        AdditionalInstructions = pmnRequest.AdditionalInstructions,
                        ReportAggregationLevel = pmnRequest.ReportAggregationLevel,
                        Description = pmnRequest.Description,
                        Identifier = pmnRequest.Identifier,
                        MSRequestID = pmnRequest.MSRequestID,
                        Name = pmnRequest.Name,
                        PhiDisclosureLevel = pmnRequest.PhiDisclosureLevel,
                        Project = pmnRequest.Project,
                        PurposeOfUse = pmnRequest.PurposeOfUse,
                        RequestorCenter = pmnRequest.RequestorCenter,
                        RequestType = pmnRequest.RequestType,
                        SubmittedOn = pmnRequest.SubmittedOn,
                        SubmittedBy = pmnRequest.SubmittedBy,
                        PmnTimestamp = pmnRequest.Timestamp,
                        WorkPlanType = pmnRequest.WorkPlanType,

                        TaskOrder = pmnRequest.TaskOrder,
                        Activity = pmnRequest.Activity,
                        ActivityProject = pmnRequest.ActivityProject,

                        SourceTaskOrder = pmnRequest.SourceTaskOrder,
                        SourceActivity = pmnRequest.SourceActivity,
                        SourceActivityProject = pmnRequest.SourceActivityProject,
                    })).Entity;

                    await db.SaveChangesAsync();
                }

                //sync the routes
                var dataMartIDs = dto.Routes.Select(rt => rt.DataMartID).ToArray();
                var dataMartDetails = await db.DataMarts.Where(dm => dataMartIDs.Contains(dm.ID)).Select(dm => new { dm.ID, dm.Name }).ToDictionaryAsync(k => k.ID, v => v.Name);

                var dmcsRoutes = await db.RequestDataMarts.Include(rdm => rdm.DataMart).Where(rdm => rdm.RequestID == dmcsRequest.ID).ToListAsync();

                //remove any routes not existing in the pmn data
                var routesToRemove = dmcsRoutes.Where(rdm => dto.Routes.All(rdm1 => rdm1.ID != rdm.ID)).ToArray();
                db.RequestDataMarts.RemoveRange(routesToRemove);
                dmcsRoutes.RemoveAll(rt => routesToRemove.Contains(rt));
                routingChangeNotifications.AddRange(routesToRemove.Select(rdm => new Models.Notifications.ChangeNotification<Models.RoutingDTO>(Models.Notifications.ChangeType.Deleted, new Models.RoutingDTO { ID = rdm.ID })));

                //add any routes missing
                var routesToAdd = dto.Routes.Where(rt => dmcsRoutes.All(rdm => rdm.ID != rt.ID)).ToArray();
                
                foreach (var rt in routesToAdd)
                {
                    var newRoute = new RequestDataMart
                    {
                        ID = rt.ID,
                        DataMartID = rt.DataMartID,
                        DueDate = rt.DueDate,
                        ModelID = rt.ModelID,
                        ModelText = rt.ModelText,
                        PmnTimestamp = rt.Timestamp,
                        Priority = rt.Priority,
                        RejectReason = rt.RejectReason,
                        RequestID = rt.RequestID,
                        RoutingType = rt.RoutingType,
                        Status = rt.Status,
                        UpdatedOn = rt.UpdatedOn
                    };

                    await db.RequestDataMarts.AddAsync(newRoute);
                    dmcsRoutes.Add(newRoute);
                    
                    string datamartName = "";
                    dataMartDetails.TryGetValue(rt.DataMartID, out datamartName);
                    
                    routingChangeNotifications.Add(
                        new Models.Notifications.ChangeNotification<Models.RoutingDTO>(
                            Models.Notifications.ChangeType.Added,
                            new Models.RoutingDTO {
                                ID = rt.ID,
                                DataMartName = datamartName,
                                DataModel = rt.ModelText,
                                DueDate = rt.DueDate,
                                MSRequestID = dmcsRequest.MSRequestID,
                                 Priority = rt.Priority,
                                 Project= dmcsRequest.Project,
                                 RequestDate = dmcsRequest.SubmittedOn,
                                 RequestIdentifier = dmcsRequest.Identifier,
                                 RequestName = dmcsRequest.Name,
                                 RequestType = dmcsRequest.RequestType,
                                 Status = rt.Status,
                                 SubmittedBy = dmcsRequest.SubmittedBy
                            }
                            ));
                }

                //update routes
                var routeEntityComparer = new Data.Model.RequestDataMartEqualityComparer();
                var routesToUpdate = dmcsRoutes.Where(rt =>
                {
                    var pmnRoute = dto.Routes.FirstOrDefault(rdm => rdm.ID == rt.ID);
                    return pmnRoute != null && (rt.PmnTimestamp == null || !routeEntityComparer.Equals(rt, pmnRoute));
                }).ToArray();

                foreach (var rt in routesToUpdate)
                {
                    var pmnRoute = dto.Routes.FirstOrDefault(rdm => rdm.ID == rt.ID);
                    rt.DataMartID = pmnRoute.DataMartID;
                    rt.DueDate = pmnRoute.DueDate;
                    rt.ModelID = pmnRoute.ModelID;
                    rt.ModelText = pmnRoute.ModelText;
                    rt.PmnTimestamp = pmnRoute.Timestamp;
                    rt.Priority = pmnRoute.Priority;
                    rt.RejectReason = pmnRoute.RejectReason;
                    rt.RequestID = pmnRoute.RequestID;
                    rt.RoutingType = pmnRoute.RoutingType;
                    rt.Status = pmnRoute.Status;
                    rt.UpdatedOn = pmnRoute.UpdatedOn;

                    string datamartName = "";
                    dataMartDetails.TryGetValue(rt.DataMartID, out datamartName);

                    routingChangeNotifications.Add(
                        new Models.Notifications.ChangeNotification<Models.RoutingDTO>(
                            Models.Notifications.ChangeType.Updated,
                            new Models.RoutingDTO
                            {
                                ID = rt.ID,
                                DataMartName = datamartName,
                                DataModel = rt.ModelText,
                                DueDate = rt.DueDate,
                                MSRequestID = dmcsRequest.MSRequestID,
                                Priority = rt.Priority,
                                Project = dmcsRequest.Project,
                                RequestDate = dmcsRequest.SubmittedOn,
                                RequestIdentifier = dmcsRequest.Identifier,
                                RequestName = dmcsRequest.Name,
                                RequestType = dmcsRequest.RequestType,
                                Status = rt.Status,
                                SubmittedBy = dmcsRequest.SubmittedBy
                            }
                            ));

                }

                await db.SaveChangesAsync();

                

                //sync the responses
                var dmcsResponses = await db.Responses.Where(rsp => rsp.RequestDataMart.RequestID == dmcsRequest.ID).ToListAsync();
                //remove any responses that do not existing in the pmn data
                var responsesToRemove = dmcsResponses.Where(rsp => dto.Responses.All(rdm1 => rdm1.ID != rsp.ID)).ToArray();
                db.Responses.RemoveRange(responsesToRemove);
                dmcsResponses.RemoveAll(rt => responsesToRemove.Contains(rt));

                //add any responses missing
                var responsesToAdd = dto.Responses.Where(rt => dmcsResponses.All(rsp => rsp.ID != rt.ID)).ToArray();
                foreach (var rt in responsesToAdd)
                {
                    var newResponse = new Response
                    {
                        ID = rt.ID,
                        Count = rt.Count,
                        PmnTimestamp = rt.Timestamp,
                        RequestDataMartID = rt.RequestDataMartID,
                        RespondedBy = rt.RespondedBy,
                        ResponseMessage = rt.ResponseMessage,
                        ResponseTime = rt.ResponseTime
                    };
                    await db.Responses.AddAsync(newResponse);
                    dmcsResponses.Add(newResponse);
                }

                var responseMetadataComparer = new Data.Model.ResponseMetadataEqualityComparer();
                var responsesToUpdate = dmcsResponses.Where(rsp =>
                {
                    var pmnResponse = dto.Responses.FirstOrDefault(rrsp => rrsp.ID == rsp.ID);
                    return pmnResponse != null && (rsp.PmnTimestamp == null || !responseMetadataComparer.Equals(rsp, pmnResponse));
                }).ToArray();

                foreach (var rt in responsesToUpdate)
                {
                    var pmnResponse = dto.Responses.FirstOrDefault(rsp => rsp.ID == rt.ID);
                    rt.Count = pmnResponse.Count;
                    rt.RequestDataMartID = pmnResponse.RequestDataMartID;
                    rt.PmnTimestamp = pmnResponse.Timestamp;
                    rt.RespondedBy = pmnResponse.RespondedBy;
                    rt.ResponseMessage = pmnResponse.ResponseMessage;
                    rt.ResponseTime = pmnResponse.ResponseTime;
                }

                await db.SaveChangesAsync();

                //sync the documents

                //TODO: evict from cache if the document has been downloaded before

                //get all the documents associated to the request
                var dmcsDocuments = await (from d in db.Documents
                                           where
                                           //document is associated to the request via the requestdocument
                                           db.RequestDocuments.Any(rd => rd.RevisionSetID == d.RevisionSetID && rd.Response.RequestDataMart.RequestID == dmcsRequest.ID)
                                           select d).ToArrayAsync();

                //remove any remote request documents that does not exist in the entire documents list
                var documentsToRemove = from d in dmcsDocuments
                                        where !dto.Documents.Any(dd => dd.ID == d.ID)
                                        && d.DocumentState == Data.Enums.DocumentStates.Remote
                                        select d;

                db.Documents.RemoveRange(documentsToRemove);

                var documentMetadataComparer = new Data.Model.DocumentMetadataEqualityComparer();
                //get the documents that are referenced by requestdocuments associated with the current request
                var pmnDocuments = dto.Documents.Where(d => dto.RequestDocuments.Any(r => r.RevisionSetID == d.RevisionSetID)).ToArray();
                foreach (var pmnDocument in pmnDocuments)
                {
                    if (pmnDocument.UploadedByID.HasValue)
                    {
                        User dmcsUser = await db.Users.FindAsync(pmnDocument.UploadedByID.Value);
                        if (dmcsUser == null)
                        {
                            await db.Users.AddAsync(new User { ID = pmnDocument.UploadedByID.Value, UserName = pmnDocument.UploadedByUserName, Email = pmnDocument.UploadedByEmail });
                        }
                    }

                    Document localDocument = dmcsDocuments.FirstOrDefault(d => d.ID == pmnDocument.ID);

                    if(localDocument == null)
                    {
                        //add the document
                        var newDocument = new Document
                        {
                            ID = pmnDocument.ID,
                            ContentCreatedOn = pmnDocument.ContentCreatedOn,
                            ContentModifiedOn = pmnDocument.ContentModifiedOn,
                            CreatedOn = pmnDocument.CreatedOn,
                            ItemID = pmnDocument.ItemID,
                            Kind = pmnDocument.Kind,
                            Length = pmnDocument.Length,
                            MimeType = pmnDocument.MimeType,
                            Name = pmnDocument.Name,
                            PmnTimestamp = pmnDocument.Timestamp,
                            RevisionSetID = pmnDocument.RevisionSetID,
                            UploadedByID = pmnDocument.UploadedByID,
                            Version = pmnDocument.Version,
                            DocumentState = Data.Enums.DocumentStates.Remote
                        };

                        await db.Documents.AddAsync(newDocument);

                    }
                    else if (localDocument.PmnTimestamp == null || !documentMetadataComparer.Equals(localDocument, pmnDocument))
                    {
                        //TODO: remove document from cache if the content modified on date or length is different

                        //update the document
                        localDocument.ContentCreatedOn = pmnDocument.ContentCreatedOn;
                        localDocument.ContentModifiedOn = pmnDocument.ContentModifiedOn;
                        localDocument.CreatedOn = pmnDocument.CreatedOn;
                        localDocument.RevisionSetID = pmnDocument.RevisionSetID;
                        localDocument.ItemID = pmnDocument.ItemID;
                        localDocument.Kind = pmnDocument.Kind;
                        localDocument.Length = pmnDocument.Length;
                        localDocument.MimeType = pmnDocument.MimeType;
                        localDocument.Name = pmnDocument.Name;
                        localDocument.PmnTimestamp = pmnDocument.Timestamp;
                        localDocument.UploadedByID = pmnDocument.UploadedByID;
                        localDocument.Version = pmnDocument.Version;
                        localDocument.DocumentState = Data.Enums.DocumentStates.Remote;
                    }

                }

                await db.SaveChangesAsync();

                //update the request documents
                var dmcsRequestDocuments = await db.RequestDocuments.Where(rd => rd.Response.RequestDataMart.RequestID == dmcsRequest.ID).Select(rd => new { RequestDocument = rd, IsLocalOnly = db.Documents.Where(d => d.RevisionSetID == rd.RevisionSetID && d.DocumentState == Data.Enums.DocumentStates.Local).Any() }).ToListAsync();

                var requestDocumentsToRemove = dmcsRequestDocuments.Where(d => !d.IsLocalOnly && !dto.RequestDocuments.Any(dd => dd.ResponseID == d.RequestDocument.ResponseID && dd.RevisionSetID == d.RequestDocument.RevisionSetID)).ToArray();
                db.RequestDocuments.RemoveRange(requestDocumentsToRemove.Select(rd => rd.RequestDocument));
                dmcsRequestDocuments.RemoveAll(doc => requestDocumentsToRemove.Contains(doc));

                //add any missing  request documents
                var requestDocumentsToAdd = dto.RequestDocuments.Where(dd => !dmcsRequestDocuments.Any(rd => rd.RequestDocument.ResponseID == dd.ResponseID && rd.RequestDocument.RevisionSetID == dd.RevisionSetID)).ToArray();
                foreach (var rd in requestDocumentsToAdd)
                {
                    var newRequestDocument = new RequestDocument { ResponseID = rd.ResponseID, RevisionSetID = rd.RevisionSetID, DocumentType = rd.DocumentType };
                    await db.RequestDocuments.AddAsync(newRequestDocument);
                }

                //should not have to update any since the request document should never be altered after creation
                await db.SaveChangesAsync();

                await trx.CommitAsync();

                //broadcast sync notification
                if(routingChangeNotifications.Count > 0)
                {
                    foreach(var notification in routingChangeNotifications)
                    {
                        await _hub.Clients.Group(RequestHub.RequestListGroupName).SendAsync(RequestHub.EventIdentifiers.RequestList_RequestListUpdated, notification);
                    }
                }

            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _pmn.Dispose();
                    db.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~RouteSyncPersister()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
