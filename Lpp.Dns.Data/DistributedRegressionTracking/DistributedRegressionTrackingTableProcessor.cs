using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Lpp.Dns.Data;
using System.Data.Entity;
using System.IO;
using System.Text;

namespace Lpp.Dns.Data
{
    /// <summary>
    /// Inspects and processes the response documents for any tracking tables.
    /// </summary>
    public class DistributedRegressionTrackingTableProcessor
    {
        const string TrackingTableDocumentKind = "DistributedRegression.TrackingTable";
        readonly DataContext _db;

        /// <summary>
        /// Initializes a new tracking table processor using the specified DataContext.
        /// </summary>
        /// <param name="db"></param>
        public DistributedRegressionTrackingTableProcessor(DataContext db)
        {
            _db = db;
        }

        /// <summary>
        /// For the specified response processes any associated tracking table documents, converting from csv to json array.
        /// The json array is set on the ResponseData property of the response, save is _not_ called.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public async Task Process(Response response)
        {
            Guid[] trackingTableDocuments = await _db.Documents.AsNoTracking().Where(d => d.ItemID == response.ID && d.Kind == TrackingTableDocumentKind).Select(d => d.ID).ToArrayAsync();
            if(trackingTableDocuments.Length == 0)
            {
                return;
            }
            
            StringBuilder buffer = new StringBuilder();

            string[] tableHeader;
            string[] currentLine;

            using (var writer = new Newtonsoft.Json.JsonTextWriter(new StringWriter(buffer)))
            {
                writer.QuoteName = true;

                writer.WriteStartArray();

                foreach (Guid trackingTableDocumentID in trackingTableDocuments)
                {
                    //read the tracking table csv into a dictionary, each dictionary represents a row in the csv document
                    using(var dsDataContext = new DataContext())
                    using (var reader = new StreamReader(new Data.Documents.DocumentStream(dsDataContext, trackingTableDocumentID)))
                    using (var csv = new Microsoft.VisualBasic.FileIO.TextFieldParser(reader))
                    {
                        csv.SetDelimiters(",");
                        csv.TrimWhiteSpace = true;

                        tableHeader = csv.ReadFields();

                        while (csv.EndOfData == false)
                        {
                            currentLine = csv.ReadFields();
                            if (currentLine.Length == 0)
                                continue;

                            writer.WriteStartObject();

                            for (int i = 0; i < currentLine.Length; i++)
                            {
                                writer.WritePropertyName(tableHeader[i]);
                                writer.WriteValue(currentLine[i]);
                            }

                            writer.WriteEndObject();

                        }

                        reader.Close();
                    }

                }

                writer.WriteEndArray();
                writer.Flush();

                response.ResponseData = buffer.ToString();
            }
        }

        /// <summary>
        /// Reads the tracking table from the supplied IO stream.
        /// </summary>
        /// <param name="stream">The tracking table content.</param>
        /// <returns></returns>
        public static async Task<IEnumerable<DistributedRegressionTracking.TrackingTableItem>> Read(Stream stream)
        {
            List<DistributedRegressionTracking.TrackingTableItem> items = new List<DistributedRegressionTracking.TrackingTableItem>(100);
            using(var csv = new Microsoft.VisualBasic.FileIO.TextFieldParser(stream))
            {
                csv.SetDelimiters(",");
                csv.TrimWhiteSpace = true;

                string[] header = csv.ReadFields();
                string[] currentLine = Array.Empty<string>();

                while(csv.EndOfData == false)
                {
                    currentLine = await Task.Run(() => csv.ReadFields());
                 
                    int utc_offset = int.Parse(currentLine[Array.IndexOf(header, "UTC_OFFSET_SEC")]) * -1;
                    DateTime start = DateTime.SpecifyKind(DateTime.ParseExact(currentLine[Array.IndexOf(header, "START_DTM")], "ddMMMyyyy:HH:mm:ss.ff", null).AddSeconds(utc_offset), DateTimeKind.Utc);
                    DateTime end = DateTime.SpecifyKind(DateTime.ParseExact(currentLine[Array.IndexOf(header, "END_DTM")], "ddMMMyyyy:HH:mm:ss.ff", null).AddSeconds(utc_offset), DateTimeKind.Utc);

                    items.Add(new DistributedRegressionTracking.TrackingTableItem
                    {
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

            return items;
        }

    }
}