﻿using Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model;
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
        static readonly string Oracle12ConnectionString;

        static readonly log4net.ILog Logger;

        static PCORNET_Adaptable_Queries()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PCORNET"].ConnectionString;
            Oracle12ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PCORNET_ORACLE"].ConnectionString;

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
        public void ConvertAdmitDateToDateOnlyString()
        {
            //using(var db = Helper.CreatePCORIDataContext(Settings.SQLProvider.Oracle, Oracle12ConnectionString, false, "C##PCORNET_4_1_UPDATE"))
            using(var db = Helper.CreatePCORIDataContext(ConnectionString, false))
            {
                db.Database.Log = (l) =>
                {
                    Logger.Info(l);
                };

                var encounters = from enc in db.Encounters
                          select new
                          {
                              enc.ID,
                              enc.AdmittedOn,
                              //AdmittedOnDate = DbFunctions.TruncateTime(enc.AdmittedOn).ToString().Substring(0,10)
                              AdmittedOnDate = enc.AdmittedOn.ToString().Substring(0,10)
                          };

                foreach(var result in encounters.Take(50))
                {
                    Logger.Debug($"ID: {result.ID }\tAdmittedOn: {result.AdmittedOn.ToString() }\tAdmittedOnDate: { result.AdmittedOnDate }");
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
            //var response = RunRequest("PCORNET_PatientCount_TrialID.json", ConnectionString);
            var response = RunRequestForSingleQueryResult("PCORNET_PatientCount_TrialID.json", Oracle12ConnectionString, Settings.SQLProvider.Oracle, "C##PCORNET_4_1_UPDATE");
            var serialized = SerializeJsonToString(response);

            Logger.Debug(serialized);
        }

        [TestMethod]
        public void PatientCountForTrials_ByTrial_Adapter()
        {
            //var response = RunRequest("PCORNET_PatientCount_ByTrial_TrialID.json", ConnectionString);
            var response = RunRequestForSingleQueryResult("PCORNET_PatientCount_ByTrial_TrialID.json", Oracle12ConnectionString, Settings.SQLProvider.Oracle, "C##PCORNET_4_1_UPDATE");
            var serialized = SerializeJsonToString(response);

            Logger.Debug(serialized);
        }

        [TestMethod]
        public void SingleTrialStratifiedByTrialIDAndSex_Adapter()
        {
            var response = RunRequestForSingleQueryResult("PCORNET_TrialID_TrialIDAndSex.json", ConnectionString);
            //var response = RunRequest("PCORNET_TrialID_TrialIDAndSex.json", Oracle12ConnectionString, Settings.SQLProvider.Oracle, "C##PCORNET_4_1_UPDATE");
            var serialized = SerializeJsonToString(response);

            Logger.Debug(serialized);
        }

        [TestMethod]
        public void SingleTrialStratifiedByTrialIDAndSex_WithExclusion_Adapter()
        {
            var response = RunRequestForSingleQueryResult("PCORNET_TrialID_TrialIDAndSex_WithExclusion.json", ConnectionString);
            //var response = RunRequest("PCORNET_TrialID_TrialIDAndSex_WithExclusion.json", Oracle12ConnectionString, Settings.SQLProvider.Oracle, "C##PCORNET_4_1_UPDATE");
            var serialized = SerializeJsonToString(response);

            Logger.Debug(serialized);
        }

        [TestMethod]
        public void PatientCountForPRO_Adapter()
        {
            //var response = RunRequest("PCORNET_PRO_PatientCount.json", ConnectionString);
            var response = RunRequestForSingleQueryResult("PCORNET_PRO_PatientCount.json", Oracle12ConnectionString, Settings.SQLProvider.Oracle, "C##PCORNET_4_1_UPDATE");
            var serialized = SerializeJsonToString(response);

            Logger.Debug(serialized);
        }

        [TestMethod]
        public void PRO_StratifiedByPRO_Adapter()
        {
            //var response = RunRequest("PCORNET_PRO_StratifyByPRO.json", ConnectionString);
            var response = RunRequestForSingleQueryResult("PCORNET_PRO_StratifyByPRO.json", Oracle12ConnectionString, Settings.SQLProvider.Oracle, "C##PCORNET_4_1_UPDATE");
            var serialized = SerializeJsonToString(response);

            Logger.Debug(serialized);
        }

        [TestMethod]
        public void PRO_StratifiedBySex_Adapter()
        {
            //var response = RunRequest("PCORNET_PRO_StratifyBySex.json", ConnectionString);
            var response = RunRequestForSingleQueryResult("PCORNET_PRO_StratifyBySex.json", Oracle12ConnectionString, Settings.SQLProvider.Oracle, "C##PCORNET_4_1_UPDATE");
            var serialized = SerializeJsonToString(response);

            Logger.Debug(serialized);
        }


        [TestMethod]
        public void PRO_StratifiedBySexAndTrial_Adapter()
        {
            //var response = RunRequest("PCORNET_PRO_StratifyBySexAndTrial.json", ConnectionString);
            var response = RunRequestForSingleQueryResult("PCORNET_PRO_StratifyBySexAndTrial.json", Oracle12ConnectionString, Settings.SQLProvider.Oracle, "C##PCORNET_4_1_UPDATE");
            var serialized = SerializeJsonToString(response);

            Logger.Debug(serialized);
        }

        [TestMethod]
        public void PRO_StratifiedBySexAndTrialAndPRO_Adapter()
        {
            //var response = RunRequest("PCORNET_PRO_StratifyBySexAndTrialAndPRO.json", ConnectionString);
            var response = RunRequestForSingleQueryResult("PCORNET_PRO_StratifyBySexAndTrialAndPRO.json", Oracle12ConnectionString, Settings.SQLProvider.Oracle, "C##PCORNET_4_1_UPDATE");
            var serialized = SerializeJsonToString(response);

            Logger.Debug(serialized);
        }

        [TestMethod]
        public void PRO_StratifiedBySexAndTrialAndPRO_WithExclusion_Adapter()
        {
            var response = RunRequestForSingleQueryResult("PCORNET_PRO_StratifyBySexAndTrialAndPRO_WithExclusion.json", ConnectionString);
            //var response = RunRequest("PCORNET_PRO_StratifyBySexAndTrialAndPRO_WithExclusion.json", Oracle12ConnectionString, Settings.SQLProvider.Oracle, "C##PCORNET_4_1_UPDATE");
            var serialized = SerializeJsonToString(response);

            Logger.Debug(serialized);
        }

        [TestMethod]
        public void PCORNET_Adaptable_TimeWindow_TrialID()
        {
            var response = RunRequestForSingleQueryResult("PCORNET_Adaptable_TimeWindow_TrialID.json", ConnectionString);            
            var serialized = SerializeJsonToString(response);

            Logger.Debug(serialized);

            Logger.Debug("### ORACLE ###");
            var oracleResponse = RunRequestForSingleQueryResult("PCORNET_Adaptable_TimeWindow_TrialID.json", Oracle12ConnectionString, Settings.SQLProvider.Oracle, "C##PCORNET_4_1_UPDATE");
            var oracleSerialized = SerializeJsonToString(oracleResponse);
            Logger.Debug(oracleSerialized);
        }

        [TestMethod]
        public void PCORNET_Adaptable_TimeWindow_MultiTrialID()
        {
            var response = RunRequestForSingleQueryResult("PCORNET_Adaptable_TimeWindow_MultiTrialID.json", ConnectionString);
            var serialized = SerializeJsonToString(response);

            Logger.Debug(serialized);

            Logger.Debug("### ORACLE ###");
            var oracleResponse = RunRequestForSingleQueryResult("PCORNET_Adaptable_TimeWindow_MultiTrialID.json", Oracle12ConnectionString, Settings.SQLProvider.Oracle, "C##PCORNET_4_1_UPDATE");
            var oracleSerialized = SerializeJsonToString(oracleResponse);
            Logger.Debug(oracleSerialized);
        }

        [TestMethod]
        public void PCORNET_Adaptable_TimeWindow_TrialID_PRO()
        {
            var response = RunRequestForSingleQueryResult("PCORNET_Adaptable_TimeWindow_TrialID_PRO.json", ConnectionString);
            var serialized = SerializeJsonToString(response);
            Logger.Debug(serialized);

            Logger.Debug("### ORACLE ###");
            var oracleResponse = RunRequestForSingleQueryResult("PCORNET_Adaptable_TimeWindow_TrialID_PRO.json", Oracle12ConnectionString, Settings.SQLProvider.Oracle, "C##PCORNET_4_1_UPDATE");
            var oracleSerialized = SerializeJsonToString(oracleResponse);
            Logger.Debug(oracleSerialized);
        }

        [TestMethod]
        public void PCORNET_Adaptable_TimeWindow_TrialID_WithCriteria()
        {
            var response = RunRequestForSingleQueryResult("PCORNET_Adaptable_TimeWindow_TrialID_WithCriteria.json", ConnectionString);
            var serialized = SerializeJsonToString(response);
            Logger.Debug(serialized);

            Logger.Debug("### ORACLE ###");
            var oracleResponse = RunRequestForSingleQueryResult("PCORNET_Adaptable_TimeWindow_TrialID_WithCriteria.json", Oracle12ConnectionString, Settings.SQLProvider.Oracle, "C##PCORNET_4_1_UPDATE");
            var oracleSerialized = SerializeJsonToString(oracleResponse);
            Logger.Debug(oracleSerialized);
        }

        [TestMethod]
        public void PCORNET_Adaptable_UC2_PMNDEV_7294()
        {
            var response = RunRequestForSingleQueryResult("PCORNET_Adaptable_UC2_PMNDEV-7294.json", ConnectionString);
            var serialized = SerializeJsonToString(response);
            Logger.Debug(serialized);

            Logger.Debug("### ORACLE ###");
            var oracleResponse = RunRequestForSingleQueryResult("PCORNET_Adaptable_UC2_PMNDEV-7294.json", Oracle12ConnectionString, Settings.SQLProvider.Oracle, "C##PCORNET_4_1_UPDATE");
            var oracleSerialized = SerializeJsonToString(oracleResponse);
            Logger.Debug(oracleSerialized);
        }

        [TestMethod]
        public void PCORNET_Adaptable_UC2_PMNDEV_7297_1()
        {
            var response = RunRequestForSingleQueryResult("PCORNET_Adaptable_UC2_PMNDEV-7297-1.json", ConnectionString);
            var serialized = SerializeJsonToString(response);
            Logger.Debug(serialized);

            //Logger.Debug("### ORACLE ###");
            //var oracleResponse = RunRequest("PCORNET_Adaptable_UC2_PMNDEV-7297-1.json", Oracle12ConnectionString, Settings.SQLProvider.Oracle, "C##PCORNET_4_1_UPDATE");
            //var oracleSerialized = SerializeJsonToString(oracleResponse);
            //Logger.Debug(oracleSerialized);
        }

        [TestMethod]
        public void PCORNET_Adaptable_json47766()
        {
            var response = RunRequestForSingleQueryResult("PCORNET_Adaptable_json47766.json", ConnectionString);
            var serialized = SerializeJsonToString(response);
            Logger.Debug(serialized);

            //Logger.Debug("### ORACLE ###");
            //var oracleResponse = RunRequest("PCORNET_Adaptable_json47766.json", Oracle12ConnectionString, Settings.SQLProvider.Oracle, "C##PCORNET_4_1_UPDATE");
            //var oracleSerialized = SerializeJsonToString(oracleResponse);
            //Logger.Debug(oracleSerialized);
        }

        [TestMethod]
        public void PCORNET_Adaptable_json47760_PMNDEV_7296()
        {
            var response = RunRequestForSingleQueryResult("PCORNET_Adaptable_json47760_PMNDEV_7296.json", ConnectionString);
            var serialized = SerializeJsonToString(response);
            Logger.Debug(serialized);

            var firstResult = response.Queries.FirstOrDefault();
            foreach (var table in firstResult.Results)
            {
                foreach (var row in table)
                {
                    Logger.Debug($"{row["PRO_ITEM_NAME"]}\t{row["TRIALID"]}\t{row["PARTICIPANTID"]}");
                }
            }

            //Logger.Debug("### ORACLE ###");
            //var oracleResponse = RunRequest("PCORNET_Adaptable_json47760_PMNDEV_7296.json", Oracle12ConnectionString, Settings.SQLProvider.Oracle, "C##PCORNET_4_1_UPDATE");
            //var oracleSerialized = SerializeJsonToString(oracleResponse);
            //Logger.Debug(oracleSerialized);
        }


        static Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO RunRequestForSingleQueryResult(string requestJsonFilepath, string connectionString, Settings.SQLProvider sqlProvider = Settings.SQLProvider.SQLServer, string schema = null)
        {
            var request = LoadRequest(requestJsonFilepath);
            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(connectionString, sqlProvider, schema))
            {
                List<DTO.QueryComposer.QueryComposerResponseQueryResultDTO> queryResults = new List<DTO.QueryComposer.QueryComposerResponseQueryResultDTO>();
                foreach(var query in request.Queries)
                {
                    foreach(var result in adapter.Execute(query, false))
                    {
                        queryResults.Add(result);
                    }
                }
                var response = new DTO.QueryComposer.QueryComposerResponseDTO {
                    Header = new DTO.QueryComposer.QueryComposerResponseHeaderDTO
                    {
                        DocumentID = Guid.NewGuid(),
                        RequestID = Guid.NewGuid()
                    },
                    Queries = queryResults
                };
                response.RefreshQueryDates();
                response.RefreshErrors();

                return response;
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
