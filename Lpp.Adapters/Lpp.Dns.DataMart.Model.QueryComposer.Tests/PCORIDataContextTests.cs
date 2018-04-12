using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Tests
{
    [TestClass]
    public class PCORIDataContextTests
    {
        static readonly log4net.ILog Logger;
        static readonly string ConnectionString;
        static readonly string PostgreSQLConnectionString;
        static readonly string MySQLConnectionString;
        static readonly string OracleConnectionString;

        static PCORIDataContextTests()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PCORNET"].ConnectionString;
            PostgreSQLConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PCORNET_PostgreSQL"].ConnectionString;
            MySQLConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PCORNET_MySQL"].ConnectionString;
            OracleConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PCORNET_ORACLE"].ConnectionString;

            log4net.Config.XmlConfigurator.Configure();
            Logger = log4net.LogManager.GetLogger(typeof(PCORIDataContextTests));
        }

        [TestMethod]
        public void ConfirmTables_SqlServer()
        {
            using (var db = Helper.CreatePCORIDataContext(Model.Settings.SQLProvider.SQLServer, ConnectionString))
            {
                var patient = db.Patients.FirstOrDefault();
                var causeOfDeath = db.CauseOfDeaths.FirstOrDefault();
                var clinicalTrial = db.ClinicalTrials.FirstOrDefault();
                var condition = db.Conditions.FirstOrDefault();
                var death = db.Deaths.FirstOrDefault();
                var diagnosis = db.Diagnoses.FirstOrDefault();
                var prescription = db.Prescriptions.FirstOrDefault();
                var dispensing = db.Dispensings.FirstOrDefault();
                var encounter = db.Encounters.FirstOrDefault();
                var enrollment = db.Enrollments.FirstOrDefault();
                var labResult = db.LabResultCommonMeasures.FirstOrDefault();
                var procedure = db.Procedures.FirstOrDefault();
                var reportedOutcome = db.ReportedOutcomeCommonMeasures.FirstOrDefault();
                var vital = db.Vitals.FirstOrDefault();
            }

        }

        [TestMethod]
        public void ConfirmTables_PostgreSQL()
        {
            using (var db = Helper.CreatePCORIDataContext(Model.Settings.SQLProvider.PostgreSQL, PostgreSQLConnectionString))
            {
                var patient = db.Patients.Select(p => p.ID).FirstOrDefault();
                var causeOfDeath = db.CauseOfDeaths.FirstOrDefault();
                var clinicalTrial = db.ClinicalTrials.FirstOrDefault();
                var condition = db.Conditions.FirstOrDefault();
                var death = db.Deaths.FirstOrDefault();
                var diagnosis = db.Diagnoses.FirstOrDefault();
                var prescription = db.Prescriptions.FirstOrDefault();
                var dispensing = db.Dispensings.FirstOrDefault();
                var encounter = db.Encounters.FirstOrDefault();
                var enrollment = db.Enrollments.FirstOrDefault();
                var labResult = db.LabResultCommonMeasures.FirstOrDefault();
                var procedure = db.Procedures.FirstOrDefault();
                var reportedOutcome = db.ReportedOutcomeCommonMeasures.FirstOrDefault();
                var vital = db.Vitals.FirstOrDefault();
            }
        }

        [TestMethod]
        public void PostgreSQL1()
        {
            var settings = new Dictionary<string, object>(){
                    {"Server", "10.28.119.215" },
                    {"Port", "5434" },
                    {"UserID", "pcornet" },
                    {"Password", "HpHc082817@#" },
                    {"Database", "PcorNetV3" },
                    {"ConnectionTimeout", "60" },
                    {"CommandTimeout", "60"},
                    {"DataProvider", Model.Settings.SQLProvider.PostgreSQL.ToString()}
                };

            var connBuilder = new Npgsql.NpgsqlConnectionStringBuilder();
            connBuilder.Host = settings["Server"].ToString();
            connBuilder.Port = Convert.ToInt32(settings["Port"]);
            connBuilder.Username = settings["UserID"].ToString();
            connBuilder.Password = settings["Password"].ToString();
            connBuilder.Database = settings["Database"].ToString();

            string connectionString = connBuilder.ToString();

            using (var connection = new Npgsql.NpgsqlConnection(connBuilder.ToString()))
            {
                /** Of the connection is opened prior to giving to the datacontext Npgsql error when trying to execute query **/
                connection.Open();
                //using (var db = new Lpp.Dns.DataMart.Model.PCORIQueryBuilder.DataContext(connection, "dbo"))
                //{
                //    var patient = db.Patients.Select(p => p.ID).FirstOrDefault();
                //}
            }
        }

        [TestMethod]
        public void ConfirmDbFunctions_PostgreSQL()
        {
            using (var db = Helper.CreatePCORIDataContext(Model.Settings.SQLProvider.PostgreSQL, PostgreSQLConnectionString))
            {
                try
                {
                    DateTime now = DateTime.Now.Date;
                    //var query = db.Patients.Select(p => System.Data.Entity.DbFunctions.DiffYears(p.BornOn, now)).FirstOrDefault();

                    //var query = db.Patients.Select(p => System.Data.Entity.DbFunctions.DiffMonths(p.BornOn, now)).FirstOrDefault();
                    var query = db.Patients.Select(p => System.Data.Entity.DbFunctions.DiffMonths(p.BornOn, System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)));

                    Console.WriteLine(query.Expression.ToString());
                    Console.WriteLine(query.ToString());

                    query = db.Patients.Select(p => System.Data.Entity.DbFunctions.DiffMonths(p.BornOn, System.Data.Entity.DbFunctions.TruncateTime(now)));

                    Console.WriteLine(query.Expression.ToString());
                    Console.WriteLine(query.ToString());
                    query.FirstOrDefault();

                    
                }
                catch (Exception ex)
                {
                    //currently failing using 2.2.4, apparently fixed in latest code, need to see if there is a newer version than the google forum
                    //2.2.5 is available released on March 11, 2015
                    //3.0 beta is available released on May 25

                    //https://github.com/npgsql/npgsql/issues/624

                    Assert.Fail(ex.UnwindException());
                }

                //not supported, used for building the stratification value used by the EncounterObservationPeriod term.
                //var query2 = db.Patients.Where(p => p.BornOn.HasValue).Select(p => System.Data.Entity.DbFunctions.CreateDateTime(p.BornOn.Value.Year, 1, 1, 0, 0, 0d)).FirstOrDefault();

                //going to have to modify the EncounterObservationPeriod stratification not to use the CreateDateTime function, not supported by PostgreSQL.
                //not going to have to futher test this function, will not be used after update applied
                //var query2 = db.Patients.Where(p => p.BornOn.HasValue).Select(p => p.BornOn.Value.Year + "-" + p.BornOn.Value.Month).FirstOrDefault();                

            }
        }


        [TestMethod]
        public void ConfirmTables_MySQL()
        {
            using (var db = Helper.CreatePCORIDataContext(Model.Settings.SQLProvider.MySQL, MySQLConnectionString))
            {
                var patient = db.Patients.Select(p => p.ID).FirstOrDefault();
                var causeOfDeath = db.CauseOfDeaths.FirstOrDefault();
                var clinicalTrial = db.ClinicalTrials.FirstOrDefault();
                var condition = db.Conditions.FirstOrDefault();
                var death = db.Deaths.FirstOrDefault();
                var diagnosis = db.Diagnoses.FirstOrDefault();
                var prescription = db.Prescriptions.FirstOrDefault();
                var dispensing = db.Dispensings.FirstOrDefault();
                var encounter = db.Encounters.FirstOrDefault();
                var enrollment = db.Enrollments.FirstOrDefault();
                var labResult = db.LabResultCommonMeasures.FirstOrDefault();
                var procedure = db.Procedures.FirstOrDefault();
                var reportedOutcome = db.ReportedOutcomeCommonMeasures.FirstOrDefault();
                var vital = db.Vitals.FirstOrDefault();
            }
        }

        [TestMethod]
        public void ConfirmDbFunctions_MySQL()
        {
            using (var db = Helper.CreatePCORIDataContext(Model.Settings.SQLProvider.MySQL, MySQLConnectionString))
            {
                try
                {
                    DateTime now = DateTime.Now.Date;
                    var query = db.Patients.Select(p => System.Data.Entity.DbFunctions.DiffYears(p.BornOn, now)).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.UnwindException());
                }
                //not fully tested yet, but apparently DiffYears does not exist
            }
        }


        [TestMethod]
        public void ConfirmTables_Oracle()
        {
            using (var db = Helper.CreatePCORIDataContext(Model.Settings.SQLProvider.Oracle, OracleConnectionString))
            {
                var patient = db.Patients.FirstOrDefault();
                var causeOfDeath = db.CauseOfDeaths.FirstOrDefault();
                var clinicalTrial = db.ClinicalTrials.FirstOrDefault();
                var condition = db.Conditions.FirstOrDefault();
                var death = db.Deaths.FirstOrDefault();
                var diagnosis = db.Diagnoses.FirstOrDefault();
                var prescription = db.Prescriptions.FirstOrDefault();
                var dispensing = db.Dispensings.FirstOrDefault();
                var encounter = db.Encounters.FirstOrDefault();
                var enrollment = db.Enrollments.FirstOrDefault();
                var labResult = db.LabResultCommonMeasures.FirstOrDefault();
                var procedure = db.Procedures.FirstOrDefault();
                var reportedOutcome = db.ReportedOutcomeCommonMeasures.FirstOrDefault();
                var vital = db.Vitals.FirstOrDefault();
            }
        }

        [TestMethod]
        public void ConfirmDbFunctions_Oracle()
        {
            using (var db = Helper.CreatePCORIDataContext(Model.Settings.SQLProvider.Oracle, OracleConnectionString))
            {
                try
                {
                    DateTime now = DateTime.Now.Date;
                    var query = db.Patients.Select(p => System.Data.Entity.DbFunctions.DiffYears(p.BornOn, now)).FirstOrDefault();
                    Console.WriteLine(query);
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.UnwindException());
                }
            }
        }

        [TestMethod]
        public void ConfirmOracleAgeCalculation()
        {

            DateTime asOf = DateTime.Now;
            using (var db = Helper.CreatePCORIDataContext(Model.Settings.SQLProvider.Oracle, OracleConnectionString))
            {
                var query = db.Patients;
                var result = query.Where(p => p.BornOn.HasValue).Select(p => new
                {
                    BornOn = p.BornOn.Value,
                    Age = p.BornOn.HasValue == false ? null : (int?)(DbFunctions.DiffYears(p.BornOn.Value, asOf).Value - (((p.BornOn.Value.Month > asOf.Month) || (p.BornOn.Value.Month == asOf.Month && p.BornOn.Value.Day > asOf.Day)) ? 1 : 0)),
                    YearDiff = DbFunctions.DiffYears(p.BornOn, asOf)
                }).OrderBy(p => p.BornOn);



                //loop through results and confirm age calculation
                Dictionary<DateTime, string> failed = new Dictionary<DateTime,string>();

                foreach (var item in result)
                {
                    DateTime birthdate = item.BornOn;
                    int? databaseCalculatedAge = item.Age;


                    int computedAge = (asOf.Year - birthdate.Year) - ((birthdate.Month > asOf.Month || (birthdate.Month == asOf.Month && birthdate.Day > asOf.Day)) ? 1 : 0);

                    if (computedAge != databaseCalculatedAge)
                    {
                        if (!failed.ContainsKey(birthdate))
                        {
                            failed.Add(birthdate, string.Format("{0:yyyy-MM-dd}:\t{1}\t{2}\t{3}\t{4}", birthdate, databaseCalculatedAge, computedAge, item.YearDiff, (asOf.Year - birthdate.Year)));
                        }
                    }

                }

                if (failed.Count > 0)
                {
                    Console.WriteLine("Date\t\tDB\tCalc\tYDB\tYCalc");
                    foreach(var pair in failed)
                        Console.WriteLine(pair.Value);
                }

            }

        }

    }
}
