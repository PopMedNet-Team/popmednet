using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using pcori = Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters
{
    public static class PredicateHelper
    {
        public static Expression<TDelegate> Negate<TDelegate>(Expression<TDelegate> expression)
        {
            return Expression.Lambda<TDelegate>(Expression.Not(expression.Body), expression.Parameters);
        }

        /// <summary>
        /// Builds a predicate that applies a collection of date ranges to the Encounter table Admitted On column.
        /// </summary>
        /// <param name="observationPeriodRanges"></param>
        /// <returns></returns>
        public static Expression<Func<pcori.Encounter, bool>> ApplyDateRangesToEncounter(IEnumerable<DateRangeValues> observationPeriodRanges)
        {
            Expression<Func<pcori.Encounter, bool>> obsPred = null;

            foreach (var range in observationPeriodRanges)
            {
                DateTime? start = null;
                if (range.StartDate.HasValue)
                    start = range.StartDate.Value.DateTime.Date;

                DateTime? end = null;
                if (range.EndDate.HasValue)
                    end = range.EndDate.Value.Date;

                Expression<Func<pcori.Encounter, bool>> obsPredicate = null;

                if (start.HasValue && end.HasValue)
                {
                    obsPredicate = d => d.AdmittedOn >= start && d.AdmittedOn <= end;
                }
                else if (start.HasValue)
                {
                    obsPredicate = d => d.AdmittedOn >= start;
                }
                else if (end.HasValue)
                {
                    obsPredicate = d => d.AdmittedOn <= end;
                }

                if (obsPred == null)
                {
                    obsPred = obsPredicate;
                }
                else
                {
                    obsPred = obsPred.Or(obsPredicate);
                }
            }

            return obsPred;
        }

        /// <summary>
        /// Builds a predicate that applies the specified settings to the Encounters table EncounterType column.
        /// </summary>
        /// <param name="settingValues"></param>
        /// <returns></returns>
        public static Expression<Func<pcori.Encounter, bool>> ApplySettingsToEncounter(IEnumerable<string> settingValues)
        {
            Expression<Func<pcori.Encounter, bool>> settingPred = null;
            foreach (var value in settingValues)
            {
                Expression<Func<pcori.Encounter, bool>> innerSettingPred = null;

                if (value == "AN")
                    innerSettingPred = (enc) => !string.IsNullOrEmpty(enc.EncounterType);
                else
                    innerSettingPred = (enc) => enc.EncounterType == value;

                if (settingPred == null)
                {
                    settingPred = innerSettingPred;
                }
                else
                {
                    settingPred = settingPred.Or(innerSettingPred);
                }
            }
            return settingPred;
        }
    }
}
