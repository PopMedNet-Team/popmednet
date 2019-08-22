using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Tests
{
    /// <summary>
    /// Test class for parsing Request.json and determining structure of query.
    /// </summary>
    [TestClass]
    public class QueryCompositionTests
    {
        const string ResourceFolder = "../Resources/QueryComposition";
        static readonly string MSSqlConnectionString;
        static readonly string PostgreSQLConnectionString;
        static readonly string MySQLConnectionString;
        static readonly string OracleConnectionString;
        static readonly log4net.ILog Logger;

        const bool RunPostgreSQL = true;
        const bool RunOracle = false;
        const bool RunMySql = false;

        static QueryCompositionTests()
        {
            MSSqlConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PCORNET"].ConnectionString;
            PostgreSQLConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PCORNET_PostgreSQL"].ConnectionString;
            MySQLConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PCORNET_MySQL"].ConnectionString;
            OracleConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PCORNET_ORACLE"].ConnectionString;

            log4net.Config.XmlConfigurator.Configure();
            Logger = log4net.LogManager.GetLogger(typeof(QueryCompositionTests));
        }

        [TestMethod]
        public void PMNDEV_6347()
        {
            string filename = "PMNDEV_6347.json";
            //var response = RunRequest(filename);
            //Logger.Debug(SerializeJsonToString(response));

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL, "PCORNET_MAIN_TEST_3_1");
                Assert.IsNull(npgsqlResponse.Errors);
                //Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void PMNDEV_6287_66()
        {
            string filename = "PMNDEV_6287_6_6.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            //var table = response.Results.First();
            //var row = table.First();

            //Assert.IsTrue(row.ContainsKey("Sex"));

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                //Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                //Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                //Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void PMNDEV_6287_532()
        {
            string filename = "PMNDEV_6287_5_3_2.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            //var table = response.Results.First();
            //var row = table.First();

            //Assert.IsTrue(row.ContainsKey("Sex"));

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                //Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                //Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                //Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        /// <summary>
        /// Looks for Patients with Diagnosis 250% or Procedure 100% with Encounter Observation Period 2008-01-01 to 2010-12-31 AND Outpatient.
        /// </summary>
        [TestMethod]
        public void PMNMAINT_579_001()
        {
            string filename = "PMNMAINT-579_001.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();
            var row = table.First();

            Assert.IsTrue(row.ContainsKey("Sex"));

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(
                table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        /// <summary>
        /// Diagnosis is ICD9 Starts with 250
        /// OR Procedure is Any Starts with 100
        /// OR Diagnosis is ICD9 Starts with 401
        /// OR Procedure is Any Starts with 992
        /// AND Observation Period year 2008 or 2010
        /// AND Setting is AV, IP, or ED.
        /// </summary>
        [TestMethod]
        public void PMNMAINT_579_002()
        {
            string filename = "PMNMAINT-579_002.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();
            var row = table.First();

            Assert.IsTrue(row.ContainsKey("Sex"));

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(
                table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        /// <summary>
        /// Setting is Outpatient (Ambulatory) OR Inpatient Hospital Stay
        /// AND Observation Period: 2008/1/1 to 2010/12/31
        /// </summary>
        [TestMethod]
        public void PMNMAINT_579_003()
        {
            string filename = "PMNMAINT-579_003.json";
            //var response = RunRequest(filename);
            //Logger.Debug(SerializeJsonToString(response));

            //Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            //var table = response.Results.First();
            //var row = table.First();

            //Assert.IsTrue(row.ContainsKey("Sex"));

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                //Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                //Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                //Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        /// <summary>
        /// Diagnosis Starts with 250 (ICD-9)
        /// OR Diagnosis Starts with 401 (ICD-9)
        /// OR Procedure Starts with 100
        /// OR Procedure Starts with 200
        /// </summary>
        [TestMethod]
        public void PMNMAINT_579_004()
        {
            string filename = "PMNMAINT-579_004.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();
            var row = table.First();

            Assert.IsTrue(row.ContainsKey("Sex"));

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(
                table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        /// <summary>
        /// Criteria Group 1:
        /// Setting = Emergency Department AND
        /// Observation Period: 2008/1/1 to 2008/12/31
        /// AND (Diagnosis is ICD-9 Starts With 250 "OR" Procedure is CPT or HCPCS Starts With 40)
        /// 
        /// Criteria Group 2:
        /// Observation Period: 2010/1/1 to 2010/12/31
        /// AND Procedure Starts with 80.
        /// </summary>
        [TestMethod]
        public void PMNMAINT_579_005()
        {
            string filename = "PMNMAINT-579_005.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();
            var row = table.First();

            Assert.IsTrue(row.ContainsKey("Sex"));

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(
                table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        /// <summary>
        /// Simple query that returns the fields included in the critieria; no stratification, no aggregation
        /// </summary>
        [TestMethod]
        public void SimpleSelect_NoCritieria_NoStratification_NoAggregation()
        {
            //equivalent linq:         
            //using (var db = Helper.CreatePCORIDataContext(ConnectionString))
            //{
            //    var query = db.Patients.Select(p => new { p.Sex, p.Hispanic });
            //}

            string filename = "SimpleSelect_NoCritieria_NoStratification_NoAggregation.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            //make sure that the results have two columns, with the correct names
            Assert.IsTrue(row.Keys.Count == 2);
            Assert.IsTrue(row.ContainsKey("Sex"));
            Assert.IsTrue(row.ContainsKey("Hispanic"));

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(
                table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void SimpleAggregation_NoCritieria_NoStratification_NoAggregation()
        {
            int expectedCount = 0;
            //equivalent linq:         
            using (var db = Helper.CreatePCORIDataContext(MSSqlConnectionString))
            {
                expectedCount = db.Patients.Select(p => new { p.Sex }).Count();
            }

            string filename = "SimpleAggregation_NoCritieria_NoStratification_NoAggregation.json";

            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();
            //should return a single table with a single row that contains the count.
            Assert.IsTrue(row.Keys.Count == 2);
            Assert.IsTrue(row.ContainsKey("Sex"));

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void SimpleAggregation_SameCritieria_NoStratification_NoAggregation()
        {
            //equivalent linq:
            //using (var db = Helper.CreatePCORIDataContext(ConnectionString))
            //{
            //    var query = db.Patients.Where(p => p.Sex == "M")
            //                           .Select(p => new { PatientID = p.ID, Sex = p.Sex })
            //                           .GroupBy(p => new { p.Sex })
            //                           .Select(k => new { k.Key.Sex, Count = k.Count() });

            //    Console.WriteLine(query.Expression);

            //    Console.WriteLine(query.ToString());
            //}

            string filename = "SimpleAggregation_SameCritieria_NoStratification_NoAggregation.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void SingleStratification_NoCritiera_NoAggregation()
        {
            string filename = "SingleStratification_NoCritiera_NoAggregation.json";
            var response = RunRequest(filename);

            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                //var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                //Assert.IsNull(npgsqlResponse.Errors);
                //Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                /* MySql fails due to DiffYears function */
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void SingleStratificationAndAggregation_NoCritieria()
        {
            string filename = "SingleStratification_NoCritieria_WithSelfAggregation.json";
            var response = RunRequest(filename);

            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());

            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());

            }

            if (RunMySql)
            {
                /* MySql fails due to DiffYears function */
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void ActualRequest_180191()
        {
            string filename = "180191_request.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void ActualRequest_180192()
        {
            string filename = "180192_request.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void ActualRequest_180193()
        {
            string filename = "180193_request.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");
            
            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void ActualRequest_180194()
        {
            string filename = "180194_request.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void Age_between_10_and_36_no_stratification()
        {
            //using (var db = Helper.CreatePCORIDataContext(ConnectionString))
            //{
            //    int minAge = 10; 
            //    int maxAge = 36;
            //    var query = db.Patients.Where(p => (System.Data.Entity.DbFunctions.DiffYears(p.BornOn.Value, DateTime.Now).Value - (((p.BornOn.Value.Month > DateTime.Now.Month) || (p.BornOn.Value.Month == DateTime.Now.Month && p.BornOn.Value.Day > DateTime.Now.Day)) ? 1 : 0)) <= maxAge
            //            &&
            //            (System.Data.Entity.DbFunctions.DiffYears(p.BornOn.Value, DateTime.Now).Value - (((p.BornOn.Value.Month > DateTime.Now.Month) || (p.BornOn.Value.Month == DateTime.Now.Month && p.BornOn.Value.Day > DateTime.Now.Day)) ? 1 : 0)) >= minAge
            //        );

            //    var q2 = query.Select(p => new { PatientID = p.ID, Age = (System.Data.Entity.DbFunctions.DiffYears(p.BornOn.Value, DateTime.Now).Value - (((p.BornOn.Value.Month > DateTime.Now.Month) || (p.BornOn.Value.Month == DateTime.Now.Month && p.BornOn.Value.Day > DateTime.Now.Day)) ? 1 : 0)) })
            //                  .GroupBy(p => new { Age = p.Age })
            //                  .Select(p => new { Age = p.Key.Age, Count = p.Count() });

            //    Console.WriteLine(q2.ToString());
            //}            

            string filename = "Age_between_10_and_36_no_stratification.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");
            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();
            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                ///* MySql fails due to DiffYears function */
                //var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                //Assert.IsNull(mysqlResponse.Errors);
                //Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void PMNDEV_6163_AgeRangeForPostgres()
        {
            var npgsqlResponse = RunRequest("PMNDEV-6163.json", PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
            Assert.IsNull(npgsqlResponse.Errors);
            Assert.IsTrue(npgsqlResponse.Results != null && npgsqlResponse.Results.Count() > 0);
        }

        [TestMethod]
        public void WeightBetween_10_and_150_lb()
        {
            string filename = "Weight_10_to_150_lb.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();

            /* oracle fails
             Test Name:	WeightBetween_10_and_150_lb
Test FullName:	Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.WeightBetween_10_and_150_lb
Test Source:	c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer.Tests\QueryCompositionTests.cs : line 387
Test Outcome:	Failed
Test Duration:	0:00:02.6861947

Result Message:	
Test method Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.WeightBetween_10_and_150_lb threw exception: 
System.Data.Entity.Core.EntityCommandExecutionException: An error occurred while executing the command definition. See the inner exception for details. ---> Oracle.ManagedDataAccess.Client.OracleException: ORA-00932: inconsistent datatypes: expected NCHAR got NCLOB
Result StackTrace:	
at OracleInternal.ServiceObjects.OracleCommandImpl.VerifyExecution(OracleConnectionImpl connectionImpl, Int32& cursorId, Boolean bThrowArrayBindRelatedErrors, OracleException& exceptionForArrayBindDML, Boolean& hasMoreRowsInDB, Boolean bFirstIterationDone)
   at OracleInternal.ServiceObjects.OracleCommandImpl.ExecuteReader(String commandText, OracleParameterCollection paramColl, CommandType commandType, OracleConnectionImpl connectionImpl, OracleDataReaderImpl& rdrImpl, Int32 longFetchSize, Int64 clientInitialLOBFS, OracleDependencyImpl orclDependencyImpl, Int64[] scnForExecution, Int64[]& scnFromExecution, OracleParameterCollection& bindByPositionParamColl, Boolean& bBindParamPresent, Int64& internalInitialLOBFS, OracleException& exceptionForArrayBindDML, Boolean isDescribeOnly, Boolean isFromEF)
   at Oracle.ManagedDataAccess.Client.OracleCommand.ExecuteReader(Boolean requery, Boolean fillRequest, CommandBehavior behavior)
   at Oracle.ManagedDataAccess.Client.OracleCommand.ExecuteDbDataReader(CommandBehavior behavior)
   at System.Data.Common.DbCommand.ExecuteReader(CommandBehavior behavior)
   at System.Data.Entity.Infrastructure.Interception.DbCommandDispatcher.<Reader>b__c(DbCommand t, DbCommandInterceptionContext`1 c)
   at System.Data.Entity.Infrastructure.Interception.InternalDispatcher`1.Dispatch[TTarget,TInterceptionContext,TResult](TTarget target, Func`3 operation, TInterceptionContext interceptionContext, Action`3 executing, Action`3 executed)
   at System.Data.Entity.Infrastructure.Interception.DbCommandDispatcher.Reader(DbCommand command, DbCommandInterceptionContext interceptionContext)
   at System.Data.Entity.Internal.InterceptableDbCommand.ExecuteDbDataReader(CommandBehavior behavior)
   at System.Data.Common.DbCommand.ExecuteReader(CommandBehavior behavior)
   at System.Data.Entity.Core.EntityClient.Internal.EntityCommandDefinition.ExecuteStoreCommands(EntityCommand entityCommand, CommandBehavior behavior)
 --- End of inner exception stack trace ---
    at System.Data.Entity.Core.EntityClient.Internal.EntityCommandDefinition.ExecuteStoreCommands(EntityCommand entityCommand, CommandBehavior behavior)
   at System.Data.Entity.Core.Objects.Internal.ObjectQueryExecutionPlan.Execute[TResultType](ObjectContext context, ObjectParameterCollection parameterValues)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__6()
   at System.Data.Entity.Core.Objects.ObjectContext.ExecuteInTransaction[T](Func`1 func, IDbExecutionStrategy executionStrategy, Boolean startLocalTransaction, Boolean releaseConnectionOnSuccess)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__5()
   at System.Data.Entity.Infrastructure.DefaultExecutionStrategy.Execute[TResult](Func`1 operation)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.GetResults(Nullable`1 forMergeOption)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<System.Collections.Generic.IEnumerable<T>.GetEnumerator>b__0()
   at System.Data.Entity.Internal.LazyEnumerator`1.MoveNext()
   at Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI.PCORIModelAdapter.Execute(QueryComposerRequestDTO request, Boolean viewSQL) in c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer\Adapters\PCORI\PCORIModelAdapter.cs:line 231
   at Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.RunRequest(String requestJsonFilepath, String connectionString, SQLProvider sqlProvider) in c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer.Tests\QueryCompositionTests.cs:line 700
   at Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.WeightBetween_10_and_150_lb() in c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer.Tests\QueryCompositionTests.cs:line 399            
             */
            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void MenBetween_36_66_InchTall()
        {
            string filename = "Men_36_66_Inches.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();

            /*Oracle fails:
             Test Name:	MenBetween_36_66_InchTall
Test FullName:	Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.MenBetween_36_66_InchTall
Test Source:	c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer.Tests\QueryCompositionTests.cs : line 450
Test Outcome:	Failed
Test Duration:	0:00:02.7317655

Result Message:	
Test method Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.MenBetween_36_66_InchTall threw exception: 
System.Data.Entity.Core.EntityCommandExecutionException: An error occurred while executing the command definition. See the inner exception for details. ---> Oracle.ManagedDataAccess.Client.OracleException: ORA-00932: inconsistent datatypes: expected NCHAR got NCLOB
Result StackTrace:	
at OracleInternal.ServiceObjects.OracleCommandImpl.VerifyExecution(OracleConnectionImpl connectionImpl, Int32& cursorId, Boolean bThrowArrayBindRelatedErrors, OracleException& exceptionForArrayBindDML, Boolean& hasMoreRowsInDB, Boolean bFirstIterationDone)
   at OracleInternal.ServiceObjects.OracleCommandImpl.ExecuteReader(String commandText, OracleParameterCollection paramColl, CommandType commandType, OracleConnectionImpl connectionImpl, OracleDataReaderImpl& rdrImpl, Int32 longFetchSize, Int64 clientInitialLOBFS, OracleDependencyImpl orclDependencyImpl, Int64[] scnForExecution, Int64[]& scnFromExecution, OracleParameterCollection& bindByPositionParamColl, Boolean& bBindParamPresent, Int64& internalInitialLOBFS, OracleException& exceptionForArrayBindDML, Boolean isDescribeOnly, Boolean isFromEF)
   at Oracle.ManagedDataAccess.Client.OracleCommand.ExecuteReader(Boolean requery, Boolean fillRequest, CommandBehavior behavior)
   at Oracle.ManagedDataAccess.Client.OracleCommand.ExecuteDbDataReader(CommandBehavior behavior)
   at System.Data.Common.DbCommand.ExecuteReader(CommandBehavior behavior)
   at System.Data.Entity.Infrastructure.Interception.DbCommandDispatcher.<Reader>b__c(DbCommand t, DbCommandInterceptionContext`1 c)
   at System.Data.Entity.Infrastructure.Interception.InternalDispatcher`1.Dispatch[TTarget,TInterceptionContext,TResult](TTarget target, Func`3 operation, TInterceptionContext interceptionContext, Action`3 executing, Action`3 executed)
   at System.Data.Entity.Infrastructure.Interception.DbCommandDispatcher.Reader(DbCommand command, DbCommandInterceptionContext interceptionContext)
   at System.Data.Entity.Internal.InterceptableDbCommand.ExecuteDbDataReader(CommandBehavior behavior)
   at System.Data.Common.DbCommand.ExecuteReader(CommandBehavior behavior)
   at System.Data.Entity.Core.EntityClient.Internal.EntityCommandDefinition.ExecuteStoreCommands(EntityCommand entityCommand, CommandBehavior behavior)
 --- End of inner exception stack trace ---
    at System.Data.Entity.Core.EntityClient.Internal.EntityCommandDefinition.ExecuteStoreCommands(EntityCommand entityCommand, CommandBehavior behavior)
   at System.Data.Entity.Core.Objects.Internal.ObjectQueryExecutionPlan.Execute[TResultType](ObjectContext context, ObjectParameterCollection parameterValues)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__6()
   at System.Data.Entity.Core.Objects.ObjectContext.ExecuteInTransaction[T](Func`1 func, IDbExecutionStrategy executionStrategy, Boolean startLocalTransaction, Boolean releaseConnectionOnSuccess)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__5()
   at System.Data.Entity.Infrastructure.DefaultExecutionStrategy.Execute[TResult](Func`1 operation)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.GetResults(Nullable`1 forMergeOption)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<System.Collections.Generic.IEnumerable<T>.GetEnumerator>b__0()
   at System.Data.Entity.Internal.LazyEnumerator`1.MoveNext()
   at Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI.PCORIModelAdapter.Execute(QueryComposerRequestDTO request, Boolean viewSQL) in c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer\Adapters\PCORI\PCORIModelAdapter.cs:line 231
   at Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.RunRequest(String requestJsonFilepath, String connectionString, SQLProvider sqlProvider) in c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer.Tests\QueryCompositionTests.cs:line 736
   at Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.MenBetween_36_66_InchTall() in c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer.Tests\QueryCompositionTests.cs:line 462
             */

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void Patients_with_Observation_Period()
        {
            string filename = "Patients_with_Observation_Period.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();
            /*Oracle fails:
             Test Name:	Patients_with_Observation_Period
Test FullName:	Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.Patients_with_Observation_Period
Test Source:	c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer.Tests\QueryCompositionTests.cs : line 513
Test Outcome:	Failed
Test Duration:	0:00:03.8140572

Result Message:	
Test method Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.Patients_with_Observation_Period threw exception: 
System.Data.Entity.Core.EntityCommandExecutionException: An error occurred while executing the command definition. See the inner exception for details. ---> Oracle.ManagedDataAccess.Client.OracleException: ORA-00932: inconsistent datatypes: expected NCHAR got NCLOB
Result StackTrace:	
at OracleInternal.ServiceObjects.OracleCommandImpl.VerifyExecution(OracleConnectionImpl connectionImpl, Int32& cursorId, Boolean bThrowArrayBindRelatedErrors, OracleException& exceptionForArrayBindDML, Boolean& hasMoreRowsInDB, Boolean bFirstIterationDone)
   at OracleInternal.ServiceObjects.OracleCommandImpl.ExecuteReader(String commandText, OracleParameterCollection paramColl, CommandType commandType, OracleConnectionImpl connectionImpl, OracleDataReaderImpl& rdrImpl, Int32 longFetchSize, Int64 clientInitialLOBFS, OracleDependencyImpl orclDependencyImpl, Int64[] scnForExecution, Int64[]& scnFromExecution, OracleParameterCollection& bindByPositionParamColl, Boolean& bBindParamPresent, Int64& internalInitialLOBFS, OracleException& exceptionForArrayBindDML, Boolean isDescribeOnly, Boolean isFromEF)
   at Oracle.ManagedDataAccess.Client.OracleCommand.ExecuteReader(Boolean requery, Boolean fillRequest, CommandBehavior behavior)
   at Oracle.ManagedDataAccess.Client.OracleCommand.ExecuteDbDataReader(CommandBehavior behavior)
   at System.Data.Common.DbCommand.ExecuteReader(CommandBehavior behavior)
   at System.Data.Entity.Infrastructure.Interception.DbCommandDispatcher.<Reader>b__c(DbCommand t, DbCommandInterceptionContext`1 c)
   at System.Data.Entity.Infrastructure.Interception.InternalDispatcher`1.Dispatch[TTarget,TInterceptionContext,TResult](TTarget target, Func`3 operation, TInterceptionContext interceptionContext, Action`3 executing, Action`3 executed)
   at System.Data.Entity.Infrastructure.Interception.DbCommandDispatcher.Reader(DbCommand command, DbCommandInterceptionContext interceptionContext)
   at System.Data.Entity.Internal.InterceptableDbCommand.ExecuteDbDataReader(CommandBehavior behavior)
   at System.Data.Common.DbCommand.ExecuteReader(CommandBehavior behavior)
   at System.Data.Entity.Core.EntityClient.Internal.EntityCommandDefinition.ExecuteStoreCommands(EntityCommand entityCommand, CommandBehavior behavior)
 --- End of inner exception stack trace ---
    at System.Data.Entity.Core.EntityClient.Internal.EntityCommandDefinition.ExecuteStoreCommands(EntityCommand entityCommand, CommandBehavior behavior)
   at System.Data.Entity.Core.Objects.Internal.ObjectQueryExecutionPlan.Execute[TResultType](ObjectContext context, ObjectParameterCollection parameterValues)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__6()
   at System.Data.Entity.Core.Objects.ObjectContext.ExecuteInTransaction[T](Func`1 func, IDbExecutionStrategy executionStrategy, Boolean startLocalTransaction, Boolean releaseConnectionOnSuccess)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__5()
   at System.Data.Entity.Infrastructure.DefaultExecutionStrategy.Execute[TResult](Func`1 operation)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.GetResults(Nullable`1 forMergeOption)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<System.Collections.Generic.IEnumerable<T>.GetEnumerator>b__0()
   at System.Data.Entity.Internal.LazyEnumerator`1.MoveNext()
   at Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI.PCORIModelAdapter.Execute(QueryComposerRequestDTO request, Boolean viewSQL) in c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer\Adapters\PCORI\PCORIModelAdapter.cs:line 231
   at Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.RunRequest(String requestJsonFilepath, String connectionString, SQLProvider sqlProvider) in c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer.Tests\QueryCompositionTests.cs:line 772
   at Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.Patients_with_Observation_Period() in c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer.Tests\QueryCompositionTests.cs:line 525
             */
            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }
            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }
            /* MySql Fails:
             Test Name:	Patients_with_Observation_Period
Test FullName:	Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.Patients_with_Observation_Period
Test Source:	c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer.Tests\QueryCompositionTests.cs : line 590
Test Outcome:	Failed
Test Duration:	0:00:04.3553115

Result Message:	
Test method Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.Patients_with_Observation_Period threw exception: 
System.Data.Entity.Core.EntityCommandExecutionException: An error occurred while executing the command definition. See the inner exception for details. ---> MySql.Data.MySqlClient.MySqlException: Unknown column 'Project2.C1' in 'field list'
Result StackTrace:	
at MySql.Data.MySqlClient.MySqlStream.ReadPacket()
   at MySql.Data.MySqlClient.NativeDriver.GetResult(Int32& affectedRow, Int64& insertedId)
   at MySql.Data.MySqlClient.Driver.GetResult(Int32 statementId, Int32& affectedRows, Int64& insertedId)
   at MySql.Data.MySqlClient.Driver.NextResult(Int32 statementId, Boolean force)
   at MySql.Data.MySqlClient.MySqlDataReader.NextResult()
   at MySql.Data.MySqlClient.MySqlCommand.ExecuteReader(CommandBehavior behavior)
   at MySql.Data.Entity.EFMySqlCommand.ExecuteDbDataReader(CommandBehavior behavior)
   at System.Data.Common.DbCommand.ExecuteReader(CommandBehavior behavior)
   at System.Data.Entity.Infrastructure.Interception.DbCommandDispatcher.<Reader>b__c(DbCommand t, DbCommandInterceptionContext`1 c)
   at System.Data.Entity.Infrastructure.Interception.InternalDispatcher`1.Dispatch[TTarget,TInterceptionContext,TResult](TTarget target, Func`3 operation, TInterceptionContext interceptionContext, Action`3 executing, Action`3 executed)
   at System.Data.Entity.Infrastructure.Interception.DbCommandDispatcher.Reader(DbCommand command, DbCommandInterceptionContext interceptionContext)
   at System.Data.Entity.Internal.InterceptableDbCommand.ExecuteDbDataReader(CommandBehavior behavior)
   at System.Data.Common.DbCommand.ExecuteReader(CommandBehavior behavior)
   at System.Data.Entity.Core.EntityClient.Internal.EntityCommandDefinition.ExecuteStoreCommands(EntityCommand entityCommand, CommandBehavior behavior)
 --- End of inner exception stack trace ---
    at System.Data.Entity.Core.EntityClient.Internal.EntityCommandDefinition.ExecuteStoreCommands(EntityCommand entityCommand, CommandBehavior behavior)
   at System.Data.Entity.Core.Objects.Internal.ObjectQueryExecutionPlan.Execute[TResultType](ObjectContext context, ObjectParameterCollection parameterValues)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__6()
   at System.Data.Entity.Core.Objects.ObjectContext.ExecuteInTransaction[T](Func`1 func, IDbExecutionStrategy executionStrategy, Boolean startLocalTransaction, Boolean releaseConnectionOnSuccess)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__5()
   at System.Data.Entity.Infrastructure.DefaultExecutionStrategy.Execute[TResult](Func`1 operation)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.GetResults(Nullable`1 forMergeOption)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<System.Collections.Generic.IEnumerable<T>.GetEnumerator>b__0()
   at System.Data.Entity.Internal.LazyEnumerator`1.MoveNext()
   at Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI.PCORIModelAdapter.Execute(QueryComposerRequestDTO request, Boolean viewSQL) in c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer\Adapters\PCORI\PCORIModelAdapter.cs:line 231
   at Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.RunRequest(String requestJsonFilepath, String connectionString, SQLProvider sqlProvider) in c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer.Tests\QueryCompositionTests.cs:line 1223
   at Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.Patients_with_Observation_Period() in c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer.Tests\QueryCompositionTests.cs:line 694


*/
            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void Patients_between_36_and_66_tall_with_Observation_Period()
        {
            string filename = "Patients_between_36_and_66_tall_with_Observation_Period.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            //Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();

            /*Oracle fails:
             Test Name:	Patients_between_36_and_66_tall_with_Observation_Period
Test FullName:	Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.Patients_between_36_and_66_tall_with_Observation_Period
Test Source:	c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer.Tests\QueryCompositionTests.cs : line 618
Test Outcome:	Failed
Test Duration:	0:00:02.9010126

Result Message:	
Test method Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.Patients_between_36_and_66_tall_with_Observation_Period threw exception: 
System.Data.Entity.Core.EntityCommandExecutionException: An error occurred while executing the command definition. See the inner exception for details. ---> Oracle.ManagedDataAccess.Client.OracleException: ORA-00932: inconsistent datatypes: expected NCHAR got NCLOB
Result StackTrace:	
at OracleInternal.ServiceObjects.OracleCommandImpl.VerifyExecution(OracleConnectionImpl connectionImpl, Int32& cursorId, Boolean bThrowArrayBindRelatedErrors, OracleException& exceptionForArrayBindDML, Boolean& hasMoreRowsInDB, Boolean bFirstIterationDone)
   at OracleInternal.ServiceObjects.OracleCommandImpl.ExecuteReader(String commandText, OracleParameterCollection paramColl, CommandType commandType, OracleConnectionImpl connectionImpl, OracleDataReaderImpl& rdrImpl, Int32 longFetchSize, Int64 clientInitialLOBFS, OracleDependencyImpl orclDependencyImpl, Int64[] scnForExecution, Int64[]& scnFromExecution, OracleParameterCollection& bindByPositionParamColl, Boolean& bBindParamPresent, Int64& internalInitialLOBFS, OracleException& exceptionForArrayBindDML, Boolean isDescribeOnly, Boolean isFromEF)
   at Oracle.ManagedDataAccess.Client.OracleCommand.ExecuteReader(Boolean requery, Boolean fillRequest, CommandBehavior behavior)
   at Oracle.ManagedDataAccess.Client.OracleCommand.ExecuteDbDataReader(CommandBehavior behavior)
   at System.Data.Common.DbCommand.ExecuteReader(CommandBehavior behavior)
   at System.Data.Entity.Infrastructure.Interception.DbCommandDispatcher.<Reader>b__c(DbCommand t, DbCommandInterceptionContext`1 c)
   at System.Data.Entity.Infrastructure.Interception.InternalDispatcher`1.Dispatch[TTarget,TInterceptionContext,TResult](TTarget target, Func`3 operation, TInterceptionContext interceptionContext, Action`3 executing, Action`3 executed)
   at System.Data.Entity.Infrastructure.Interception.DbCommandDispatcher.Reader(DbCommand command, DbCommandInterceptionContext interceptionContext)
   at System.Data.Entity.Internal.InterceptableDbCommand.ExecuteDbDataReader(CommandBehavior behavior)
   at System.Data.Common.DbCommand.ExecuteReader(CommandBehavior behavior)
   at System.Data.Entity.Core.EntityClient.Internal.EntityCommandDefinition.ExecuteStoreCommands(EntityCommand entityCommand, CommandBehavior behavior)
 --- End of inner exception stack trace ---
    at System.Data.Entity.Core.EntityClient.Internal.EntityCommandDefinition.ExecuteStoreCommands(EntityCommand entityCommand, CommandBehavior behavior)
   at System.Data.Entity.Core.Objects.Internal.ObjectQueryExecutionPlan.Execute[TResultType](ObjectContext context, ObjectParameterCollection parameterValues)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__6()
   at System.Data.Entity.Core.Objects.ObjectContext.ExecuteInTransaction[T](Func`1 func, IDbExecutionStrategy executionStrategy, Boolean startLocalTransaction, Boolean releaseConnectionOnSuccess)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__5()
   at System.Data.Entity.Infrastructure.DefaultExecutionStrategy.Execute[TResult](Func`1 operation)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.GetResults(Nullable`1 forMergeOption)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<System.Collections.Generic.IEnumerable<T>.GetEnumerator>b__0()
   at System.Data.Entity.Internal.LazyEnumerator`1.MoveNext()
   at Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI.PCORIModelAdapter.Execute(QueryComposerRequestDTO request, Boolean viewSQL) in c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer\Adapters\PCORI\PCORIModelAdapter.cs:line 231
   at Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.RunRequest(String requestJsonFilepath, String connectionString, SQLProvider sqlProvider) in c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer.Tests\QueryCompositionTests.cs:line 850
   at Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.Patients_between_36_and_66_tall_with_Observation_Period() in c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer.Tests\QueryCompositionTests.cs:line 630
*/
            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void Patients_with_Setting_and_Sex()
        {
            string filename = "Setting and Sex.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            //Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void Height_with_Measure_Date()
        {
            string filename = "Height_with_Measure_Date.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            //Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void CombinedDiagnosisCodes()
        {
            string filename = "CombinedDiagnosisCodes.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            //Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            /*mysql fails:
             Test Name:	CombinedDiagnosisCodes
Test FullName:	Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.CombinedDiagnosisCodes
Test Source:	c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer.Tests\QueryCompositionTests.cs : line 735
Test Outcome:	Failed
Test Duration:	0:00:08.6419965

Result Message:	
Test method Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.CombinedDiagnosisCodes threw exception: 
System.Data.Entity.Core.EntityCommandExecutionException: An error occurred while executing the command definition. See the inner exception for details. ---> MySql.Data.MySqlClient.MySqlException: Can't group on 'A1'
Result StackTrace:	
at MySql.Data.MySqlClient.MySqlStream.ReadPacket()
   at MySql.Data.MySqlClient.NativeDriver.GetResult(Int32& affectedRow, Int64& insertedId)
   at MySql.Data.MySqlClient.Driver.GetResult(Int32 statementId, Int32& affectedRows, Int64& insertedId)
   at MySql.Data.MySqlClient.Driver.NextResult(Int32 statementId, Boolean force)
   at MySql.Data.MySqlClient.MySqlDataReader.NextResult()
   at MySql.Data.MySqlClient.MySqlCommand.ExecuteReader(CommandBehavior behavior)
   at MySql.Data.Entity.EFMySqlCommand.ExecuteDbDataReader(CommandBehavior behavior)
   at System.Data.Common.DbCommand.ExecuteReader(CommandBehavior behavior)
   at System.Data.Entity.Infrastructure.Interception.DbCommandDispatcher.<Reader>b__c(DbCommand t, DbCommandInterceptionContext`1 c)
   at System.Data.Entity.Infrastructure.Interception.InternalDispatcher`1.Dispatch[TTarget,TInterceptionContext,TResult](TTarget target, Func`3 operation, TInterceptionContext interceptionContext, Action`3 executing, Action`3 executed)
   at System.Data.Entity.Infrastructure.Interception.DbCommandDispatcher.Reader(DbCommand command, DbCommandInterceptionContext interceptionContext)
   at System.Data.Entity.Internal.InterceptableDbCommand.ExecuteDbDataReader(CommandBehavior behavior)
   at System.Data.Common.DbCommand.ExecuteReader(CommandBehavior behavior)
   at System.Data.Entity.Core.EntityClient.Internal.EntityCommandDefinition.ExecuteStoreCommands(EntityCommand entityCommand, CommandBehavior behavior)
 --- End of inner exception stack trace ---
    at System.Data.Entity.Core.EntityClient.Internal.EntityCommandDefinition.ExecuteStoreCommands(EntityCommand entityCommand, CommandBehavior behavior)
   at System.Data.Entity.Core.Objects.Internal.ObjectQueryExecutionPlan.Execute[TResultType](ObjectContext context, ObjectParameterCollection parameterValues)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__6()
   at System.Data.Entity.Core.Objects.ObjectContext.ExecuteInTransaction[T](Func`1 func, IDbExecutionStrategy executionStrategy, Boolean startLocalTransaction, Boolean releaseConnectionOnSuccess)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__5()
   at System.Data.Entity.Infrastructure.DefaultExecutionStrategy.Execute[TResult](Func`1 operation)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.GetResults(Nullable`1 forMergeOption)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<System.Collections.Generic.IEnumerable<T>.GetEnumerator>b__0()
   at System.Data.Entity.Internal.LazyEnumerator`1.MoveNext()
   at Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI.PCORIModelAdapter.Execute(QueryComposerRequestDTO request, Boolean viewSQL) in c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer\Adapters\PCORI\PCORIModelAdapter.cs:line 231
   at Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.RunRequest(String requestJsonFilepath, String connectionString, SQLProvider sqlProvider) in c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer.Tests\QueryCompositionTests.cs:line 886
   at Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.CombinedDiagnosisCodes() in c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer.Tests\QueryCompositionTests.cs:line 755
*/
            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void CombinedDiagnosisCodes_And_Sex()
        {
            string filename = "CombinedDiagnosisCodes_And_Sex.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            //Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void RaceOnly_MultiSelect()
        {
            string filename = "RaceOnly_MultiSelect.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            //Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            /*mysql fails:Test Name:	
             RaceOnly_MultiSelect
Test FullName:	Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.RaceOnly_MultiSelect
Test Source:	c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer.Tests\QueryCompositionTests.cs : line 830
Test Outcome:	Failed
Test Duration:	0:00:03.8677122

Result Message:	
Test method Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.RaceOnly_MultiSelect threw exception: 
System.Data.Entity.Core.EntityCommandExecutionException: An error occurred while executing the command definition. See the inner exception for details. ---> MySql.Data.MySqlClient.MySqlException: Can't group on 'A1'
Result StackTrace:	
at MySql.Data.MySqlClient.MySqlStream.ReadPacket()
   at MySql.Data.MySqlClient.NativeDriver.GetResult(Int32& affectedRow, Int64& insertedId)
   at MySql.Data.MySqlClient.Driver.GetResult(Int32 statementId, Int32& affectedRows, Int64& insertedId)
   at MySql.Data.MySqlClient.Driver.NextResult(Int32 statementId, Boolean force)
   at MySql.Data.MySqlClient.MySqlDataReader.NextResult()
   at MySql.Data.MySqlClient.MySqlCommand.ExecuteReader(CommandBehavior behavior)
   at MySql.Data.Entity.EFMySqlCommand.ExecuteDbDataReader(CommandBehavior behavior)
   at System.Data.Common.DbCommand.ExecuteReader(CommandBehavior behavior)
   at System.Data.Entity.Infrastructure.Interception.DbCommandDispatcher.<Reader>b__c(DbCommand t, DbCommandInterceptionContext`1 c)
   at System.Data.Entity.Infrastructure.Interception.InternalDispatcher`1.Dispatch[TTarget,TInterceptionContext,TResult](TTarget target, Func`3 operation, TInterceptionContext interceptionContext, Action`3 executing, Action`3 executed)
   at System.Data.Entity.Infrastructure.Interception.DbCommandDispatcher.Reader(DbCommand command, DbCommandInterceptionContext interceptionContext)
   at System.Data.Entity.Internal.InterceptableDbCommand.ExecuteDbDataReader(CommandBehavior behavior)
   at System.Data.Common.DbCommand.ExecuteReader(CommandBehavior behavior)
   at System.Data.Entity.Core.EntityClient.Internal.EntityCommandDefinition.ExecuteStoreCommands(EntityCommand entityCommand, CommandBehavior behavior)
 --- End of inner exception stack trace ---
    at System.Data.Entity.Core.EntityClient.Internal.EntityCommandDefinition.ExecuteStoreCommands(EntityCommand entityCommand, CommandBehavior behavior)
   at System.Data.Entity.Core.Objects.Internal.ObjectQueryExecutionPlan.Execute[TResultType](ObjectContext context, ObjectParameterCollection parameterValues)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__6()
   at System.Data.Entity.Core.Objects.ObjectContext.ExecuteInTransaction[T](Func`1 func, IDbExecutionStrategy executionStrategy, Boolean startLocalTransaction, Boolean releaseConnectionOnSuccess)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__5()
   at System.Data.Entity.Infrastructure.DefaultExecutionStrategy.Execute[TResult](Func`1 operation)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.GetResults(Nullable`1 forMergeOption)
   at System.Data.Entity.Core.Objects.ObjectQuery`1.<System.Collections.Generic.IEnumerable<T>.GetEnumerator>b__0()
   at System.Data.Entity.Internal.LazyEnumerator`1.MoveNext()
   at Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI.PCORIModelAdapter.Execute(QueryComposerRequestDTO request, Boolean viewSQL) in c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer\Adapters\PCORI\PCORIModelAdapter.cs:line 231
   at Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.RunRequest(String requestJsonFilepath, String connectionString, SQLProvider sqlProvider) in c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer.Tests\QueryCompositionTests.cs:line 927
   at Lpp.Dns.DataMart.Model.QueryComposer.Tests.QueryCompositionTests.RaceOnly_MultiSelect() in c:\LPP\PMN\Lpp.Adapters\Lpp.Dns.DataMart.Model.QueryComposer.Tests\QueryCompositionTests.cs:line 850

*/
            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void AgeRange_AgeAsOfObservationPeriodStart()
        {
            string filename = "Age Range - age as of observation start date.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            //Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void AgeRange_AgeAtFirstEncounterWithinCriteria()
        {
            string filename = "Age Range - age at first encounter within criteria.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            //Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void AgeRange_AgeAtLastEncounterSystemWide()
        {
            //using (var db = Helper.CreatePCORIDataContext(Settings.SQLProvider.SQLServer, MSSqlConnectionString, true))
            //{
            //    // Females between 5 and 25 at last encounter system wide
            //    var query = from p in db.Patients
            //                where p.Sex == "F"
            //                && p.Encounters.Where(ec => ec == p.Encounters.OrderByDescending(x => x.AdmittedOn).FirstOrDefault() &&
            //                5 <= (System.Data.Entity.DbFunctions.DiffYears(p.BornOn, ec.AdmittedOn).Value - (((p.BornOn.Value.Month > ec.AdmittedOn.Month) || (ec.Patient.BornOn.Value.Month == ec.AdmittedOn.Month && p.BornOn.Value.Day > ec.AdmittedOn.Day)) ? 1 : 0))
            //                && (System.Data.Entity.DbFunctions.DiffYears(p.BornOn, ec.AdmittedOn).Value - (((p.BornOn.Value.Month > ec.AdmittedOn.Month) || (ec.Patient.BornOn.Value.Month == ec.AdmittedOn.Month && p.BornOn.Value.Day > ec.AdmittedOn.Day)) ? 1 : 0)) <= 25
            //                ).Any()
            //                select new
            //                {
            //                    PatientID = p.ID,
            //                    Sex = p.Sex,
            //                    Age = p.Encounters.Where(ec => ec == p.Encounters.OrderByDescending(x => x.AdmittedOn).FirstOrDefault()).Select(ec => (System.Data.Entity.DbFunctions.DiffYears(p.BornOn, ec.AdmittedOn).Value - (((p.BornOn.Value.Month > ec.AdmittedOn.Month) || (ec.Patient.BornOn.Value.Month == ec.AdmittedOn.Month && p.BornOn.Value.Day > ec.AdmittedOn.Day)) ? 1 : 0))).FirstOrDefault()
            //                };

            //    var result = query.GroupBy(k => new { k.Sex, k.Age }).Select(k => new { k.Key.Sex, k.Key.Age, Count = k.Count() });
            //    foreach (var r in result)
            //    {
            //        Console.WriteLine("{0}-{1}: {2}", r.Sex, r.Age, r.Count);
            //    }

            //}

            string filename = "Age Range - age at last encounter system wide.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void PMNDEV4551()
        {
            string filename = "PMNDEV-4551.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());
        }

        [TestMethod]
        public void PMNDEV4557_PostgreSQL(){
            var response = RunRequest("PMNDEV-4557.json", PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsNull(response.Errors);
            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());
        }

        [TestMethod]
        public void PMNDEV4769()
        {
            var response = RunRequest("PMNDEV-4769.json", MSSqlConnectionString, Settings.SQLProvider.SQLServer);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsNull(response.Errors);
            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());
        }

        [TestMethod]
        public void Males_with_observationperiod()
        {
            string filename = "Males_with_observationperiod.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void PMNDEV5055_CombindedDiagnosisCount()
        {
            string filename = "PMNDEV-5055.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void PMNDEV5136_DiagnosisCodes()
        {
            //ICD9 exact match for 250.00
            Logger.Debug("ICD9 exact match for 250.00");
            int linqCount = 0;
            using (var db = Helper.CreatePCORIDataContext(MSSqlConnectionString, true))
            {
                linqCount = db.Diagnoses.Where(dx => dx.CodeType == "09" && dx.Code == "250.00").Select(dx => dx.PatientID).Distinct().Count();
            }
            Logger.Debug("Patient Count: " + linqCount);

            var response = RunRequest("PMNDEV-5136-1.json");
            Logger.Debug(SerializeJsonToString(response));

            var table = response.Results.First();
            var row = table.First();
            int resultCount;
            if (!int.TryParse(row["Patients"].ToString(), out resultCount))
            {
                Assert.Fail("Could not determine patient count from result.");
            }
            Assert.AreEqual(linqCount, resultCount, "Patient counts do not match.");

            Logger.Debug("ICD10 exact match for 272.0");
            //ICD10 exact match for 272.0
            using (var db = Helper.CreatePCORIDataContext(MSSqlConnectionString, true))
            {
                linqCount = db.Diagnoses.Where(dx => dx.CodeType == "10" && dx.Code == "272.0").Select(dx => dx.PatientID).Distinct().Count();
            }
            Logger.Debug("Patient Count: " + linqCount);

            response = RunRequest("PMNDEV-5136-2.json");
            Logger.Debug(SerializeJsonToString(response));

            table = response.Results.First();
            row = table.First();            
            if (!int.TryParse(row["Patients"].ToString(), out resultCount))
            {
                Assert.Fail("Could not determine patient count from result.");
            }
            Assert.AreEqual(linqCount, resultCount, "Patient counts do not match.");


            Logger.Debug("ICD9 250.00 OR ICD10 272.0");
            using (var db = Helper.CreatePCORIDataContext(MSSqlConnectionString, true))
            {
                linqCount = db.Diagnoses.Where(dx => (dx.CodeType == "09" && dx.Code == "250.00") || (dx.CodeType == "10" && dx.Code == "272.0")).Select(dx => dx.PatientID).Distinct().Count();
            }
            Logger.Debug("Patient Count: " + linqCount);

            //ICD9 250.00 OR ICD10 272.0
            response = RunRequest("PMNDEV-5136-3.json");
            Logger.Debug(SerializeJsonToString(response));

            table = response.Results.First();
            row = table.First();
            if (!int.TryParse(row["Patients"].ToString(), out resultCount))
            {
                Assert.Fail("Could not determine patient count from result.");
            }
            Assert.AreEqual(linqCount, resultCount, "Patient counts do not match.");

            ////ICD10 272.0 OR ICD9 250.00
            response = RunRequest("PMNDEV-5136-4.json");
            Logger.Debug(SerializeJsonToString(response));

            table = response.Results.First();
            row = table.First();
            if (!int.TryParse(row["Patients"].ToString(), out resultCount))
            {
                Assert.Fail("Could not determine patient count from result.");
            }
            Assert.AreEqual(linqCount, resultCount, "Patient counts do not match.");




        }

        [TestMethod]
        public void PMNDEV5068()
        {
            string filename = "PMNDEV-5068.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            //Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            Assert.IsNull(response.Errors);
            //Assert.IsTrue(response.Results.Any());

            //var table = response.Results.First();

            //if (RunOracle)
            //{
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                //Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            //}

            //if (RunPostgreSQL)
            //{
            //    var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
            //    Assert.IsNull(npgsqlResponse.Errors);
            //    Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            //}

            //if (RunMySql)
            //{
            //    var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
            //    Assert.IsNull(mysqlResponse.Errors);
            //    Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            //}


            List<RaceAgeRangeResult> sqlServer = new List<RaceAgeRangeResult>();
            IEnumerable<Dictionary<string, object>> table = response.Results.First();
            foreach (var row in table)
            {
                sqlServer.Add(new RaceAgeRangeResult { Race = (row["Race"] ?? "").ToString(), AgeRange = (row["Age"] ?? "").ToString(), Count = Convert.ToInt32((row["Patients"] ?? "0").ToString()) });
            }

            List<RaceAgeRangeResult> oracle = new List<RaceAgeRangeResult>();
            table = oracleResponse.Results.First();
            foreach (var row in table)
            {
                oracle.Add(new RaceAgeRangeResult { Race = (row["Race"] ?? "").ToString(), AgeRange = (row["Age"] ?? "").ToString(), Count = Convert.ToInt32((row["Patients"] ?? "0").ToString()) });
            }

            List<RaceAgeRangeResult> noMatch = new List<RaceAgeRangeResult>();
            foreach (var item in sqlServer)
            {
                var oitem = oracle.Where(o => o.AgeRange == item.AgeRange && o.Race == item.Race && o.Count == item.Count).FirstOrDefault();
                if (oitem == null)
                {
                    noMatch.Add(item);
                }
            }

            if (noMatch.Count > 0)
            {
                Console.WriteLine("Found non-matching results!!");
                Console.WriteLine("");
                foreach (var item in noMatch)
                {
                    Console.WriteLine("{0}\t{1}\t{2}", item.Race, item.AgeRange, item.Count);
                }
            }

            Assert.IsTrue(noMatch.Count == 0, "Found non-matching results!!");


        }

        public class RaceAgeRangeResult
        {
            public string Race { get; set; }

            public string AgeRange { get; set; }

            public int Count { get; set; }
        }

        [TestMethod]
        public void PMNDEV5128_AgeRangeOracleDifferences()
        {
            string filename = "PMNDEV-5128-1.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
            Logger.Debug(SerializeJsonToString(oracleResponse));

            var sqlserverResults = response.Results.SelectMany(t => t.Select(r => new Tuple<string, int>(r["Age"].ToString(), Convert.ToInt32(r["Patients"])))).OrderBy(k => Convert.ToInt32(k.Item1));
            var oracleResults = oracleResponse.Results.SelectMany(t => t.Select(r => new Tuple<string, int>(r["Age"].ToString(), Convert.ToInt32(r["Patients"]))));


            foreach (var sr in sqlserverResults)
            {
                var or = oracleResults.FirstOrDefault(o => o.Item1 == sr.Item1);
                if(or == null){
                    Logger.Debug(string.Format("{0}\t\t{1}\tNOT FOUND", sr.Item1, sr.Item2));
                }else{
                    Logger.Debug(string.Format("{0}\t\t{1}\t{2}{3}", sr.Item1, sr.Item2, or.Item2, (sr.Item2 != or.Item2 ? "\t**" : "")));
                }               
            }


        }

        [TestMethod]
        public void PMNDEV5130_HeightValues()
        {
            string filename = "PMNDEV-5130-1.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            var response2 = RunRequest("PMNDEV-5130-2.json");
            Logger.Debug(SerializeJsonToString(response2));

        }

        [TestMethod]
        public void PMNDEV5133_WeightValues()
        {
            string filename = "PMNDEV-5133-1.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            var response2 = RunRequest("PMNDEV-5133-2.json");
            Logger.Debug(SerializeJsonToString(response2));
        }

        [TestMethod]
        public void PMNDEV5144_AgeAsOfLastEncounterInSystem()
        {
            var request = LoadRequest("PMNDEV-5144.json");
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(MSSqlConnectionString, Settings.SQLProvider.SQLServer))
            {
                var response = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(response));
            }

            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL))
            {
                var response = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(response));
            }

            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(OracleConnectionString, Settings.SQLProvider.Oracle))
            {
                var response = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(response));
            }

            //var response = RunRequest("PMNDEV-5144.json", PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
            //Logger.Debug(SerializeJsonToString(response));

            
            //using (var adapter = Helper.CreatePCORIModelAdapterAdapter(PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL))
            //{
            //    var response =  adapter.Execute(request, false);
            //}
        }

        [TestMethod]
        public void PMNDEV5158_AgeAsOfObservationPeriodEndDate()
        {

            var request = LoadRequest("PMNDEV-5158.json");
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(MSSqlConnectionString, Settings.SQLProvider.SQLServer))
            {
                var response = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(response));
            }

            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL))
            {
                var response = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(response));
            }

            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(OracleConnectionString, Settings.SQLProvider.Oracle))
            {
                var response = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(response));
            }


        }

        [TestMethod]
        public void PMNDEV5207_AgeAsOfObservationPeriodStartDate_NoAgeRangeSpecified()
        {
            //var request = LoadRequest("PMNDEV-5207-EmptyAgeRange.json");            

            //Empty age, no stratifications. Should return patient count
            //var request = LoadRequest("PMNDEV-5207-2-EmptyAgeRage.json");

            var request = LoadRequest("PMNDEV-5207-3-EmptyAgeRage.json");

            //using (var adapter = Helper.CreatePCORIModelAdapterAdapter(MSSqlConnectionString, Settings.SQLProvider.SQLServer))
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL))
            {
                var response = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(response));
            }

        }

        [TestMethod]
        public void PMNDEV5248()
        {
            var request = LoadRequest("PMNDEV-5248- age at last encounter system wide no stratifications.json");
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(MSSqlConnectionString, Settings.SQLProvider.SQLServer))
            {
                var response = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(response));
            }
        }

        [TestMethod]
        public void PMNDEV5250()
        {
            var request = LoadRequest("PMNDEV-5250 - Race.json");
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(MSSqlConnectionString, Settings.SQLProvider.SQLServer))
            {
                var response = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(response));
            }
        }

        [TestMethod]
        public void AgeRangeCalculationDifferences()
        {
            DateTime asOf = DateTime.Now;            
            asOf = new DateTime(2015, 11, 1);
            
            List<Tuple<DateTime, int>> sqlResults = null;
            using (var db = Helper.CreatePCORIDataContext(MSSqlConnectionString, true))
            {
                var query = from p in db.Patients
                            where p.BornOn.HasValue
                            orderby p.BornOn
                            select new
                            {
                                BornOn = p.BornOn.Value,
                                Age = System.Data.Entity.DbFunctions.DiffYears(p.BornOn, asOf).Value - (((p.BornOn.Value.Month > asOf.Month) || (p.BornOn.Value.Month == asOf.Month && p.BornOn.Value.Day > asOf.Day)) ? 1 : 0)
                            };

                sqlResults = query.ToArray().GroupBy(p => p.BornOn).Select(p => new Tuple<DateTime, int>(p.Key, p.First().Age)).OrderByDescending(p => p.Item1.Month).ThenByDescending(p => p.Item1.Day).ThenByDescending(p => p.Item1.Year).ToList();
            }


            List<Tuple<DateTime, int>> oracleResults = null;
            using (var db = Helper.CreatePCORIDataContext(Settings.SQLProvider.Oracle, OracleConnectionString, true))
            {
                DateTime upTo = asOf.AddMonths(1);
                upTo = asOf.AddDays(30);

                var query = from p in db.Patients
                            where p.BornOn.HasValue
                            orderby p.BornOn
                            select new
                            {
                                BornOn = p.BornOn.Value,
                                //This is always different by 1 year more
                                //Age = System.Data.Entity.e
                                //This is off if the birth date is same month but day is after current date
                                //Age = System.Data.Entity.DbFunctions.DiffYears(System.Data.Entity.DbFunctions.TruncateTime(p.BornOn.Value), asOf).Value,

                                //WE HAVE A WINNER!!
                                Age = asOf.Year - p.BornOn.Value.Year - (p.BornOn.Value.Month >= asOf.Month && (p.BornOn.Value.Month > asOf.Month || (p.BornOn.Value.Month == asOf.Month && p.BornOn.Value.Day > asOf.Day)) ? 1 : 0)
                                
                                //always different by 1 year less
                                //Age = System.Data.Entity.DbFunctions.DiffMonths(p.BornOn, asOf).Value / 12,
                                //Age = System.Data.Entity.DbFunctions.DiffYears(p.BornOn, asOf).Value - (p.BornOn.Value.Month == asOf.Month && p.BornOn.Value.Day > asOf.Day ? 1 : 0)
                                //Age = System.Data.Entity.DbFunctions.DiffYears(p.BornOn, asOf).Value - ((p.BornOn.Value.Month == asOf.Month || p.BornOn.Value.Month == upTo.Month) && ((p.BornOn.Value.Month == asOf.Month && p.BornOn.Value.Day > asOf.Day) || (p.BornOn.Value.Month == upTo.Month && p.BornOn.Value.Day >= upTo.Day)) ? 1: 0)
                            };

                oracleResults = query.ToArray().GroupBy(p => p.BornOn).Select(p => new Tuple<DateTime, int>(p.Key, p.First().Age)).ToList();
            }

            StringBuilder noMatches = new StringBuilder();
            foreach (var r in sqlResults)
            {
                var ora = oracleResults.FirstOrDefault(o => o.Item1 == r.Item1);
                if (ora == null)
                {
                    Logger.Debug(r.Item1 + "\t\t" + r.Item2 + "\t\tMISSING ORACLE");
                }
                else if (r.Item2 != ora.Item2)
                {
                    Logger.Debug(r.Item1 + "\t\t" + r.Item2 + "\t\t" + ora.Item2);
                    noMatches.AppendLine(r.Item1 + "\t\t" + r.Item2 + "\t\t" + ora.Item2);
                }
                else
                {
                    Logger.Debug(r.Item1 + "\t\t" + r.Item2 + "\t\tMATCHES");
                }
            }
        }


        [TestMethod]
        public void ProcessorTest_RaceOnlyMultiSelect()
        {
            var processor = Helper.CreateQueryComposerModelProcessorForPCORI(MSSqlConnectionString);

            string filepath = System.IO.Path.Combine(ResourceFolder, "RaceOnly_MultiSelect.json");
            System.IO.FileInfo fi = new System.IO.FileInfo(filepath);            

            IDictionary<string, string> properties;
            Document[] documents;

            processor.Request("1",
                new NetworkConnectionMetadata
                {
                    OrganizationName = "Test Org",
                    UserEmail = "test@test.com",
                    UserFullName = "Administrator",
                    UserLogin = "administrator"
                },
                new RequestMetadata
                {
                    DataMartId = "dm1",
                    DataMartName = "Test DataMart",
                    DataMartOrganizationId = "1",
                    DataMartOrganizationName = "Test Org",
                    IsMetadataRequest = false,
                    RequestTypeId = ""
                },
                new Document[] {
                    new Document("1", "application/json", "request.json"){ IsViewable = false, Size = Convert.ToInt32(fi.Length) }
                },
                out  properties,
                out documents);

            processor.RequestDocument("1", "1", fi.OpenRead());

            processor.Start("1");

            Document[] responseDocuments = processor.Response("1");

            System.IO.Stream responseStream = null;
            try
            {
                processor.ResponseDocument("1", responseDocuments[0].DocumentID, out responseStream, int.MaxValue);

                string json;
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    json = reader.ReadToEnd();
                }
                System.IO.File.WriteAllText("response.json", json, System.Text.Encoding.UTF8);

                Console.WriteLine(json);
            }
            finally
            {
                if (responseStream != null)
                    responseStream.Dispose();
            }
        }

        [TestMethod]
        public void AgeRange_As_of_submission_no_other_terms()
        {
            string filename = "Age Range - age as of submission no other terms.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();
            var sqlserverResults = response.Results.SelectMany(t => t.Select(r => new Tuple<string, int>(r["Age"].ToString(), Convert.ToInt32(r["Patients"])))).OrderBy(k => Convert.ToInt32(k.Item1));

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());

               
                var oracleResults = oracleResponse.Results.SelectMany(t => t.Select(r => new Tuple<string, int>(r["Age"].ToString(), Convert.ToInt32(r["Patients"]))));

                foreach (var sr in sqlserverResults)
                {
                    var or = oracleResults.FirstOrDefault(o => o.Item1 == sr.Item1);
                    if (or == null)
                    {
                        Logger.Debug(string.Format("{0}\t\t{1}\tNOT FOUND", sr.Item1, sr.Item2));
                    }
                    else
                    {
                        Logger.Debug(string.Format("{0}\t\t{1}\t{2}{3}", sr.Item1, sr.Item2, or.Item2, (sr.Item2 != or.Item2 ? "\t**" : "")));
                    }
                }
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void Patients_Count_By_Observation_Period()
        {
            var request = LoadRequest("Patients_Count_By_Observation_Period.json");
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(MSSqlConnectionString, Settings.SQLProvider.SQLServer))
            {
                var response = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(response));
            }

            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL))
            {
                var response = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(response));
            }

            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(OracleConnectionString, Settings.SQLProvider.Oracle))
            {
                var response = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(response));
            }
        }

        [TestMethod]
        public void PMNDEV5283_VitalMeasureObservationPeriod()
        {
            var request = LoadRequest("PMNDEV-5283-1.json");
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(MSSqlConnectionString, Settings.SQLProvider.SQLServer))
            {
                var response = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(response));
            }

            request = LoadRequest("PMNDEV-5283-2.json");
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(MSSqlConnectionString, Settings.SQLProvider.SQLServer))
            {
                var response = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(response));
            }

            request = LoadRequest("PMNDEV-5283-4.json");
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(MSSqlConnectionString, Settings.SQLProvider.SQLServer))
            {
                var response = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(response));
            }
        }

        [TestMethod]
        public void PMNDEV5319_ExclusionNotWorking()
        {
            //no exclusion
            //var request = LoadRequest("PMNDEV-5319-1.json");
            //using (var adapter = Helper.CreatePCORIModelAdapterAdapter(MSSqlConnectionString, Settings.SQLProvider.SQLServer))
            //{
            //    var response = adapter.Execute(request, false);
            //    Logger.Debug(SerializeJsonToString(response));
            //}

            //with exclusion
            var request = LoadRequest("PMNDEV-5319-2.json");
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(MSSqlConnectionString, Settings.SQLProvider.SQLServer))
            {
                var response = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(response));
            }
        }

        [TestMethod]
        public void PMNDEV5322_EmptyHeightAndWeightWithVitalsMeasureDate()
        {
            var request = LoadRequest("PMNDEV-5322.json");
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(MSSqlConnectionString, Settings.SQLProvider.SQLServer))
            {
                var response = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(response));
            }

            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL))
            {
                var response = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(response));
            }

            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(OracleConnectionString, Settings.SQLProvider.Oracle))
            {
                var response = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(response));
            }
        }

        [TestMethod]
        public void PMNDEV5326_StratificationIncludesNull()
        {
            var request = LoadRequest("PMNDEV-5326.json");
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(MSSqlConnectionString, Settings.SQLProvider.SQLServer))
            {
                var response = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(response));
            }
        }

        [TestMethod]
        public void PMNDEV5329_10yrGroupingFormatting()
        {
            var request = LoadRequest("PMNDEV-5329 - 10yrAgeGrouping.json");
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(MSSqlConnectionString, Settings.SQLProvider.SQLServer))
            {
                var response = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(response));


                //var results = new List<Tuple<int, int>>();
                //var table = response.Results.First();
                //foreach (var row in table)
                //{
                //    results.Add(new Tuple<int, int>(int.Parse(row["Age"].ToString()), int.Parse(row["Patients"].ToString())));
                //}

                //foreach (var r in results.OrderBy(x => x.Item1))
                //{
                //    Logger.Debug(string.Format("{0}, {1}", r.Item1, r.Item2));
                //}
            }
        }

        [TestMethod]
        public void MathFloor()
        {
            Console.WriteLine(Math.Floor(-1m / 5m));
            Console.WriteLine(Math.Floor(-2m / 5m));
            Console.WriteLine(Math.Ceiling(-1m / 5m));
        }

        [TestMethod]
        public void PMNDEV5330()
        {
            var request = LoadRequest("PMNDEV-5330-5yr Age Groupings.json");
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(MSSqlConnectionString, Settings.SQLProvider.SQLServer))
            {
                var response = adapter.Execute(request, false);
                //Logger.Debug(SerializeJsonToString(response));


                var results = new List<Tuple<int, int>>();
                var table = response.Results.First();
                foreach (var row in table)
                {
                    results.Add(new Tuple<int, int>(int.Parse(row["Age"].ToString()), int.Parse(row["Patients"].ToString())));
                }

                foreach (var r in results.OrderBy(x => x.Item1))
                {
                    Logger.Debug(string.Format("{0}, {1}", r.Item1, r.Item2));
                }
            }


            //using (var adapter = Helper.CreatePCORIModelAdapterAdapter(PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL))
            //{
            //    var response = adapter.Execute(request, false);
            //    Logger.Debug(SerializeJsonToString(response));
            //}

            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(OracleConnectionString, Settings.SQLProvider.Oracle))
            {
                var response = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(response));

                var results = new List<Tuple<int, int>>();
                var table = response.Results.First();
                foreach (var row in table)
                {
                    results.Add(new Tuple<int, int>(int.Parse(row["Age"].ToString()), int.Parse(row["Patients"].ToString())));
                }

                foreach (var r in results.OrderBy(x => x.Item1))
                {
                    Logger.Debug(string.Format("{0}, {1}", r.Item1, r.Item2));
                }
            }



            //using (var db = Helper.CreatePCORIDataContext(Settings.SQLProvider.SQLServer, MSSqlConnectionString, true))
            //{
            //    //int maxAge = 1;
            //    DateTime encounterStart = DateTime.Parse("7/3/2003").Date;
            //    DateTime encounterEnd = DateTime.Parse("10/9/2006").Date;
            //    var query = db.Patients.Where(p => p.Encounters.Any(enc => enc.AdmittedOn >= encounterStart && enc.AdmittedOn <= encounterEnd)
            //        );

            //    var q2 = query.Select(p => new
            //    {
            //        PatientID = p.ID,
            //        BornOn = p.BornOn,
            //        //Age = (System.Data.Entity.DbFunctions.DiffYears(p.BornOn.Value, DateTime.Now).Value - (((p.BornOn.Value.Month > DateTime.Now.Month) || (p.BornOn.Value.Month == DateTime.Now.Month && p.BornOn.Value.Day > DateTime.Now.Day)) ? 1 : 0))
            //        Age = (int?)((p.BornOn.Value > encounterStart) ?
            //                (DbFunctions.DiffYears(p.BornOn.Value, encounterStart).Value + ((p.BornOn.Value.Month < encounterStart.Month || (p.BornOn.Value.Month == encounterStart.Month && p.BornOn.Value.Day < encounterStart.Day)) ? 1 : 0))
            //                :
            //                (DbFunctions.DiffYears(p.BornOn.Value, encounterStart).Value - (((p.BornOn.Value.Month > encounterStart.Month) || (p.BornOn.Value.Month == encounterStart.Month && p.BornOn.Value.Day > encounterStart.Day)) ? 1 : 0))
            //            )
            //    })
            //    .OrderByDescending(p => p.BornOn);
            //    //.GroupBy(p => new { Age = p.Age })
            //    //.Select(p => new { Age = p.Key.Age, Count = p.Count() });

            //    foreach (var p in q2)
            //    {
            //        Logger.Debug(string.Format("{0:D}\t\t{1}", p.BornOn, p.Age));
            //    }



            //}
        }

        [TestMethod]
        public void PMNDEV5395_WeightStratificationByMeasureDate(){
            var request = LoadRequest("PMNDEV-5395-WeightStratificationByMeasureDate.json");
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(MSSqlConnectionString, Settings.SQLProvider.SQLServer))
            {
                var response = adapter.Execute(request, false);
                //Logger.Debug(SerializeJsonToString(response));


                var results = new List<Tuple<string, int>>();
                var table = response.Results.First();
                foreach (var row in table)
                {
                    //results.Add(new Tuple<double, int>(double.Parse((row["Weight"] ?? "-99").ToString()), int.Parse(row["Patients"].ToString())));
                    results.Add(new Tuple<string, int>((row["Weight"] ?? "<<null>>").ToString(), int.Parse(row["Patients"].ToString())));
                }

                Logger.Debug(string.Format("Total Patients: {0}", results.Sum(r => r.Item2)));

                foreach (var r in results.OrderBy(x => x.Item1))
                {
                    //Logger.Debug(string.Format("{0}, {1}", (r.Item1 * 10).ToString() + " - " + (r.Item1 * 10 + 10), r.Item2));
                    Logger.Debug(string.Format("{0}, {1}", r.Item1, r.Item2));
                }
            }

            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL))
            {
                var response = adapter.Execute(request, false);
            }


            //using (var db = Helper.CreatePCORIDataContext(Settings.SQLProvider.SQLServer, MSSqlConnectionString))
            //{
            //    DateTime measureOnStart = DateTime.Parse("1990-02-01T00:00:00.000Z");
            //    DateTime measureOnEnd = DateTime.Parse("2010-02-02T00:00:00.000Z");
            //    var results = from p in db.Patients
            //                  where p.Vitals.Any(v => v.MeasuredOn >= measureOnStart && v.MeasuredOn <= measureOnEnd)
            //                  select new
            //                  {
            //                      Weight = p.Vitals.Where(v => v.MeasuredOn >= measureOnStart && v.MeasuredOn <= measureOnEnd).OrderBy(v => v.MeasuredOn).Select(v => v.Weight).FirstOrDefault()
            //                  };

            //    var grouped = results.GroupBy(k => k.Weight).OrderBy(k => k.Key).Select(k => new { Weight = k.Key, Count = k.Count() });

            //    Logger.Debug(string.Format("Total Patients: {0}", grouped.Sum(k => k.Count)));

            //    foreach (var v in grouped)
            //    {
            //        Logger.Debug(string.Format("{0}\t\t\t{1}", v.Weight.HasValue ? v.Weight.ToString() : "<<null>>", v.Count));
            //    }
            //}
        }

        [TestMethod]
        public void PMNDEV5395_WeightStratificationNoCriteria()
        {
            var request = LoadRequest("PMNDEV-5395-WeightStratificationNoCriteria.json");
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(MSSqlConnectionString, Settings.SQLProvider.SQLServer))
            {
                var response = adapter.Execute(request, false);
                //Logger.Debug(SerializeJsonToString(response));


                var results = new List<Tuple<double, int>>();
                var table = response.Results.First();
                foreach (var row in table)
                {
                    results.Add(new Tuple<double, int>(double.Parse((row["Weight"] ?? "-99").ToString()), int.Parse(row["Patients"].ToString())));
                    //results.Add(new Tuple<string, int>((row["Weight"] ?? "-99").ToString(), int.Parse(row["Patients"].ToString())));
                }

                Logger.Debug(string.Format("Total Patients: {0}", results.Sum(r => r.Item2)));

                foreach (var r in results.OrderBy(x => x.Item1))
                {
                    Logger.Debug(string.Format("{0}, {1}", (r.Item1 * 10).ToString() + " - " + (r.Item1 * 10 + 10), r.Item2));
                }
            }

            using (var db = Helper.CreatePCORIDataContext(Settings.SQLProvider.SQLServer, MSSqlConnectionString))
            {
                var results = from p in db.Patients
                              select new
                              {
                                  Weight = p.Vitals.OrderBy(v => v.MeasuredOn).Select(v => v.Weight).FirstOrDefault()
                              };

                var grouped = results.GroupBy(k => k.Weight).OrderBy(k => k.Key).Select(k => new { Weight = k.Key, Count = k.Count() });

                foreach (var v in grouped)
                {
                    Logger.Debug(string.Format("{0}\t\t\t{1}", v.Weight.HasValue ? v.Weight.ToString() : "<<null>>", v.Count));
                }
            }
        }

        [TestMethod]
        public void PMNDEV5396_HeightStratificationByMeasuredDate(){
            var request = LoadRequest("PMNDEV-5396-HeightStratificationByMeasuredDate.json");
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(MSSqlConnectionString, Settings.SQLProvider.SQLServer))
            {
                var response = adapter.Execute(request, false);
                //Logger.Debug(SerializeJsonToString(response));


                var results = new List<Tuple<string, int>>();
                var table = response.Results.First();
                foreach (var row in table)
                {
                    //results.Add(new Tuple<double, int>(double.Parse((row["Height"] ?? "-99").ToString()), int.Parse(row["Patients"].ToString())));
                    results.Add(new Tuple<string, int>((row["Height"] ?? "<<null>>").ToString(), int.Parse(row["Patients"].ToString())));
                }

                Logger.Debug(string.Format("Total Patients: {0}", results.Sum(r => r.Item2)));

                foreach (var r in results.OrderBy(x => x.Item1))
                {
                    //Logger.Debug(string.Format("{0}, {1}", (r.Item1 * 10).ToString() + " - " + (r.Item1 * 10 + 10), r.Item2));
                    Logger.Debug(string.Format("{0}, {1}", r.Item1, r.Item2));
                }
            }


            using (var db = Helper.CreatePCORIDataContext(Settings.SQLProvider.SQLServer, MSSqlConnectionString))
            {
                DateTime measureOnStart = DateTime.Parse("1990-02-01T00:00:00.000Z");
                DateTime measureOnEnd = DateTime.Parse("2010-02-01T00:00:00.000Z");
                var results = from p in db.Patients
                              where p.Vitals.Any(v => v.MeasuredOn >= measureOnStart && v.MeasuredOn <= measureOnEnd)
                              select new
                              {
                                  Height = p.Vitals.Where(v => v.MeasuredOn >= measureOnStart && v.MeasuredOn <= measureOnEnd).OrderBy(v => v.MeasuredOn).Select(v => v.Height).FirstOrDefault()
                              };

                var grouped = results.GroupBy(k => k.Height).OrderBy(k => k.Key).Select(k => new { Height = k.Key, Count = k.Count() });

                foreach (var v in grouped)
                {
                    Logger.Debug(string.Format("{0}\t\t\t{1}", v.Height.HasValue ? v.Height.ToString() : "<<null>>", v.Count));
                }
            }
        }

        [TestMethod]
        public void PMNMAINT_1138_HPHCI_long_running_Oracle()
        {

            var timer = new System.Timers.Timer(60 * 1000);
            timer.AutoReset = false;
            timer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
            {
                throw new Exception("Out of time!");
            };

            //var request = LoadRequest("PMNMAINT_1138_HPHCI-long running Oracle.json");
            //using (var adapter = Helper.CreatePCORIModelAdapterAdapter(OracleConnectionString, Settings.SQLProvider.Oracle))
            //{
            //    timer.Start();

            //    var response = adapter.Execute(request, false);

            //    timer.Stop();
            //    Logger.Debug(SerializeJsonToString(response));
            //}

            #region old
            //using (var adapter = Helper.CreatePCORIModelAdapterAdapter(OracleConnectionString, Settings.SQLProvider.Oracle))
            //{
            //    var response = adapter.Execute(request, true);
            //    //Logger.Debug(SerializeJsonToString(response));
            //    var table = response.Results.First();
            //    var row = table.First();
            //    Logger.Debug(row["SQL"]);
            //}

            //Current query as LINQ
            using (var db = Helper.CreatePCORIDataContext(Settings.SQLProvider.Oracle, OracleConnectionString, true))
            {
                string codeType = "09";
                var query = from p in db.Patients
                            where p.Encounters.Any() &&
                            p.Vitals.Any(v => v.Weight.HasValue && v.Weight >= 20 && v.Weight <= 300) &&
                            p.Diagnoses.Any(d => ((d.CodeType != null && d.CodeType == codeType) || (d.CodeType == null && codeType == null)) && d.Code.StartsWith("V") && d.Code != null) &&
                            p.Encounters.Any(enc => enc.Diagnoses.Any(d => ((d.CodeType != null && d.CodeType == codeType) || (d.CodeType == null && codeType == null)) && d.Code.StartsWith("250") && d.Code != null))
                            select new
                            {
                                p.Hispanic,
                                p.Race,
                                PatientID = p.ID,
                                AdmittedOn = p.Encounters.OrderBy(enc => enc.AdmittedOn).Select(enc => (DateTime?)enc.AdmittedOn).FirstOrDefault(),
                                AdmittedOnYear = p.Encounters.OrderBy(enc => enc.AdmittedOn).Select(enc => (DateTime?)enc.AdmittedOn).FirstOrDefault().Value.Year,
                                AdmittedOnMonth = p.Encounters.OrderBy(enc => enc.AdmittedOn).Select(enc => (DateTime?)enc.AdmittedOn).FirstOrDefault().Value.Month
                            };

                var g = query.GroupBy(k => new { Hispanic = k.Hispanic, Race = k.Race, AdmittedOnYear = k.AdmittedOnYear })
                             .Select(k => new { Hispanic = k.Key.Hispanic, Race = k.Key.Race, AdmittedOnYear = k.Key.AdmittedOnYear, Patients = k.Count() });

                Logger.Debug(g.ToString());
            }

            #endregion


            //using (var db = Helper.CreatePCORIDataContext(Settings.SQLProvider.Oracle, OracleConnectionString, true))
            ////using (var db = Helper.CreatePCORIDataContext(Settings.SQLProvider.SQLServer, MSSqlConnectionString, true))
            //{
            //    var query = from p in db.Patients
            //                where p.Encounters.Any() &&
            //                p.Vitals.Any(v => v.Weight.HasValue && v.Weight >= 20 && v.Weight <= 300) &&
            //                p.Diagnoses.Any(d => ((d.CodeType != null && d.CodeType == "09") || (d.CodeType == null && "09" == null)) && d.Code.StartsWith("V") && d.Code != null) &&
            //                !p.Encounters.Any(enc => enc.Diagnoses.Any(d => ((d.CodeType != null && d.CodeType == "09") || (d.CodeType == null && "09" == null)) && d.Code.StartsWith("250") && d.Code != null))
            //                select new
            //                {
            //                    p.Hispanic,
            //                    p.Race,                                
            //                    AdmittedOn = p.Encounters.OrderBy(enc => enc.AdmittedOn).Select(enc => (DateTime?)enc.AdmittedOn).FirstOrDefault(),
            //                    AdmittedOnYear = p.Encounters.OrderBy(enc => enc.AdmittedOn).Select(enc => (DateTime?)enc.AdmittedOn).FirstOrDefault().Value.Year,
            //                    AdmittedOnMonth = p.Encounters.OrderBy(enc => enc.AdmittedOn).Select(enc => (DateTime?)enc.AdmittedOn).FirstOrDefault().Value.Month,
            //                    PatientID = p.ID
            //                };

            //    var g = query.GroupBy(k => new { Hispanic = k.Hispanic, Race = k.Race, AdmittedOnYear = k.AdmittedOnYear })
            //                 .Select(k => new { Hispanic = k.Key.Hispanic, Race = k.Key.Race, AdmittedOnYear = k.Key.AdmittedOnYear, Patients = k.Count() });

            //    Logger.Debug(g.Expression.ToString());
            //    Logger.Debug(g.ToString());

            //    //foreach (var k in g)
            //    //{
            //    //    Logger.Debug(string.Format("Race: {0} Patients: {1}", k.Race, k.Patients));
            //    //}
            //}

        }

        [TestMethod]
        public void PMNMAINT_1138_HPHCI_Acceptance1()
        {
            var timer = new System.Timers.Timer(60 * 1000);
            timer.AutoReset = false;
            timer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
            {
                throw new Exception("Out of time!");
            };

            var request = LoadRequest("PMNMAINT_1138_HPHCI-Acceptance1.json");

            if (RunOracle)
            {
                using (var adapter = Helper.CreatePCORIModelAdapterAdapter(OracleConnectionString, Settings.SQLProvider.Oracle))
                {
                    timer.Start();

                    var oracleResponse = adapter.Execute(request, false);

                    timer.Stop();
                    Logger.Debug(SerializeJsonToString(oracleResponse));
                }
            }

            if (RunPostgreSQL)
            {
                using (var adapter = Helper.CreatePCORIModelAdapterAdapter(PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL))
                {
                    timer.Start();

                    var postgresResponse = adapter.Execute(request, false);

                    timer.Stop();
                    Logger.Debug(SerializeJsonToString(postgresResponse));
                }
            }
        }

        [TestMethod]
        public void PMNMAINT_1138_HPHCI_Acceptance2()
        {
            var timer = new System.Timers.Timer(60 * 1000);
            timer.AutoReset = false;
            timer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
            {
                throw new Exception("Out of time!");
            };

            var request = LoadRequest("PMNMAINT_1138_HPHCI-Acceptance2.json");
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(MSSqlConnectionString, Settings.SQLProvider.SQLServer))
            {
                timer.Start();

                var response = adapter.Execute(request, false);

                timer.Stop();
                Logger.Debug(SerializeJsonToString(response));
            }

            if (RunOracle)
            {
                using (var adapter = Helper.CreatePCORIModelAdapterAdapter(OracleConnectionString, Settings.SQLProvider.Oracle))
                {
                    timer.Start();

                    var oracleResponse = adapter.Execute(request, false);

                    timer.Stop();
                    Logger.Debug(SerializeJsonToString(oracleResponse));
                }
            }

            if (RunPostgreSQL)
            {
                using (var adapter = Helper.CreatePCORIModelAdapterAdapter(PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL))
                {
                    timer.Start();

                    var postgresResponse = adapter.Execute(request, false);

                    timer.Stop();
                    Logger.Debug(SerializeJsonToString(postgresResponse));
                }
            }
        }

        [TestMethod]
        public void PMNMAINT_1138_HPHCI_Acceptance3()
        {
            var timer = new System.Timers.Timer(60 * 1000);
            timer.AutoReset = false;
            timer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
            {
                throw new Exception("Out of time!");
            };

            var request = LoadRequest("PMNMAINT_1138_HPHCI-Acceptance3.json");
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(MSSqlConnectionString, Settings.SQLProvider.SQLServer))
            {
                timer.Start();

                var response = adapter.Execute(request, false);

                timer.Stop();
                Logger.Debug(SerializeJsonToString(response));
            }

            if (RunOracle)
            {
                using (var adapter = Helper.CreatePCORIModelAdapterAdapter(OracleConnectionString, Settings.SQLProvider.Oracle))
                {
                    timer.Start();

                    var oracleResponse = adapter.Execute(request, false);

                    timer.Stop();
                    Logger.Debug(SerializeJsonToString(oracleResponse));
                }
            }

            if (RunPostgreSQL)
            {
                using (var adapter = Helper.CreatePCORIModelAdapterAdapter(PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL))
                {
                    timer.Start();

                    var postgresResponse = adapter.Execute(request, false);

                    timer.Stop();
                    Logger.Debug(SerializeJsonToString(postgresResponse));
                }
            }
        }

        [TestMethod]
        public void PMNMAINT_1138_HPHCI_Acceptance4()
        {
            var timer = new System.Timers.Timer(60 * 1000);
            timer.AutoReset = false;
            timer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
            {
                throw new Exception("Out of time!");
            };

            var request = LoadRequest("PMNMAINT_1138_HPHCI-Acceptance4.json");
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(MSSqlConnectionString, Settings.SQLProvider.SQLServer))
            {
                timer.Start();

                var response = adapter.Execute(request, false);

                timer.Stop();
                Logger.Debug(SerializeJsonToString(response));
            }

            if (RunOracle)
            {
                using (var adapter = Helper.CreatePCORIModelAdapterAdapter(OracleConnectionString, Settings.SQLProvider.Oracle))
                {
                    timer.Start();

                    var oracleResponse = adapter.Execute(request, false);

                    timer.Stop();
                    Logger.Debug(SerializeJsonToString(oracleResponse));
                }
            }

            if (RunPostgreSQL)
            {
                using (var adapter = Helper.CreatePCORIModelAdapterAdapter(PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL))
                {
                    timer.Start();

                    var postgresResponse = adapter.Execute(request, false);

                    timer.Stop();
                    Logger.Debug(SerializeJsonToString(postgresResponse));
                }
            }
        }

        [TestMethod]
        public void PMNMAINT_1154_OracleSchemaIssue()
        {
            var request = LoadRequest("PMNMAINT-1154.json");

            if (RunOracle)
            {
                using (var adapter = Helper.CreatePCORIModelAdapterAdapter(OracleConnectionString, Settings.SQLProvider.Oracle, "C##PCORNETUSER"))
                {
                    var oracleResponse = adapter.Execute(request, false);
                    Logger.Debug(SerializeJsonToString(oracleResponse));
                }

                using (var adapter = Helper.CreatePCORIModelAdapterAdapter(OracleConnectionString, Settings.SQLProvider.Oracle))
                {
                    var oracleResponse = adapter.Execute(request, false);
                    Logger.Debug(SerializeJsonToString(oracleResponse));
                }

                // Compare the two responses. Other than millisec time, should be the same.
            }

        }

        [TestMethod]
        public void PMNDEV_6000_PostgresError()
        {
            var request = LoadRequest("PMNDEV-6000.json");
            using(var adapter = Helper.CreatePCORIModelAdapterAdapter(PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL))
            {
                var postgresResponse = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(postgresResponse));
            }

            //using (var adapter = Helper.CreatePCORIModelAdapterAdapter(MSSqlConnectionString, Settings.SQLProvider.SQLServer))
            //{
            //    var sqlServerResponse = adapter.Execute(request, false);
            //}

            //using (var adapter = Helper.CreatePCORIModelAdapterAdapter(OracleConnectionString, Settings.SQLProvider.Oracle))
            //{
            //    var oracleResponse = adapter.Execute(request, false);
            //}
        }

        [TestMethod]
        public void PMNDEV_6041_CriteriaNotAppliedToCount()
        {
            var request = LoadRequest("PMNDEV-6041.json");
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(MSSqlConnectionString, Settings.SQLProvider.SQLServer))
            {
                var response = adapter.Execute(request, false);
                Logger.Debug(SerializeJsonToString(response));
            }
        }

        [TestMethod]
        public void PMNMAINT_1205_ObservationPeriodAssociation()
        {
            string filename = "PMNMAINT-1205.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            //Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }

        [TestMethod]
        public void PMNMAINT_1172()
        {
            string filename = "PMNMAINT-1172.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            //Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            Assert.IsNull(response.Errors);
            Assert.IsTrue(response.Results.Any());

            var table = response.Results.First();

            if (RunOracle)
            {
                var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
                Assert.IsNull(oracleResponse.Errors);
                Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            }

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            }

            if (RunMySql)
            {
                var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
                Assert.IsNull(mysqlResponse.Errors);
                Assert.AreEqual(table.Count(), mysqlResponse.Results.First().Count());
            }
        }


        [TestMethod]
        public void ProcedureCodes_Cardiac_CT_Scan_PatCount()
        {

            string filename = "ProcedureCodes_Cardiac_CT_Scan_PatCount.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");
            Console.WriteLine("Response Count from MSSQL is " + (response.Results.First().First().First().Value));
            var table = response.Results.First();
            var row = table.First();

            ////make sure that the results have two columns, with the correct names
            //Assert.IsTrue(row.Keys.Count == 2);
            //Assert.IsTrue(row.ContainsKey("Sex"));
            //Assert.IsTrue(row.ContainsKey("Hispanic"));

            //if (RunOracle)
            //{
            //    var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
            //    Assert.IsNull(oracleResponse.Errors);
            //    Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            //}

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
                Console.WriteLine("Response Count from Postgres is " + (response.Results.First().First().First().Value));
            }

            //if (RunMySql)
            //{
            //    var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
            //    Assert.IsNull(mysqlResponse.Errors);
            //    Assert.AreEqual(
            //    table.Count(), mysqlResponse.Results.First().Count());
            //}
        }

        [TestMethod]
        public void ProcedureCodes_Cardiac_CT_Scan_PatCount_WithStrats()
        {

            string filename = "ProcedureCodes_Cardiac_CT_Scan_PatCount_WithStrats.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");
            Console.WriteLine("Response Count from MSSQL is " + (response.Results.First().First().First().Value));
            var table = response.Results.First();
            var row = table.First();

            //if (RunOracle)
            //{
            //    var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
            //    Assert.IsNull(oracleResponse.Errors);
            //    Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            //}

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
                Console.WriteLine("Response Count from Postgres is " + (response.Results.First().First().First().Value));
            }

            //if (RunMySql)
            //{
            //    var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
            //    Assert.IsNull(mysqlResponse.Errors);
            //    Assert.AreEqual(
            //    table.Count(), mysqlResponse.Results.First().Count());
            //}
        }


        [TestMethod]
        public void ProcedureCodes_Cardiac_CT_Scan_PatCount2()
        {

            string filename = "ProcedureCodes_Cardiac_CT_Scan_PatCount2.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");
            Console.WriteLine("Response Count from MSSQL is " + (response.Results.First().First().First().Value));
            var table = response.Results.First();
            var row = table.First();

            //if (RunOracle)
            //{
            //    var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
            //    Assert.IsNull(oracleResponse.Errors);
            //    Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            //}

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
                Console.WriteLine("Response Count from Postgres is " + (response.Results.First().First().First().Value));
            }

            //if (RunMySql)
            //{
            //    var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
            //    Assert.IsNull(mysqlResponse.Errors);
            //    Assert.AreEqual(
            //    table.Count(), mysqlResponse.Results.First().Count());
            //}
        }

        [TestMethod]
        public void ProcedureCodes_Cardiac_CT_Scan_PatCount_WithAge()
        {

            string filename = "ProcedureCodes_Cardiac_CT_Scan_PatCount_WithAge.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");
            Console.WriteLine("Response Count from MSSQL is " + (response.Results.First().First().First().Value));
            var table = response.Results.First();
            var row = table.First();

            ////make sure that the results have two columns, with the correct names
            //Assert.IsTrue(row.Keys.Count == 2);
            //Assert.IsTrue(row.ContainsKey("Sex"));
            //Assert.IsTrue(row.ContainsKey("Hispanic"));

            //if (RunOracle)
            //{
            //    var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
            //    Assert.IsNull(oracleResponse.Errors);
            //    Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            //}

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
                Console.WriteLine("Response Count from Postgres is " + (response.Results.First().First().First().Value));
            }

            //if (RunMySql)
            //{
            //    var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
            //    Assert.IsNull(mysqlResponse.Errors);
            //    Assert.AreEqual(
            //    table.Count(), mysqlResponse.Results.First().Count());
            //}
        }

        [TestMethod]
        public void ProcedureCodes_Cardiac_CT_Scan_AndHypertension_PatCount_WithAge()
        {

            string filename = "ProcedureCodes_Cardiac_CT_Scan_AndHypertension_PatCount_WithAge.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");
            Console.WriteLine("Response Count from MSSQL is " + (response.Results.First().First().First().Value));
            var table = response.Results.First();
            var row = table.First();

            ////make sure that the results have two columns, with the correct names
            //Assert.IsTrue(row.Keys.Count == 2);
            //Assert.IsTrue(row.ContainsKey("Sex"));
            //Assert.IsTrue(row.ContainsKey("Hispanic"));

            //if (RunOracle)
            //{
            //    var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
            //    Assert.IsNull(oracleResponse.Errors);
            //    Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            //}

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
                Console.WriteLine("Response Count from Postgres is " + (response.Results.First().First().First().Value));
            }

            //if (RunMySql)
            //{
            //    var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
            //    Assert.IsNull(mysqlResponse.Errors);
            //    Assert.AreEqual(
            //    table.Count(), mysqlResponse.Results.First().Count());
            //}
        }

        [TestMethod]
        public void ProcedureCodes_Cardiac_CT_Scan_PatCount_WithAgeAndExclusion()
        {

            string filename = "ProcedureCodes_Cardiac_CT_Scan_PatCount_WithAgeAndExclusion.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");
            Console.WriteLine("Response Count from MSSQL is " + (response.Results.First().First().First().Value));
            var table = response.Results.First();
            var row = table.First();

            ////make sure that the results have two columns, with the correct names
            //Assert.IsTrue(row.Keys.Count == 2);
            //Assert.IsTrue(row.ContainsKey("Sex"));
            //Assert.IsTrue(row.ContainsKey("Hispanic"));

            //if (RunOracle)
            //{
            //    var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
            //    Assert.IsNull(oracleResponse.Errors);
            //    Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            //}

            if (RunPostgreSQL)
            {
                var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
                Assert.IsNull(npgsqlResponse.Errors);
                Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
                Console.WriteLine("Response Count from Postgres is " + (response.Results.First().First().First().Value));
            }

            //if (RunMySql)
            //{
            //    var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
            //    Assert.IsNull(mysqlResponse.Errors);
            //    Assert.AreEqual(
            //    table.Count(), mysqlResponse.Results.First().Count());
            //}
        }

		[TestMethod]
		public void PMNSUPPORT_44()
		{

			string filename = "Request Criteria_fd0039_mdq_wp037_nsd1_v01.json";
			var response = RunRequest(filename);
			Logger.Debug(SerializeJsonToString(response));

			Console.WriteLine("Response Count from MSSQL is " + (response.Results.Count()));
			//Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");
			//Console.WriteLine("Response Count from MSSQL is " + (response.Results.First().First().First().Value));
			//var table = response.Results.First();
			//var row = table.First();

			//if (RunOracle)
			//{
			//    var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle);
			//    Assert.IsNull(oracleResponse.Errors);
			//    Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
			//}

			if (RunPostgreSQL)
			{
				var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
				Assert.IsNull(npgsqlResponse.Errors);
				//Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
				Console.WriteLine("Response Count from Postgres is " + (response.Results.Count()));
			}

			//if (RunMySql)
			//{
			//    var mysqlResponse = RunRequest(filename, MySQLConnectionString, Settings.SQLProvider.MySQL);
			//    Assert.IsNull(mysqlResponse.Errors);
			//    Assert.AreEqual(
			//    table.Count(), mysqlResponse.Results.First().Count());
			//}
		}

        [TestMethod]
        public void Oracle11gFailing_PMNDEV_6412()
        {
            string filename = "PMNDEV-6412.json";
            var response = RunRequest(filename);
            Logger.Debug(SerializeJsonToString(response));

            //Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            Assert.IsTrue(response.Errors == null || response.Errors.Any() == false, "There were errors");

            var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle, schema: "PCORNET_MAIN_TEST_3_1");
            Assert.IsTrue(oracleResponse.Errors == null || response.Errors.Any() == false, "There were errors");

            //var table = response.Results.First();
            //var row = table.First();

            //if (RunOracle)
            //{
            //    var oracleResponse = RunRequest(filename, OracleConnectionString, Settings.SQLProvider.Oracle, schema: "PCORNET_MAIN_TEST_3_1");
            //    Assert.IsNull(oracleResponse.Errors);
            //    //Assert.AreEqual(table.Count(), oracleResponse.Results.First().Count());
            //}

            //if (RunPostgreSQL)
            //{
            //    var npgsqlResponse = RunRequest(filename, PostgreSQLConnectionString, Settings.SQLProvider.PostgreSQL);
            //    Assert.IsNull(npgsqlResponse.Errors);
            //    //Assert.AreEqual(table.Count(), npgsqlResponse.Results.First().Count());
            //}

        }

		Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO RunRequest(string requestJsonFilepath)
        {
            return RunRequest(requestJsonFilepath, MSSqlConnectionString, Settings.SQLProvider.SQLServer);
        }

        Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO RunRequest(string requestJsonFilepath, string connectionString, Settings.SQLProvider sqlProvider, string schema = null)
        {
            var request = LoadRequest(requestJsonFilepath);
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(connectionString, sqlProvider, schema))
            {
                return adapter.Execute(request, false);
            }
        }


        Lpp.Dns.DTO.QueryComposer.QueryComposerRequestDTO LoadRequest(string filename, string folder = ResourceFolder)
        {
            string filepath = System.IO.Path.Combine(folder, filename);
            var json = System.IO.File.ReadAllText(filepath);
            Newtonsoft.Json.JsonSerializerSettings jsonSettings = new Newtonsoft.Json.JsonSerializerSettings();
            jsonSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.IgnoreAndPopulate;
            Lpp.Dns.DTO.QueryComposer.QueryComposerRequestDTO request = Newtonsoft.Json.JsonConvert.DeserializeObject<Lpp.Dns.DTO.QueryComposer.QueryComposerRequestDTO>(json, jsonSettings);

            return request;
        }

        static string SerializeJsonToString(Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO response)
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings();
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;

            return Newtonsoft.Json.JsonConvert.SerializeObject(response, settings);
        }


    }
}
