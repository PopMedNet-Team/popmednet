using Lpp.Objects.Dynamic;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI.Terms
{
    public class Setting : TermImplementation
    {
        public Setting(PCORIQueryBuilder.DataContext db)
            : base(ModelTermsFactory.SettingID, db)
        {
        }

        public override Objects.Dynamic.IPropertyDefinition[] InnerSelectPropertyDefinitions()
        {
            return new[]{
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "EncounterType",
                        Type = typeof(string).FullName
                    }
            };
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> InnerSelectBindings(Type selectType, System.Linq.Expressions.ParameterExpression sourceTypeParameter)
        {
            //the earliest  encounter date that satisfies the query critieria

            ParameterExpression pe_sourceQueryType = sourceTypeParameter;

            Expression encountersProp = Expressions.AsQueryable(Expression.Property(pe_sourceQueryType, "Encounters"));

            ParameterExpression pe_encountersQueryType = Expression.Parameter(typeof(PCORIQueryBuilder.Model.Encounter), "enc");

            Expression admittedOnSelector = Expression.Property(pe_encountersQueryType, "AdmittedOn");

            Expression encounterTypeSelector = Expression.Property(pe_encountersQueryType, "EncounterType");

            BinaryExpression be_encounterPatient = Expression.Equal(Expression.Property(pe_sourceQueryType, "ID"), Expression.Property(pe_encountersQueryType, "PatientID"));

            BinaryExpression predicate = be_encounterPatient;
            if (Criteria.Any(c => c.Terms.Any(t => t.Type == ModelTermsFactory.ObservationPeriodID) || c.Criteria.Any(ci => ci.Terms.Any(t => t.Type == ModelTermsFactory.ObservationPeriodID))))
            {
                var observationPeriodCriteria = Criteria.SelectMany(c => c.Terms.Where(t => t.Type == ModelTermsFactory.ObservationPeriodID)).Union(Criteria.SelectMany(c => c.Criteria.SelectMany(ci => ci.Terms.Where(t => t.Type == ModelTermsFactory.ObservationPeriodID))));
                foreach (var obpTerm in observationPeriodCriteria)
                {
                    var range = AdapterHelpers.ParseDateRangeValues(obpTerm);
                    if (range.StartDate.HasValue)
                    {
                        predicate = Expression.AndAlso(predicate, Expression.GreaterThanOrEqual(admittedOnSelector, Expression.Constant(range.StartDate.Value.DateTime.Date)));
                    }
                    if (range.EndDate.HasValue)
                    {
                        predicate = Expression.AndAlso(predicate, Expression.LessThanOrEqual(admittedOnSelector, Expression.Constant(range.EndDate.Value.DateTime.Date)));
                    }
                }
            }

            if (Criteria.Any(c => c.Terms.Any(t => t.Type == ModelTermsFactory.SettingID) || c.Criteria.Any(ci => ci.Terms.Any(t => t.Type == ModelTermsFactory.SettingID))))
            {
                var settingCriteria = Criteria.SelectMany(c => c.Terms.Where(t => t.Type == ModelTermsFactory.SettingID)).Union(Criteria.SelectMany(c => c.Criteria.SelectMany(ci => ci.Terms.Where(t => t.Type == ModelTermsFactory.SettingID)))); ;
                foreach (var term in settingCriteria)
                {
                    string value = term.GetStringValue("Setting");
                    DTO.Enums.Settings enumValue;
                    if (Enum.TryParse<DTO.Enums.Settings>(value, out enumValue))
                    {
                        value = enumValue.ToString("G");
                    }

                    if (enumValue == DTO.Enums.Settings.AN)
                    {
                        //any encounter setting that is not null or string.Empty.
                        predicate = Expression.AndAlso(predicate, Expression.AndAlso(Expression.NotEqual(encounterTypeSelector, Expression.Constant("", typeof(string))), Expression.NotEqual(encounterTypeSelector, Expression.Constant(null))));
                    }
                    else if (!string.IsNullOrEmpty(value))
                    {                        
                        predicate = Expression.AndAlso(predicate, Expression.Equal(encounterTypeSelector, Expression.Constant(value, typeof(string))));
                    }

                }
            }

            MethodCallExpression encounterTypeWhere = Expressions.Where(encountersProp, Expression.Lambda(predicate, pe_encountersQueryType));

            MethodCallExpression orderByAdmittedOn = Expressions.OrderByAscending(encounterTypeWhere, Expression.Lambda(admittedOnSelector, pe_encountersQueryType));

            MethodCallExpression encounterTypeSelect = Expressions.Select(pe_encountersQueryType.Type, encounterTypeSelector.Type, orderByAdmittedOn, Expression.Lambda(encounterTypeSelector, pe_encountersQueryType));

            MethodCallExpression firstOrDefaultEncounterType = Expressions.FirstOrDefault<string>(encounterTypeSelect);
            
            return new[] { 
                Expression.Bind(selectType.GetProperty("EncounterType"), firstOrDefaultEncounterType)
            };
        }        

        public override Objects.Dynamic.IPropertyDefinition[] GroupKeyPropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "EncounterType",
                    Type = typeof(string).FullName
                }
            };
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> GroupKeySelectBindings(Type selectType, System.Linq.Expressions.ParameterExpression sourceTypeParameter)
        {
            return new[]{
                Expression.Bind(selectType.GetProperty("EncounterType"), Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("EncounterType")))
            };
        }

        public override Objects.Dynamic.IPropertyDefinition[] FinalSelectPropertyDefinitions()
        {
            return new[]{
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "EncounterType",
                        Type = typeof(string).FullName
                    }
            };
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> FinalSelectBindings(Type selectType, System.Linq.Expressions.ParameterExpression sourceTypeParameter)
        {
            return new[] { Expression.Bind(selectType.GetProperty("EncounterType"), Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "EncounterType")) };
        }
    }
}
