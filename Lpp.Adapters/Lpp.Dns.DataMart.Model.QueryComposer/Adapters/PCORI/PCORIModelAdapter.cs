using LinqKit;
using log4net;
using Lpp.Dns.DataMart.Model.PCORIQueryBuilder;
using Lpp.Dns.DTO.QueryComposer;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using pcori = Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model;



namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI
{
    public class PCORIModelAdapter : DynamicModelAdapter<pcori.Patient>
    {
        static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataContext db = null;
        Model.Settings.SQLProvider _sqlProvider = Settings.SQLProvider.SQLServer;
        QueryComposerResponseDTO _currentResponse = null;

        public PCORIModelAdapter(RequestMetadata requestMetadata)
            : base(new Guid("85EE982E-F017-4BC4-9ACD-EE6EE55D2446"), requestMetadata)
        {
            ParagraphPredicateBuilders.Add(ApplyVitalsTerms);
            ParagraphPredicateBuilders.Add(ApplyCombinedDiagnosisAndProcedureCodeTerms);
            ParagraphPredicateBuilders.Add(ApplySexTerms);
            ParagraphPredicateBuilders.Add(ApplyVisitsTerms);
            ParagraphPredicateBuilders.Add(ApplyAgeRangeTerms);
            ParagraphPredicateBuilders.Add(ApplyRaceTerms);
            ParagraphPredicateBuilders.Add(ApplyHispanicTerms);
            ParagraphPredicateBuilders.Add(ApplyClinicalTrialTerms);
            ParagraphPredicateBuilders.Add(ApplyPatientReportedOutcomeTerms);
        }

        public override bool CanGeneratePatientIdentifierLists => true;

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~PCORIModelAdapter()
        {
            Dispose(false);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (db != null)
            {
                db.Dispose();
                db = null;
            }
        }

        public override void Initialize(IDictionary<string, object> settings, string requestId)
        {
            base.Initialize(settings, requestId);

            _sqlProvider = (Model.Settings.SQLProvider)Enum.Parse(typeof(Model.Settings.SQLProvider), Lpp.Dns.DataMart.Model.Settings.ProcessorSettings.GetAsString(settings, "DataProvider", Model.Settings.SQLProvider.SQLServer.ToString()));
            string defaultSchema = Lpp.Dns.DataMart.Model.Settings.ProcessorSettings.GetAsString(settings, "DatabaseSchema", string.Empty);
            if (string.IsNullOrEmpty(defaultSchema))
            {
                if (_sqlProvider == Model.Settings.SQLProvider.PostgreSQL)
                {
                    defaultSchema = "dbo";
                }
                if (_sqlProvider == Model.Settings.SQLProvider.MySQL)
                {
                    defaultSchema = "dbo";
                }
                if (_sqlProvider == Model.Settings.SQLProvider.Oracle)
                {
                    defaultSchema = "C##PCORNETUSER";
                }
            }

            db = new DataContext(Utilities.OpenConnection(settings, logger, false), defaultSchema);
            db.Database.CommandTimeout = Utilities.GetCommandTimeout(settings);

            db.Database.Log = (sql) =>
            {
                if (!string.IsNullOrWhiteSpace(sql))
                    logger.Debug(sql);
            };
        }

        protected override string[] LowThresholdColumns(DTO.QueryComposer.QueryComposerResponseDTO response)
        {
            return new string[] { "Patients" };
        }

        QueryComposerRequestInterrogator _queryInterrogator = null;

        public override DTO.QueryComposer.QueryComposerResponseDTO Execute(DTO.QueryComposer.QueryComposerRequestDTO request, bool viewSQL)
        {
            _queryInterrogator = new QueryComposerRequestInterrogator(request);

            if (_queryInterrogator.IsSQLDistribution)
            {
                return ExecuteSqlDistribution(request, viewSQL);
            }

            IQueryable<pcori.Patient> rootQuery = db.Patients;            
            if (_queryInterrogator.HasCriteria)
            {
                System.Linq.Expressions.Expression<Func<pcori.Patient, bool>> queryPredicate = ApplyCriteria(request.Where.Criteria.Where(c => c.Type == DTO.Enums.QueryComposerCriteriaTypes.Paragraph));

                //TODO: if age is part of the query need to modify the result type to include the calculated age
                rootQuery = rootQuery.Where(queryPredicate);
            }

            /**
             * Order of operations
             * 1) build the root query with the critieria
             * 2) build the overall select containing any fields that will grouped on, selected, or aggregated on, or computed, or stratified on
             *  - if aggregate append _{aggregate} to the field name
             *  - if stratification value, use the field name - ie replace original value with stratify value
             *  - if computed use the specified field name
             *  
             *  - can determine if stratification value or aggregate based on fielddto
             *  ! need to be able to determine if computed value
             *  ! need to be able to build the stratification value based on the type of stratification and term
             *  ! stratifications and computed expressions will need to exist before being able to bind to the select class/call
             *  
             * * If there are no aggregates or grouping this is the final select * *
             * 
             * 3) build the grouping with the computed key
             *      - will contain properties that are not part of any aggregate
             *      
             * 4) build the select including all the key properies and any aggregates
             *      - build the final select type
             *      - bind group key properties
             *      - bind aggregates to appropriate initial select fields
             * 
             * NOTE: if any count aggregate is included, regardless of field it will always be count of the patient.
             * 
             */

            Dictionary<Guid, TermImplementation> termImplementations = new Dictionary<Guid, TermImplementation>();
            termImplementations.Add(ModelTermsFactory.SexID, new Terms.Sex(db));
            termImplementations.Add(ModelTermsFactory.HispanicID, new Terms.Hispanic(db));
            termImplementations.Add(ModelTermsFactory.RaceID, new Terms.Race(db));
            termImplementations.Add(ModelTermsFactory.AgeRangeID, new Terms.AgeRange(db, _sqlProvider));
            termImplementations.Add(ModelTermsFactory.SettingID, new Terms.Setting(db));
            termImplementations.Add(ModelTermsFactory.ObservationPeriodID, new Terms.EncounterObservationPeriod(db));
            termImplementations.Add(ModelTermsFactory.HeightID, new Terms.Height(db));
            termImplementations.Add(ModelTermsFactory.WeightID, new Terms.Weight(db));
            termImplementations.Add(ModelTermsFactory.VitalsMeasureDateID, new Terms.VitalMeasureDate(db));
            termImplementations.Add(ModelTermsFactory.CombinedDiagnosisCodesID, new Terms.CombinedDiagnosisCodes(db));
            termImplementations.Add(ModelTermsFactory.ProcedureCodesID, new Terms.ProcedureCodes(db));
            termImplementations.Add(ModelTermsFactory.TrialID, new Terms.Trial(db));
            termImplementations.Add(ModelTermsFactory.PatientReportedOutcomeID, new Terms.PatientReportedOutcome(db));



            foreach (var field in request.Select.Fields)
            {
                TermImplementation termImp;
                if (!termImplementations.TryGetValue(field.Type, out termImp))
                {
                    throw new NotSupportedException("The term with type ID of '" + field.Type.ToString("D") + "' is not supported by this adapter.");
                }

                termImp.RegisterQueryComposerField(field);

                //Note: not adding directly to the terms collection now to avoid having todo duplicate check in the terms collection.
                //Simpler to just get the terms that have registered fields when done.
            }

            List<TermImplementation> terms = termImplementations.Values.Where(t => t.HasFields).ToList();

            //register the criteria with each term being used
            var criteria = request.Where.Criteria.ToArray();
            terms.ForEach(t => t.RegisterCriteria(criteria));

            //if there are any stratifications or counts, or no selects specified; remove counts then add PatientID count
            if (terms.Any(t => t.HasCountAggregate || t.HasStratifications) || terms.Count == 0)
            {
                if (terms.Count > 0)
                    terms.ForEach(t => t.RemoveCountAggregates());

                //Add the term implementation for the root patient count which replaces the others
                terms.Add(new Terms.PatientCount(db));
            }

            Guid[] nonRootQueryTerms = new[] { ModelTermsFactory.TrialID, ModelTermsFactory.PatientReportedOutcomeID, ModelTermsFactory.PatientReportedOutcomeEncounterID };

            //note: this parameter may need to get changed to just be based on the type of the root query
            ParameterExpression patientParameterExpr = Expression.Parameter(rootQuery.ElementType, "p");

            //get the properties for the root query: exclude properties that are used for the join types
            IEnumerable<Objects.Dynamic.IPropertyDefinition> selectPropertyDefinitions = terms.Where(t => !nonRootQueryTerms.Contains(t.TermID)).SelectMany(t => t.InnerSelectPropertyDefinitions()).ToArray();

            //build the inner select Type
            Type innerSelectType = Lpp.Objects.Dynamic.TypeBuilderHelper.CreateType("s", selectPropertyDefinitions);

            //get the bindings against the inner select type from the terms.
            IEnumerable<MemberBinding> innerSelectBindings = terms.SelectMany(t => t.InnerSelectBindings(innerSelectType, patientParameterExpr));

            //create the innner select expression call
            LambdaExpression innerSelector = Expression.Lambda(Expression.MemberInit(Expression.New(innerSelectType), innerSelectBindings), patientParameterExpr);
            MethodCallExpression innerSelectCall = Expression.Call(typeof(Queryable), "Select", new Type[] { rootQuery.ElementType, innerSelectType }, rootQuery.Expression, Expression.Quote(innerSelector));


            //the current Type that will be returned by the query
            Type currentSelectType = innerSelectType;
            //the current expression to execute the query
            Expression currentSelectCall = innerSelectCall;


            var trialIDTermImp = terms.FirstOrDefault(t => t.TermID == ModelTermsFactory.TrialID);
            if(trialIDTermImp != null){
                IQueryable<pcori.ClinicalTrial> trialQuery = db.ClinicalTrials;
                var trialPredicate = ApplyClinicalTrialCriteria(request.Where.Criteria.Where(c => c.Type == DTO.Enums.QueryComposerCriteriaTypes.Paragraph));
                if(trialPredicate != null)
                {
                    trialQuery = trialQuery.Where(trialPredicate);
                }

                ParameterExpression trialParamExpr = Expression.Parameter(trialQuery.ElementType, "tr");
                var trialSelectTypePropertyDefinitions = trialIDTermImp.InnerSelectPropertyDefinitions().ToArray();
                Type trialSelectType = Objects.Dynamic.TypeBuilderHelper.CreateType("trials", trialSelectTypePropertyDefinitions);

                IEnumerable<MemberBinding> trialSelectBindings = trialIDTermImp.InnerSelectBindings(trialSelectType, trialParamExpr);
                LambdaExpression trialSelector = Expression.Lambda(Expression.MemberInit(Expression.New(trialSelectType), trialSelectBindings), trialParamExpr);
                MethodCallExpression trialSelectCall = Expression.Call(typeof(Queryable), "Select", new Type[] { trialQuery.ElementType, trialSelectType }, trialQuery.Expression, Expression.Quote(trialSelector));

                //define the selector for the outer
                ParameterExpression rootInnerParameterExpr = Expression.Parameter(currentSelectType, "k1");                
                Expression outerKeySelector = Expression.Lambda(Expression.Property(rootInnerParameterExpr, currentSelectType, "PatientID"), rootInnerParameterExpr);

                //define the selector for the inner
                ParameterExpression trialSelectParamExpr = Expression.Parameter(trialSelectType, "tr1");
                Expression innerKeySelector = Expression.Lambda(Expression.Property(trialSelectParamExpr, trialSelectType, "Trial_PatientID"), trialSelectParamExpr);

                //build the join result type: root query inner select definitions + trial select definitions
                var joinPropertyDefinitions = selectPropertyDefinitions.Concat(trialSelectTypePropertyDefinitions).ToArray();
                Type innerJoinTrialType = Objects.Dynamic.TypeBuilderHelper.CreateType("ClinicalTrial_Join", joinPropertyDefinitions);
                ParameterExpression innerJoinTrialParamExpr = Expression.Parameter(innerJoinTrialType, "ijtt");
                IEnumerable<MemberBinding> innerJoinBindings = terms.Where(t => !nonRootQueryTerms.Contains(t.TermID)).SelectMany(t => t.InnerSelectBindings(innerJoinTrialType, rootInnerParameterExpr)).Concat(trialIDTermImp.InnerSelectBindings(innerJoinTrialType, trialSelectParamExpr));
                LambdaExpression resultSelector = Expression.Lambda(Expression.MemberInit(Expression.New(innerJoinTrialType), innerJoinBindings), rootInnerParameterExpr, trialSelectParamExpr);


                MethodCallExpression joinCall = Expression.Call(
                    typeof(Queryable), 
                    "Join",
                    new Type[]
                    {
                        currentSelectType, //TOuter
                        trialSelectType, //TInner
                        typeof(string), //TKey
                        innerJoinTrialType //TResult
                    },
                    new Expression[] 
                    {
                        innerSelectCall,
                        trialSelectCall,
                        Expression.Quote(outerKeySelector),
                        Expression.Quote(innerKeySelector),
                        Expression.Quote(resultSelector)
                    }
                    );

                currentSelectType = innerJoinTrialType;
                currentSelectCall = joinCall;
            }

            //IQueryable tempQuery = rootQuery.Provider.CreateQuery(currentSelectCall);
            //var tempResult = tempQuery.ToTraceQuery();

            var patientReportedOutcomeTermImpl = terms.FirstOrDefault(t => t.TermID == ModelTermsFactory.PatientReportedOutcomeID);
            if(patientReportedOutcomeTermImpl != null)
            {
                IQueryable<pcori.ReportedOutcome> proQuery = db.ReportedOutcomeCommonMeasures;
                Expression<Func<pcori.ReportedOutcome, bool>> proPredicate = ApplyPatientReportedOutcomeCriteria(request.Where.Criteria.Where(c => c.Type == DTO.Enums.QueryComposerCriteriaTypes.Paragraph));
                if(proPredicate != null)
                {
                    proQuery = proQuery.Where(proPredicate);
                }

                ParameterExpression proParamExpr = Expression.Parameter(proQuery.ElementType, "pro");
                var proSelectTypePropertyDefinitions = patientReportedOutcomeTermImpl.InnerSelectPropertyDefinitions().ToArray();
                Type proSelectType = Objects.Dynamic.TypeBuilderHelper.CreateType("patientReportedOutcomes", proSelectTypePropertyDefinitions);

                IEnumerable<MemberBinding> proSelectBindings = patientReportedOutcomeTermImpl.InnerSelectBindings(proSelectType, proParamExpr);
                LambdaExpression proSelector = Expression.Lambda(Expression.MemberInit(Expression.New(proSelectType), proSelectBindings), proParamExpr);
                MethodCallExpression proSelectCall = Expression.Call(typeof(Queryable), "Select", new Type[] { proQuery.ElementType, proSelectType }, proQuery.Expression, Expression.Quote(proSelector));

                //define the selector for the outer
                ParameterExpression rootInnerParameterExpr = Expression.Parameter(currentSelectType, "k2");
                Expression outerKeySelector = Expression.Lambda(Expression.Property(rootInnerParameterExpr, currentSelectType, "PatientID"), rootInnerParameterExpr);

                //define the selector for the inner
                ParameterExpression proSelectParamExpr = Expression.Parameter(proSelectType, "pro1");
                Expression innerKeySelector = Expression.Lambda(Expression.Property(proSelectParamExpr, proSelectType, "PRO_PatientID"), proSelectParamExpr);

                //build the join result type: root query inner select definitions + trial select definitions
                var joinPropertyDefinitions = selectPropertyDefinitions.Concat(proSelectTypePropertyDefinitions).ToArray();
                Type innerJoinPROType = Objects.Dynamic.TypeBuilderHelper.CreateType("PRO_CM_Join", joinPropertyDefinitions);
                ParameterExpression innerJoinTrialParamExpr = Expression.Parameter(innerJoinPROType, "pro_cmj");
                IEnumerable<MemberBinding> innerJoinBindings = terms.Where(t => !nonRootQueryTerms.Contains(t.TermID)).SelectMany(t => t.InnerSelectBindings(innerJoinPROType, rootInnerParameterExpr)).Concat(patientReportedOutcomeTermImpl.InnerSelectBindings(innerJoinPROType, proSelectParamExpr));
                LambdaExpression resultSelector = Expression.Lambda(Expression.MemberInit(Expression.New(innerJoinPROType), innerJoinBindings), rootInnerParameterExpr, proSelectParamExpr);


                MethodCallExpression joinCall = Expression.Call(
                    typeof(Queryable),
                    "Join",
                    new Type[]
                    {
                        currentSelectType, //TOuter
                        proSelectType, //TInner
                        typeof(string), //TKey
                        innerJoinPROType //TResult
                    },
                    new Expression[]
                    {
                        innerSelectCall,
                        proSelectCall,
                        Expression.Quote(outerKeySelector),
                        Expression.Quote(innerKeySelector),
                        Expression.Quote(resultSelector)
                    }
                    );

                currentSelectType = innerJoinPROType;
                currentSelectCall = joinCall;

            }

            IQueryable tempQuery = rootQuery.Provider.CreateQuery(currentSelectCall);
            var tempResult = tempQuery.ToTraceQuery();


            IEnumerable<Objects.Dynamic.IPropertyDefinition> groupKeyPropertyDefinitions = new QueryComposerResponsePropertyDefinitionDTO[0];

            if (terms.Any(t => t.HasCountAggregate || t.HasStratifications))
            {
                groupKeyPropertyDefinitions = terms.SelectMany(t => t.GroupKeyPropertyDefinitions()).ToArray();

                //build the grouping key
                Type groupingKeyType = Lpp.Objects.Dynamic.TypeBuilderHelper.CreateType("g", groupKeyPropertyDefinitions);

                //the parameter expression of the select type the key is being generated from
                ParameterExpression groupKeySelectParameterExpression = Expression.Parameter(currentSelectType, "k");

                //get the group key bindings
                IEnumerable<MemberBinding> groupKeySelectBindings = terms.SelectMany(t => t.GroupKeySelectBindings(groupingKeyType, groupKeySelectParameterExpression));

                //call the grouping on the key
                LambdaExpression groupKeySelector = Expression.Lambda(Expression.MemberInit(Expression.New(groupingKeyType), groupKeySelectBindings), groupKeySelectParameterExpression);
                MethodCallExpression groupCall = Expression.Call(typeof(Queryable), "GroupBy", new Type[] { currentSelectType, groupKeySelector.Body.Type }, currentSelectCall, Expression.Quote(groupKeySelector));

                selectPropertyDefinitions = terms.SelectMany(t => t.FinalSelectPropertyDefinitions());
                //build the final select type
                currentSelectType = Lpp.Objects.Dynamic.TypeBuilderHelper.CreateType("ss", selectPropertyDefinitions);

                //get the final select type bindings
                Type groupingType = Lpp.Objects.Dynamic.Expressions.GetGenericType(typeof(IQueryable<>), groupCall.Type).GetGenericArguments().First();
                //TODO: we know we want an IGrouping<innerSelectType>, can we just go there directly and avoid going into the groupCal query type?

                ParameterExpression groupKeyParameterExpr = Expression.Parameter(groupingType, "k");
                IEnumerable<MemberBinding> finalSelectBindings = terms.SelectMany(t => t.FinalSelectBindings(currentSelectType, groupKeyParameterExpr));

                //call the final select
                LambdaExpression finalSelector = Expression.Lambda(Expression.MemberInit(Expression.New(currentSelectType), finalSelectBindings), groupKeyParameterExpr);
                MethodCallExpression finalSelectCall = Expression.Call(typeof(Queryable), "Select", new Type[] { groupingType, currentSelectType }, groupCall, Expression.Quote(finalSelector));

                currentSelectCall = finalSelectCall;
            }

            logger.Debug("Final expression: " + currentSelectCall.ToString());

            IQueryable query = rootQuery.Provider.CreateQuery(currentSelectCall);

            IEnumerable<ITermResultTransformer> resultTransformers = terms.Where(t => (t as ITermResultTransformer) != null).Cast<ITermResultTransformer>().ToArray();

            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();
            if (!viewSQL)
            {
                try
                {
                    foreach (var item in query)
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();
                        Type itemType = item.GetType();
                        foreach (var propInfo in itemType.GetProperties())
                        {
                            object value = propInfo.GetValue(item, null);
                            row.Add(propInfo.Name, value);
                        }

                        foreach (var transformer in resultTransformers)
                        {
                            row = transformer.Visit(row);
                        }

                        if (_lowThresholdValue.HasValue && row.ContainsKey("Patients"))
                        {
                            double value = Convert.ToDouble(row["Patients"]);
                            if (value > 0 && value < _lowThresholdValue)
                            {
                                //need to mark that the value is less than the low threshold - not zero'd until post process triggered
                                if (!row.ContainsKey(LowThresholdColumnName))
                                {
                                    row.Add(LowThresholdColumnName, true);
                                }
                                else
                                {
                                    row[LowThresholdColumnName] = true;
                                }
                            }
                        }

                        results.Add(row);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                    throw;
                }
            }
            else
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("SQL", query.ToString());
                results.Add(row);
            }

            logger.Debug("Number of results found:" + results.Count);

            //update the final select and group key property definitions for any of the terms that had result transforms
            selectPropertyDefinitions = VisitPropertyDefinitionsForTransforms(resultTransformers, selectPropertyDefinitions);
            if (_lowThresholdValue.HasValue)
            {
                selectPropertyDefinitions = selectPropertyDefinitions.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
            }

            groupKeyPropertyDefinitions = VisitPropertyDefinitionsForTransforms(resultTransformers, groupKeyPropertyDefinitions);

            QueryComposerResponseDTO response = new QueryComposerResponseDTO
            {
                ResponseDateTime = DateTime.UtcNow,
                Results = new[] { results },
                Properties = selectPropertyDefinitions.Cast<QueryComposerResponsePropertyDefinitionDTO>(),
                Aggregation = new QueryComposerResponseAggregationDefinitionDTO
                {
                    GroupBy = groupKeyPropertyDefinitions.Select(k => k.Name).ToArray(),
                    Select = selectPropertyDefinitions
                }
            };

            if (request.ID.HasValue)
                response.RequestID = request.ID.Value;

            _currentResponse = response;

            return response;
        }

        public override QueryComposerModelProcessor.DocumentEx[] OutputDocuments()
        {
            if (_currentResponse == null)
                return new QueryComposerModelProcessor.DocumentEx[0];

            return new[] { SerializeResponse(_currentResponse, QueryComposerModelProcessor.NewGuid(), "response.json") };
        }

        public override void PostProcess(QueryComposerResponseDTO response)
        {
            base.PostProcess(response);

            _currentResponse = response;
        }

        public override void GeneratePatientIdentifierLists(DTO.QueryComposer.QueryComposerRequestDTO request, IDictionary<Guid, string> outputSettings, string format)
        {
            //foreach query in the request, if the output paths collection contains a path, execute the query resulting in distinct Patient Identifiers. Save the result to disk.
            //build criteria same as usual, remove all stratifiers and selects, add new PatientIdentifier term builder
            //continue same as normal executing the request, write the results to the specified output path
            //if no results, empty file
            //if error throw exception

            _queryInterrogator = new QueryComposerRequestInterrogator(request);

            if (_queryInterrogator.IsSQLDistribution)
            {
                throw new InvalidOperationException("SQL Distribution query is not supported by this method. Execute should be called instead.");
            }

            IQueryable<pcori.Patient> rootQuery = db.Patients;
            if (_queryInterrogator.HasCriteria)
            {
                System.Linq.Expressions.Expression<Func<pcori.Patient, bool>> queryPredicate = ApplyCriteria(request.Where.Criteria.Where(c => c.Type == DTO.Enums.QueryComposerCriteriaTypes.Paragraph));
                rootQuery = rootQuery.Where(queryPredicate);
            }

            logger.Debug("Beginning query for Patient Identifier list for Request: " + _settings["MSRequestID"]);
            var patietIdentifiers = rootQuery.Select(p => p.ID).Distinct().ToArray();
            logger.Debug($"Finish query execution for Patient Identifier list for Request: { _settings["MSRequestID"] }. { patietIdentifiers.Length } patient identifiers found.");

            string outputPath = outputSettings.Values.First();
            logger.Info($"Saving { patietIdentifiers.Length } patient identifiers for Request: { _settings["MSRequestID"] } to { outputPath }");

            using(var output = new System.IO.FileStream(outputPath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Write))
            {
                var delimited = string.Join(",", patietIdentifiers);
                byte[] buffer = Encoding.Default.GetBytes(delimited);
                output.Write(buffer, 0, buffer.Length);

                output.Flush();
                output.Close();
            }

        }

        private QueryComposerResponseDTO ExecuteSqlDistribution(QueryComposerRequestDTO request, bool viewSQL)
        {
            QueryComposerResponseDTO response = new QueryComposerResponseDTO
            {
                ResponseDateTime = DateTime.UtcNow,
                Errors = Array.Empty<QueryComposerResponseErrorDTO>()
            };
            
            var allTerms = request.Where.Criteria.SelectMany(c => GetAllCriteriaTerms(c, new Nullable<Guid>())).ToArray();
            if (allTerms.Any(t => t.Type != ModelTermsFactory.SqlDistributionID))
            {
                //error: cannot mix sql distribution with any other term
                throw new NotSupportedException("Only a single Sql Distribution term can be specified per request. The term cannot be mixed with other terms.");
            }

            if (allTerms.Length > 1)
            {
                //limit to a single sql dist request
                throw new NotSupportedException("Only a single Sql Distribution term can be specified per request.");
            }           

            string sql = allTerms[0].GetStringValue("Sql");

            if (viewSQL)
            {
                response.Results = new[] {
                    new [] {
                        new Dictionary<string,object>(){
                            { "SQL", sql }
                        }
                    }
                };

                response.Properties = new[] {
                    new QueryComposerResponsePropertyDefinitionDTO { 
                        Name = "SQL",
                        Type = "System.String"
                    }
                };

                return response;
            }           

            List<DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> columnProperties = new List<QueryComposerResponsePropertyDefinitionDTO>();
            List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();

            using (var conn = Utilities.OpenConnection(_settings, logger, true))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                using (var reader = cmd.ExecuteReader(CommandBehavior.KeyInfo))
                {
                    int noNameIndex = 1;
                    DataTable schemaTable = reader.GetSchemaTable();
                    foreach (DataRow row in schemaTable.Rows)
                    {
                        foreach (DataColumn column in schemaTable.Columns)
                        {
                            if (column.ColumnName == "ColumnName")
                            {
                                string columnName = row[column].ToString();
                                if (string.IsNullOrWhiteSpace(columnName))
                                {
                                    columnName = "NoColumnName" + noNameIndex;
                                    noNameIndex++;
                                }
                                columnProperties.Add(new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = columnName, Type = column.DataType.FullName });
                            }
                        }
                    }

                    while (reader.Read())
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();
                        //have to enumerate over the record using ordinal index since there may not be a column name in the reader
                        for (int i = 0; i < columnProperties.Count; i++)
                        {
                            row.Add(columnProperties[i].Name, reader.GetValue(i));
                        }
                        queryResults.Add(row);
                    }
                    reader.Close();
                }
            }

            if (request.ID.HasValue)
                response.RequestID = request.ID.Value;

            response.ResponseDateTime = DateTime.UtcNow;
            response.Results = new[] { queryResults };
            response.Properties = columnProperties;

            _currentResponse = response;

            return response;
        }

        /// <summary>
        /// Merges two paragraph predicates together. (PMNMAINT-1205)
        /// </summary>
        /// <param name="queryPredicate"></param>
        /// <param name="nextParagraphPredicate"></param>
        /// <param name="conjunction"></param>
        /// <returns></returns>
        protected override Expression<Func<pcori.Patient, bool>> MergeParagraphPredicates(Expression<Func<pcori.Patient, bool>> queryPredicate, Expression<Func<pcori.Patient, bool>> nextParagraphPredicate, DTO.Enums.QueryComposerOperators conjunction, bool isExclusion)
        {
            if (conjunction == DTO.Enums.QueryComposerOperators.And && isExclusion == false)
            {
                //Conjunction = AND
                var internalPredicate = queryPredicate.Expand().And(first => (from pats in db.Patients.AsExpandable()
                                                                              where nextParagraphPredicate.Invoke(pats)
                                                                              select pats.ID).Contains(first.ID));

                //AsExpandable wrapper also lets us write expressions that call other expressions
                //This requires that we call Expand on the final result, else LINQ to Entity will result in a NotSupported Exception.
                return internalPredicate.Expand();
            }
            else if (conjunction == DTO.Enums.QueryComposerOperators.AndNot || (conjunction == DTO.Enums.QueryComposerOperators.And && isExclusion == true))
            {
                //Case: AndNot
                var internalPredicate = queryPredicate.Expand().And(first => (from pats in db.Patients.AsExpandable()
                                                                              where nextParagraphPredicate.Invoke(pats)
                                                                              select pats.ID).Contains(first.ID) == false);

                //AsExpandable wrapper also lets us write expressions that call other expressions
                //This requires that we call Expand on the final result, else LINQ to Entity will result in a NotSupported Exception.
                return internalPredicate.Expand();
            }
            else if (conjunction == DTO.Enums.QueryComposerOperators.Or && isExclusion == false)
            {
                //Case: OR
                //We can simply OR the two predicates and return the resulting predicate.
                return queryPredicate.Expand().Or(nextParagraphPredicate.Expand());
            }
            else
            {
                //Or Not.
                return queryPredicate.Expand().Or(PredicateHelper.Negate(nextParagraphPredicate).Expand());
            }
        }

        IEnumerable<Objects.Dynamic.IPropertyDefinition> VisitPropertyDefinitionsForTransforms(IEnumerable<ITermResultTransformer> resultTransformers, IEnumerable<Objects.Dynamic.IPropertyDefinition> propertyDefinitions)
        {
            List<Objects.Dynamic.IPropertyDefinition> definitions = propertyDefinitions.ToList();
            foreach (var transformer in resultTransformers)
            {
                transformer.TransformPropertyDefinitions(definitions);
            }
            return definitions;
        }

        public static IEnumerable<QueryComposerTermDTO> GetAllCriteriaTerms(QueryComposerCriteriaDTO paragraph, Guid? termTypeID)
        {
            if(termTypeID.HasValue)
                return paragraph.Terms.Where(t => t.Type == termTypeID.Value).Concat(paragraph.Criteria.SelectMany(c => c.Terms.Where(t => t.Type == termTypeID.Value)));

            return paragraph.Terms.Concat(paragraph.Criteria.SelectMany(c => c.Terms));
        }

        public static IEnumerable<QueryComposerTermDTO> GetAllCriteriaTerms(QueryComposerCriteriaDTO paragraph, IEnumerable<Guid> termTypeIDs)
        {
            return paragraph.Terms.Where(t => termTypeIDs.Contains(t.Type)).Concat(paragraph.Criteria.SelectMany(c => c.Terms.Where(t => termTypeIDs.Contains(t.Type))));
        }

        Expression<Func<pcori.Patient, bool>> ApplyAgeRangeTerms(QueryComposerCriteriaDTO paragraph, Expression<Func<pcori.Patient, bool>> patientPredicate)
        {
            var terms = GetAllCriteriaTerms(paragraph, ModelTermsFactory.AgeRangeID).ToArray();
            if (!terms.Any())
            {
                return patientPredicate;
            }

            IEnumerable<DTO.Enums.AgeRangeCalculationType> calculationTypes = new[] { 
                DTO.Enums.AgeRangeCalculationType.AsOfSpecifiedDate,
                DTO.Enums.AgeRangeCalculationType.AsOfDateOfRequestSubmission,
                DTO.Enums.AgeRangeCalculationType.AtLastEncounterWithinHealthSystem
            };

            IEnumerable<AgeRangeValues> ageRanges = AdapterHelpers.ParseAgeRangeValues(terms, calculationTypes).ToArray();
            if (!ageRanges.Any())
            {
                return patientPredicate;
            }

            if (ageRanges.All(r => r.MaxAge == null && r.MinAge == null))
            {
                if (ageRanges.Any(r => r.CalculationType == DTO.Enums.AgeRangeCalculationType.AtLastEncounterWithinHealthSystem))
                {
                    //make sure the patient has a birthdate and at least one encounter in the system (PMNDEV-5248)
                    return patientPredicate.And(p => p.BornOn.HasValue && p.Encounters.Any());
                }
                else
                {
                    //make sure the patient has a birthdate
                    return patientPredicate.And(p => p.BornOn.HasValue);
                }
            }

            //each range represent a single term that should be OR'd together into a group that is AND'd against the rest
            Expression<Func<pcori.Patient, bool>> ageGroupingPredicate = (p) => false;

            foreach (AgeRangeValues range in ageRanges)
            {
                AgeRangeValues ageRangeValues = range;

                if (ageRangeValues.MinAge == null && ageRangeValues.MaxAge == null)
                    continue;

                if (ageRangeValues.CalculationType.Value == DTO.Enums.AgeRangeCalculationType.AtLastEncounterWithinHealthSystem)
                {
                    Expression<Func<pcori.Patient, bool>> px = p => p.Encounters.Any();
                    int minAge, maxAge;
                    if (ageRangeValues.MinAge.HasValue && ageRangeValues.MaxAge.HasValue)
                    {
                        minAge = ageRangeValues.MinAge.Value;
                        maxAge = ageRangeValues.MaxAge.Value;
                        if (_sqlProvider != Settings.SQLProvider.Oracle)
                        {
                            px = px.And(p => p.Encounters.Where(enc => enc == p.Encounters.OrderByDescending(x => x.AdmittedOn).FirstOrDefault())
                                .Select(e => ((p.BornOn.Value > e.AdmittedOn) ?
                                                (DbFunctions.DiffYears(p.BornOn.Value, e.AdmittedOn).Value + ((p.BornOn.Value.Month < e.AdmittedOn.Month || (p.BornOn.Value.Month == e.AdmittedOn.Month && p.BornOn.Value.Day < e.AdmittedOn.Day)) ? 1 : 0))
                                                :
                                                (DbFunctions.DiffYears(p.BornOn, e.AdmittedOn).Value - (((p.BornOn.Value.Month > e.AdmittedOn.Month) || (e.Patient.BornOn.Value.Month == e.AdmittedOn.Month && e.Patient.BornOn.Value.Day > e.AdmittedOn.Day)) ? 1 : 0)))
                                       )
                                .Any(x => x >= minAge && x <= maxAge));
                        }
                        else
                        {
                            px = px.And(p => p.Encounters.Where(enc => enc == p.Encounters.OrderByDescending(x => x.AdmittedOn).FirstOrDefault())
                                .Select(e => (p.BornOn.Value > e.AdmittedOn) ?
                                    (e.AdmittedOn.Year - e.Patient.BornOn.Value.Year + ((p.BornOn.Value.Month < e.AdmittedOn.Month || (p.BornOn.Value.Month == e.AdmittedOn.Month && p.BornOn.Value.Day < e.AdmittedOn.Day)) ? 1 :0 ))
                                    :
                                    (e.AdmittedOn.Year - e.Patient.BornOn.Value.Year - (e.Patient.BornOn.Value.Month >= e.AdmittedOn.Month && (e.Patient.BornOn.Value.Month > e.AdmittedOn.Month || (e.Patient.BornOn.Value.Month == e.AdmittedOn.Month && e.Patient.BornOn.Value.Day > e.AdmittedOn.Day)) ? 1 : 0)))
                                .Any(x => x >= minAge && x <= maxAge));
                        }
                        
                        ageGroupingPredicate = ageGroupingPredicate.Or(px);
                    }
                    else if (ageRangeValues.MinAge.HasValue)
                    {
                        minAge = ageRangeValues.MinAge.Value;
                        if (_sqlProvider != Settings.SQLProvider.Oracle)
                        {
                            px = px.And(p => p.Encounters.Where(enc => enc == p.Encounters.OrderByDescending(x => x.AdmittedOn).FirstOrDefault())
                                .Select(e => (p.BornOn.Value > e.AdmittedOn) ?
                                    (DbFunctions.DiffYears(p.BornOn.Value, e.AdmittedOn).Value + ((p.BornOn.Value.Month < e.AdmittedOn.Month || (p.BornOn.Value.Month == e.AdmittedOn.Month && p.BornOn.Value.Day < e.AdmittedOn.Day)) ? 1 : 0))
                                    :
                                    (DbFunctions.DiffYears(p.BornOn, e.AdmittedOn).Value - (((p.BornOn.Value.Month > e.AdmittedOn.Month) || (e.Patient.BornOn.Value.Month == e.AdmittedOn.Month && e.Patient.BornOn.Value.Day > e.AdmittedOn.Day)) ? 1 : 0)))
                                .Any(x => x >= minAge));
                        }
                        else
                        {
                            px = px.And(p => p.Encounters.Where(enc => enc == p.Encounters.OrderByDescending(x => x.AdmittedOn).FirstOrDefault())
                                .Select(e => (p.BornOn.Value > e.AdmittedOn) ?
                                    (e.AdmittedOn.Year - e.Patient.BornOn.Value.Year + ((p.BornOn.Value.Month < e.AdmittedOn.Month || (p.BornOn.Value.Month == e.AdmittedOn.Month && p.BornOn.Value.Day < e.AdmittedOn.Day)) ? 1 : 0))
                                    :
                                    (e.AdmittedOn.Year - e.Patient.BornOn.Value.Year - (e.Patient.BornOn.Value.Month >= e.AdmittedOn.Month && (e.Patient.BornOn.Value.Month > e.AdmittedOn.Month || (e.Patient.BornOn.Value.Month == e.AdmittedOn.Month && e.Patient.BornOn.Value.Day > e.AdmittedOn.Day)) ? 1 : 0)))
                                .Any(x => x >= minAge));
                        }
                        ageGroupingPredicate = ageGroupingPredicate.Or(px);
                    }
                    else if (ageRangeValues.MaxAge.HasValue)
                    {
                        maxAge = ageRangeValues.MaxAge.Value;
                        if (_sqlProvider != Settings.SQLProvider.Oracle)
                        {
                            px = px.And(p => p.Encounters.Where(enc => enc == p.Encounters.OrderByDescending(x => x.AdmittedOn).FirstOrDefault())
                                .Select(e => (p.BornOn.Value > e.AdmittedOn) ?
                                    (DbFunctions.DiffYears(p.BornOn.Value, e.AdmittedOn).Value + ((p.BornOn.Value.Month < e.AdmittedOn.Month || (p.BornOn.Value.Month == e.AdmittedOn.Month && p.BornOn.Value.Day < e.AdmittedOn.Day)) ? 1 : 0))
                                    :
                                    (DbFunctions.DiffYears(p.BornOn, e.AdmittedOn).Value - (((p.BornOn.Value.Month > e.AdmittedOn.Month) || (e.Patient.BornOn.Value.Month == e.AdmittedOn.Month && e.Patient.BornOn.Value.Day > e.AdmittedOn.Day)) ? 1 : 0)))
                                .Any(x => x <= maxAge));
                        }
                        else
                        {
                            px = px.And(p => p.Encounters.Where(enc => enc == p.Encounters.OrderByDescending(x => x.AdmittedOn).FirstOrDefault())
                                .Select(e => (p.BornOn.Value > e.AdmittedOn) ?
                                    (e.AdmittedOn.Year - e.Patient.BornOn.Value.Year + ((p.BornOn.Value.Month < e.AdmittedOn.Month || (p.BornOn.Value.Month == e.AdmittedOn.Month && p.BornOn.Value.Day < e.AdmittedOn.Day)) ? 1 : 0))
                                    :
                                    (e.AdmittedOn.Year - e.Patient.BornOn.Value.Year - (e.Patient.BornOn.Value.Month >= e.AdmittedOn.Month && (e.Patient.BornOn.Value.Month > e.AdmittedOn.Month || (e.Patient.BornOn.Value.Month == e.AdmittedOn.Month && e.Patient.BornOn.Value.Day > e.AdmittedOn.Day)) ? 1 : 0)))
                                .Any(x => x <= maxAge));
                        }
                        ageGroupingPredicate = ageGroupingPredicate.Or(px);
                    }

                    continue;
                }

                DateTime calculateAsOf;

                if (ageRangeValues.CalculationType.Value == DTO.Enums.AgeRangeCalculationType.AsOfSpecifiedDate)
                {
                    if (!ageRangeValues.CalculateAsOf.HasValue)
                        throw new ArgumentException("Missing calculate as of specific date value for applying age range criteria.");

                    calculateAsOf = ageRangeValues.CalculateAsOf.Value.Date;
                }
                else if (ageRangeValues.CalculationType.Value == DTO.Enums.AgeRangeCalculationType.AsOfDateOfRequestSubmission)
                {
                    if (!_queryInterrogator.HasRequestSubmissionDate)
                        throw new ArgumentException("Missing request submission date value for applying age range criteria.");

                    calculateAsOf = _queryInterrogator.RequestSubmissionDate.Value;
                }
                else
                {
                    calculateAsOf = DateTime.Now.Date;
                }

                Expression<Func<pcori.Patient, bool>> calculationRangePredicate = null;
                if (ageRangeValues.MinAge.HasValue)
                {
                    int minAge = ageRangeValues.MinAge.Value;
                    if (_sqlProvider != Settings.SQLProvider.Oracle)
                    {
                        calculationRangePredicate = p => minAge <= ((p.BornOn.Value > calculateAsOf) ?
                                    (DbFunctions.DiffYears(p.BornOn.Value, calculateAsOf).Value + ((p.BornOn.Value.Month < calculateAsOf.Month || (p.BornOn.Value.Month == calculateAsOf.Month && p.BornOn.Value.Day < calculateAsOf.Day)) ? 1 : 0))
                                    :
                                    (DbFunctions.DiffYears(p.BornOn, calculateAsOf).Value - (((p.BornOn.Value.Month > calculateAsOf.Month) || (p.BornOn.Value.Month == calculateAsOf.Month && p.BornOn.Value.Day > calculateAsOf.Day)) ? 1 : 0)));
                    }
                    else
                    {
                        calculationRangePredicate = p => minAge <= ((p.BornOn.Value > calculateAsOf) ?
                                    (calculateAsOf.Year - p.BornOn.Value.Year + ((p.BornOn.Value.Month < calculateAsOf.Month || (p.BornOn.Value.Month == calculateAsOf.Month && p.BornOn.Value.Day < calculateAsOf.Day)) ? 1 : 0))
                                    :
                                    (calculateAsOf.Year - p.BornOn.Value.Year - (p.BornOn.Value.Month >= calculateAsOf.Month && (p.BornOn.Value.Month > calculateAsOf.Month || (p.BornOn.Value.Month == calculateAsOf.Month && p.BornOn.Value.Day > calculateAsOf.Day)) ? 1 : 0)));
                    }
                }

                if (ageRangeValues.MaxAge.HasValue)
                {
                    int maxAge = ageRangeValues.MaxAge.Value;
                    Expression<Func<pcori.Patient, bool>> maxPred;
                    if (_sqlProvider != Settings.SQLProvider.Oracle)
                    {
                        maxPred = p => ((p.BornOn.Value > calculateAsOf) ?
                                    (DbFunctions.DiffYears(p.BornOn.Value, calculateAsOf).Value + ((p.BornOn.Value.Month < calculateAsOf.Month || (p.BornOn.Value.Month == calculateAsOf.Month && p.BornOn.Value.Day < calculateAsOf.Day)) ? 1 : 0))
                                    :
                                    (DbFunctions.DiffYears(p.BornOn, calculateAsOf).Value - (((p.BornOn.Value.Month > calculateAsOf.Month) || (p.BornOn.Value.Month == calculateAsOf.Month && p.BornOn.Value.Day > calculateAsOf.Day)) ? 1 : 0))) <= maxAge;
                    }
                    else
                    {
                        maxPred = p => ((p.BornOn.Value > calculateAsOf) ?
                                    (calculateAsOf.Year - p.BornOn.Value.Year + ((p.BornOn.Value.Month < calculateAsOf.Month || (p.BornOn.Value.Month == calculateAsOf.Month && p.BornOn.Value.Day < calculateAsOf.Day)) ? 1 : 0))
                                    :
                                    (calculateAsOf.Year - p.BornOn.Value.Year - (p.BornOn.Value.Month >= calculateAsOf.Month && (p.BornOn.Value.Month > calculateAsOf.Month || (p.BornOn.Value.Month == calculateAsOf.Month && p.BornOn.Value.Day > calculateAsOf.Day)) ? 1 : 0))) <= maxAge;
                    }

                    calculationRangePredicate = calculationRangePredicate == null ? maxPred : calculationRangePredicate.And(maxPred);
                }

                if(calculationRangePredicate != null)
                    ageGroupingPredicate = ageGroupingPredicate.Or(calculationRangePredicate);
            }


            Expression<Func<pcori.Patient, bool>> predicate = (p) => p.BornOn.HasValue;
            predicate = predicate.And(ageGroupingPredicate);

            return patientPredicate.And(predicate);
        }

        protected override System.Linq.Expressions.Expression<Func<pcori.Patient, bool>> ApplyAgeRangeTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term)
        {
            //handled by the paragraph predicate method above
            return null;
        }


        protected override Expression<Func<pcori.Patient, bool>> ApplySexTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term)
        {
            //handled by the paragraph predicate method
            return null;
        }

        Expression<Func<pcori.Patient, bool>> ApplySexTerms(QueryComposerCriteriaDTO paragraph, Expression<Func<pcori.Patient, bool>> patientPredicate)
        {
            var terms = GetAllCriteriaTerms(paragraph, ModelTermsFactory.SexID).ToArray();

            if (!terms.Any(t => t.Type == ModelTermsFactory.SexID))
            {
                //only apply if there are any terms that act
                return patientPredicate;
            }

            Expression<Func<pcori.Patient, bool>> sexGroupingPredicate = null;
            foreach(var term in terms)
            {
                IEnumerable<string> sexes = TranslateSex(term.GetStringValue("Sex"));
                if(!sexes.Any())
                    continue;

                if(sexGroupingPredicate == null)
                {
                    sexGroupingPredicate = (p) =>  sexes.Contains(p.Sex.ToUpper());
                }
                else
                {
                     sexGroupingPredicate = sexGroupingPredicate.Or((p) =>  sexes.Contains(p.Sex.ToUpper()));
                }

            }

            
            if(sexGroupingPredicate != null)
            {
                return patientPredicate.And(sexGroupingPredicate);
            }

            //sex term specified but no value.
            return (p) => false;


        }

        static IEnumerable<string> TranslateSex(object raw)
        {
            Lpp.Dns.DTO.Enums.SexStratifications value;
            if (Enum.TryParse<Lpp.Dns.DTO.Enums.SexStratifications>((raw ?? string.Empty).ToString(), out value))
            {
                if (value == DTO.Enums.SexStratifications.FemaleOnly)
                {
                    return new[] { "F" };
                }
                if (value == DTO.Enums.SexStratifications.MaleOnly)
                {
                    return new[] { "M" };
                }
                if (value == DTO.Enums.SexStratifications.MaleAndFemale || value == DTO.Enums.SexStratifications.MaleAndFemaleAggregated)
                {
                    return new[] { "F", "M" };
                }
                if (value == DTO.Enums.SexStratifications.Ambiguous)
                {
                    return new[] { "A" };
                }
                if (value == DTO.Enums.SexStratifications.NoInformation)
                {
                    return new[] { "NI" };
                }
                if (value == DTO.Enums.SexStratifications.Unknown)
                {
                    return new[] { "UN" };
                }
                if (value == DTO.Enums.SexStratifications.Other)
                {
                    return new[] { "OT" };
                }
            }
            else
            {
                throw new Exception("Value for Sex Term is not Valid");
            }

            return Enumerable.Empty<string>();
        }

        Expression<Func<pcori.Patient, bool>> ApplyHispanicTerms(QueryComposerCriteriaDTO paragraph, Expression<Func<pcori.Patient, bool>> patientPredicate)
        {
            var terms = GetAllCriteriaTerms(paragraph, ModelTermsFactory.HispanicID).ToArray();

            if (!terms.Any(t => t.Type == ModelTermsFactory.HispanicID))
            {
                //only apply if there are any terms that act
                return patientPredicate;
            }

            Expression<Func<pcori.Patient, bool>> hispanicGroupingPredicate = null;
            foreach (var term in terms)
            {
                string value = term.GetStringValue("Hispanic");

                switch (value)
                {
                    case "0":
                        value = "UN";
                        break;
                    case "1":
                        value = "Y";
                        break;
                    case "2":
                        value = "N";
                        break;
                    case "3":
                        value = "R";
                        break;
                    case "4":
                        value = "NI";
                        break;
                    case "5":
                        value = "OT";
                        break;
                }

                if (!string.IsNullOrEmpty(value))
                {
                    if (hispanicGroupingPredicate == null)
                    {
                        hispanicGroupingPredicate = (p) => p.Hispanic == value;
                    }
                    else
                    {
                        hispanicGroupingPredicate = hispanicGroupingPredicate.Or((p) => p.Hispanic == value);
                    }
                }
                else
                {
                    throw new Exception("Value for Hispanic Term is not Valid");
                }
            }
            
            if (hispanicGroupingPredicate != null)
            {
                return patientPredicate.And(hispanicGroupingPredicate);
            }

            //hispanic term specified but no value.
            return (p) => false;
        }

        protected override System.Linq.Expressions.Expression<Func<pcori.Patient, bool>> ApplyHispanicTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term)
        {
            string value = term.GetStringValue("Hispanic");

            switch (value)
            {
                case "0":
                    value = "UN";
                    break;
                case "1":
                    value = "Y";
                    break;
                case "2":
                    value = "N";
                    break;
                case "3":
                    value = "R";
                    break;
                case "4":
                    value = "NI";
                    break;
                case "5":
                    value = "OT";
                    break;
            }

            if (!string.IsNullOrEmpty(value))
            {
                return (p) => p.Hispanic == value;
            }
            else {
                throw new Exception("Value for Hispanic Term is not Valid");
            }
        }

        protected override System.Linq.Expressions.Expression<Func<pcori.Patient, bool>> ApplyObservationPeriodTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term)
        {
            DateRangeValues observationRange = AdapterHelpers.ParseDateRangeValues(term);

            Expression<Func<pcori.Patient, bool>> observationPredicate = null;

            DateTime observationStartDate, observationEndDate;
            if (observationRange.StartDate.HasValue && observationRange.EndDate.HasValue)
            {
                observationStartDate = observationRange.StartDate.Value.Date;
                observationEndDate = observationRange.EndDate.Value.Date;
                observationPredicate = (p) => p.Encounters.Any(e => e.AdmittedOn >= observationStartDate && e.AdmittedOn <= observationEndDate);
            }
            else if (observationRange.StartDate.HasValue)
            {
                observationStartDate = observationRange.StartDate.Value.Date;
                observationPredicate = (p) => p.Encounters.Any(e => e.AdmittedOn >= observationStartDate);
            }
            else if (observationRange.EndDate.HasValue)
            {
                observationEndDate = observationRange.EndDate.Value.Date;
                observationPredicate = (p) => p.Encounters.Any(e => e.AdmittedOn <= observationEndDate);
            }

            if (observationPredicate == null)
            {
                //term specified but no values

                return (p) => p.Encounters.Any();
            }

            //apply any applicable age range restrictions
            DTO.Enums.AgeRangeCalculationType[] calculationTypes = new[] { 
                DTO.Enums.AgeRangeCalculationType.AsOfObservationPeriodStartDateWithinCriteriaGroup,
                DTO.Enums.AgeRangeCalculationType.AsOfObservationPeriodEndDateWithinCriteriaGroup
            };
            IEnumerable<AgeRangeValues> ageRanges = AdapterHelpers.ParseAgeRangeValues(GetAllCriteriaTerms(paragraph, ModelTermsFactory.AgeRangeID), calculationTypes).ToArray();

            if (!ageRanges.Any() || ageRanges.All(a => a.MinAge == null && a.MaxAge == null))
            {
                return observationPredicate;
            }
            
            foreach (var range in ageRanges)
            {
                //if the calculation type is for start of observation period and there is no start observation period value, cannot apply age range restriction
                if (range.CalculationType.Value == DTO.Enums.AgeRangeCalculationType.AsOfObservationPeriodStartDateWithinCriteriaGroup && !observationRange.StartDate.HasValue)
                    continue;

                //if the calculation type is for end of observation period and there is no end observation period value, cannot apply age range restriction
                if (range.CalculationType == DTO.Enums.AgeRangeCalculationType.AsOfObservationPeriodEndDateWithinCriteriaGroup && !observationRange.EndDate.HasValue)
                    continue;

                DateTime calculateAsOf = range.CalculationType.Value == DTO.Enums.AgeRangeCalculationType.AsOfObservationPeriodStartDateWithinCriteriaGroup ? observationRange.StartDate.Value.Date : observationRange.EndDate.Value.Date;

                if (range.MinAge.HasValue)
                {
                    int minAge = range.MinAge.Value;
                    if (_sqlProvider != Settings.SQLProvider.Oracle)
                    {
                        observationPredicate = observationPredicate.And(p => minAge <= ((p.BornOn.Value > calculateAsOf) ?
                                    (DbFunctions.DiffYears(p.BornOn.Value, calculateAsOf).Value + ((p.BornOn.Value.Month < calculateAsOf.Month || (p.BornOn.Value.Month == calculateAsOf.Month && p.BornOn.Value.Day < calculateAsOf.Day)) ? 1 : 0))
                                    :
                                    (DbFunctions.DiffYears(p.BornOn, calculateAsOf).Value - (((p.BornOn.Value.Month > calculateAsOf.Month) || (p.BornOn.Value.Month == calculateAsOf.Month && p.BornOn.Value.Day > calculateAsOf.Day)) ? 1 : 0))));
                    }
                    else
                    {
                        observationPredicate = observationPredicate.And(p => minAge <= ((p.BornOn.Value > calculateAsOf) ?
                                    (calculateAsOf.Year - p.BornOn.Value.Year + ((p.BornOn.Value.Month < calculateAsOf.Month || (p.BornOn.Value.Month == calculateAsOf.Month && p.BornOn.Value.Day < calculateAsOf.Day)) ? 1 : 0))
                                    :
                                    (calculateAsOf.Year - p.BornOn.Value.Year - (p.BornOn.Value.Month >= calculateAsOf.Month && (p.BornOn.Value.Month > calculateAsOf.Month || (p.BornOn.Value.Month == calculateAsOf.Month && p.BornOn.Value.Day > calculateAsOf.Day)) ? 1 : 0))));
                    }
                }
                if (range.MaxAge.HasValue)
                {
                    int maxAge = range.MaxAge.Value;
                    if (_sqlProvider != Settings.SQLProvider.Oracle)
                    {
                        observationPredicate = observationPredicate.And(p => ((p.BornOn.Value > calculateAsOf) ?
                                    (DbFunctions.DiffYears(p.BornOn.Value, calculateAsOf).Value + ((p.BornOn.Value.Month < calculateAsOf.Month || (p.BornOn.Value.Month == calculateAsOf.Month && p.BornOn.Value.Day < calculateAsOf.Day)) ? 1 : 0))
                                    :
                                    (DbFunctions.DiffYears(p.BornOn, calculateAsOf).Value - (((p.BornOn.Value.Month > calculateAsOf.Month) || (p.BornOn.Value.Month == calculateAsOf.Month && p.BornOn.Value.Day > calculateAsOf.Day)) ? 1 : 0))) <= maxAge);
                    }
                    else
                    {
                        observationPredicate = observationPredicate.And(p => ((p.BornOn.Value > calculateAsOf) ?
                                    (calculateAsOf.Year - p.BornOn.Value.Year + ((p.BornOn.Value.Month < calculateAsOf.Month || (p.BornOn.Value.Month == calculateAsOf.Month && p.BornOn.Value.Day < calculateAsOf.Day)) ? 1 : 0))
                                    :
                                    (calculateAsOf.Year - p.BornOn.Value.Year - (p.BornOn.Value.Month >= calculateAsOf.Month && (p.BornOn.Value.Month > calculateAsOf.Month || (p.BornOn.Value.Month == calculateAsOf.Month && p.BornOn.Value.Day > calculateAsOf.Day)) ? 1 : 0))) <= maxAge);
                    }
                }
            }

            return observationPredicate;
        }

        protected override System.Linq.Expressions.Expression<Func<pcori.Patient, bool>> ApplyVisitsTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term)
        {
            //handled by the paragraph predicate method
            return null;
        }

        Expression<Func<pcori.Patient, bool>> ApplyVisitsTerms(QueryComposerCriteriaDTO paragraph, Expression<Func<pcori.Patient, bool>> patientPredicate)
        {
            var terms = GetAllCriteriaTerms(paragraph, ModelTermsFactory.VisitsID).ToArray();
            if (!terms.Any())
            {
                return patientPredicate;
            }

            int? minimumVisits = terms.Select(t =>
            {
                int v;
                if (int.TryParse(t.GetStringValue("Visits"), out v))
                {
                    return (int?)v;
                }
                return null;

            }).Where(t => t != null).Min();

            if (minimumVisits == null)
                return patientPredicate;

            int val = minimumVisits.Value;
            return patientPredicate.And(p => p.Encounters.Count() >= val);

        }

        Expression<Func<pcori.Patient, bool>> ApplyVitalsTerms(QueryComposerCriteriaDTO paragraph, Expression<Func<pcori.Patient, bool>> patientPredicate)
        {
            //create the applicable predicates based on height, weight, vital measure date, and observation period

            var vitalsTerms = GetAllCriteriaTerms(paragraph, new[] { ModelTermsFactory.HeightID, ModelTermsFactory.WeightID, ModelTermsFactory.VitalsMeasureDateID });
            if (vitalsTerms.Any() == false)
            {
                //only apply if there are any terms that act
                return patientPredicate;
            }

            List<Expression<Func<pcori.Vital, bool>>> predicates = new List<Expression<Func<pcori.Vital, bool>>>();

            /*
             * Note: the following Age Range calculations may not be applicable for Vitals, as encounters may or may not be defined/linked for vital entries.
             * For more information, refer to PMNDEV-5959.
             */

            //limit to the encounters where the patient age falls within the specified range.
            DTO.Enums.AgeRangeCalculationType[] calculationTypes = new[] { 
                DTO.Enums.AgeRangeCalculationType.AtFirstMatchingEncounterWithinCriteriaGroup,
                DTO.Enums.AgeRangeCalculationType.AtLastMatchingEncounterWithinCriteriaGroup
            };
            IEnumerable<AgeRangeValues> ageRanges = AdapterHelpers.ParseAgeRangeValues(GetAllCriteriaTerms(paragraph, ModelTermsFactory.AgeRangeID), calculationTypes).Where(a => a.MinAge.HasValue || a.MaxAge.HasValue).ToArray();
            foreach (var range in ageRanges)
            {

                int age = 0;
                if (range.MinAge.HasValue)
                {
                    age = range.MinAge.Value;
                    if (_sqlProvider != Settings.SQLProvider.Oracle)
                    {
                        predicates.Add(v => !string.IsNullOrEmpty(v.EncounterID) && age <= ((v.Patient.BornOn.Value > v.Encounter.AdmittedOn) ?
                                    (DbFunctions.DiffYears(v.Patient.BornOn.Value, v.Encounter.AdmittedOn).Value + ((v.Patient.BornOn.Value.Month < v.Encounter.AdmittedOn.Month || (v.Patient.BornOn.Value.Month == v.Encounter.AdmittedOn.Month && v.Patient.BornOn.Value.Day < v.Encounter.AdmittedOn.Day)) ? 1 : 0))
                                    :
                                    (DbFunctions.DiffYears(v.Patient.BornOn, v.Encounter.AdmittedOn).Value - (((v.Patient.BornOn.Value.Month > v.Encounter.AdmittedOn.Month) || (v.Patient.BornOn.Value.Month == v.Encounter.AdmittedOn.Month && v.Patient.BornOn.Value.Day > v.Encounter.AdmittedOn.Day)) ? 1 : 0))));
                    }
                    else
                    {
                        predicates.Add(v => !string.IsNullOrEmpty(v.EncounterID) && age <= ((v.Patient.BornOn.Value > v.Encounter.AdmittedOn) ?
                                    (v.Encounter.AdmittedOn.Year - v.Patient.BornOn.Value.Year + ((v.Patient.BornOn.Value.Month < v.Encounter.AdmittedOn.Month || (v.Patient.BornOn.Value.Month == v.Encounter.AdmittedOn.Month && v.Patient.BornOn.Value.Day < v.Encounter.AdmittedOn.Day)) ? 1 : 0))
                                    :
                                    (v.Encounter.AdmittedOn.Year - v.Patient.BornOn.Value.Year - (v.Patient.BornOn.Value.Month >= v.Encounter.AdmittedOn.Month && (v.Patient.BornOn.Value.Month > v.Encounter.AdmittedOn.Month || (v.Patient.BornOn.Value.Month == v.Encounter.AdmittedOn.Month && v.Patient.BornOn.Value.Day > v.Encounter.AdmittedOn.Day)) ? 1 : 0))));
                    }
                }
                if (range.MaxAge.HasValue)
                {
                    age = range.MaxAge.Value;
                    if (_sqlProvider != Settings.SQLProvider.Oracle)
                    {
                        predicates.Add(v => !string.IsNullOrEmpty(v.EncounterID) && ((v.Patient.BornOn.Value > v.Encounter.AdmittedOn) ?
                                    (DbFunctions.DiffYears(v.Patient.BornOn.Value, v.Encounter.AdmittedOn).Value + ((v.Patient.BornOn.Value.Month < v.Encounter.AdmittedOn.Month || (v.Patient.BornOn.Value.Month == v.Encounter.AdmittedOn.Month && v.Patient.BornOn.Value.Day < v.Encounter.AdmittedOn.Day)) ? 1 : 0))
                                    :
                                    (DbFunctions.DiffYears(v.Patient.BornOn, v.Encounter.AdmittedOn).Value - (((v.Patient.BornOn.Value.Month > v.Encounter.AdmittedOn.Month) || (v.Patient.BornOn.Value.Month == v.Encounter.AdmittedOn.Month && v.Patient.BornOn.Value.Day > v.Encounter.AdmittedOn.Day)) ? 1 : 0))) <= age);
                    }
                    else
                    {
                        predicates.Add(v => !string.IsNullOrEmpty(v.EncounterID) && ((v.Patient.BornOn.Value > v.Encounter.AdmittedOn) ?
                                    (v.Encounter.AdmittedOn.Year - v.Patient.BornOn.Value.Year + ((v.Patient.BornOn.Value.Month < v.Encounter.AdmittedOn.Month || (v.Patient.BornOn.Value.Month == v.Encounter.AdmittedOn.Month && v.Patient.BornOn.Value.Day < v.Encounter.AdmittedOn.Day)) ? 1 : 0))
                                    :
                                    (v.Encounter.AdmittedOn.Year - v.Patient.BornOn.Value.Year - (v.Patient.BornOn.Value.Month >= v.Encounter.AdmittedOn.Month && (v.Patient.BornOn.Value.Month > v.Encounter.AdmittedOn.Month || (v.Patient.BornOn.Value.Month == v.Encounter.AdmittedOn.Month && v.Patient.BornOn.Value.Day > v.Encounter.AdmittedOn.Day)) ? 1 : 0))) <= age);
                    }
                }
            }

            DateRangeValues measureDateRange = null;
            HeightValues heightRange = null;
            WeightValues weightRange = null;

            bool hasVitalsMeasureTerm = vitalsTerms.Any(t => t.Type == ModelTermsFactory.VitalsMeasureDateID);

            if (hasVitalsMeasureTerm)
            {
                measureDateRange = new DateRangeValues();
                foreach (var measureTerm in vitalsTerms.Where(t => t.Type == ModelTermsFactory.VitalsMeasureDateID))
                {
                    var range = AdapterHelpers.ParseDateRangeValues(measureTerm);

                    if (range.StartDate.HasValue && (measureDateRange.StartDate == null || measureDateRange.StartDate.Value < range.StartDate.Value))
                    {
                        measureDateRange.StartDate = range.StartDate;
                    }
                    if (range.EndDate.HasValue && (measureDateRange.EndDate == null || measureDateRange.EndDate.Value > range.EndDate.Value))
                    {
                        measureDateRange.EndDate = range.EndDate;
                    }
                }
            }

            if (vitalsTerms.Any(t => t.Type == ModelTermsFactory.HeightID))
            {
                heightRange = new HeightValues();
                foreach (var heightTerm in vitalsTerms.Where(t => t.Type == ModelTermsFactory.HeightID))
                {
                    var range = AdapterHelpers.ParseHeightValues(heightTerm);

                    if (range.MinHeight.HasValue)
                    {
                        heightRange.MinHeight = heightRange.MinHeight.HasValue ? Math.Max(heightRange.MinHeight.Value, range.MinHeight.Value) : range.MinHeight;
                    }
                    if (range.MaxHeight.HasValue)
                    {
                        heightRange.MaxHeight = heightRange.MaxHeight.HasValue ? Math.Min(heightRange.MaxHeight.Value, range.MaxHeight.Value) : range.MaxHeight;
                    }
                }
            }

            if (vitalsTerms.Any(t => t.Type == ModelTermsFactory.WeightID))
            {
                weightRange = new WeightValues();
                foreach (var weightTerm in vitalsTerms.Where(t => t.Type == ModelTermsFactory.WeightID))
                {
                    var range = AdapterHelpers.ParseWeightValues(weightTerm);

                    if (range.MinWeight.HasValue)
                    {
                        //ie if current is 20lb and the term is 25 as minmum, the overal minimum should be 25
                        weightRange.MinWeight = weightRange.MinWeight.HasValue ? Math.Max(weightRange.MinWeight.Value, range.MinWeight.Value) : range.MinWeight;
                    }

                    if (range.MaxWeight.HasValue )
                    {
                        //ie if the current max is 60lb but the term limits to max of 45lb, change current max to the smaller
                        weightRange.MaxWeight = weightRange.MaxWeight.HasValue ? Math.Min(weightRange.MaxWeight.Value, range.MaxWeight.Value) : range.MaxWeight;
                    }
                }
            }

            if (measureDateRange != null)
            {
                if (measureDateRange.StartDate.HasValue)
                {
                    DateTime startDate = measureDateRange.StartDate.Value.Date;
                    predicates.Add(v => v.MeasuredOn >= startDate);
                }
                if (measureDateRange.EndDate.HasValue)
                {
                    DateTime endDate = measureDateRange.EndDate.Value.Date;
                    predicates.Add(v => v.MeasuredOn <= endDate);
                }
            }

            if (heightRange != null)
            {
                predicates.Add(v => v.Height.HasValue);

                if (heightRange.MinHeight.HasValue)
                {
                    double minHeight = heightRange.MinHeight.Value;
                    predicates.Add(v => v.Height.Value >= minHeight);
                }
                if (heightRange.MaxHeight.HasValue)
                {
                    double maxHeight = heightRange.MaxHeight.Value;
                    predicates.Add(v => v.Height.Value <= maxHeight);
                }
            }
            

            if (weightRange != null)
            {
                predicates.Add(v => v.Weight.HasValue);

                if (weightRange.MinWeight.HasValue)
                {
                    double minWeight = weightRange.MinWeight.Value;
                    predicates.Add(v => v.Weight.Value >= minWeight);
                }
                if (weightRange.MaxWeight.HasValue)
                {
                    double maxWeight = weightRange.MaxWeight.Value;
                    predicates.Add(v => v.Weight.Value <= maxWeight);
                }
            }
            
            //As per PMNDEV-5959, Vitals are not to be joined with the Encounter table.
            //This is due to the fact that the EncounterID field in the Vitals table is optional and usually unpopulated.
            //So, given this, if we are applying any critera against vitals, then we should check for Observation Period Ranges
            //and apply the observation period against the patientPredicate itself looking for patients that have encounters within the date range.
            //This filter is independent of the record in Vitals itself.
            if((predicates.Count == 0 && hasVitalsMeasureTerm) || predicates.Count > 0)
            {
                List<DateRangeValues> observationPeriodRanges = _queryInterrogator.ParagraphObservationPeriodDateRanges(paragraph);
                if (observationPeriodRanges != null && observationPeriodRanges.Any())
                {
                    Expression<Func<pcori.Patient, bool>> observationPeriodPredicate = null;

                    foreach (var range in observationPeriodRanges)
                    {
                        DateTime? start = null;
                        if (range.StartDate.HasValue)
                            start = range.StartDate.Value.DateTime.Date;

                        DateTime? end = null;
                        if (range.EndDate.HasValue)
                            end = range.EndDate.Value.Date;

                        if (start.HasValue && end.HasValue)
                        {
                            if (observationPeriodPredicate == null)
                                observationPeriodPredicate = (p) => p.Encounters.Any((enc) => enc.AdmittedOn >= start && enc.AdmittedOn <= end);
                            else
                                observationPeriodPredicate = observationPeriodPredicate.Or(p => p.Encounters.Any((enc) => enc.AdmittedOn >= start && enc.AdmittedOn <= end));
                        }
                        else if (start.HasValue)
                        {
                            if (observationPeriodPredicate == null)
                                observationPeriodPredicate = (p) => p.Encounters.Any((enc) => enc.AdmittedOn >= start);
                            else
                                observationPeriodPredicate = observationPeriodPredicate.Or(p => p.Encounters.Any((enc) => enc.AdmittedOn >= start));
                        }
                        else if (end.HasValue)
                        {
                            if (observationPeriodPredicate == null)
                                observationPeriodPredicate = (p) => p.Encounters.Any((enc) => enc.AdmittedOn <= end);
                            else
                                observationPeriodPredicate = observationPeriodPredicate.Or(p => p.Encounters.Any((enc) => enc.AdmittedOn <= end));
                        }
                    }

                    if (observationPeriodPredicate != null)
                        patientPredicate = patientPredicate.And(observationPeriodPredicate);
                }
            }

            if (predicates.Count == 0)
            {
                if (hasVitalsMeasureTerm)
                    patientPredicate = patientPredicate.And(p => p.Vitals.Any());
                return patientPredicate;
            }

            var vitalsPredicate = predicates[0];
            for (int i = 1; i <= predicates.Count - 1; i++)
            {
                vitalsPredicate = vitalsPredicate.And(predicates[i]);
            }

            return patientPredicate.And((p) => p.Vitals.AsQueryable().Any(vitalsPredicate));
        }

        protected override Expression<Func<pcori.Patient, bool>> ApplyVitalsMeasureDateObservationPeriod(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term)
        {
            //term handled in combinded vitals predicate function
            return null;
        }

        protected override System.Linq.Expressions.Expression<Func<pcori.Patient, bool>> ApplyHeightTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term)
        {
            //term handled in combinded vitals predicate function
            return null;
        }

        protected override System.Linq.Expressions.Expression<Func<pcori.Patient, bool>> ApplyWeightTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term)
        {
            //term handled in combinded vitals predicate function
            return null;
        }

        Expression<Func<pcori.Patient, bool>> ApplyClinicalTrialTerms(QueryComposerCriteriaDTO paragraph, Expression<Func<pcori.Patient, bool>> patientPredicate)
        {
            var terms = GetAllCriteriaTerms(paragraph, ModelTermsFactory.TrialID).ToArray();
            if (!terms.Any())
                return patientPredicate;

            Expression<Func<pcori.ClinicalTrial, bool>> trialIDPredicate = null;
            foreach(var term in terms)
            {
                string trialID = term.GetStringValue("Trial");
                if (!string.IsNullOrWhiteSpace(trialID))
                {
                    if(trialIDPredicate == null)
                    {
                        trialIDPredicate = (tr) => tr.TrialID == trialID;
                    }
                    else
                    {
                        trialIDPredicate = trialIDPredicate.Or((tr) => tr.TrialID == trialID);
                    }
                }
                else
                {
                    patientPredicate = patientPredicate.And((p)=> p.ClinicalTrials.Any());
                }
            }

            return trialIDPredicate == null ? patientPredicate : patientPredicate.And((p) => p.ClinicalTrials.Any(trialIDPredicate.Compile()));
        }

        Expression<Func<pcori.Patient, bool>> ApplyPatientReportedOutcomeTerms(QueryComposerCriteriaDTO paragraph, Expression<Func<pcori.Patient, bool>> patientPredicate)
        {
            var terms = GetAllCriteriaTerms(paragraph, ModelTermsFactory.PatientReportedOutcomeID).ToArray();
            if (!terms.Any())
                return patientPredicate;

            Expression<Func<pcori.ReportedOutcome, bool>> proPredicate = null;
            foreach(var term in terms)
            {
                string itemName = term.GetStringValue("ItemName");
                string itemText = term.GetStringValue("ItemResponse");

                Expression<Func<pcori.ReportedOutcome, bool>> itemPredictate = null;
                if(!string.IsNullOrEmpty(itemName) && !string.IsNullOrEmpty(itemText))
                {
                    //apply name and response text and'd
                    itemPredictate = (pro) => pro.ItemName == itemName && pro.ResponseText == itemText;

                }else if(!string.IsNullOrEmpty(itemName) && string.IsNullOrEmpty(itemText))
                {
                    //apply only itemName
                    itemPredictate = (pro) => pro.ItemName == itemName;

                }else if(string.IsNullOrEmpty(itemName) && !string.IsNullOrEmpty(itemText))
                {
                    //apply only response text
                    itemPredictate = (pro) => pro.ResponseText == itemText;
                }
                else
                {
                    patientPredicate = patientPredicate.And((p) => p.ReportedOutcomes.Any());
                }

                if(itemPredictate != null)
                {
                    if(proPredicate == null)
                    {
                        proPredicate = itemPredictate;
                    }
                    else
                    {
                        proPredicate = proPredicate.Or(itemPredictate);
                    }
                }

            }

            return proPredicate == null ? patientPredicate : patientPredicate.And((p) => p.ReportedOutcomes.Any(proPredicate.Compile()));
        }

        Expression<Func<pcori.Patient, bool>> ApplyRaceTerms(QueryComposerCriteriaDTO paragraph, Expression<Func<pcori.Patient, bool>> patientPredicate)
        {
            var terms = GetAllCriteriaTerms(paragraph, ModelTermsFactory.RaceID).ToArray();

            if (!terms.Any(t => t.Type == ModelTermsFactory.RaceID))
            {
                //only apply if there are any terms that act
                return patientPredicate;
            }

            Expression<Func<pcori.Patient, bool>> raceGroupingPredicate = null;
            foreach (var term in terms)
            {
                IEnumerable<string> values = term.GetStringCollection("Race").ToArray();

                if (values == null && !values.Any())
                    throw new Exception("Value for Race Term is not Valid");

                foreach (string value in values)
                {
                    string translatedValue = TranslateRace(value);
                    if (raceGroupingPredicate == null)
                    {
                        raceGroupingPredicate = (p) => p.Race == translatedValue;
                    }
                    else
                    {
                        raceGroupingPredicate = raceGroupingPredicate.Or((p) => p.Race == translatedValue);
                    }
                }
            }

            if (raceGroupingPredicate != null)
            {
                return patientPredicate.And(raceGroupingPredicate);
            }

            //hispanic term specified but no value.
            return (p) => false;
        }

        static string TranslateRace(string raw)
        {
            switch (raw)
            {
                case "0":
                    return "UN";
                case "1":
                    return "01";
                case "2":
                    return "02";
                case "3":
                    return "03";
                case "4":
                    return "04";
                case "5":
                    return "05";
                case "6":
                    return "06";
                case "7":
                    return "07";
                case "8":
                    return "NI";
                case "9":
                    return "OT";
                default:
                    throw new Exception("Value for Race Term is not Valid");

            }
        }

        protected override Expression<Func<pcori.Patient, bool>> ApplyRaceTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term)
        {
            var innerPredicate = PredicateBuilder.False<pcori.Patient>();

            IEnumerable<string> values = term.GetStringCollection("Race").ToArray();

            if (values == null && !values.Any())
                throw new Exception("Value for Race Term is not Valid");

            foreach (string value in values)
            {
                string translatedValue = TranslateRace(value);
                innerPredicate = innerPredicate.Or(r => r.Race == translatedValue);
            }

            return innerPredicate;
        }

        protected override Expression<Func<pcori.Patient, bool>> ApplySettingTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term)
        {
            string value = term.GetStringValue("Setting");
            DTO.Enums.Settings enumValue;
            if (Enum.TryParse<DTO.Enums.Settings>(value, out enumValue))
            {
                value = enumValue.ToString("G");
            }

            if (enumValue == DTO.Enums.Settings.AN)
            {
                return (p) => p.Encounters.Any(e => !string.IsNullOrEmpty(e.EncounterType));
            }

            return (p) => p.Encounters.Any(e => e.EncounterType == value);
        }

        Expression<Func<pcori.Encounter, bool>> GetParagraphEncounterPredicate(QueryComposerCriteriaDTO paragraph)
        {
            Expression<Func<pcori.Encounter, bool>> observationPeriodPredicate = null;
            Expression<Func<pcori.Encounter, bool>> settingPredicate = null;

            List<DateRangeValues> observationPeriodRanges = _queryInterrogator.ParagraphObservationPeriodDateRanges(paragraph);
            List<string> settingValues = _queryInterrogator.ParagraphEncounterTypes(paragraph);

            foreach (var range in observationPeriodRanges)
            {
                DateTime? start = null;
                if (range.StartDate.HasValue)
                    start = range.StartDate.Value.DateTime.Date;

                DateTime? end = null;
                if (range.EndDate.HasValue)
                    end = range.EndDate.Value.Date;

                if (start.HasValue && end.HasValue)
                {
                    if (observationPeriodPredicate == null)
                        observationPeriodPredicate = (d) => d.AdmittedOn >= start && d.AdmittedOn <= end;
                    else
                        observationPeriodPredicate = observationPeriodPredicate.Or(d => d.AdmittedOn >= start && d.AdmittedOn <= end);
                }
                else if (start.HasValue)
                {
                    if (observationPeriodPredicate == null)
                        observationPeriodPredicate = (d) => d.AdmittedOn >= start;
                    else
                        observationPeriodPredicate = observationPeriodPredicate.Or(d => d.AdmittedOn >= start);
                }
                else if (end.HasValue)
                {
                    if (observationPeriodPredicate == null)
                        observationPeriodPredicate = (d) => d.AdmittedOn <= end;
                    else
                        observationPeriodPredicate = observationPeriodPredicate.Or(d => d.AdmittedOn <= end);
                }
            }

            //apply any applicable age range restrictions
            DTO.Enums.AgeRangeCalculationType[] calculationTypes = new[] {
                DTO.Enums.AgeRangeCalculationType.AsOfObservationPeriodStartDateWithinCriteriaGroup,
                DTO.Enums.AgeRangeCalculationType.AsOfObservationPeriodEndDateWithinCriteriaGroup
            };
            IEnumerable<AgeRangeValues> ageRanges = AdapterHelpers.ParseAgeRangeValues(GetAllCriteriaTerms(paragraph, ModelTermsFactory.AgeRangeID), calculationTypes).ToArray();

            if (ageRanges.Any() && observationPeriodPredicate == null)
            {
                throw new ArgumentException("No observation period term is defined in the criteria group for applying age range criteria.");
            }
            else if (ageRanges.Any()
                && ageRanges.Any(p => p.CalculationType == DTO.Enums.AgeRangeCalculationType.AsOfObservationPeriodStartDateWithinCriteriaGroup)
                && observationPeriodRanges.All(p => p.StartDate.HasValue == false))
            {
                throw new ArgumentException("No observation period term with a Start Date is defined in the criteria group for applying age range criteria.");
            }
            else if (ageRanges.Any()
                && ageRanges.Any(p => p.CalculationType == DTO.Enums.AgeRangeCalculationType.AsOfObservationPeriodEndDateWithinCriteriaGroup)
                && observationPeriodRanges.All(p => p.EndDate.HasValue == false))
            {
                throw new ArgumentException("No observation period term with an End Date is defined in the criteria group for applying age range criteria.");
            }
            else if (ageRanges.Any() && observationPeriodPredicate != null)
            {
                Expression<Func<pcori.Encounter, bool>> ageGroupingPredicate = (d) => false;

                foreach (var range in ageRanges.Where(r => r.MinAge.HasValue || r.MaxAge.HasValue))
                {
                    foreach(var observationRange in observationPeriodRanges)
                    {
                        //if the calculation type is for start of observation period and there is no start observation period value, cannot apply age range restriction
                        if (range.CalculationType.Value == DTO.Enums.AgeRangeCalculationType.AsOfObservationPeriodStartDateWithinCriteriaGroup && !observationRange.StartDate.HasValue)
                            continue;

                        //if the calculation type is for end of observation period and there is no end observation period value, cannot apply age range restriction
                        if (range.CalculationType == DTO.Enums.AgeRangeCalculationType.AsOfObservationPeriodEndDateWithinCriteriaGroup && !observationRange.EndDate.HasValue)
                            continue;

                        DateTime calculateAsOf = range.CalculationType.Value == DTO.Enums.AgeRangeCalculationType.AsOfObservationPeriodStartDateWithinCriteriaGroup ? observationRange.StartDate.Value.Date : observationRange.EndDate.Value.Date;

                        Expression<Func<pcori.Encounter, bool>> ageRangePredicate = (d) => true;
                        if (range.MinAge.HasValue)
                        {
                            int minAge = range.MinAge.Value;
                            if (_sqlProvider != Settings.SQLProvider.Oracle)
                            {
                                ageRangePredicate = ageRangePredicate.And(d => minAge <= ((d.Patient.BornOn > calculateAsOf) ?
                                    (DbFunctions.DiffYears(d.Patient.BornOn.Value, calculateAsOf).Value + ((d.Patient.BornOn.Value.Month < calculateAsOf.Month || (d.Patient.BornOn.Value.Month == calculateAsOf.Month && d.Patient.BornOn.Value.Day < calculateAsOf.Day)) ? 1 : 0))
                                    :
                                    (DbFunctions.DiffYears(d.Patient.BornOn, calculateAsOf).Value - (((d.Patient.BornOn.Value.Month > calculateAsOf.Month) || (d.Patient.BornOn.Value.Month == calculateAsOf.Month && d.Patient.BornOn.Value.Day > calculateAsOf.Day)) ? 1 : 0))));
                            }
                            else
                            {
                                ageRangePredicate = ageRangePredicate.And(d => minAge <= ((d.Patient.BornOn.Value > calculateAsOf) ?
                                    (calculateAsOf.Year - d.Patient.BornOn.Value.Year + ((d.Patient.BornOn.Value.Month < calculateAsOf.Month || (d.Patient.BornOn.Value.Month == calculateAsOf.Month && d.Patient.BornOn.Value.Day < calculateAsOf.Day)) ? 1 : 0))
                                    :
                                    (calculateAsOf.Year - d.Patient.BornOn.Value.Year - (d.Patient.BornOn.Value.Month >= calculateAsOf.Month && (d.Patient.BornOn.Value.Month > calculateAsOf.Month || (d.Patient.BornOn.Value.Month == calculateAsOf.Month && d.Patient.BornOn.Value.Day > calculateAsOf.Day)) ? 1 : 0))));
                            }
                        }
                        if (range.MaxAge.HasValue)
                        {
                            int maxAge = range.MaxAge.Value;
                            if (_sqlProvider != Settings.SQLProvider.Oracle)
                            {
                                ageRangePredicate = ageRangePredicate.And(d => ((d.Patient.BornOn > calculateAsOf) ?
                                    (DbFunctions.DiffYears(d.Patient.BornOn.Value, calculateAsOf).Value + ((d.Patient.BornOn.Value.Month < calculateAsOf.Month || (d.Patient.BornOn.Value.Month == calculateAsOf.Month && d.Patient.BornOn.Value.Day < calculateAsOf.Day)) ? 1 : 0))
                                    :
                                    (DbFunctions.DiffYears(d.Patient.BornOn, calculateAsOf).Value - (((d.Patient.BornOn.Value.Month > calculateAsOf.Month) || (d.Patient.BornOn.Value.Month == calculateAsOf.Month && d.Patient.BornOn.Value.Day > calculateAsOf.Day)) ? 1 : 0))) <= maxAge);
                            }
                            else
                            {
                                ageRangePredicate = ageRangePredicate.And(d => ((d.Patient.BornOn.Value > calculateAsOf) ?
                                    (calculateAsOf.Year - d.Patient.BornOn.Value.Year + ((d.Patient.BornOn.Value.Month < calculateAsOf.Month || (d.Patient.BornOn.Value.Month == calculateAsOf.Month && d.Patient.BornOn.Value.Day < calculateAsOf.Day)) ? 1 : 0))
                                    :
                                    (calculateAsOf.Year - d.Patient.BornOn.Value.Year - (d.Patient.BornOn.Value.Month >= calculateAsOf.Month && (d.Patient.BornOn.Value.Month > calculateAsOf.Month || (d.Patient.BornOn.Value.Month == calculateAsOf.Month && d.Patient.BornOn.Value.Day > calculateAsOf.Day)) ? 1 : 0))) <= maxAge);
                            }
                        }
                        ageGroupingPredicate = ageGroupingPredicate.Or(ageRangePredicate);
                    }
                }

                observationPeriodPredicate = observationPeriodPredicate.And(ageGroupingPredicate.Expand());
            }

            foreach (var value in settingValues)
            {
                if (settingPredicate == null)
                {
                    if (value == "AN")
                        settingPredicate = (enc) => !string.IsNullOrEmpty(enc.EncounterType);
                    else
                        settingPredicate = (enc) => enc.EncounterType == value;
                }
                else
                {
                    if (value == "AN")
                        settingPredicate = settingPredicate.Or(enc => !string.IsNullOrEmpty(enc.EncounterType));
                    else
                        settingPredicate = settingPredicate.Or(enc => enc.EncounterType == value);
                }
            }

            if (settingPredicate == null && observationPeriodPredicate == null)
            {
                return null;
            }
            else if (settingPredicate != null && observationPeriodPredicate == null)
            {
                return settingPredicate;
            }
            else if (settingPredicate == null && observationPeriodPredicate != null)
            {
                return observationPeriodPredicate;
            }
            else
            {
                return settingPredicate.And(observationPeriodPredicate.Expand());
            }
        }

        Expression<Func<pcori.Patient, bool>> ApplyCombinedDiagnosisAndProcedureCodeTerms(QueryComposerCriteriaDTO paragraph, Expression<Func<pcori.Patient, bool>> patientPredicate)
        {
            //the terms could exist in the main terms collection or in the subcriteria holding multiple combined diagnosis terms
            var diagnosisTerms = GetAllCriteriaTerms(paragraph, ModelTermsFactory.CombinedDiagnosisCodesID).ToArray();
            var procedureTerms = GetAllCriteriaTerms(paragraph, ModelTermsFactory.ProcedureCodesID).ToArray();

            //The encounter predicate
            //This starts off with any criteria in the paragraph dictated by the Settings + Observation Period terms, if any.
            Expression<Func<pcori.Encounter, bool>> encounterPredicate = GetParagraphEncounterPredicate(paragraph);

            //Return the patient predicate if neither the diagnosis and procedure terms exist in the paragraph, and the encounter predicate is not defined.
            if (!diagnosisTerms.Any() && !procedureTerms.Any() && encounterPredicate == null)
            {
                return patientPredicate;
            }

            #region "Diagnosis"
            //Start processing of Combined Diagnosis term here.
            Expression<Func<pcori.Diagnosis, bool>> diagnosisTermGroupingPredicate = null;
            //each term should be OR'd together, and each code value OR'd within the term
            foreach (var term in diagnosisTerms)
            {
                DTO.Enums.DiagnosisCodeTypes codeType;
                if (!Enum.TryParse<DTO.Enums.DiagnosisCodeTypes>(term.GetStringValue("CodeType"), out codeType))
                {
                    codeType = DTO.Enums.DiagnosisCodeTypes.Any;
                }

                DTO.Enums.TextSearchMethodType searchMethod;
                if (!Enum.TryParse<DTO.Enums.TextSearchMethodType>(term.GetStringValue("SearchMethodType"), out searchMethod))
                {
                    searchMethod = DTO.Enums.TextSearchMethodType.ExactMatch;
                }

                var codes = (term.GetStringValue("CodeValues") ?? "").Split(new[] { ';' }).Where(x => !string.IsNullOrEmpty(x.Trim())).Select(s => s.Trim()).Distinct().ToArray();
                if (codes.Length == 0)
                    continue;

                Expression<Func<pcori.Diagnosis, bool>> codeTypePredicate;
                if (codeType == DTO.Enums.DiagnosisCodeTypes.Any)
                {
                    codeTypePredicate = (d) => true;
                }
                else
                {
                    string translatedCode = Terms.CombinedDiagnosisCodes.FromDiagnosisCodeType(codeType);
                    codeTypePredicate = (d) => d.CodeType != null && d.CodeType == translatedCode;
                }


                //limit to the encounters where the patient age falls within the specified range.
                DTO.Enums.AgeRangeCalculationType[] calculationTypes = new[] {
                    DTO.Enums.AgeRangeCalculationType.AtFirstMatchingEncounterWithinCriteriaGroup,
                    DTO.Enums.AgeRangeCalculationType.AtLastMatchingEncounterWithinCriteriaGroup
                };

                AgeRangeValues[] ageRanges = AdapterHelpers.ParseAgeRangeValues(GetAllCriteriaTerms(paragraph, ModelTermsFactory.AgeRangeID), calculationTypes).ToArray();

                if (ageRanges.Length > 0)
                {
                    Expression<Func<pcori.Diagnosis, bool>> ageGroupingPredicate = (d) => false;

                    foreach (var range in ageRanges.Where(r => r.MinAge.HasValue || r.MaxAge.HasValue))
                    {
                        Expression<Func<pcori.Diagnosis, bool>> ageRangePredicate = (d) => true;
                        if (range.MinAge.HasValue)
                        {
                            int minAge = range.MinAge.Value;
                            if (_sqlProvider != Settings.SQLProvider.Oracle)
                            {
                                ageRangePredicate = ageRangePredicate.And(d => !string.IsNullOrEmpty(d.EncounterID) && minAge <= ((d.Patient.BornOn > d.Encounter.AdmittedOn) ?
                                    (DbFunctions.DiffYears(d.Patient.BornOn.Value, d.Encounter.AdmittedOn).Value + ((d.Patient.BornOn.Value.Month < d.Encounter.AdmittedOn.Month || (d.Patient.BornOn.Value.Month == d.Encounter.AdmittedOn.Month && d.Patient.BornOn.Value.Day < d.Encounter.AdmittedOn.Day)) ? 1 : 0))
                                    :
                                    (DbFunctions.DiffYears(d.Patient.BornOn, d.Encounter.AdmittedOn).Value - (((d.Patient.BornOn.Value.Month > d.Encounter.AdmittedOn.Month) || (d.Patient.BornOn.Value.Month == d.Encounter.AdmittedOn.Month && d.Patient.BornOn.Value.Day > d.Encounter.AdmittedOn.Day)) ? 1 : 0))));
                            }
                            else
                            {
                                ageRangePredicate = ageRangePredicate.And(d => !string.IsNullOrEmpty(d.EncounterID) && minAge <= ((d.Patient.BornOn.Value > d.Encounter.AdmittedOn) ?
                                    (d.Encounter.AdmittedOn.Year - d.Patient.BornOn.Value.Year + ((d.Patient.BornOn.Value.Month < d.Encounter.AdmittedOn.Month || (d.Patient.BornOn.Value.Month == d.Encounter.AdmittedOn.Month && d.Patient.BornOn.Value.Day < d.Encounter.AdmittedOn.Day)) ? 1 : 0))
                                    :
                                    (d.Encounter.AdmittedOn.Year - d.Patient.BornOn.Value.Year - (d.Patient.BornOn.Value.Month >= d.Encounter.AdmittedOn.Month && (d.Patient.BornOn.Value.Month > d.Encounter.AdmittedOn.Month || (d.Patient.BornOn.Value.Month == d.Encounter.AdmittedOn.Month && d.Patient.BornOn.Value.Day > d.Encounter.AdmittedOn.Day)) ? 1 : 0))));
                            }
                        }
                        if (range.MinAge.HasValue)
                        {
                            int maxAge = range.MaxAge.Value;
                            if (_sqlProvider != Settings.SQLProvider.Oracle)
                            {
                                ageRangePredicate = ageRangePredicate.And(d => !string.IsNullOrEmpty(d.EncounterID) && ((d.Patient.BornOn > d.Encounter.AdmittedOn) ?
                                    (DbFunctions.DiffYears(d.Patient.BornOn.Value, d.Encounter.AdmittedOn).Value + ((d.Patient.BornOn.Value.Month < d.Encounter.AdmittedOn.Month || (d.Patient.BornOn.Value.Month == d.Encounter.AdmittedOn.Month && d.Patient.BornOn.Value.Day < d.Encounter.AdmittedOn.Day)) ? 1 : 0))
                                    :
                                    (DbFunctions.DiffYears(d.Patient.BornOn, d.Encounter.AdmittedOn).Value - (((d.Patient.BornOn.Value.Month > d.Encounter.AdmittedOn.Month) || (d.Patient.BornOn.Value.Month == d.Encounter.AdmittedOn.Month && d.Patient.BornOn.Value.Day > d.Encounter.AdmittedOn.Day)) ? 1 : 0))) <= maxAge);
                            }
                            else
                            {
                                ageRangePredicate = ageRangePredicate.And(d => !string.IsNullOrEmpty(d.EncounterID) && ((d.Patient.BornOn.Value > d.Encounter.AdmittedOn) ?
                                    (d.Encounter.AdmittedOn.Year - d.Patient.BornOn.Value.Year + ((d.Patient.BornOn.Value.Month < d.Encounter.AdmittedOn.Month || (d.Patient.BornOn.Value.Month == d.Encounter.AdmittedOn.Month && d.Patient.BornOn.Value.Day < d.Encounter.AdmittedOn.Day)) ? 1 : 0))
                                    :
                                    (d.Encounter.AdmittedOn.Year - d.Patient.BornOn.Value.Year - (d.Patient.BornOn.Value.Month >= d.Encounter.AdmittedOn.Month && (d.Patient.BornOn.Value.Month > d.Encounter.AdmittedOn.Month || (d.Patient.BornOn.Value.Month == d.Encounter.AdmittedOn.Month && d.Patient.BornOn.Value.Day > d.Encounter.AdmittedOn.Day)) ? 1 : 0))) <= maxAge);
                            }
                        }

                        ageGroupingPredicate = ageGroupingPredicate.Or(ageRangePredicate);
                    }

                    codeTypePredicate = codeTypePredicate.And(ageGroupingPredicate);
                }


                Expression<Func<pcori.Diagnosis, bool>> valuesPredicate = null;

                if (searchMethod == DTO.Enums.TextSearchMethodType.ExactMatch)
                {
                    if (codes.Length == 1)
                    {
                        string codeValue = codes[0];
                        valuesPredicate = d => d.Code != null && d.Code == codeValue;
                    }
                    else
                    {
                        valuesPredicate = d => d.Code != null && codes.Contains(d.Code);
                    }
                }
                else if (searchMethod == DTO.Enums.TextSearchMethodType.StartsWith)
                {
                    string value = codes[0];
                    valuesPredicate = d => d.Code.StartsWith(value);
                    for (int i = 1; i < codes.Length; i++)
                    {
                        string valueinner = codes[i];
                        valuesPredicate = valuesPredicate.Or(d => d.Code.StartsWith(valueinner));
                    }
                    valuesPredicate = valuesPredicate.And(d => d.Code != null);
                }
                else
                {
                    throw new NotSupportedException("The search method type '" + searchMethod + "' is not supported.");
                }

                codeTypePredicate = codeTypePredicate.And(valuesPredicate);

                //if there are more than one term they need to be OR'd not AND'd, the grouping will then be AND'd against the other terms
                if (diagnosisTermGroupingPredicate == null)
                {
                    diagnosisTermGroupingPredicate = codeTypePredicate;
                }
                else
                {
                    diagnosisTermGroupingPredicate = diagnosisTermGroupingPredicate.Or(codeTypePredicate);
                }
            }
            #endregion

            #region "Procedures"
            //Now process the Procedure terms.
            Expression<Func<pcori.Procedure, bool>> procedureTermGroupingPredicate = null;
            //each term should be OR'd together, and each code value OR'd within the term
            foreach (var term in procedureTerms)
            {
                DTO.Enums.ProcedureCode codeType;
                if (!Enum.TryParse<DTO.Enums.ProcedureCode>(term.GetStringValue("CodeType"), out codeType))
                {
                    codeType = DTO.Enums.ProcedureCode.Any;
                }

                DTO.Enums.TextSearchMethodType searchMethod;
                if (!Enum.TryParse<DTO.Enums.TextSearchMethodType>(term.GetStringValue("SearchMethodType"), out searchMethod))
                {
                    searchMethod = DTO.Enums.TextSearchMethodType.ExactMatch;
                }

                var codes = (term.GetStringValue("CodeValues") ?? "").Split(new[] { ';' }).Where(x => !string.IsNullOrEmpty(x.Trim())).Select(s => s.Trim()).Distinct().ToArray();
                if (codes.Length == 0)
                    continue;

                Expression<Func<pcori.Procedure, bool>> codeTypePredicate;
                if (codeType == DTO.Enums.ProcedureCode.Any)
                {
                    codeTypePredicate = (d) => d.CodeType != null;
                }
                else
                {
                    IEnumerable<string> translatedCode = Terms.ProcedureCodes.FromProcedureCodeType(codeType);
                    codeTypePredicate = (d) => d.CodeType != null && translatedCode.Contains(d.CodeType);
                }


                //limit to the encounters where the patient age falls within the specified range.
                DTO.Enums.AgeRangeCalculationType[] calculationTypes = new[] {
                    DTO.Enums.AgeRangeCalculationType.AtFirstMatchingEncounterWithinCriteriaGroup,
                    DTO.Enums.AgeRangeCalculationType.AtLastMatchingEncounterWithinCriteriaGroup
                };

                AgeRangeValues[] ageRanges = AdapterHelpers.ParseAgeRangeValues(GetAllCriteriaTerms(paragraph, ModelTermsFactory.AgeRangeID), calculationTypes).ToArray();

                if (ageRanges.Length > 0)
                {
                    Expression<Func<pcori.Procedure, bool>> ageGroupingPredicate = (d) => false;

                    foreach (var range in ageRanges.Where(r => r.MinAge.HasValue || r.MaxAge.HasValue))
                    {
                        Expression<Func<pcori.Procedure, bool>> ageRangePredicate = (d) => true;
                        if (range.MinAge.HasValue)
                        {
                            int minAge = range.MinAge.Value;
                            if (_sqlProvider != Settings.SQLProvider.Oracle)
                            {
                                ageRangePredicate = ageRangePredicate.And(d => !string.IsNullOrEmpty(d.EncounterID) && minAge <= ((d.Patient.BornOn > d.Encounter.AdmittedOn) ?
                                    (DbFunctions.DiffYears(d.Patient.BornOn.Value, d.Encounter.AdmittedOn).Value + ((d.Patient.BornOn.Value.Month < d.Encounter.AdmittedOn.Month || (d.Patient.BornOn.Value.Month == d.Encounter.AdmittedOn.Month && d.Patient.BornOn.Value.Day < d.Encounter.AdmittedOn.Day)) ? 1 : 0))
                                    :
                                    (DbFunctions.DiffYears(d.Patient.BornOn, d.Encounter.AdmittedOn).Value - (((d.Patient.BornOn.Value.Month > d.Encounter.AdmittedOn.Month) || (d.Patient.BornOn.Value.Month == d.Encounter.AdmittedOn.Month && d.Patient.BornOn.Value.Day > d.Encounter.AdmittedOn.Day)) ? 1 : 0))));
                            }
                            else
                            {
                                ageRangePredicate = ageRangePredicate.And(d => !string.IsNullOrEmpty(d.EncounterID) && minAge <= ((d.Patient.BornOn.Value > d.Encounter.AdmittedOn) ?
                                    (d.Encounter.AdmittedOn.Year - d.Patient.BornOn.Value.Year + ((d.Patient.BornOn.Value.Month < d.Encounter.AdmittedOn.Month || (d.Patient.BornOn.Value.Month == d.Encounter.AdmittedOn.Month && d.Patient.BornOn.Value.Day < d.Encounter.AdmittedOn.Day)) ? 1 : 0))
                                    :
                                    (d.Encounter.AdmittedOn.Year - d.Patient.BornOn.Value.Year - (d.Patient.BornOn.Value.Month >= d.Encounter.AdmittedOn.Month && (d.Patient.BornOn.Value.Month > d.Encounter.AdmittedOn.Month || (d.Patient.BornOn.Value.Month == d.Encounter.AdmittedOn.Month && d.Patient.BornOn.Value.Day > d.Encounter.AdmittedOn.Day)) ? 1 : 0))));
                            }
                        }
                        if (range.MinAge.HasValue)
                        {
                            int maxAge = range.MaxAge.Value;
                            if (_sqlProvider != Settings.SQLProvider.Oracle)
                            {
                                ageRangePredicate = ageRangePredicate.And(d => !string.IsNullOrEmpty(d.EncounterID) && ((d.Patient.BornOn > d.Encounter.AdmittedOn) ?
                                    (DbFunctions.DiffYears(d.Patient.BornOn.Value, d.Encounter.AdmittedOn).Value + ((d.Patient.BornOn.Value.Month < d.Encounter.AdmittedOn.Month || (d.Patient.BornOn.Value.Month == d.Encounter.AdmittedOn.Month && d.Patient.BornOn.Value.Day < d.Encounter.AdmittedOn.Day)) ? 1 : 0))
                                    :
                                    (DbFunctions.DiffYears(d.Patient.BornOn, d.Encounter.AdmittedOn).Value - (((d.Patient.BornOn.Value.Month > d.Encounter.AdmittedOn.Month) || (d.Patient.BornOn.Value.Month == d.Encounter.AdmittedOn.Month && d.Patient.BornOn.Value.Day > d.Encounter.AdmittedOn.Day)) ? 1 : 0))) <= maxAge);
                            }
                            else
                            {
                                ageRangePredicate = ageRangePredicate.And(d => !string.IsNullOrEmpty(d.EncounterID) && ((d.Patient.BornOn.Value > d.Encounter.AdmittedOn) ?
                                    (d.Encounter.AdmittedOn.Year - d.Patient.BornOn.Value.Year + ((d.Patient.BornOn.Value.Month < d.Encounter.AdmittedOn.Month || (d.Patient.BornOn.Value.Month == d.Encounter.AdmittedOn.Month && d.Patient.BornOn.Value.Day < d.Encounter.AdmittedOn.Day)) ? 1 : 0))
                                    :
                                    (d.Encounter.AdmittedOn.Year - d.Patient.BornOn.Value.Year - (d.Patient.BornOn.Value.Month >= d.Encounter.AdmittedOn.Month && (d.Patient.BornOn.Value.Month > d.Encounter.AdmittedOn.Month || (d.Patient.BornOn.Value.Month == d.Encounter.AdmittedOn.Month && d.Patient.BornOn.Value.Day > d.Encounter.AdmittedOn.Day)) ? 1 : 0))) <= maxAge);
                            }
                        }

                        ageGroupingPredicate = ageGroupingPredicate.Or(ageRangePredicate);
                    }

                    codeTypePredicate = codeTypePredicate.And(ageGroupingPredicate);
                }


                Expression<Func<pcori.Procedure, bool>> valuesPredicate = null;

                if (searchMethod == DTO.Enums.TextSearchMethodType.ExactMatch)
                {
                    if (codes.Length == 1)
                    {
                        string codeValue = codes[0];
                        valuesPredicate = d => d.Code != null && d.Code == codeValue;
                    }
                    else
                    {
                        valuesPredicate = d => d.Code != null && codes.Contains(d.Code);
                    }
                }
                else if (searchMethod == DTO.Enums.TextSearchMethodType.StartsWith)
                {
                    string value = codes[0];
                    valuesPredicate = d => d.Code.StartsWith(value);
                    for (int i = 1; i < codes.Length; i++)
                    {
                        string valueinner = codes[i];
                        valuesPredicate = valuesPredicate.Or(d => d.Code.StartsWith(valueinner));
                    }
                    valuesPredicate = valuesPredicate.And(d => d.Code != null);
                }
                else
                {
                    throw new NotSupportedException("The search method type '" + searchMethod + "' is not supported.");
                }

                codeTypePredicate = codeTypePredicate.And(valuesPredicate);

                //if there are more than one term they need to be OR'd not AND'd, the grouping will then be AND'd against the other terms
                if (procedureTermGroupingPredicate == null)
                {
                    procedureTermGroupingPredicate = codeTypePredicate;
                }
                else
                {
                    procedureTermGroupingPredicate = procedureTermGroupingPredicate.Or(codeTypePredicate);
                }
            }
            #endregion

            //PMNDEV-6287: Previously diagnosis and procedures were being mapped through Encounters, which was performing a lateral inline view in Oracle.
            //Since Oracle 11 doesn't support lateral inline views or cross joins, we had to update the implementation below to apply the encounter
            //predicate separately to both procedures and diagnosis.
            if (diagnosisTermGroupingPredicate != null && procedureTermGroupingPredicate != null)
            {
                //Combine and OR the diagnosis and procedure predicates, as per PMNDEV-6184
                if (encounterPredicate == null)
                {
                    return patientPredicate.And((p) => p.Diagnoses.AsQueryable().Any(diagnosisTermGroupingPredicate) || p.Procedures.AsQueryable().Any(procedureTermGroupingPredicate));
                }
                else
                {
                    diagnosisTermGroupingPredicate = diagnosisTermGroupingPredicate.And(p => encounterPredicate.Invoke(p.Encounter));
                    procedureTermGroupingPredicate = procedureTermGroupingPredicate.And(p => encounterPredicate.Invoke(p.Encounter));
                    return patientPredicate.And((p) =>
                        p.Diagnoses.AsQueryable().Any(diagnosisTermGroupingPredicate)
                        ||
                        p.Procedures.AsQueryable().Any(procedureTermGroupingPredicate)
                        );
                }
            }
            else if (diagnosisTermGroupingPredicate != null)
            {
                if (encounterPredicate == null)
                {
                    return patientPredicate.And((p) => p.Diagnoses.AsQueryable().Any(diagnosisTermGroupingPredicate));
                }
                else
                {
                    diagnosisTermGroupingPredicate = diagnosisTermGroupingPredicate.And(p => encounterPredicate.Invoke(p.Encounter));
                    return patientPredicate.And((p) =>
                        p.Diagnoses.AsQueryable().Any(diagnosisTermGroupingPredicate)
                        );

                }
            }
            else if (procedureTermGroupingPredicate != null)
            {
                if (encounterPredicate == null)
                {
                    return patientPredicate.And((p) => p.Procedures.AsQueryable().Any(procedureTermGroupingPredicate));
                }
                else
                {
                    procedureTermGroupingPredicate = procedureTermGroupingPredicate.And(p => encounterPredicate.Invoke(p.Encounter));
                    return patientPredicate.And((p) =>
                        p.Procedures.AsQueryable().Any(procedureTermGroupingPredicate)
                        );
                }
            }
            else if (encounterPredicate == null)
            {
                //No diagnosis or procedure criteria was defined.
                //Check if the encounter predicate was defined. If yes, then apply that.
                return patientPredicate;
            }

            //Procedure & Diagnosis predicate not defined. Apply Encounter predicate.
            return patientPredicate.And((p) => p.Encounters.AsQueryable().Any(encounterPredicate));
        }



        Expression<Func<pcori.ClinicalTrial, bool>> ApplyClinicalTrialCriteria(IEnumerable<QueryComposerCriteriaDTO> criteria)
        {
            Expression<Func<pcori.ClinicalTrial, bool>> queryPredicate = null;
            foreach (var paragraph in criteria)
            {
                var paragraphPredicate = ParseParagraphForClinicalTrial(paragraph, queryPredicate);

                if(queryPredicate == null)
                {
                    if (paragraph.Exclusion)
                    {
                        queryPredicate = PredicateHelper.Negate(paragraphPredicate).Expand();
                    }
                    else
                    {
                        queryPredicate = paragraphPredicate;
                    }
                }
                else
                {
                    var conjunction = paragraph.Operator;
                    if(conjunction == DTO.Enums.QueryComposerOperators.And && paragraph.Exclusion == false)
                    {
                        queryPredicate = queryPredicate.Expand().And(first => (from tr in db.ClinicalTrials.AsExpandable()
                                                                              where paragraphPredicate.Invoke(tr)
                                                                              select tr.TrialID).Contains(first.TrialID));

                    }else if(conjunction == DTO.Enums.QueryComposerOperators.AndNot || (conjunction == DTO.Enums.QueryComposerOperators.And && paragraph.Exclusion))
                    {

                        queryPredicate = queryPredicate.Expand().And(first => (from tr in db.ClinicalTrials.AsExpandable()
                                                                               where paragraphPredicate.Invoke(tr)
                                                                               select tr.TrialID).Contains(first.TrialID) == false);

                    }else if(conjunction == DTO.Enums.QueryComposerOperators.Or && paragraph.Exclusion == false)
                    {
                        queryPredicate = queryPredicate.Expand().Or(paragraphPredicate.Expand());
                    }
                    else
                    {
                        queryPredicate = queryPredicate.Expand().Or(PredicateHelper.Negate(paragraphPredicate.Expand()));
                    }
                }

            }

            return queryPredicate;
        }


        Expression<Func<pcori.ClinicalTrial, bool>> ParseParagraphForClinicalTrial(QueryComposerCriteriaDTO paragraph, Expression<Func<pcori.ClinicalTrial,bool>> predicate)
        {
            var terms = GetAllCriteriaTerms(paragraph, ModelTermsFactory.TrialID).ToArray();
            if (!terms.Any())
                return predicate;

            Expression<Func<pcori.ClinicalTrial, bool>> trialIDPredicate = null;
            foreach (var term in terms)
            {
                string trialID = term.GetStringValue("Trial");
                if (!string.IsNullOrWhiteSpace(trialID))
                {
                    if (trialIDPredicate == null)
                    {
                        trialIDPredicate = (tr) => tr.TrialID == trialID;
                    }
                    else
                    {
                        trialIDPredicate = trialIDPredicate.Or((tr) => tr.TrialID == trialID);
                    }
                }
            }

            return trialIDPredicate == null ? predicate : (predicate == null ? trialIDPredicate : predicate.And(trialIDPredicate.Expand()));
        }

        Expression<Func<pcori.ReportedOutcome, bool>> ApplyPatientReportedOutcomeCriteria(IEnumerable<QueryComposerCriteriaDTO> criteria)
        {
            Expression<Func<pcori.ReportedOutcome, bool>> queryPredicate = null;
            foreach (var paragraph in criteria)
            {
                var paragraphPredicate = ParseParagraphForPatientReportedOutcome(paragraph, queryPredicate);

                if (queryPredicate == null)
                {
                    if (paragraph.Exclusion)
                    {
                        queryPredicate = PredicateHelper.Negate(paragraphPredicate).Expand();
                    }
                    else
                    {
                        queryPredicate = paragraphPredicate;
                    }
                }
                else
                {
                    var conjunction = paragraph.Operator;
                    if (conjunction == DTO.Enums.QueryComposerOperators.And && paragraph.Exclusion == false)
                    {
                        queryPredicate = queryPredicate.Expand().And(first => (from pro in db.ReportedOutcomeCommonMeasures.AsExpandable()
                                                                               where paragraphPredicate.Invoke(pro)
                                                                               select pro.ID).Contains(first.ID));

                    }
                    else if (conjunction == DTO.Enums.QueryComposerOperators.AndNot || (conjunction == DTO.Enums.QueryComposerOperators.And && paragraph.Exclusion))
                    {

                        queryPredicate = queryPredicate.Expand().And(first => (from pro in db.ReportedOutcomeCommonMeasures.AsExpandable()
                                                                               where paragraphPredicate.Invoke(pro)
                                                                               select pro.ID).Contains(first.ID) == false);

                    }
                    else if (conjunction == DTO.Enums.QueryComposerOperators.Or && paragraph.Exclusion == false)
                    {
                        queryPredicate = queryPredicate.Expand().Or(paragraphPredicate.Expand());
                    }
                    else
                    {
                        queryPredicate = queryPredicate.Expand().Or(PredicateHelper.Negate(paragraphPredicate.Expand()));
                    }
                }

            }

            return queryPredicate;
        }

        Expression<Func<pcori.ReportedOutcome, bool>> ParseParagraphForPatientReportedOutcome(QueryComposerCriteriaDTO paragraph, Expression<Func<pcori.ReportedOutcome, bool>> predicate)
        {
            var terms = GetAllCriteriaTerms(paragraph, ModelTermsFactory.PatientReportedOutcomeID).ToArray();
            if (!terms.Any())
                return predicate;

            Expression<Func<pcori.ReportedOutcome, bool>> proPredicate = null;
            foreach (var term in terms)
            {
                string itemName = term.GetStringValue("ItemName");
                string itemText = term.GetStringValue("ItemResponse");

                Expression<Func<pcori.ReportedOutcome, bool>> itemPredictate = null;
                if (!string.IsNullOrEmpty(itemName) && !string.IsNullOrEmpty(itemText))
                {
                    //apply name and response text and'd
                    itemPredictate = (pro) => pro.ItemName == itemName && pro.ResponseText == itemText;

                }
                else if (!string.IsNullOrEmpty(itemName) && string.IsNullOrEmpty(itemText))
                {
                    //apply only itemName
                    itemPredictate = (pro) => pro.ItemName == itemName;

                }
                else if (string.IsNullOrEmpty(itemName) && !string.IsNullOrEmpty(itemText))
                {
                    //apply only response text
                    itemPredictate = (pro) => pro.ResponseText == itemText;
                }

                if (itemPredictate != null)
                {
                    if (proPredicate == null)
                    {
                        proPredicate = itemPredictate;
                    }
                    else
                    {
                        proPredicate = proPredicate.Or(itemPredictate);
                    }
                }
            }

            return proPredicate == null ? predicate : (predicate == null ? proPredicate : predicate.And(proPredicate.Expand()));
        }




    }
}
