using Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model;
using Lpp.Objects.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI
{
    public class TimeWindowQueryBuilder
    {
        readonly PCORIQueryBuilder.DataContext _db;
        readonly IEnumerable<DTO.QueryComposer.QueryComposerTemporalEventDTO> _indexEventCriteria;
        readonly IEnumerable<DTO.QueryComposer.QueryComposerCriteriaDTO> _pro_QueryCriteria;
        static readonly DateTime SAS_Start_Date = new DateTime(1960,1,1);

        /// <summary>
        /// Parameter expression for <see cref="ReportedOutcome"/>
        /// </summary>
        readonly ParameterExpression pe_proType = Expression.Parameter(typeof(ReportedOutcome), "pro");
        /// <summary>
        /// Parameter expression for <see cref="Encounter"/>
        /// </summary>
        readonly ParameterExpression pe_encounters = Expression.Parameter(typeof(Encounter), "enc");
        /// <summary>
        /// Parameter expression for <see cref="Diagnosis"/>
        /// </summary>
        readonly ParameterExpression pe_diagnosis = Expression.Parameter(typeof(Diagnosis), "dia");
        /// <summary>
        /// Parameter expression for <see cref="Procedure"/>
        /// </summary>
        readonly ParameterExpression pe_procedures = Expression.Parameter(typeof(Procedure), "proc");
        /// <summary>
        /// Parameter expression for <see cref="IndexEvent"/>
        /// </summary>
        readonly ParameterExpression pe_indexEvent = Expression.Parameter(typeof(IndexEvent), "indxEvt");
        /// <summary>
        /// Parameter expression for <see cref="EncounterWithDetails"/>
        /// </summary>
        readonly ParameterExpression pe_encounterWithDetails = Expression.Parameter(typeof(EncounterWithDetails), "encDet");

        List<Objects.Dynamic.IPropertyDefinition> _finalSelectTypeProperties = new List<Objects.Dynamic.IPropertyDefinition>();

        public TimeWindowQueryBuilder(PCORIQueryBuilder.DataContext db, IEnumerable<DTO.QueryComposer.QueryComposerTemporalEventDTO> indexEventCriteria, IEnumerable<DTO.QueryComposer.QueryComposerCriteriaDTO> criteria)
        {
            _db = db;
            _indexEventCriteria = indexEventCriteria;
            _pro_QueryCriteria = (criteria ?? Enumerable.Empty<DTO.QueryComposer.QueryComposerCriteriaDTO>()).Where(c => c.Criteria.Any(cc => cc.Terms.Any(tt => tt.Type == Lpp.QueryComposer.ModelTermsFactory.PatientReportedOutcomeID)) || c.Terms.Any(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.PatientReportedOutcomeID));
        }

        /// <summary>
        /// The generates the query expression.
        /// </summary>
        /// <param name="rootQueryExpr">The root patient based query.</param>
        /// <param name="rootQuerySelectType">The select type of the root patient based query.</param>
        /// <returns></returns>
        public Expression Generate(Expression rootQueryExpr, Type rootQuerySelectType)
        {
            var indexEventsExpr = BuildIndexEvents();

            var encountersExpr = BuildEncounters();

            var indexEvt_enc_joinType = typeof(EventWithEncounter);

            var indexEvt_enc_joinBindings = new[] {
                Expression.Bind(indexEvt_enc_joinType.GetProperty("IndexEvent"), pe_indexEvent),
                Expression.Bind(indexEvt_enc_joinType.GetProperty("Encounter"), pe_encounterWithDetails)
            };

            var encountersJoinCall = Expression.Call(
                typeof(Queryable),
                "Join",
                new Type[] {
                    typeof(IndexEvent),
                    typeof(EncounterWithDetails),
                    typeof(string),
                    indexEvt_enc_joinType
                },
                new Expression[] {
                    indexEventsExpr,
                    encountersExpr,
                    Expression.Quote(Expression.Lambda(Expression.Property(pe_indexEvent, "PatientID"), pe_indexEvent)),
                    Expression.Quote(Expression.Lambda(Expression.Property(pe_encounterWithDetails, "PatientID"), pe_encounterWithDetails)),
                    Expression.Quote(Expression.Lambda(
                        Expression.MemberInit(Expression.New(indexEvt_enc_joinType), indexEvt_enc_joinBindings),
                        pe_indexEvent,
                        pe_encounterWithDetails
                        ))
                }
                );

            List<Objects.Dynamic.IPropertyDefinition> joinTypePropertyDefinitions = new List<Objects.Dynamic.IPropertyDefinition>();
            var rootQueryProperties = Objects.Dynamic.TypeBuilderHelper.GeneratePropertyDefinitionsFromType(rootQuerySelectType).ToArray();
            joinTypePropertyDefinitions.AddRange(rootQueryProperties);

            var joinType = Objects.Dynamic.TypeBuilderHelper.CreateType("j1", joinTypePropertyDefinitions, indexEvt_enc_joinType, null);
            var pe_indexEvt_enc_joinType = Expression.Parameter(indexEvt_enc_joinType);
            
            var joinBindings = new List<MemberAssignment> {
                Expression.Bind(joinType.GetProperty("IndexEvent"), Expression.Property(pe_indexEvt_enc_joinType, pe_indexEvt_enc_joinType.Type.GetProperty("IndexEvent"))),
                Expression.Bind(joinType.GetProperty("Encounter"), Expression.Property(pe_indexEvt_enc_joinType, pe_indexEvt_enc_joinType.Type.GetProperty("Encounter")))
            };

            var pe_rootQuerySelectType = Expression.Parameter(rootQuerySelectType);

            foreach(var prop in rootQueryProperties)
            {
                joinBindings.Add(Expression.Bind(joinType.GetProperty(prop.Name), Expression.Property(pe_rootQuerySelectType, rootQuerySelectType.GetProperty(prop.Name))));
            }

            var resultSelector = Expression.Lambda(
                    Expression.MemberInit(Expression.New(joinType), joinBindings),
                    pe_indexEvt_enc_joinType,
                    pe_rootQuerySelectType
                );

            var joinCall = Expression.Call(
                typeof(Queryable),
                "Join",
                new Type[] {
                    encountersJoinCall.Type.GetGenericArguments()[0],
                    rootQueryExpr.Type.GetGenericArguments()[0],
                    typeof(string),
                    joinType
                },
                new[] {
                    encountersJoinCall,
                    rootQueryExpr,
                    Expression.Quote(Expression.Lambda(Expression.Property(Expression.Property(pe_indexEvt_enc_joinType, pe_indexEvt_enc_joinType.Type.GetProperty("IndexEvent")), typeof(IndexEvent).GetProperty("PatientID")), pe_indexEvt_enc_joinType)),
                    Expression.Quote(Expression.Lambda(Expression.Property(pe_rootQuerySelectType, rootQuerySelectType.GetProperty("PatientID")), pe_rootQuerySelectType)),
                    Expression.Quote(resultSelector)
                }
                );

            var pe_joinResultType = Expression.Parameter(joinType, "f1");
            var encounterSASDateBeforeTimeWindowPredicateExpr = Expression.GreaterThanOrEqual(Expression.Convert(Expression.Property(Expression.Property(pe_joinResultType, pe_joinResultType.Type.GetProperty("Encounter")), typeof(EncounterWithDetails).GetProperty("EncounterSASDate")), typeof(float?)), Expression.Convert(Expression.Property(Expression.Property(pe_joinResultType, pe_joinResultType.Type.GetProperty("IndexEvent")), typeof(IndexEvent).GetProperty("TimeWindowBefore")), typeof(float?)));
            var encounterSASDateAfterTimeWindowPredicateExpr = Expression.LessThanOrEqual(Expression.Convert(Expression.Property(Expression.Property(pe_joinResultType, pe_joinResultType.Type.GetProperty("Encounter")), typeof(EncounterWithDetails).GetProperty("EncounterSASDate")), typeof(float?)), Expression.Convert(Expression.Property(Expression.Property(pe_joinResultType, pe_joinResultType.Type.GetProperty("IndexEvent")), typeof(IndexEvent).GetProperty("TimeWindowAfter")), typeof(float?)));
            var wherePredicateExpr = Expression.AndAlso(encounterSASDateBeforeTimeWindowPredicateExpr, encounterSASDateAfterTimeWindowPredicateExpr);

            //only Diagnosis and Procedure codes will be used as criteria against the index events, all terms are OR'd, there will only be a single root criteria
            var terms = _indexEventCriteria.SelectMany(tevt => tevt.Criteria.SelectMany(rootCriteria => PCORIModelAdapter.GetAllCriteriaTerms(rootCriteria, new Guid[] { Lpp.QueryComposer.ModelTermsFactory.CombinedDiagnosisCodesID, Lpp.QueryComposer.ModelTermsFactory.ProcedureCodesID })));
            if (terms.Any())
            {
                bool isExclusion = _indexEventCriteria.Select(tevt => tevt.Criteria.Select(c => c.Exclusion).FirstOrDefault()).FirstOrDefault();

                List<Expression> predicateExpressions = new List<Expression>();
                foreach(var codeTerm in terms)
                {
                    var codes = (codeTerm.GetStringValue("CodeValues") ?? "").Split(new[] { ';' }).Where(x => !string.IsNullOrEmpty(x.Trim())).Select(s => s.Trim()).Distinct().ToArray();
                    if (codes.Length == 0)
                        continue;

                    DTO.Enums.TextSearchMethodType searchMethod;
                    if (!Enum.TryParse<DTO.Enums.TextSearchMethodType>(codeTerm.GetStringValue("SearchMethodType"), out searchMethod))
                    {
                        searchMethod = DTO.Enums.TextSearchMethodType.ExactMatch;
                    }

                    if (codeTerm.Type == Lpp.QueryComposer.ModelTermsFactory.CombinedDiagnosisCodesID)
                    {
                        DTO.Enums.DiagnosisCodeTypes codeType;
                        if (!Enum.TryParse<DTO.Enums.DiagnosisCodeTypes>(codeTerm.GetStringValue("CodeType"), out codeType))
                        {
                            codeType = DTO.Enums.DiagnosisCodeTypes.Any;
                        }                        

                        Expression diagPredicateExpression;

                        if(codeType != DTO.Enums.DiagnosisCodeTypes.Any)
                        {
                            diagPredicateExpression = Expression.AndAlso(
                                Expression.NotEqual(Expressions.ChildPropertyExpression(pe_joinResultType, "Encounter", "DiagnosisCodeType"), Expression.Constant(null)),
                                Expression.Equal(Expressions.ChildPropertyExpression(pe_joinResultType, "Encounter", "DiagnosisCodeType"), Expression.Constant(Terms.CombinedDiagnosisCodes.FromDiagnosisCodeType(codeType)))
                                );
                        }
                        else
                        {
                            diagPredicateExpression = Expression.NotEqual(Expressions.ChildPropertyExpression(pe_joinResultType, "Encounter", "DiagnosisCodeType"), Expression.Constant(null));
                        }

                        Expression codesPredicate = null;
                        if(searchMethod == DTO.Enums.TextSearchMethodType.ExactMatch)
                        {
                            if(codes.Length == 1)
                            {
                                string codeValue = codes[0];
                                codesPredicate = Expression.Equal(Expressions.ChildPropertyExpression(pe_joinResultType, "Encounter", "DiagnosisCode"), Expression.Constant(codeValue));
                            }
                            else
                            {
                                codesPredicate = Expressions.Contains<string>(codes.AsQueryable(), Expressions.ChildPropertyExpression(pe_joinResultType, "Encounter", "DiagnosisCode"));
                            }


                        }else if(searchMethod == DTO.Enums.TextSearchMethodType.StartsWith)
                        {
                            string codeValue = codes[0];
                            codesPredicate = Expressions.StringStartsWith(Expressions.ChildPropertyExpression(pe_joinResultType, "Encounter", "DiagnosisCode"), codeValue);
                            for (int i = 1; i < codes.Length; i++)
                            {
                                string valueinner = codes[i];
                                codesPredicate = Expression.Or(codesPredicate, Expressions.StringStartsWith(Expressions.ChildPropertyExpression(pe_joinResultType, "Encounter", "DiagnosisCode"), valueinner));
                            }
                        }
                        else
                        {
                            throw new NotSupportedException("The search method type '" + searchMethod + "' is not supported.");
                        }

                        diagPredicateExpression = Expression.AndAlso(diagPredicateExpression, codesPredicate);
                        predicateExpressions.Add(diagPredicateExpression);
                    }
                    else if(codeTerm.Type == Lpp.QueryComposer.ModelTermsFactory.ProcedureCodesID)
                    {
                        DTO.Enums.ProcedureCode codeType;
                        if (!Enum.TryParse<DTO.Enums.ProcedureCode>(codeTerm.GetStringValue("CodeType"), out codeType))
                        {
                            codeType = DTO.Enums.ProcedureCode.Any;
                        }

                        Expression procedurePredicateExpression;

                        if (codeType != DTO.Enums.ProcedureCode.Any)
                        {
                            var codeTypeValues = Terms.ProcedureCodes.FromProcedureCodeType(codeType);

                            procedurePredicateExpression = Expression.AndAlso(
                                Expression.NotEqual(Expressions.ChildPropertyExpression(pe_joinResultType, "Encounter", "ProcedureCodeType"), Expression.Constant(null)),
                                codeTypeValues.Count() > 1 ?
                                    (Expression)Expressions.Contains<string>(codeTypeValues.AsQueryable(), Expressions.ChildPropertyExpression(pe_joinResultType, "Encounter", "ProcedureCodeType")) :
                                    Expression.Equal(Expressions.ChildPropertyExpression(pe_joinResultType, "Encounter", "ProcedureCodeType"), Expression.Constant(((string[])codeTypeValues)[0]))
                                );
                        }
                        else
                        {
                            procedurePredicateExpression = Expression.NotEqual(Expressions.ChildPropertyExpression(pe_joinResultType, "Encounter", "ProcedureCodeType"), Expression.Constant(null));                            
                        }

                        Expression codesPredicate = null;
                        if (searchMethod == DTO.Enums.TextSearchMethodType.ExactMatch)
                        {
                            if (codes.Length == 1)
                            {
                                string codeValue = codes[0];
                                codesPredicate = Expression.Equal(Expressions.ChildPropertyExpression(pe_joinResultType, "Encounter", "ProcedureCode"), Expression.Constant(codeValue));
                            }
                            else
                            {
                                codesPredicate = Expressions.Contains<string>(codes.AsQueryable(), Expressions.ChildPropertyExpression(pe_joinResultType, "Encounter", "ProcedureCode"));
                            }


                        }
                        else if (searchMethod == DTO.Enums.TextSearchMethodType.StartsWith)
                        {
                            string codeValue = codes[0];
                            codesPredicate = Expressions.StringStartsWith(Expressions.ChildPropertyExpression(pe_joinResultType, "Encounter", "ProcedureCode"), codeValue);
                            for (int i = 1; i < codes.Length; i++)
                            {
                                string valueinner = codes[i];
                                codesPredicate = Expression.Or(codesPredicate, Expressions.StringStartsWith(Expressions.ChildPropertyExpression(pe_joinResultType, "Encounter", "ProcedureCode"), valueinner));
                            }
                        }
                        else
                        {
                            throw new NotSupportedException("The search method type '" + searchMethod + "' is not supported.");
                        }

                        procedurePredicateExpression = Expression.AndAlso(procedurePredicateExpression, codesPredicate);
                        predicateExpressions.Add(procedurePredicateExpression);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("term.Type", "Invalid type of term to use as criteria for Index Events in a time window query. Type ID: " + codeTerm.Type.ToString("D"));
                    }
                }


                if(predicateExpressions.Count == 1)
                {
                    wherePredicateExpr = Expression.AndAlso(wherePredicateExpr, isExclusion ? Expression.Not(predicateExpressions[0]) : predicateExpressions[0]);
                }
                else
                {
                    var orPredicate = predicateExpressions[0];
                    for(int i = 1; i < predicateExpressions.Count; i++)
                    {
                        orPredicate = Expression.Or(orPredicate, predicateExpressions[i]);
                    }

                    wherePredicateExpr = Expression.AndAlso(wherePredicateExpr, isExclusion ? Expression.Not(orPredicate) : orPredicate);
                }


            }


            var whereCall = Expression.Call(
                typeof(Queryable),
                "Where",
                new Type[] { joinType },
                joinCall,
                Expression.Quote(Expression.Lambda(wherePredicateExpr, pe_joinResultType))
                );

            //if there are PRO criteria, need to union against index events
            if (_pro_QueryCriteria.Any())
            {
                var nonIndexEvents = BuildNonIndexEventsQuery(rootQueryExpr, pe_rootQuerySelectType, pe_indexEvt_enc_joinType, joinType, joinBindings);

                var pe_nonIndexEvents = Expression.Parameter(joinType, "nonIndexEvents");

                //limit the secondary events to the same patient cohort as returned for the index events
                var whereNonIndexCall = Expression.Call(
                    typeof(Queryable),
                    "Where",
                    new Type[] { joinType },
                    nonIndexEvents,
                    Expression.Quote(
                        Expression.Lambda(
                        Expression.Call(
                           typeof(Enumerable), nameof(Enumerable.Any), new Type[] { joinType },
                               whereCall,
                               Expression.Lambda(Expression.Equal(Expression.Property(pe_nonIndexEvents, "PatientID"), Expression.Property(pe_joinResultType, "PatientID")), pe_joinResultType)
                            ) 
                        ,pe_nonIndexEvents)                        
                        )
                    
                    );

                var unionCall = Expression.Call(
                    typeof(Queryable),
                    "Union",
                    new Type[] { joinType },
                    whereCall,
                    whereNonIndexCall
                    );

                whereCall = unionCall;
            }
            

            //shape the result to the base select, and the fields required by the PRO encounters term

            _finalSelectTypeProperties = new List<Objects.Dynamic.IPropertyDefinition> {
                new TypedPropertyDefinition<string>("ENCOUNTERID"),
                new TypedPropertyDefinition<string>("ENC_TYPE"),
                new TypedPropertyDefinition<DateTime?>("ADMIT_DATE"),
                new TypedPropertyDefinition<string>("DX"),
                new TypedPropertyDefinition<string>("DX_TYPE"),
                new TypedPropertyDefinition<string>("PX"),
                new TypedPropertyDefinition<string>("PX_TYPE"),
                new TypedPropertyDefinition<string>("PRO_ITEM_NAME"),
                new TypedPropertyDefinition<string>("PRO_RESPONSE_TEXT"),
                new TypedPropertyDefinition<float?>("PRO_RESPONSE_NUM"),
                new TypedPropertyDefinition<string>("PRO_MEASURE_SEQ")
            };

            _finalSelectTypeProperties.AddRange(rootQueryProperties.Where(p => p.Name.Contains("PatientID") == false));

            var finalSelectType = Objects.Dynamic.TypeBuilderHelper.CreateType("PatientReportedEncounters", _finalSelectTypeProperties);

            var finalSelectBindings = new List<MemberAssignment>
            {
                Expression.Bind(finalSelectType.GetProperty("ENCOUNTERID"), Expression.Property(Expression.Property(pe_joinResultType, pe_joinResultType.Type.GetProperty("Encounter")), typeof(EncounterWithDetails).GetProperty("EncounterID"))),
                Expression.Bind(finalSelectType.GetProperty("ENC_TYPE"), Expression.Property(Expression.Property(pe_joinResultType, pe_joinResultType.Type.GetProperty("Encounter")), typeof(EncounterWithDetails).GetProperty("EncounterType"))),
                Expression.Bind(finalSelectType.GetProperty("ADMIT_DATE"), Expression.Property(Expression.Property(pe_joinResultType, pe_joinResultType.Type.GetProperty("Encounter")), typeof(EncounterWithDetails).GetProperty("AdmitDate"))),
                Expression.Bind(finalSelectType.GetProperty("DX"), Expression.Property(Expression.Property(pe_joinResultType, pe_joinResultType.Type.GetProperty("Encounter")), typeof(EncounterWithDetails).GetProperty("DiagnosisCode"))),
                Expression.Bind(finalSelectType.GetProperty("DX_TYPE"), Expression.Property(Expression.Property(pe_joinResultType, pe_joinResultType.Type.GetProperty("Encounter")), typeof(EncounterWithDetails).GetProperty("DiagnosisCodeType"))),
                Expression.Bind(finalSelectType.GetProperty("PX"), Expression.Property(Expression.Property(pe_joinResultType, pe_joinResultType.Type.GetProperty("Encounter")), typeof(EncounterWithDetails).GetProperty("ProcedureCode"))),
                Expression.Bind(finalSelectType.GetProperty("PX_TYPE"), Expression.Property(Expression.Property(pe_joinResultType, pe_joinResultType.Type.GetProperty("Encounter")), typeof(EncounterWithDetails).GetProperty("ProcedureCodeType"))),
                Expression.Bind(finalSelectType.GetProperty("PRO_ITEM_NAME"), Expression.Property(Expression.Property(pe_joinResultType, pe_joinResultType.Type.GetProperty("IndexEvent")), typeof(IndexEvent).GetProperty("ItemName"))),
                Expression.Bind(finalSelectType.GetProperty("PRO_RESPONSE_TEXT"), Expression.Property(Expression.Property(pe_joinResultType, pe_joinResultType.Type.GetProperty("IndexEvent")), typeof(IndexEvent).GetProperty("ResponseText"))),
                Expression.Bind(finalSelectType.GetProperty("PRO_RESPONSE_NUM"), Expression.Convert(Expression.Property(Expression.Property(pe_joinResultType, pe_joinResultType.Type.GetProperty("IndexEvent")), typeof(IndexEvent).GetProperty("ResponseNumber")), typeof(float?))),
                Expression.Bind(finalSelectType.GetProperty("PRO_MEASURE_SEQ"), Expression.Property(Expression.Property(pe_joinResultType, pe_joinResultType.Type.GetProperty("IndexEvent")), typeof(IndexEvent).GetProperty("ResponseSequence")))
            };

            foreach (var prop in rootQueryProperties.Where(p => p.Name.Contains("PatientID") == false))
            {
                finalSelectBindings.Add(Expression.Bind(finalSelectType.GetProperty(prop.Name), Expression.Property(pe_joinResultType, pe_joinResultType.Type.GetProperty(prop.Name))));
            }

            var admitDateSelector = Expression.Property(Expression.Property(pe_joinResultType, joinType.GetProperty("Encounter")), typeof(EncounterWithDetails).GetProperty("AdmitDate"));

            var orderbyAdmitDate = Expression.Call(
                typeof(Queryable),
                "OrderBy",
                new Type[] { joinType, admitDateSelector.Type },
                whereCall,
                Expression.Quote(Expression.Lambda(admitDateSelector, pe_joinResultType))
                );

            var finalSelector = Expression.Lambda(Expression.MemberInit(Expression.New(finalSelectType), finalSelectBindings), pe_joinResultType);
            var finalSelectCall = Expression.Call(
                typeof(Queryable),
                "Select",
                new Type[] { joinType, finalSelectType },
                orderbyAdmitDate,
                Expression.Quote(finalSelector)
                );

            return finalSelectCall;
        }

        public IEnumerable<IPropertyDefinition> FinalGroupKeyProperyDefinitions()
        {
            return _finalSelectTypeProperties.Select(p => new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO {
                Name = p.Name,
                As = p.As,
                Aggregate = p.Aggregate,
                Type = p.Type
            });
        }

        public IEnumerable<IPropertyDefinition> FinalSelectPropertyDefinitions()
        {

            return _finalSelectTypeProperties.Select(p => new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
            {
                Name = p.Name,
                As = p.As,
                Aggregate = p.Aggregate,
                Type = p.Type
            }); ;
        }

        Expression BuildIndexEvents()
        {
            Type selectType = typeof(IndexEvent);

            var indexCriteria = _indexEventCriteria.First();

            var ce_daysAfter = Expression.Constant(Convert.ToSingle(indexCriteria.DaysAfter), typeof(float));
            var ce_daysBefore = Expression.Constant(Convert.ToSingle(indexCriteria.DaysBefore), typeof(float));

            var selectBindings = new[] {
                Expression.Bind(selectType.GetProperty("PRO_ID"), Expression.Property(pe_proType, pe_proType.Type.GetProperty("ID"))),
                Expression.Bind(selectType.GetProperty("PatientID"), Expression.Property(pe_proType, pe_proType.Type.GetProperty("PatientID"))),
                Expression.Bind(selectType.GetProperty("ItemName"), Expression.Property(pe_proType, pe_proType.Type.GetProperty("ItemName"))),
                Expression.Bind(selectType.GetProperty("ResponseText"), Expression.Property(pe_proType, pe_proType.Type.GetProperty("ResponseText"))),
                Expression.Bind(selectType.GetProperty("ResponseNumber"), Expression.Property(pe_proType, pe_proType.Type.GetProperty("ResponseNumber"))),
                Expression.Bind(selectType.GetProperty("ResponseSequence"), Expression.Property(pe_proType, pe_proType.Type.GetProperty("MeasureSequence"))),
                Expression.Bind(selectType.GetProperty("TimeWindowAfter"), Expression.Add(Expression.Convert(Expression.Property(pe_proType, pe_proType.Type.GetProperty("ResponseNumber")), typeof(float)), ce_daysAfter)),
                Expression.Bind(selectType.GetProperty("TimeWindowBefore"), Expression.Subtract(Expression.Convert(Expression.Property(pe_proType, pe_proType.Type.GetProperty("ResponseNumber")), typeof(float)), ce_daysBefore))
            };

            var resultSelector = Expression.Lambda(
                Expression.MemberInit(Expression.New(selectType), selectBindings),
                pe_proType
                );

            var selectCall = Expression.Call(typeof(Queryable), "Select",
                new[] { typeof(ReportedOutcome), selectType },
                _db.ReportedOutcomeCommonMeasures.Where(pro => pro.ItemName == indexCriteria.IndexEventDateIdentifier && pro.ResponseNumber != null).Expression,
                Expression.Quote(resultSelector)
                );

            return selectCall;
        }

        Expression BuildEncounters()
        {
            MethodCallExpression sasDateCalc = Expression.Call(
                        typeof(System.Data.Entity.DbFunctions),
                        "DiffDays",
                        null,
                        Expression.Constant(SAS_Start_Date, typeof(DateTime?)),
                        Expression.Convert(Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("AdmittedOn")), typeof(DateTime?))
                    );

            Type joinResultType = typeof(EncounterWithDetails);

            var diagnosisBindings = new[] {
                    Expression.Bind(joinResultType.GetProperty("EncounterID"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("ID"))),
                    Expression.Bind(joinResultType.GetProperty("PatientID"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("PatientID"))),
                    Expression.Bind(joinResultType.GetProperty("AdmitDate"), Expression.Convert(Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("AdmittedOn")), typeof(DateTime?))),
                    Expression.Bind(joinResultType.GetProperty("EncounterSASDate"), sasDateCalc),
                    Expression.Bind(joinResultType.GetProperty("EncounterType"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("EncounterType"))),
                    Expression.Bind(joinResultType.GetProperty("DiagnosisCode"), Expression.Property(pe_diagnosis, pe_diagnosis.Type.GetProperty("Code"))),
                    Expression.Bind(joinResultType.GetProperty("DiagnosisCodeType"), Expression.Property(pe_diagnosis, pe_diagnosis.Type.GetProperty("CodeType"))),
                    Expression.Bind(joinResultType.GetProperty("ProcedureCode"), Expression.Convert(Expression.Constant(null), typeof(string))),
                    Expression.Bind(joinResultType.GetProperty("ProcedureCodeType"), Expression.Convert(Expression.Constant(null), typeof(string)))
                };

            var diagnosisResultSelector = Expression.Lambda(
                        Expression.MemberInit(Expression.New(joinResultType), diagnosisBindings),
                        pe_encounters,
                        pe_diagnosis
                    );

            //get all the encounters associated to diagnosis entries
            MethodCallExpression diagnosisJoinCall = JoinExpression<Encounter, Diagnosis, string, EncounterWithDetails>(
                    _db.Encounters,
                    _db.Diagnoses,
                    Expression.Quote(Expression.Lambda(Expression.Property(pe_encounters, "ID"), pe_encounters)),
                    Expression.Quote(Expression.Lambda(Expression.Property(pe_diagnosis, "EncounterID"), pe_diagnosis)),
                    Expression.Quote(diagnosisResultSelector)
                );

            var procedureBindings = new[] {
                    Expression.Bind(joinResultType.GetProperty("EncounterID"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("ID"))),
                    Expression.Bind(joinResultType.GetProperty("PatientID"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("PatientID"))),
                    Expression.Bind(joinResultType.GetProperty("AdmitDate"), Expression.Convert(Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("AdmittedOn")), typeof(DateTime?))),
                    Expression.Bind(joinResultType.GetProperty("EncounterSASDate"), sasDateCalc),
                    Expression.Bind(joinResultType.GetProperty("EncounterType"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("EncounterType"))),
                    Expression.Bind(joinResultType.GetProperty("DiagnosisCode"), Expression.Convert(Expression.Constant(null), typeof(string))),
                    Expression.Bind(joinResultType.GetProperty("DiagnosisCodeType"), Expression.Convert(Expression.Constant(null), typeof(string))),
                    Expression.Bind(joinResultType.GetProperty("ProcedureCode"), Expression.Property(pe_procedures, pe_procedures.Type.GetProperty("Code"))),
                    Expression.Bind(joinResultType.GetProperty("ProcedureCodeType"), Expression.Property(pe_procedures, pe_procedures.Type.GetProperty("CodeType")))
                };

            var procedureResultSelector = Expression.Lambda(
                    Expression.MemberInit(Expression.New(joinResultType), procedureBindings),
                    pe_encounters,
                    pe_procedures
                );

            //get all encounters associated to procedure entries
            MethodCallExpression proceduresJoinCall = JoinExpression<Encounter, Procedure, string, EncounterWithDetails>(
                    _db.Encounters,
                    _db.Procedures,
                    Expression.Quote(Expression.Lambda(Expression.Property(pe_encounters, "ID"), pe_encounters)),
                    Expression.Quote(Expression.Lambda(Expression.Property(pe_procedures, "EncounterID"), pe_procedures)),
                    procedureResultSelector
                );

            var isolatedEncountersBindings = new[] {
                    Expression.Bind(joinResultType.GetProperty("EncounterID"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("ID"))),
                    Expression.Bind(joinResultType.GetProperty("PatientID"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("PatientID"))),
                    Expression.Bind(joinResultType.GetProperty("AdmitDate"), Expression.Convert(Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("AdmittedOn")), typeof(DateTime?))),
                    Expression.Bind(joinResultType.GetProperty("EncounterSASDate"), sasDateCalc),
                    Expression.Bind(joinResultType.GetProperty("EncounterType"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("EncounterType"))),
                    Expression.Bind(joinResultType.GetProperty("DiagnosisCode"), Expression.Convert(Expression.Constant(null), typeof(string))),
                    Expression.Bind(joinResultType.GetProperty("DiagnosisCodeType"), Expression.Convert(Expression.Constant(null), typeof(string))),
                    Expression.Bind(joinResultType.GetProperty("ProcedureCode"), Expression.Convert(Expression.Constant(null), typeof(string))),
                    Expression.Bind(joinResultType.GetProperty("ProcedureCodeType"), Expression.Convert(Expression.Constant(null), typeof(string)))
                };

            var isolatedEncountersResultSelector = Expression.Lambda(
                    Expression.MemberInit(Expression.New(joinResultType), isolatedEncountersBindings),
                    pe_encounters
                );

            var isolatedEncountersQuery = from enc in _db.Encounters
                                          where !_db.Diagnoses.Any(dia => dia.EncounterID == enc.ID)
                                          && !_db.Procedures.Any(dia => dia.EncounterID == enc.ID)
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
                    new Type[] { joinResultType },
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

            return unionDPandEncountersCall;
        }

        Expression BuildNonIndexEvents()
        {
            Type selectType = typeof(IndexEvent);

            var indexCriteria = _indexEventCriteria.First();

            var selectBindings = new[] {
                Expression.Bind(selectType.GetProperty("PRO_ID"), Expression.Property(pe_proType, pe_proType.Type.GetProperty("ID"))),
                Expression.Bind(selectType.GetProperty("PatientID"), Expression.Property(pe_proType, pe_proType.Type.GetProperty("PatientID"))),
                Expression.Bind(selectType.GetProperty("ItemName"), Expression.Property(pe_proType, pe_proType.Type.GetProperty("ItemName"))),
                Expression.Bind(selectType.GetProperty("ResponseText"), Expression.Property(pe_proType, pe_proType.Type.GetProperty("ResponseText"))),
                Expression.Bind(selectType.GetProperty("ResponseNumber"), Expression.Property(pe_proType, pe_proType.Type.GetProperty("ResponseNumber"))),
                Expression.Bind(selectType.GetProperty("ResponseSequence"), Expression.Property(pe_proType, pe_proType.Type.GetProperty("MeasureSequence"))),
                Expression.Bind(selectType.GetProperty("TimeWindowAfter"), Expression.Convert(Expression.Constant(null), typeof(float?))),
                Expression.Bind(selectType.GetProperty("TimeWindowBefore"), Expression.Convert(Expression.Constant(null), typeof(float?)))
            };

            var resultSelector = Expression.Lambda(
                Expression.MemberInit(Expression.New(selectType), selectBindings),
                pe_proType
                );

            var selectCall = Expression.Call(typeof(Queryable), "Select",
                new[] { typeof(ReportedOutcome), selectType },
                _db.ReportedOutcomeCommonMeasures.Where(pro => pro.ItemName != indexCriteria.IndexEventDateIdentifier).Expression,
                Expression.Quote(resultSelector)
                );

            return selectCall;
        }

        Expression BuildNonIndexEncounters()
        {
            Type joinResultType = typeof(EncounterWithDetails);

            var isolatedEncountersBindings = new[] {
                    Expression.Bind(joinResultType.GetProperty("EncounterID"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("ID"))),
                    Expression.Bind(joinResultType.GetProperty("PatientID"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("PatientID"))),
                    Expression.Bind(joinResultType.GetProperty("AdmitDate"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("AdmittedOn"))),
                    Expression.Bind(joinResultType.GetProperty("EncounterSASDate"), Expression.Convert(Expression.Constant(null), typeof(int?))),
                    Expression.Bind(joinResultType.GetProperty("EncounterType"), Expression.Property(pe_encounters, pe_encounters.Type.GetProperty("EncounterType"))),
                    Expression.Bind(joinResultType.GetProperty("DiagnosisCode"), Expression.Convert(Expression.Constant(null), typeof(string))),
                    Expression.Bind(joinResultType.GetProperty("DiagnosisCodeType"), Expression.Convert(Expression.Constant(null), typeof(string))),
                    Expression.Bind(joinResultType.GetProperty("ProcedureCode"), Expression.Convert(Expression.Constant(null), typeof(string))),
                    Expression.Bind(joinResultType.GetProperty("ProcedureCodeType"), Expression.Convert(Expression.Constant(null), typeof(string)))
                };

            var isolatedEncountersResultSelector = Expression.Lambda(
                    Expression.MemberInit(Expression.New(joinResultType), isolatedEncountersBindings),
                    pe_encounters
                );

            MethodCallExpression encountersSelectCall = Expression.Call(
                    typeof(Queryable),
                    "Select",
                    new Type[] { typeof(Encounter), joinResultType },
                    _db.Encounters.AsQueryable().Expression,
                    Expression.Quote(isolatedEncountersResultSelector)
                );

            return encountersSelectCall;
        }

        

        Expression BuildNonIndexEventsQuery(Expression rootQueryExpr, ParameterExpression pe_rootQuerySelectType, ParameterExpression pe_indexEvt_enc_joinType, Type finalSelectType, List<MemberAssignment> finalSelectBindings)
        {
            var nonIndexEventsExpression = BuildNonIndexEvents();
            var nonIndexEncountersExpression = BuildNonIndexEncounters();

            var groupJoinType = Objects.Dynamic.TypeBuilderHelper.CreateType("grpJoin1", new List<Objects.Dynamic.IPropertyDefinition> {
                new TypedPropertyDefinition<IndexEvent>("PRO"),
                new TypedPropertyDefinition<IEnumerable<EncounterWithDetails>>("Encounters")
            });

            //need to join on encounters to allow for union with temporal events, however we do not want any of the encounter information only the events
            //since it is a left outer join the encounters can be null to return only the PRO items that match
            nonIndexEncountersExpression = Expression.Call(
                typeof(Queryable),
                "Where",
                new Type[] { typeof(EncounterWithDetails) },
                nonIndexEncountersExpression,
                Expression.Quote(Expression.Lambda(Expression.Equal(Expression.Property(pe_encounterWithDetails, "EncounterID"), Expression.Constant(null, typeof(string))), pe_encounterWithDetails ))
                );

            var pe_encountersWithDetailCollection = Expression.Parameter(typeof(IEnumerable<EncounterWithDetails>), "encounters");
            var groupJoinBindings = new[] {
                Expression.Bind(groupJoinType.GetProperty("PRO"), pe_indexEvent),
                Expression.Bind(groupJoinType.GetProperty("Encounters"), pe_encountersWithDetailCollection)
            };


            var groupJoinMethodInfo = typeof(Queryable).GetMethods().First(x => x.Name == "GroupJoin" && x.GetParameters().Length == 5).MakeGenericMethod(typeof(IndexEvent), typeof(EncounterWithDetails), typeof(string), groupJoinType);
            var proGroupJoinCall = Expression.Call(
                groupJoinMethodInfo,
                nonIndexEventsExpression,
                nonIndexEncountersExpression,
                Expression.Quote(Expression.Lambda(Expression.Property(pe_indexEvent, "PatientID"), pe_indexEvent)),
                Expression.Quote(Expression.Lambda(Expression.Property(pe_encounterWithDetails, "PatientID"), pe_encounterWithDetails)),
                Expression.Quote(Expression.Lambda(
                    Expression.MemberInit(Expression.New(groupJoinType), groupJoinBindings),
                    pe_indexEvent,
                    pe_encountersWithDetailCollection
                    ))
                );

            var pe_groupJoinType = Expression.Parameter(groupJoinType, "groupJoin1");

            var Enumerable_DefaultIfEmpty = typeof(Enumerable).GetMethods().First(x => x.Name == "DefaultIfEmpty" && x.GetParameters().Length == 1);
            var collectionSelector = Expression.Lambda(
                        Expression.Call(
                            null,
                            Enumerable_DefaultIfEmpty.MakeGenericMethod(typeof(EncounterWithDetails)),
                            Expression.MakeMemberAccess(pe_groupJoinType, groupJoinType.GetProperty("Encounters"))
                            ), pe_groupJoinType
                    );

            var resultSelectorForManyBindings = new[] {
                Expression.Bind(pe_indexEvt_enc_joinType.Type.GetProperty("IndexEvent"), Expression.Property(pe_groupJoinType, pe_groupJoinType.Type.GetProperty("PRO"))),
                Expression.Bind(pe_indexEvt_enc_joinType.Type.GetProperty("Encounter"), pe_encounterWithDetails)
            };

            var resultSelectorForSelectMany = Expression.Lambda(
                    Expression.MemberInit(Expression.New(pe_indexEvt_enc_joinType.Type), resultSelectorForManyBindings),
                    pe_groupJoinType,
                    pe_encounterWithDetails
                );

            var selectManyMethodInfo = typeof(Queryable).GetMethods().Where(x => x.Name == "SelectMany" && x.GetParameters().Length == 3).OrderBy(x => x.ToString().Length).First()
                .MakeGenericMethod(groupJoinType, typeof(EncounterWithDetails), pe_indexEvt_enc_joinType.Type);

            var selectManyCall = Expression.Call(
                selectManyMethodInfo,
                proGroupJoinCall,
                collectionSelector,
                resultSelectorForSelectMany
                );

            var resultSelector = Expression.Lambda(
                    Expression.MemberInit(Expression.New(finalSelectType), finalSelectBindings),
                    pe_indexEvt_enc_joinType,
                    pe_rootQuerySelectType
                );

            var joinCall = Expression.Call(
                typeof(Queryable),
                "Join",
                new Type[] {
                    pe_indexEvt_enc_joinType.Type,
                    rootQueryExpr.Type.GetGenericArguments()[0],
                    typeof(string),
                    finalSelectType
                },
                new[] {
                    selectManyCall,
                    rootQueryExpr,
                    Expression.Quote(Expression.Lambda(Expression.Property(Expression.Property(pe_indexEvt_enc_joinType, pe_indexEvt_enc_joinType.Type.GetProperty("IndexEvent")), typeof(IndexEvent).GetProperty("PatientID")), pe_indexEvt_enc_joinType)),
                    Expression.Quote(Expression.Lambda(Expression.Property(pe_rootQuerySelectType, pe_rootQuerySelectType.Type.GetProperty("PatientID")), pe_rootQuerySelectType)),
                    Expression.Quote(resultSelector)
                }
                );

            //var temp = _db.Patients.AsQueryable().Provider.CreateQuery(joinCall);
            //var sql = temp.ToTraceString();

            //apply the criteria limiting the non index events
            var indexCriteria = _indexEventCriteria.First();
            var pe_joinResultType = Expression.Parameter(finalSelectType, "g1");
            var propEx_IndexEvent_ItemName = Expression.Property(Expression.Property(pe_joinResultType, pe_joinResultType.Type.GetProperty("IndexEvent")), typeof(IndexEvent).GetProperty("ItemName"));
            var propEx_IndexEvent_ResponseText = Expression.Property(Expression.Property(pe_joinResultType, pe_joinResultType.Type.GetProperty("IndexEvent")), typeof(IndexEvent).GetProperty("ResponseText"));

            BinaryExpression queryPredicate = null;
            foreach (var paragraph in _pro_QueryCriteria)
            {
                var terms = PCORIModelAdapter.GetAllCriteriaTerms(paragraph, Lpp.QueryComposer.ModelTermsFactory.PatientReportedOutcomeID);
                if (terms.Any() == false)
                    continue;

                BinaryExpression proPredicate = null;
                foreach (var term in terms) {
                    string itemName = term.GetStringValue("ItemName");
                    string itemText = term.GetStringValue("ItemResponse");

                    BinaryExpression itemPredicate = null;
                    if (!string.IsNullOrEmpty(itemName) && !string.IsNullOrEmpty(itemText))
                    {
                        //apply name and response text and'd
                        itemPredicate = Expression.AndAlso(
                            Expression.Equal(propEx_IndexEvent_ItemName, Expression.Constant(itemName, typeof(string))),
                            Expression.Equal(propEx_IndexEvent_ResponseText, Expression.Constant(itemText, typeof(string)))    
                        );

                    }
                    else if (!string.IsNullOrEmpty(itemName) && string.IsNullOrEmpty(itemText))
                    {
                        //apply only itemName
                        itemPredicate = Expression.Equal(propEx_IndexEvent_ItemName, Expression.Constant(itemName, typeof(string)));

                    }
                    else if (string.IsNullOrEmpty(itemName) && !string.IsNullOrEmpty(itemText))
                    {
                        //apply only response text
                        itemPredicate = Expression.Equal(propEx_IndexEvent_ResponseText, Expression.Constant(itemText, typeof(string)));
                    }

                    if (itemPredicate != null)
                    {
                        if (proPredicate == null)
                        {
                            proPredicate = itemPredicate;
                        }
                        else
                        {
                            proPredicate = Expression.OrElse(proPredicate, itemPredicate);
                        }
                    }
                }

                if (queryPredicate == null)
                {
                    if (paragraph.Exclusion)
                    {
                        queryPredicate = Expression.NotEqual(proPredicate, Expression.Constant(true, typeof(bool)));
                    }
                    else
                    {
                        queryPredicate = proPredicate;
                    }
                }
                else
                {
                    var conjunction = paragraph.Operator;
                    if (conjunction == DTO.Enums.QueryComposerOperators.And && paragraph.Exclusion == false)
                    {
                        queryPredicate = Expression.AndAlso(queryPredicate, proPredicate);

                    }
                    else if (conjunction == DTO.Enums.QueryComposerOperators.AndNot || (conjunction == DTO.Enums.QueryComposerOperators.And && paragraph.Exclusion))
                    {
                        queryPredicate = Expression.AndAlso(queryPredicate, Expression.NotEqual(proPredicate, Expression.Constant(true, typeof(bool))));

                    }
                    else if (conjunction == DTO.Enums.QueryComposerOperators.Or && paragraph.Exclusion == false)
                    {
                        queryPredicate = Expression.OrElse(queryPredicate, proPredicate);
                    }
                    else
                    {
                        queryPredicate = Expression.OrElse(queryPredicate, Expression.NotEqual(proPredicate, Expression.Constant(true, typeof(bool))));

                    }
                }


            }

            //exclude the index events
            var eventPredicateExpression = Expression.NotEqual(propEx_IndexEvent_ItemName, Expression.Constant(indexCriteria.IndexEventDateIdentifier, typeof(string)));

            if (queryPredicate != null)
            {
                eventPredicateExpression = Expression.AndAlso(eventPredicateExpression, Expression.Convert(queryPredicate, typeof(bool)));
            }

            var whereCall = Expression.Call(
                typeof(Queryable),
                "Where",
                new Type[] { finalSelectType },
                joinCall,
                Expression.Quote(Expression.Lambda(eventPredicateExpression, pe_joinResultType))
                );

            //temp = _db.Patients.AsQueryable().Provider.CreateQuery(whereCall);
            //sql = temp.ToTraceString();

            return whereCall;
        }


        static MethodCallExpression JoinExpression<Outer, Inner, Key, Result>(IQueryable<Outer> outerQueryable, IQueryable<Inner> innerQueryable, Expression outerKey, Expression innerKey, Expression selector)
        {
            return Expression.Call(
                typeof(Queryable),
                "Join",
                new Type[] {
                    typeof(Outer),
                    typeof(Inner),
                    typeof(Key),
                    typeof(Result)
                },
                new Expression[] {
                    outerQueryable.Expression,
                    innerQueryable.Expression,
                    outerKey,
                    innerKey,
                    selector
                }
                );
        }


        public class IndexEvent
        {
            public string PRO_ID { get; set; }
            public string PatientID { get; set; }
            public string ItemName { get; set; }
            public string ResponseText { get; set; }
            public double? ResponseNumber { get; set; }
            public string ResponseSequence { get; set; }
            public float? TimeWindowAfter { get; set; }
            public float? TimeWindowBefore { get; set; }
        }

        public class EncounterWithDetails
        {
            public string EncounterID { get; set; }
            public string PatientID { get; set; }
            public DateTime? AdmitDate { get; set; }
            public int? EncounterSASDate { get; set; }
            public string EncounterType { get; set; }
            public string DiagnosisCode { get; set; }
            public string DiagnosisCodeType { get; set; }
            public string ProcedureCode { get; set; }
            public string ProcedureCodeType { get; set; }
        }

        public class PatientEncounterDetails
        {
            public string ParticipantID { get; set; }
            public DateTime? Admit_Date { get; set; }
            public string Encounter_Type { get; set; }
            public string DX { get; set; }
            public string DX_Type { get; set; }
            public string PX { get; set; }
            public string PX_Type { get; set; }
            public string PRO_Item_Name { get; set; }
            public string PRO_Response_Text { get; set; }
            public string PRO_Measure_Sequence { get; set; }

        }

        public class EventWithEncounter
        {
            public IndexEvent IndexEvent { get; set; }

            public EncounterWithDetails Encounter { get; set; }
        }

        public class TypedPropertyDefinition<T> : Objects.Dynamic.IPropertyDefinition
        {
            public TypedPropertyDefinition(string name)
            {
                this.Name = name;
            }

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
