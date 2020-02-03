using Lpp.Objects.Dynamic;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI.Terms
{
    public class EncounterObservationPeriod : TermImplementation, ITermResultTransformer
    {
        public EncounterObservationPeriod(PCORIQueryBuilder.DataContext db)
            : base(ModelTermsFactory.ObservationPeriodID, db)
        {
        }

        public override Objects.Dynamic.IPropertyDefinition[] InnerSelectPropertyDefinitions()
        {
            List<Objects.Dynamic.IPropertyDefinition> properties = new List<IPropertyDefinition>();
            properties.Add(
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                {
                    Name = "AdmittedOn",
                    Type = typeof(DateTime?).FullName
                }
            );

            if (HasStratifications)
            {
                properties.Add(
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "AdmittedOnYear",
                        Type = typeof(int?).FullName
                    }
                );
                properties.Add(
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "AdmittedOnMonth",
                        Type = typeof(int?).FullName
                    }
                );
            }


            return properties.ToArray();
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> InnerSelectBindings(Type selectType, System.Linq.Expressions.ParameterExpression sourceTypeParameter)
        {
            //the earliest  encounter date that satisfies the query critieria

            ParameterExpression pe_sourceQueryType = sourceTypeParameter;

            Expression encountersProp = Expressions.AsQueryable(Expression.Property(pe_sourceQueryType, "Encounters"));

            ParameterExpression pe_encountersQueryType = Expression.Parameter(typeof(PCORIQueryBuilder.Model.Encounter), "enc");

            Expression admittedOnSelector = Expression.Property(pe_encountersQueryType, "AdmittedOn");

            BinaryExpression be_encounterPatient = Expression.Equal(Expression.Property(pe_sourceQueryType, "ID"), Expression.Property(pe_encountersQueryType, "PatientID"));

            BinaryExpression predicate = be_encounterPatient;
            if (Criteria.Any(c => c.Terms.Any(t => t.Type == ModelTermsFactory.ObservationPeriodID) || c.Criteria.Any(ci => ci.Terms.Any(t => t.Type == ModelTermsFactory.ObservationPeriodID))))
            {
                var observationPeriodCriteria = Criteria.SelectMany(c => c.Terms.Where(t => t.Type == ModelTermsFactory.ObservationPeriodID)).Union(Criteria.SelectMany(c => c.Criteria.SelectMany(ci => ci.Terms.Where(t => t.Type == ModelTermsFactory.ObservationPeriodID)))); ;
                foreach (var obpTerm in observationPeriodCriteria)
                {
                    DateTime dateValue;
                    var range = AdapterHelpers.ParseDateRangeValues(obpTerm);
                    if (range.StartDate.HasValue)
                    {
                        dateValue = range.StartDate.Value.DateTime.Date;
                        predicate = Expression.AndAlso(predicate, Expression.GreaterThanOrEqual(admittedOnSelector, Expression.Constant(dateValue)));
                    }
                    if (range.EndDate.HasValue)
                    {
                        dateValue = range.EndDate.Value.DateTime.Date;
                        predicate = Expression.AndAlso(predicate, Expression.LessThanOrEqual(admittedOnSelector, Expression.Constant(dateValue)));
                    }
                }
            }
                        
            MethodCallExpression admittedOnWhere = Expressions.Where(encountersProp, Expression.Lambda(predicate, pe_encountersQueryType));

            MethodCallExpression orderByAdmittedOn = Expressions.OrderByAscending(admittedOnWhere, Expression.Lambda(admittedOnSelector, pe_encountersQueryType));

            //need to cast the return type of the select to a nullable datetime so that the FirstOrDefault will be null as the default
            MethodCallExpression admittedOnSelect = Expressions.Select(pe_encountersQueryType.Type, typeof(DateTime?), orderByAdmittedOn, Expression.Lambda(Expression.Convert(admittedOnSelector, typeof(DateTime?)), pe_encountersQueryType));

            MethodCallExpression firstOrDefaultAdmittedOn = Expressions.FirstOrDefault<DateTime?>(admittedOnSelect);

            if (!HasStratifications)
            {
                return new[] { 
                    Expression.Bind(selectType.GetProperty("AdmittedOn"), firstOrDefaultAdmittedOn)
                };
            }

            //if stratified by Month -> return as string in format yyyy-MM
            //if stratified by Year -> return as string in format yyyy

            DTO.Enums.PeriodStratification stratification;
            if (!Enum.TryParse<DTO.Enums.PeriodStratification>(Stratifications.First().ToString(), out stratification))
            {
                throw new ArgumentException("Unable to parse the specified stratification value as an PeriodStratification: " + Stratifications.First().ToString());
            }

            Expression prop = Expression.Property(firstOrDefaultAdmittedOn, "Value");
            //Expression yearPartString = Expressions.CallToString<int>(Expression.Property(prop, "Year"));
            //if (stratification == DTO.Enums.PeriodStratification.Monthly)
            //{
            //    //if stratified by Month -> return as string in format yyyy-MM                    
            //    Expression monthPartString = Expressions.CallToString<int>(Expression.Property(prop, "Month"));

            //    prop = Expressions.ConcatStrings(yearPartString, Expression.Constant("-"), monthPartString);
            //}
            //else if (stratification == DTO.Enums.PeriodStratification.Yearly)
            //{
            //    //if stratified by Year -> return as string in format yyyy
            //    prop = yearPartString;
            //}
            //else
            //{
            //    throw new NotSupportedException("The specified period stratifcation is not currently supported; stratification value: " + Stratifications.First().ToString());
            //}

            //Expression stratificationModifier = Expression.Condition(Expression.NotEqual(firstOrDefaultAdmittedOn, Expression.Constant(null)), prop, Expression.Constant("", typeof(string)));
            //return new[] { 
            //        Expression.Bind(selectType.GetProperty("AdmittedOn"), stratificationModifier)
            //    };

            Expression yearProp = Expression.Property(prop, "Year");
            Expression monthProp = Expression.Property(prop, "Month");

            return new[] { 
                Expression.Bind(selectType.GetProperty("AdmittedOn"), firstOrDefaultAdmittedOn),
                Expression.Bind(selectType.GetProperty("AdmittedOnYear"), yearProp),
                Expression.Bind(selectType.GetProperty("AdmittedOnMonth"), monthProp)
            };
        }

        public override Objects.Dynamic.IPropertyDefinition[] GroupKeyPropertyDefinitions()
        {
            //return new[]{
            //new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
            //    {
            //        Name = "AdmittedOn",
            //        Type = HasStratifications ? typeof(string).FullName : typeof(DateTime?).FullName
            //    }
            //};

            if (!HasStratifications)
            {
                return new[]{
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "AdmittedOn",
                        Type = typeof(DateTime?).FullName
                    }
                };
            }

            DTO.Enums.PeriodStratification stratification;
            if (!Enum.TryParse<DTO.Enums.PeriodStratification>(Stratifications.First().ToString(), out stratification))
            {
                throw new ArgumentException("Unable to parse the specified stratification value as an PeriodStratification: " + Stratifications.First().ToString());
            }


            List<Objects.Dynamic.IPropertyDefinition> properties = new List<IPropertyDefinition>();
            properties.Add(
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "AdmittedOnYear",
                        Type = typeof(int?).FullName
                    }
                );

            if (stratification == DTO.Enums.PeriodStratification.Monthly)
            {
                properties.Add(
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "AdmittedOnMonth",
                        Type = typeof(int?).FullName
                    }
                );
            }


            return properties.ToArray();
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> GroupKeySelectBindings(Type selectType, System.Linq.Expressions.ParameterExpression sourceTypeParameter)
        {
            if (!HasStratifications)
            {
                return new[]{
                    Expression.Bind(selectType.GetProperty("AdmittedOn"), Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("AdmittedOn")))
                };
            }

            DTO.Enums.PeriodStratification stratification;
            if (!Enum.TryParse<DTO.Enums.PeriodStratification>(Stratifications.First().ToString(), out stratification))
            {
                throw new ArgumentException("Unable to parse the specified stratification value as an PeriodStratification: " + Stratifications.First().ToString());
            }

            List<MemberAssignment> bindings = new List<MemberAssignment>();
            bindings.Add(Expression.Bind(selectType.GetProperty("AdmittedOnYear"), Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("AdmittedOnYear"))));

            if (stratification == DTO.Enums.PeriodStratification.Monthly)
            {
                bindings.Add(Expression.Bind(selectType.GetProperty("AdmittedOnMonth"), Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("AdmittedOnMonth"))));
            }

            return bindings;
        }

        public override Objects.Dynamic.IPropertyDefinition[] FinalSelectPropertyDefinitions()
        {
            if (!HasStratifications)
            {
                return new[]{
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                        {
                            Name = "AdmittedOn",
                            Type = HasStratifications ? typeof(string).FullName : typeof(DateTime?).FullName
                        }
                };
            }

            DTO.Enums.PeriodStratification stratification;
            if (!Enum.TryParse<DTO.Enums.PeriodStratification>(Stratifications.First().ToString(), out stratification))
            {
                throw new ArgumentException("Unable to parse the specified stratification value as an PeriodStratification: " + Stratifications.First().ToString());
            }


            List<Objects.Dynamic.IPropertyDefinition> properties = new List<IPropertyDefinition>();
            properties.Add(
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "AdmittedOnYear",
                        Type = typeof(int?).FullName
                    }
                );

            if (stratification == DTO.Enums.PeriodStratification.Monthly)
            {
                properties.Add(
                    new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "AdmittedOnMonth",
                        Type = typeof(int?).FullName
                    }
                );
            }


            return properties.ToArray();
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> FinalSelectBindings(Type selectType, System.Linq.Expressions.ParameterExpression sourceTypeParameter)
        {
            if (!HasStratifications)
            {
                Expression prop = Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "AdmittedOn");

                return new[] { 
                    Expression.Bind(selectType.GetProperty("AdmittedOn"), prop)
                };
            }

            DTO.Enums.PeriodStratification stratification;
            if (!Enum.TryParse<DTO.Enums.PeriodStratification>(Stratifications.First().ToString(), out stratification))
            {
                throw new ArgumentException("Unable to parse the specified stratification value as an PeriodStratification: " + Stratifications.First().ToString());
            }

            List<MemberAssignment> bindings = new List<MemberAssignment>();
            bindings.Add(Expression.Bind(selectType.GetProperty("AdmittedOnYear"), Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "AdmittedOnYear")));

            if (stratification == DTO.Enums.PeriodStratification.Monthly)
            {
                bindings.Add(Expression.Bind(selectType.GetProperty("AdmittedOnMonth"), Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "AdmittedOnMonth")));
            }

            return bindings;
        }

        public Dictionary<string, object> Visit(Dictionary<string, object> row)
        {
            if (!HasStratifications)
            {
                return row;
            }


            DTO.Enums.PeriodStratification stratification;
            if (!Enum.TryParse<DTO.Enums.PeriodStratification>(Stratifications.First().ToString(), out stratification))
            {
                throw new ArgumentException("Unable to parse the specified stratification value as an PeriodStratification: " + Stratifications.First().ToString());
            }


            if (stratification == DTO.Enums.PeriodStratification.Monthly)
            {
                row["AdmittedOn"] = string.Format("{0}-{1:00}", row["AdmittedOnYear"], row["AdmittedOnMonth"]);
                row.Remove("AdmittedOnMonth");
            }
            else
            {
                row["AdmittedOn"] = row["AdmittedOnYear"];
            }
            
            row.Remove("AdmittedOnYear");   

            return row;
        }


        public void TransformPropertyDefinitions(List<IPropertyDefinition> definitions)
        {
            if (!HasStratifications)
                return;

            //remove the year and month definitions
            if (definitions.Where(p => p.Name == "AdmittedOnMonth" || p.Name == "AdmittedOnYear").Any())
            {
                definitions.RemoveAll(p => p.Name == "AdmittedOnMonth" || p.Name == "AdmittedOnYear");
            }

            var definition = definitions.FirstOrDefault(p => p.Name == "AdmittedOn");
            if(definition == null){
                definitions.Add(new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                        {
                            Name = "AdmittedOn",
                            Type = typeof(string).FullName
                        });
            }else{
                definition.Type = typeof(string).FullName;
            }
        }
    }
}
