using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PopMedNet.DMCS.Data.Model;
using PopMedNet.DMCS.Models;
using PopMedNet.DMCS.PMNApi;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PopMedNet.DMCS.Code
{
    public class SyncService
    {
        readonly IServiceProvider services;
        readonly IOptions<DMCSConfiguration> config;
        readonly ILogger logger;

        public SyncService(IServiceProvider services, IOptions<DMCSConfiguration> config, ILogger logger)
        {
            this.services = services;
            this.config = config;
            this.logger = logger.ForContext<SyncService>();
        }

        public async Task UpdateDatamarts()
        {
            await GetDataMartMetadata();
            await GetDataMartRequests();
        }

        private async Task GetDataMartMetadata()
        {
            logger.Information("Starting to sync datamart metadata");

            using (var scope = this.services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ModelContext>();

                var internalDms = await db.DataMarts.ToArrayAsync();

                if (internalDms.Any())
                {
                    using (var http = new PMNApiClient(config.Value.PopMedNet.ApiServiceURL, config.Value.PopMedNet.ServiceUserCredentials.UserName, config.Value.PopMedNet.ServiceUserCredentials.GetPassword()))
                    {
                        var dms = await http.GetDataMartMetadata(internalDms.Select(x => x.ID).ToArray());

                        if (dms.Any())
                        {
                            var newDms = dms.Where(x => !internalDms.Select(y => y.ID).Contains(x.ID)).ToArray();
                            var updateDms = dms.Where(x => internalDms.Select(y => y.ID).Contains(x.ID)).ToArray();
                            var removeDms = internalDms.Where(x => !dms.Select(y => y.ID).Contains(x.ID)).ToArray();

                            logger.Information($"Beginning sync of datamart metadata: { newDms.Length } new, { updateDms.Length } to update, {removeDms.Length} to remove");

                            using (var tran = await db.Database.BeginTransactionAsync())
                            {
                                foreach (var newDm in newDms)
                                {
                                    await db.DataMarts.AddAsync(new DataMart
                                    {
                                        ID = newDm.ID,
                                        Name = newDm.Name,
                                        Acronym = newDm.Acronym,
                                        Description = newDm.Description,
                                        AdapterID = newDm.AdapterID,
                                        Adapter = newDm.Adapter,
                                        PmnTimestamp = newDm.Timestamp
                                    });
                                }

                                var comparer = new Data.Model.DataMartMetadataEqualityComparer();
                                foreach (var update in updateDms)
                                {
                                    var interDm = internalDms.Where(x => x.ID == update.ID).FirstOrDefault();

                                    if (interDm != null && !comparer.Equals(interDm, update))
                                    {
                                        interDm.Name = update.Name;
                                        interDm.Acronym = update.Acronym;
                                        interDm.Description = update.Description;
                                        interDm.AdapterID = update.AdapterID;
                                        interDm.Adapter = update.Adapter;
                                        interDm.PmnTimestamp = update.Timestamp;
                                    }
                                }

                                foreach (var delete in removeDms)
                                {
                                    var deleteDM = internalDms.Where(x => x.ID == delete.ID).FirstOrDefault();

                                    if (deleteDM != null)
                                    {
                                        var sb = new System.Text.StringBuilder();
                                        sb.AppendLine($"Deleting datamart: { deleteDM.Name } and associated entities.");

                                        //need to delete any user/datamart associations
                                        var udms = db.UserDataMarts.Where(u => u.DataMartID == deleteDM.ID);
                                        sb.AppendLine($"    Deleting { await udms.CountAsync() } user/datamart association(s).");
                                        db.UserDataMarts.RemoveRange(udms);

                                        var requestDataMartsToDelete = db.RequestDataMarts.Where(rdm => rdm.DataMartID == deleteDM.ID);
                                        var responsesToDelete = requestDataMartsToDelete.SelectMany(rdm => rdm.Responses);
                                        var requestsToDelete = from r in db.Requests
                                                               where r.Routes.Where(rdm => !requestDataMartsToDelete.Contains(rdm)).Count() == 0
                                                               select r;

                                        var rdocs = from rdoc in db.RequestDocuments
                                                    join rsp in responsesToDelete on rdoc.ResponseID equals rsp.ID
                                                    select rdoc;                                       

                                        //delete request documents assocted to responses belonging to datamart routes
                                        sb.AppendLine($"    Deleting { await rdocs.CountAsync() } response document association(s).");
                                        db.RequestDocuments.RemoveRange(rdocs);

                                        //delete responses belonging to the datamart routes
                                        sb.AppendLine($"    Deleting { await responsesToDelete.CountAsync() } response(s).");
                                        db.Responses.RemoveRange(responsesToDelete);

                                        //delete the datamart routes
                                        sb.AppendLine($"    Deleting { await requestDataMartsToDelete.CountAsync() } route(s).");
                                        db.RequestDataMarts.RemoveRange(requestDataMartsToDelete);

                                        //delete the request if it does not have any routes other than the ones being deleted
                                        var requestsToDeleteResult = await requestsToDelete.ToArrayAsync();
                                        sb.AppendLine($"    Deleting { requestsToDeleteResult.Length } request(s) that no longer have any routes.");
                                        if(requestsToDeleteResult.Length > 0)
                                        {
                                            db.Requests.RemoveRange(requestsToDeleteResult);
                                        }

                                        sb.AppendLine($"    Deleting route entity.");
                                        db.Remove(deleteDM);

                                        //remove any documents that no longer have any associations
                                        var documentsToDelete = from doc in db.Documents
                                                                where db.RequestDocuments.Where(rd => rd.RevisionSetID == doc.RevisionSetID).Count() == 0
                                                                select doc;
                                        sb.AppendLine($"    Removing { await documentsToDelete.CountAsync() } document(s) that no longer have associations.");
                                        db.RemoveRange(documentsToDelete);

                                        logger.Information(sb.ToString());

                                        //cleanup local cache for datamart
                                        string cachePath = BuildCachePathForDataMart(deleteDM.ID);
                                        try
                                        {                                            
                                            if (System.IO.Directory.Exists(cachePath))
                                            {
                                                System.IO.Directory.Delete(cachePath, true);
                                            }
                                        }
                                        catch(System.IO.IOException ioEx)
                                        {
                                            logger.Error(ioEx, $"Error cleaning up cache for { deleteDM.Name }, path: { cachePath} ");
                                        }
                                    }
                                }

                                await db.SaveChangesAsync();
                                await tran.CommitAsync();
                            }
                        }
                        this.logger.Information("Finished syncing datamart metadata");
                    }
                }
                else
                {
                    logger.Information("No datamarts to sync");
                }
            }
        }        

        private async Task GetDataMartRequests()
        {   
            PMNApi.PMNDto.RoutesForRequestsDTO requestResponse = null;

            using (var scope = this.services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ModelContext>();
                var dmIds = await db.DataMarts.Select(x => x.ID).ToArrayAsync();
                
                if(dmIds.Length > 0)
                {
                    using (var pmn = new PMNApiClient(config.Value.PopMedNet))
                    {
                        requestResponse = await pmn.GetRequestsAndRoutes(dmIds);
                    }
                }
            }

            if (requestResponse == null)
                return;

            foreach(var request in requestResponse.Requests)
            {
                try
                {
                    using (var scope = this.services.CreateScope())
                    {
                        using (var syncPersister = scope.ServiceProvider.GetService<Code.Sync.RouteSyncPersister>())
                        {
                            var details = new PMNApi.PMNDto.RoutesForRequestsDTO
                            {
                                Requests = new[] { request },
                                Routes = requestResponse.Routes.Where(rt => rt.RequestID == request.ID).ToArray()
                            };

                            details.Responses = requestResponse.Responses.Where(rsp => details.Routes.Any(rt => rt.ID == rsp.RequestDataMartID)).ToArray();
                            details.RequestDocuments = requestResponse.RequestDocuments.Where(rd => details.Responses.Any(rsp => rsp.ID == rd.ResponseID)).ToArray();
                            
                            //provide all documents, not just documents for the request
                            details.Documents = requestResponse.Documents;

                            await syncPersister.Sync(details);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.logger.Error(ex, "Error synchronizing request \"{0}\" [ID: {1}]", request.MSRequestID, request.ID);
                }
            }

        }

        string BuildCachePathForDataMart(Guid dataMartID)
        {
            return System.IO.Path.Combine(config.Value.Settings.CacheFolder, config.Value.Settings.DMCSIdentifier.ToString("D"), dataMartID.ToString("D"));
        }

    }
}
