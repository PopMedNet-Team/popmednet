using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Web;

namespace Lpp.Dns.General
{
    public class ExcelHelper
    {
        //Row limits older Excel version per sheet
        const int rowLimit = 65000;

        private static string getWorkbookTemplate()
        {
            var sb = new StringBuilder();
            sb.Append("<xml version>\r\n<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"\r\n");
            sb.Append(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"\r\nxmlns:x=\"urn:schemas- microsoft-com:office:excel\"\r\nxmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">\r\n");
            sb.Append(" <Styles>\r\n<Style ss:ID=\"Default\" ss:Name=\"Normal\">\r\n<Alignment ss:Vertical=\"Bottom\"/>\r\n <Borders/>");
            sb.Append("\r\n <Font/>\r\n <Interior/>\r\n <NumberFormat/>\r\n<Protection/>\r\n </Style>\r\n<Style ss:ID=\"BoldColumn\">\r\n <Font ");
            sb.Append("x:Family=\"Swiss\" ss:Bold=\"1\"/>\r\n </Style>\r\n<Style ss:ID=\"s62\">\r\n <NumberFormat");
            sb.Append(" ss:Format=\"@\"/>\r\n </Style>\r\n<Style ss:ID=\"Decimal\">\r\n<NumberFormat ss:Format=\"0.0000\"/>\r\n </Style>\r\n ");
            sb.Append("<Style ss:ID=\"Integer\">\r\n<NumberFormat ss:Format=\"0\"/>\r\n </Style>\r\n<Style ss:ID=\"DateLiteral\">\r\n <NumberFormat ");
            sb.Append("ss:Format=\"mm/dd/yyyy;@\"/>\r\n </Style>\r\n<Style ss:ID=\"s28\">\r\n");
            sb.Append("<Alignment ss:Horizontal=\"Left\" ss:Vertical=\"Top\" ss:ReadingOrder=\"LeftToRight\" ss:WrapText=\"1\"/>\r\n");
            sb.Append("<Font x:CharSet=\"1\" ss:Size=\"9\" ss:Color=\"#808080\" ss:Underline=\"Single\"/>\r\n");
            sb.Append("<Interior ss:Color=\"#FFFFFF\" ss:Pattern=\"Solid\"/></Style>\r\n</Styles>\r\n {0}</Workbook>");
            return sb.ToString();
        }

        private static string replaceXmlChar(string input)
        {
            input = input.Replace("&", "&");
            input = input.Replace("<", "<");
            input = input.Replace(">", ">");
            input = input.Replace("\\\"", "\"");
            input = input.Replace("'", "&apos;");
            return input;
        }

        private static string getWorksheets(DataSet source, int rowStart = 0)
        {
            var sw = new StringWriter();
            if (source == null || source.Tables.Count == 0)
            {
                sw.Write("<Worksheet ss:Name=\"Sheet1\"><Table><Row><Cell  ss:StyleID=\"s62\"><Data ss:Type=\"String\"></Data></Cell></Row></Table></Worksheet>");
                return sw.ToString();
            }
            foreach (DataTable dt in source.Tables)
            {
                if (dt.Rows.Count == 0)
                    sw.Write("<Worksheet ss:Name=\"" + replaceXmlChar(dt.TableName) + "\"><Table><Row><Cell  ss:StyleID=\"s62\"><Data ss:Type=\"String\"></Data></Cell></Row></Table></Worksheet>");
                else
                {
                    //write each row data
                    var sheetCount = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if ((i % rowLimit) == 0)
                        {
                            //add close tags for previous sheet of the same data table
                            if ((i / rowLimit) > sheetCount)
                            {
                                sw.Write("</Table></Worksheet>");
                                sheetCount = (i / rowLimit);
                            }
                            sw.Write("<Worksheet ss:Name=\"" + replaceXmlChar(dt.TableName) +
                                     (((i / rowLimit) == 0) ? "" :
                Convert.ToString(i / rowLimit)) + "\"><Table>");
                            //If rowStart >0, then rows = 0 to rowStart-1 should be treated as group headers and shown before column headers.
                            if (rowStart > 0 && dt.Rows.Count > rowStart )
                            {
                                for (int hdr = 0; hdr < rowStart; hdr++)
                                {
                                    sw.Write("<Row>");
                                    foreach (DataColumn dc in dt.Columns)
                                        sw.Write(
                                            string.Format("<Cell ss:StyleID=\"BoldColumn\"><Data ss:Type=\"String\">{0}</Data></Cell>",
                                            replaceXmlChar((dt.Rows[hdr][dc.ColumnName] == null) ? "" : dt.Rows[hdr][dc.ColumnName].ToString())));
                                    sw.Write("</Row>\r\n");
                                }
                            }
                            //write column name row
                            sw.Write("<Row>");
                            foreach (DataColumn dc in dt.Columns)
                                sw.Write(
                                    string.Format("<Cell ss:StyleID=\"BoldColumn\"><Data ss:Type=\"String\">{0}</Data></Cell>",
                                        replaceXmlChar(dc.ColumnName)));
                            sw.Write("</Row>\r\n");
                        }

                        if (i >= rowStart)
                        {
                            sw.Write("<Row>\r\n");
                            foreach (DataColumn dc in dt.Columns)
                                sw.Write(
                                    string.Format(
                                        "<Cell ss:StyleID=\"s62\"><Data ss:Type=\"String\">{0}</Data></Cell>",
                                                replaceXmlChar
                        (dt.Rows[i][dc.ColumnName].ToString())));
                            sw.Write("</Row>\r\n");
                        }
                    }
                    sw.Write("</Table></Worksheet>");
                }
            }

            return sw.ToString();
        }

        public static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        public static string GetExcelXml(DataTable dtInput, string filename, int rowStart = 0)
        {
            var excelTemplate = getWorkbookTemplate();
            var ds = new DataSet();
            ds.Tables.Add(dtInput.Copy());
            var worksheets = getWorksheets(ds, rowStart);
            var excelXml = string.Format(excelTemplate, worksheets);
            return excelXml;
        }

        public static string GetExcelXml(DataSet dsInput, string filename, int rowStart = 0)
        {
            var excelTemplate = getWorkbookTemplate();
            var worksheets = getWorksheets(dsInput, rowStart);
            var excelXml = string.Format(excelTemplate, worksheets);
            return excelXml;
        }

        public static void ToExcel
        (DataSet dsInput, string filename, HttpResponse response, int rowStart = 0)
        {
            var excelXml = GetExcelXml(dsInput, filename, rowStart);
            response.Clear();
            response.AppendHeader("Content-Type", "application/vnd.ms-excel");
            response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
            response.Write(excelXml);
            response.Flush();
            response.End();
        }

        public static void ToExcel
        (DataTable dtInput, string filename, HttpResponse response, int rowStart = 0)
        {
            var ds = new DataSet();
            ds.Tables.Add(dtInput.Copy());
            ToExcel(ds, filename, response);
        }

        /// <summary>
        /// TODO: Text of the form "10-14" are being auto-interpreted as dates by Excel. This can be
        /// "fixed" by coding it as "=""10-14""", but may not be supported by other applications,
        /// so I've decided not to "fix" it at this time.
        /// </summary>
        /// <param name="dsInput"></param>
        /// <param name="filename"></param>
        /// <param name="sw"></param>
        public static void ToCSV(DataSet dsInput, StringWriter sw)
        {
            bool isHeaderAdded = false;

            foreach (DataTable dt in dsInput.Tables)
            {
                // Prepend a space to force each cell to be a string.
                
                DataRowCollection rows = dt.Rows;
                for (int idx = 0; idx < rows.Count; idx++)
                {
                    DataRow r = rows[idx];
                    for (int cIdx = 0; cIdx < dt.Columns.Count; cIdx++)
                    {
                        if (cIdx == 2)
                            r[cIdx] = " " + Convert.ToString(r[cIdx]);
                    }
                }

                // Write the column headers.
                int iColCount = dt.Columns.Count;
                if (!isHeaderAdded)
                {
                    for (int i = 0; i < iColCount; i++)
                    {
                        sw.Write("\"" + dt.Columns[i].ToString().Replace("\"", "\"\"") + "\"");
                        if (i < iColCount - 1)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                    isHeaderAdded = true;
                }

                // Now write all the rows. Quote each cell to allow for comma in the text.
                foreach (DataRowView drv in dt.DefaultView)
                {
                    DataRow dr = drv.Row;
                    for (int i = 0; i < iColCount; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            sw.Write("\"" + dr[i].ToString().Replace("\"", "\"\"") + "\"");
                        }
                        if (i < iColCount - 1)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
            }
        }
    }
}
