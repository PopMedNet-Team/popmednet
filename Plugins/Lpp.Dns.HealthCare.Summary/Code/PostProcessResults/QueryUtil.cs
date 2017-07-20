using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
//using Lpp.Data;
using Lpp.Dns.HealthCare.Summary.Data;
using Lpp.Dns.DTO.Enums;
using Lpp.Dns.Data;

namespace Lpp.Dns.HealthCare.Summary.Code
{
    public class QueryUtil
    {
        //[Import] public static IRepository<SummaryDomain, StratificationAgeRangeMapping> StratificationAgeRanges { get; set; }
        //[Import] public static IRepository<SummaryDomain, LookupListValue> LookupListValues { get; set; }

        private static DataTable GenerateSummaryQueryResultCombinations(string summaryRequestArgsXml, IDnsResponseContext context)
        {
            SummaryRequestType requestType = SummaryRequestType.All.FirstOrDefault(rt => rt.ID == context.Request.RequestType.ID);
            DataTable dt = new DataTable("Results");

            if (!string.IsNullOrEmpty(summaryRequestArgsXml))
            {
                DataColumn col;
                DataRow dr;
                string PeriodColumn = "Period";
                string SettingColumn = "Setting";
                string SexColumn = "Sex";
                string AgeColumn = "AgeGroup";
                string CodeColumn = string.Empty;
                string NameColumn = string.Empty;
                bool IsQualifiedForExpansion = false;
                bool IsDrugOrGenericName = false;
                Lists? CodeList = null;
                col = new DataColumn(PeriodColumn, "string".GetType());
                dt.Columns.Add(col);
                col = new DataColumn(SexColumn, "string".GetType());
                dt.Columns.Add(col);
                col = new DataColumn(AgeColumn, "string".GetType());
                dt.Columns.Add(col);
                col = new DataColumn(SettingColumn, "string".GetType());
                dt.Columns.Add(col);

                #region "Read Query.XML and populate rows"

                XmlDataDocument summaryRequestArgsDoc = new XmlDataDocument();
                summaryRequestArgsDoc.LoadXml(summaryRequestArgsXml);
                XPathNavigator Nav = summaryRequestArgsDoc.CreateNavigator();
                XPathExpression Expr;
                XPathNodeIterator INode = null;
                string nodeValue = string.Empty;

                string[] Years = new string[] { };
                IEnumerable<StratificationAgeRangeMapping> SelectedStratificationAgeRanges = null;
                List<string> Genders = new List<string>();
                List<string> Settings = new List<string>();
                string[] Codes = new string[] { };
                List<string> Names = new List<string>();

                switch (requestType.StringId)
                {
                    case SummaryRequestType.GenericName:
                    case SummaryRequestType.Incident_GenericName:
                        CodeList = Lists.GenericName;
                        break;
                    case SummaryRequestType.DrugClass:
                    case SummaryRequestType.Incident_DrugClass:
                        CodeList = Lists.DrugClass;
                        break;
                    //case SummaryRequestType.NDC:
                    //    CodeList = Lists.DrugCode;
                    //    break;
                    case SummaryRequestType.ICD9Diagnosis:
                    case SummaryRequestType.Incident_ICD9Diagnosis:
                        CodeList = Lists.ICD9Diagnosis;
                        break;
                    case SummaryRequestType.ICD9Diagnosis_4_digit:
                        CodeList = Lists.ICD9Diagnosis4Digits;
                        break;
                    case SummaryRequestType.ICD9Diagnosis_5_digit:
                        CodeList = Lists.ICD9Diagnosis5Digits;
                        break;
                    case SummaryRequestType.ICD9Procedures:
                        CodeList = Lists.ICD9Procedures;
                        break;
                    case SummaryRequestType.ICD9Procedures_4_digit:
                        CodeList = Lists.ICD9Procedures4Digits;
                        break;
                    case SummaryRequestType.HCPCSProcedures:
                        CodeList = Lists.HCPCSProcedures;
                        break;
                }

                #region "Get Names for code column e.g. pxcode,dxcode,drugclass,etc..and name column e.g. pxname,dxname,etc.."

                //The Generation of Combination Rows and Subsequent Expansion Of results applicable only to Prevalence,Incidence Queries.
                switch (requestType.StringId)
                {
                    case SummaryRequestType.ICD9Diagnosis:
                    case SummaryRequestType.ICD9Diagnosis_4_digit:
                    case SummaryRequestType.ICD9Diagnosis_5_digit:
                    case SummaryRequestType.Incident_ICD9Diagnosis:
                        CodeColumn = "DxCode";
                        NameColumn = "DxName";
                        IsQualifiedForExpansion = true;
                        break;
                    case SummaryRequestType.ICD9Procedures:
                    case SummaryRequestType.ICD9Procedures_4_digit:
                    case SummaryRequestType.HCPCSProcedures:
                        CodeColumn = "PxCode";
                        NameColumn = "PxName";
                        IsQualifiedForExpansion = true;
                        break;
                    case SummaryRequestType.GenericName:
                    case SummaryRequestType.Incident_GenericName:
                        CodeColumn = string.Empty;
                        NameColumn = "GenericName";
                        IsQualifiedForExpansion = true;
                        IsDrugOrGenericName = true;
                        break;
                    case SummaryRequestType.DrugClass:
                    case SummaryRequestType.Incident_DrugClass:
                        CodeColumn = string.Empty;
                        NameColumn = "DrugClass";
                        IsQualifiedForExpansion = true;
                        IsDrugOrGenericName = true;
                        break;
                }

                #endregion

                if (IsQualifiedForExpansion)
                {
                    if (!string.IsNullOrEmpty(CodeColumn))
                    {
                        col = new DataColumn(CodeColumn, "string".GetType());
                        dt.Columns.Add(col);
                    }
                    if (!string.IsNullOrEmpty(NameColumn))
                    {
                        col = new DataColumn(NameColumn, "string".GetType());
                        dt.Columns.Add(col);
                    }

                    //Read Selected Periods e.g. 2016,2017, etc.
                    Expr = Nav.Compile("SummaryRequestModel/Period");
                    INode = Nav.Select(Expr);
                    if (INode.MoveNext())
                    {
                        // The Period node contains a list of SINGLE-QUOTED years or quarters (for direct insertion into a SQL statement).
                        // We need to remove the quotes, along with any whitespace, for our purposes here.
                        nodeValue = INode.Current.Value.Replace("'","").Replace(" ","");
                        if (!string.IsNullOrEmpty(nodeValue))
                        {
                            Years = nodeValue.Split(',');
                        }
                    }

                    //Read Selected Sex stratifications
                    Expr = Nav.Compile("SummaryRequestModel/SexStratification");
                    INode = Nav.Select(Expr);
                    if (INode.MoveNext())
                    {
                        nodeValue = INode.Current.Value.Trim();
                        if (!string.IsNullOrEmpty(nodeValue))
                        {
                            switch (nodeValue)
                            {
                                case "1"://Female only
                                    Genders.Add("F");
                                    break;
                                case "2"://Male only
                                    Genders.Add("M");
                                    break;
                                case "3"://Male and Female
                                    Genders.Add("F");
                                    Genders.Add("M");
                                    break;
                                case "4": //Male,Female Aggregated
                                    Genders.Add("All");
                                    break;
                            }
                        }
                    }

                    //Read Selected Age stratifications
                    Expr = Nav.Compile("SummaryRequestModel/AgeStratification");
                    INode = Nav.Select(Expr);
                    if (INode.MoveNext())
                    {
                        nodeValue = INode.Current.Value.Trim();
                        int ageStratificationCategoryId;
                        if (Int32.TryParse(nodeValue, out ageStratificationCategoryId))
                        {
                            throw new Lpp.Utilities.CodeToBeUpdatedException();

                            //SelectedStratificationAgeRanges = StratificationAgeRanges.All.Where(s => s.AgeStratificationCategoryId == ageStratificationCategoryId).ToList();
                        }
                    }

                    //Read Selected setting
                    Expr = Nav.Compile("SummaryRequestModel/Setting");
                    INode = Nav.Select(Expr);
                    if (INode.MoveNext())
                    {
                        nodeValue = INode.Current.Value.Trim();
                        if (!string.IsNullOrEmpty(nodeValue))
                        {
                            //The value 'NotSpecified' means no setting selected i.e. all settings considerred.
                            if (nodeValue.Trim().ToLower() != "notspecified")
                                Settings.Add(nodeValue);
                            else
                                Settings.Add("All Settings");
                        }
                    }

                    //Read Selected Codes and their names.
                    if (CodeList != null)
                    {
                        Expr = Nav.Compile("SummaryRequestModel/Codes");
                        INode = Nav.Select(Expr);
                        if (INode.MoveNext())
                        {
                            // Remove any spaces in the comma-delimited list of codes.
                            nodeValue = INode.Current.Value.Replace(" ", "");
                            if (!string.IsNullOrEmpty(nodeValue))
                            {
                                Codes = nodeValue.Split(',');
                            }
                            // Look up the names from the codes.
                            LookupListValue matchingNames;
                            foreach (string code in Codes)
                            {
                                // TODO: Verify this works properly if there's no match.
                                throw new Lpp.Utilities.CodeToBeUpdatedException();

                                //matchingNames = LookupListValues.All.FirstOrDefault(v => v.ListId == (int)CodeList && v.ItemCode == code);
                                //if (matchingNames != null)
                                //{
                                //    Names.Add(matchingNames.ItemName);
                                //}
                                //else
                                //{
                                //    Names.Add(string.Empty);
                                //}
                            }
                        }
                    }

                    //Create Combination Rows based on selected Period,AgeGroup,Sex,Setting and Codes.
                    if (Years.Length > 0 && SelectedStratificationAgeRanges != null && SelectedStratificationAgeRanges.Count() > 0 && Genders.Count > 0 && Settings.Count > 0)
                    {
                        foreach (string year in Years)
                        {
                            foreach (string sex in Genders)
                            {
                                foreach (StratificationAgeRangeMapping ageRange in SelectedStratificationAgeRanges)
                                {
                                    foreach (string setting in Settings)
                                    {
                                        //If Code exists for Non-drug/generic Prevalence/Incidence queries.
                                        if (Codes.Length > 0)
                                        {
                                            for (int ctr = 0; ctr < Codes.Length; ctr++)
                                            {
                                                dr = dt.NewRow();
                                                dr[PeriodColumn] = year;
                                                dr[SexColumn] = sex;
                                                dr[AgeColumn] = ageRange.AgeClassification;
                                                dr[SettingColumn] = setting;
                                                dr[CodeColumn] = Codes[ctr];
                                                if (Names.Count > ctr) dr[NameColumn] = Names[ctr];
                                                dt.Rows.Add(dr);
                                            }
                                        }
                                        else
                                        {
                                            //Code does not exist for Non-drug/generic Prevalence/Incidence queries.
                                            for (int ctr = 0; ctr < Names.Count; ctr++)
                                            {
                                                dr = dt.NewRow();
                                                dr[PeriodColumn] = year;
                                                dr[SexColumn] = sex;
                                                dr[AgeColumn] = ageRange.AgeClassification;
                                                dr[SettingColumn] = setting;
                                                if (Codes.Length > ctr) dr[CodeColumn] = Codes[ctr];
                                                dr[NameColumn] = Names[ctr];
                                                dt.Rows.Add(dr);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //For Drug or Generic Query No need of Settings Columns.
                    if (IsDrugOrGenericName)
                    {
                        if (dt.Columns.Contains(SettingColumn)) dt.Columns.Remove(SettingColumn);
                    }
                }

                #endregion

            }

            return dt;

        }

        public static DataTable ExpandSummaryResults(DataTable RowsWithData, string QueryXML, IDnsResponseContext context)
        {
            DataTable CombinationRows = GenerateSummaryQueryResultCombinations(QueryXML, context);
            if (CombinationRows != null && CombinationRows.Rows.Count > 0)
                return ExpandSummaryResults(CombinationRows, RowsWithData, context);
            else
                return null;
        }
        public static DataTable ExpandSummaryResults(DataTable CombinationRows, DataTable RowsWithData, IDnsResponseContext context)
        {
            DataTable MergedTable = null;
            string AgeColumn = "AgeGroup";
            string SexColumn = "Sex";
            string PeriodColumn = "Period";
            string EnrollmentColumn = "Total Enrollment in Strata(Members)";

            if (CombinationRows != null && CombinationRows.Rows.Count > 0)
            {
                string FilterExpression = string.Empty;

                #region "Create Table Structure For Merged Results"

                //MergedTable gets the structure of CombinationRows. 
                //Then Columns from RowsWithData are Copied into MergedTable structure.
                MergedTable = CombinationRows.Copy();
                foreach (DataColumn dcol in RowsWithData.Columns)
                {
                    if (!MergedTable.Columns.Contains(dcol.ColumnName))
                    {
                        DataColumn newCol = MergedTable.Columns.Add(dcol.ColumnName, "string".GetType());
                    }
                }

                //For all new columns copied from RowsWithData, initialize with value "0"
                foreach (DataRow dr in MergedTable.Rows)
                    foreach (DataColumn dc in MergedTable.Columns)
                        if (!CombinationRows.Columns.Contains(dc.ColumnName))
                            dr[dc.ColumnName] = "0";

                #endregion

                #region "Merge RowsWithData into MergedTable."

                /*LOGIC
                 * For Each Row Containing Result Data in RowsWithData, Find Matching Combination Rows in MergedTable.
                 * Copy the data From RowsWithData row into matching combination rows.
                 */
                foreach (DataRow drow in RowsWithData.Rows)
                {
                    FilterExpression = string.Empty;
                    foreach (DataColumn dcol in CombinationRows.Columns)
                    {
                        //Do not match with pxname and dxname, they can differ in data mart client database than what is selected in portal. Matching with pxcode,dxcode only makes sense.
                        if (dcol.ColumnName.ToLower() != "pxname" && dcol.ColumnName.ToLower() != "dxname" && RowsWithData.Columns.Contains(dcol.ColumnName) &&
                            drow[dcol.ColumnName] != null && !string.IsNullOrEmpty(drow[dcol.ColumnName].ToString()))
                            FilterExpression = FilterExpression + (string.IsNullOrEmpty(FilterExpression) ? "" : " AND ") + "Trim(" + dcol.ColumnName + ")='" + drow[dcol.ColumnName].ToString().Trim() + "'";
                    }
                    DataRow[] MatchingRows = MergedTable.Select(FilterExpression);
                    //Copy the data from Matching Row to the Associated MergedTable.
                    if (MatchingRows != null && MatchingRows.Length > 0)
                    {
                        foreach (DataRow drMerged in MatchingRows)
                        {
                            foreach (DataColumn dc in drow.Table.Columns)
                            {
                                if (!CombinationRows.Columns.Contains(dc.ColumnName))
                                {
                                    if ((drow[dc.ColumnName] != null && !string.IsNullOrEmpty(drow[dc.ColumnName].ToString())))
                                        drMerged[dc.ColumnName] = drow[dc.ColumnName];
                                }
                            }
                        }
                    }
                }

                #region "Copy Enrollment Totals as they are based on AgeGroup, Sex, Period only"

                if (RowsWithData.Columns.Contains(EnrollmentColumn) && RowsWithData.Columns.Contains(AgeColumn) && RowsWithData.Columns.Contains(SexColumn) && RowsWithData.Columns.Contains(PeriodColumn))
                {
                    foreach (DataRow drow in RowsWithData.Rows)
                    {
                        FilterExpression = "Trim(" + AgeColumn + ")='" + drow[AgeColumn].ToString().Trim() + "' AND Trim(" + SexColumn + ")='" + drow[SexColumn].ToString().Trim() + "' AND Trim(" + PeriodColumn + ")='" + drow[PeriodColumn].ToString().Trim() + "'";
                        DataRow[] MatchingRows = MergedTable.Select(FilterExpression);
                        //Copy the data from Matching Row to the Associated MergedTable.
                        if (MatchingRows != null && MatchingRows.Length > 0)
                        {
                            foreach (DataRow drMerged in MatchingRows)
                            {
                                drMerged[EnrollmentColumn] = drow[EnrollmentColumn];
                            }
                        }
                    }

                }

                #endregion


                #endregion
            }
            else
            {
                MergedTable = RowsWithData;
            }
            return MergedTable;
        }
    
    }
}
