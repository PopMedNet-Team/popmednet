using Lpp.Dns.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace Lpp.Dns.HealthCare.Summary.Code
{
    public class ResultsManager
    {

        static public DataSet GetQueryResults(IDnsResponseContext context)
        {
            return GetQueryResults(context, false);
        }

        static public DataSet AggregateQueryResults(IDnsResponseContext context)
        {
            return GetQueryResults(context, true);
        }

        static public DataSet UnaggregateQueryResults(IDnsResponseContext context)
        {
            StringReader sr;
            DataSet _ds = new DataSet();

            // TODO: Replace with code that does not use deprecated DRN 2.x classes!
            //Query q = Query.Load(queryId);
            using (var db = new DataContext())
            {
                foreach (var r in context.DataMartResponses)
                {
                    DataSet ds = new DataSet();
                    bool dataMartResponsesExist = false; // Assume false until proven true

                    foreach (var doc in r.Documents)
                    {

                        var responseXmlString = doc.ReadStreamAsString(db).Replace("Results", "Table");

                        if (!string.IsNullOrEmpty(responseXmlString))
                        {
                            dataMartResponsesExist = true;

                            responseXmlString = responseXmlString.Replace("Total_x0020_Enrollment_x0020_in_x0020_Strata_x0028_Members_x0029_", "Total_Enrollment_in_Strata_Members");
                            responseXmlString = responseXmlString.Replace("Days_x0020_Covered", "Days_Covered");
                            responseXmlString = responseXmlString.Replace("Prevalence_x0020_Rate_x0020__x0028_Users_x0020_per_x0020_1000_x0020_enrollees_x0029_", "Prevalence_Rate_Users_per_1000_enrollees");
                            responseXmlString = responseXmlString.Replace("Dispensing_x0020_Rate_x0020__x0028_Dispensings_x0020_per_x0020_1000_x0020_enrollees_x0029_", "Dispensing_Rate_Dispensings_per_1000_enrollees");
                            responseXmlString = responseXmlString.Replace("Days_x0020_Per_x0020_Dispensing", "Days_Per_Dispensing");
                            responseXmlString = responseXmlString.Replace("Days_x0020_Per_x0020_user", "Days_Per_user");
                            responseXmlString = responseXmlString.Replace("Event_x0020_Rate_x0020__x0028_Events_x0020_per_x0020_1000_x0020_enrollees_x0029_", "Event_Rate_Events_per_1000_enrollees");
                            responseXmlString = responseXmlString.Replace("Events_x0020_Per_x0020_member", "Events_Per_member");

                            sr = new StringReader(responseXmlString);
                            ds.ReadXml(sr);
                        }
                    }

                    if (dataMartResponsesExist && ds.Tables.Count > 0)
                    {
                        DataView v = new DataView(GetQueryResults(context, true, ds).Tables[0]);
                        DataTable dt = v.ToTable();
                        dt.TableName = r.DataMart.Name;
                        dt.Columns.Add("DataMart");
                        dt.Columns["DataMart"].SetOrdinal(0);
                        foreach (DataRow row in dt.Rows)
                            row["DataMart"] = r.DataMart.Name;
                        _ds.Tables.Add(dt);
                    }
                }
            }

            return _ds;
        }

        /// <summary>
        /// Get results for specified Query.  Performs result aggregation.
        /// </summary>
        static public DataSet GetQueryResults(IDnsResponseContext context, bool ExpandResults)
        {
            bool dataMartResponsesExist = false; // Assume false until proven true
            StringReader sr;

            // TODO: Replace with code that does not use deprecated DRN 2.x classes!
            //Query q = Query.Load(queryId);

            DataSet ds = new DataSet();

            using (var db = new DataContext())
            {
                foreach (var r in context.DataMartResponses)
                {
                    foreach (var doc in r.Documents)
                    {
                        string responseXmlString = new StreamReader(doc.GetStream(db)).ReadToEnd().Replace("Results", "Table");

                        if (!string.IsNullOrEmpty(responseXmlString))
                        {
                            dataMartResponsesExist = true;

                            responseXmlString = responseXmlString.Replace("Total_x0020_Enrollment_x0020_in_x0020_Strata_x0028_Members_x0029_", "Total_Enrollment_in_Strata_Members");
                            responseXmlString = responseXmlString.Replace("Days_x0020_Covered", "Days_Covered");
                            responseXmlString = responseXmlString.Replace("Prevalence_x0020_Rate_x0020__x0028_Users_x0020_per_x0020_1000_x0020_enrollees_x0029_", "Prevalence_Rate_Users_per_1000_enrollees");
                            responseXmlString = responseXmlString.Replace("Dispensing_x0020_Rate_x0020__x0028_Dispensings_x0020_per_x0020_1000_x0020_enrollees_x0029_", "Dispensing_Rate_Dispensings_per_1000_enrollees");
                            responseXmlString = responseXmlString.Replace("Days_x0020_Per_x0020_Dispensing", "Days_Per_Dispensing");
                            responseXmlString = responseXmlString.Replace("Days_x0020_Per_x0020_user", "Days_Per_user");
                            responseXmlString = responseXmlString.Replace("Event_x0020_Rate_x0020__x0028_Events_x0020_per_x0020_1000_x0020_enrollees_x0029_", "Event_Rate_Events_per_1000_enrollees");
                            responseXmlString = responseXmlString.Replace("Events_x0020_Per_x0020_member", "Events_Per_member");

                            sr = new StringReader(responseXmlString);
                            ds.ReadXml(sr); // TODO: merge!
                        }
                    }
                }

                if (!dataMartResponsesExist)
                    return null;

                if (ds.Tables.Count < 1)
                    return ds;

                return GetQueryResults(context, ExpandResults, ds);
            }
        }

        static private DataSet GetQueryResults(IDnsResponseContext context, bool ExpandResults, DataSet ds)
        {
            DataSetHelper dsHelper = new DataSetHelper(ref ds);
            //string GenderColumnName =(ds.Tables[0].Columns.Contains("gender"))? "gender Sex" : "Sex";
            //string SortColumnName = (ds.Tables[0].Columns.Contains("gender"))? "gender" : "Sex";

            SummaryRequestType requestType = SummaryRequestType.All.FirstOrDefault(rt => rt.ID == context.Request.RequestType.ID);
            GroupResults(ref ds, requestType);

            string aggregatedXmlString = ds.GetXml();

            //reading the xml in to a dataset does not allow spaces for the columns. So replace with apprpriate hex values.

            aggregatedXmlString = aggregatedXmlString.Replace("Total_Enrollment_in_Strata_Members", "Total_x0020_Enrollment_x0020_in_x0020_Strata_x0028_Members_x0029_");
            aggregatedXmlString = aggregatedXmlString.Replace("Days_Covered", "Days_x0020_Covered");
            aggregatedXmlString = aggregatedXmlString.Replace("Prevalence_Rate_Users_per_1000_enrollees", "Prevalence_x0020_Rate_x0020__x0028_Users_x0020_per_x0020_1000_x0020_enrollees_x0029_");
            aggregatedXmlString = aggregatedXmlString.Replace("Dispensing_Rate_Dispensings_per_1000_enrollees", "Dispensing_x0020_Rate_x0020__x0028_Dispensings_x0020_per_x0020_1000_x0020_enrollees_x0029_");
            aggregatedXmlString = aggregatedXmlString.Replace("Days_Per_Dispensing", "Days_x0020_Per_x0020_Dispensing");
            aggregatedXmlString = aggregatedXmlString.Replace("Days_Per_user", "Days_x0020_Per_x0020_user");
            aggregatedXmlString = aggregatedXmlString.Replace("Event_Rate_Events_per_1000_enrollees", "Event_x0020_Rate_x0020__x0028_Events_x0020_per_x0020_1000_x0020_enrollees_x0029_");
            aggregatedXmlString = aggregatedXmlString.Replace("Events_Per_member", "Events_x0020_Per_x0020_member");

            ds = new DataSet();
            StringReader sr = new StringReader(aggregatedXmlString);
            ds.ReadXml(sr);

            #region "Expand the result to contain all combinations of Period,AgeGroup,Sex,Setting,Code"

            // TODO: Replace with code that does not use deprecated DRN 2.x classes!
            string summaryRequestArgsXml = "";

            if (ExpandResults)
            {
                DataSet dsExpandedResult = new DataSet();
                if (ds.Tables.Count > 0)
                {
                    DataTable dtMerged = QueryUtil.ExpandSummaryResults(ds.Tables[0], summaryRequestArgsXml, context);
                    if (dtMerged != null && dtMerged.Rows.Count > 0)
                    {
                        dsExpandedResult.Tables.Clear();
                        dsExpandedResult.Tables.Add(dtMerged);
                        ds = dsExpandedResult;
                        // TODO: Replace with code that does not use deprecated DRN 2.x classes!
                        //AddComputedColumnsIfNotExist(q, ref ds);
                    }
                }
            }
            #endregion

            return ds;
        }

        private static void GroupResults(ref DataSet ds, SummaryRequestType requestType)
        {
            DataSetHelper dsHelper = new DataSetHelper(ref ds);
            string GenderColumnName = (ds.Tables[0].Columns.Contains("gender")) ? "gender Sex" : "Sex";
            string SortColumnName = (ds.Tables[0].Columns.Contains("gender")) ? "gender" : "Sex";
            switch (requestType.StringId)
            {
                case SummaryRequestType.GenericName:
                    dsHelper.SelectGroupByInto("Results", ds.Tables[0], "AgeGroup, Sex, Period, GenericName,sum(Dispensings) Dispensings, sum(Members) Members, sum(DaysSupply) DaysSupply, sum(Total_Enrollment_in_Strata_Members) Total_Enrollment_in_Strata_Members, sum(Days_Covered) Days_Covered, avg(Prevalence_Rate_Users_per_1000_enrollees) Prevalence_Rate_Users_per_1000_enrollees,avg(Dispensing_Rate_Dispensings_per_1000_enrollees) Dispensing_Rate_Dispensings_per_1000_enrollees, avg(Days_Per_Dispensing) Days_Per_Dispensing, avg(Days_Per_user) Days_Per_user", "", "AgeGroup, Sex, Period,GenericName");
                    ds.Tables.RemoveAt(0);
                    break;
                case SummaryRequestType.Incident_GenericName:
                    dsHelper.SelectGroupByInto("Results", ds.Tables[0], "AgeGroup, Sex, Period, GenericName,sum(Total_Enrollment_in_Strata_Members) Total_Enrollment_in_Strata_Members, sum(Days_Covered) Days_Covered, " +
                        "sum(Members90) Members90, sum(Dispensings90) Dispensings90, sum(DaysSupply90) DaysSupply90, sum(Members90Q1) Members90Q1, sum(Members90Q2) Members90Q2, sum(Members90Q3) Members90Q3, sum(Members90Q4) Members90Q4, " +
                        "sum(Members180) Members180, sum(Dispensings180) Dispensings180, sum(DaysSupply180) DaysSupply180, sum(Members180Q1) Members180Q1, sum(Members180Q2) Members180Q2, sum(Members180Q3) Members180Q3, sum(Members180Q4) Members180Q4, " +
                        "sum(Members270) Members270, sum(Dispensings270) Dispensings270, sum(DaysSupply270) DaysSupply270, sum(Members270Q1) Members270Q1, sum(Members270Q2) Members270Q2, sum(Members270Q3) Members270Q3, sum(Members270Q4) Members270Q4", "", "AgeGroup, Sex, Period,GenericName");
                    ds.Tables.RemoveAt(0);
                    break;

                case SummaryRequestType.DrugClass:
                    dsHelper.SelectGroupByInto("Results", ds.Tables[0], "AgeGroup, Sex, Period, DrugClass,sum(Dispensings) Dispensings, sum(Members) Members, sum(DaysSupply) DaysSupply, sum(Total_Enrollment_in_Strata_Members) Total_Enrollment_in_Strata_Members, sum(Days_Covered) Days_Covered, avg(Prevalence_Rate_Users_per_1000_enrollees) Prevalence_Rate_Users_per_1000_enrollees,avg(Dispensing_Rate_Dispensings_per_1000_enrollees) Dispensing_Rate_Dispensings_per_1000_enrollees, avg(Days_Per_Dispensing) Days_Per_Dispensing, avg(Days_Per_user) Days_Per_user", "", "AgeGroup, Sex, Period,DrugClass");
                    ds.Tables.RemoveAt(0);
                    break;
                case SummaryRequestType.Incident_DrugClass:
                    dsHelper.SelectGroupByInto("Results", ds.Tables[0], "AgeGroup, Sex, Period, DrugClass,sum(Total_Enrollment_in_Strata_Members) Total_Enrollment_in_Strata_Members, sum(Days_Covered) Days_Covered, " +
                        "sum(Members90) Members90, sum(Dispensings90) Dispensings90, sum(DaysSupply90) DaysSupply90, sum(Members90Q1) Members90Q1, sum(Members90Q2) Members90Q2, sum(Members90Q3) Members90Q3, sum(Members90Q4) Members90Q4, " +
                        "sum(Members180) Members180, sum(Dispensings180) Dispensings180, sum(DaysSupply180) DaysSupply180, sum(Members180Q1) Members180Q1, sum(Members180Q2) Members180Q2, sum(Members180Q3) Members180Q3, sum(Members180Q4) Members180Q4, " +
                        "sum(Members270) Members270, sum(Dispensings270) Dispensings270, sum(DaysSupply270) DaysSupply270, sum(Members270Q1) Members270Q1, sum(Members270Q2) Members270Q2, sum(Members270Q3) Members270Q3, sum(Members270Q4) Members270Q4", "", "AgeGroup, Sex, Period,DrugClass");
                    ds.Tables.RemoveAt(0);
                    break;

                //case SummaryRequestType.NDC:
                //    dsHelper.SelectGroupByInto("Results", ds.Tables[0], "GenericName,_NDC,sum(dispensing) dispensing,sum(Members) Members", "", "GenericName,_NDC");
                //    ds.Tables.RemoveAt(0);
                //    break;

                case SummaryRequestType.ICD9Diagnosis:
                case SummaryRequestType.ICD9Diagnosis_4_digit:
                case SummaryRequestType.ICD9Diagnosis_5_digit:
                    dsHelper.SelectGroupByInto("Results", ds.Tables[0], "AgeGroup,Sex, Period, DXCode, DXName, Setting, sum(Events) Events, sum(Members) Members, sum(Total_Enrollment_in_Strata_Members) Total_Enrollment_in_Strata_Members, sum(Days_Covered) Days_Covered, avg(Prevalence_Rate_Users_per_1000_enrollees) Prevalence_Rate_Users_per_1000_enrollees, avg(Event_Rate_Events_per_1000_enrollees) Event_Rate_Events_per_1000_enrollees, avg(Events_Per_member) Events_Per_member", "", "AgeGroup, Sex,Period, DXCode, DXName, Setting");
                    ds.Tables.RemoveAt(0);
                    break;
                case SummaryRequestType.Incident_ICD9Diagnosis:
                    dsHelper.SelectGroupByInto("Results", ds.Tables[0], "AgeGroup,Sex, Period, DXCode, DXName, Setting, sum(Members90) Members90, sum(Members180) Members180, sum(Members270) Members270, sum(Total_Enrollment_in_Strata_Members) Total_Enrollment_in_Strata_Members, sum(Days_Covered) Days_Covered", "", "AgeGroup, Sex,Period, DXCode, DXName, Setting");
                    ds.Tables.RemoveAt(0);
                    break;

                case SummaryRequestType.ICD9Procedures:
                case SummaryRequestType.ICD9Procedures_4_digit:
                    dsHelper.SelectGroupByInto("Results", ds.Tables[0], "AgeGroup, Sex, Period, PXCode, PXName, Setting, sum(Events) Events, sum(Members) Members, sum(Total_Enrollment_in_Strata_Members) Total_Enrollment_in_Strata_Members, sum(Days_Covered) Days_Covered, avg(Prevalence_Rate_Users_per_1000_enrollees) Prevalence_Rate_Users_per_1000_enrollees, avg(Event_Rate_Events_per_1000_enrollees) Event_Rate_Events_per_1000_enrollees, avg(Events_Per_member) Events_Per_member", "", "AgeGroup, Sex, Period, PXCode, PXName, Setting");
                    ds.Tables.RemoveAt(0);
                    break;

                case SummaryRequestType.HCPCSProcedures:
                    dsHelper.SelectGroupByInto("Results", ds.Tables[0], "AgeGroup, Sex, Period, PXCode, PXName, Setting, sum(Events) Events, sum(Members) Members, sum(Total_Enrollment_in_Strata_Members) Total_Enrollment_in_Strata_Members, sum(Days_Covered) Days_Covered, avg(Prevalence_Rate_Users_per_1000_enrollees) Prevalence_Rate_Users_per_1000_enrollees, avg(Event_Rate_Events_per_1000_enrollees) Event_Rate_Events_per_1000_enrollees, avg(Events_Per_member) Events_Per_member", "", "AgeGroup, Sex, Period, PXCode, PXName, Setting");
                    ds.Tables.RemoveAt(0);
                    break;

                case SummaryRequestType.EligibilityAndEnrollment:
                    dsHelper.SelectGroupByInto("Results", ds.Tables[0], "AgeGroup, Sex, DrugCoverage, MedicalCoverage, Year, sum(Members) Members", "", "AgeGroup, Sex, DrugCoverage, MedicalCoverage, Year");
                    ds.Tables.RemoveAt(0);
                    break;

                default:
                    break;
            }
        }

    }
}
