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

    }
}