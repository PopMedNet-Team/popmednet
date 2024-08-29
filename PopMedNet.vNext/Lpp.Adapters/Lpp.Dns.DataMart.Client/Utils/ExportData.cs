using System;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Lpp.Utilities;

namespace Lpp.Dns.DataMart.Client.Utils
{
    class ExportData
    {
        public enum ExportFormat : int { CSV = 1, Excel = 2 }; // Export format enumeration			

        public ExportData()
        {
        }

        public string ExportDetails(DataSet dataset, ExportFormat formatType, string filename)
        {
            if(dataset.Tables.Count < 2 && formatType == ExportFormat.CSV)
            {
                using(var writer = File.CreateText(filename))
                {
                    WriteCSV(dataset.Tables[0], writer);
                    writer.Flush();
                }
                return filename;
            }

            if(formatType == ExportFormat.CSV)
            {
                //need to write each query result to a separate file and zip up
                if(System.IO.Path.GetExtension(filename) != "zip")
                {
                    filename = Path.ChangeExtension(filename, "zip");
                }

                using(var filestream = new FileStream(filename, FileMode.Create))
                using(var zipstream = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(filestream))
                {
                    for(int i =0; i< dataset.Tables.Count; i++)
                    {
                        var table = dataset.Tables[i];

                        string zipEntryFilename = Path.ChangeExtension(table.TableName, "csv");
                        
                        var zipEntry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(zipEntryFilename);
                        zipEntry.DateTime = DateTime.Now;
                        zipstream.PutNextEntry(zipEntry);

                        using(var writer = new StreamWriter(zipstream, Encoding.Default, 1024, true))
                        {
                            WriteCSV(table, writer);
                            writer.Flush();
                        }
                        
                    }
                }

                return filename;
            }
            else if(formatType == ExportFormat.Excel)
            {
                //need to write each query result to a separate tab, with the tab name the queryname
                using(var filestream = File.Create(filename, 1024))
                using(SpreadsheetDocument doc = SpreadsheetDocument.Create(filestream, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = doc.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();
                    doc.WorkbookPart.Workbook.Sheets = new Sheets();
                    Sheets sheets = doc.WorkbookPart.Workbook.GetFirstChild<Sheets>();

                    var excelValidator = new Lpp.Utilities.Excel.ExcelEx();

                    for(uint sheetID = 1; sheetID <= dataset.Tables.Count; sheetID++)
                    {
                        var table = dataset.Tables[((int)sheetID) -1];

                        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                        Sheet sheet = new Sheet { Id = doc.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = sheetID, Name = excelValidator.ValidateTabName(table.TableName) };
                        sheets.Append(sheet);

                        SheetData sheetData = new SheetData();
                        worksheetPart.Worksheet = new Worksheet(sheetData);

                        Row headerRow = new Row();
                        for(int i = 0; i < table.Columns.Count; i++)
                        {
                            headerRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue(table.Columns[i].ColumnName) });
                        }
                        sheetData.AppendChild(headerRow);

                        Row dataRow;
                        for(int j = 0; j < table.Rows.Count; j++)
                        {
                            dataRow = new Row();
                            var row = table.Rows[j];
                            for(int k=0; k < row.ItemArray.Length; k++)
                            {
                                dataRow.AppendChild(new Cell { DataType = CellValues.String, CellValue = new CellValue(row.ItemArray[k].ToStringEx()) });
                            }
                            sheetData.AppendChild(dataRow);
                        }

                        worksheetPart.Worksheet.Save();
                    }

                }
                
                return filename;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(formatType), "formatType", "Invalid export format type value: " + formatType.ToString());
            }


        }

        void WriteCSV(DataTable table, StreamWriter writer)
        {
            var rowValues = new List<string>();

            //write the header values
            for (int j = 0; j < table.Columns.Count; j++)
            {
                rowValues.Add(EscapeForCsv(table.Columns[j].ColumnName));
            }
            writer.WriteLine(string.Join(",", rowValues.ToArray()));

            //write the data
            for (int k = 0; k < table.Rows.Count; k++)
            {
                rowValues.Clear();
                var row = table.Rows[k];
                rowValues.AddRange(row.ItemArray.Select(obj => EscapeForCsv((obj ?? string.Empty).ToString())));

                writer.WriteLine(string.Join(",", rowValues.ToArray()));
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

    }
}
