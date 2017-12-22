using LinqKit;
using Lpp.Dns.DTO.QueryComposer;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters
{
    /// <summary>
    /// Instantiates a base dynamic model adapter.
    /// </summary>
    /// <typeparam name="TBaseQuery">The type representing the root query object type.</typeparam>
    public abstract class DynamicModelAdapter<TBaseQuery> : ModelAdapter
    {
        /// <summary>
        /// Lookup containing predicate builders that are term specific.
        /// </summary>
        protected readonly Dictionary<Guid, Func<QueryComposerCriteriaDTO, QueryComposerTermDTO, System.Linq.Expressions.Expression<Func<TBaseQuery, bool>>>> TermPredicateBuilders = new Dictionary<Guid, Func<QueryComposerCriteriaDTO, QueryComposerTermDTO, System.Linq.Expressions.Expression<Func<TBaseQuery, bool>>>>(50);
        
        /// <summary>
        /// Collection of predicate builders that are run for each paragraph.
        /// The current paragraph and paragraph predictate after the term specific predicates have been applied will be supplied to the Func.
        /// </summary>
        /// <remarks>
        /// Register predicate building methods that should apply a predicate based on multiple terms. It will be up to the method implmentation to determine if applicable or not.
        /// </remarks>
        protected readonly HashSet<Func<QueryComposerCriteriaDTO, System.Linq.Expressions.Expression<Func<TBaseQuery, bool>>, System.Linq.Expressions.Expression<Func<TBaseQuery, bool>>>> ParagraphPredicateBuilders = new HashSet<Func<QueryComposerCriteriaDTO, System.Linq.Expressions.Expression<Func<TBaseQuery, bool>>, System.Linq.Expressions.Expression<Func<TBaseQuery, bool>>>>();
        
        public DynamicModelAdapter(Guid modelID)
            : base(modelID)
        {

            //register the default predicate actions
            TermPredicateBuilders.Add(ModelTermsFactory.AgeRangeID, ApplyAgeRangeTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.CodeMetricID, ApplyCodeMetricTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.ConditionsID, ApplyConditionsTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.CoverageID, ApplyCoverageTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.CriteriaID, ApplyCriteriaTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.DispensingMetricID, ApplyDispensingMetricTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.DrugClassID, ApplyDrugClassTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.DrugNameID, ApplyDrugNameTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.EthnicityID, ApplyEthnicityTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.FacilityID, ApplyFacilityTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.SexID, ApplySexTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.HCPCSProcedureCodesID, ApplyHCPCSProcedureCodesTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.HeightID, ApplyHeightTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.HispanicID, ApplyHispanicTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.ICD9DiagnosisCodes3digitID, ApplyICD9DiagnosisCodesTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.ICD9DiagnosisCodes4digitID, ApplyICD9DiagnosisCodesTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.ICD9DiagnosisCodes5digitID, ApplyICD9DiagnosisCodesTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.ICD9ProcedureCodes3digitID, ApplyICD9ProcedureCodesTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.ICD9ProcedureCodes4digitID, ApplyICD9ProcedureCodesTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.ModularProgramID, ApplyModularProgramTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.ObservationPeriodID, ApplyObservationPeriodTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.QuarterYearID, ApplyQuarterYearTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.RaceID, ApplyRaceTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.SettingID, ApplySettingTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.SqlDistributionID, ApplySqlTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.TobaccoUseID, ApplyTobaccoUseTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.VisitsID, ApplyVisitsTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.WeightID, ApplyWeightTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.YearID, ApplyYearTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.ZipCodeID, ApplyZipCodeTerm);
            TermPredicateBuilders.Add(ModelTermsFactory.VitalsMeasureDateID, ApplyVitalsMeasureDateObservationPeriod);
        }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyCriteria(IEnumerable<QueryComposerCriteriaDTO> criteria)
        {
            System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> queryPredicate = null;

            foreach (var paragraph in criteria)
            {
                var paragraphPredicate = ParseParagraph(paragraph);
                if (queryPredicate == null)
                {
                    queryPredicate = PredicateBuilder.True<TBaseQuery>();

                    //The  initial predicate is set to TRUE and ANDed with the first paragraph conditions.
                    queryPredicate = queryPredicate.And(paragraphPredicate.Expand());
                }
                else
                {
                    //Merge the paragraph predicates (PMNMAINT-1206)
                    queryPredicate = MergeParagraphPredicates(queryPredicate, paragraphPredicate, paragraph.Operator, paragraph.Exclusion);
                }
            }

            return queryPredicate;
        }

        /// <summary>
        /// Implementation to merge two predicates together.
        /// </summary>
        /// <param name="queryPredicate"></param>
        /// <param name="nextParagraphPredicate"></param>
        /// <param name="conjunction"></param>
        /// <returns></returns>
        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> MergeParagraphPredicates(System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> queryPredicate, 
            System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> nextParagraphPredicate, DTO.Enums.QueryComposerOperators conjunction, bool isExclusion)
        {
            throw new NotImplementedException();
        }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ParseParagraph(QueryComposerCriteriaDTO paragraph)
        {
            //The next operator is used to define the relationship between the current concept with the next concept, if any.
            DTO.Enums.QueryComposerOperators nextOperator = DTO.Enums.QueryComposerOperators.And;

            //Create the paragraph predicate.
            //The predicate is set to TRUE and ANDed with the first concept.
            var paragraphPredicate = PredicateBuilder.True<TBaseQuery>();

            //Process the concepts within the paragraph.
            foreach (var term in paragraph.Terms)
            {
                var termID = term.Type;
                var termPredicate = PredicateBuilder.True<TBaseQuery>();

                //By default do not apply the term unless it has been registered.
                System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> innerPredicate = null;
                //Parse the term here.
                Func<QueryComposerCriteriaDTO, QueryComposerTermDTO, System.Linq.Expressions.Expression<Func<TBaseQuery, bool>>> action = null;
                if (TermPredicateBuilders.TryGetValue(term.Type, out action))
                {
                    //The term has been registered replace the inner predicate.
                    innerPredicate = action(paragraph, term);
                }

                if (innerPredicate == null)
                    continue;

                termPredicate = termPredicate.And(innerPredicate.Expand());

                if (nextOperator == DTO.Enums.QueryComposerOperators.And)
                {
                    paragraphPredicate = paragraphPredicate.And(termPredicate.Expand());
                }
                else
                {
                    paragraphPredicate = paragraphPredicate.Or(termPredicate.Expand());
                }
                //Handle AndNot and OrNot here.
                nextOperator = term.Operator;
            }

            foreach (var paragraphAction in ParagraphPredicateBuilders)
            {
                //the predicate method is responsible for determining how to apply to the paragraph predicate
                paragraphPredicate = paragraphAction(paragraph, paragraphPredicate);
            }



            //TODO: parse the child criteria and append to the paragraph predicate

            return paragraphPredicate;
        }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyAgeRangeTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyCodeMetricTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyConditionsTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyCoverageTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyCriteriaTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyDispensingMetricTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyDrugClassTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyDrugNameTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyEthnicityTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyFacilityTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplySexTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyHCPCSProcedureCodesTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyHeightTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyHispanicTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyICD9DiagnosisCodesTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyICD9ProcedureCodesTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyModularProgramTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyObservationPeriodTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyQuarterYearTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyRaceTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplySettingTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplySqlTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyTobaccoUseTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyVisitsTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyWeightTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyYearTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyZipCodeTerm(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }

        protected virtual System.Linq.Expressions.Expression<Func<TBaseQuery, bool>> ApplyVitalsMeasureDateObservationPeriod(QueryComposerCriteriaDTO paragraph, QueryComposerTermDTO term) { throw new NotImplementedException("The critieria builder for this term has not been implemented."); }
    }
}
