using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PopMedNet.DMCS.Code;
using PopMedNet.DMCS.Models;
using PopMedNet.DMCS.PMNApi;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PopMedNet.DMCS.Controllers
{
    [Route("api/cache")]
    [ApiController, Authorize]
    public class CacheController : BaseApiController
    {
        readonly IOptions<DMCSConfiguration> config;
        readonly IHubContext<RequestHub> requestHub;

        public CacheController(Data.Model.ModelContext modelDb, IOptions<DMCSConfiguration> config, IHubContext<RequestHub> requestHub, ILogger logger)
            : base(modelDb, logger.ForContext("SourceContext", typeof(CacheController).FullName))
        {
            this.requestHub = requestHub;
            this.config = config;
        }

        /// <summary>
        /// Removes a document from 
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="responseID"></param>
        /// <returns></returns>
        [HttpGet, Route("remove-document")]
        public async Task RemoveDocument(Guid documentID, Guid responseID)
        {
            var req = await (from doc in this.modelDb.Documents
                             from rd in this.modelDb.RequestDocuments
                             where doc.ID == documentID
                             && rd.ResponseID == responseID && rd.RevisionSetID == doc.RevisionSetID
                             select new
                             {
                                 Document = doc,
                                 RequestDocument = rd,
                                 RequestDataMart = rd.Response.RequestDataMart,
                                 DataMart = rd.Response.RequestDataMart.DataMart
                             }).FirstOrDefaultAsync();

            if(req.Document.DocumentState != Data.Enums.DocumentStates.Local)
            {
                throw new InvalidOperationException("Documents that have already been posted to PopMedNet cannot be deleted from the DataMart Client Server.");
            }

            this.logger.AddResponse(req.Document.ItemID).Information($"Deleting document '{req.Document.Name}'(ID: { req.Document.ID })");

            this.modelDb.RequestDocuments.Remove(req.RequestDocument);
            this.modelDb.Documents.Remove(req.Document);            

            var cacheManager = new CacheManager(config, req.DataMart, req.RequestDataMart.RequestID, responseID, this.logger);
            cacheManager.Remove(req.Document, responseID);

            //TODO: change the status back to submitted if the initial status is UploadPending and there are no other documents to upload
            //if(this.modelDb.RequestDocuments.Where(rd => rd.ResponseID == responseID))

            await modelDb.SaveChangesAsync();

            await requestHub.Clients.Group(req.RequestDataMart.RequestID.ToString("D")).SendAsync(RequestHub.EventIdentifiers.Response_DocumentRemoved, documentID);
            await AlertRequestChanged(req.RequestDataMart.ID, GetUserID(), requestHub);
        }

        [HttpGet, Route("clear-for-route")]
        public async Task ClearForRoute(Guid id)
        {
            var userID = GetUserID();
            var req = await (from response in this.modelDb.Responses
                             join rdm in this.modelDb.RequestDataMarts on response.RequestDataMartID equals rdm.ID
                             join request in this.modelDb.Requests on rdm.RequestID equals request.ID
                             where rdm.ID == id && response.Count == rdm.Responses.Max(rr => rr.Count)
                             select new
                             {
                                 MSRequestID = request.MSRequestID,
                                 RequestDatamart = rdm,
                                 rdm.DataMart,
                                 ResponseID = response.ID,
                             }).FirstOrDefaultAsync();

            this.logger.AddResponse(req.ResponseID).Information($"Deleting all local response documents, and clearing all files from the cache for RequestID: {req.MSRequestID}, DataMart: { req.DataMart.Name }");

            var docs = await (from doc in this.modelDb.Documents
                              join reqDoc in this.modelDb.RequestDocuments on doc.RevisionSetID equals reqDoc.RevisionSetID
                              where reqDoc.ResponseID == req.ResponseID && reqDoc.DocumentType == Data.Enums.DocumentType.Output
                              select new
                              {
                                  RequestDoc = reqDoc,
                                  Document = doc
                              }).ToArrayAsync();

            if (docs.Length > 0)
            {
                var cacheManager = new CacheManager(config, req.DataMart, req.RequestDatamart.RequestID, req.ResponseID, this.logger);

                foreach (var doc in docs)
                {
                    //only delete local documents from the database
                    if (doc.Document.DocumentState == Data.Enums.DocumentStates.Local)
                    {
                        modelDb.RequestDocuments.Remove(doc.RequestDoc);
                        modelDb.Documents.Remove(doc.Document);

                        this.logger.AddResponse(doc.Document.ItemID).Information($"Deleting document '{doc.Document.Name}'(ID: { doc.Document.ID })");
                    }

                    cacheManager.Remove(doc.Document, doc.RequestDoc.ResponseID);
                }

                if (req.RequestDatamart.Status == Data.Enums.RoutingStatus.PendingUpload)
                {
                    req.RequestDatamart.Status = Data.Enums.RoutingStatus.Submitted;
                }

                await modelDb.SaveChangesAsync();

                await requestHub.Clients.Group(id.ToString("D")).SendAsync(RequestHub.EventIdentifiers.Response_CacheCleared);
                await AlertRequestChanged(id, userID, requestHub);
            }
        }
    }
}
