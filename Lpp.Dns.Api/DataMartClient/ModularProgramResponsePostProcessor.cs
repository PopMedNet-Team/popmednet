using Lpp.Dns.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ICSharpCode.SharpZipLib.Zip;

namespace Lpp.Dns.Api.DataMartClient
{
    /// <summary>
    /// A document content upload post processsor for Modular Program.
    /// </summary>
    public class ModularProgramResponsePostProcessor
    {
        const string AllRunCSVFilename = "allrun_signature.csv";
        const string AllRunSasFilename = "allrun_signature.sas7bdat";
        static readonly string[] CommonData = new string[] { "MSReqID", "MSProjID", "MSWPType", "MSWPID", "MSVerID", "NumCycle", "NumScen",
                                            "MP1Cycles", "MP2Cycles", "MP3Cycles", "MP4Cycles", "MP5Cycles", "MP6Cycles",  "MP7Cycles", "MP8Cycles",
                                            "MP1Scenarios", "MP2Scenarios", "MP3Scenarios", "MP4Scenarios", "MP5Scenarios", "MP6Scenarios", "MP7Scenarios", "MP8Scenarios"
                                          };
        readonly DataContext db;

        /// <summary>
        /// Initializes a new ModularProgram document upload post processor.
        /// </summary>
        /// <param name="db"></param>
        public ModularProgramResponsePostProcessor(DataContext db) 
        {
            this.db = db;
        }

        /// <summary>
        /// Reads the specified response and parses out the search terms.
        /// </summary>
        /// <remarks>This method should be wrap with exception handling, it is possible that it is only part of the document.</remarks>
        /// <param name="requestID">The ID of the request.</param>
        /// <param name="document">The response document metadata.</param>
        /// <param name="documentContent">The uploaded document content.</param>
        /// <returns></returns>
        public async Task ExecuteAsync(Guid requestID, Document document, Stream documentContent)
        {
            //only parse if the document either matches one of the designated filenames or is a zip and contains one of the designated files.
            if ((!document.FileName.Equals(AllRunCSVFilename, StringComparison.OrdinalIgnoreCase)
                && !document.FileName.Equals(AllRunSasFilename, StringComparison.OrdinalIgnoreCase)
                && !document.FileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase)))
                return;

            //only parse and add if there are no existing search terms
            if (db.RequestSearchTerms.Any(t => t.RequestID == requestID))
                return;

            ZipInputStream zf = null;
            try
            {

                System.IO.StreamReader reader = null;
                if (document.FileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                {
                    zf = new ZipInputStream(documentContent);
                    ZipEntry zipEntry = null;
                    while ((zipEntry = zf.GetNextEntry()) != null && reader == null)
                    {
                        if (!zipEntry.IsFile)
                            continue;

                        IEnumerable<RequestSearchTerm> searchTerms = null;
                        if (zipEntry.Name.EndsWith(AllRunCSVFilename, StringComparison.OrdinalIgnoreCase))
                        {
                            // Done if cvs version found.
                            searchTerms = await ReadFile(zf, requestID);
                            if (searchTerms.Any())
                            {
                                db.RequestSearchTerms.AddRange(searchTerms);
                            }
                            break;
                        }
                        else if (zipEntry.Name.EndsWith(AllRunSasFilename, StringComparison.OrdinalIgnoreCase))
                        {
                            searchTerms = await ReadFile(zf, requestID);
                            if (searchTerms.Any())
                            {
                                db.RequestSearchTerms.AddRange(searchTerms);
                            }
                        }
                    }
                }
                else
                {
                    var searchTerms = await ReadFile(documentContent, requestID);
                    if (searchTerms.Any())
                    {
                        db.RequestSearchTerms.AddRange(searchTerms);
                    }
                }

                await db.SaveChangesAsync();
            }
            finally
            {
                if (zf != null)
                {
                    zf.Dispose();
                    zf = null;
                }
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