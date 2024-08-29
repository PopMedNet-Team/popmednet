using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using Lpp.Dns.DTO.QueryComposer;
using Lpp.Dns.DTO.Enums;
using Lpp.Utilities;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Lpp.Dns.Api.Requests
{
    /// <summary>
    /// A helper class for creating response exports.
    /// </summary>
    public class ResponseExportHelper
    {
        readonly string _filename;
        readonly IEnumerable<QueryComposerResponseDTO> _responses;
        readonly IEnumerable<ResponseDataSource> _responseDataSources;

        /// <summary>
        /// Initializes a new export helper.
        /// </summary>
        /// <param name="filename">The filename of the export.</param>
        /// <param name="responses">The responses to export.</param>
        /// <param name="responseDataSources">The datasource names for each response.</param>
        public ResponseExportHelper(string filename, IEnumerable<QueryComposerResponseDTO> responses, IEnumerable<ResponseDataSource> responseDataSources)
        {
            _responses = responses;
            _responseDataSources = responseDataSources;
            _filename = CleanFilename(filename);
        }

        /// <summary>
        /// Exports the responses in csv format, if the response is multi-query each query response will be a separate csv file zipped into a file of the request name.
        /// </summary>
        /// <param name="viewType">The response result view type, Individual or Aggregate.</param>
        /// <returns></returns>
        public Stream ExportAsCSV(TaskItemTypes viewType)
        {
            var queries = Queries.ToArray();
            var includeDataMartName = viewType != TaskItemTypes.AggregateResponse;

            MemoryStream ms = new MemoryStream();
            if (queries.Count() > 1)
            {
                var zipStream = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(ms);
                zipStream.IsStreamOwner = false;

                foreach (var grouping in Queries)
                {
                    var datamartAcronym = includeDataMartName ? "-" + grouping.Select(g => g.DataMartAcronym).FirstOrDefault() : string.Empty;
                    string zipEntryFilename = Path.ChangeExtension(CleanFilename(grouping.Key.QueryName + datamartAcronym), "csv");

                    var zipEntry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(zipEntryFilename);
                    zipEntry.DateTime = DateTime.Now;
                    zipStream.PutNextEntry(zipEntry);

                    using (var writer = new StreamWriter(zipStream, System.Text.Encoding.Default, 1024, true))
                    {
                        for (int i = 0; i < grouping.Count(); i++)
                        {
                            var responseResult = grouping.ElementAt(i);
                            WriteCSV(writer, responseResult, i == 0, includeDataMartName);
                        }

                        writer.Flush();
                        zipStream.CloseEntry();
                    }
                }

                zipStream.Close();
            }
            else
            {
                var firstQueryGrouping = Queries.ElementAt(0).ToArray();
                using(var writer = new StreamWriter(ms, System.Text.Encoding.Default, 1024, true))
                {
                    for (int i = 0; i < firstQueryGrouping.Length; i++)
                    {
                        WriteCSV(writer, firstQueryGrouping[i], i == 0, includeDataMartName);
                    }
                    writer.Flush();
                }
            }

            ms.Position = 0;
            return ms;
        }

        void WriteCSV(StreamWriter writer, QueryResultGrouping responseResult, bool writeHeader, bool includeDataMart)
        {
            var rowValues = new List<string>();
            if (writeHeader)
            {
                if (!string.IsNullOrWhiteSpace(responseResult.DataMart) && includeDataMart)
                {
                    rowValues.Add("DataMart");
                }

                rowValues.AddRange(responseResult.Query.Properties.Where(p => !string.Equals("LowThreshold", p.As, StringComparison.OrdinalIgnoreCase)).Select(p => EscapeForCsv(p.As)));

                writer.WriteLine(string.Join(",", rowValues));
            }

            foreach (var table in responseResult.Query.Results)
            {
                if (table.Any() == false && includeDataMart && !string.IsNullOrWhiteSpace(responseResult.DataMart))
                {
                    string row = responseResult.DataMart;
                    for(int i = 0; i < responseResult.Query.Properties.Where(p => !string.Equals("LowThreshold", p.As, StringComparison.OrdinalIgnoreCase)).Count(); i++)
                    {
                        row += ",";
                    }
                    writer.WriteLine(row);
                }
                else
                {
                    foreach (var row in table)
                    {
                        rowValues.Clear();

                        if (!string.IsNullOrWhiteSpace(responseResult.DataMart) && includeDataMart)
                        {
                            rowValues.Add(responseResult.DataMart);
                        }

                        rowValues.AddRange(row.Where(v => !string.Equals("LowThreshold", v.Key, StringComparison.OrdinalIgnoreCase)).Select(k => EscapeForCsv(k.Value.ToStringEx())).ToArray());
                        writer.WriteLine(string.Join(",", rowValues));
                    }
                }
            }
        }

        IEnumerable<IGrouping<QueryResultKey, QueryResultGrouping>> Queries
        {
            get
            {
                return (from r in _responses
                        from q in r.Queries
                        let datasource = _responseDataSources.Where(ds => r.Header.ID.HasValue && ds.ResponseID == r.Header.ID.Value).FirstOrDefault()
                        select new QueryResultGrouping
                        {
                            QueryID = (q.ID.HasValue && q.ID.Value != Guid.Empty) ? q.ID.Value.ToString("D") : q.Name,
                            QueryName = q.Name != null ? q.Name : "Cohort-1",
                            DataMart = datasource != null ? datasource.Title : string.Empty,
                            DataMartAcronym = datasource != null ? datasource.Acronym : string.Empty,
                            Query = q
                        }).GroupBy(k => new QueryResultKey { QueryID = k.QueryID, QueryName = k.QueryName });
            }
        }

        /// <summary>
        /// Gets the name of the file for a CSV export, the file ending will be .csv for non-multiquery or .zip for multiquery.
        /// </summary>
        public string FilenameForCSV
        {
            get
            {
                if (Queries.Count() > 1)
                {
                    return Path.ChangeExtension(_filename, "zip");
                }

                return Path.ChangeExtension(_filename, "csv");
            }
        }

        static string EscapeForCsv(string value)
        {
            if (string.IsNullOrEmpty(value))
                value = string.Empty;

            //http://tools.ietf.org/html/rfc4180

            char[] escapeValues = new[] { ',', '"' };
            if (value.Contains(Environment.NewLine) || value.Any(c => escapeValues.Contains(c)))
            {
                value = "\"" + value.Replace("\"", "\"\"") + "\"";
            }

            return value;
        }

        /// <summary>
        /// Exports the query results to an excel file per query.
        /// </summary>
        /// <param name="viewType">Indicates the type of response view.</param>
        /// <returns></returns>
        public Stream ExportAsExcel(TaskItemTypes viewType)
        {
            MemoryStream ms = new MemoryStream();
            var includeDataMartName = viewType != TaskItemTypes.AggregateResponse;

            if (Queries.Count() > 1)
            {
                var zipStream = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(ms);
                zipStream.IsStreamOwner = false;

                foreach (var grouping in Queries)
                {
                    var datamartAcronym = includeDataMartName ? "-" + grouping.Select(g => g.DataMartAcronym).FirstOrDefault() : string.Empty;
                    string zipEntryFilename = Path.ChangeExtension(CleanFilename(grouping.Key.QueryName + datamartAcronym), "xlsx");

                    var zipEntry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(zipEntryFilename);
                    zipEntry.DateTime = DateTime.Now;
                    zipStream.PutNextEntry(zipEntry);

                    WriteExcel(zipStream, grouping.AsEnumerable(), includeDataMartName);
                    zipStream.CloseEntry();
                }

                zipStream.Close();

            }
            else
            {
                WriteExcel(ms, Queries.ElementAt(0).ToArray(), includeDataMartName);
            }

            ms.Position = 0;

            return ms;
        }

        void WriteExcel(Stream output, IEnumerable<QueryResultGrouping> responseResults, bool includeDataMartName)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                SpreadsheetDocument doc = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook);
                //write the results for each datamart to a separate tab, each spreadsheet will represent a query
                WorkbookPart workbookPart = doc.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();
                doc.WorkbookPart.Workbook.Sheets = new Sheets();
                Sheets sheets = doc.WorkbookPart.Workbook.GetFirstChild<Sheets>();

                if (responseResults.Any())
                {
                    var excelValidator = new Lpp.Utilities.Excel.ExcelEx();

                    for (uint sheetID = 1; sheetID <= responseResults.Count(); sheetID++)
                    {
                        var response = responseResults.ElementAt((int)sheetID - 1);

                        string responseSourceName = response.DataMart;
                        if (string.IsNullOrWhiteSpace(responseSourceName))
                        {
                            var aggregationDefinition = response.Query.Aggregation;
                            if (aggregationDefinition != null && !string.IsNullOrWhiteSpace(aggregationDefinition.Name))
                                responseSourceName = aggregationDefinition.Name;
                        }

                        string tabName = excelValidator.ValidateTabName(includeDataMartName ? (string.IsNullOrWhiteSpace(response.DataMartAcronym) ? (string.IsNullOrEmpty(responseSourceName) ? "Sheet " + sheetID : responseSourceName ): response.DataMartAcronym) : responseResults.ElementAt(0).QueryName);

                        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                        Sheet sheet = new Sheet() { Id = doc.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = sheetID, Name = tabName };
                        sheets.Append(sheet);

                        SheetData sheetData = new SheetData();
                        worksheetPart.Worksheet = new Worksheet(sheetData);

                        int totalResultSets = response.Query.Results.Count();
                        int resultSetIndex = 0;
                        foreach (var table in response.Query.Results)
                        {
                            //foreach resultset create a header row, each set of results for a datamart/grouping will be on the same sheet

                            Row headerRow = new Row();
                            if(!string.IsNullOrEmpty(responseSourceName) && includeDataMartName)
                            {
                                headerRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue("DataMart") });
                            }

                            foreach (var property in response.Query.Properties.Where(p => !string.Equals(p.As, "LowThreshold", StringComparison.OrdinalIgnoreCase)))
                            {
                                headerRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue(property.As) });
                            }
                            sheetData.AppendChild(headerRow);

                            Row dataRow;
                            if (table.Count() > 0)
                            {   
                                foreach (var row in table)
                                {
                                    dataRow = new Row();

                                    if(!string.IsNullOrEmpty(responseSourceName) && includeDataMartName)
                                    {
                                        dataRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue(responseSourceName) });
                                    }

                                    foreach (var column in row)
                                    {
                                        if (!column.Key.Equals("LowThreshold", StringComparison.OrdinalIgnoreCase))
                                        {
                                            dataRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue(column.Value.ToStringEx()) });
                                        }
                                    }

                                    sheetData.AppendChild(dataRow);
                                }

                                resultSetIndex++;
                            }else if(!string.IsNullOrEmpty(responseSourceName) && includeDataMartName)
                            {
                                dataRow = new Row();
                                dataRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue(responseSourceName) });
                                for(int k = 1; k < headerRow.ChildElements.Count; k++)
                                {
                                    dataRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue("") });
                                }


                                sheetData.AppendChild(dataRow);
                            }

                            if (resultSetIndex < totalResultSets)
                            {
                                //add an empty row between resultsets
                                var emptyRow = new Row();
                                emptyRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue("") });
                                sheetData.AppendChild(emptyRow);
                            }
                        }

                        worksheetPart.Worksheet.Save();

                    }
                }
                else
                {
                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    Sheet sheet = new Sheet() { Id = doc.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet 1" };
                    sheets.Append(sheet);

                    SheetData sheetData = new SheetData();
                    worksheetPart.Worksheet = new Worksheet(sheetData);

                    worksheetPart.Worksheet.Save();
                }

                workbookPart.Workbook.Save();
                doc.Close();

                ms.Flush();
                ms.Position = 0;

                ms.CopyTo(output);
                output.Flush();
            }
        }

        /// <summary>
        /// Gets the name of the file for a Excel export, the file ending will be .xlxs for non-multiquery or .zip for multiquery.
        /// </summary>
        public string FilenameForExcel
        {
            get
            {
                if (Queries.Count() > 1)
                {
                    return Path.ChangeExtension(_filename, "zip");
                }

                return Path.ChangeExtension(_filename, "xlsx");
            }
        }

        string CleanFilename(string filename)
        {
            return string.Join("", filename.Select(c => Path.GetInvalidFileNameChars().Contains(c) ? '_' : c).ToArray());
        }

        /// <summary>
        /// Represents the name of the datasource for the response.
        /// </summary>
        public class ResponseDataSource
        {
            /// <summary>
            /// Gets or sets the ID of the response.
            /// </summary>
            public Guid ResponseID { get; set; }
            /// <summary>
            /// Gets or sets the title of the response datasource.
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// Gets or sets the acronym of the datasource.
            /// </summary>
            public string Acronym { get; set; }
        }

        internal class QueryResultKey : IEquatable<QueryResultKey>, IComparable<QueryResultKey>
        {
            public QueryResultKey()
            {
            }

            public string QueryID { get; set; }

            public string QueryName { get; set; }

            public int CompareTo(QueryResultKey other)
            {
                if ( other == null || !QueryID.Equals(other.QueryID))
                    return -1;

                return QueryID.CompareTo(other.QueryID);
            }

            public override bool Equals(object obj)
            {
                QueryResultKey other = obj as QueryResultKey;
                string otherQueryID = null;
                if(other != null)
                {
                    otherQueryID = other.QueryID;
                }

                if(QueryID == null && otherQueryID == null)
                {
                    return true;
                }

                if ((QueryID == null && otherQueryID != null) || (QueryID != null && otherQueryID == null))
                    return false;

                return QueryID.Equals(otherQueryID);
            }

            public bool Equals(QueryResultKey other)
            {
                if (other == null)
                    return false;

                return QueryID.Equals(other.QueryID);
            }

            public override int GetHashCode()
            {
                //Best algorithm for overriding GetHashCode(): https://stackoverflow.com/a/263416
                int hash = 17;
                hash = hash * 23 + (string.IsNullOrEmpty(QueryID) ? string.Empty.GetHashCode() : QueryID.GetHashCode());
                hash = hash * 23 + (string.IsNullOrEmpty(QueryName) ? string.Empty.GetHashCode() : QueryName.GetHashCode());
                return hash;
            }
        }

        internal class QueryResultGrouping
        {
            public string QueryID { get; set; }

            public string QueryName { get; set; }

            public string DataMart { get; set; }

            public string DataMartAcronym { get; set; }

            public QueryComposerResponseQueryResultDTO Query { get; set; }
        }
    }
}