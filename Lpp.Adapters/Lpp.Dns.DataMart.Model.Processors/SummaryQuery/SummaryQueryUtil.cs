using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Data;
using System.Xml;
using System.Xml.XPath;

#pragma warning disable 618 //XmlDataDocument is obsolete
namespace Lpp.Dns.DataMart.Model
{
    public class SummaryQueryUtil
    {

        #region Constants        

        internal const string LOW_CELL_COUNT_COL = "LowCellCount(Reset to 0)";
        internal const string LOW_CELL_COUNT_INDICATOR = "Yes";
        internal const string LOW_CELL_COUNT_COLUMNS = "dispensings members dayssupply events episodespans";

        //public const string LoggedInStatus = "LoggedIn";
        //public const string LoggedOutStatus = "LoggedOut";
        //public const string LoginFailureStatus = "Failed";
        //public const string InCompleteStatus = "InComplete";


        //static string passPhrase = "688F021D-2F3B-465b-BB90-2F4056D79340";
        //static string saltValue = "10A450C7-C15A-4f12-842B-1861131EA0E5";//MacAddress;
        //static string hashAlgorithm = "SHA1";
        //static int passwordIterations = 2;
        //static string initVector = "20EB643D-2EFA-4e";
        //static int keySize = 256;
        #endregion

        #region "QueryTypeConstants"

        //public const int GenericName = 1;
        //public const int DrugClass = 2;
        //public const int NDC = 3;
        //public const int ICD9Diagnosis = 4;
        //public const int ICD9Procedures = 5;
        //public const int HCPCSProcedures = 6;
        //public const int EligibilityAndEntrolMent = 7;
        //public const int FileDistribution = 8;
        //public const int RefreshDates = 9;
        //public const int ICD9Diagnosis_4_digit = 10;
        //public const int ICD9Diagnosis_5_digit = 11;
        //public const int ICD9Procedures_4_digit = 12;
        //public const int SAS = 13;
        //public const int Incident_GenericName = 14;
        //public const int Incident_DrugClass = 15;
        //public const int Incident_NDC = 16;
        //public const int Incident_ICD9Diagnosis = 17;
        //public const int Incident_ICD9Procedures = 18;
        //public const int Incident_HCPCSProcedures = 19;
        //public const int Incident_EligibilityAndEntrolMent = 20;
        //public const int Incident_ICD9Diagnosis_4_digit = 21;
        //public const int Incident_ICD9Diagnosis_5_digit = 22;
        //public const int Incident_ICD9Procedures_4_digit = 23;
        //public const int DataMart_Client_Application_Update = 24;
        //public const int Query_Builder_Obesity_Module1 = 25;
        //public const int Query_Builder_Obesity_Module3 = 26;
        //public const int Query_Builder_ADHD_Module1 = 27;
        //public const int Query_Builder_ADHD_Module3 = 28;

        //public const int MFU_GenericName = 29;
        //public const int MFU_DrugClass = 30;
        //public const int MFU_ICD9Diagnosis = 31; // 3 digits
        //public const int MFU_ICD9Procedures = 32; // 3 digits
        //public const int MFU_HCPCSProcedures = 33;
        //public const int MFU_ICD9Diagnosis_4_digit = 34;
        //public const int MFU_ICD9Diagnosis_5_digit = 35;
        //public const int MFU_ICD9Procedures_4_digit = 36;

        #endregion



        #region "Public Static Methods for Expanding Summary Query Results using all combinations of Period,AgeGroup,Sex,Setting,Drug/Code"


        /// <summary>
        /// Check if the result has low-cell counts.
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="thresholdCount"></param>
        /// <returns></returns>
        public static Boolean CheckCellCountsInQueryResult(DataSet ds,int ThresholdCount)
        {
            Boolean showAlert = false;

            DataTable dt = ds.Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string cellValue = dr[i].ToString();
                    double Num;
                    bool isNum = double.TryParse(cellValue, out Num);

                    //if (isNum && LOW_CELL_COUNT_COLUMNS.ToLower().Contains(dt.Columns[i].ColumnName.ToLower()) && Num > 0 && Num < Convert.ToDouble(ThresholdCount))
                    if (isNum && IsCheckedColumn(dt.Columns[i].ColumnName, dt) && Num > 0 && Num < Convert.ToDouble(ThresholdCount))
                    {
                        showAlert = true;
                        break;
                    }
                }

            }

            return showAlert;
        }

        /// <summary>
        /// Set the low-cell counts in the query result to zero.
        /// </summary>
        /// <param name="queryResult"></param>
        /// <param name="ThresholdCount"></param>
        public static int[] FindLowCellCountsInQueryResult(DataTable queryResult, int ThresholdCount)
        {
            DataTable dt = queryResult;
            List<int> zeroRows = new List<int>();

            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string cellValue = dr[i].ToString();
                    double Num;

                    bool isNum = double.TryParse(cellValue, out Num);

                    if (isNum)
                    {
                        double dValue = Convert.ToDouble(dr[i]);

                        //if (LOW_CELL_COUNT_COLUMNS.ToLower().Contains(dt.Columns[i].ColumnName.ToLower()) && dValue > 0 && dValue < Convert.ToDouble(ThresholdCount))
                        if (IsCheckedColumn(dt.Columns[i].ColumnName, dt) && dValue > 0 && dValue < Convert.ToDouble(ThresholdCount))
                        {
                                zeroRows.Add(dt.Rows.IndexOf(dr));
                        }
                    }
                }
            }

            return zeroRows.ToArray();

        }

        /// <summary>
        /// Set the low-cell counts in the query result to zero.
        /// </summary>
        /// <param name="queryResult"></param>
        /// <param name="ThresholdCount"></param>
        public static void ZeroLowCellCountsInQueryResult(DataTable queryResult, int ThresholdCount)
        {
            DataTable dt = queryResult;
            List<int> zeroRows = new List<int>();

            //AddLowCellCountColumnToDataTable(dt);

            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string cellValue = dr[i].ToString();
                    double Num;

                    bool isNum = double.TryParse(cellValue, out Num);

                    if (isNum)
                    {
                        double dValue = Convert.ToDouble(dr[i]);

                        //if (LOW_CELL_COUNT_COLUMNS.ToLower().Contains(dt.Columns[i].ColumnName.ToLower()) && dValue > 0 && dValue < Convert.ToDouble(ThresholdCount))
                        if (IsCheckedColumn(dt.Columns[i].ColumnName, dt) && dValue > 0 && dValue < Convert.ToDouble(ThresholdCount))
                        {
                            dr[i] = 0;
                            //Set dependent computed columns to zero.
                            foreach (string s in GetDependentComputedColumns(dt.Columns[i].ColumnName,dt))
                            {
                                dr[s] = 0;
                            }
                            
                            //dr[LOW_CELL_COUNT_COL] = LOW_CELL_COUNT_INDICATOR;
                        }
                    }
                }
            }

        }

        private static bool IsCheckedColumn(string columnName, DataTable dt)
        {
            string[] lowCellCountColumns = LOW_CELL_COUNT_COLUMNS.Split(' ');
            string lcColumnName = columnName.ToLower();

            return (from c in lowCellCountColumns
                    where lcColumnName.IndexOf(c) >= 0 && !GetDependentComputedColumns(c, dt).Contains(columnName)
                    select c).ToList().Count() > 0;
        }

       ///// <summary>
       // /// Add a column to the datatable labelled as 'Low Cell Count'
       // /// </summary>
       // /// <param name="dt"></param>
       // private static void AddLowCellCountColumnToDataTable(DataTable dt)
       // {
       //     if (dt != null && !dt.Columns.Contains(LOW_CELL_COUNT_COL))
       //     {
       //         DataColumn colLowCellCount = new DataColumn(LOW_CELL_COUNT_COL, System.Type.GetType("System.String"));
       //         dt.Columns.Add(colLowCellCount);
       //     }
       // }

        private static List<string> GetDependentComputedColumns(string ColName,DataTable dt)
        {
            List<string> columns = new List<string>();

            switch (ColName.ToLower())
            {
                case "events":
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if(dc.ColumnName.ToLower().Contains("event rate"))
                            columns.Add(dc.ColumnName);
                    }
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.ColumnName.ToLower().Contains("events per member"))
                            columns.Add(dc.ColumnName);
                    }
                    break;
                case "members":
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.ColumnName.ToLower().Contains("user rate"))
                            columns.Add(dc.ColumnName);
                    }
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.ColumnName.ToLower().Contains("prevalence rate"))
                            columns.Add(dc.ColumnName);
                    }
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.ColumnName.ToLower().Contains("incidence rate"))
                            columns.Add(dc.ColumnName);
                    }
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.ColumnName.ToLower().Contains("days per user"))
                            columns.Add(dc.ColumnName);
                    }
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.ColumnName.ToLower().Contains("events per member"))
                            columns.Add(dc.ColumnName);
                    }
                    break;
                case "dayssupply":
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.ColumnName.ToLower().Contains("dayssupply rate"))
                            columns.Add(dc.ColumnName);
                    }
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.ColumnName.ToLower().Contains("days per user"))
                            columns.Add(dc.ColumnName);
                    }
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.ColumnName.ToLower().Contains("days per dispensing"))
                            columns.Add(dc.ColumnName);
                    }
                    break;
                case "dispensings":
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.ColumnName.ToLower().Contains("dispensing rate"))
                            columns.Add(dc.ColumnName);
                    }
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.ColumnName.ToLower().Contains("days per dispensing"))
                            columns.Add(dc.ColumnName);
                    }
                    break;
            }

            return columns;
        }
        #endregion
    }
}
