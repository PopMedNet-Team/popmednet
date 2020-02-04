using Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model;
using Lpp.Objects.Dynamic;
using Lpp.QueryComposer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Lpp.Dns.DataMart.Model.QueryComposer.Tests
{
    [TestClass]
    public class PCORNET_Adaptable_Queries
    {
        const string ResourceFolder = "../Resources/QueryComposition";
        static readonly string ConnectionString;

        static readonly log4net.ILog Logger;

        static PCORNET_Adaptable_Queries()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PCORNET"].ConnectionString;

            log4net.Config.XmlConfigurator.Configure();
            Logger = log4net.LogManager.GetLogger(typeof(PCORNET_Adaptable_Queries));
        }

        [TestMethod]
        public void QueryForTrials_LINQ()
        {

            using(var db = Helper.CreatePCORIDataContext(ConnectionString, false))
            {
                db.Database.Log = (l) =>
                {
                    Logger.Info(l);
                };

                var q = from p in db.Patients
                        join tr in db.ClinicalTrials on p.ID equals tr.PatientID
                        orderby tr.TrialID, tr.PatientID
                        select new
                        {
                            tr.PatientID,
                            tr.TrialID,
                            tr.ParticipantID
                        };

                Logger.Debug(q.Expression.ToString());

                foreach (var p in q.Take(1000))
                {
                    Logger.Debug($"TrialID: {p.TrialID}\tPatientID: {p.PatientID}\tParticipantID: {p.ParticipantID}");
                }
            }
        }

        [TestMethod]
        public void ExpressionJoin()
        {
            //using (var db = Helper.CreatePCORIDataContext(ConnectionString, false))
            //{
            //    db.Database.Log = (l) => Logger.Info(l);

            //    var q = db.Patients
            //        .Join(db.ClinicalTrials, p => p.ID, tr => tr.PatientID, (p, tr) => new { tr.TrialID, tr.ParticipantID, tr.PatientID })
            //        .Select(r => r.)

            //}

            
        }

        [TestMethod]
        public void QueryForTrials_Expressions()
        {
            using (var db = Helper.CreatePCORIDataContext(ConnectionString, false)) {
                db.Database.Log = (l) => Logger.Info(l);

                //ParameterExpression patientParam = Expression.Parameter(typeof(Patient), "p");
                //ParameterExpression trialParam = Expression.Parameter(typeof(ClinicalTrial), "tr");

                //Expression<Func<Patient, string>> outerKeySelector = Expression.Lambda<Func<Patient, string>>(Expression.Convert(Expression.PropertyOrField(patientParam, "ID"), typeof(string)), patientParam);
                //Expression<Func<ClinicalTrial, string>> innerKeySelector = Expression.Lambda<Func<ClinicalTrial, string>>(Expression.Convert(Expression.PropertyOrField(trialParam, "PatientID"), typeof(string)), trialParam);

                //Expression<Func<Patient, ClinicalTrial, IQueryable>> resultSelector = Expression.Lambda<Func<Patient, ClinicalTrial, IQueryable>>(db.Patients.AsQueryable().Expression, patientParam, trialParam);


                //var join = Expression.Call(typeof(Queryable), "Join", new Type[] {
                //    typeof(Patient),
                //    typeof(ClinicalTrial),

                //}
                //    );

                var query = (from p in db.Patients
                             where p.Sex == "F"
                             select new { PatientID = p.ID, p.Sex })
                             .Join(
                        from tr in db.ClinicalTrials where tr.TrialID == "FAKE_TRIAL-15" select new { tr.TrialID, tr.ParticipantID, tr.PatientID },
                        p => p.PatientID,
                        tr => tr.PatientID,
                        (p, tr) => new { p.PatientID, p.Sex, tr.TrialID, tr.ParticipantID }
                        );

                Logger.Debug(query.Expression.ToString());

                var result = query.ToArray();

                foreach(var r in result)
                {
                    Logger.Debug($"PatientID: {r.PatientID}\tSex:{r.Sex}\tTrialID:{r.TrialID}\tParticipantID:{r.ParticipantID}");
                }

            }
        }

        [TestMethod]
        public void PatientCountForTrials_LINQ()
        {
            using(var db = Helper.CreatePCORIDataContext(ConnectionString))
            {
                db.Database.Log = (l) => Logger.Debug(l);

                var q = from p in db.Patients
                        join tr in db.ClinicalTrials on p.ID equals tr.PatientID
                        where p.ClinicalTrials.Any(t => t.TrialID == "FAKE_TRIAL-15")
                        select new {
                            PatientID = p.ID,
                            TrialID = tr.TrialID, 
                            ParticipantID = tr.ParticipantID
                        };

                //Logger.Debug($"{ q.Count() } patients in trial \"FAKE_TRIAL-15\"");

                foreach(var r in q)
                {
                    Logger.Debug($"PatientID:{r.PatientID}\tTrialID:{r.TrialID}\tParticipantID:{r.ParticipantID}");
                }
            }
        }

        [TestMethod]
        public void PatientCountForTrials_Adapter()
        {
            var response = RunRequest("PCORNET_PatientCount_TrialID.json", ConnectionString);
            var serialized = SerializeJsonToString(response);

            Logger.Debug(serialized);
        }

        [TestMethod]
        public void PatientCountForTrials_ByTrial_Adapter()
        {
            var response = RunRequest("PCORNET_PatientCount_ByTrial_TrialID.json", ConnectionString);
            var serialized = SerializeJsonToString(response);

            Logger.Debug(serialized);
        }

        [TestMethod]
        public void SingleTrialStratifiedByTrialIDAndSex_Adapter()
        {
            var response = RunRequest("PCORNET_TrialID_TrialIDAndSex.json", ConnectionString);
            var serialized = SerializeJsonToString(response);

            Logger.Debug(serialized);
        }

        [TestMethod]
        public void PatientCountForPRO_Adapter()
        {
            var response = RunRequest("PCORNET_PRO_PatientCount.json", ConnectionString);
            var serialized = SerializeJsonToString(response);

            Logger.Debug(serialized);
        }

        [TestMethod]
        public void PRO_StratifiedByPRO_Adapter()
        {
            var response = RunRequest("PCORNET_PRO_StratifyByPRO.json", ConnectionString);
            var serialized = SerializeJsonToString(response);

            Logger.Debug(serialized);
        }

        [TestMethod]
        public void PRO_StratifiedBySex_Adapter()
        {
            var response = RunRequest("PCORNET_PRO_StratifyBySex.json", ConnectionString);
            var serialized = SerializeJsonToString(response);

            Logger.Debug(serialized);
        }


        [TestMethod]
        public void PRO_StratifiedBySexAndTrial_Adapter()
        {
            var response = RunRequest("PCORNET_PRO_StratifyBySexAndTrial.json", ConnectionString);
            var serialized = SerializeJsonToString(response);

            Logger.Debug(serialized);
        }

        [TestMethod]
        public void PRO_StratifiedBySexAndTrialAndPRO_Adapter()
        {
            var response = RunRequest("PCORNET_PRO_StratifyBySexAndTrialAndPRO.json", ConnectionString);
            var serialized = SerializeJsonToString(response);

            Logger.Debug(serialized);
        }


        static Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO RunRequest(string requestJsonFilepath, string connectionString, Settings.SQLProvider sqlProvider = Settings.SQLProvider.SQLServer, string schema = null)
        {
            var request = LoadRequest(requestJsonFilepath);
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(connectionString, sqlProvider, schema))
            {
                return adapter.Execute(request, false);
            }
        }


        static Lpp.Dns.DTO.QueryComposer.QueryComposerRequestDTO LoadRequest(string filename, string folder = ResourceFolder)
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
