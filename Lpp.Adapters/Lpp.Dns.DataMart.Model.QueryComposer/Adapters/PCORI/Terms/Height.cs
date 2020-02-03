using Lpp.Objects.Dynamic;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI.Terms
{
    public class Height : TermImplementation, ITermResultTransformer
    {
        public Height(PCORIQueryBuilder.DataContext db)
            : base(ModelTermsFactory.HeightID, db)
        {
        }

        public override Objects.Dynamic.IPropertyDefinition[] InnerSelectPropertyDefinitions()
        {
            return new[]{
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "Height",
                        Type = typeof(double?).FullName
                    }
            };
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> InnerSelectBindings(Type selectType, System.Linq.Expressions.ParameterExpression sourceTypeParameter)
        {
            ParameterExpression pe_sourceQueryType = sourceTypeParameter;
            ParameterExpression pe_vitalsQueryType = Expression.Parameter(typeof(PCORIQueryBuilder.Model.Vital), "v");
            Expression heightSelector = Expression.Property(pe_vitalsQueryType, "Height");
            Expression heightSelector_Value = Expression.Property(heightSelector, "Value");

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
                //if the height term is included in critieria it enforces that the height value cannot be null
                predicates.Add(Expression.Equal(Expression.Property(heightSelector, "HasValue"), Expression.Constant(true)));

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
                MethodCallExpression heightWhere = Expressions.Where(vitalsProp, Expression.Lambda(predicateExpression, pe_vitalsQueryType));
                orderByMeasureOn = Expressions.OrderByAscending(heightWhere, Expression.Lambda(measureOnSelector, pe_vitalsQueryType));
            }
            
            MethodCallExpression heightSelect = Expression.Call(typeof(Queryable), "Select", new Type[] { pe_vitalsQueryType.Type, heightSelector.Type }, orderByMeasureOn, Expression.Quote(Expression.Lambda(heightSelector, pe_vitalsQueryType)));

            MethodCallExpression firstOrDefaultVitalHeight = Expressions.FirstOrDefault<double?>(heightSelect);

            if (!HasStratifications)
            {
                return new[] { 
                    Expression.Bind(selectType.GetProperty("Height"), firstOrDefaultVitalHeight)
                };
            }

            // apply the modifier for stratification
            DTO.Enums.HeightStratification stratification;
            if (!Enum.TryParse<DTO.Enums.HeightStratification>(Stratifications.First().ToString(), out stratification))
            {
                throw new ArgumentException("Unable to parse the specified stratification value as an HeightStratification: " + Stratifications.First().ToString());
            }

            var c1 = Expression.Equal(Expression.Property(firstOrDefaultVitalHeight, "HasValue"), Expression.Constant(true));
            var c2 = Expressions.MathFloor<double?>(Expression.Divide(Expression.Property(firstOrDefaultVitalHeight, "Value"), Expression.Constant(new Nullable<double>(Convert.ToDouble((int)stratification)))));
            //var c3 = Expression.Convert(Expression.Constant(null), typeof(double?));
            Expression stratificationExp = Expression.Condition(c1, c2, firstOrDefaultVitalHeight);

            return new[] { 
                    Expression.Bind(selectType.GetProperty("Height"), stratificationExp)
                };
        }

        public override Objects.Dynamic.IPropertyDefinition[] GroupKeyPropertyDefinitions()
        {
            return new[]{
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "Height",
                        Type = typeof(double?).FullName
                    }
            };
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> GroupKeySelectBindings(Type selectType, System.Linq.Expressions.ParameterExpression sourceTypeParameter)
        {
            return new[]{
                Expression.Bind(selectType.GetProperty("Height"), Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("Height")))
            };
        }

        public override Objects.Dynamic.IPropertyDefinition[] FinalSelectPropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "Height",
                    Type = typeof(double?).FullName
                    //Type = HasStratifications ? typeof(string).FullName : typeof(double?).FullName
                }
            };
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> FinalSelectBindings(Type selectType, System.Linq.Expressions.ParameterExpression sourceTypeParameter)
        {
            Expression prop = Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "Height");


            /*** Oracle fails when trying to convert a number to a string, so a result transformer is needed until the provider is fixed. ***/

            //var pi = selectType.GetProperty("Height");
            //if (HasStratifications && pi.PropertyType == typeof(string))
            //{
            //    DTO.Enums.HeightStratification stratification;
            //    if (!Enum.TryParse<DTO.Enums.HeightStratification>(Stratifications.First().ToString(), out stratification))
            //    {
            //        throw new ArgumentException("Unable to parse the specified stratification value as an HeightStratification: " + Stratifications.First().ToString());
            //    }

            //    double stratificationValue = Convert.ToDouble((int)stratification);

            //    Expression fygString1 = Expressions.CallToString<double>(Expression.Multiply(Expression.Property(prop, "Value"), Expression.Constant(stratificationValue)));
            //    Expression fygStringJoin = Expression.Constant(" - ", typeof(string));
            //    Expression fygString2 = Expressions.CallToString<double>(Expression.Add(Expression.Multiply(Expression.Property(prop, "Value"), Expression.Constant(stratificationValue)), Expression.Constant(stratificationValue - 1d)));

            //    //prop = Expressions.ConcatStrings(fygString1, fygStringJoin, fygString2);

            //    prop = Expression.Condition(Expression.Equal(prop, Expression.Constant(null)), Expression.Convert(Expression.Constant(null), typeof(string)), Expressions.ConcatStrings(fygString1, fygStringJoin, fygString2));
            //}

            return new[] { 
                Expression.Bind(selectType.GetProperty("Height"), prop)
            };
        }

        public Dictionary<string, object> Visit(Dictionary<string, object> row)
        {
            if (!HasStratifications)
                return row;

            object value;
            if (!row.TryGetValue("Height", out value) || value == null)
                return row;

            double height = Convert.ToDouble(value);


            DTO.Enums.HeightStratification stratification;
            if (!Enum.TryParse<DTO.Enums.HeightStratification>(Stratifications.First().ToString(), out stratification))
            {
                throw new ArgumentException("Unable to parse the specified stratification value as an HeightStratification: " + Stratifications.First().ToString());
            }
            double stratificationValue = Convert.ToDouble((int)stratification);

            row["Height"] = string.Format(">={0:0.0} and <{1:0.0}", (height * stratificationValue), (height * stratificationValue) + stratificationValue);

            return row;
        }


        public void TransformPropertyDefinitions(List<IPropertyDefinition> definitions)
        {
            if (!HasStratifications)
                return;

            var heightPropertyDefinitions = definitions.Where(p => p.Name == "Height");
            foreach (var propertyDefinition in heightPropertyDefinitions)
            {
                propertyDefinition.Type = typeof(string).FullName;
            }
        }
    }
}
