using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;
using Lpp.Objects;
using Lpp.Utilities;
using System.Threading.Tasks;
using System.Text;

namespace Lpp.Dns.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class DistributedRegressionRoutingProcessor
    {
        static readonly Guid WorkflowID = new Guid("E9656288-33FF-4D1F-BA77-C82EB0BF0192");
        static readonly Guid CompleteDistributionActivityID = new Guid("D0E659B8-1155-4F44-9728-B4B6EA4D4D55");
        static readonly Guid ConductAnalysisActivityID = new Guid("370646FC-7A47-43B5-A4B3-659F90A188A9");
        static readonly Guid CompletedActivityID = new Guid("9392ACEF-1AF3-407C-B19C-BAE88C389BFC");
        const string StopProcessingTriggerDocumentKind = "DistributedRegression.Stop";
        const string JobFailTriggerFileKind = "DistributedRegression.Failed";

        readonly DataContext DB;
        readonly Guid IdentityID;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="identityID"></param>
        public DistributedRegressionRoutingProcessor(DataContext db, Guid identityID)
        {
            DB = db;
            IdentityID = identityID;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task Process(RequestDataMart reqDM)
        {
            if (reqDM.Request.WorkFlowActivityID.Value == CompleteDistributionActivityID)
            {
                //Complete Distribution step: Data Partners => Analysis Center
                await FromDataPartner(reqDM);
            }
            else if (reqDM.Request.WorkFlowActivityID.Value == ConductAnalysisActivityID)
            {
                //Conduct Analysis step: Analysis Center => Data Partners or Stop
                await FromAnalysisCenter(reqDM);
            }
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        async Task FromDataPartner(RequestDataMart reqDM)
        {
            //close the current task
            var currentTask = await PmnTask.GetActiveTaskForRequestActivityAsync(reqDM.Request.ID, reqDM.Request.WorkFlowActivityID.Value, DB);
            CompleteTask(currentTask);

            //open new task and set the request to the new activity
            var task = DB.Actions.Add(PmnTask.CreateForWorkflowActivity(reqDM.Request.ID, ConductAnalysisActivityID, WorkflowID, DB));
            reqDM.Request.WorkFlowActivityID = ConductAnalysisActivityID;

            //create new routing
            var analysisCenterRouting = await DB.RequestDataMarts.Include(rdm => rdm.Responses).Where(rdm => rdm.RequestID == reqDM.Request.ID && rdm.RoutingType == RoutingType.AnalysisCenter).FirstOrDefaultAsync();

            Lpp.Dns.Data.Response analysisCenterResponse = null;
            if (analysisCenterRouting.Status == RoutingStatus.Draft && analysisCenterRouting.Responses.Count == 1 && analysisCenterRouting.Responses.Where(rsp => rsp.ResponseTime.HasValue == false).Any())
            {
                //if the initial status of the routing is draft, and there is only a single response assume this is the first time hitting the analysis center.
                //use the existing response to submit to the analysis center
                analysisCenterResponse = analysisCenterRouting.Responses.First();
            }
            else if (analysisCenterRouting.Status != RoutingStatus.Draft)
            {
                analysisCenterRouting.Status = RoutingStatus.Draft;
            }
            
            if (analysisCenterResponse == null)
            {
                analysisCenterResponse = analysisCenterRouting.AddResponse(IdentityID);
            }

            //use all the dp output documents to be the input documents for the AC routing
            //build a manifest for where the documents are coming from
            List<Lpp.Dns.DTO.QueryComposer.DistributedRegressionAnalysisCenterManifestItem> manifestItems = new List<DTO.QueryComposer.DistributedRegressionAnalysisCenterManifestItem>();
            var q = from rd in DB.RequestDocuments
                    join rsp in DB.Responses on rd.ResponseID equals rsp.ID
                    join rdm in DB.RequestDataMarts on rsp.RequestDataMartID equals rdm.ID
                    join dm in DB.DataMarts on rdm.DataMartID equals dm.ID
                    join doc in DB.Documents on rd.RevisionSetID equals doc.RevisionSetID
                    where rsp.Count == rsp.RequestDataMart.Responses.Max(r => r.Count)
                    && rdm.RequestID == reqDM.Request.ID
                    && rd.DocumentType == RequestDocumentType.Output
                    && rdm.RoutingType == RoutingType.DataPartner
                    && doc.ItemID == rsp.ID
                    && doc.ID == DB.Documents.Where(dd => dd.RevisionSetID == doc.RevisionSetID && doc.ItemID == rsp.ID).OrderByDescending(dd => dd.MajorVersion).ThenByDescending(dd => dd.MinorVersion).ThenByDescending(dd => dd.BuildVersion).ThenByDescending(dd => dd.RevisionVersion).Select(dd => dd.ID).FirstOrDefault()
                    select new
                    {
                        DocumentID = doc.ID,
                        DocumentKind = doc.Kind,
                        DocumentFileName = doc.FileName,
                        ResponseID = rd.ResponseID,
                        RevisionSetID = rd.RevisionSetID,
                        RequestDataMartID = rsp.RequestDataMartID,
                        DataMartID = rdm.DataMartID,
                        DataPartnerIdentifier = dm.DataPartnerIdentifier,
                        DataMart = dm.Name
                    };

            var documents = await (q).ToArrayAsync();

            // further filtering based on if a output filelist document was included needs to be done. Only the files indicated should be passed on to the analysis center
            foreach(var dpDocuments in documents.GroupBy(k => k.RequestDataMartID))
            {

                var filelistDocument = dpDocuments.Where(d => !string.IsNullOrEmpty(d.DocumentKind) && string.Equals("DistributedRegression.FileList", d.DocumentKind, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if(filelistDocument != null)
                {

                    //only include the files indicated in the filelist document
                    using (var ds = new Lpp.Dns.Data.Documents.DocumentStream(DB, filelistDocument.DocumentID))
                    using (var reader = new System.IO.StreamReader(ds))
                    {
                        //read the header line
                        reader.ReadLine();

                        string line, filename;
                        bool includeInDistribution = false;
                        while (!reader.EndOfStream)
                        {
                            line = reader.ReadLine();
                            string[] split = line.Split(',');
                            if (split.Length > 0)
                            {
                                filename = split[0].Trim();
                                if (split.Length > 1)
                                {
                                    includeInDistribution = string.Equals(split[1].Trim(), "1");
                                }
                                else
                                {
                                    includeInDistribution = false;
                                }

                                if (includeInDistribution == false)
                                    continue;

                                if (!string.IsNullOrEmpty(filename))
                                {
                                    Guid? revisionSetID = dpDocuments.Where(d => string.Equals(d.DocumentFileName, filename, StringComparison.OrdinalIgnoreCase)).Select(d => d.RevisionSetID).FirstOrDefault();
                                    if (revisionSetID.HasValue)
                                    {
                                        DB.RequestDocuments.AddRange(dpDocuments.Where(d => d.RevisionSetID == revisionSetID.Value).Select(d => new RequestDocument { DocumentType = RequestDocumentType.Input, ResponseID = analysisCenterResponse.ID, RevisionSetID = d.RevisionSetID }).ToArray());
                                        manifestItems.AddRange(dpDocuments.Where(d => d.RevisionSetID == revisionSetID.Value).Select(d => new DTO.QueryComposer.DistributedRegressionAnalysisCenterManifestItem
                                        {
                                            DocumentID = d.DocumentID,
                                            DataMart = d.DataMart,
                                            DataMartID = d.DataMartID,
                                            DataPartnerIdentifier = d.DataPartnerIdentifier,
                                            RequestDataMartID = d.RequestDataMartID,
                                            ResponseID = d.ResponseID,
                                            RevisionSetID = d.RevisionSetID
                                        }).ToArray());
                                    }
                                }
                            }
                        }

                        reader.Close();
                    }

                }
                else
                {
                    var inputDocuments = dpDocuments.Where(d => d.DocumentKind != "DistributedRegression.AdapterEventLog" && d.DocumentKind != "DistributedRegression.TrackingTable");
                    if (inputDocuments.Count() > 0)
                    {
                        DB.RequestDocuments.AddRange(inputDocuments.Select(d => new RequestDocument { DocumentType = RequestDocumentType.Input, ResponseID = analysisCenterResponse.ID, RevisionSetID = d.RevisionSetID }).ToArray());
                        manifestItems.AddRange(inputDocuments.Select(d => new DTO.QueryComposer.DistributedRegressionAnalysisCenterManifestItem
                        {
                            DocumentID = d.DocumentID,
                            DataMart = d.DataMart,
                            DataMartID = d.DataMartID,
                            DataPartnerIdentifier = d.DataPartnerIdentifier,
                            RequestDataMartID = d.RequestDataMartID,
                            ResponseID = d.ResponseID,
                            RevisionSetID = d.RevisionSetID
                        }).ToArray());
                    }
                }
            }

            //serialize the manifest of dataparter documents to the analysis center
            byte[] buffer;
            using (var ms = new System.IO.MemoryStream())
            using (var sw = new System.IO.StreamWriter(ms))
            using (var jw = new Newtonsoft.Json.JsonTextWriter(sw))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                serializer.Serialize(jw, manifestItems);
                jw.Flush();

                buffer = ms.ToArray();
            }

            //create and add the manifest file
            Document analysisCenterManifest = DB.Documents.Add(new Document
            {
                Description = "Contains information about the input documents and the datamart they came from.",
                Name = "Internal: Analysis Center Manifest",
                FileName = "manifest.json",
                ItemID = task.ID,
                Kind = DocumentKind.SystemGeneratedNoLog,
                UploadedByID = IdentityID,
                Viewable = false,
                MimeType = "application/json",
                Length = buffer.Length
            });

            analysisCenterManifest.RevisionSetID = analysisCenterManifest.ID;

            //TODO:determine if there is a parent document to make the manifest a revision of. If there is update the revisionset id, and version numbers
            //chances are there should not be unless this is a resubmit for the same task

            var allTasks = await DB.ActionReferences.Where(tr => tr.ItemID == reqDM.Request.ID
                                                  && tr.Type == DTO.Enums.TaskItemTypes.Request
                                                  && tr.Task.Type == DTO.Enums.TaskTypes.Task
                                                 )
                                                 .Select(tr => tr.Task.ID).ToArrayAsync();

            var attachments = await (from doc in DB.Documents.AsNoTracking()
                                     join x in (
                                             DB.Documents.Where(dd => allTasks.Contains(dd.ItemID))
                                             .GroupBy(k => k.RevisionSetID)
                                             .Select(k => k.OrderByDescending(d => d.MajorVersion).ThenByDescending(d => d.MinorVersion).ThenByDescending(d => d.BuildVersion).ThenByDescending(d => d.RevisionVersion).Select(y => y.ID).Distinct().FirstOrDefault())
                                         ) on doc.ID equals x
                                     where allTasks.Contains(doc.ItemID) && doc.Kind == "Attachment.Input"
                                     orderby doc.ItemID descending, doc.RevisionSetID descending, doc.CreatedOn descending
                                     select doc).ToArrayAsync();

            DB.RequestDocuments.Add(new RequestDocument { DocumentType = RequestDocumentType.Input, ResponseID = analysisCenterResponse.ID, RevisionSetID = analysisCenterManifest.RevisionSetID.Value });

            foreach (var attachment in attachments)
            {
                DB.RequestDocuments.Add(new RequestDocument { RevisionSetID = attachment.RevisionSetID.Value, ResponseID = analysisCenterResponse.ID, DocumentType = DTO.Enums.RequestDocumentType.AttachmentInput });
            }

            await DB.SaveChangesAsync();
            await DB.Entry(analysisCenterRouting).ReloadAsync();

            //post the manifest content
            analysisCenterManifest.SetData(DB, buffer);

            //submit the routing
            analysisCenterRouting.Status = analysisCenterResponse.Count > 1 ? RoutingStatus.Resubmitted : RoutingStatus.Submitted;
            await DB.SaveChangesAsync();

            //Check for Job Fail trigger, and update Data Partner Routing to Failed. 
            //Request status will not be affected.
            if (documents.Any(d => d.DocumentKind == JobFailTriggerFileKind))
            {
                //Reload the entity before updating the status, else EF will report an exception.
                await DB.Entry(reqDM).ReloadAsync();

                reqDM.Status = RoutingStatus.Failed;
                await DB.SaveChangesAsync();
            }

            //change the status of the request to conducting analysis
            //manually override the request status using sql direct, EF does not allow update of computed
            await DB.Database.ExecuteSqlCommandAsync("UPDATE Requests SET Status = @status WHERE ID = @ID", new System.Data.SqlClient.SqlParameter("@status", (int)RequestStatuses.ConductingAnalysis), new System.Data.SqlClient.SqlParameter("@ID", reqDM.Request.ID));

            await DB.Entry(reqDM.Request).ReloadAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        async Task FromAnalysisCenter(RequestDataMart reqDM)
        {
            //close the current task
            var currentTask = await PmnTask.GetActiveTaskForRequestActivityAsync(reqDM.Request.ID, reqDM.Request.WorkFlowActivityID.Value, DB);
            currentTask.Status = DTO.Enums.TaskStatuses.Complete;
            currentTask.EndOn = DateTime.UtcNow;
            currentTask.PercentComplete = 100d;

            //open new task and set the request to the new activity
            var task = DB.Actions.Add(PmnTask.CreateForWorkflowActivity(reqDM.Request.ID, CompleteDistributionActivityID, WorkflowID, DB));
            reqDM.Request.WorkFlowActivityID = CompleteDistributionActivityID;

            //get the documents uploaded by the analysis center
            var documents = await (from rd in DB.RequestDocuments
                                   join rsp in DB.Responses on rd.ResponseID equals rsp.ID
                                   join rdm in DB.RequestDataMarts on rsp.RequestDataMartID equals rdm.ID
                                   join dm in DB.DataMarts on rdm.DataMartID equals dm.ID
                                   join doc in DB.Documents on rd.RevisionSetID equals doc.RevisionSetID
                                   where rdm.RequestID == reqDM.Request.ID
                                   && rdm.RoutingType == RoutingType.AnalysisCenter
                                   && rd.DocumentType == RequestDocumentType.Output
                                   && rsp.Count == rdm.Responses.Max(r => r.Count)
                                   && doc.ID == DB.Documents.Where(dd => dd.RevisionSetID == doc.RevisionSetID).OrderByDescending(dd => dd.MajorVersion).ThenByDescending(dd => dd.MinorVersion).ThenByDescending(dd => dd.BuildVersion).ThenByDescending(dd => dd.RevisionVersion).Select(dd => dd.ID).FirstOrDefault()
                                   select new
                                   {
                                       DocumentID = doc.ID,
                                       RevisionSetID = rd.RevisionSetID,
                                       DocumentKind = doc.Kind,
                                       FileName = doc.FileName
                                   }).ToArrayAsync();

            if (documents.Any(d => d.DocumentKind == JobFailTriggerFileKind))
            {
                //stop file, end regression.
                CompleteTask(task);
                reqDM.Request.WorkFlowActivityID = CompletedActivityID;

                //Save changes and reload entity
                //If this is not done, EF throws an exception on updating the status of the Request DataMart.
                await DB.SaveChangesAsync();
                await DB.Entry(reqDM).ReloadAsync();

                //Set the routing status to Failed.
                reqDM.Status = RoutingStatus.Failed;
                await DB.SaveChangesAsync();

                //change the status of the request to Failed
                //manually override the request status using sql direct, EF does not allow update of computed
                await DB.Database.ExecuteSqlCommandAsync("UPDATE Requests SET Status = @status WHERE ID = @ID", new System.Data.SqlClient.SqlParameter("@status", (int)RequestStatuses.Failed), new System.Data.SqlClient.SqlParameter("@ID", reqDM.Request.ID));

                return;
            }

            if (documents.Any(d => d.DocumentKind == StopProcessingTriggerDocumentKind))
            {
                //stop file, end regression. All the routes should be complete now and the request status should be already in Completed. Just need to update the current activity.
                CompleteTask(task);
                reqDM.Request.WorkFlowActivityID = CompletedActivityID;                

                //NOTE: notification of complete request will be handled outside of the helper
            }
            else
            {
                
                if (documents.Length > 0)
                {//ASPE-605: change the association of the document owner from the analysis center response to the complete distribution task (current task).
                 //This is to support showing the AC documents in the task documents tab.

                    //add a reference to the response to the current task so that we can track back to the source.
                    try
                    {
                        Guid currentResponseID = await DB.Responses.Where(rsp => rsp.RequestDataMartID == reqDM.ID && rsp == rsp.RequestDataMart.Responses.OrderByDescending(rrsp => rrsp.Count).FirstOrDefault()).Select(rsp => rsp.ID).FirstAsync();
                        if (DB.Entry(currentTask).Collection(t => t.References).IsLoaded == false)
                        {
                            await DB.Entry(currentTask).Collection(t => t.References).LoadAsync();
                        }
                        if (currentTask.References.Any(tr => tr.ItemID == currentResponseID) == false)
                        {
                            task.References.Add(new TaskReference { TaskID = currentTask.ID, ItemID = currentResponseID, Type = TaskItemTypes.Response });
                        }
                    }catch
                    {
                        //do not kill processing if this fails, eventlog builder has contingencies to handle if the task reference to the response is not set.
                    }

                    StringBuilder query = new StringBuilder();
                    query.Append("UPDATE Documents SET ItemID='" + currentTask.ID.ToString("D") + "'");
                    query.Append(" WHERE ID IN (");
                    query.Append(string.Join(",", documents.Select(d => string.Format("'{0:D}'", d.DocumentID))));
                    query.Append(")");
                    await DB.Database.ExecuteSqlCommandAsync(query.ToString());
                }

                //the output files from the analysis center will now become the input files for each active dataparter route
                List<Guid> responseIDs = new List<Guid>();

                var allTasks = await DB.ActionReferences.Where(tr => tr.ItemID == reqDM.Request.ID
                                                  && tr.Type == DTO.Enums.TaskItemTypes.Request
                                                  && tr.Task.Type == DTO.Enums.TaskTypes.Task
                                                 )
                                                 .Select(tr => tr.Task.ID).ToArrayAsync();

                var attachments = await (from doc in DB.Documents.AsNoTracking()
                                         join x in (
                                                 DB.Documents.Where(dd => allTasks.Contains(dd.ItemID))
                                                 .GroupBy(k => k.RevisionSetID)
                                                 .Select(k => k.OrderByDescending(d => d.MajorVersion).ThenByDescending(d => d.MinorVersion).ThenByDescending(d => d.BuildVersion).ThenByDescending(d => d.RevisionVersion).Select(y => y.ID).Distinct().FirstOrDefault())
                                             ) on doc.ID equals x
                                         where allTasks.Contains(doc.ItemID) && doc.Kind == "Attachment.Input"
                                         orderby doc.ItemID descending, doc.RevisionSetID descending, doc.CreatedOn descending
                                         select doc).ToArrayAsync();

                var dataPartnerRoutes = await DB.RequestDataMarts.Include(rdm => rdm.Responses).Where(rdm => rdm.RequestID == reqDM.Request.ID && rdm.Status != RoutingStatus.Canceled && rdm.RoutingType == RoutingType.DataPartner).ToArrayAsync();
                foreach (var route in dataPartnerRoutes)
                {
                    var response = route.AddResponse(IdentityID);
                    responseIDs.Add(response.ID);

                    route.Status = response.Count > 1 ? RoutingStatus.Resubmitted : RoutingStatus.Submitted;

                    foreach (var attachment in attachments)
                    {
                        DB.RequestDocuments.Add(new RequestDocument { RevisionSetID = attachment.RevisionSetID.Value, ResponseID = response.ID, DocumentType = DTO.Enums.RequestDocumentType.AttachmentInput });
                    }
                }

                var filelistDocument = documents.Where(d => !string.IsNullOrEmpty(d.DocumentKind) && string.Equals("DistributedRegression.FileList", d.DocumentKind, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if(filelistDocument != null)
                {
                    //only include the files indicated in the filelist document
                    using (var ds = new Lpp.Dns.Data.Documents.DocumentStream(DB, filelistDocument.DocumentID))
                    using(var reader = new System.IO.StreamReader(ds))
                    {
                        //read the header line
                        reader.ReadLine();

                        string line, filename;
                        bool includeInDistribution = false;
                        while (!reader.EndOfStream)
                        {
                            line = reader.ReadLine();
                            string[] split = line.Split(',');
                            if (split.Length > 0)
                            {
                                filename = split[0].Trim();
                                if (split.Length > 1)
                                {
                                    includeInDistribution = string.Equals(split[1].Trim(), "1");
                                }
                                else
                                {
                                    includeInDistribution = false;
                                }

                                if (includeInDistribution == false)
                                    continue;

                                if (!string.IsNullOrEmpty(filename))
                                {
                                    Guid? revisionSetID = documents.Where(d => string.Equals(d.FileName, filename, StringComparison.OrdinalIgnoreCase)).Select(d => d.RevisionSetID).FirstOrDefault();
                                    if (revisionSetID.HasValue)
                                    {
                                        DB.RequestDocuments.AddRange(responseIDs.Select(rspID => new RequestDocument { DocumentType = RequestDocumentType.Input, ResponseID = rspID, RevisionSetID = revisionSetID.Value }).ToArray());
                                    }
                                }
                            }
                        }

                        reader.Close();
                    }

                }
                else
                {
                    var requestDocuments = (from d in documents
                                           from responseID in responseIDs
                                           where d.DocumentKind != "DistributedRegression.AdapterEventLog" && d.DocumentKind != "DistributedRegression.TrackingTable"
                                           select new RequestDocument
                                           {
                                               DocumentType = RequestDocumentType.Input,
                                               ResponseID = responseID,
                                               RevisionSetID = d.RevisionSetID
                                           }).ToArray();

                    if (requestDocuments.Length > 0)
                    {
                        DB.RequestDocuments.AddRange(requestDocuments);
                    }
                }

            }

            await DB.SaveChangesAsync();
        }

        void CompleteTask(PmnTask task)
        {
            task.Status = DTO.Enums.TaskStatuses.Complete;
            task.EndOn = DateTime.UtcNow;
            task.PercentComplete = 100d;
        }

    }
}