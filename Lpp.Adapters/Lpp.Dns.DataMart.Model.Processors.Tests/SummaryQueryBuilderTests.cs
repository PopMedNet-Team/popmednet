using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lpp.Dns.DataMart.Model
{
    [TestClass]
    public class SummaryQueryBuilderTests
    {
        private static SummaryQueryBuilder builder = SummaryQueryBuilder.Instance;

        [TestMethod]
        public void Incidence_MetadataRefreshDates()
        {
            RunTest(SummaryQueryBuilder.Guid_Incident_RefreshDates, "Incidence_MetadataRefreshDates", "<SummaryRequestModel></SummaryRequestModel>", true);
        }

        /// <summary>
        /// Tests Incidence: Pharmacy by Generic Name.
        /// 
        /// Names:   ANTI-INHIBITOR COAGULANT COMP.
        /// Setting: Inpatient
        /// Sex:     F
        /// Strat:   2
        /// Periods: 2008-9
        /// </summary>
        [TestMethod]
        public void Incidence_PharmacyGenericName()
        {
            RunTest(SummaryQueryBuilder.Guid_Incident_GenericName, "Incidence_PharmacyGenericName");
        }


        /// <summary>
        /// Tests Incidence: Pharmacy by Generic Name with two names.
        /// 
        /// Names:   IBUPROFEN, RIVAROXABAN
        /// Setting: Inpatient
        /// Sex:     M/F
        /// Strat:   10
        /// Periods: 2008-9
        /// </summary>
        [TestMethod]
        public void Incidence_PharmacyGenericName2()
        {
            RunTest(SummaryQueryBuilder.Guid_Incident_GenericName, "Incidence_PharmacyGenericName2");
        }


        /// <summary>
        /// Tests Incidence: Pharmacy by Generic Name with duplicate names.
        /// 
        /// Names:   IBUPROFEN, RIVAROXABAN, IBUPROFEN
        /// Setting: Inpatient
        /// Sex:     M/F
        /// Strat:   2
        /// Periods: 2008-9
        /// </summary>
        [TestMethod]
        public void Incidence_PharmacyGenericName3()
        {
            RunTest(SummaryQueryBuilder.Guid_Incident_GenericName, "Incidence_PharmacyGenericName3");
        }

        /// <summary>
        /// Tests Incidence: ICD9 Diagnosis 3 Digits where one of the codes does not have data. This test ensures that the code without data
        /// still shows as a row.
        /// 
        /// Codes:   140, 141, E951 (E951 has no data in demo database)
        /// Setting: Inpatient
        /// Sex:     M/F
        /// Strat:   4
        /// Periods: 2008-9
        /// </summary>
        [TestMethod]
        public void Incidence_ICD9Diagnosis()
        {
            RunTest(SummaryQueryBuilder.Guid_Incident_ICD9Diagnosis, "Incidence_ICD9Diagnosis");
        }

        /// <summary>
        /// Tests Incidence: ICD9 Diagnosis 3 Digits where one of the codes does not have data. This test ensures that the code without data
        /// still shows as a row.
        /// 
        /// Codes:   140, 141, E951 (E951 has no data in demo database)
        /// Setting: Emergency
        /// Sex:     M/F
        /// Strat:   4
        /// Periods: 2008-9
        /// </summary>
        [TestMethod]
        public void Incidence_ICD9Diagnosis2()
        {
            RunTest(SummaryQueryBuilder.Guid_Incident_ICD9Diagnosis, "Incidence_ICD9Diagnosis2");
        }

        /// <summary>
        /// Tests Incidence: ICD9 Diagnosis 3 Digits with 0 age stratification and aggregated genders. This test checks that the enrollment values
        /// for all rows for the same year have the same total enrollment.
        /// 
        /// Codes:   140, 141, E951
        /// Setting: Emergency
        /// Sex:     M/F Aggr
        /// Strat:   0
        /// Periods: 2008-9
        /// </summary>
        [TestMethod]
        public void Incidence_ICD9Diagnosis3()
        {
            RunTest(SummaryQueryBuilder.Guid_Incident_ICD9Diagnosis, "Incidence_ICD9Diagnosis3");
        }

        /// <summary>
        /// Tests Incidence: ICD9 Diagnosis 3 Digits with 10 age stratification and aggregated genders.
        /// 
        /// Codes:   140
        /// Setting: Inpatient
        /// Sex:     M/F Aggr
        /// Strat:   10
        /// Periods: 2008-9
        /// </summary>
        [TestMethod]
        public void Incidence_ICD9Diagnosis4()
        {
            RunTest(SummaryQueryBuilder.Guid_Incident_ICD9Diagnosis, "Incidence_ICD9Diagnosis4");
        }

        /// <summary>
        /// Tests MFU: ICD9 Diagnosis 4 Digits with output criteria of 20.
        /// 
        /// Metric Type: Event
        /// Output Criteria: 20
        /// Setting: Inpatient
        /// Sex:     M/F
        /// Strat:   10
        /// Periods: 2008
        /// </summary>
        [TestMethod]
        public void MFU_ICD9Diagnosis4Digits()
        {
            RunTest(SummaryQueryBuilder.Guid_MFU_ICD9Diagnosis_4_digit, "MFU_ICD9Diagnosis4Digits");
        }

        /// <summary>
        /// Tests MFU: ICD9 Diagnosis 5 Digits with 2 stratification and MF Aggregation.
        /// 
        /// Metric Type: Event
        /// Output Criteria: 10
        /// Setting: Inpatient
        /// Sex:     M/F Aggr
        /// Strat:   2
        /// Periods: 2009
        /// </summary>
        [TestMethod]
        public void MFU_ICD9Diagnosis5Digits()
        {
            RunTest(SummaryQueryBuilder.Guid_MFU_ICD9Diagnosis_5_digit, "MFU_ICD9Diagnosis5Digits");
        }

        /// <summary>
        /// Tests MFU: ICD9 Diagnosis 5 Digits with 2 stratification and MF Aggregation.
        /// 
        /// Metric Type: User
        /// Output Criteria: 10
        /// Setting: Inpatient
        /// Sex:     M/F Aggr
        /// Strat:   2
        /// Periods: 2009
        /// </summary>
        [TestMethod]
        public void MFU_ICD9Diagnosis5Digits2()
        {
            RunTest(SummaryQueryBuilder.Guid_MFU_ICD9Diagnosis_5_digit, "MFU_ICD9Diagnosis5Digits2");
        }

        /// <summary>
        /// Tests MFU: ICD9 Diagnosis 5 Digits with 0 stratification and MF Aggregation.
        /// 
        /// Metric Type: User
        /// Output Criteria: 10
        /// Setting: Inpatient
        /// Sex:     M/F Aggr
        /// Strat:   0
        /// Periods: 2009
        /// </summary>
        [TestMethod]
        public void MFU_ICD9Diagnosis5Digits3()
        {
            RunTest(SummaryQueryBuilder.Guid_MFU_ICD9Diagnosis_5_digit, "MFU_ICD9Diagnosis5Digits3");
        }

        private void RunTest(Guid requestTypeId, string testName, string argsXml=null, bool isMetadata=false)
        {
            try
            {
                string[] queries = builder.BuildSQLQueries(requestTypeId, argsXml ?? GetXml(testName + "_Args"), isMetadata, Lpp.Dns.DataMart.Model.Settings.SQLProvider.ODBC);
                DataSet ds = ExecuteQueries(queries);
                Assert.AreEqual(GetXml(testName), DSToXml(ds));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        private string GetXml(string testName)
        {
            using (StreamReader r = new StreamReader(this.GetType().Assembly.GetManifestResourceStream("Lpp.Dns.DataMart.Model.Processors.Tests.SummaryQuery.ResultReference." + testName + ".xml")))
            {
                return r.ReadToEnd().Trim();
            }
        }

        private string DSToXml(DataSet ds)
        {
            var stream = new MemoryStream();
            ds.WriteXml(stream, XmlWriteMode.WriteSchema);
            stream.Seek(0, SeekOrigin.Begin);
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd().Trim();
            }
        }

        private DataSet ExecuteQueries(string[] queries)
        {
            DataSet _ds = new DataSet();
            foreach (var query in queries)
            {
                if (!string.IsNullOrEmpty(query.Trim()))
                {
                    DataSet ds = ExecuteQuery(query);
                    _ds.Merge(ds.Tables[0]);
                }
            }

            return _ds;
        }

        private DataSet ExecuteQuery(string QueryText)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DemonstrationQueryTool"].ConnectionString;
            OdbcConnection cn = new OdbcConnection(connectionString);
            try
            {
                OdbcDataAdapter da = new OdbcDataAdapter(QueryText, cn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            finally
            {
                cn.Close();
            }
        }

    }
}
