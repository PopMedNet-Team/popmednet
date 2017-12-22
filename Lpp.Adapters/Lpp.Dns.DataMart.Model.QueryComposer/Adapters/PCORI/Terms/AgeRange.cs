using Lpp.Objects.Dynamic;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using pcori = Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI.Terms
{
    public class AgeRange : TermImplementation, ITermResultTransformer
    {
        readonly Model.Settings.SQLProvider _sqlProvider;
        public AgeRange(PCORIQueryBuilder.DataContext db, Model.Settings.SQLProvider sqlProvider)
            : base(ModelTermsFactory.AgeRangeID, db)
        {
            _sqlProvider = sqlProvider;
        }

        public override Objects.Dynamic.IPropertyDefinition[] InnerSelectPropertyDefinitions()
        {
            //initial select will be always be numeric, Age will only be a string for the final select where it is the stratification text
            return new[]{
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "Age",
                        Type = typeof(int?).FullName
                    }
            };
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> InnerSelectBindings(Type selectType, System.Linq.Expressions.ParameterExpression sourceTypeParameter)
        {
            Expression bornOnExpression = Expression.Convert(Expression.Property(sourceTypeParameter, "BornOn"), typeof(DateTime?));
            Expression bornOnMonthExpression = Expressions.ChildPropertyExpression(bornOnExpression, "Value", "Month");

            Expression asOfExpression = BuildCalculationDateExpression(sourceTypeParameter, bornOnExpression, bornOnMonthExpression);
            Expression asOfMonthExpression = Expressions.ChildPropertyExpression(asOfExpression, "Value", "Month");


            Expression computeAge;
            if (_sqlProvider != Settings.SQLProvider.Oracle)
            {
                //build up the age modifier depending upon the caculation date and birth date, ie: if calculate date before birth date need to subtract a year from the year diff value

                Expression callDiffYears = Expressions.DbFunctionsDateDiff(bornOnExpression, asOfExpression);

                //if the birthdate is prior to the asof date
                Expression currentMonthDayGreaterThanExpression = Expression.AndAlso(
                        Expression.Equal(bornOnMonthExpression, asOfMonthExpression),
                        Expression.GreaterThan(Expressions.ChildPropertyExpression(bornOnExpression, "Value", "Day"), Expressions.ChildPropertyExpression(asOfExpression, "Value", "Day"))
                    );

                Expression gtBornOnMonth = Expression.GreaterThan(bornOnMonthExpression, asOfMonthExpression);
                Expression ageMod = Expression.Condition(Expression.OrElse(gtBornOnMonth, currentMonthDayGreaterThanExpression), Expression.Constant(1), Expression.Constant(0));

                Expression computeAge_Prior = Expression.Subtract(callDiffYears, Expression.Convert(ageMod, typeof(int?)));

                //if the birthdate is after the asof date
                Expression currentMonthDayLessThanExpression = Expression.AndAlso(
                        Expression.Equal(bornOnMonthExpression, asOfMonthExpression),
                        Expression.LessThan(Expressions.ChildPropertyExpression(bornOnExpression, "Value", "Day"), Expressions.ChildPropertyExpression(asOfExpression, "Value", "Day"))
                    );
                Expression ltBornOnMonth = Expression.LessThan(bornOnMonthExpression, asOfMonthExpression);
                Expression computeAge_After = Expression.Add(callDiffYears, Expression.Convert(Expression.Condition(Expression.OrElse(ltBornOnMonth, currentMonthDayLessThanExpression), Expression.Constant(1), Expression.Constant(0)), typeof(int?)));


                computeAge = Expression.Condition(Expression.GreaterThan(bornOnExpression, asOfExpression), computeAge_After, computeAge_Prior);
            }
            else
            {
                //Oracle does not return the same value for DiffYears as all the other providers....

                Expression ageMod = Expression.Condition(
                    Expression.AndAlso(Expression.GreaterThanOrEqual(bornOnMonthExpression, bornOnMonthExpression),
                        Expression.OrElse(Expression.GreaterThan(bornOnMonthExpression, asOfMonthExpression),
                            Expression.AndAlso(Expression.Equal(bornOnMonthExpression, asOfMonthExpression),
                                Expression.GreaterThan(Expressions.ChildPropertyExpression(bornOnExpression, "Value", "Day"), Expressions.ChildPropertyExpression(asOfExpression, "Value", "Day"))
                            )
                        )
                    ),
                    Expression.Constant(1),
                    Expression.Constant(0)
                );

                Expression bornOnYearExpression = Expressions.ChildPropertyExpression(bornOnExpression, "Value", "Year");
                Expression asOfYearExpression = Expressions.ChildPropertyExpression(asOfExpression, "Value", "Year");

                Expression computeAge_Prior = Expression.Convert(Expression.Subtract(Expression.Subtract(asOfYearExpression, bornOnYearExpression), ageMod), typeof(int?));

                //if the birthdate is after the asof date
                Expression currentMonthDayLessThanExpression = Expression.AndAlso(
                        Expression.Equal(bornOnMonthExpression, asOfMonthExpression),
                        Expression.LessThan(Expressions.ChildPropertyExpression(bornOnExpression, "Value", "Day"), Expressions.ChildPropertyExpression(asOfExpression, "Value", "Day"))
                    );
                Expression ltBornOnMonth = Expression.LessThan(bornOnMonthExpression, asOfMonthExpression);
                Expression computeAge_After = Expression.Add(Expression.Convert(Expression.Subtract(asOfYearExpression, bornOnYearExpression),typeof(int?)), Expression.Convert(Expression.Condition(Expression.OrElse(ltBornOnMonth, currentMonthDayLessThanExpression), Expression.Constant(1), Expression.Constant(0)), typeof(int?)));

                computeAge = Expression.Condition(Expression.GreaterThan(bornOnExpression, asOfExpression), computeAge_After, computeAge_Prior);
            }

            Expression toBind = computeAge;

            //at this point gets the computed age, now need to slot in to the stratification selected and create it's expressions
            if (HasStratifications)
            {
                DTO.Enums.AgeStratifications stratification;
                if (!Enum.TryParse<DTO.Enums.AgeStratifications>(Stratifications.First().ToString(), out stratification))
                {
                    throw new ArgumentException("Unable to parse the specified stratification value as an AgeStratification: " + Stratifications.First().ToString());
                }

                switch (stratification)
                {
                    case DTO.Enums.AgeStratifications.Ten:
                        //10 Stratifications (0-1,2-4,5-9,10-14,15-18,19-21,22-44,45-64,65-74,75+)
                        toBind = Expression.Condition(
                                Expression.LessThan(computeAge, Expression.Constant(2, typeof(int?))), Expression.Constant(0, typeof(int?)),
                                Expression.Condition(
                                    Expression.LessThan(computeAge, Expression.Constant(5, typeof(int?))), Expression.Constant(1, typeof(int?)),
                                    Expression.Condition(
                                        Expression.LessThan(computeAge, Expression.Constant(10, typeof(int?))), Expression.Constant(2, typeof(int?)),
                                        Expression.Condition(
                                            Expression.LessThan(computeAge, Expression.Constant(15, typeof(int?))), Expression.Constant(3, typeof(int?)),
                                            Expression.Condition(
                                                Expression.LessThan(computeAge, Expression.Constant(19, typeof(int?))), Expression.Constant(4, typeof(int?)),
                                                Expression.Condition(
                                                    Expression.LessThan(computeAge, Expression.Constant(22, typeof(int?))), Expression.Constant(5, typeof(int?)),
                                                    Expression.Condition(
                                                        Expression.LessThan(computeAge, Expression.Constant(45, typeof(int?))), Expression.Constant(6, typeof(int?)),
                                                        Expression.Condition(
                                                            Expression.LessThan(computeAge, Expression.Constant(65, typeof(int?))), Expression.Constant(7, typeof(int?)),
                                                            Expression.Condition(
                                                                Expression.LessThan(computeAge, Expression.Constant(75, typeof(int?))), Expression.Constant(8, typeof(int?)),
                                                                Expression.Constant(9, typeof(int?))
                                                            )
                                                        )
                                                    )
                                                )
                                            )
                                        )
                                    )
                                )
                            );
                        break;
                    case DTO.Enums.AgeStratifications.Seven:
                        //7 Stratifications (0-4,5-9,10-18,19-21,22-44,45-64,65+)
                        toBind = Expression.Condition(
                                Expression.LessThan(computeAge, Expression.Constant(5, typeof(int?))), Expression.Constant(0, typeof(int?)),
                                Expression.Condition(
                                    Expression.LessThan(computeAge, Expression.Constant(10, typeof(int?))), Expression.Constant(1, typeof(int?)),
                                    Expression.Condition(
                                        Expression.LessThan(computeAge, Expression.Constant(19, typeof(int?))), Expression.Constant(2, typeof(int?)),
                                        Expression.Condition(
                                            Expression.LessThan(computeAge, Expression.Constant(22, typeof(int?))), Expression.Constant(3, typeof(int?)),
                                            Expression.Condition(
                                                Expression.LessThan(computeAge, Expression.Constant(45, typeof(int?))), Expression.Constant(4, typeof(int?)),
                                                Expression.Condition(
                                                    Expression.LessThan(computeAge, Expression.Constant(65, typeof(int?))), Expression.Constant(5, typeof(int?)),
                                                    Expression.Constant(6, typeof(int?))
                                                )
                                            )
                                        )
                                    )
                                )
                            );
                        break;
                    case DTO.Enums.AgeStratifications.Four:
                        //4 Stratifications (0-21,22-44,45-64,65+)
                        toBind = Expression.Condition(
                                Expression.LessThan(computeAge, Expression.Constant(22, typeof(int?))), Expression.Constant(0, typeof(int?)),
                                Expression.Condition(
                                    Expression.LessThan(computeAge, Expression.Constant(45, typeof(int?))), Expression.Constant(1, typeof(int?)),
                                    Expression.Condition(
                                        Expression.LessThan(computeAge, Expression.Constant(65, typeof(int?))), Expression.Constant(2, typeof(int?)),
                                        Expression.Constant(3, typeof(int?))
                                   )
                                )
                            );
                        break;
                    case DTO.Enums.AgeStratifications.Two:
                        //2 Stratifications (Under 65,65+)
                        toBind = Expression.Condition(Expression.LessThan(computeAge, Expression.Constant(65, typeof(int?))), Expression.Constant((int?)0, typeof(int?)), Expression.Constant((int?)1, typeof(int?)));
                        break;
                    case DTO.Enums.AgeStratifications.None:
                        //no stratification on the age, use the computedAge as the bound expression
                        toBind = computeAge;
                        break;
                    case DTO.Enums.AgeStratifications.FiveYearGrouping:

                        toBind = Expressions.MathFloor<int?>(Expression.Divide(Expression.Convert(computeAge, typeof(decimal)), Expression.Constant(5m)));

                        break;
                    case DTO.Enums.AgeStratifications.TenYearGrouping:

                        toBind = Expressions.MathFloor<int?>(Expression.Divide(Expression.Convert(computeAge, typeof(decimal)), Expression.Constant(10m)));

                        break;
                    default:
                        throw new ArgumentOutOfRangeException("stratification", stratification, "Action for the specified age stratification could not be determined.");

                }
            }

            return new[]{
                    Expression.Bind(selectType.GetProperty("Age"), toBind)
                };
        }

        public override Objects.Dynamic.IPropertyDefinition[] GroupKeyPropertyDefinitions()
        {
            return new[]{
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "Age",
                        Type = typeof(int?).FullName
                    }
            };
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> GroupKeySelectBindings(Type selectType, System.Linq.Expressions.ParameterExpression sourceTypeParameter)
        {
            return new[]{
                Expression.Bind(selectType.GetProperty("Age"), sourceTypeParameter.MemberExpression("Age"))
            };
        }

        public override Objects.Dynamic.IPropertyDefinition[] FinalSelectPropertyDefinitions()
        {
            //return the field as a string with the stratification description
            //no stratification, return as a numeric value
            string selectTypeName = typeof(int?).FullName;

            if (HasStratifications)
            {

                DTO.Enums.AgeStratifications stratification;
                if (!Enum.TryParse<DTO.Enums.AgeStratifications>(Stratifications.First().ToString(), out stratification))
                {
                    throw new ArgumentException("Unable to parse the specified stratification value as an AgeStratification: " + Stratifications.First().ToString());
                }

                if (stratification == DTO.Enums.AgeStratifications.None ||
                   stratification == DTO.Enums.AgeStratifications.FiveYearGrouping ||
                   stratification == DTO.Enums.AgeStratifications.TenYearGrouping)
                {
                    selectTypeName = typeof(int?).FullName;
                }
                else
                {
                    selectTypeName = typeof(string).FullName;
                }

            }


            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "Age",
                    Type = selectTypeName
                }
            };
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> FinalSelectBindings(Type selectType, System.Linq.Expressions.ParameterExpression sourceTypeParameter)
        {
            //There is no good way to determine if the type of sourceTypeParameter is an IGrouping, so assume that it will always be grouped
            MemberExpression keyProp = Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("Key"));
            Expression innerProp = Expression.Property(keyProp, keyProp.Type.GetProperty("Age"));

            var pi = selectType.GetProperty("Age");

            if (HasStratifications && pi.PropertyType == typeof(string))
            {
                //if a stratification was specified, translate the stratification value into text

                DTO.Enums.AgeStratifications stratification;
                if (!Enum.TryParse<DTO.Enums.AgeStratifications>(Stratifications.First().ToString(), out stratification))
                {
                    throw new ArgumentException("Unable to parse the specified stratification value as an AgeStratification: " + Stratifications.First().ToString());
                }

                MemberExpression initialValue = Expression.Property(keyProp, keyProp.Type.GetProperty("Age"));

                switch (stratification)
                {
                    case DTO.Enums.AgeStratifications.Ten:
                        //10 Stratifications (0-1,2-4,5-9,10-14,15-18,19-21,22-44,45-64,65-74,75+)
                        innerProp = Expression.Condition(
                                Expression.Equal(initialValue, Expression.Constant(0, typeof(int?))), Expression.Constant("0-1", typeof(string)),
                                Expression.Condition(
                                    Expression.Equal(initialValue, Expression.Constant(1, typeof(int?))), Expression.Constant("2-4", typeof(string)),
                                    Expression.Condition(
                                        Expression.Equal(initialValue, Expression.Constant(2, typeof(int?))), Expression.Constant("5-9", typeof(string)),
                                        Expression.Condition(
                                            Expression.Equal(initialValue, Expression.Constant(3, typeof(int?))), Expression.Constant("10-14", typeof(string)),
                                            Expression.Condition(
                                                Expression.Equal(initialValue, Expression.Constant(4, typeof(int?))), Expression.Constant("15-18", typeof(string)),
                                                Expression.Condition(
                                                    Expression.Equal(initialValue, Expression.Constant(5, typeof(int?))), Expression.Constant("19-21", typeof(string)),
                                                    Expression.Condition(
                                                        Expression.Equal(initialValue, Expression.Constant(6, typeof(int?))), Expression.Constant("22-44", typeof(string)),
                                                        Expression.Condition(
                                                            Expression.Equal(initialValue, Expression.Constant(7, typeof(int?))), Expression.Constant("45-64", typeof(string)),
                                                            Expression.Condition(
                                                                Expression.Equal(initialValue, Expression.Constant(8, typeof(int?))), Expression.Constant("65-74", typeof(string)),
                                                                Expression.Constant("75+", typeof(string))
                                                            )
                                                        )
                                                    )
                                                )
                                            )
                                        )
                                    )
                                )
                            );
                        break;
                    case DTO.Enums.AgeStratifications.Seven:
                        //7 Stratifications (0-4,5-9,10-18,19-21,22-44,45-64,65+)
                        innerProp = Expression.Condition(
                                Expression.Equal(initialValue, Expression.Constant(0, typeof(int?))), Expression.Constant("0-4", typeof(string)),
                                Expression.Condition(
                                    Expression.Equal(initialValue, Expression.Constant(1, typeof(int?))), Expression.Constant("5-9", typeof(string)),
                                    Expression.Condition(
                                        Expression.Equal(initialValue, Expression.Constant(2, typeof(int?))), Expression.Constant("10-18", typeof(string)),
                                        Expression.Condition(
                                            Expression.Equal(initialValue, Expression.Constant(3, typeof(int?))), Expression.Constant("19-21", typeof(string)),
                                            Expression.Condition(
                                                Expression.Equal(initialValue, Expression.Constant(4, typeof(int?))), Expression.Constant("22-44", typeof(string)),
                                                Expression.Condition(
                                                    Expression.Equal(initialValue, Expression.Constant(5, typeof(int?))), Expression.Constant("45-64", typeof(string)),
                                                    Expression.Constant("65+", typeof(string))
                                                )
                                            )
                                        )
                                    )
                                )
                            );

                        break;
                    case DTO.Enums.AgeStratifications.Four:
                        //4 Stratifications (0-21,22-44,45-64,65+)
                        innerProp = Expression.Condition(
                                Expression.Equal(initialValue, Expression.Constant(0, typeof(int?))), Expression.Constant("0-21", typeof(string)),
                                Expression.Condition(
                                    Expression.Equal(initialValue, Expression.Constant(1, typeof(int?))), Expression.Constant("22-44", typeof(string)),
                                    Expression.Condition(
                                        Expression.Equal(initialValue, Expression.Constant(2, typeof(int?))), Expression.Constant("45-64", typeof(string)),
                                        Expression.Constant("65+", typeof(string))
                                   )
                                )
                            );
                        break;
                    case DTO.Enums.AgeStratifications.Two:
                        //2 Stratifications (Under 65,65+)
                        innerProp = Expression.Condition(Expression.Equal(initialValue, Expression.Constant(0, typeof(int?))), Expression.Constant("Under 65", typeof(string)), Expression.Constant("65+", typeof(string)));
                        break;
                    case DTO.Enums.AgeStratifications.None:
                        //no stratification on the age, use the computedAge as the bound expression converted to a string
                        //innerProp = Expression.Call(Expression.Property(initialValue, "Value"), typeof(int).GetMethod("ToString", Type.EmptyTypes));
                        break;
                    case DTO.Enums.AgeStratifications.FiveYearGrouping:

                        //Expression fygString1 = Expressions.CallToString<int>(Expression.Multiply(Expression.Property(initialValue, "Value"), Expression.Constant(5)));
                        //Expression fygStringJoin = Expression.Constant(" - ", typeof(string));
                        //Expression fygString2 = Expressions.CallToString<int>(Expression.Add(Expression.Multiply(Expression.Property(initialValue, "Value"), Expression.Constant(5)), Expression.Constant(4)));

                        //innerProp = Expressions.ConcatStrings(fygString1, fygStringJoin, fygString2);

                        break;
                    case DTO.Enums.AgeStratifications.TenYearGrouping:

                        //Expression tygString1 = Expressions.CallToString<int>(Expression.Multiply(Expression.Property(initialValue, "Value"), Expression.Constant(10)));
                        //Expression tygStringJoin = Expression.Constant(" - ", typeof(string));
                        //Expression tygString2 = Expressions.CallToString<int>(Expression.Add(Expression.Multiply(Expression.Property(initialValue, "Value"), Expression.Constant(10)), Expression.Constant(9)));

                        //innerProp = Expressions.ConcatStrings(tygString1, tygStringJoin, tygString2);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException("stratification", stratification, "Action for the specified age stratification could not be determined.");

                }

            }

            return new[] { 
                Expression.Bind(selectType.GetProperty("Age"), innerProp)
            };
        }

        Expression BuildCalculationDateExpression(ParameterExpression pe_sourceQueryType, Expression bornOnExpression, Expression bornOnMonthExpression)
        {
            //use the first age range critieria
            var ageRangeTerms = Criteria.SelectMany(c => c.Terms.Where(t => t.Type == ModelTermsFactory.AgeRangeID).Concat(c.Criteria.SelectMany(cc => cc.Terms.Where(t => t.Type == ModelTermsFactory.AgeRangeID)))).ToArray();
            AgeRangeValues rangeValues = AdapterHelpers.ParseAgeRangeValues(ageRangeTerms, null).FirstOrDefault();
            if (rangeValues == null || rangeValues.CalculationType == null)
            {
                return Expression.Convert(Expression.Constant(DateTime.Now), typeof(DateTime?));
            }

            //based on the value expressed by the age range criteria determine the calculate as of date
            if (rangeValues.CalculationType.Value == DTO.Enums.AgeRangeCalculationType.AsOfDateOfRequestSubmission)
            {
                return Expression.Convert(Expression.Constant(DateTime.Now), typeof(DateTime?));
            }

            if (rangeValues.CalculationType.Value == DTO.Enums.AgeRangeCalculationType.AsOfSpecifiedDate)
            {
                DateTime asOf = rangeValues.CalculateAsOf.Value.Date;
                return Expression.Convert(Expression.Constant(asOf), typeof(DateTime?));
            }

            var primaryCriteria = Criteria.FirstOrDefault();
            if (primaryCriteria == null)
                throw new ArgumentException("Unable to determine the primary criteria to build the age select expression.");

            if (rangeValues.CalculationType.Value == DTO.Enums.AgeRangeCalculationType.AtLastEncounterWithinHealthSystem)
            {
                Expression encountersProp = Expressions.AsQueryable(Expression.Property(pe_sourceQueryType, "Encounters"));
                ParameterExpression pe_encounterQuery = Expression.Parameter(typeof(PCORIQueryBuilder.Model.Encounter), "enc");

                //apply predicate to limit to a specific patient
                BinaryExpression be_encounterPatient = Expression.Equal(Expression.Property(pe_sourceQueryType, "ID"), Expression.Property(pe_encounterQuery, "PatientID"));
                MethodCallExpression encWhere = Expressions.Where(encountersProp, Expression.Lambda(be_encounterPatient, pe_encounterQuery));

                //apply the order by descending against admit date
                Expression admittedOnSelector = Expression.Property(pe_encounterQuery, "AdmittedOn");
                MethodCallExpression orderByAdmittedOn = Expressions.OrderByDescending(encWhere, Expression.Lambda(admittedOnSelector, pe_encounterQuery));

                //apply the select of the date values
                //need to cast the return type of the select to a nullable datetime so that the FirstOrDefault will be null as the default
                MethodCallExpression admittedOnSelect = Expressions.Select(pe_encounterQuery.Type, typeof(DateTime?), orderByAdmittedOn, Expression.Lambda(Expression.Convert(admittedOnSelector, typeof(DateTime?)), pe_encounterQuery));

                //apply the firstordefault
                MethodCallExpression firstOrDefaultAdmittedOn = Expressions.FirstOrDefault<DateTime?>(admittedOnSelect);
                return firstOrDefaultAdmittedOn;
            }

            if (rangeValues.CalculationType.Value == DTO.Enums.AgeRangeCalculationType.AsOfObservationPeriodStartDateWithinCriteriaGroup)
            {
                var observation = PCORIModelAdapter.GetAllCriteriaTerms(primaryCriteria, ModelTermsFactory.ObservationPeriodID);
                var observationPeriodValues = observation.Select(t => AdapterHelpers.ParseDateRangeValues(t)).Where(rv => rv.StartDate.HasValue).FirstOrDefault();
                //var observationPeriodValues = primaryCriteria.Terms.Where(t => t.Type == ModelTermsFactory.ObservationPeriodID).Select(t => AdapterHelpers.ParseDateRangeValues(t)).Where(rv => rv.StartDate.HasValue).FirstOrDefault();

                if (observationPeriodValues == null)
                    throw new ArgumentException("Unable to calculate age based on Observation Period Start Date, a value has not been specified for any Observation Period terms within the primary criteria group.");

                DateTime observationPeriodStartDate = observationPeriodValues.StartDate.Value.Date;
                return Expression.Convert(Expression.Constant(observationPeriodStartDate), typeof(DateTime?));
            }

            if (rangeValues.CalculationType.Value == DTO.Enums.AgeRangeCalculationType.AsOfObservationPeriodEndDateWithinCriteriaGroup)
            {
                var observation = PCORIModelAdapter.GetAllCriteriaTerms(primaryCriteria, ModelTermsFactory.ObservationPeriodID);
                var observationPeriodValues = observation.Select(t => AdapterHelpers.ParseDateRangeValues(t)).Where(rv => rv.StartDate.HasValue).FirstOrDefault();

                if (observationPeriodValues == null)
                    throw new ArgumentException("Unable to calculate age based on Observation Period End Date, a value has not been specified for any Observation Period terms within the primary criteria group.");

                DateTime observationPeriodEndDate = observationPeriodValues.EndDate.Value.Date;
                return Expression.Convert(Expression.Constant(observationPeriodEndDate), typeof(DateTime?));
            }

            /*The last two calculation types are based on encounters limited by the groups critieria, need to apply all the criteria to limit the encouters and then select the appropriate date*/

            return BuildEncounterCriteriaExpression(pe_sourceQueryType, primaryCriteria, rangeValues.CalculationType.Value, bornOnExpression, bornOnMonthExpression);
        }

        Expression BuildEncounterCriteriaExpression(ParameterExpression pe_sourceQueryType, DTO.QueryComposer.QueryComposerCriteriaDTO primaryCriteria, DTO.Enums.AgeRangeCalculationType calculationType, Expression bornOnExpression, Expression bornOnMonthExpression)
        {
            Expression encountersProp = Expressions.AsQueryable(Expression.Property(pe_sourceQueryType, "Encounters"));
            ParameterExpression pe_encountersQueryType = Expression.Parameter(typeof(PCORIQueryBuilder.Model.Encounter), "enc");

            Expression vitalsProp = Expressions.AsQueryable(Expression.Property(pe_sourceQueryType, "Vitals"));
            ParameterExpression pe_vitalsQueryType = Expression.Parameter(typeof(PCORIQueryBuilder.Model.Vital));

            Expression admittedOnSelector = Expression.Property(pe_encountersQueryType, "AdmittedOn");
            Expression admittedOnMonthSelector = Expression.Property(admittedOnSelector, "Month");

            Expression encounterTypeSelector = Expression.Property(pe_encountersQueryType, "EncounterType");

            //kind of a redundant expression to start with since going through the patients encounters property anyhow, but give a safe place to start building the predicate
            BinaryExpression predicate = Expression.Equal(Expression.Property(pe_sourceQueryType, "ID"), Expression.Property(pe_encountersQueryType, "PatientID"));

            var encounterTerms = (primaryCriteria.Terms.Concat(primaryCriteria.Criteria.SelectMany(c => c.Terms))).Where(t => t.Type == ModelTermsFactory.AgeRangeID || t.Type == ModelTermsFactory.SettingID || t.Type == ModelTermsFactory.ObservationPeriodID).ToArray();
            foreach (var encTerm in encounterTerms)
            {
                if (encTerm.Type == ModelTermsFactory.AgeRangeID)
                {
                    //Expression bornOnExpression = Expression.Convert(Expression.Property(pe_sourceQueryType, "BornOn"), typeof(DateTime?));
                    //Expression asOfExpression = admittedOnSelector;
                    //Expression bornOnMonthExpression = Expressions.ChildPropertyExpression(bornOnExpression, "Value", "Month");
                    //Expression asOfMonthExpression = Expression.Property(asOfExpression, "Month");

                    //Expression dateNowMonthExpression = Expressions.ChildPropertyExpression(asOfExpression, "Value", "Month");

                    Expression callDiffYears = Expressions.DbFunctionsDateDiff(bornOnExpression, Expression.Convert(admittedOnSelector, typeof(DateTime?)));

                    //build up the age modifier depending upon the caculation date and birth date, ie: if calculate date before birth date need to subtract a year from the year diff value
                    Expression currentMonthDayGreaterThanExpression = Expression.AndAlso(
                            Expression.Equal(bornOnMonthExpression, admittedOnMonthSelector),
                            Expression.GreaterThan(Expressions.ChildPropertyExpression(bornOnExpression, "Value", "Day"), Expression.Property(admittedOnSelector, "Day"))
                        );

                    Expression gtBornOnMonth = Expression.GreaterThan(bornOnMonthExpression, admittedOnMonthSelector);
                    Expression ageMod = Expression.Condition(Expression.OrElse(gtBornOnMonth, currentMonthDayGreaterThanExpression), Expression.Constant(1), Expression.Constant(0));
                    Expression computeAge = Expression.Subtract(callDiffYears, Expression.Convert(ageMod, typeof(int?)));

                    AgeRangeValues ageRange = AdapterHelpers.ParseAgeRangeValues(encTerm);
                    int age = 0;
                    if (ageRange.MinAge.HasValue)
                    {
                        age = ageRange.MinAge.Value;
                        predicate = Expression.AndAlso(predicate, Expression.GreaterThanOrEqual(computeAge, Expression.Constant(ageRange.MinAge, typeof(int?))));
                    }
                    if (ageRange.MaxAge.HasValue)
                    {
                        age = ageRange.MaxAge.Value;
                        predicate = Expression.AndAlso(predicate, Expression.LessThanOrEqual(computeAge, Expression.Constant(ageRange.MaxAge, typeof(int?))));
                    }
                }
                else if (encTerm.Type == ModelTermsFactory.SettingID)
                {
                    string value = encTerm.GetStringValue("Setting");
                    DTO.Enums.Settings enumValue;
                    if (Enum.TryParse<DTO.Enums.Settings>(value, out enumValue))
                    {
                        value = enumValue.ToString("G");
                    }

                    if (enumValue == DTO.Enums.Settings.AN)
                    {
                        predicate = Expression.AndAlso(predicate, Expressions.StringIsNullOrEmpty(encounterTypeSelector));
                    }
                    else
                    {
                        predicate = Expression.AndAlso(predicate, Expression.Equal(encounterTypeSelector, Expression.Constant(value, typeof(string))));
                    }
                }
                else if (encTerm.Type == ModelTermsFactory.ObservationPeriodID)
                {
                    DateTime dateValue;
                    var range = AdapterHelpers.ParseDateRangeValues(encTerm);
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

            var vitalsTerms = primaryCriteria.Terms.Where(t => t.Type == ModelTermsFactory.HeightID || t.Type == ModelTermsFactory.WeightID || t.Type == ModelTermsFactory.VitalsMeasureDateID).ToArray();
            if (vitalsTerms.Length > 0)
            {
                BinaryExpression vitalsPredicate = Expression.Equal(Expression.Property(pe_encountersQueryType, "ID"), Expression.Property(pe_vitalsQueryType, "EncounterID"));
                foreach (var vitalTerm in vitalsTerms)
                {
                    //build an any against the encounters related vitals matching the critieria
                    if (vitalTerm.Type == ModelTermsFactory.HeightID)
                    {
                        var heightRange = AdapterHelpers.ParseHeightValues(vitalTerm);
                        if (heightRange.HasValues)
                        {
                            Expression heightSelector = Expression.Property(pe_vitalsQueryType, "Height");
                            if (heightRange.MinHeight.HasValue)
                            {
                                vitalsPredicate = Expression.AndAlso(vitalsPredicate, Expression.GreaterThanOrEqual(heightSelector, Expression.Constant(heightRange.MinHeight.Value, typeof(double?))));
                            }
                            if (heightRange.MaxHeight.HasValue)
                            {
                                vitalsPredicate = Expression.AndAlso(vitalsPredicate, Expression.LessThanOrEqual(heightSelector, Expression.Constant(heightRange.MaxHeight.Value, typeof(double?))));
                            }
                        }

                    }
                    else if (vitalTerm.Type == ModelTermsFactory.WeightID)
                    {
                        var weightRange = AdapterHelpers.ParseWeightValues(vitalTerm);
                        if (weightRange.HasValues)
                        {
                            Expression weightSelector = Expression.Property(pe_vitalsQueryType, "Weight");
                            if (weightRange.MinWeight.HasValue)
                            {
                                vitalsPredicate = Expression.AndAlso(vitalsPredicate, Expression.GreaterThanOrEqual(weightSelector, Expression.Constant(weightRange.MinWeight.Value, typeof(double?))));
                            }
                            if (weightRange.MaxWeight.HasValue)
                            {
                                vitalsPredicate = Expression.AndAlso(vitalsPredicate, Expression.LessThanOrEqual(weightSelector, Expression.Constant(weightRange.MaxWeight.Value, typeof(double?))));
                            }
                        }
                    }
                    else if (vitalTerm.Type == ModelTermsFactory.VitalsMeasureDateID)
                    {
                        var md_values = AdapterHelpers.ParseDateRangeValues(vitalTerm);
                        Expression measuredOnSelector = Expression.Property(pe_vitalsQueryType, "MeasuredOn");
                        Expression md_predicate = null;
                        if (md_values.StartDate.HasValue && md_values.EndDate.HasValue)
                        {
                            md_predicate = Expression.AndAlso(Expression.GreaterThanOrEqual(measuredOnSelector, Expression.Constant(md_values.StartDate.Value.Date)), Expression.LessThanOrEqual(measuredOnSelector, Expression.Constant(md_values.EndDate.Value.Date)));
                        }
                        else if (md_values.StartDate.HasValue)
                        {
                            md_predicate = Expression.GreaterThanOrEqual(measuredOnSelector, Expression.Constant(md_values.StartDate.Value.Date));

                        }
                        else if (md_values.EndDate.HasValue)
                        {
                            md_predicate = Expression.LessThanOrEqual(measuredOnSelector, Expression.Constant(md_values.EndDate.Value.Date));
                        }

                        if (md_predicate != null)
                            vitalsPredicate = Expression.AndAlso(vitalsPredicate, md_predicate);
                    }
                }

                //apply the vitals predicate to the main predicate
                predicate = Expression.AndAlso(predicate, vitalsPredicate);
            }

            var diagnosisTerms = (primaryCriteria.Terms.Concat(primaryCriteria.Criteria.SelectMany(t => t.Terms))).Where(t => t.Type == ModelTermsFactory.CombinedDiagnosisCodesID || t.Type == ModelTermsFactory.ICD9DiagnosisCodes3digitID).ToArray();
            if (diagnosisTerms.Length > 0)
            {
                throw new NotImplementedException("All code terms should be OR'd together and then AND'd to the main predicate, need to update.");

                Expression pe_diagnosisQueryType = Expression.Property(pe_sourceQueryType, "Diagnoses");

                //pe_diagnosisQueryType is an ICollection<Diagnosis>, need to redo so that it is actually quering correctly

                Expression codeTypeSelector = Expression.Property(pe_diagnosisQueryType, "CodeType");
                Expression codeSelector = Expression.Property(pe_diagnosisQueryType, "Code");

                foreach (var diagTerm in diagnosisTerms)
                {
                    if (diagTerm.Type == ModelTermsFactory.CombinedDiagnosisCodesID)
                    {
                        DTO.Enums.DiagnosisCodeTypes codeType;
                        if (!Enum.TryParse<DTO.Enums.DiagnosisCodeTypes>(diagTerm.GetStringValue("CodeType"), out codeType))
                        {
                            codeType = DTO.Enums.DiagnosisCodeTypes.Any;
                        }

                        DTO.Enums.TextSearchMethodType searchMethod;
                        if (!Enum.TryParse<DTO.Enums.TextSearchMethodType>(diagTerm.GetStringValue("SearchMethodType"), out searchMethod))
                        {
                            searchMethod = DTO.Enums.TextSearchMethodType.ExactMatch;
                        }

                        var codes = (diagTerm.GetStringValue("CodeValues") ?? "").Split(new[] { ';' }).Select(s => s.Trim()).Distinct().ToArray();
                        if (codes.Length == 0)
                            continue;

                        BinaryExpression diagnosisPredicate = Expression.Equal(Expression.Property(pe_encountersQueryType, "ID"), Expression.Property(pe_diagnosisQueryType, "EncounterID"));
                        if (codeType != DTO.Enums.DiagnosisCodeTypes.Any)
                        {
                            string translatedCode = Terms.CombinedDiagnosisCodes.FromDiagnosisCodeType(codeType);
                            diagnosisPredicate = Expression.AndAlso(diagnosisPredicate, Expression.Equal(codeTypeSelector, Expression.Constant(translatedCode)));
                        }

                        BinaryExpression codesValuesPredicate = null;
                        foreach (var code in codes)
                        {
                            BinaryExpression codePredicate = null;
                            if (searchMethod == DTO.Enums.TextSearchMethodType.ExactMatch)
                            {
                                codePredicate = Expression.Equal(codeSelector, Expression.Constant(code));
                            }
                            else if (searchMethod == DTO.Enums.TextSearchMethodType.StartsWith)
                            {
                                codePredicate = Expressions.StringStartsWith(codeSelector, code);
                            }

                            if (codePredicate == null)
                            {
                                continue;
                            }
                            else if (codesValuesPredicate == null)
                            {
                                codesValuesPredicate = codePredicate;
                            }
                            else
                            {
                                codesValuesPredicate = Expression.OrElse(codesValuesPredicate, codePredicate);
                            }
                        }

                        diagnosisPredicate = Expression.AndAlso(diagnosisPredicate, codesValuesPredicate);

                        //apply the diagnosis predicate against the main predicate
                        predicate = Expression.AndAlso(predicate, diagnosisPredicate);
                    }
                    else if (diagTerm.Type == ModelTermsFactory.ICD9DiagnosisCodes3digitID)
                    {
                        string[] codes = AdapterHelpers.ParseCodeTermValues(diagTerm).ToArray();

                        if (codes.Length == 0)
                        {
                            continue;
                        }

                        //limit to ICD9 codes
                        BinaryExpression icd9Predicate = Expression.Equal(codeTypeSelector, Expression.Constant("09"));

                        BinaryExpression codeValuesPredicate = null;
                        foreach (var code in codes)
                        {
                            string codeWithoutDecimal = code.Replace(".", "");
                            BinaryExpression pred = Expression.OrElse(Expressions.StringStartsWith(codeSelector, code), Expressions.StringStartsWith(codeSelector, codeWithoutDecimal));

                            codeValuesPredicate = codeValuesPredicate == null ? pred : Expression.OrElse(codeValuesPredicate, pred);
                        }

                        icd9Predicate = Expression.AndAlso(icd9Predicate, codeValuesPredicate);
                        predicate = Expression.AndAlso(predicate, icd9Predicate);
                    }
                }
            }


            MethodCallExpression admittedOnWhere = Expressions.Where(encountersProp, Expression.Lambda(predicate, pe_encountersQueryType));

            MethodCallExpression orderByAdmittedOn;
            if (calculationType == DTO.Enums.AgeRangeCalculationType.AtFirstMatchingEncounterWithinCriteriaGroup)
            {
                //first encounter by admitted on, order by admitted on ascending - oldest first
                orderByAdmittedOn = Expressions.OrderByAscending(admittedOnWhere, Expression.Lambda(admittedOnSelector, pe_encountersQueryType));
            }
            else
            {
                //last encounter by admitted on, order by admitted on descending - newest first
                orderByAdmittedOn = Expressions.OrderByDescending(admittedOnWhere, Expression.Lambda(admittedOnSelector, pe_encountersQueryType));
            }

            ////need to cast the return type of the select to a nullable datetime so that the FirstOrDefault will be null as the default
            MethodCallExpression admittedOnSelect = Expressions.Select(pe_encountersQueryType.Type, typeof(DateTime?), orderByAdmittedOn, Expression.Lambda(Expression.Convert(admittedOnSelector, typeof(DateTime?)), pe_encountersQueryType));

            MethodCallExpression firstOrDefaultAdmittedOn = Expressions.FirstOrDefault<DateTime?>(admittedOnSelect);

            return firstOrDefaultAdmittedOn;
        }

        public Dictionary<string, object> Visit(Dictionary<string, object> row)
        {
            if (!HasStratifications)
                return row;

            object value;
            if (!row.TryGetValue("Age", out value) || value == null)
                return row;

            DTO.Enums.AgeStratifications stratification;
            if (!Enum.TryParse<DTO.Enums.AgeStratifications>(Stratifications.First().ToString(), out stratification))
            {
                throw new ArgumentException("Unable to parse the specified stratification value as an AgeStratification: " + Stratifications.First().ToString());
            }

            var noProcessStratifications = new[] {
                DTO.Enums.AgeStratifications.Ten,
                DTO.Enums.AgeStratifications.Seven,
                DTO.Enums.AgeStratifications.Four,
                DTO.Enums.AgeStratifications.Two,
                DTO.Enums.AgeStratifications.None
            };

            if (noProcessStratifications.Contains(stratification))
                return row;

            double age = Convert.ToInt32(value);
            if (stratification == DTO.Enums.AgeStratifications.FiveYearGrouping)
            {
                if (age < 0)
                {
                    row["Age"] = string.Format("{0} - {1}", (age * 5) + 4, (age * 5));
                }
                else
                {
                    row["Age"] = string.Format("{0} - {1}", (age * 5), (age * 5) + 4);
                }
            }
            else if (stratification == DTO.Enums.AgeStratifications.TenYearGrouping)
            {
                row["Age"] = string.Format("{0} - {1}", (age * 10), (age * 10) + 9);
            }
            else
            {
                throw new NotImplementedException("The specified age stratification has not been implemented by the Age Range results transformer.");
            }

            return row;
        }

        public void TransformPropertyDefinitions(List<IPropertyDefinition> definitions)
        {
            if (!HasStratifications)
                return;

            DTO.Enums.AgeStratifications stratification;
            if (!Enum.TryParse<DTO.Enums.AgeStratifications>(Stratifications.First().ToString(), out stratification))
            {
                throw new ArgumentException("Unable to parse the specified stratification value as an AgeStratification: " + Stratifications.First().ToString());
            }
            if (stratification == DTO.Enums.AgeStratifications.FiveYearGrouping || stratification == DTO.Enums.AgeStratifications.TenYearGrouping)
            {
                //type will change from int? to a string describing the range

                var agePropertyDefinitions = definitions.Where(p => p.Name == "Age");
                foreach (var propertyDefinition in agePropertyDefinitions)
                {
                    propertyDefinition.Type = typeof(string).FullName;
                }
            }
        }

    }
}
