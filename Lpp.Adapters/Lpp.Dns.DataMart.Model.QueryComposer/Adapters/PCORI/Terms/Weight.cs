using Lpp.Objects.Dynamic;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI.Terms
{
    public class Weight : TermImplementation, ITermResultTransformer
    {
        public Weight(PCORIQueryBuilder.DataContext db)
            : base(ModelTermsFactory.WeightID, db)
        {
        }

        public override Objects.Dynamic.IPropertyDefinition[] InnerSelectPropertyDefinitions()
        {
            //initial select will be always be numeric, Weight will only be a string for the final select where it is the stratification text
            return new[]{
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "Weight",
                        Type = typeof(double?).FullName
                    }
            };
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> InnerSelectBindings(Type selectType, System.Linq.Expressions.ParameterExpression sourceTypeParameter)
        {
            ParameterExpression pe_sourceQueryType = sourceTypeParameter;
            ParameterExpression pe_vitalsQueryType = Expression.Parameter(typeof(PCORIQueryBuilder.Model.Vital), "v");
            Expression weightSelector = Expression.Property(pe_vitalsQueryType, "Weight");
            Expression weightSelector_Value = Expression.Property(Expression.Property(pe_vitalsQueryType, "Weight"), "Value");

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
                Expression heightSelector = Expression.Property(pe_vitalsQueryType, "Height");
                predicates.Add(Expression.Equal(Expression.Property(heightSelector, "HasValue"), Expression.Constant(true)));

                Expression heightSelector_Value = Expression.Property(heightSelector, "Value");

                var heightCriteria = Criteria.SelectMany(c => c.Terms.Where(t => t.Type == ModelTermsFactory.HeightID));
                foreach (var heightTerm in heightCriteria)
                {
                    var heightRange = AdapterHelpers.ParseHeightValues(heightTerm);
                    if (heightRange.MinHeight.HasValue)
                    {
                        double minHeight = heightRange.MinHeight.Value;
                        predicates.Add(
                                Expression.GreaterThanOrEqual(heightSelector_Value, Expression.Constant(minHeight))
                            );
                    }
                    if (heightRange.MaxHeight.HasValue)
                    {
                        double maxHeight = heightRange.MaxHeight.Value;
                        predicates.Add(
                                Expression.LessThanOrEqual(heightSelector_Value, Expression.Constant(maxHeight))
                            );
                    }
                }
            }

            if (Criteria.Any(c => c.Terms.Any(t => t.Type == ModelTermsFactory.WeightID)))
            {
                //if the weight term is included in critieria it enforces that the weight value cannot be null
                predicates.Add(Expression.Equal(Expression.Property(weightSelector, "HasValue"), Expression.Constant(true)));

                var weightCriteria = Criteria.SelectMany(c => c.Terms.Where(t => t.Type == ModelTermsFactory.WeightID));
                foreach (var weightTerm in weightCriteria)
                {
                    var weightRange = AdapterHelpers.ParseWeightValues(weightTerm);
                    if (weightRange.MinWeight.HasValue)
                    {
                        double minWeight = weightRange.MinWeight.Value;
                        predicates.Add(
                                Expression.GreaterThanOrEqual(weightSelector_Value, Expression.Constant(minWeight))
                            );
                    }
                    if (weightRange.MaxWeight.HasValue)
                    {
                        double maxWeight = weightRange.MaxWeight.Value;
                        predicates.Add(
                                Expression.LessThanOrEqual(weightSelector_Value, Expression.Constant(maxWeight))
                            );
                    }
                }
            }

            if (Criteria.Any(c => c.Terms.Any(t => t.Type == ModelTermsFactory.VitalsMeasureDateID)))
            {
                Expression measuredOnSelector = Expression.Property(pe_vitalsQueryType, "MeasuredOn");

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

            Expression predicateExpression = null;
            if(predicates.Count > 0)
            {
                predicateExpression = predicates[0];
                if (predicates.Count > 1)
                {
                    for (int i = 1; i < predicates.Count; i++)
                    {
                        predicateExpression = Expression.AndAlso(predicateExpression, predicates[i]);
                    }
                }
            }

            Expression vitalsProp = Expressions.AsQueryable(Expression.Property(sourceTypeParameter, "Vitals"));            

            Expression measureOnSelector = Expression.Property(pe_vitalsQueryType, "MeasuredOn");
            MethodCallExpression orderByMeasureOn;
            if (predicateExpression == null)
            {
                orderByMeasureOn = Expressions.OrderByAscending(vitalsProp, Expression.Lambda(measureOnSelector, pe_vitalsQueryType));
            }
            else
            {
                MethodCallExpression weightWhere = Expressions.Where(vitalsProp, Expression.Lambda(predicateExpression, pe_vitalsQueryType));
                orderByMeasureOn = Expressions.OrderByAscending(weightWhere, Expression.Lambda(measureOnSelector, pe_vitalsQueryType));
            }

            MethodCallExpression weightSelect = Expressions.Select(pe_vitalsQueryType.Type, weightSelector.Type, orderByMeasureOn, Expression.Lambda(weightSelector, pe_vitalsQueryType));

            MethodCallExpression firstOrDefaultVitalWeight = Expressions.FirstOrDefault<double?>(weightSelect);

            if (!HasStratifications)
            {
                return new[] { 
                    Expression.Bind(selectType.GetProperty("Weight"), firstOrDefaultVitalWeight)
                };
            }

            // apply the modifier for stratification
            DTO.Enums.WeightStratification stratification;
            if (!Enum.TryParse<DTO.Enums.WeightStratification>(Stratifications.First().ToString(), out stratification))
            {
                throw new ArgumentException("Unable to parse the specified stratification value as an WeightStratification: " + Stratifications.First().ToString());
            }

            Expression stratificationExp = Expression.Condition(Expression.Equal(Expression.Property(firstOrDefaultVitalWeight, "HasValue"), Expression.Constant(true, typeof(bool))),                
                Expression.Convert(Expressions.MathFloor<double>(Expression.Divide(Expression.Property(firstOrDefaultVitalWeight, "Value"), Expression.Constant(Convert.ToDouble((int)stratification)))), typeof(double?)),
                firstOrDefaultVitalWeight);

            return new[] { 
                    Expression.Bind(selectType.GetProperty("Weight"), stratificationExp)
                };

        }

        public override Objects.Dynamic.IPropertyDefinition[] GroupKeyPropertyDefinitions()
        {
            return new[]{
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "Weight",
                        Type = typeof(double?).FullName
                    }
            };
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> GroupKeySelectBindings(Type selectType, System.Linq.Expressions.ParameterExpression sourceTypeParameter)
        {
            return new[]{
                Expression.Bind(selectType.GetProperty("Weight"), Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("Weight")))
            };
        }

        public override Objects.Dynamic.IPropertyDefinition[] FinalSelectPropertyDefinitions()
        {
            //return the field as a string with the stratification description
            //no stratification, return as a numeric value
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "Weight",
                    Type = typeof(double?).FullName
                    //Type = HasStratifications ? typeof(string).FullName : typeof(double?).FullName
                }
            };  
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> FinalSelectBindings(Type selectType, System.Linq.Expressions.ParameterExpression sourceTypeParameter)
        {
            Expression prop = Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "Weight");


            /*** Oracle fails when trying to convert a number to a string, so a result transformer is needed until the provider is fixed. ***/

            //var pi = selectType.GetProperty("Weight");
            //if (HasStratifications && pi.PropertyType == typeof(string))
            //{
            //    DTO.Enums.WeightStratification stratification;
            //    if (!Enum.TryParse<DTO.Enums.WeightStratification>(Stratifications.First().ToString(), out stratification))
            //    {
            //        throw new ArgumentException("Unable to parse the specified stratification value as an WeightStratification: " + Stratifications.First().ToString());
            //    }

            //    double stratificationValue = Convert.ToDouble((int)stratification);

            //    Expression fygString1 = Expressions.CallToString<double>(Expression.Multiply(Expression.Property(prop, "Value"), Expression.Constant(stratificationValue)));
            //    Expression fygStringJoin = Expression.Constant(" - ", typeof(string));
            //    Expression fygString2 = Expressions.CallToString<double>(Expression.Add(Expression.Multiply(Expression.Property(prop, "Value"), Expression.Constant(stratificationValue)), Expression.Constant(stratificationValue - 1d)));

            //    prop = Expressions.ConcatStrings(fygString1, fygStringJoin, fygString2);
            //}

            return new[] { 
                Expression.Bind(selectType.GetProperty("Weight"), prop)
            };
        }

        public Dictionary<string, object> Visit(Dictionary<string, object> row)
        {
            if (!HasStratifications)
                return row;

            object value;
            if (!row.TryGetValue("Weight", out value) || value == null)
                return row;

            double weight = Convert.ToDouble(value);


            DTO.Enums.WeightStratification stratification;
            if (!Enum.TryParse<DTO.Enums.WeightStratification>(Stratifications.First().ToString(), out stratification))
            {
                throw new ArgumentException("Unable to parse the specified stratification value as an WeightStratification: " + Stratifications.First().ToString());
            }
            double stratificationValue = Convert.ToDouble((int)stratification);

            row["Weight"] = string.Format(">={0:0.0} and <{1:0.0}", (weight * stratificationValue), (weight * stratificationValue) + stratificationValue);

            return row;
        }


        public void TransformPropertyDefinitions(List<IPropertyDefinition> definitions)
        {
            if (!HasStratifications)
                return;

            var weightPropertyDefinitions = definitions.Where(p => p.Name == "Weight");
            foreach (var propertyDefinition in weightPropertyDefinitions)
            {
                propertyDefinition.Type = typeof(string).FullName;
            }
        }
    }
}
