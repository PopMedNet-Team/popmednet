using System.Collections.Generic;
using System.Runtime.Serialization;
using System;

namespace RequestCriteria.Models
{
    // This list mimics the /Content/Models/Terms.ts file, make sure they stay in sync
    [DataContract]
    public enum TermTypes
    {
        [EnumMember]
        AgeRangeTerm = 1,
        [EnumMember]
        AgeStratifierTerm = 2,
        [EnumMember]
        ClinicalSettingTerm = 3,
        [EnumMember]
        CodesTerm = 4,
        [EnumMember]
        DateRangeTerm = 5,
        [EnumMember]
        ProjectTerm = 6,
        [EnumMember]
        RequestStatusTerm = 7,
        [EnumMember]
        SexTerm = 8,
        [EnumMember]
        RaceTerm = 9,
        [EnumMember]
        EthnicityTerm = 10,
        [EnumMember]
        MetricTerm = 11,
        [EnumMember]
        DataPartnerTerm = 12,
        [EnumMember]
        WorkplanTypeTerm = 13,
        [EnumMember]
        RequesterCenterTerm = 14,
        [EnumMember]
        PDXTerm = 15,
        [EnumMember]
        RxSupTerm = 16,
        [EnumMember]
        RxAmtTerm = 17,
        [EnumMember]
        EncounterTypeTerm = 18,
        [EnumMember]
        MetaDataTableTerm = 19,
        [EnumMember]
        ReportAggregationLevelTerm = 20
    }

    [DataContract]
    public enum CodesTermTypes
    {
        [EnumMember]
        Diagnosis_ICD9Term = 101,
        [EnumMember]
        Drug_ICD9Term = 102,
        [EnumMember]
        DrugClassTerm = 103,
        [EnumMember]
        GenericDrugTerm = 104,
        [EnumMember]
        HCPCSTerm = 105,
        [EnumMember]
        Procedure_ICD9Term = 106,
        [EnumMember]
        NDCTerm = 107,
    }

    [DataContract]
    public enum DateRangeTermTypes
    {
        [EnumMember]
        ObservationPeriod = 201,
        [EnumMember]
        SubmitDateRange = 202
    }

    //[DataContract]
    //public enum GuidsTermTypes
    //{
    //    [EnumMember]
    //    Organizations = 301,
    //    [EnumMember]
    //    Projects = 302
    //}
    
    [DataContract]
    public enum SearchMethodTypes {
        [EnumMember]
        ExactMatch = 0,
        [EnumMember]
        StartsWith = 1
    }

    public interface ITermData
    {
        TermTypes TermType { get; set; }
    }

    [DataContract]
    public abstract class TermData : ITermData
    {
        [DataMember]
        public TermTypes TermType { get; set; }
    }

    [DataContract]
    public class CodesData : TermData
    {
        [DataMember]
        public string Codes { get; set; }
        [DataMember]
        public CodesTermTypes CodesTermType { get; set; }
        [DataMember]
        public string CodeType { get; set; }
        [DataMember]
        public SearchMethodTypes SearchMethodType { get; set; }
    }

    [DataContract]
    public class DateRangeData : TermData
    {
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public DateTime? StartDate { get; set; }
        [DataMember]
        public DateTime? EndDate { get; set; }
        [DataMember]
        public DateRangeTermTypes DateRangeTermType { get; set; }
    }

    //[DataContract]
    //public class GuidsData : TermData
    //{
    //    [DataMember]
    //    public Guid[] Guids { get; set; }
    //    [DataMember]
    //    public GuidsTermTypes GuidsTermType { get; set; }
    //}
}