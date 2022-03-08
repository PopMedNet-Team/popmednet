using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Lpp.QueryComposer;
using Lpp.Dns.DTO.QueryComposer;

namespace Lpp.Dns.DataMart.Model.QueryComposer
{
    public class QueryComposerQueryInterrogator
    {
        static readonly ILog _logger = LogManager.GetLogger(typeof(QueryComposerQueryInterrogator));
        readonly DTO.QueryComposer.QueryComposerQueryDTO _query;
        readonly IEnumerable<DTO.QueryComposer.QueryComposerFieldDTO> _selectFields;
        readonly DTO.QueryComposer.QueryComposerCriteriaDTO _primaryCriteria = null;
        DTO.QueryComposer.QueryComposerTermDTO _primaryObservationPeriodTerm = null;

        public QueryComposerQueryInterrogator(DTO.QueryComposer.QueryComposerQueryDTO query)
        {
            _query = query;
            if (_query.Select == null)
            {
                _query.Select = new DTO.QueryComposer.QueryComposerSelectDTO
                {
                    Fields = Enumerable.Empty<DTO.QueryComposer.QueryComposerFieldDTO>()
                };
            }

            if (_query.Select.Fields == null)
            {
                _query.Select.Fields = Enumerable.Empty<DTO.QueryComposer.QueryComposerFieldDTO>();
            }

            _selectFields = _query.Select.Fields;

            if (_query.Where.Criteria == null)
                _query.Where.Criteria = Enumerable.Empty<DTO.QueryComposer.QueryComposerCriteriaDTO>();

            _primaryCriteria = _query.Where.Criteria.FirstOrDefault();
            if (_primaryCriteria != null)
            {
                _primaryObservationPeriodTerm = _primaryCriteria.Terms.FirstOrDefault(t => t.Type == ModelTermsFactory.ObservationPeriodID);
            }
        }

        public bool HasCriteria
        {
            get
            {
                return _query.Where != null &&
                       _query.Where.Criteria != null &&
                       _query.Where.Criteria.Any() &&
                       _query.Where.Criteria.SelectMany(c => c.Criteria.SelectMany(cc => cc.Terms)).Concat(_query.Where.Criteria.SelectMany(c => c.Terms)).Any();
            }
        }

        public bool IsSQLDistribution
        {
            get {
                if (!HasCriteria)
                    return false;

                return _query.FlattenToTerms().Where(t => t.Type == ModelTermsFactory.SqlDistributionID).Any();
            }
        }

        public bool HasStratifiers
        {
            get
            {
                return _selectFields.Any(f => f.StratifyBy != null);
            }
        }

        public bool HasAggregates
        {
            get
            {
                return _selectFields.Any(f => f.Aggregate != null);
            }
        }

        public bool IsSimpleAggregate
        {
            get
            {
                return _selectFields.Count() == 1 && _selectFields.Any(f => f.Aggregate != null);
            }
        }

        public bool NeedsToGroup
        {
            get
            {
                //grouping occurs when the select contains fields that have aggregation and some don't.
                return _selectFields.Count() > 1 && _selectFields.Any(f => f.Aggregate == null) && _selectFields.Any(f => f.Aggregate != null);
            }
        }

        public bool HasQuerySubmissionDate
        {
            get
            {
                return _query.Header.SubmittedOn.HasValue;
            }
        }

        public DateTimeOffset? QuerySubmissionDate {
            get
            {
                return _query.Header.SubmittedOn;
            }
        }

        public DTO.QueryComposer.QueryComposerTermDTO PrimaryObservationPeriodTerm
        {
            get
            {
                return _primaryObservationPeriodTerm;
            }
        }

        public Lpp.Dns.DataMart.Model.QueryComposer.Adapters.DateRangeValues PrimaryObservationPeriodDateRange
        {
            get
            {
                if (_primaryObservationPeriodTerm != null)
                {
                    var range = Lpp.Dns.DataMart.Model.QueryComposer.Adapters.AdapterHelpers.ParseDateRangeValues(_primaryObservationPeriodTerm);
                    if (range.StartDate.HasValue || range.EndDate.HasValue)
                    {
                        return range;
                    }
                }

                return null;
            }
        }

        public bool HasTemporalEvents
        {
            get
            {
                return _query.TemporalEvents != null && _query.TemporalEvents.Any();
            }
        }

        /// <summary>
        /// As per PMNMAINT-1025:
        /// If multiple Observation Period terms and a Diagnosis term are added to the same criteria group, 
        /// the query should be looking for the specified codes in either of those two Observation Periods.
        /// As Observation Periods in this case should be ORed with each other.
        /// </summary>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        public List<Lpp.Dns.DataMart.Model.QueryComposer.Adapters.DateRangeValues> ParagraphObservationPeriodDateRanges(DTO.QueryComposer.QueryComposerCriteriaDTO paragraph)
        {
            return ParagraphDateRanges(paragraph, ModelTermsFactory.ObservationPeriodID);
        }

        /// <summary>
        /// For all the terms matching the specified termID parse the date ranges into a DateRangeValues object.
        /// </summary>
        /// <param name="paragraph">The paragraph to get the date ranges for.</param>
        /// <param name="termID">The ID of the term containing the date ranges.</param>
        /// <returns></returns>
        public List<Adapters.DateRangeValues> ParagraphDateRanges(QueryComposerCriteriaDTO paragraph, Guid termID)
        {
            var terms = paragraph.FlattenCriteriaToTerms().Where(t => t.Type == termID).ToArray();
            List<Adapters.DateRangeValues> ranges = new List<Adapters.DateRangeValues>();
            if (terms != null && terms.Length > 0)
            {
                ranges.AddRange(terms.Select(t => Adapters.AdapterHelpers.ParseDateRangeValues(t)).Where(t => t.HasValue));
            }
            return ranges;
        }

        public List<string> ParagraphEncounterTypes(DTO.QueryComposer.QueryComposerCriteriaDTO paragraph)
        {
            var terms = paragraph.FlattenCriteriaToTerms().Where(t => t.Type == ModelTermsFactory.SettingID);

            List<string> values = new List<string>();
            if (terms != null)
            {
                foreach (var term in terms)
                {
                    string value = term.GetStringValue("Setting");
                    DTO.Enums.Settings enumValue;
                    if (Enum.TryParse<DTO.Enums.Settings>(value, out enumValue))
                    {
                        value = enumValue.ToString("G");
                    }

                    if (enumValue == DTO.Enums.Settings.AN)
                    {
                        values.Add("AN");
                    }
                    else
                    {
                        values.Add(value);
                    }
                }
            }
            return values.Distinct().ToList();
        }

    }
}
