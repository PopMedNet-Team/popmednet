using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PopMedNet.DMCS.Code;
using PopMedNet.DMCS.Data;
using PopMedNet.DMCS.Data.Model;
using PopMedNet.DMCS.Data.Enums;
using PopMedNet.DMCS.Models;
using PopMedNet.DMCS.PMNApi;
using PopMedNet.DMCS.PMNApi.PMNDto;
using PopMedNet.DMCS.Utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PopMedNet.DMCS.Controllers
{
    [Route("api/route")]
    [ApiController]
    [Authorize]
    public class RouteController : BaseApiController
    {
        readonly IOptions<DMCSConfiguration> config;

        public RouteController(Data.Model.ModelContext modelDb, IOptions<DMCSConfiguration> config, ILogger logger) 
            : base(modelDb, logger.ForContext("SourceContext", typeof(RouteController).FullName))
        {
            this.config = config;
        }

        
        /// <summary>
        /// Gets the details for the specified route.
        /// </summary>
        /// <param name="id">The ID of the RequestDataMart.</param>
        /// <returns></returns>
        [HttpGet, Route("{id}")]
        public async Task<RequestMetadataDTO> Get(Guid id)
        {
            var userID = GetUserID();

            Models.RoutePermissionsComponent permissions = null;
            //get the permissions for the user for this routing from PMN API.
            using (var pmn = new PMNApiClient(config.Value.PopMedNet))
            {
                permissions = await pmn.GetRoutePermissionsForUser(userID, id);
            }

            if (permissions == null || permissions.SeeRequest == false)
            {
                throw new UnauthorizedAccessException("Unauthorized exception: the user does not have permission to view the request.");
            }


            var metadata = await (from rdm in this.modelDb.RequestDataMarts.AsNoTracking()
                                  join resp in this.modelDb.Responses.AsNoTracking() on rdm.ID equals resp.RequestDataMartID
                                  join dm in this.modelDb.DataMarts.AsNoTracking() on rdm.DataMartID equals dm.ID
                                  join udm in this.modelDb.UserDataMarts.AsNoTracking() on dm.ID equals udm.DataMartID
                                  join r in this.modelDb.Requests.AsNoTracking() on rdm.RequestID equals r.ID
                                  where rdm.ID == id && udm.UserID == userID && resp.Count == rdm.Responses.Max(rr => rr.Count)
                                  select new RequestMetadataDTO
                                  {
                                      ID = rdm.ID,
                                      ResponseID = resp.ID,
                                      DataMartName = dm.Name,
                                      DataModel = rdm.ModelText,
                                      DueDate = rdm.DueDate,
                                      MSRequestID = r.MSRequestID,
                                      RequestName = r.Name,
                                      Project = r.Project,
                                      Priority = rdm.Priority,
                                      RequestDate = r.SubmittedOn,
                                      RequestType = r.RequestType,
                                      Status = rdm.Status,
                                      ResponseMessage = resp.ResponseMessage,
                                      SubmittedBy = r.SubmittedBy,
                                      RequestIdentifier = r.Identifier,
                                      LevelOfReportAggregation = r.ReportAggregationLevel,
                                      PurposeOfUse = r.PurposeOfUse,
                                      RequestID = r.ID,
                                      RequestorCenter = r.RequestorCenter,
                                      AdditionalInstructions = r.AdditionalInstructions,
                                      Description = r.Description,

                                      TaskOrder = r.TaskOrder,
                                      Activity = r.Activity,
                                      ActivityProject = r.ActivityProject,

                                      SourceTaskOrder = r.SourceTaskOrder,
                                      SourceActivity = r.SourceActivity,
                                      SourceActivityProject = r.SourceActivityProject,
                                  }).FirstOrDefaultAsync();

            metadata.Permissions = permissions;

            return metadata;
        }

        [HttpGet, Route("documents/{id}")]
        public async Task<IEnumerable<DocumentDTO>> GetDocuments(Guid id, [FromQuery] Data.Enums.DocumentType[] type)
        {
            var query = from doc in this.modelDb.Documents.AsNoTracking()
                        join reqDoc in this.modelDb.RequestDocuments.AsNoTracking() on doc.RevisionSetID equals reqDoc.RevisionSetID
                        join response in this.modelDb.Responses.AsNoTracking() on reqDoc.ResponseID equals response.ID
                        join rdm in this.modelDb.RequestDataMarts.AsNoTracking() on response.RequestDataMartID equals rdm.ID
                        where rdm.ID == id && response.Count == rdm.Responses.Max(rr => rr.Count)
                        select new DocumentDTO { ID = doc.ID, Length = doc.Length, Name = doc.Name, DocumentType = reqDoc.DocumentType, DocumentState = doc.DocumentState, Timestamp = doc.PmnTimestamp };

            if(type != null && type.Any())
            {
                query = query.Where(d => type.Contains(d.DocumentType));
            }

            query = query.OrderBy(d => d.Name);

            return await query.ToArrayAsync();
        }

        [HttpPost, Route("reject")]
        public async Task RejectRequest([FromBody] RequestStatusChange request, [FromServices] IHubContext<RequestHub> requestHub)
        {
            var userID = GetUserID();
            var requestDm = await this.modelDb.RequestDataMarts.Where(x => x.ID == request.RequestDataMartID).FirstOrDefaultAsync();
            var response = await this.modelDb.Responses.Where(x => x.RequestDataMartID == request.RequestDataMartID && x.Count == x.RequestDataMart.Responses.Max(rr => rr.Count)).FirstOrDefaultAsync();

            requestDm.Status = Data.Enums.RoutingStatus.ResponseRejectedBeforeUpload;
            requestDm.RejectReason = request.Message;
            response.ResponseMessage = request.Message;            

            using (var http = PMNApiClient.CreateForUser(config.Value, Request.Cookies))
            {
                var updateResult = await http.SetRouteStatus(new PMNApi.PMNDto.SetRequestDataMartStatusDTO { RequestDataMartID = requestDm.ID, Status = Data.Enums.RoutingStatus.ResponseRejectedBeforeUpload, Message = request.Message });
                if (updateResult.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception("Error updating status to 'Rejected'.", new Exception(updateResult.Message));
                }
            }

            await this.modelDb.SaveChangesAsync();

            var user = await modelDb.Users.FindAsync(userID);

            logger.AddResponse(response.ID).Information($"Route status has been changed to 'Rejected Before Upload' by '{ user.UserName }'");

            await AlertRequestChanged(request.RequestDataMartID, userID, requestHub);
        }

        [HttpPost, Route("hold")]
        public async Task HoldRequest([FromBody] RequestStatusChange request, [FromServices] IHubContext<RequestHub> requestHub)
        {
            var userID = GetUserID();

            var details = await (from rdm in modelDb.RequestDataMarts
                          join r in modelDb.Requests on rdm.RequestID equals r.ID
                          let currentResponse = rdm.Responses.Where(rsp => rsp == rdm.Responses.OrderByDescending(x => x.Count).FirstOrDefault()).FirstOrDefault()
                          let user = modelDb.Users.Where(u => u.ID == userID).FirstOrDefault()
                                 where rdm.ID == request.RequestDataMartID
                          select new { 
                            RequestDataMart = rdm,
                            Response = currentResponse,
                            Request = new {
                                r.ID,
                                r.Identifier,
                                r.MSRequestID,
                                r.Name
                            },
                            User = user
                          }).FirstOrDefaultAsync();

            var requestDm = details.RequestDataMart;
            requestDm.Status = Data.Enums.RoutingStatus.Hold;
            var response = details.Response;
            response.ResponseMessage = request.Message;

            //update status to Hold in PMN API first, if fails do not continue
            using (var http = PMNApiClient.CreateForUser(config.Value, Request.Cookies))
            {
                var updateResult = await http.SetRouteStatus(new PMNApi.PMNDto.SetRequestDataMartStatusDTO { RequestDataMartID = requestDm.ID, Status = Data.Enums.RoutingStatus.Hold, Message = request.Message });
                if (updateResult.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception("Error updating route status to 'Hold'.", new Exception(updateResult.Message));
                }
            }

            await this.modelDb.SaveChangesAsync();

            this.logger.AddResponse(response.ID).Information($"Route status has been changed to 'Hold' by '{ details.User.UserName }'");
            

            await AlertRequestChanged(request.RequestDataMartID, userID, requestHub);
        }

        [HttpPost, Route("remove-hold")]
        public async Task RemoveHoldRequest([FromBody] RequestStatusChange request, [FromServices] IHubContext<RequestHub> requestHub)
        {
            var userID = GetUserID();

            var details = await (from rdm in modelDb.RequestDataMarts
                                 join r in modelDb.Requests on rdm.RequestID equals r.ID
                                 let currentResponse = rdm.Responses.Where(rsp => rsp == rdm.Responses.OrderByDescending(x => x.Count).FirstOrDefault()).FirstOrDefault()
                                 let user = modelDb.Users.Where(u => u.ID == userID).FirstOrDefault()
                                 where rdm.ID == request.RequestDataMartID
                                 select new
                                 {
                                     RequestDataMart = rdm,
                                     Response = currentResponse,
                                     Request = new
                                     {
                                         r.ID,
                                         r.Identifier,
                                         r.MSRequestID,
                                         r.Name
                                     },
                                     User = user
                                 }).FirstOrDefaultAsync();

            var requestDm = details.RequestDataMart;
            var response = details.Response;

            var newStatus = Data.Enums.RoutingStatus.Submitted;
            if (modelDb.RequestDocuments.Where(rd => rd.DocumentType == Data.Enums.DocumentType.Output && rd.ResponseID == response.ID).Select(rd => rd).Count() > 0)
            {
                newStatus = Data.Enums.RoutingStatus.PendingUpload;
            }

            requestDm.Status = newStatus;
            requestDm.RejectReason = request.Message;
            response.ResponseMessage = request.Message;

            //update the status in PMN, if fails do not save locally
            using (var http = PMNApiClient.CreateForUser(config.Value, Request.Cookies))
            {
                var updateResult = await http.SetRouteStatus(new PMNApi.PMNDto.SetRequestDataMartStatusDTO { RequestDataMartID = requestDm.ID, Status = newStatus, Message = request.Message });
                if (updateResult.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception($"Error updating route status to '{ newStatus.ToString() }'.", new Exception(updateResult.Message));
                }
            }

            //save at PMN complete, save locally
            await modelDb.SaveChangesAsync();

            var requestStatusText = newStatus == Data.Enums.RoutingStatus.PendingUpload ? "Pending Upload" : "Submitted";
            logger.AddResponse(response.ID).Information($"Route status has been changed to '{ requestStatusText }' by '{ details.User.UserName }'");

            await AlertRequestChanged(request.RequestDataMartID, userID, requestHub);
        }

        [HttpPost, Route("upload-part")]
        public async Task UploadPart([FromForm] DocumentUploadDTO dto, [FromServices] IHubContext<RequestHub> requestHub)
        {
            var userID = GetUserID();

            var req = await (from rsp in this.modelDb.Responses
                             join rdm in this.modelDb.RequestDataMarts on rsp.RequestDataMartID equals rdm.ID
                             where rsp.ID == dto.RequestResponseID
                             select new
                             {
                                 rdm.ID,
                                 rdm.RequestID,
                                 rdm.DataMart,
                                 ResponseID = rsp.ID
                             }).FirstOrDefaultAsync();

            var cacheManager = new CacheManager(config, req.DataMart, req.RequestID, req.ResponseID, this.logger);

            cacheManager.AddTempFile(dto);

            if (dto.CurrentChunk == dto.TotalChunks)
            {
                var docID = DatabaseEx.NewGuid();
                var newDoc = new Document
                {
                    ID = docID,
                    RevisionSetID = docID,
                    Name = dto.FileName,
                    MimeType = ObjectExtensions.GetMimeTypeByExtension(dto.FileName),
                    Length = dto.FileSize,
                    CreatedOn = DateTime.UtcNow,
                    ContentCreatedOn = DateTime.UtcNow,
                    ContentModifiedOn = DateTime.UtcNow,
                    ItemID = dto.RequestResponseID,
                    UploadedByID = userID,
                    DocumentState = DocumentStates.Local
                };

                await this.modelDb.AddAsync(newDoc);

                await this.modelDb.AddAsync(new RequestDocument { RevisionSetID = newDoc.RevisionSetID, ResponseID = req.ResponseID, DocumentType = Data.Enums.DocumentType.Output });

                var route = await this.modelDb.RequestDataMarts.Where(x => x.ID == dto.RequestDataMartID).FirstOrDefaultAsync();

                if (route.Status == RoutingStatus.Submitted || route.Status == RoutingStatus.Resubmitted)
                {
                    route.Status = RoutingStatus.PendingUpload;
                }

                await this.modelDb.SaveChangesAsync();

                cacheManager.FinalizeChunks(dto.UploadID, newDoc, dto.TotalChunks);

                await requestHub.Clients.Group(dto.RequestDataMartID.ToString("D")).SendAsync(RequestHub.EventIdentifiers.Response_DocumentAdded, new DocumentDTO { ID = newDoc.ID, Name = newDoc.Name, Length = newDoc.Length, DocumentState = newDoc.DocumentState, DocumentType = DocumentType.Output, Timestamp = newDoc.PmnTimestamp });

                await AlertRequestChanged(dto.RequestDataMartID, userID, requestHub);
            }
        }

        

        [HttpPost, Route("post-response")]
        public async Task PostResponseToPMN(RequestStatusChange request, [FromServices] IHubContext<RequestHub> requestHub)
        {
            var userID = GetUserID();

            //get the route
            var route = await this.modelDb.RequestDataMarts.Where(x => x.ID == request.RequestDataMartID).Include(x => x.DataMart).FirstOrDefaultAsync();
            if (route == null)
            {
                throw new KeyNotFoundException($"Route not found with ID: { request.RequestDataMartID }");
            }

            //check that the user has permission to update the routing status
            Models.RoutePermissionsComponent permissions = null;
            //get the permissions for the user for this routing from PMN API.
            using (var pmn = new PMNApiClient(config.Value.PopMedNet))
            {
                permissions = await pmn.GetRoutePermissionsForUser(userID, route.ID);
            }

            if (permissions == null || permissions.UploadResults == false)
            {
                throw new UnauthorizedAccessException("Unauthorized exception: the user does not have permission to view the request.");
            }


            //get the current response
            var requestResponse = await (from rsp in modelDb.Responses where rsp == modelDb.Responses.Where(rr => rr.RequestDataMartID == route.ID).OrderByDescending(rr => rr.Count).FirstOrDefault() select rsp).FirstOrDefaultAsync();


            var docs = await (from doc in this.modelDb.Documents
                              join rsp in this.modelDb.Responses on doc.ItemID equals rsp.ID
                              join rdm in this.modelDb.RequestDataMarts on rsp.RequestDataMartID equals rdm.ID
                              join rd in this.modelDb.RequestDocuments on rsp.ID equals rd.ResponseID
                              where rd.DocumentType == Data.Enums.DocumentType.Output
                              && rd.RevisionSetID == doc.RevisionSetID
                              && rd.ResponseID == requestResponse.ID
                              && doc.DocumentState == DocumentStates.Local
                              select new
                              {
                                  Document = doc,
                                  RequestID = rdm.RequestID,
                                  DataMartID = rdm.DataMartID
                              }).ToArrayAsync();


            var cookie = Request.Cookies.Where(x => x.Key == "DMCS-User").Select(x => x.Value).FirstOrDefault();
            var unEncryptedStringArray = Crypto.DecryptStringAES(cookie, config.Value.Settings.Hash, config.Value.Settings.Key).Split(':');

            var user = await modelDb.Users.Where(u => u.ID == userID).Select(u => new { u.UserName }).FirstOrDefaultAsync();

            route.Status = Data.Enums.RoutingStatus.AwaitingResponseApproval;
            requestResponse.ResponseMessage = request.Message;
            requestResponse.RespondedBy = user.UserName;
            requestResponse.ResponseTime = DateTime.UtcNow;

            try
            {
                if (docs.Length > 0)
                {
                    Parallel.ForEach(docs, async (doc) =>
                    {
                        using (var http = new PMNApiClient(config.Value.PopMedNet.ApiServiceURL, unEncryptedStringArray[0], unEncryptedStringArray[1]))
                        {
                            var cacheManager = new CacheManager(this.config, route.DataMart, doc.RequestID, doc.Document.ItemID, this.logger);

                            var docDTO = new ResponseDocumentUploadDTO { 
                                DataMartID = doc.DataMartID,
                                DocumentID = doc.Document.ID,
                                FileName = doc.Document.Name,
                                MimeType = doc.Document.MimeType,
                                Length = doc.Document.Length,
                                RequestID = doc.RequestID,
                                ResponseID = doc.Document.ItemID,
                                RevisionSetID = doc.Document.RevisionSetID,
                                TotalChunks = cacheManager.GetDocuments(doc.Document.ID).Count()
                            };

                            //doc.TotalChunks = cacheManager.GetDocuments(doc.Document.ID).Count();

                            for (int i = 1; i <= docDTO.TotalChunks; i++)
                            {
                                docDTO.CurrentChunk = i;
                                await http.PostDocumentChunk(docDTO, cacheManager.GetChunkStream(docDTO.DocumentID, i));
                            }

                            doc.Document.DocumentState = DocumentStates.Remote;
                        }
                    });

                    
                }

                using (var http = new PMNApiClient(config.Value.PopMedNet.ApiServiceURL, unEncryptedStringArray[0], unEncryptedStringArray[1]))
                {
                    var pmnResponse = await http.SetRouteStatus(new SetRequestDataMartStatusDTO { RequestDataMartID = route.ID, Status = route.Status, Message = requestResponse.ResponseMessage });
                    if (pmnResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        logger.Error($"Error updating the status in PMN: { pmnResponse.Message } (Response code: { pmnResponse.StatusCode }) RequestDataMartID: { route.ID }");
                        throw new Exception("There was an error updating the status of the route.");
                    }

                    route.Status = pmnResponse.RoutingStatus;
                    route.PmnTimestamp = pmnResponse.RequestDataMartTimestamp;

                }

                await this.modelDb.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                //Log exception here
                logger.Error(ex, $"Error updating the status of the route for request ID: { route.RequestID }, DataMart ID: { route.DataMartID }, Route ID: { route.ID }");
                throw;
            }

            await AlertRequestChanged(request.RequestDataMartID, userID, requestHub);
        }

        public class RequestStatusChange
        {
            public Guid RequestDataMartID { get; set; }
            public string Message { get; set; }
        }
    }
}
