using Lpp.Objects.Dynamic;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI.Terms
{
    public class VitalMeasureDate : TermImplementation
    {
        public VitalMeasureDate(PCORIQueryBuilder.DataContext db)
            : base(ModelTermsFactory.VitalsMeasureDateID, db)
        {
        }

        public override Objects.Dynamic.IPropertyDefinition[] InnerSelectPropertyDefinitions()
        {
            return new[]{
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "MeasuredOn",
                        Type = typeof(DateTime).FullName
                    }
            };
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> InnerSelectBindings(Type selectType, System.Linq.Expressions.ParameterExpression sourceTypeParameter)
        {

            ParameterExpression pe_sourceQueryType = sourceTypeParameter;
            ParameterExpression pe_vitalsQueryType = Expression.Parameter(typeof(PCORIQueryBuilder.Model.Vital), "v");
            Expression measuredOnSelector = Expression.Property(pe_vitalsQueryType, "MeasuredOn");

            BinaryExpression be_vitalsPatient = Expression.Equal(Expression.Property(pe_vitalsQueryType, "PatientID"), Expression.Property(pe_sourceQueryType, "ID"));

            //apply all the applicable criteria to the inner value select as well.
            List<Expression> predicates = new List<Expression>();

            if (Criteria.Any(c => c.Terms.Any(t => t.Type == ModelTermsFactory.ObservationPeriodID)))
            {
                Expression prop_Encounter = Expression.Property(pe_vitalsQueryType, "Encounter");
                Expression proper_Encounter_AdmittedOn = Expression.Property(prop_Encounter, "AdmittedOn");

                BinaryExpression observationPeriodPredicate = null;

                var observationPeriodCriteria = Criteria.SelectMany(c => c.Terms.Where(t => t.Type == ModelTermsFactory.ObservationPeriodID));
                foreach (var obpTerm in observationPeriodCriteria)
                {
                    BinaryExpression obp_predicate = null;
                    var obp_values = AdapterHelpers.ParseDateRangeValues(obpTerm);
                    if (obp_values.StartDate.HasValue && obp_values.EndDate.HasValue)
                    {
                        DateTime startDate = obp_values.StartDate.Value.Date;
                        DateTime endDate = obp_values.EndDate.Value.Date;
                        obp_predicate = Expression.AndAlso(Expression.GreaterThanOrEqual(proper_Encounter_AdmittedOn, Expression.Constant(startDate)), Expression.LessThanOrEqual(proper_Encounter_AdmittedOn, Expression.Constant(endDate)));
                    }
                    else if (obp_values.StartDate.HasValue)
                    {
                        DateTime startDate = obp_values.StartDate.Value.Date;
                        obp_predicate = Expression.GreaterThanOrEqual(proper_Encounter_AdmittedOn, Expression.Constant(startDate));
                    }
                    else if (obp_values.EndDate.HasValue)
                    {
                        DateTime endDate = obp_values.EndDate.Value.Date;
                        obp_predicate = Expression.LessThanOrEqual(proper_Encounter_AdmittedOn, Expression.Constant(endDate));
                    }

                    if (obp_predicate == null)
                        continue;

                    if (observationPeriodPredicate == null)
                    {
                        observationPeriodPredicate = Expression.AndAlso(Expression.NotEqual(Expression.Property(pe_vitalsQueryType, "EncounterID"), Expression.Constant(null)), obp_predicate);
                    }
                    else
                    {
                        observationPeriodPredicate = Expression.AndAlso(observationPeriodPredicate, obp_predicate);
                    }

                }

                if (observationPeriodPredicate != null)
                    predicates.Add(observationPeriodPredicate);
            }

            if (Criteria.Any(c => c.Terms.Any(t => t.Type == ModelTermsFactory.HeightID)))
            {
                Expression heightExp = Expression.Property(pe_vitalsQueryType, "Height");
                predicates.Add(Expression.Equal(Expression.Property(heightExp, "HasValue"), Expression.Constant(true)));

                Expression heightSelector = Expression.Property(heightExp, "Value");

                var heightCriteria = Criteria.SelectMany(c => c.Terms.Where(t => t.Type == ModelTermsFactory.HeightID));
                foreach (var heightTerm in heightCriteria)
                {
                    var heightRange = AdapterHelpers.ParseHeightValues(heightTerm);
                    if (heightRange.MinHeight.HasValue)
                    {
                        double minHeight = heightRange.MinHeight.Value;
                        predicates.Add(
                                Expression.GreaterThanOrEqual(heightSelector, Expression.Constant(minHeight))
                            );
                    }
                    if (heightRange.MaxHeight.HasValue)
                    {
                        double maxHeight = heightRange.MaxHeight.Value;
                        predicates.Add(
                                Expression.LessThanOrEqual(heightSelector, Expression.Constant(maxHeight))
                            );
                    }
                }
                
            }

            if (Criteria.Any(c => c.Terms.Any(t => t.Type == ModelTermsFactory.WeightID)))
            {
                Expression weightExp = Expression.Property(pe_vitalsQueryType, "Weight");
                predicates.Add(Expression.Equal(Expression.Property(weightExp, "HasValue"), Expression.Constant(true)));

                Expression weightSelector = Expression.Property(weightExp, "Value");

                var weightCriteria = Criteria.SelectMany(c => c.Terms.Where(t => t.Type == ModelTermsFactory.WeightID));
                foreach (var weightTerm in weightCriteria)
                {
                    var weightRange = AdapterHelpers.ParseWeightValues(weightTerm);
                    if (weightRange.MinWeight.HasValue)
                    {
                        double minWeight = weightRange.MinWeight.Value;
                        predicates.Add(
                                Expression.GreaterThanOrEqual(weightSelector, Expression.Constant(minWeight))
                            );
                    }
                    if (weightRange.MaxWeight.HasValue)
                    {
                        double maxWeight = weightRange.MaxWeight.Value;
                        predicates.Add(
                                Expression.LessThanOrEqual(weightSelector, Expression.Constant(maxWeight))
                            );
                    }
                }
            }

            if (Criteria.Any(c => c.Terms.Any(t => t.Type == ModelTermsFactory.VitalsMeasureDateID)))
            {
                var measureDateCriteria = Criteria.SelectMany(c => c.Terms.Where(t => t.Type == ModelTermsFactory.VitalsMeasureDateID));
                foreach (var measureDateTerm in measureDateCriteria)
                {

                    BinaryExpression md_predicate = null;
                    var md_values = AdapterHelpers.ParseDateRangeValues(measureDateTerm);
                    if (md_values.StartDate.HasValue && md_values.EndDate.HasValue)
                    {
                        DateTime startDate = md_values.StartDate.Value.Date;
                        DateTime endDate = md_values.EndDate.Value.Date;
                        md_predicate = Expression.AndAlso(Expression.GreaterThanOrEqual(measuredOnSelector, Expression.Constant(startDate)), Expression.LessThanOrEqual(measuredOnSelector, Expression.Constant(endDate)));
                    }
                    else if (md_values.StartDate.HasValue)
                    {
                        DateTime startDate = md_values.StartDate.Value.Date;
                        md_predicate = Expression.GreaterThanOrEqual(measuredOnSelector, Expression.Constant(startDate));

                    }
                    else if (md_values.EndDate.HasValue)
                    {
                        DateTime endDate = md_values.EndDate.Value.Date;
                        md_predicate = Expression.LessThanOrEqual(measuredOnSelector, Expression.Constant(endDate));
                    }

                    if (md_predicate != null)
                        predicates.Add(md_predicate);

                }
            }

            Expression predicateExpression;
            if (predicates.Count == 0)
            {
                predicateExpression = be_vitalsPatient;
            }
            else
            {
                predicateExpression = Expression.AndAlso(be_vitalsPatient, predicates[0]);
                if (predicates.Count > 1)
                {
                    for (int i = 1; i < predicates.Count; i++)
                    {
                        predicateExpression = Expression.AndAlso(predicateExpression, predicates[i]);
                    }
                }
            }

            Expression vitalsProp = Expressions.AsQueryable(Expression.Property(sourceTypeParameter, "Vitals"));
            MethodCallExpression measuredOnWhere = Expressions.Where(vitalsProp, Expression.Lambda(predicateExpression, pe_vitalsQueryType));

            MethodCallExpression orderByMeasureOn = Expressions.OrderByAscending(measuredOnWhere, Expression.Lambda(measuredOnSelector, pe_vitalsQueryType));

            MethodCallExpression measureOnSelect = Expression.Call(typeof(Queryable), "Select", new Type[] { pe_vitalsQueryType.Type, measuredOnSelector.Type }, orderByMeasureOn, Expression.Quote(Expression.Lambda(measuredOnSelector, pe_vitalsQueryType)));

            MethodCallExpression firstOrDefaultVitalMeasuredOn = Expression.Call(typeof(Queryable), "FirstOrDefault", new Type[] { typeof(DateTime) }, measureOnSelect);

            return new[] { 
                    Expression.Bind(selectType.GetProperty("MeasuredOn"), firstOrDefaultVitalMeasuredOn)
                };
        }

        public override Objects.Dynamic.IPropertyDefinition[] GroupKeyPropertyDefinitions()
        {
            return new[]{
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "MeasuredOn",
                        Type = typeof(DateTime).FullName
                    }
            };
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> GroupKeySelectBindings(Type selectType, System.Linq.Expressions.ParameterExpression sourceTypeParameter)
        {
            return new[]{
                Expression.Bind(selectType.GetProperty("MeasuredOn"), Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("MeasuredOn")))
            }; 
        }

        public override Objects.Dynamic.IPropertyDefinition[] FinalSelectPropertyDefinitions()
        {
            return new[]{
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "MeasuredOn",
                        Type = typeof(DateTime).FullName
                    }
            };
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> FinalSelectBindings(Type selectType, System.Linq.Expressions.ParameterExpression sourceTypeParameter)
        {
            //There is no good way to determine if the type of sourceTypeParameter is an IGrouping, so assume that it will always be grouped
            return new[] { Expression.Bind(selectType.GetProperty("MeasuredOn"), Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "MeasuredOn")) };
        }
    }
}
