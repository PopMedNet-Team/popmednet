using Lpp.Dns.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ICSharpCode.SharpZipLib.Zip;
using System.Data.Entity;

namespace Lpp.Dns.Api.DataMartClient
{
    /// <summary>
    /// A document content upload post processsor for Modular Program.
    /// </summary>
    public class ModularProgramResponsePostProcessor : IPostProcessDocumentContent
    {
        static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(ModularProgramResponsePostProcessor));
        const string AllRunCSVFilename = "allrun_signature.csv";
        const string AllRunSasFilename = "allrun_signature.sas7bdat";
        static readonly string[] CommonData = new string[] { "MSReqID", "MSProjID", "MSWPType", "MSWPID", "MSVerID", "NumCycle", "NumScen",
                                            "MP1Cycles", "MP2Cycles", "MP3Cycles", "MP4Cycles", "MP5Cycles", "MP6Cycles",  "MP7Cycles", "MP8Cycles",
                                            "MP1Scenarios", "MP2Scenarios", "MP3Scenarios", "MP4Scenarios", "MP5Scenarios", "MP6Scenarios", "MP7Scenarios", "MP8Scenarios"
                                          };

        static readonly Guid ModularProgramTermID = new Guid("A1AE0001-E5B4-46D2-9FAD-A3D8014FFFD8");
        static readonly Guid ModularModelID = new Guid("1B0FFD4C-3EEF-479D-A5C4-69D8BA0D0154");

        private DataContext db;      
        private string _uploadDir = string.Empty;
        
        public void Initialize(DataContext db, string uploadDir)
        {
            this.db = db;
            _uploadDir = uploadDir;
        }

        public async Task ExecuteAsync(Document document)
        {
            var details = await(from d in db.Documents
                                let requestDataMart = db.Responses.Where(r => r.ID == d.ItemID).Select(r => r.RequestDataMart).FirstOrDefault()
                                let processModularProgramSearchTerms = (
                                   from rt in db.RequestTypes
                                       //make sure the request type has modular program term
                                    where (rt.Terms.Any(t => t.TermID == ModularProgramTermID) || rt.Models.Any(m => m.DataModelID == ModularModelID))
                                   //make sure the request for the document is associated to the request type
                                   && requestDataMart.Request.RequestTypeID == rt.ID
                                   //make sure the terms have not been processed for the request yet
                                   && !requestDataMart.Request.SearchTerms.Any()
                                   select rt
                                ).Any()
                                where d.ID == document.ID
                                select new
                                {
                                    Document = d,
                                    RequestID = requestDataMart.RequestID,
                                    DataMartID = requestDataMart.DataMartID,
                                    ProcessModularProgramSearchTerms = processModularProgramSearchTerms
                                }
                                   ).FirstOrDefaultAsync();

            if (!details.ProcessModularProgramSearchTerms && document.Length == 0)
            {
                Logger.Debug("Document was not for Modular Program");
                return;
            }

            //only parse if the document either matches one of the designated filenames or is a zip and contains one of the designated files.
            if ((!document.FileName.Equals(AllRunCSVFilename, StringComparison.OrdinalIgnoreCase)
                && !document.FileName.Equals(AllRunSasFilename, StringComparison.OrdinalIgnoreCase)
                && !document.FileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase)))
                return;

            //only parse and add if there are no existing search terms
            if (db.RequestSearchTerms.Any(t => t.RequestID == details.RequestID))
                return;

            ZipInputStream zf = null;
            try
            {
                using (var stream = new FileStream(Path.Combine(_uploadDir, document.ID + ".part"), FileMode.Open, FileAccess.Read))
                {
                    System.IO.StreamReader reader = null;
                    if (document.FileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                    {
                        zf = new ZipInputStream(stream);
                        ZipEntry zipEntry = null;
                        while ((zipEntry = zf.GetNextEntry()) != null && reader == null)
                        {
                            if (!zipEntry.IsFile)
                                continue;

                            IEnumerable<RequestSearchTerm> searchTerms = null;
                            if (zipEntry.Name.EndsWith(AllRunCSVFilename, StringComparison.OrdinalIgnoreCase))
                            {
                                // Done if cvs version found.
                                searchTerms = await ReadFile(zf, details.RequestID);
                                if (searchTerms.Any())
                                {
                                    db.RequestSearchTerms.AddRange(searchTerms);
                                }
                                break;
                            }
                            else if (zipEntry.Name.EndsWith(AllRunSasFilename, StringComparison.OrdinalIgnoreCase))
                            {
                                searchTerms = await ReadFile(zf, details.RequestID);
                                if (searchTerms.Any())
                                {
                                    db.RequestSearchTerms.AddRange(searchTerms);
                                }
                            }
                        }
                    }
                    else
                    {
                        var searchTerms = await ReadFile(stream, details.RequestID);
                        if (searchTerms.Any())
                        {
                            db.RequestSearchTerms.AddRange(searchTerms);
                        }
                    }

                    await db.SaveChangesAsync();
                }

            }
            finally
            {
                if (zf != null)
                {
                    zf.Dispose();
                    zf = null;
                }
                Logger.Debug($"Finished parsing the response doucment {document.ID}");
            }
        }

        async Task<IEnumerable<RequestSearchTerm>> ReadFile(Stream source, Guid requestID)
        {
            List<RequestSearchTerm> searchTerms = new List<RequestSearchTerm>();
            using (var reader = new StreamReader(source))
            {
                string line = null;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    string[] rowData = line.Split(',');
                    if (CommonData.Contains(rowData[0], StringComparer.OrdinalIgnoreCase))
                    {
                        RequestSearchTermType termType;
                        if (Enum.TryParse(rowData[0], out termType))
                        {
                            searchTerms.Add( new RequestSearchTerm
                                            {
                                                RequestID = requestID,
                                                Type = (int)termType,
                                                StringValue = rowData[1]
                                            });
                        }
                    }
                }
            }

            return searchTerms;
        }

        enum RequestSearchTermType
        {
            Text,
            ICD9DiagnosisCode,
            ICD9ProcedureCode,
            HCPCSCode,
            GenericDrugCode,
            DrugClassCode,
            SexStratifier,
            AgeStratifier,
            ClinicalSetting,
            ObservationPeriod,
            Coverage,
            OutputCriteria,
            MetricType,
            MSReqID,
            MSProjID,
            MSWPType,
            MSWPID,
            MSVerID,
            RequestID,
            MP1Cycles,
            MP2Cycles,
            MP3Cycles,
            MP4Cycles,
            MP5Cycles,
            MP6Cycles,
            MP7Cycles,
            MP8Cycles,
            MP1Scenarios,
            MP2Scenarios,
            MP3Scenarios,
            MP4Scenarios,
            MP5Scenarios,
            MP6Scenarios,
            MP7Scenarios,
            MP8Scenarios,
            NumScen,
            NumCycle,
            RequesterCenter,
            WorkplanType
        };

    }
}