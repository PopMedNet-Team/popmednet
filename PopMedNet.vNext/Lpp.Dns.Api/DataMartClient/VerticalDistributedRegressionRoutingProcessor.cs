using Lpp.Dns.Data;
using Lpp.Dns.DTO.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Lpp.Dns.Api.DataMartClient
{
    /// <summary>
    /// 
    /// </summary>
    public class VerticalDistributedRegressionRoutingProcessor
    {
        static readonly Guid WorkflowID = new Guid("047AC2E4-3F74-40DB-BA9F-2DFA0FB3123A");
        static readonly Guid CompleteDistributionActivityID = new Guid("D0E659B8-1155-4F44-9728-B4B6EA4D4D55");
        static readonly Guid CompletedActivityID = new Guid("9392ACEF-1AF3-407C-B19C-BAE88C389BFC");
        const string StopProcessingTriggerDocumentKind = "DistributedRegression.Stop";
        const string JobFailTriggerFileKind = "DistributedRegression.Failed";

        readonly DataContext DB;
        readonly Guid IdentityID;

        TrustMatrix[] TrustMatrix;

        private static readonly string TrustMatrixKind = "DistributedRegression.TrustMatrix";
        private static readonly string TrustMatrixFileAndExtension = "TrustMatrix.json";
        private static readonly string TrustMatrixFile = "TrustMatrix";

        static readonly Random RandomGenerator = new Random((DateTime.Now.Hour * 60) + DateTime.Now.Second);

        static readonly System.Collections.Concurrent.ConcurrentDictionary<Guid, DateTime> ActiveProcessingRequests = new System.Collections.Concurrent.ConcurrentDictionary<Guid, DateTime>();

        static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(VerticalDistributedRegressionRoutingProcessor));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="identityID"></param>
        public VerticalDistributedRegressionRoutingProcessor(DataContext db, Guid identityID)
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
            //wait a small random amount of time before processing to try to avoid collision with webfarm containing multiple API
            System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(RandomGenerator.NextDouble() * 1500));


            var ifAdvaceStatuses = new[] {
                    RoutingStatus.Completed,
                    RoutingStatus.ResultsModified,
                    RoutingStatus.RequestRejected,
                    RoutingStatus.ResponseRejectedBeforeUpload,
                    RoutingStatus.ResponseRejectedAfterUpload,
                    RoutingStatus.AwaitingResponseApproval,
                    RoutingStatus.Draft
                };

            var routes = await DB.RequestDataMarts.Where(x => x.RequestID == reqDM.RequestID).ToArrayAsync();

            if (routes.All(x => ifAdvaceStatuses.Contains(x.Status)))
            {
                if (ActiveProcessingRequests.ContainsKey(reqDM.RequestID))
                    return;


                ActiveProcessingRequests.TryAdd(reqDM.RequestID, DateTime.UtcNow);

                try
                {
                    var matrixDoc = await (from d in DB.Documents.AsNoTracking()
                                           where d.ItemID == reqDM.RequestID
                                           && d.FileName == TrustMatrixFileAndExtension
                                           && d.Kind == TrustMatrixKind
                                           select new { d.ID }).FirstOrDefaultAsync();

                    if (matrixDoc != null)
                    {
                        using (var docDB = new DataContext())
                        using (var sr = new StreamReader(LocalDiskCache.Instance.GetStream(DB, matrixDoc.ID)))
                        {
                            TrustMatrix = JsonConvert.DeserializeObject<TrustMatrix[]>(sr.ReadToEnd());
                            sr.Close();
                        }
                    }

                    int incrementedCount = (from res in DB.Responses
                                            join rdm in DB.RequestDataMarts on res.RequestDataMartID equals rdm.ID
                                            where rdm.RequestID == reqDM.RequestID
                                            select res.Count).Max() + 1;

                    var routeIDs = routes.Select(x => x.ID);

                    var currentTask = await PmnTask.GetActiveTaskForRequestActivityAsync(reqDM.Request.ID, reqDM.Request.WorkFlowActivityID.Value, DB);
                    List<DTO.QueryComposer.DistributedRegressionAnalysisCenterManifestItem> manifestItems = new List<DTO.QueryComposer.DistributedRegressionAnalysisCenterManifestItem>();

                    var q = from rd in DB.RequestDocuments
                            join rsp in DB.Responses on rd.ResponseID equals rsp.ID
                            join rdm in DB.RequestDataMarts on rsp.RequestDataMartID equals rdm.ID
                            join dm in DB.DataMarts on rdm.DataMartID equals dm.ID
                            join doc in DB.Documents on rd.RevisionSetID equals doc.RevisionSetID
                            where rsp.Count == (incrementedCount - 1)
                            && rdm.RequestID == reqDM.Request.ID
                            && routeIDs.Contains(reqDM.ID)
                            && rd.DocumentType == RequestDocumentType.Output
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

                    if (documents.Any(d => d.DocumentKind == StopProcessingTriggerDocumentKind))
                    {
                        var openRoutes = await DB.RequestDataMarts.Where(x => x.RequestID == reqDM.RequestID
                                                                            && (x.Status == RoutingStatus.Submitted || x.Status == RoutingStatus.Resubmitted || x.Status == RoutingStatus.Draft)
                                                                        ).ToArrayAsync();

                        foreach (var route in openRoutes)
                        {
                            route.Status = RoutingStatus.Canceled;
                        }

                        currentTask.Status = TaskStatuses.Complete;
                        currentTask.EndOn = DateTime.UtcNow;
                        currentTask.PercentComplete = 100d;

                        reqDM.Request.WorkFlowActivityID = CompletedActivityID;

                        await DB.SaveChangesAsync();
                    }
                    else
                    {
                        List<KeyValuePair<string, Guid>> docsToSend = new List<KeyValuePair<string, Guid>>();

                        var fileListFiles = documents.Where(d => !string.IsNullOrEmpty(d.DocumentKind) && string.Equals("DistributedRegression.FileList", d.DocumentKind, StringComparison.OrdinalIgnoreCase));

                        var requestDataMarts = await (from dm in DB.DataMarts
                                                      join rdm in DB.RequestDataMarts on dm.ID equals rdm.DataMartID
                                                      where rdm.RequestID == reqDM.Request.ID
                                                      select new DTO.QueryComposer.DistributedRegressionManifestDataPartner
                                                      {
                                                          DataMartID = dm.ID,
                                                          DataMartIdentifier = dm.DataPartnerIdentifier,
                                                          DataMartCode = dm.DataPartnerCode,
                                                          RouteType = rdm.RoutingType.Value
                                                      }).ToArrayAsync();

                        foreach (var fileListFile in fileListFiles)
                        {
                            //only include the files indicated in the filelist document
                            using (var stream = LocalDiskCache.Instance.GetStream(DB, fileListFile.DocumentID))
                            using (var reader = new System.IO.StreamReader(stream))
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
                                            Guid? revisionSetID = documents.Where(d => string.Equals(d.DocumentFileName, filename, StringComparison.OrdinalIgnoreCase) && d.DataMartID == fileListFile.DataMartID).Select(d => d.RevisionSetID).FirstOrDefault();
                                            if (revisionSetID.HasValue)
                                            {
                                                foreach (var dp in split[2].Trim().Split(' '))
                                                {
                                                    List<Guid> dmIDs = new List<Guid>();
                                                    dmIDs.Add(fileListFile.DataMartID);
                                                    dmIDs.Add(requestDataMarts.Where(x => x.DataMartCode == dp).Select(x => x.DataMartID).FirstOrDefault());

                                                    var isAllowed = TrustMatrix.Where(x => dmIDs.Contains(x.DataPartner1ID) && dmIDs.Contains(x.DataPartner2ID)).Select(x => x.Trusted).FirstOrDefault();

                                                    if (TrustMatrix.Length == 0 || isAllowed)
                                                    {
                                                        docsToSend.Add(new KeyValuePair<string, Guid>(dp, revisionSetID.Value));
                                                        manifestItems.AddRange(documents.Where(d => d.RevisionSetID == revisionSetID.Value).Select(d => new DTO.QueryComposer.DistributedRegressionAnalysisCenterManifestItem
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
                                    }
                                }

                                reader.Close();
                            }
                        }



                        foreach (var dp in docsToSend.GroupBy(x => x.Key))
                        {
                            var dataMart = await (from dm in DB.DataMarts
                                                  join rdm in DB.RequestDataMarts on dm.ID equals rdm.DataMartID
                                                  where rdm.RequestID == reqDM.Request.ID
                                                  && rdm.Status != RoutingStatus.Canceled
                                                  && dm.DataPartnerCode == dp.Key
                                                  select rdm).Include(rdm => rdm.Responses).FirstOrDefaultAsync();

                            if (dataMart != null)
                            {
                                var response = dataMart.AddResponse(IdentityID, incrementedCount);

                                await DB.SaveChangesAsync();

                                await DB.Entry(dataMart).ReloadAsync();                                

                                List<DTO.QueryComposer.DistributedRegressionAnalysisCenterManifestItem> filteredManifestItems = new List<DTO.QueryComposer.DistributedRegressionAnalysisCenterManifestItem>();

                                foreach (var revisionSet in dp)
                                {
                                    DB.RequestDocuments.Add(new RequestDocument { DocumentType = RequestDocumentType.Input, ResponseID = response.ID, RevisionSetID = revisionSet.Value });
                                    filteredManifestItems.Add(manifestItems.Where(x => x.RevisionSetID == revisionSet.Value).FirstOrDefault());
                                }

                                byte[] buffer;
                                using (var ms = new System.IO.MemoryStream())
                                using (var sw = new System.IO.StreamWriter(ms))
                                using (var jw = new Newtonsoft.Json.JsonTextWriter(sw))
                                {
                                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                                    serializer.Serialize(jw, new DTO.QueryComposer.DistributedRegressionManifestFile { Items = filteredManifestItems, DataPartners = requestDataMarts });
                                    jw.Flush();

                                    buffer = ms.ToArray();
                                }

                                Document analysisCenterManifest = DB.Documents.Add(new Document
                                {
                                    Description = "Contains information about the input documents and the datamart they came from.",
                                    Name = "Internal: Routing Manifest",
                                    FileName = "manifest.json",
                                    ItemID = currentTask.ID,
                                    Kind = DocumentKind.SystemGeneratedNoLog,
                                    UploadedByID = IdentityID,
                                    Viewable = false,
                                    MimeType = "application/json",
                                    Length = buffer.Length
                                });

                                analysisCenterManifest.RevisionSetID = analysisCenterManifest.ID;

                                DB.RequestDocuments.Add(new RequestDocument { DocumentType = RequestDocumentType.Input, ResponseID = response.ID, RevisionSetID = analysisCenterManifest.RevisionSetID.Value });

                                await DB.SaveChangesAsync();

                                analysisCenterManifest.SetData(DB, buffer);

                                dataMart.Status = dataMart.Responses.Count > 1 ? RoutingStatus.Resubmitted : RoutingStatus.Submitted;
                                await DB.SaveChangesAsync();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"Error processing DEC switch for requestID:{reqDM.RequestID}", ex);
                    throw;
                }
                finally
                {
                    DateTime startTime;
                    ActiveProcessingRequests.TryRemove(reqDM.RequestID, out startTime);
                }
            }
        }

        void CompleteTask(PmnTask task)
        {
            task.Status = DTO.Enums.TaskStatuses.Complete;
            task.EndOn = DateTime.UtcNow;
            task.PercentComplete = 100d;
        }
    }
    internal class TrustMatrix
    {
        public Guid DataPartner1ID { get; set; }
        public Guid DataPartner2ID { get; set; }
        public bool Trusted { get; set; }
    }
}
