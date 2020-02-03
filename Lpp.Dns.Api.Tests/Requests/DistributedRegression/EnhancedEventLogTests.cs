using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Lpp.Dns.Data;
using System.Data.Entity;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.Api.Tests.Requests.DistributedRegression
{
    [TestClass]
    public class EnhancedEventLogTests
    {
        const string ResourceFolderPath = "../Requests/DistributedRegression";
        const string SampleACTrackingTableFileName = "AC_TrackingTable_Sample1.csv";

        readonly log4net.ILog Logger;

        public EnhancedEventLogTests()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = log4net.LogManager.GetLogger(typeof(EnhancedEventLogTests));
        }

        [TestMethod]
        public void ReadACTrackingTableForEvents()
        {

            List<TrackingTableItem> trackingTable = new List<TrackingTableItem>(100);
            using (var reader = File.OpenText(Path.Combine(ResourceFolderPath, SampleACTrackingTableFileName)))
            using(var csv = new Microsoft.VisualBasic.FileIO.TextFieldParser(reader))
            {
                csv.SetDelimiters(",");
                csv.TrimWhiteSpace = true;

                string[] header = csv.ReadFields();
                string[] currentLine;

                while(csv.EndOfData == false)
                {
                    currentLine = csv.ReadFields();
                    if (currentLine.Length == 0)
                        continue;

                    int utc_offset = int.Parse(currentLine[Array.IndexOf(header, "UTC_OFFSET_SEC")]) * -1;
                    DateTime start = DateTime.SpecifyKind(DateTime.ParseExact(currentLine[Array.IndexOf(header, "START_DTM")], "ddMMMyyyy:HH:mm:ss.ff", null).AddSeconds(utc_offset), DateTimeKind.Utc);
                    DateTime end = DateTime.SpecifyKind(DateTime.ParseExact(currentLine[Array.IndexOf(header, "END_DTM")], "ddMMMyyyy:HH:mm:ss.ff", null).AddSeconds(utc_offset), DateTimeKind.Utc);

                    trackingTable.Add(new TrackingTableItem {
                        DataPartnerCode = currentLine[Array.IndexOf(header, "DP_CD")],
                        Iteration = int.Parse(currentLine[Array.IndexOf(header, "ITER_NB")]),
                        Step = int.Parse(currentLine[Array.IndexOf(header, "STEP_NB")]),
                        Start = start,
                        End = end,
                        CurrentStepInstruction = int.Parse(currentLine[Array.IndexOf(header, "CURR_STEP_IN")]),
                        LastIterationIn = int.Parse(currentLine[Array.IndexOf(header, "LAST_ITER_IN")]),
                        UTC_Offset_Seconds = utc_offset
                    });


                }

            }

            trackingTable.OrderBy(i => i.Iteration).ThenBy(i => i.Step).ThenBy(i => i.DataPartnerCode).ToList().ForEach(item => Console.WriteLine("{0}\t{1}.{2}\t{3}\t{4}", item.DataPartnerCode, item.Iteration, item.Step, item.Start, item.End));








        }


        [TestMethod]
        public void DownloadDocument()
        {
            Guid documentID = new Guid("6F1D10BB-B3E2-4028-A1E5-A84D011E4249");
            using(var db = new DataContext())
            {

                var document = db.Documents.Single(d => d.ID == documentID);

                using (var writer = File.OpenWrite(document.FileName))
                using(var reader = new Data.Documents.DocumentStream(db, documentID))
                {
                    reader.CopyTo(writer);
                    reader.Flush();
                    writer.Flush();
                    writer.Close();
                }
            }
        }




        [TestMethod]
        public async Task CreateLogForNonDistributedRegressionRequest()
        {
            Guid requestID = Guid.Parse("C8702FA2-C9AC-4CC4-82BE-A84D0118904C");
            var Identity = new Lpp.Utilities.Security.ApiIdentity(Guid.Parse("9F400001-FAD6-4E84-8933-A2380151C648"), "jmalenfant", null);

            using(var DataContext = new Dns.Data.DataContext())
            {
                var requestQuery = DataContext.Secure<Request>(Identity).Where(r => r.ID == requestID);


                var builder = new Lpp.Dns.Data.DistributedRegressionTracking.EnhancedEventLogBuilder();

                builder.RequestStatusChangeEvents = async () => {

                    var evts = await (from l in DataContext.LogsRequestStatusChanged
                                      let dtTimestamp = DbFunctions.CreateDateTime(l.TimeStamp.Year, l.TimeStamp.Month, l.TimeStamp.Day, l.TimeStamp.Hour, l.TimeStamp.Minute, l.TimeStamp.Second)
                                      where requestQuery.Any()
                                      && l.RequestID == requestID
                                      select new
                                      {
                                          l.TimeStamp,
                                          //treat the step as the lowest response count where the response is submitted after the status change log item timestamp or zero
                                          Step = DataContext.Responses.Where(rsp => rsp.RequestDataMart.RequestID == l.RequestID && rsp.SubmittedOn >= dtTimestamp).Select(rsp => (int?)rsp.Count).Min() ?? 0,
                                          l.Description
                                      }).ToArrayAsync();

                    return evts.Select(l => new EnhancedEventLogItemDTO
                    {
                        Timestamp = l.TimeStamp.DateTime,
                        Source = string.Empty,
                        Step = l.Step,
                        Description = l.Description
                    });

                };

                builder.RoutingStatusChangeEvents = async () =>
                {
                    var evts = await (from l in DataContext.LogsRoutingStatusChange
                                      let dtTimestamp = DbFunctions.CreateDateTime(l.TimeStamp.Year, l.TimeStamp.Month, l.TimeStamp.Day, l.TimeStamp.Hour, l.TimeStamp.Minute, l.TimeStamp.Second)
                                      where requestQuery.Any()
                                      && l.RequestDataMart.RequestID == requestID
                                      select new
                                      {
                                          Timestamp = l.TimeStamp,
                                          Source = l.RequestDataMart.DataMart.Name,
                                          Description = l.Description,
                                          //treat the step as the maximum response count where the response is submitted before the status change log item timestamp or zero
                                          Step = l.RequestDataMart.Responses.Where(rsp => rsp.SubmittedOn <= dtTimestamp).Select(rsp => (int?)rsp.Count).Max() ?? 0
                                      }).ToArrayAsync();

                    return evts.Select(l => new EnhancedEventLogItemDTO
                    {
                        Timestamp = l.Timestamp.DateTime,
                        Source = l.Source,
                        Step = l.Step,
                        Description = l.Description
                    });
                };

                builder.DocumentUploadEvents = async () =>
                {
                    var lastDocumentUpload = await (from rsp in DataContext.Responses
                                                    let lastDoc = (from rd in DataContext.RequestDocuments
                                                                   join doc in DataContext.Documents on rd.RevisionSetID equals doc.RevisionSetID
                                                                   where rd.ResponseID == rsp.ID
                                                                   && rd.DocumentType == RequestDocumentType.Output
                                                                   orderby doc.CreatedOn descending
                                                                   select doc).FirstOrDefault()
                                                    where rsp.RequestDataMart.RequestID == requestID
                                                    && requestQuery.Any()
                                                    && rsp.ResponseTime != null
                                                    && lastDoc != null
                                                    select new
                                                    {
                                                        Iteration = rsp.Count,
                                                        DataMart = rsp.RequestDataMart.DataMart.Name,
                                                        DocumentCreatedOn = lastDoc.CreatedOn
                                                    }).ToArrayAsync();


                    return lastDocumentUpload.Select(l => new EnhancedEventLogItemDTO
                    {
                        Timestamp = l.DocumentCreatedOn,
                        Source = l.DataMart,
                        Step = l.Iteration,
                        Description = "Files finished uploading"
                    });

                };



                ////parse latest AC tracking table
                ////parse any DP tracking tables that are iteration a head of AC

                var dataPartners = await DataContext.RequestDataMarts.Where(rdm => rdm.RequestID == requestID).Select(rdm => new { rdm.DataMart.Name, Identifier = (rdm.DataMart.DataPartnerIdentifier ?? rdm.DataMart.Acronym), rdm.RoutingType }).ToDictionaryAsync(k => k.Identifier);

                builder.TrackingTableEvents = async () =>
                {

                    //get the ID of the latest Analysis tracking document
                    var latestACTrackingDocumentID = await (from rd in DataContext.RequestDocuments
                                                            join doc in DataContext.Documents on rd.RevisionSetID equals doc.RevisionSetID
                                                            where rd.Response.RequestDataMart.RequestID == requestID
                                                            && requestQuery.Any()
                                                            && rd.Response.RequestDataMart.RoutingType == RoutingType.AnalysisCenter
                                                            && rd.Response.Count == rd.Response.RequestDataMart.Responses.Max(rsp => rsp.Count)
                                                            && doc.Kind == "DistributedRegression.TrackingTable"
                                                            orderby doc.MajorVersion, doc.MinorVersion, doc.BuildVersion, doc.RevisionVersion descending
                                                            select doc.ID).FirstOrDefaultAsync();

                    if (latestACTrackingDocumentID == default(Guid))
                    {
                        return Array.Empty<EnhancedEventLogItemDTO>();
                    }

                    IEnumerable<Data.DistributedRegressionTracking.TrackingTableItem> trackingTableItems;
                    using (var db = new DataContext())
                    using (var stream = new Data.Documents.DocumentStream(db, latestACTrackingDocumentID))
                    {
                        trackingTableItems = await DistributedRegressionTrackingTableProcessor.Read(stream);
                    }

                    List<EnhancedEventLogItemDTO> logItems = new List<EnhancedEventLogItemDTO>(trackingTableItems.Count());

                    int lastIteration = trackingTableItems.Max(t => t.Iteration);

                    foreach (var partnerEntries in trackingTableItems.GroupBy(k => k.DataPartnerCode))
                    {
                        var dataPartnerName = dataPartners[TranslatePartnerIdentifier(partnerEntries.Key)].Name;

                        foreach (var iteration in partnerEntries.GroupBy(k => k.Iteration))
                        {
                            

                            if (iteration.Key == 0 || iteration.Key == lastIteration)
                            {
                                //read from the last start time
                                logItems.Add(new EnhancedEventLogItemDTO
                                {
                                    Step = iteration.Key,
                                    Description = "SAS program execution begin",
                                    Source = dataPartnerName,
                                    Timestamp = iteration.Max(l => l.Start)
                                });
                            }
                            else
                            {
                                //if DP read the latest start
                                //if AC read the 2nd last start time

                                //TODO: talk to Qoua - I don't think this is valid logic/rule
                                logItems.Add(new EnhancedEventLogItemDTO
                                {
                                    Step = iteration.Key,
                                    Description = "SAS program execution begin",
                                    Source = dataPartnerName,
                                    Timestamp = iteration.Max(l => l.Start)
                                });

                            }
                            //read the last end time
                            logItems.Add(new EnhancedEventLogItemDTO
                            {
                                Step = iteration.Key,
                                Description = "SAS program execution complete, output files written.",
                                Source = dataPartnerName,
                                Timestamp = iteration.Max(l => l.End)
                            });
                        };

                    };


                    return logItems;
                };

                //builder.AdapterLoggedEvents = async () => {

                //    List<EnhancedEventLogItemDTO> logItems = new List<EnhancedEventLogItemDTO>();

                //    //get the adapter event logs, will need to know the response iteration, and the datamart name

                //    var adapterLogs = await (from rd in DataContext.RequestDocuments
                //                             let doc = (from d in DataContext.Documents where d.RevisionSetID == rd.RevisionSetID && d.Kind == "DistributedRegression.AdapterEventLog" select d).DefaultIfEmpty()
                //                             where rd.DocumentType == RequestDocumentType.Output
                //                             && rd.Response.RequestDataMart.RequestID == requestID
                //                             && requestQuery.Any()
                //                             && doc.Any()
                //                             select
                //                             new
                //                             {
                //                                 ResponseID = rd.ResponseID,
                //                                 ResponseIteration = rd.Response.Count,
                //                                 DataMart = rd.Response.RequestDataMart.DataMart.Name,
                //                                 DocumentID = doc.OrderByDescending(d => d.MajorVersion).ThenByDescending(d => d.MinorVersion).ThenByDescending(d => d.BuildVersion).ThenByDescending(d => d.RevisionVersion).Select(d => d.ID).FirstOrDefault()
                //                             }).ToArrayAsync();

                //    foreach (var log in adapterLogs)
                //    {
                //        //get the log content
                //        using (var db = new DataContext())
                //        using (var streamReader = new StreamReader(new Data.Documents.DocumentStream(db, log.DocumentID)))
                //        using (var reader = new Newtonsoft.Json.JsonTextReader(streamReader))
                //        {
                //            var serializer = new Newtonsoft.Json.JsonSerializer();
                //            var adapterEvents = serializer.Deserialize<IEnumerable<EventLogItem>>(reader)
                //            .Select(al => new EnhancedEventLogItemDTO
                //            {
                //                Step = log.ResponseIteration,
                //                Source = log.DataMart,
                //                Description = al.Description,
                //                Timestamp = al.Timestamp
                //            }).ToArray();

                //            if (adapterEvents.Length > 0)
                //            {
                //                logItems.AddRange(adapterEvents);
                //            }
                //        }
                //    }

                //    return logItems;
                //};

                var eventLog = await builder.GetItems();

                StringBuilder sb = new StringBuilder();
                foreach(var item in eventLog)
                {
                    //Console.WriteLine("{0}\t{1:o}\t{2}\t{3}", item.Step, item.Timestamp, item.Source, item.Description);
                    //Logger.InfoFormat("{0}\t{1:o}\t{2}\t{3}\r\n", item.Step, item.Timestamp, item.Source, item.Description);
                    sb.AppendLine(string.Format("{0}\t{1:o}\t{2}\t{3}", item.Step, item.Timestamp, item.Source, item.Description));
                }

                Logger.Info("Event Log:\n" + sb.ToString());

                

            }
        }

        [TestMethod]
        public async Task QueryForAdapterEventLogs()
        {
            //request: v_glc_01_114 (25437) on UAT Feb 2018
            Guid requestID = new Guid("ef68a8c7-cc5a-4de3-b4b7-a88200b5ea33");
            using (var DataContext = new DataContext())
            {
                DataContext.Database.Log = (s) => {
                    Logger.Debug(s);
                };

                var adapterLogs = await (from d in DataContext.Documents
                                         let rqID = requestID
                                         let response = DataContext.Responses.Where(rsp =>
                                            rsp.RequestDataMart.RequestID == rqID
                                            && (
                                            //document is linked via ItemID to the response (datapartners, and AC if not updated to task)
                                            rsp.ID == d.ItemID
                                            // document is associated to task, and task is associated to the response and request (AC tasks only)
                                            || DataContext.Actions.Where(t => t.ID == d.ItemID && t.References.Any(tr => tr.ItemID == rqID) && (t.References.Any(tr => tr.ItemID == rsp.ID))).Any()
                                            // document is associated to task, but reference to task/response is not available, use the latest response prior to the document creation
                                            || DataContext.Responses.Where(rx => rx.RequestDataMart.RequestID == rqID && rx.RequestDataMart.RoutingType == RoutingType.AnalysisCenter && rx.SubmittedOn <= d.CreatedOn && (DataContext.Actions.Where(t => t.ID == d.ItemID && t.References.Any(tr => tr.ItemID == rqID)).Any())).OrderByDescending(rx => rx.SubmittedOn).Select(rx => rx.ID).FirstOrDefault() == rsp.ID

                                            )).FirstOrDefault()
                                         where
                                         response != null
                                         && d.Kind == "DistributedRegression.AdapterEventLog"
                                         && d == DataContext.Documents.Where(dd => dd.RevisionSetID == d.RevisionSetID).OrderByDescending(dd => dd.MajorVersion).ThenByDescending(dd => dd.MinorVersion).ThenByDescending(dd => dd.BuildVersion).ThenByDescending(dd => dd.RevisionVersion).FirstOrDefault()
                                         select new
                                         {
                                             ResponseID = d.ItemID,
                                             ResponseIteration = response.Count,
                                             DataMart = response.RequestDataMart.DataMart.Name,
                                             DataMartID = response.RequestDataMart.DataMartID,
                                             DocumentID = d.ID
                                         }).ToArrayAsync();

                Guid analysisCenterID = new Guid("03878ecc-c6b1-4de4-b576-a6d500c5ca9a");
                Assert.IsTrue(adapterLogs.Where(l => l.DataMartID == analysisCenterID).Count() == 8, "Did not get all the AC adapter logs.");
            }

        }


        static string TranslatePartnerIdentifier(string trackingTableValue)
        {
            if(trackingTableValue == "0")
            {
                return "msoc";
            }

            return "msoc" + trackingTableValue;
        }

    }


    public struct TrackingTableItem
    {
        public string DataPartnerCode;
        public int Iteration;
        public int Step;
        public DateTime Start;
        public DateTime End;
        public int CurrentStepInstruction;
        public int LastIterationIn;
        public int UTC_Offset_Seconds;
    }


    /// <summary>
    /// An enhanced event log item
    /// </summary>
    public class EnhancedEventLogItem
    {
        /// <summary>
        /// Gets or sets the step, the step is composed of {iteration}.{step}.
        /// </summary>
        public decimal Step { get; set; }
        /// <summary>
        /// Gets or sets the timestamp of the event.
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Gets or sets the description of the event.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the event source as applicable (ie the datamart name)
        /// </summary>
        public string Source { get; set; }
    }

    internal struct EventLogItem
    {
        public readonly DateTime Timestamp;
        public readonly string Type;
        public readonly string Description;

        public EventLogItem(string type, string description) : this(DateTime.UtcNow, type, description)
        {
        }

        [Newtonsoft.Json.JsonConstructor]
        public EventLogItem(DateTime timestamp, string type, string description)
        {
            Timestamp = timestamp;
            Type = type;
            Description = description;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
