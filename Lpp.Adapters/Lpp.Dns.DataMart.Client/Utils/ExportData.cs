# region Includes...

using System;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Threading;
using MSExcel=Microsoft.Office.Interop.Excel;

# endregion // Includes...


namespace Lpp.Dns.DataMart.Client.Utils
{
    class ExportData
    {
        public enum ExportFormat : int { CSV = 1, Excel = 2 }; // Export format enumeration			

        public ExportData()
        {
        }

        #region ExportDetails OverLoad : Type#1

        public void ExportDetails(DataTable DetailsTable, ExportFormat FormatType, string FileName)
        {
            try
            {
                if (DetailsTable !=null && DetailsTable.Rows.Count == 0)
                    throw new Exception("There are no details to export.");

                if (FormatType == ExportFormat.Excel)
                {
                    ExportToExcel(DetailsTable, FileName);
                }
                else
                {
                    // Create Dataset
                    string delimeter = ",";
                    int idxAgeGroup = -1;
                    StringBuilder sb = new StringBuilder();
                    DataTable dtExport = DetailsTable.Copy();
                    dtExport.TableName = "Values";

                    // Getting Field Names
                    string[] sHeaders = new string[dtExport.Columns.Count];

                    //Insert Header Row
                    for (int i = 0; i < dtExport.Columns.Count; i++)
                    {
                        if (i < dtExport.Columns.Count && i > 0)
                            sb.Append(delimeter);
                        sb.Append(dtExport.Columns[i].ColumnName);
                        if (dtExport.Columns[i].ColumnName == "Age Group" || dtExport.Columns[i].ColumnName == "AgeGroup")
                            idxAgeGroup = i;
                    }
                    sb.AppendLine("");

                    //Add values.
                    foreach (DataRow dr in dtExport.Rows)
                    {
                        for (int i = 0; i < dtExport.Columns.Count; i++)
                        {
                            string value = string.Empty;
                            if (dr[i] != null) value=dr[i].ToString();
                            //Mofify AgeGroups
                            if (i == idxAgeGroup) // AgeRange"
                            {
                                if (!string.IsNullOrEmpty(dr[i].ToString()))
                                {
                                    if (value == "0-1" || value == "2-4" || value == "5-9" || value == "0-4" || value == "0-21")
                                        value = " " + value;
                                    else
                                        value = " " + value;
                                }
                            }

                            if (i < dtExport.Columns.Count && i > 0) sb.Append(delimeter);
                            sb.Append(value);
                        }
                        sb.AppendLine("");
                    }

                    File.WriteAllText(FileName, sb.ToString());
                }

            }
            catch (Exception Ex)
            {
                //If file created by this method..then delete it.
                throw Ex;
            }
        }

        public void ExportToExcel(System.Data.DataTable Dt,string FileName)  
        {  
            MSExcel.Application app = new MSExcel.Application();  
            app.Visible = false;

            try
            {
                //Enclose age-group by apostroph ', this is to avoid the agegroup being taken as date format by excel column.

                DataRowCollection rows = Dt.Rows;
                for (int idx = 0; idx < rows.Count; idx++)
                {
                    DataRow r = rows[idx];
                    for (int cIdx = 0; cIdx < Dt.Columns.Count; cIdx++)
                    {

                        if (Dt.Columns[cIdx].DataType.ToString().ToLower() == "system.string")
                        {
                            string c = r[cIdx].ToString();
                            if (!r[cIdx].ToString().StartsWith("'"))
                                r[cIdx] = "'" + c;
                        }

                    }
                }
                MSExcel.Workbook wb = app.Workbooks.Add(MSExcel.XlWBATemplate.xlWBATWorksheet);
                MSExcel.Worksheet ws = (MSExcel.Worksheet)wb.ActiveSheet;

                // Headers.  
                for (int i = 0; i < Dt.Columns.Count; i++)
                {
                    ws.Cells[1, i + 1] = Dt.Columns[i].ColumnName;
                }

                // Content.  
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    for (int j = 0; j < Dt.Columns.Count; j++)
                    {
                        ws.Cells[i + 2, j + 1] = Dt.Rows[i][j].ToString();
                    }
                }

                // Lots of options here. See the documentation.  
                wb.SaveAs(FileName, MSExcel.XlFileFormat.xlWorkbookNormal
                    , null, null, null, null, MSExcel.XlSaveAsAccessMode.xlShared
                    , null, null, null, null, null);
            }
            finally
            {
                app.Quit();  
            }
        } 
        
        #endregion

    }
}
