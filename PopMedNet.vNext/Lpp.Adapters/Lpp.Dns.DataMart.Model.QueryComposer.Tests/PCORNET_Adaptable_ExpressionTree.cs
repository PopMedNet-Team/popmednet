using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model;
using Lpp.Objects.Dynamic;
using System.Linq.Expressions;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Tests
{
    [TestClass]
    public class PCORNET_Adaptable_ExpressionTree
    {
        static readonly log4net.ILog Logger;
        static readonly string ConnectionString;

        static PCORNET_Adaptable_ExpressionTree()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PCORNET"].ConnectionString;

            log4net.Config.XmlConfigurator.Configure();
            Logger = log4net.LogManager.GetLogger(typeof(PCORNET_Adaptable_ExpressionTree));
        }

        [TestMethod]
        public void GetIndexEvents_LINQ()
        {
            using (var db = Helper.CreatePCORIDataContext(ConnectionString))
            {
                //var q = from pro in db.ReportedOutcomeCommonMeasures
                //        join tr in db.ClinicalTrials on pro.PatientID equals tr.PatientID
                //        where pro.ItemName == "HOSPITALIZATION_DATE" && tr.TrialID == "FAKE_TRIAL-15" && pro.ResponseNumber != null
                //        select new
                //        {
                //            PRO_ID = pro.ID,
                //            PatientID = pro.PatientID,
                //            ItemName = pro.ItemName,
                //            ResponseText = pro.ResponseText,
                //            ResponseNumber = pro.ResponseNumber,
                //            ResponseSequence = pro.MeasureSequence,
                //            ParticipantID = tr.ParticipantID,
                //            TrialID = tr.TrialID,
                //            TimeWindowAfter = pro.ResponseNumber + 300,
                //            TimeWindowBefore = pro.ResponseNumber - 0,
                //        };


                //var q = db.ReportedOutcomeCommonMeasures
                //    .Join(db.ClinicalTrials, pro => pro.PatientID, tr => tr.PatientID, (pro, tr) => new { P1 = pro, P2 = tr })
                //    .Where(j => j.P1.ItemName == "HOSPITALIZATION_DATE" && j.P2.TrialID == "FAKE_TRIAL-15" && j.P1.ResponseNumber != null)
                //    .Select(j => new {
                //        PRO_ID = j.P1.ID,
                //        PatientID = j.P1.PatientID,
                //        ItemName = j.P1.ItemName,
                //        ResponseText = j.P1.ResponseText,
                //        ResponseNumber = j.P1.ResponseNumber,
                //        ResponseSequence = j.P1.MeasureSequence,
                //        ParticipantID = j.P2.ParticipantID,
                //        TrialID = j.P2.TrialID,
                //        TimeWindowAfter = j.P1.ResponseNumber + 300,
                //        TimeWindowBefore = j.P1.ResponseNumber - 0
                //    });


                var q = db.ReportedOutcomeCommonMeasures
                    .Join(db.ClinicalTrials, pro => pro.PatientID, tr => tr.PatientID, (pro, tr) => new {
                        PRO_ID = pro.ID,
                        PatientID = pro.PatientID,
                        ItemName = pro.ItemName,
                        ResponseText = pro.ResponseText,
                        ResponseNumber = pro.ResponseNumber,
                        ResponseSequence = pro.MeasureSequence,
                        ParticipantID = tr.ParticipantID,
                        TrialID = tr.TrialID,
                        TimeWindowAfter = pro.ResponseNumber + 300,
                        TimeWindowBefore = pro.ResponseNumber - 0
                    })
                    .Where(j => j.ItemName == "HOSPITALIZATION_DATE" && j.TrialID == "FAKE_TRIAL-15" && j.ResponseNumber != null);

                Logger.Debug(q.Expression.ToString());

                var results = q.ToArray();

                foreach (var r in results)
                {
                    Logger.Debug($"PRO_ID: {r.PRO_ID }, PatID: { r.PatientID }, ItemName: {r.ItemName}, ResponseText: {r.ResponseText}, ResponseNumber: {r.ResponseNumber}, MeasureSequence: {r.ResponseSequence}, ParticipantID: {r.ParticipantID}, TrialID: {r.TrialID}, TimeWindowAfter: {r.TimeWindowAfter}, TimeWindowBefore: {r.TimeWindowBefore}");
                }
            }
        }

        [TestMethod]
        public void LinqJoin()
        {
            using(var db = Helper.CreatePCORIDataContext(ConnectionString))
            {
                var query = db.ReportedOutcomeCommonMeasures
                    .Join(db.Encounters, pro => pro.PatientID, enc => enc.PatientID, (pro, enc) => new { PRO = pro, ENC = enc });

                Logger.Debug(query.Expression.ToString());

                var pe_pro = Expression.Parameter(typeof(ReportedOutcome), "pro");
                var pe_enc = Expression.Parameter(typeof(Encounter), "enc");

                var joinType = TypeBuilderHelper.CreateType("PRO_Enc", new IPropertyDefinition[] {
                    new TypedPropertyDefinition<ReportedOutcome>{ Name = "PRO" },
                    new TypedPropertyDefinition<Encounter> { Name = "ENC" }
                });

                var joinBindings = new[] {
                    Expression.Bind(joinType.GetProperty("PRO"), pe_pro),
                    Expression.Bind(joinType.GetProperty("ENC"), pe_enc)
                };

                var resultSelector = Expression.Lambda(
                        Expression.MemberInit(Expression.New(joinType), joinBindings),
                        pe_pro,
                        pe_enc
                    );

                var joinCall = Expression.Call(
                    typeof(Queryable),
                    "Join",
                    new Type[] {
                        typeof(ReportedOutcome),
                        typeof(Encounter),
                        typeof(string),
                        joinType
                    },
                    new Expression[] {
                    db.ReportedOutcomeCommonMeasures.AsQueryable().Expression,
                    db.Encounters.AsQueryable().Expression,
                    Expression.Quote(Expression.Lambda(Expression.Property(pe_pro, "PatientID"), pe_pro)),
                    Expression.Quote(Expression.Lambda(Expression.Property(pe_enc, "PatientID"), pe_enc)),
                    Expression.Quote(resultSelector)
                    }
                    );


                Logger.Debug(joinCall.ToString());
            }
        }

        [TestMethod]
        public void GetIndexEvents_ExpressionTree()
        {
            using (var db = Helper.CreatePCORIDataContext(ConnectionString))
            {
                ParameterExpression pe_proQueryType = ParameterExpression.Parameter(typeof(PCORIQueryBuilder.Model.ReportedOutcome), "pro");
                ParameterExpression pe_clinicalTrialsQueryType = ParameterExpression.Parameter(typeof(ClinicalTrial), "tr");
                ConstantExpression ce_daysBefore = ConstantExpression.Constant(0f, typeof(float));
                ConstantExpression ce_daysAfter = ConstantExpression.Constant(300f, typeof(float));
                ConstantExpression ce_proItemName = ConstantExpression.Constant("HOSPITALIZATION_DATE", typeof(string));
                ConstantExpression ce_trialID = ConstantExpression.Constant("FAKE_TRIAL-15", typeof(string));


                var outerSelector = Expression.Lambda(Expression.Property(pe_proQueryType, "PatientID"), pe_proQueryType);
                var innerSelector = Expression.Lambda(Expression.Property(pe_clinicalTrialsQueryType, "PatientID"), pe_clinicalTrialsQueryType);

                Type joinResultType = Objects.Dynamic.TypeBuilderHelper.CreateType("IndexEvents", new[] {
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name="PRO_ID", Type="System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name="PatientID", Type="System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name="ItemName", Type="System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name="ResponseText", Type="System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name="ResponseNumber", Type= typeof(double?).ToString() },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name="ResponseSequence", Type="System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name="ParticipantID", Type="System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name="TrialID", Type="System.String" },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name="TimeWindowAfter", Type= typeof(float).ToString() },
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{ Name="TimeWindowBefore", Type= typeof(float).ToString() }
                });

                IEnumerable<MemberBinding> joinBindings = new[] {
                    Expression.Bind(joinResultType.GetProperty("PRO_ID"), Expression.Property(pe_proQueryType, pe_proQueryType.Type.GetProperty("ID"))),
                    Expression.Bind(joinResultType.GetProperty("PatientID"), Expression.Property(pe_proQueryType, pe_proQueryType.Type.GetProperty("PatientID"))),
                    Expression.Bind(joinResultType.GetProperty("ItemName"), Expression.Property(pe_proQueryType, pe_proQueryType.Type.GetProperty("ItemName"))),
                    Expression.Bind(joinResultType.GetProperty("ResponseText"), Expression.Property(pe_proQueryType, pe_proQueryType.Type.GetProperty("ResponseText"))),
                    Expression.Bind(joinResultType.GetProperty("ResponseNumber"), Expression.Property(pe_proQueryType, pe_proQueryType.Type.GetProperty("ResponseNumber"))),
                    Expression.Bind(joinResultType.GetProperty("ResponseSequence"), Expression.Property(pe_proQueryType, pe_proQueryType.Type.GetProperty("MeasureSequence"))),
                    Expression.Bind(joinResultType.GetProperty("ParticipantID"), Expression.Property(pe_clinicalTrialsQueryType, pe_clinicalTrialsQueryType.Type.GetProperty("ParticipantID"))),
                    Expression.Bind(joinResultType.GetProperty("TrialID"), Expression.Property(pe_clinicalTrialsQueryType, pe_clinicalTrialsQueryType.Type.GetProperty("TrialID"))),
                    Expression.Bind(joinResultType.GetProperty("TimeWindowAfter"), Expression.Add(Expression.Convert(Expression.Property(pe_proQueryType, pe_proQueryType.Type.GetProperty("ResponseNumber")), typeof(float)), ce_daysAfter)),
                    Expression.Bind(joinResultType.GetProperty("TimeWindowBefore"), Expression.Subtract(Expression.Convert(Expression.Property(pe_proQueryType, pe_proQueryType.Type.GetProperty("ResponseNumber")), typeof(float)), ce_daysBefore))
                };

                var pe_joinType = ParameterExpression.Parameter(joinResultType, "j");

                var resultSelector = Expression.Lambda(
                        Expression.MemberInit(Expression.New(joinResultType), joinBindings),
                        pe_proQueryType,
                        pe_clinicalTrialsQueryType
                    );

                MethodCallExpression joinCall = Expression.Call(
                    typeof(Queryable),
                    "Join",
                    new Type[] {
                    typeof(ReportedOutcome), //outer
                    typeof(ClinicalTrial), //inner
                    typeof(string), //key
                    joinResultType
                    },
                    new Expression[] {
                        db.ReportedOutcomeCommonMeasures.AsQueryable().Expression,
                        db.ClinicalTrials.AsQueryable().Expression,
                        Expression.Quote(outerSelector),
                        Expression.Quote(innerSelector),
                        Expression.Quote(resultSelector)
                    }
                    );

                BinaryExpression itemNameExprs = BinaryExpression.Equal(Expression.Property(pe_joinType, "ItemName"), ce_proItemName);
                BinaryExpression trialIDExprs = Expression.Equal(Expression.Property(pe_joinType, "TrialID"), ce_trialID);
                BinaryExpression requireResponseNumExprs = Expression.NotEqual(Expression.Property(pe_joinType, "ResponseNumber"), Expression.Constant(null));

                BinaryExpression predicate = Expression.AndAlso(Expression.AndAlso(itemNameExprs, trialIDExprs), requireResponseNumExprs);

                var whereCall = Expression.Call(typeof(Queryable), "Where", new[] { joinResultType }, joinCall, Expression.Quote(Expression.Lambda(predicate, pe_joinType)));

                var query = db.ReportedOutcomeCommonMeasures.AsQueryable().Provider.CreateQuery(whereCall);
                Logger.Debug(query.Expression);

                Logger.Debug(query.ToTraceQuery());

            }


            //left outer join on procedures where procedure ID is null (omit procedures), use group join
            //https://docs.microsoft.com/en-us/dotnet/csharp/linq/perform-left-outer-joins
        }

        [TestMethod]
        public void GetEncountersWithAdmitDate_LINQ()
        {
            using (var db = Helper.CreatePCORIDataContext(ConnectionString))
            {
                ParameterExpression pe_encounters = Expression.Parameter(typeof(Encounter), "enc");
                ParameterExpression pe_diagnosis = Expression.Parameter(typeof(Diagnosis), "dia");
                ParameterExpression pe_procedures = Expression.Parameter(typeof(Procedure), "proc");

                var encounters_selector = Expression.Lambda(Expression.Property(pe_encounters, "ID"), pe_encounters);
                var diagnosis_selector = Expression.Lambda(Expression.Property(pe_diagnosis, "EncounterID"), pe_diagnosis);
                var procedures_selector = Expression.Lambda(Expression.Property(pe_procedures, "EncounterID"), pe_procedures);

                Type joinResultType = Objects.Dynamic.TypeBuilderHelper.CreateType("EncountersByJoin", new[]{
                    new BasicPropertyDefinition{ Name = "EncounterID", Type = "System.String" },
                    new BasicPropertyDefinition{ Name = "PatientID", Type = "System.String" },
                    new BasicPropertyDefinition{ Name = "AdmitDate", Type = "System.DateTime" },
                    new BasicPropertyDefinition{ Name = "EncounterSASDate", Type = typeof(int?).ToString() },
                    new BasicPropertyDefinition{ Name = "EncounterType", Type = "System.String" },
                    new BasicPropertyDefinition{ Name = "DiagnosisCode", Type = "System.String" },
                    new BasicPropertyDefinition{ Name = "DiagnosisCodeType", Type = "System.String" },
                    new BasicPropertyDefinition{ Name = "ProcedureCode", Type = "System.String" },
                    new BasicPropertyDefinition{ Name = "ProcedureCodeType", Type = "System.String" },
                    new BasicPropertyDefinition{ Name = "ProcedureDate", Type = typeof(DateTime?).ToString() }
                });

                DateTime SASstartDate = new DateTime(1960, 1, 1);
                MethodCallExpression sasDateCalc = Expression.Call(
                    typeof(System.Data.Entity.DbFunctions),
                    "DiffDays",
                    null,
                    Expression.Constant(SASstartDate, typeof(DateTime?)),
                    Expression.Convert(Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("AdmittedOn")), typeof(DateTime?))
                    );

                var diagnosisBindings = new[] {
                    Expression.Bind(joinResultType.GetProperty("EncounterID"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("ID"))),
                    Expression.Bind(joinResultType.GetProperty("PatientID"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("PatientID"))),
                    Expression.Bind(joinResultType.GetProperty("AdmitDate"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("AdmittedOn"))),
                    Expression.Bind(joinResultType.GetProperty("EncounterSASDate"), sasDateCalc),
                    Expression.Bind(joinResultType.GetProperty("EncounterType"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("EncounterType"))),
                    Expression.Bind(joinResultType.GetProperty("DiagnosisCode"), Expression.Property(pe_diagnosis, pe_diagnosis.Type.GetProperty("Code"))),                    
                    Expression.Bind(joinResultType.GetProperty("DiagnosisCodeType"), Expression.Property(pe_diagnosis, pe_diagnosis.Type.GetProperty("CodeType"))),
                    Expression.Bind(joinResultType.GetProperty("ProcedureCode"), Expression.Convert(Expression.Constant(null), typeof(string))),
                    Expression.Bind(joinResultType.GetProperty("ProcedureCodeType"), Expression.Convert(Expression.Constant(null), typeof(string))),
                    Expression.Bind(joinResultType.GetProperty("ProcedureDate"), Expression.Convert(Expression.Constant(null), typeof(DateTime?)))
                };

                

                var pe_joinType = ParameterExpression.Parameter(joinResultType, "j");

                var diagnosisResultSelector = Expression.Lambda(
                        Expression.MemberInit(Expression.New(joinResultType), diagnosisBindings),
                        pe_encounters,
                        pe_diagnosis
                    );

                //get all the encounters associated to diagnosis entries
                MethodCallExpression diagnosisJoinCall = Expression.Call(
                    typeof(Queryable),
                    "Join",
                    new Type[] {
                    typeof(Encounter),
                    typeof(Diagnosis),
                    typeof(string),
                    joinResultType
                    },
                    new Expression[] {
                        db.Encounters.AsQueryable().Expression,
                        db.Diagnoses.AsQueryable().Expression,
                        Expression.Quote(Expression.Lambda(Expression.Property(pe_encounters, "ID"), pe_encounters)),
                        Expression.Quote(Expression.Lambda(Expression.Property(pe_diagnosis, "EncounterID"), pe_diagnosis)),
                        Expression.Quote(diagnosisResultSelector)
                    }
                    );


                //var query = db.Encounters.AsQueryable().Provider.CreateQuery(joinCall);
                //Logger.Debug(query.Expression);

                //Logger.Debug(query.ToTraceQuery());

                var procedureBindings = new[] {
                    Expression.Bind(joinResultType.GetProperty("EncounterID"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("ID"))),
                    Expression.Bind(joinResultType.GetProperty("PatientID"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("PatientID"))),
                    Expression.Bind(joinResultType.GetProperty("AdmitDate"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("AdmittedOn"))),
                    Expression.Bind(joinResultType.GetProperty("EncounterSASDate"), sasDateCalc),
                    Expression.Bind(joinResultType.GetProperty("EncounterType"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("EncounterType"))),
                    Expression.Bind(joinResultType.GetProperty("DiagnosisCode"), Expression.Convert(Expression.Constant(null), typeof(string))),
                    Expression.Bind(joinResultType.GetProperty("DiagnosisCodeType"), Expression.Convert(Expression.Constant(null), typeof(string))),
                    Expression.Bind(joinResultType.GetProperty("ProcedureCode"), Expression.Property(pe_procedures, pe_procedures.Type.GetProperty("Code"))),
                    Expression.Bind(joinResultType.GetProperty("ProcedureCodeType"), Expression.Property(pe_procedures, pe_procedures.Type.GetProperty("CodeType"))),
                    Expression.Bind(joinResultType.GetProperty("ProcedureDate"), Expression.Property(pe_procedures, pe_procedures.Type.GetProperty("ProcedureDate")))
                };

                var procedureResultSelector = Expression.Lambda(
                        Expression.MemberInit(Expression.New(joinResultType), procedureBindings),
                        pe_encounters,
                        pe_procedures                        
                    );

                //get all encounters associated to procedure entries
                MethodCallExpression proceduresJoinCall = Expression.Call(
                    typeof(Queryable),
                    "Join",
                    new Type[] {
                        typeof(Encounter),
                        typeof(Procedure),
                        typeof(string),
                        joinResultType
                    },
                    new Expression[] {
                        db.Encounters.AsQueryable().Expression,
                        db.Procedures.AsQueryable().Expression,
                        Expression.Quote(Expression.Lambda(Expression.Property(pe_encounters, "ID"), pe_encounters)),
                        Expression.Quote(Expression.Lambda(Expression.Property(pe_procedures, "EncounterID"), pe_procedures)),
                        procedureResultSelector
                    }
                    );


                var isolatedEncountersBindings = new[] {
                    Expression.Bind(joinResultType.GetProperty("EncounterID"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("ID"))),
                    Expression.Bind(joinResultType.GetProperty("PatientID"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("PatientID"))),
                    Expression.Bind(joinResultType.GetProperty("AdmitDate"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("AdmittedOn"))),
                    //Expression.Bind(joinResultType.GetProperty("EncounterSASDate"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("ResponseText"))),
                    Expression.Bind(joinResultType.GetProperty("EncounterSASDate"), sasDateCalc),
                    Expression.Bind(joinResultType.GetProperty("EncounterType"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("EncounterType"))),
                    Expression.Bind(joinResultType.GetProperty("DiagnosisCode"), Expression.Convert(Expression.Constant(null), typeof(string))),
                    Expression.Bind(joinResultType.GetProperty("DiagnosisCodeType"), Expression.Convert(Expression.Constant(null), typeof(string))),
                    Expression.Bind(joinResultType.GetProperty("ProcedureCode"), Expression.Convert(Expression.Constant(null), typeof(string))),
                    Expression.Bind(joinResultType.GetProperty("ProcedureCodeType"), Expression.Convert(Expression.Constant(null), typeof(string))),
                    Expression.Bind(joinResultType.GetProperty("ProcedureDate"), Expression.Convert(Expression.Constant(null), typeof(DateTime?)))
                };

                var isolatedEncountersResultSelector = Expression.Lambda(
                        Expression.MemberInit(Expression.New(joinResultType), isolatedEncountersBindings),
                        pe_encounters
                    );

                var isolatedEncountersQuery = from enc in db.Encounters
                                              where !db.Diagnoses.Any(dia => dia.EncounterID == enc.ID)
                                              && !db.Procedures.Any(dia => dia.EncounterID == enc.ID)
                                              select enc;

                MethodCallExpression encountersSelectCall = Expression.Call(
                    typeof(Queryable),
                    "Select",
                    new Type[] { isolatedEncountersQuery.ElementType, joinResultType },
                    isolatedEncountersQuery.Expression,
                    Expression.Quote(isolatedEncountersResultSelector)
                    );


                //union everything together
                MethodCallExpression unionDiagnosisProceduresCall = Expression.Call(
                    typeof(Queryable),
                    "Union",
                    new Type[] {
                        joinResultType
                    },
                    diagnosisJoinCall,
                    proceduresJoinCall
                    );

                MethodCallExpression unionDPandEncountersCall = Expression.Call(
                    typeof(Queryable),
                    "Union",
                    new Type[] { joinResultType },
                    unionDiagnosisProceduresCall,
                    encountersSelectCall
                    );


                var query = db.Encounters.AsQueryable().Provider.CreateQuery(unionDPandEncountersCall);
                Logger.Debug(query.Expression);

                Logger.Debug(query.ToTraceQuery());                

            }

        }

        [TestMethod]
        public void TimeWindowQueryBuilder()
        {
            using(var db = Helper.CreatePCORIDataContext(ConnectionString))
            {
                var trials = new[] { "FAKE_TRIAL-15" };
                var query = from d in db.Patients
                           join tr in db.ClinicalTrials on d.ID equals tr.PatientID
                           where trials.Contains(tr.TrialID)
                           select new
                           {
                               PatientID = d.ID,
                               tr.ParticipantID,
                               tr.TrialID
                           };

                //var query = db.Patients.Join(db.ClinicalTrials, p => p.ID, tr => tr.PatientID, )

                var indexEventCriteria = new[] {
                    new DTO.QueryComposer.QueryComposerTemporalEventDTO{
                        IndexEventDateIdentifier = "HOSPITALIZATION_DATE",
                        DaysBefore = 2,
                        DaysAfter = 13
                    }
                };

                var builder = new Adapters.PCORI.TimeWindowQueryBuilder(db, indexEventCriteria, Enumerable.Empty<DTO.QueryComposer.QueryComposerCriteriaDTO>());
                var expr = builder.Generate(query.Expression, query.Expression.Type.GetGenericArguments()[0]);

                Logger.Debug(expr.ToString());
            }
        }

        [TestMethod]
        public void ReportedOutcomesAndEncounters()
        {
            using(var _db = Helper.CreatePCORIDataContext(ConnectionString))
            {
                var q = _db.ReportedOutcomeCommonMeasures.GroupJoin(_db.Encounters,
                    k => k.PatientID,
                    k => k.PatientID,
                    (pro, encounters) => new { pro, encounters }
                    )
                .SelectMany(
                    i => i.encounters.DefaultIfEmpty(),
                    (i, enc) => new
                    {
                        IndexEvent = new { ItemName = i.pro.ItemName, PatientID = i.pro.PatientID, PRO_ID = i.pro.ID, ResponseNumber = i.pro.ResponseNumber, ResponseSequence = i.pro.MeasureSequence, ResponseText = i.pro.ResponseText },
                        Encounter = new { EncounterID = enc.ID, PatientID = enc.PatientID, AdmitDate = enc.AdmittedOn, EncounterSASDate = System.Data.Entity.DbFunctions.DiffDays(new DateTime(1960, 1, 1), enc.AdmittedOn) }
                    }
                );


                Logger.Debug(q.Expression.ToString());

                Logger.Debug(q.ToTraceQuery());

            }
        }




        [TestMethod]
        public void CreateAnonymousType()
        {
            var anonymousType = GetAnonType<ReportedOutcome, ClinicalTrial>();

            Logger.Debug(anonymousType.ToString());
        }


        static Type GetAnonType<T, V>()
        {
            return new { P1 = default(T), P2 = default(V) }.GetType();
        }

        static Type GetAnonType<T, V>(string propertyOne, string propertyTwo)
        {
            return new { propertyOne = default(T), propertyTwo = default(V) }.GetType();
        }


        private class BasicPropertyDefinition : IPropertyDefinition
        {
            string _as = string.Empty;

            public string Name { get; set; }

            public string Type { get; set; }

            public string As
            {
                get
                {
                    if (string.IsNullOrEmpty(_as))
                    {
                        return Name;
                    }

                    return _as;
                }
                set
                {
                    _as = value;
                }
            }

            public string Aggregate { get; set; }

            public Type AsType()
            {
                return System.Type.GetType(this.Type);
            }
        }

        class TypedPropertyDefinition<T> : IPropertyDefinition
        {
            public string Name { get; set; }

            string _as = string.Empty;
            public string As
            {
                get
                {
                    if (string.IsNullOrEmpty(_as))
                    {
                        return Name;
                    }

                    return _as;
                }
                set
                {
                    _as = value;
                }
            }

            public string Aggregate { get; set; }

            public string Type
            {
                get
                {
                    return typeof(T).FullName;
                }
                set { }
            }

            public Type AsType()
            {
                return typeof(T);
            }
        }


    }
}
