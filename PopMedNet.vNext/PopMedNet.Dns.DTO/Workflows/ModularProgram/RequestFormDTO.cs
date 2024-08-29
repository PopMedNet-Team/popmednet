using PopMedNet.Dns.DTO.Enums;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Request Form
    /// </summary>
    [DataContract]
    public class RequestFormDTO
    {
        #region Section1
        //Section 1
        /// <summary>
        /// Request due date
        /// </summary>
        [DataMember]
        public DateTimeOffset? RequestDueDate { get; set; }
        /// <summary>
        /// contact information
        /// </summary>
        [DataMember]
        public string ContactInfo { get; set; }
        /// <summary>
        /// Requesting team
        /// </summary>
        [DataMember]
        public string RequestingTeam { get; set; }
        /// <summary>
        /// FDA review
        /// </summary>
        [DataMember]
        public string FDAReview { get; set; }
        /// <summary>
        /// Gets or sets the FDADivision to NA
        /// </summary>
        [DataMember]
        public bool FDADivisionNA { get; set; }
        /// <summary>
        /// Gets or sets the FDADivision to DAAAP
        /// </summary>
        [DataMember]
        public bool FDADivisionDAAAP { get; set; }
        /// <summary>
        /// Gets or sets the FDADivision to DBRUP
        /// </summary>
        [DataMember]
        public bool FDADivisionDBRUP { get; set; }
        /// <summary>
        /// Gets or sets the FDADivision to DCARP
        /// </summary>
        [DataMember]
        public bool FDADivisionDCARP { get; set; }
        /// <summary>
        ///  Gets or sets the FDADivision to DDDP
        /// </summary>
        [DataMember]
        public bool FDADivisionDDDP { get; set; }
        /// <summary>
        /// Gets or sets the FDADivision to DGIEP
        /// </summary>
        [DataMember]
        public bool FDADivisionDGIEP { get; set; }
        /// <summary>
        /// FDADivisionDMIP
        /// </summary>
        [DataMember]
        public bool FDADivisionDMIP { get; set; }
        /// <summary>
        /// FDADivisionDMEP
        /// </summary>
        [DataMember]
        public bool FDADivisionDMEP { get; set; }
        /// <summary>
        /// Gets or sets the FDADivision to DNP
        /// </summary>
        [DataMember]
        public bool FDADivisionDNP { get; set; }
        /// <summary>
        /// FDADivisionDDP
        /// </summary>
        [DataMember]
        public bool FDADivisionDDP { get; set; }
        /// <summary>
        /// FDADivisionDPARP
        /// </summary>
        [DataMember]
        public bool FDADivisionDPARP { get; set; }
        /// <summary>
        /// FDADivisionOther
        /// </summary>
        [DataMember]
        public bool FDADivisionOther { get; set; }
#endregion

        #region Section2
        //Section 2
        /// <summary>
        /// Query level
        /// </summary>
        [DataMember]
        public string QueryLevel { get; set; }
        /// <summary>
        /// Adjustment method
        /// </summary>
        [DataMember]
        public string AdjustmentMethod { get; set; }
        /// <summary>
        /// Cohort id
        /// </summary>
        [DataMember]
        public string CohortID { get; set; }
        /// <summary>
        /// study objectives
        /// </summary>
        [DataMember]
        public string StudyObjectives { get; set; }
        /// <summary>
        /// RequestStartDate
        /// </summary>
        [DataMember]
        public DateTimeOffset? RequestStartDate { get; set; }
        /// <summary>
        /// Request End Date
        /// </summary>
        [DataMember]
        public DateTimeOffset? RequestEndDate { get; set; }
        /// <summary>
        /// Age Groups
        /// </summary>
        [DataMember]
        public string AgeGroups { get; set; }
        /// <summary>
        /// Coverage types
        /// </summary>
        [DataMember]
        public string CoverageTypes { get; set; }
        /// <summary>
        /// Enrollment Gap
        /// </summary>
        [DataMember]
        public string EnrollmentGap { get; set; }
        /// <summary>
        /// Enrollment Exposure
        /// </summary>
        [DataMember]
        public string EnrollmentExposure { get; set; }
        #endregion

        #region Section 3
        //Section 3
        #region Section3A
        [DataMember]
        public string DefineExposures { get; set; }
        [DataMember]
        public string WashoutPeirod { get; set; }
        [DataMember]
        public string OtherExposures { get; set; }
        [DataMember]
        public string OneOrManyExposures { get; set; }
        [DataMember]
        public string AdditionalInclusion { get; set; }
        [DataMember]
        public string AdditionalInclusionEvaluation { get; set; }
        [DataMember]
        public string AdditionalExclusion { get; set; }
        [DataMember]
        public string AdditionalExclusionEvaluation { get; set; }
        [DataMember]
        public string VaryWashoutPeirod { get; set; }
        [DataMember]
        public string VaryExposures { get; set; }
        
        #endregion

        #region Section3B
        /// <summary>
        /// Define Exposures 1
        /// </summary>
        [DataMember]
        public string DefineExposures1 { get; set; }
        /// <summary>
        /// Define Exposures 2
        /// </summary>
        [DataMember]
        public string DefineExposures2 { get; set; }
        /// <summary>
        /// Define Exposures 3
        /// </summary>
        [DataMember]
        public string DefineExposures3 { get; set; }
        /// <summary>
        /// Define Exposures 4
        /// </summary>
        [DataMember]
        public string DefineExposures4 { get; set; }
        /// <summary>
        /// Define Exposures 5
        /// </summary>
        [DataMember]
        public string DefineExposures5 { get; set; }
        /// <summary>
        /// Define Exposures 6
        /// </summary>
        [DataMember]
        public string DefineExposures6 { get; set; }
        /// <summary>
        /// Define Exposures 7
        /// </summary>
        [DataMember]
        public string DefineExposures7 { get; set; }
        /// <summary>
        /// Define Exposures 8
        /// </summary>
        [DataMember]
        public string DefineExposures8 { get; set; }
        /// <summary>
        /// Define Exposures 9
        /// </summary>
        [DataMember]
        public string DefineExposures9 { get; set; }
        /// <summary>
        /// Define Exposures 10
        /// </summary>
        [DataMember]
        public string DefineExposures10 { get; set; }
        /// <summary>
        /// Define Exposures 11
        /// </summary>
        [DataMember]
        public string DefineExposures11 { get; set; }
        /// <summary>
        /// Define Exposures 12
        /// </summary>
        [DataMember]
        public string DefineExposures12 { get; set; }
        /// <summary>
        /// Returns WashoutPeriod1
        /// </summary>

        [DataMember]
        public int? WashoutPeriod1 { get; set; }
        /// <summary>
        /// Returns WashoutPeriod2
        /// </summary>
        [DataMember]
        public int? WashoutPeriod2 { get; set; }
        /// <summary>
        /// Returns WashoutPeriod3
        /// </summary>
        [DataMember]
        public int? WashoutPeriod3 { get; set; }
        /// <summary>
        /// Returns WashoutPeriod4
        /// </summary>
        [DataMember]
        public int? WashoutPeriod4 { get; set; }
        /// <summary>
        /// Returns WashoutPeriod5
        /// </summary>
        [DataMember]
        public int? WashoutPeriod5 { get; set; }
        /// <summary>
        /// Returns WashoutPeriod6
        /// </summary>
        [DataMember]
        public int? WashoutPeriod6 { get; set; }
        /// <summary>
        /// Returns WashoutPeriod7
        /// </summary>
        [DataMember]
        public int? WashoutPeriod7 { get; set; }
        /// <summary>
        /// Returns WashoutPeriod8
        /// </summary>
        [DataMember]
        public int? WashoutPeriod8 { get; set; }
        /// <summary>
        /// Returns WashoutPeriod9
        /// </summary>
        [DataMember]
        public int? WashoutPeriod9 { get; set; }
        /// <summary>
        /// Returns WashoutPeriod10
        /// </summary>
        [DataMember]
        public int? WashoutPeriod10 { get; set; }
        /// <summary>
        /// Returns WashoutPeriod11
        /// </summary>
        [DataMember]
        public int? WashoutPeriod11 { get; set; }
        /// <summary>
        /// Returns WashoutPeriod12
        /// </summary>
        [DataMember]
        public int? WashoutPeriod12 { get; set; }
        /// <summary>
        /// Incidence Refinement 1
        /// </summary>

        [DataMember]
        public string IncidenceRefinement1 { get; set; }
        /// <summary>
        /// Incidence Refinement 2
        /// </summary>
        [DataMember]
        public string IncidenceRefinement2 { get; set; }
        /// <summary>
        /// Incidence Refinement 3
        /// </summary>
        [DataMember]
        public string IncidenceRefinement3 { get; set; }
        /// <summary>
        /// Incidence Refinement 4
        /// </summary>
        [DataMember]
        public string IncidenceRefinement4 { get; set; }
        /// <summary>
        /// Incidence Refinement 5
        /// </summary>
        [DataMember]
        public string IncidenceRefinement5 { get; set; }
        /// <summary>
        /// Incidence Refinement 6
        /// </summary>
        [DataMember]
        public string IncidenceRefinement6 { get; set; }
        /// <summary>
        /// Incidence Refinement 7
        /// </summary>
        [DataMember]
        public string IncidenceRefinement7 { get; set; }
        /// <summary>
        /// Incidence Refinement 8
        /// </summary>
        [DataMember]
        public string IncidenceRefinement8 { get; set; }
        /// <summary>
        /// Incidence Refinement 9
        /// </summary>
        [DataMember]
        public string IncidenceRefinement9 { get; set; }
        /// <summary>
        /// Incidence Refinement 10
        /// </summary>
        [DataMember]
        public string IncidenceRefinement10 { get; set; }
        /// <summary>
        /// Incidence Refinement 11
        /// </summary>
        [DataMember]
        public string IncidenceRefinement11 { get; set; }
        /// <summary>
        /// Incidence Refinement 12
        /// </summary>
        [DataMember]
        public string IncidenceRefinement12 { get; set; }
        /// <summary>
        /// Specify ExposedTimeAssessment1
        /// </summary>

        [DataMember]
        public WorkflowMPSpecifyExposedTimeAssessments? SpecifyExposedTimeAssessment1 { get; set; }
        /// <summary>
        /// Specify ExposedTimeAssessment2
        /// </summary>
        [DataMember]
        public WorkflowMPSpecifyExposedTimeAssessments? SpecifyExposedTimeAssessment2 { get; set; }
        /// <summary>
        /// SpecifyExposedTimeAssessment3
        /// </summary>
        [DataMember]
        public WorkflowMPSpecifyExposedTimeAssessments? SpecifyExposedTimeAssessment3 { get; set; }
        /// <summary>
        /// SpecifyExposedTimeAssessment4
        /// </summary>
        [DataMember]
        public WorkflowMPSpecifyExposedTimeAssessments? SpecifyExposedTimeAssessment4 { get; set; }
        /// <summary>
        /// SpecifyExposedTimeAssessment5
        /// </summary>
        [DataMember]
        public WorkflowMPSpecifyExposedTimeAssessments? SpecifyExposedTimeAssessment5 { get; set; }
        /// <summary>
        /// SpecifyExposedTimeAssessment6
        /// </summary>
        [DataMember]
        public WorkflowMPSpecifyExposedTimeAssessments? SpecifyExposedTimeAssessment6 { get; set; }
        /// <summary>
        /// SpecifyExposedTimeAssessment7
        /// </summary>
        [DataMember]
        public WorkflowMPSpecifyExposedTimeAssessments? SpecifyExposedTimeAssessment7 { get; set; }
        /// <summary>
        /// SpecifyExposedTimeAssessment8
        /// </summary>
        [DataMember]
        public WorkflowMPSpecifyExposedTimeAssessments? SpecifyExposedTimeAssessment8 { get; set; }
        /// <summary>
        /// SpecifyExposedTimeAssessment9
        /// </summary>
        [DataMember]
        public WorkflowMPSpecifyExposedTimeAssessments? SpecifyExposedTimeAssessment9 { get; set; }
        /// <summary>
        /// SpecifyExposedTimeAssessment10
        /// </summary>
        [DataMember]
        public WorkflowMPSpecifyExposedTimeAssessments? SpecifyExposedTimeAssessment10 { get; set; }
        /// <summary>
        /// SpecifyExposedTimeAssessment11
        /// </summary>
        [DataMember]
        public WorkflowMPSpecifyExposedTimeAssessments? SpecifyExposedTimeAssessment11 { get; set; }
        /// <summary>
        /// SpecifyExposedTimeAssessment12
        /// </summary>
        [DataMember]
        public WorkflowMPSpecifyExposedTimeAssessments? SpecifyExposedTimeAssessment12 { get; set; }
        /// <summary>
        /// Returns EpisodeAllowableGap1
        /// </summary>

        [DataMember]
        public int? EpisodeAllowableGap1 { get; set; }
        /// <summary>
        /// Returns EpisodeAllowableGap2
        /// </summary>
        [DataMember]
        public int? EpisodeAllowableGap2 { get; set; }
        /// <summary>
        /// Returns EpisodeAllowableGap3
        /// </summary>
        [DataMember]
        public int? EpisodeAllowableGap3 { get; set; }
        /// <summary>
        /// Returns EpisodeAllowableGap4
        /// </summary>
        [DataMember]
        public int? EpisodeAllowableGap4 { get; set; }
        /// <summary>
        /// Returns EpisodeAllowableGap5
        /// </summary>
        [DataMember]
        public int? EpisodeAllowableGap5 { get; set; }
        /// <summary>
        /// Returns EpisodeAllowableGap6
        /// </summary>
        [DataMember]
        public int? EpisodeAllowableGap6 { get; set; }
        /// <summary>
        /// Returns EpisodeAllowableGap7
        /// </summary>
        [DataMember]
        public int? EpisodeAllowableGap7 { get; set; }
        /// <summary>
        /// Returns EpisodeAllowableGap8
        /// </summary>
        [DataMember]
        public int? EpisodeAllowableGap8 { get; set; }
        /// <summary>
        /// Returns EpisodeAllowableGap9
        /// </summary>
        [DataMember]
        public int? EpisodeAllowableGap9 { get; set; }
        /// <summary>
        /// Returns EpisodeAllowableGap10
        /// </summary>
        [DataMember]
        public int? EpisodeAllowableGap10 { get; set; }
        /// <summary>
        /// Returns EpisodeAllowableGap11
        /// </summary>
        [DataMember]
        public int? EpisodeAllowableGap11 { get; set; }
        /// <summary>
        /// Returns EpisodeAllowableGap12
        /// </summary>
        [DataMember]
        public int? EpisodeAllowableGap12 { get; set; }
        /// <summary>
        /// Returns EpisodeExtensionPeriod1
        /// </summary>

        [DataMember]
        public int? EpisodeExtensionPeriod1 { get; set; }
        /// <summary>
        /// Returns EpisodeExtensionPeriod2
        /// </summary>
        [DataMember]
        public int? EpisodeExtensionPeriod2 { get; set; }
        /// <summary>
        /// Returns EpisodeExtensionPeriod3
        /// </summary>
        [DataMember]
        public int? EpisodeExtensionPeriod3 { get; set; }
        /// <summary>
        /// Returns EpisodeExtensionPeriod4
        /// </summary>
        [DataMember]
        public int? EpisodeExtensionPeriod4 { get; set; }
        /// <summary>
        /// Returns EpisodeExtensionPeriod5
        /// </summary>
        [DataMember]
        public int? EpisodeExtensionPeriod5 { get; set; }
        /// <summary>
        /// Returns EpisodeExtensionPeriod6
        /// </summary>
        [DataMember]
        public int? EpisodeExtensionPeriod6 { get; set; }
        /// <summary>
        /// Returns EpisodeExtensionPeriod7
        /// </summary>
        [DataMember]
        public int? EpisodeExtensionPeriod7 { get; set; }
        /// <summary>
        /// Returns EpisodeExtensionPeriod8
        /// </summary>
        [DataMember]
        public int? EpisodeExtensionPeriod8 { get; set; }
        /// <summary>
        /// Returns EpisodeExtensionPeriod9
        /// </summary>
        [DataMember]
        public int? EpisodeExtensionPeriod9 { get; set; }
        /// <summary>
        /// Returns EpisodeExtensionPeriod10
        /// </summary>
        [DataMember]
        public int? EpisodeExtensionPeriod10 { get; set; }
        /// <summary>
        /// Returns EpisodeExtensionPeriod11
        /// </summary>
        [DataMember]
        public int? EpisodeExtensionPeriod11 { get; set; }
        /// <summary>
        /// Returns EpisodeExtensionPeriod12
        /// </summary>
        [DataMember]
        public int? EpisodeExtensionPeriod12 { get; set; }
        /// <summary>
        /// Returns MinimumEpisodeDuration1
        /// </summary>

        [DataMember]
        public int? MinimumEpisodeDuration1 { get; set; }
        /// <summary>
        /// Returns MinimumEpisodeDuration2
        /// </summary>
        [DataMember]
        public int? MinimumEpisodeDuration2 { get; set; }
        /// <summary>
        /// Returns MinimumEpisodeDuration3
        /// </summary>
        [DataMember]
        public int? MinimumEpisodeDuration3 { get; set; }
        /// <summary>
        /// Returns MinimumEpisodeDuration4
        /// </summary>
        [DataMember]
        public int? MinimumEpisodeDuration4 { get; set; }
        /// <summary>
        /// Returns MinimumEpisodeDuration5
        /// </summary>
        [DataMember]
        public int? MinimumEpisodeDuration5 { get; set; }
        /// <summary>
        /// Returns MinimumEpisodeDuration6
        /// </summary>
        [DataMember]
        public int? MinimumEpisodeDuration6 { get; set; }
        /// <summary>
        /// Returns MinimumEpisodeDuration7
        /// </summary>
        [DataMember]
        public int? MinimumEpisodeDuration7 { get; set; }
        /// <summary>
        /// Returns MinimumEpisodeDuration8
        /// </summary>
        [DataMember]
        public int? MinimumEpisodeDuration8 { get; set; }
        /// <summary>
        /// Returns MinimumEpisodeDuration9
        /// </summary>
        [DataMember]
        public int? MinimumEpisodeDuration9 { get; set; }
        /// <summary>
        /// Returns MinimumEpisodeDuration10
        /// </summary>
        [DataMember]
        public int? MinimumEpisodeDuration10 { get; set; }
        /// <summary>
        /// Returns MinimumEpisodeDuration11
        /// </summary>
        [DataMember]
        public int? MinimumEpisodeDuration11 { get; set; }
        /// <summary>
        /// Returns MinimumEpisodeDuration12
        /// </summary>
        [DataMember]
        public int? MinimumEpisodeDuration12 { get; set; }
        /// <summary>
        /// Returns MinimumDaysSupply1
        /// </summary>

        [DataMember]
        public int? MinimumDaysSupply1 { get; set; }
        /// <summary>
        /// Returns MinimumDaysSupply2
        /// </summary>
        [DataMember]
        public int? MinimumDaysSupply2 { get; set; }
        /// <summary>
        /// Returns MinimumDaysSupply3
        /// </summary>
        [DataMember]
        public int? MinimumDaysSupply3 { get; set; }
        /// <summary>
        /// Returns MinimumDaysSupply4
        /// </summary>
        [DataMember]
        public int? MinimumDaysSupply4 { get; set; }
        /// <summary>
        /// Returns MinimumDaysSupply5
        /// </summary>
        [DataMember]
        public int? MinimumDaysSupply5 { get; set; }
        /// <summary>
        /// Returns MinimumDaysSupply6
        /// </summary>
        [DataMember]
        public int? MinimumDaysSupply6 { get; set; }
        /// <summary>
        /// Returns MinimumDaysSupply7
        /// </summary>
        [DataMember]
        public int? MinimumDaysSupply7 { get; set; }
        /// <summary>
        /// Returns MinimumDaysSupply8
        /// </summary>
        [DataMember]
        public int? MinimumDaysSupply8 { get; set; }
        /// <summary>
        /// Returns MinimumDaysSupply9
        /// </summary>
        [DataMember]
        public int? MinimumDaysSupply9 { get; set; }
        /// <summary>
        /// Returns MinimumDaysSupply10
        /// </summary>
        [DataMember]
        public int? MinimumDaysSupply10 { get; set; }
        /// <summary>
        /// Returns MinimumDaysSupply11
        /// </summary>
        [DataMember]
        public int? MinimumDaysSupply11 { get; set; }
        /// <summary>
        /// Returns MinimumDaysSupply12
        /// </summary>
        [DataMember]
        public int? MinimumDaysSupply12 { get; set; }
        /// <summary>
        /// Returns the value of specifyFollowUpDuration1
        /// </summary>

        [DataMember]
        public int? SpecifyFollowUpDuration1 { get; set; }
        /// <summary>
        /// Returns the value of specifyFollowUpDuration2
        /// </summary>
        [DataMember]
        public int? SpecifyFollowUpDuration2 { get; set; }
        /// <summary>
        /// Returns the value of specifyFollowUpDuration3
        /// </summary>
        [DataMember]
        public int? SpecifyFollowUpDuration3 { get; set; }
        /// <summary>
        /// Returns the value of specifyFollowUpDuration4
        /// </summary>
        [DataMember]
        public int? SpecifyFollowUpDuration4 { get; set; }
        /// <summary>
        /// Returns the value of specifyFollowUpDuration5
        /// </summary>
        [DataMember]
        public int? SpecifyFollowUpDuration5 { get; set; }
        /// <summary>
        /// Returns the value of specifyFollowUpDuration6
        /// </summary>
        [DataMember]
        public int? SpecifyFollowUpDuration6 { get; set; }
        /// <summary>
        /// Returns the value of specifyFollowUpDuration7
        /// </summary>
        [DataMember]
        public int? SpecifyFollowUpDuration7 { get; set; }
        /// <summary>
        /// Returns the value of specifyFollowUpDuration8
        /// </summary>
        [DataMember]
        public int? SpecifyFollowUpDuration8 { get; set; }
        /// <summary>
        /// Returns the value of specifyFollowUpDuration9
        /// </summary>
        [DataMember]
        public int? SpecifyFollowUpDuration9 { get; set; }
        /// <summary>
        /// Returns the value of specifyFollowUpDuration10
        /// </summary>
        [DataMember]
        public int? SpecifyFollowUpDuration10 { get; set; }
        /// <summary>
        /// Returns the value of specifyFollowUpDuration11
        /// </summary>
        [DataMember]
        public int? SpecifyFollowUpDuration11 { get; set; }
        /// <summary>
        /// Returns the value of specifyFollowUpDuration12
        /// </summary>
        [DataMember]
        public int? SpecifyFollowUpDuration12 { get; set; }
        /// <summary>
        /// gets or sets the workflow MP Allow on or multiple exposre episodes1
        /// </summary>

        [DataMember]
        public WorkflowMPAllowOnOrMultipleExposureEpisodes? AllowOnOrMultipleExposureEpisodes1 { get; set; }
        /// <summary>
        /// gets or sets the workflow MP Allow on or multiple exposre episodes2
        /// </summary>
        [DataMember]
        public WorkflowMPAllowOnOrMultipleExposureEpisodes? AllowOnOrMultipleExposureEpisodes2 { get; set; }
        /// <summary>
        /// gets or sets the workflow MP Allow on or multiple exposre episodes3
        /// </summary>
        [DataMember]
        public WorkflowMPAllowOnOrMultipleExposureEpisodes? AllowOnOrMultipleExposureEpisodes3 { get; set; }
        /// <summary>
        /// gets or sets the workflow MP Allow on or multiple exposre episodes4
        /// </summary>
        [DataMember]
        public WorkflowMPAllowOnOrMultipleExposureEpisodes? AllowOnOrMultipleExposureEpisodes4 { get; set; }
        /// <summary>
        /// gets or sets the workflow MP Allow on or multiple exposre episodes5
        /// </summary>
        [DataMember]
        public WorkflowMPAllowOnOrMultipleExposureEpisodes? AllowOnOrMultipleExposureEpisodes5 { get; set; }
        /// <summary>
        /// gets or sets the workflow MP Allow on or multiple exposre episodes6
        /// </summary>
        [DataMember]
        public WorkflowMPAllowOnOrMultipleExposureEpisodes? AllowOnOrMultipleExposureEpisodes6 { get; set; }
        /// <summary>
        /// gets or sets the workflow MP Allow on or multiple exposre episodes7
        /// </summary>
        [DataMember]
        public WorkflowMPAllowOnOrMultipleExposureEpisodes? AllowOnOrMultipleExposureEpisodes7 { get; set; }
        /// <summary>
        /// gets or sets the workflow MP Allow on or multiple exposre episodes8
        /// </summary>
        [DataMember]
        public WorkflowMPAllowOnOrMultipleExposureEpisodes? AllowOnOrMultipleExposureEpisodes8 { get; set; }
        /// <summary>
        /// gets or sets the workflow MP Allow on or multiple exposre episodes9
        /// </summary>
        [DataMember]
        public WorkflowMPAllowOnOrMultipleExposureEpisodes? AllowOnOrMultipleExposureEpisodes9 { get; set; }
        /// <summary>
        /// gets or sets the workflow MP Allow on or multiple exposre episodes10
        /// </summary>
        [DataMember]
        public WorkflowMPAllowOnOrMultipleExposureEpisodes? AllowOnOrMultipleExposureEpisodes10 { get; set; }
        /// <summary>
        /// gets or sets the workflow MP Allow on or multiple exposre episodes11
        /// </summary>
        [DataMember]
        public WorkflowMPAllowOnOrMultipleExposureEpisodes? AllowOnOrMultipleExposureEpisodes11 { get; set; }
        /// <summary>
        /// gets or sets the workflow MP Allow on or multiple exposre episodes12
        /// </summary>
        [DataMember]
        public WorkflowMPAllowOnOrMultipleExposureEpisodes? AllowOnOrMultipleExposureEpisodes12 { get; set; }
        /// <summary>
        /// Gets or sets the truncate exposedtime1
        /// </summary>

        [DataMember]
        public bool TruncateExposedtime1 { get; set; }
        /// <summary>
        /// Gets or sets the truncate exposedtime2
        /// </summary>
        [DataMember]
        public bool TruncateExposedtime2 { get; set; }
        /// <summary>
        /// Gets or sets the truncate exposedtime3
        /// </summary>
        [DataMember]
        public bool TruncateExposedtime3 { get; set; }
        /// <summary>
        /// Gets or sets the truncate exposedtime4
        /// </summary>
        [DataMember]
        public bool TruncateExposedtime4 { get; set; }
        /// <summary>
        /// Gets or sets the truncate exposedtime5
        /// </summary>
        [DataMember]
        public bool TruncateExposedtime5 { get; set; }
        /// <summary>
        /// Gets or sets the truncate exposedtime6
        /// </summary>
        [DataMember]
        public bool TruncateExposedtime6 { get; set; }
        /// <summary>
        /// Gets or sets the truncate exposedtime7
        /// </summary>
        [DataMember]
        public bool TruncateExposedtime7 { get; set; }
        /// <summary>
        /// Gets or sets the truncate exposedtime8
        /// </summary>
        [DataMember]
        public bool TruncateExposedtime8 { get; set; }
        /// <summary>
        /// Gets or sets the truncate exposedtime9
        /// </summary>
        [DataMember]
        public bool TruncateExposedtime9 { get; set; }
        /// <summary>
        /// Gets or sets the truncate exposedtime10
        /// </summary>
        [DataMember]
        public bool TruncateExposedtime10 { get; set; }
        /// <summary>
        /// Gets or sets the truncate exposedtime11
        /// </summary>
        [DataMember]
        public bool TruncateExposedtime11 { get; set; }
        /// <summary>
        /// Gets or sets the truncate exposedtime12
        /// </summary>
        [DataMember]
        public bool TruncateExposedtime12 { get; set; }
        /// <summary>
        /// Truncate Exposed Time Specified 1
        /// </summary>

        [DataMember]
        public string TruncateExposedTimeSpecified1 { get; set; }
        /// <summary>
        /// Truncate Exposed Time Specified 2
        /// </summary>
        [DataMember]
        public string TruncateExposedTimeSpecified2 { get; set; }
        /// <summary>
        /// Truncate Exposed Time Specified 3
        /// </summary>
        [DataMember]
        public string TruncateExposedTimeSpecified3 { get; set; }
        /// <summary>
        /// Truncate Exposed Time Specified 4
        /// </summary>
        [DataMember]
        public string TruncateExposedTimeSpecified4 { get; set; }
        /// <summary>
        /// Truncate Exposed Time Specified 5
        /// </summary>
        [DataMember]
        public string TruncateExposedTimeSpecified5 { get; set; }
        /// <summary>
        /// Truncate Exposed Time Specified 6
        /// </summary>
        [DataMember]
        public string TruncateExposedTimeSpecified6 { get; set; }
        /// <summary>
        /// Truncate Exposed Time Specified 7
        /// </summary>
        [DataMember]
        public string TruncateExposedTimeSpecified7 { get; set; }
        /// <summary>
        /// Truncate Exposed Time Specified 8
        /// </summary>
        [DataMember]
        public string TruncateExposedTimeSpecified8 { get; set; }
        /// <summary>
        /// Truncate Exposed Time Specified 9
        /// </summary>
        [DataMember]
        public string TruncateExposedTimeSpecified9 { get; set; }
        /// <summary>
        /// Truncate Exposed Time Specified 10
        /// </summary>
        [DataMember]
        public string TruncateExposedTimeSpecified10 { get; set; }
        /// <summary>
        /// Truncate Exposed Time Specified 11
        /// </summary>
        [DataMember]
        public string TruncateExposedTimeSpecified11 { get; set; }
        /// <summary>
        /// Truncate Exposed Time Specified 12
        /// </summary>
        [DataMember]
        public string TruncateExposedTimeSpecified12 { get; set; }
        /// <summary>
        /// Returns the value of SpecifyBlackoutPeriod1
        /// </summary>

        [DataMember]
        public int? SpecifyBlackoutPeriod1 { get; set; }
        /// <summary>
        /// Returns the value of SpecifyBlackoutPeriod2
        /// </summary>
        [DataMember]
        public int? SpecifyBlackoutPeriod2 { get; set; }
        /// <summary>
        /// Returns the value of SpecifyBlackoutPeriod3
        /// </summary>
        [DataMember]
        public int? SpecifyBlackoutPeriod3 { get; set; }
        /// <summary>
        /// Returns the value of SpecifyBlackoutPeriod4
        /// </summary>
        [DataMember]
        public int? SpecifyBlackoutPeriod4 { get; set; }
        /// <summary>
        /// Returns the value of SpecifyBlackoutPeriod5
        /// </summary>
        [DataMember]
        public int? SpecifyBlackoutPeriod5 { get; set; }
        /// <summary>
        /// Returns the value of SpecifyBlackoutPeriod6
        /// </summary>
        [DataMember]
        public int? SpecifyBlackoutPeriod6 { get; set; }
        /// <summary>
        /// Returns the value of SpecifyBlackoutPeriod7
        /// </summary>
        [DataMember]
        public int? SpecifyBlackoutPeriod7 { get; set; }
        /// <summary>
        /// Returns the value of SpecifyBlackoutPeriod8
        /// </summary>
        [DataMember]
        public int? SpecifyBlackoutPeriod8 { get; set; }
        /// <summary>
        /// Returns the value of SpecifyBlackoutPeriod9
        /// </summary>
        [DataMember]
        public int? SpecifyBlackoutPeriod9 { get; set; }
        /// <summary>
        /// Returns the value of SpecifyBlackoutPeriod10
        /// </summary>
        [DataMember]
        public int? SpecifyBlackoutPeriod10 { get; set; }
        /// <summary>
        /// Returns the value of SpecifyBlackoutPeriod11
        /// </summary>
        [DataMember]
        public int? SpecifyBlackoutPeriod11 { get; set; }
        /// <summary>
        /// Returns the value of SpecifyBlackoutPeriod12
        /// </summary>
        [DataMember]
        public int? SpecifyBlackoutPeriod12 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 11
        /// </summary>


        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup11 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 12
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup12 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 13
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup13 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 14
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup14 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 15
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup15 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 16
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup16 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Evaluation WindowGroup 11
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup11 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Evaluation WindowGroup 12
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup12 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Evaluation WindowGroup 13
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup13 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Evaluation WindowGroup 14
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup14 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Evaluation WindowGroup 15
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup15 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Evaluation WindowGroup 16
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup16 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Inclusion CriteriaGroup 21
        /// </summary>

        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup21 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Inclusion CriteriaGroup 22
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup22 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Inclusion CriteriaGroup 23
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup23 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Inclusion CriteriaGroup 24
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup24 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Inclusion CriteriaGroup 25
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup25 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Inclusion CriteriaGroup 26
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup26 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Evaluation WindowGroup 21
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup21 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Evaluation WindowGroup 22
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup22 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Evaluation WindowGroup 23
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup23 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Evaluation WindowGroup 24
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup24 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Evaluation WindowGroup 25
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup25 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Evaluation WindowGroup 26
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup26 { get; set; }
        /// <summary>
        ///  Specify AdditionalInclusion InclusionCriteriaGroup 31
        /// </summary>

        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup31 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 32
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup32 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 33
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup33 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 34
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup34 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 35
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup35 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 36
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup36 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Evaluation WindowGroup 31
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup31 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Evaluation WindowGroup 32
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup32 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Evaluation WindowGroup 33
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup33 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Evaluation WindowGroup 34
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup34 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Evaluation WindowGroup 35
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup35 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion Evaluation WindowGroup 36
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup36 { get; set; }
        /// <summary>
        ///  Specify AdditionalInclusion InclusionCriteriaGroup 41
        /// </summary>

        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup41 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 42
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup42 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 43
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup43 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 44
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup44 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 45
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup45 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 46
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup46 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 41
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup41 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 42
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup42 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 43
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup43 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 44
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup44 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 45
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup45 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 46
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup46 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 51
        /// </summary>

        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup51 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 52
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup52 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 53
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup53 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 54
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup54 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 55
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup55 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 56
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup56 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 51
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup51 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 52
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup52 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 53
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup53 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 54
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup54 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 55
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup55 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 56
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup56 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 61
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup61 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 62
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup62 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 63
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup63 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 64
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup64 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 65
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup65 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 66
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup66 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 61
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup61 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 62
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup62 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 63
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup63 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 64
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup64 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 65
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup65 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 66
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup66 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 71
        /// </summary>

        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup71 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 72
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup72 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 73
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup73 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 74
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup74 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 75
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup75 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 76
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup76 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 71
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup71 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 72
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup72 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 73
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup73 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 74
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup74 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 75
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup75 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 76
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup76 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 81
        /// </summary>

        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup81 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 82
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup82 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 83
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup83 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 84
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup84 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 85
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup85 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 86
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup86 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 81
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup81 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 82
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup82 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 83
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup83 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 84
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup84 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 85
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup85 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 86
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup86 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 91
        /// </summary>

        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup91 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 92
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup92 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 93
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup93 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 94
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup94 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 95
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup95 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 96
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup96 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 91
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup91 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 92
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup92 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 93
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup93 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 94
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup94 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 95
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup95 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 96
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup96 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 101
        /// </summary>

        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup101 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 102
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup102 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 103
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup103 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 104
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup104 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 105
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup105 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 106
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup106 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 101
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup101 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 102
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup102 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 103
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup103 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 104
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup104 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 105
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup105 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 106
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup106 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 111
        /// </summary>

        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup111 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 112
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup112 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 113
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup113 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 114
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup114 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 115
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup115 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 116
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup116 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 111
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup111 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 112
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup112 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 113
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup113 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 114
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup114 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 115
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup115 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 116
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup116 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 121
        /// </summary>

        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup121 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 122
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup122 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 123
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup123 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 124
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup124 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 125
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup125 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion InclusionCriteriaGroup 126
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionInclusionCriteriaGroup126 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 121
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup121 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 122
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup122 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 123
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup123 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 124
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup124 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 125
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup125 { get; set; }
        /// <summary>
        /// Specify AdditionalInclusion EvaluationWindowGroup 126
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalInclusionEvaluationWindowGroup126 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 11
        /// </summary>


        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup11 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 12
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup12 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 13
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup13 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 14
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup14 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 15
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup15 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 16
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup16 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 11
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup11 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 12
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup12 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 13
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup13 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 14
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup14 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 15
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup15 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 16
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup16 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 21
        /// </summary>

        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup21 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 22
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup22 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 23
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup23 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 24
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup24 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 25
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup25 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 26
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup26 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 21
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup21 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 22
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup22 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 23
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup23 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 24
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup24 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 25
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup25 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 26
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup26 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 31
        /// </summary>

        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup31 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 32
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup32 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 33
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup33 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 34
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup34 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 35
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup35 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 36
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup36 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 31
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup31 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 32
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup32 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 33
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup33 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 34
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup34 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 35
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup35 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 36
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup36 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 41
        /// </summary>

        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup41 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 42
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup42 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 43
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup43 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 44
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup44 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 45
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup45 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 46
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup46 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 41
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup41 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 42
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup42 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 43
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup43 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 44
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup44 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 45
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup45 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 46
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup46 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 51
        /// </summary>

        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup51 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 52
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup52 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 53
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup53 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 54
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup54 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 55
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup55 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 56
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup56 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 51
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup51 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 52
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup52 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 53
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup53 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 54
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup54 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 55
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup55 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 56
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup56 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 61
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup61 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 62
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup62 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 63
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup63 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 64
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup64 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 65
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup65 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 66
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup66 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 61
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup61 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 62
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup62 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 63
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup63 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 64
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup64 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 65
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup65 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 66
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup66 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 71
        /// </summary>

        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup71 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 72
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup72 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 73
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup73 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 74
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup74 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 75
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup75 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 76
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup76 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 71
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup71 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 72
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup72 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 73
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup73 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 74
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup74 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 75
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup75 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 76
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup76 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 81
        /// </summary>

        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup81 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 82
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup82 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 83
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup83 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 84
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup84 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 85
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup85 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 86
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup86 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 81
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup81 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 82
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup82 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 83
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup83 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 84
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup84 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 85
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup85 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 86
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup86 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 91
        /// </summary>

        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup91 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 92
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup92 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 93
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup93 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 94
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup94 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 95
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup95 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 96
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup96 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 91
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup91 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 92
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup92 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 93
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup93 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 94
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup94 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 95
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup95 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 96
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup96 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 101
        /// </summary>

        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup101 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 102
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup102 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 103
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup103 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 104
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup104 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 105
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup105 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 106
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup106 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 101
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup101 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 102
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup102 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 103
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup103 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 104
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup104 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 105
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup105 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 106
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup106 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 111
        /// </summary>

        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup111 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 112
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup112 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 113
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup113 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 114
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup114 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 115
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup115 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 116
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup116 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 111
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup111 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 112
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup112 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 113
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup113 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 114
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup114 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 115
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup115 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 116
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup116 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 121
        /// </summary>

        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup121 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 122
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup122 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 123
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup123 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 124
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup124 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 125
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup125 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion InclusionCriteriaGroup 126
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionInclusionCriteriaGroup126 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 121
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup121 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 122
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup122 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 123
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup123 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 124
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup124 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 125
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup125 { get; set; }
        /// <summary>
        /// Specify AdditionalExclusion EvaluationWindowGroup 126
        /// </summary>
        [DataMember]
        public string SpecifyAdditionalExclusionEvaluationWindowGroup126 { get; set; }
        /// <summary>
        /// Returns the value of LookBackPeriodGroup1
        /// </summary>



        [DataMember]
        public int? LookBackPeriodGroup1 { get; set; }
        /// <summary>
        /// Returns the value of LookBackPeriodGroup2
        /// </summary>
        [DataMember]
        public int? LookBackPeriodGroup2 { get; set; }
        /// <summary>
        /// Returns the value of LookBackPeriodGroup3
        /// </summary>
        [DataMember]
        public int? LookBackPeriodGroup3 { get; set; }
        /// <summary>
        /// Returns the value of LookBackPeriodGroup4
        /// </summary>
        [DataMember]
        public int? LookBackPeriodGroup4 { get; set; }
        /// <summary>
        /// Returns the value of LookBackPeriodGroup5
        /// </summary>
        [DataMember]
        public int? LookBackPeriodGroup5 { get; set; }
        /// <summary>
        /// Returns the value of LookBackPeriodGroup6
        /// </summary>
        [DataMember]
        public int? LookBackPeriodGroup6 { get; set; }
        /// <summary>
        /// Returns the value of LookBackPeriodGroup7
        /// </summary>
        [DataMember]
        public int? LookBackPeriodGroup7 { get; set; }
        /// <summary>
        /// Returns the value of LookBackPeriodGroup8
        /// </summary>
        [DataMember]
        public int? LookBackPeriodGroup8 { get; set; }
        /// <summary>
        /// Returns the value of LookBackPeriodGroup9
        /// </summary>
        [DataMember]
        public int? LookBackPeriodGroup9 { get; set; }
        /// <summary>
        /// Returns the value of LookBackPeriodGroup10
        /// </summary>
        [DataMember]
        public int? LookBackPeriodGroup10 { get; set; }
        /// <summary>
        /// Returns the value of LookBackPeriodGroup11
        /// </summary>
        [DataMember]
        public int? LookBackPeriodGroup11 { get; set; }
        /// <summary>
        /// Returns the value of LookBackPeriodGroup12
        /// </summary>
        [DataMember]
        public int? LookBackPeriodGroup12 { get; set; }
        /// <summary>
        /// Gets or sets the IncludeIndexDate1
        /// </summary>

        [DataMember]
        public bool IncludeIndexDate1 { get; set; }
        /// <summary>
        /// Gets or sets the IncludeIndexDate2
        /// </summary>
        [DataMember]
        public bool IncludeIndexDate2 { get; set; }
        /// <summary>
        /// Gets or sets the IncludeIndexDate3
        /// </summary>
        [DataMember]
        public bool IncludeIndexDate3 { get; set; }
        /// <summary>
        /// Gets or sets the IncludeIndexDate4
        /// </summary>
        [DataMember]
        public bool IncludeIndexDate4 { get; set; }
        /// <summary>
        /// Gets or sets the IncludeIndexDate5
        /// </summary>
        [DataMember]
        public bool IncludeIndexDate5 { get; set; }
        /// <summary>
        /// Gets or sets the IncludeIndexDate6
        /// </summary>
        [DataMember]
        public bool IncludeIndexDate6 { get; set; }
        /// <summary>
        /// Gets or sets the IncludeIndexDate7
        /// </summary>
        [DataMember]
        public bool IncludeIndexDate7 { get; set; }
        /// <summary>
        /// Gets or sets the IncludeIndexDate8
        /// </summary>
        [DataMember]
        public bool IncludeIndexDate8 { get; set; }
        /// <summary>
        /// Gets or sets the IncludeIndexDate9
        /// </summary>
        [DataMember]
        public bool IncludeIndexDate9 { get; set; }
        /// <summary>
        /// Gets or sets the IncludeIndexDate10
        /// </summary>
        [DataMember]
        public bool IncludeIndexDate10 { get; set; }
        /// <summary>
        /// Gets or sets the IncludeIndexDate11
        /// </summary>
        [DataMember]
        public bool IncludeIndexDate11 { get; set; }
        /// <summary>
        /// Gets or sets the IncludeIndexDate12
        /// </summary>
        [DataMember]
        public bool IncludeIndexDate12 { get; set; }
        /// <summary>
        /// StratificationCategories 1
        /// </summary>

        [DataMember]
        public string StratificationCategories1 { get; set; }
        /// <summary>
        /// StratificationCategories 2
        /// </summary>
        [DataMember]
        public string StratificationCategories2 { get; set; }
        /// <summary>
        /// StratificationCategories 3
        /// </summary>
        [DataMember]
        public string StratificationCategories3 { get; set; }
        /// <summary>
        /// StratificationCategories 4
        /// </summary>
        [DataMember]
        public string StratificationCategories4 { get; set; }
        /// <summary>
        /// StratificationCategories 5
        /// </summary>
        [DataMember]
        public string StratificationCategories5 { get; set; }
        /// <summary>
        /// StratificationCategories 6
        /// </summary>
        [DataMember]
        public string StratificationCategories6 { get; set; }
        /// <summary>
        /// StratificationCategories 7
        /// </summary>
        [DataMember]
        public string StratificationCategories7 { get; set; }
        /// <summary>
        /// StratificationCategories8
        /// </summary>
        [DataMember]
        public string StratificationCategories8 { get; set; }
        /// <summary>
        /// StratificationCategories9
        /// </summary>
        [DataMember]
        public string StratificationCategories9 { get; set; }
        /// <summary>
        /// StratificationCategories10
        /// </summary>
        [DataMember]
        public string StratificationCategories10 { get; set; }
        /// <summary>
        /// StratificationCategories11
        /// </summary>
        [DataMember]
        public string StratificationCategories11 { get; set; }
        /// <summary>
        /// StratificationCategories 12
        /// </summary>
        [DataMember]
        public string StratificationCategories12 { get; set; }
        /// <summary>
        /// Returns the value of TwelveSpecifyLoopBackPeriod1
        /// </summary>

        [DataMember]
        public int? TwelveSpecifyLoopBackPeriod1 { get; set; }
        /// <summary>
        /// Returns the value of TwelveSpecifyLoopBackPeriod2
        /// </summary>
        [DataMember]
        public int? TwelveSpecifyLoopBackPeriod2 { get; set; }
        /// <summary>
        /// Returns the value of TwelveSpecifyLoopBackPeriod3
        /// </summary>
        [DataMember]
        public int? TwelveSpecifyLoopBackPeriod3 { get; set; }
        /// <summary>
        /// Returns the value of TwelveSpecifyLoopBackPeriod4
        /// </summary>
        [DataMember]
        public int? TwelveSpecifyLoopBackPeriod4 { get; set; }
        /// <summary>
        /// Returns the value of TwelveSpecifyLoopBackPeriod5
        /// </summary>
        [DataMember]
        public int? TwelveSpecifyLoopBackPeriod5 { get; set; }
        /// <summary>
        /// Returns the value of TwelveSpecifyLoopBackPeriod6
        /// </summary>
        [DataMember]
        public int? TwelveSpecifyLoopBackPeriod6 { get; set; }
        /// <summary>
        /// Returns the value of TwelveSpecifyLoopBackPeriod7
        /// </summary>
        [DataMember]
        public int? TwelveSpecifyLoopBackPeriod7 { get; set; }
        /// <summary>
        /// Returns the value of TwelveSpecifyLoopBackPeriod8
        /// </summary>
        [DataMember]
        public int? TwelveSpecifyLoopBackPeriod8 { get; set; }
        /// <summary>
        /// Returns the value of TwelveSpecifyLoopBackPeriod9
        /// </summary>
        [DataMember]
        public int? TwelveSpecifyLoopBackPeriod9 { get; set; }
        /// <summary>
        /// Returns the value of TwelveSpecifyLoopBackPeriod10
        /// </summary>
        [DataMember]
        public int? TwelveSpecifyLoopBackPeriod10 { get; set; }
        /// <summary>
        /// Returns the value of TwelveSpecifyLoopBackPeriod11
        /// </summary>
        [DataMember]
        public int? TwelveSpecifyLoopBackPeriod11 { get; set; }
        /// <summary>
        /// Returns the value of TwelveSpecifyLoopBackPeriod12
        /// </summary>
        [DataMember]
        public int? TwelveSpecifyLoopBackPeriod12 { get; set; }
        /// <summary>
        /// Gets or sets the TwelveIncludeIndexDate1
        /// </summary>

        [DataMember]
        public bool TwelveIncludeIndexDate1 { get; set; }
        /// <summary>
        /// Gets or sets the TwelveIncludeIndexDate2
        /// </summary>
        [DataMember]
        public bool TwelveIncludeIndexDate2 { get; set; }
        /// <summary>
        /// Gets or sets the TwelveIncludeIndexDate3
        /// </summary>
        [DataMember]
        public bool TwelveIncludeIndexDate3 { get; set; }
        /// <summary>
        /// Gets or sets the TwelveIncludeIndexDate4
        /// </summary>
        [DataMember]
        public bool TwelveIncludeIndexDate4 { get; set; }
        /// <summary>
        /// Gets or sets the TwelveIncludeIndexDate5
        /// </summary>
        [DataMember]
        public bool TwelveIncludeIndexDate5 { get; set; }
        /// <summary>
        /// Gets or sets the TwelveIncludeIndexDate6
        /// </summary>
        [DataMember]
        public bool TwelveIncludeIndexDate6 { get; set; }
        /// <summary>
        /// Gets or sets the TwelveIncludeIndexDate7
        /// </summary>
        [DataMember]
        public bool TwelveIncludeIndexDate7 { get; set; }
        /// <summary>
        /// Gets or sets the TwelveIncludeIndexDate8
        /// </summary>
        [DataMember]
        public bool TwelveIncludeIndexDate8 { get; set; }
        /// <summary>
        /// Gets or sets the TwelveIncludeIndexDate9
        /// </summary>
        [DataMember]
        public bool TwelveIncludeIndexDate9 { get; set; }
        /// <summary>
        /// Gets or sets the TwelveIncludeIndexDate10
        /// </summary>
        [DataMember]
        public bool TwelveIncludeIndexDate10 { get; set; }
        /// <summary>
        /// Gets or sets the TwelveIncludeIndexDate11
        /// </summary>
        [DataMember]
        public bool TwelveIncludeIndexDate11 { get; set; }
        /// <summary>
        /// Gets or sets the TwelveIncludeIndexDate12
        /// </summary>
        [DataMember]
        public bool TwelveIncludeIndexDate12 { get; set; }
        /// <summary>
        /// CareSettings ToDefine MedicalVisits1
        /// </summary>
        [DataMember]
        public string CareSettingsToDefineMedicalVisits1 { get; set; }
        /// <summary>
        /// CareSettingsToDefineMedicalVisits2
        /// </summary>
        [DataMember]
        public string CareSettingsToDefineMedicalVisits2 { get; set; }
        /// <summary>
        /// CareSettingsToDefineMedicalVisits3
        /// </summary>
        [DataMember]
        public string CareSettingsToDefineMedicalVisits3 { get; set; }
        /// <summary>
        /// CareSettingsToDefineMedicalVisits4
        /// </summary>
        [DataMember]
        public string CareSettingsToDefineMedicalVisits4 { get; set; }
        /// <summary>
        /// CareSettingsToDefineMedicalVisits5
        /// </summary>
        [DataMember]
        public string CareSettingsToDefineMedicalVisits5 { get; set; }
        /// <summary>
        /// CareSettingsToDefineMedicalVisits6
        /// </summary>
        [DataMember]
        public string CareSettingsToDefineMedicalVisits6 { get; set; }
        /// <summary>
        /// CareSettingsToDefineMedicalVisits7
        /// </summary>
        [DataMember]
        public string CareSettingsToDefineMedicalVisits7 { get; set; }
        /// <summary>
        /// CareSettingsToDefineMedicalVisits8
        /// </summary>
        [DataMember]
        public string CareSettingsToDefineMedicalVisits8 { get; set; }
        /// <summary>
        /// CareSettingsToDefineMedicalVisits9
        /// </summary>
        [DataMember]
        public string CareSettingsToDefineMedicalVisits9 { get; set; }
        /// <summary>
        /// CareSettingsToDefineMedicalVisits10
        /// </summary>
        public string CareSettingsToDefineMedicalVisits10 { get; set; }
        /// <summary>
        /// CareSettingsToDefineMedicalVisits11
        /// </summary>
        [DataMember]
        public string CareSettingsToDefineMedicalVisits11 { get; set; }
        /// <summary>
        /// CareSettingsToDefineMedicalVisits12
        /// </summary>
        [DataMember]
        public string CareSettingsToDefineMedicalVisits12 { get; set; }
         /// <summary>
         /// TwelveStratificationCategories1
         /// </summary>

        [DataMember]
        public string TwelveStratificationCategories1 { get; set; }
        /// <summary>
        /// TwelveStratificationCategories2
        /// </summary>
        [DataMember]
        public string TwelveStratificationCategories2 { get; set; }
        /// <summary>
        /// TwelveStratificationCategories3
        /// </summary>
        [DataMember]
        public string TwelveStratificationCategories3 { get; set; }
        /// <summary>
        /// TwelveStratificationCategories4
        /// </summary>
        [DataMember]
        public string TwelveStratificationCategories4 { get; set; }
        /// <summary>
        /// TwelveStratificationCategories5
        /// </summary>
        [DataMember]
        public string TwelveStratificationCategories5 { get; set; }
        /// <summary>
        /// TwelveStratificationCategories6
        /// </summary>
        [DataMember]
        public string TwelveStratificationCategories6 { get; set; }
        /// <summary>
        /// TwelveStratificationCategories7
        /// </summary>
        [DataMember]
        public string TwelveStratificationCategories7 { get; set; }
        /// <summary>
        /// TwelveStratificationCategories8
        /// </summary>
        [DataMember]
        public string TwelveStratificationCategories8 { get; set; }
        /// <summary>
        /// TwelveStratificationCategories9
        /// </summary>
        [DataMember]
        public string TwelveStratificationCategories9 { get; set; }
        /// <summary>
        /// TwelveStratificationCategories10
        /// </summary>
        [DataMember]
        public string TwelveStratificationCategories10 { get; set; }
        /// <summary>
        /// TwelveStratificationCategories11
        /// </summary>
        [DataMember]
        public string TwelveStratificationCategories11 { get; set; }
        /// <summary>
        /// TwelveStratificationCategories12
        /// </summary>
        [DataMember]
        public string TwelveStratificationCategories12 { get; set; }
        /// <summary>
        /// returns the value of VaryLengthOfWashoutPeriod1
        /// </summary>

        [DataMember]
        public int? VaryLengthOfWashoutPeriod1 { get; set; }
        /// <summary>
        /// returns the value of VaryLengthOfWashoutPeriod2
        /// </summary>
        [DataMember]
        public int? VaryLengthOfWashoutPeriod2 { get; set; }
        /// <summary>
        /// returns the value of VaryLengthOfWashoutPeriod3
        /// </summary>
        [DataMember]
        public int? VaryLengthOfWashoutPeriod3 { get; set; }
        /// <summary>
        /// returns the value of VaryLengthOfWashoutPeriod4
        /// </summary>
        [DataMember]
        public int? VaryLengthOfWashoutPeriod4 { get; set; }
        /// <summary>
        /// returns the value of VaryLengthOfWashoutPeriod5
        /// </summary>
        [DataMember]
        public int? VaryLengthOfWashoutPeriod5 { get; set; }
        /// <summary>
        /// returns the value of VaryLengthOfWashoutPeriod6
        /// </summary>
        [DataMember]
        public int? VaryLengthOfWashoutPeriod6 { get; set; }
        /// <summary>
        /// returns the value of VaryLengthOfWashoutPeriod7
        /// </summary>
        [DataMember]
        public int? VaryLengthOfWashoutPeriod7 { get; set; }
        /// <summary>
        /// returns the value of VaryLengthOfWashoutPeriod8
        /// </summary>
        [DataMember]
        public int? VaryLengthOfWashoutPeriod8 { get; set; }
        /// <summary>
        /// returns the value of VaryLengthOfWashoutPeriod9
        /// </summary>
        [DataMember]
        public int? VaryLengthOfWashoutPeriod9 { get; set; }
        /// <summary>
        /// returns the value of VaryLengthOfWashoutPeriod10
        /// </summary>
        [DataMember]
        public int? VaryLengthOfWashoutPeriod10 { get; set; }
        /// <summary>
        /// returns the value of VaryLengthOfWashoutPeriod11
        /// </summary>
        [DataMember]
        public int? VaryLengthOfWashoutPeriod11 { get; set; }
        /// <summary>
        /// returns the value of VaryLengthOfWashoutPeriod12
        /// </summary>
        [DataMember]
        public int? VaryLengthOfWashoutPeriod12 { get; set; }
        /// <summary>
        /// Gets or sets the VaryUserExposedTime1
        /// </summary>

        [DataMember]
        public bool VaryUserExposedTime1 { get; set; }
        /// <summary>
        ///  Gets or sets the VaryUserExposedTime2
        /// </summary>
        [DataMember]
        public bool VaryUserExposedTime2 { get; set; }
        /// <summary>
        ///  Gets or sets the VaryUserExposedTime3
        /// </summary>
        [DataMember]
        public bool VaryUserExposedTime3 { get; set; }
        /// <summary>
        ///  Gets or sets the VaryUserExposedTime4
        /// </summary>
        [DataMember]
        public bool VaryUserExposedTime4 { get; set; }
        /// <summary>
        ///  Gets or sets the VaryUserExposedTime5
        /// </summary>
        [DataMember]
        public bool VaryUserExposedTime5 { get; set; }
        /// <summary>
        ///  Gets or sets the VaryUserExposedTime6
        /// </summary>
        [DataMember]
        public bool VaryUserExposedTime6 { get; set; }
        /// <summary>
        ///  Gets or sets the VaryUserExposedTime7
        /// </summary>
        [DataMember]
        public bool VaryUserExposedTime7 { get; set; }
        /// <summary>
        ///  Gets or sets the VaryUserExposedTime8
        /// </summary>
        [DataMember]
        public bool VaryUserExposedTime8 { get; set; }
        /// <summary>
        ///  Gets or sets the VaryUserExposedTime9
        /// </summary>
        [DataMember]
        public bool VaryUserExposedTime9 { get; set; }
        /// <summary>
        ///  Gets or sets the VaryUserExposedTime10
        /// </summary>
        [DataMember]
        public bool VaryUserExposedTime10 { get; set; }
        /// <summary>
        ///  Gets or sets the VaryUserExposedTime11
        /// </summary>
        [DataMember]
        public bool VaryUserExposedTime11 { get; set; }
        /// <summary>
        ///  Gets or sets the VaryUserExposedTime12
        /// </summary>
        [DataMember]
        public bool VaryUserExposedTime12 { get; set; }
        /// <summary>
        /// gets or sets the VaryUserFollowupPeriodDuration1
        /// </summary>

        [DataMember]
        public bool VaryUserFollowupPeriodDuration1 { get; set; }
        /// <summary>
        /// gets or set the VaryUserFollowupPeriodDuration2
        /// </summary>
        [DataMember]
        public bool VaryUserFollowupPeriodDuration2 { get; set; }
        /// <summary>
        /// gets or set the VaryUserFollowupPeriodDuration3
        /// </summary>
        [DataMember]
        public bool VaryUserFollowupPeriodDuration3 { get; set; }
        /// <summary>
        /// gets or set the VaryUserFollowupPeriodDuration4
        /// </summary>
        [DataMember]
        public bool VaryUserFollowupPeriodDuration4 { get; set; }
        /// <summary>
        /// gets or set the VaryUserFollowupPeriodDuration5
        /// </summary>
        [DataMember]
        public bool VaryUserFollowupPeriodDuration5 { get; set; }
        /// <summary>
        /// gets or set the VaryUserFollowupPeriodDuration6
        /// </summary>
        [DataMember]
        public bool VaryUserFollowupPeriodDuration6 { get; set; }
        /// <summary>
        /// gets or set the VaryUserFollowupPeriodDuration7
        /// </summary>
        [DataMember]
        public bool VaryUserFollowupPeriodDuration7 { get; set; }
        /// <summary>
        /// gets or set the VaryUserFollowupPeriodDuration8
        /// </summary>
        [DataMember]
        public bool VaryUserFollowupPeriodDuration8 { get; set; }
        /// <summary>
        /// gets or set the VaryUserFollowupPeriodDuration9
        /// </summary>
        [DataMember]
        public bool VaryUserFollowupPeriodDuration9 { get; set; }
        /// <summary>
        /// gets or set the VaryUserFollowupPeriodDuration10
        /// </summary>
        [DataMember]
        public bool VaryUserFollowupPeriodDuration10 { get; set; }
        /// <summary>
        /// gets or set the VaryUserFollowupPeriodDuration11
        /// </summary>
        [DataMember]
        public bool VaryUserFollowupPeriodDuration11 { get; set; }
        /// <summary>
        /// gets or set the VaryUserFollowupPeriodDuration12
        /// </summary>
        [DataMember]
        public bool VaryUserFollowupPeriodDuration12 { get; set; }
        /// <summary>
        /// returns the value of VaryBlackoutPeriodPeriod1
        /// </summary>

        [DataMember]
        public int? VaryBlackoutPeriodPeriod1 { get; set; }
        /// <summary>
        /// returns the value of VaryBlackoutPeriodPeriod2
        /// </summary>
        [DataMember]
        public int? VaryBlackoutPeriodPeriod2 { get; set; }
        /// <summary>
        /// returns the value of VaryBlackoutPeriodPeriod3
        /// </summary>
        [DataMember]
        public int? VaryBlackoutPeriodPeriod3 { get; set; }
        /// <summary>
        /// returns the value of VaryBlackoutPeriodPeriod4
        /// </summary>
        [DataMember]
        public int? VaryBlackoutPeriodPeriod4 { get; set; }
        /// <summary>
        /// returns the value of VaryBlackoutPeriodPeriod5
        /// </summary>
        [DataMember]
        public int? VaryBlackoutPeriodPeriod5 { get; set; }
        /// <summary>
        /// returns the value of VaryBlackoutPeriodPeriod6
        /// </summary>
        [DataMember]
        public int? VaryBlackoutPeriodPeriod6 { get; set; }
        /// <summary>
        /// returns the value of VaryBlackoutPeriodPeriod7
        /// </summary>
        [DataMember]
        public int? VaryBlackoutPeriodPeriod7 { get; set; }
        /// <summary>
        /// returns the value of VaryBlackoutPeriodPeriod8
        /// </summary>
        [DataMember]
        public int? VaryBlackoutPeriodPeriod8 { get; set; }
        /// <summary>
        /// returns the value of VaryBlackoutPeriodPeriod9
        /// </summary>
        [DataMember]
        public int? VaryBlackoutPeriodPeriod9 { get; set; }
        /// <summary>
        /// returns the value of VaryBlackoutPeriodPeriod10
        /// </summary>
        [DataMember]
        public int? VaryBlackoutPeriodPeriod10 { get; set; }
        /// <summary>
        /// returns the value of VaryBlackoutPeriodPeriod11
        /// </summary>
        [DataMember]
        public int? VaryBlackoutPeriodPeriod11 { get; set; }
        /// <summary>
        /// returns the value of VaryBlackoutPeriodPeriod12
        /// </summary>
        [DataMember]
        public int? VaryBlackoutPeriodPeriod12 { get; set; }
        /// <summary>
        /// Level2or3DefineExposures1Exposure
        /// </summary>

        #endregion

        #region Section3C
        [DataMember]
        public string Level2or3DefineExposures1Exposure { get; set; }
        /// <summary>
        /// Level2or3DefineExposures1compare
        /// </summary>
        [DataMember]
        public string Level2or3DefineExposures1Compare { get; set; }
        /// <summary>
        /// Level2or3DefineExposures2Exposure
        /// </summary>
        [DataMember]
        public string Level2or3DefineExposures2Exposure { get; set; }
        /// <summary>
        /// Level2or3DefineExposures2Compare
        /// </summary>
        [DataMember]
        public string Level2or3DefineExposures2Compare { get; set; }
        /// <summary>
        /// Level2or3DefineExposures3Exposure
        /// </summary>
        [DataMember]
        public string Level2or3DefineExposures3Exposure { get; set; }
        /// <summary>
        /// Level2or3DefineExposures3Compare
        /// </summary>
        [DataMember]
        public string Level2or3DefineExposures3Compare { get; set; }
        /// <summary>
        /// returns the value of Level2or3WashoutPeriod1Exposure
        /// </summary>

        [DataMember]
        public int? Level2or3WashoutPeriod1Exposure { get; set; }
        /// <summary>
        /// returns the value of Level2or3WashoutPeriod1Compare
        /// </summary>
        [DataMember]
        public int? Level2or3WashoutPeriod1Compare { get; set; }
        /// <summary>
        /// returns the value of  Level2or3WashoutPeriod2Exposure
        /// </summary>
        [DataMember]
        public int? Level2or3WashoutPeriod2Exposure { get; set; }
        /// <summary>
        /// returns the value of Level2or3WashoutPeriod2Compare
        /// </summary>
        [DataMember]
        public int? Level2or3WashoutPeriod2Compare { get; set; }
        /// <summary>
        /// returns the value of Level2or3WashoutPeriod3Exposure
        /// </summary>
        [DataMember]
        public int? Level2or3WashoutPeriod3Exposure { get; set; }
        /// <summary>
        /// returns the value of Level2or3WashoutPeriod3Compare
        /// </summary>
        [DataMember]
        public int? Level2or3WashoutPeriod3Compare { get; set; }
        /// <summary>
        /// Level 2or3 SpecifyExposedTimeAssessment1Exposure
        /// </summary>

        [DataMember]
        public WorkflowMPSpecifyExposedTimeAssessments? Level2or3SpecifyExposedTimeAssessment1Exposure { get; set; }
        /// <summary>
        /// Level 2or3 SpecifyExposedTimeAssessment1Compare
        /// </summary>
        [DataMember]
        public WorkflowMPSpecifyExposedTimeAssessments? Level2or3SpecifyExposedTimeAssessment1Compare { get; set; }
        /// <summary>
        /// Level 2or3 SpecifyExposedTimeAssessment2Exposure
        /// </summary>
        [DataMember]
        public WorkflowMPSpecifyExposedTimeAssessments? Level2or3SpecifyExposedTimeAssessment2Exposure { get; set; }
        /// <summary>
        /// Level 2or3 SpecifyExposedTimeAssessment2Compare
        /// </summary>
        [DataMember]
        public WorkflowMPSpecifyExposedTimeAssessments? Level2or3SpecifyExposedTimeAssessment2Compare { get; set; }
        /// <summary>
        /// Level 2or3 SpecifyExposedTimeAssessment3Exposure
        /// </summary>
        [DataMember]
        public WorkflowMPSpecifyExposedTimeAssessments? Level2or3SpecifyExposedTimeAssessment3Exposure { get; set; }
        /// <summary>
        /// Level 2or3 SpecifyExposedTimeAssessment3Compare
        /// </summary>
        [DataMember]
        public WorkflowMPSpecifyExposedTimeAssessments? Level2or3SpecifyExposedTimeAssessment3Compare { get; set; }
        /// <summary>
        /// returns the value of Level2or3EpisodeAllowableGap1Exposure
        /// </summary>

        [DataMember]
        public int? Level2or3EpisodeAllowableGap1Exposure { get; set; }
        /// <summary>
        /// returns the value of Level2or3EpisodeAllowableGap1Compare
        /// </summary>
        [DataMember]
        public int? Level2or3EpisodeAllowableGap1Compare { get; set; }
        /// <summary>
        /// returns the value of Level2or3EpisodeAllowableGap2Exposure
        /// </summary>
        [DataMember]
        public int? Level2or3EpisodeAllowableGap2Exposure { get; set; }
        /// <summary>
        /// returns the value of Level2or3EpisodeAllowableGap2Compare
        /// </summary>
        [DataMember]
        public int? Level2or3EpisodeAllowableGap2Compare { get; set; }
        /// <summary>
        /// returns the value of Level2or3EpisodeAllowableGap3Exposure
        /// </summary>
        [DataMember]
        public int? Level2or3EpisodeAllowableGap3Exposure { get; set; }
        /// <summary>
        /// returns the value of Level2or3EpisodeAllowableGap3Compare
        /// </summary>
        [DataMember]
        public int? Level2or3EpisodeAllowableGap3Compare { get; set; }
        /// <summary>
        /// returns the value of Level2or3EpisodeExtensionPeriod1Exposure
        /// </summary>


        [DataMember]
        public int? Level2or3EpisodeExtensionPeriod1Exposure { get; set; }
        /// <summary>
        /// returns the value of Level2or3EpisodeExtensionPeriod1Compare
        /// </summary>
        [DataMember]
        public int? Level2or3EpisodeExtensionPeriod1Compare { get; set; }
        /// <summary>
        /// returns the value of Level2or3EpisodeExtensionPeriod2Exposure
        /// </summary>
        [DataMember]
        public int? Level2or3EpisodeExtensionPeriod2Exposure { get; set; }
        /// <summary>
        /// returns the value of Level2or3EpisodeExtensionPeriod2Compare
        /// </summary>
        [DataMember]
        public int? Level2or3EpisodeExtensionPeriod2Compare { get; set; }
        /// <summary>
        /// returns the value of Level2or3EpisodeExtensionPeriod3Exposure
        /// </summary>
        [DataMember]
        public int? Level2or3EpisodeExtensionPeriod3Exposure { get; set; }
        /// <summary>
        /// returns the value of Level2or3EpisodeExtensionPeriod3Compare
        /// </summary>
        [DataMember]
        public int? Level2or3EpisodeExtensionPeriod3Compare { get; set; }
        /// <summary>
        /// returns the value of Level2or3MinimumEpisodeDuration1Exposure
        /// </summary>

        [DataMember]
        public int? Level2or3MinimumEpisodeDuration1Exposure { get; set; }
        /// <summary>
        /// returns the value of Level2or3MinimumEpisodeDuration1Compare
        /// </summary>
        [DataMember]
        public int? Level2or3MinimumEpisodeDuration1Compare { get; set; }
        /// <summary>
        /// returns the value of Level2or3MinimumEpisodeDuration2Exposure
        /// </summary>
        [DataMember]
        public int? Level2or3MinimumEpisodeDuration2Exposure { get; set; }
        /// <summary>
        /// returns the value of Level2or3MinimumEpisodeDuration2Compare
        /// </summary>
        [DataMember]
        public int? Level2or3MinimumEpisodeDuration2Compare { get; set; }
        /// <summary>
        /// returns the value of Level2or3MinimumEpisodeDuration3Exposure
        /// </summary>
        [DataMember]
        public int? Level2or3MinimumEpisodeDuration3Exposure { get; set; }
        /// <summary>
        /// returns the value of Level2or3MinimumEpisodeDuration3Compare
        /// </summary>
        [DataMember]
        public int? Level2or3MinimumEpisodeDuration3Compare { get; set; }
        /// <summary>
        /// returns the value of Level2or3MinimumDaysSupply1Exposure
        /// </summary>

        [DataMember]
        public int? Level2or3MinimumDaysSupply1Exposure { get; set; }
        /// <summary>
        /// returns the value of Level2or3MinimumDaysSupply1Compare
        /// </summary>
        [DataMember]
        public int? Level2or3MinimumDaysSupply1Compare { get; set; }
        /// <summary>
        /// returns the value of Level2or3MinimumDaysSupply2Exposure
        /// </summary>
        [DataMember]
        public int? Level2or3MinimumDaysSupply2Exposure { get; set; }
        /// <summary>
        /// returns the value of Level2or3MinimumDaysSupply2Compare
        /// </summary>
        [DataMember]
        public int? Level2or3MinimumDaysSupply2Compare { get; set; }
        /// <summary>
        /// returns the value of Level2or3MinimumDaysSupply3Exposure
        /// </summary>
        [DataMember]
        public int? Level2or3MinimumDaysSupply3Exposure { get; set; }
        /// <summary>
        /// Level2or3MinimumDaysSupply3Compare
        /// </summary>
        [DataMember]
        public int? Level2or3MinimumDaysSupply3Compare { get; set; }
        /// <summary>
        /// Level2or3SpecifyFollowUpDuration1Exposure
        /// </summary>

        [DataMember]
        public int? Level2or3SpecifyFollowUpDuration1Exposure { get; set; }
        /// <summary>
        /// Level2or3SpecifyFollowUpDuration1Compare
        /// </summary>
        [DataMember]
        public int? Level2or3SpecifyFollowUpDuration1Compare { get; set; }
        /// <summary>
        /// Level2or3SpecifyFollowUpDuration2Exposure
        /// </summary>
        [DataMember]
        public int? Level2or3SpecifyFollowUpDuration2Exposure { get; set; }
        /// <summary>
        /// Level2or3SpecifyFollowUpDuration2Compare
        /// </summary>
        [DataMember]
        public int? Level2or3SpecifyFollowUpDuration2Compare { get; set; }
        /// <summary>
        /// Level2or3SpecifyFollowUpDuration3Exposure
        /// </summary>
        [DataMember]
        public int? Level2or3SpecifyFollowUpDuration3Exposure { get; set; }
        /// <summary>
        /// Level2or3SpecifyFollowUpDuration3Compare
        /// </summary>
        [DataMember]
        public int? Level2or3SpecifyFollowUpDuration3Compare { get; set; }
        /// <summary>
        /// gets or sets the workflow MP Level2or3 AllowOnOrMultipleExposureEpisodes1Exposure
        /// </summary>

        [DataMember]
        public WorkflowMPAllowOnOrMultipleExposureEpisodes? Level2or3AllowOnOrMultipleExposureEpisodes1Exposure { get; set; }
        /// <summary>
        /// gets or sets the workflow MP Level2or3AllowOnOrMultipleExposureEpisodes1Compare
        /// </summary>
        [DataMember]
        public WorkflowMPAllowOnOrMultipleExposureEpisodes? Level2or3AllowOnOrMultipleExposureEpisodes1Compare { get; set; }
        /// <summary>
        /// gets or sets the workflow MP Level2or3AllowOnOrMultipleExposureEpisodes2Exposure
        /// </summary>
        [DataMember]
        public WorkflowMPAllowOnOrMultipleExposureEpisodes? Level2or3AllowOnOrMultipleExposureEpisodes2Exposure { get; set; }
        /// <summary>
        /// gets or sets the workflow MP Level2or3AllowOnOrMultipleExposureEpisodes2Compare
        /// </summary>
        [DataMember]
        public WorkflowMPAllowOnOrMultipleExposureEpisodes? Level2or3AllowOnOrMultipleExposureEpisodes2Compare { get; set; }
        /// <summary>
        /// gets or sets the workflow MP Level2or3AllowOnOrMultipleExposureEpisodes3Exposure
        /// </summary>
        [DataMember]
        public WorkflowMPAllowOnOrMultipleExposureEpisodes? Level2or3AllowOnOrMultipleExposureEpisodes3Exposure { get; set; }
        /// <summary>
        /// gets or sets the workflow MP Level2or3AllowOnOrMultipleExposureEpisodes3Compare
        /// </summary>
        [DataMember]
        public WorkflowMPAllowOnOrMultipleExposureEpisodes? Level2or3AllowOnOrMultipleExposureEpisodes3Compare { get; set; }
        /// <summary>
        /// gets or sets the Level2or3TruncateExposedtime1Exposure
        /// </summary>

        [DataMember]
        public bool Level2or3TruncateExposedtime1Exposure { get; set; }
        /// <summary>
        /// gets or sets the Level2or3TruncateExposedtime1Compare
        /// </summary>
        [DataMember]
        public bool Level2or3TruncateExposedtime1Compare { get; set; }
        /// <summary>
        /// gets or sets the Level2or3TruncateExposedtime2Exposure
        /// </summary>
        [DataMember]
        public bool Level2or3TruncateExposedtime2Exposure { get; set; }
        /// <summary>
        /// gets or sets the Level2or3TruncateExposedtime2Compare
        /// </summary>
        [DataMember]
        public bool Level2or3TruncateExposedtime2Compare { get; set; }
        /// <summary>
        /// gets or sets the Level2or3TruncateExposedtime3Exposure
        /// </summary>
        [DataMember]
        public bool Level2or3TruncateExposedtime3Exposure { get; set; }
        /// <summary>
        /// gets or sets the Level2or3TruncateExposedtime3Compare
        /// </summary>
        [DataMember]
        public bool Level2or3TruncateExposedtime3Compare { get; set; }
        /// <summary>
        /// Level2or3TruncateExposedTimeSpecified1Exposure
        /// </summary>

        [DataMember]
        public string Level2or3TruncateExposedTimeSpecified1Exposure { get; set; }
        /// <summary>
        /// Level2or3TruncateExposedTimeSpecified1Compare
        /// </summary>
        [DataMember]
        public string Level2or3TruncateExposedTimeSpecified1Compare { get; set; }
        /// <summary>
        /// Level2or3TruncateExposedTimeSpecified2Exposure
        /// </summary>
        [DataMember]
        public string Level2or3TruncateExposedTimeSpecified2Exposure { get; set; }
        /// <summary>
        /// Level2or3TruncateExposedTimeSpecified2Compare
        /// </summary>
        [DataMember]
        public string Level2or3TruncateExposedTimeSpecified2Compare { get; set; }
        /// <summary>
        /// Level2or3TruncateExposedTimeSpecified3Exposure
        /// </summary>
        [DataMember]
        public string Level2or3TruncateExposedTimeSpecified3Exposure { get; set; }
        /// <summary>
        ///  Level2or3TruncateExposedTimeSpecified3Compare
        /// </summary>
        [DataMember]
        public string Level2or3TruncateExposedTimeSpecified3Compare { get; set; }
        /// <summary>
        /// returns the value of Level2or3SpecifyBlackoutPeriod1Exposure
        /// </summary>

        [DataMember]
        public int? Level2or3SpecifyBlackoutPeriod1Exposure { get; set; }
        /// <summary>
        /// returns the value of Level2or3SpecifyBlackoutPeriod1Compare
        /// </summary>
        [DataMember]
        public int? Level2or3SpecifyBlackoutPeriod1Compare { get; set; }
        /// <summary>
        /// returns the value of Level2or3SpecifyBlackoutPeriod2Exposure
        /// </summary>
        [DataMember]
        public int? Level2or3SpecifyBlackoutPeriod2Exposure { get; set; }
        /// <summary>
        /// returns the value of Level2or3SpecifyBlackoutPeriod2Compare
        /// </summary>
        [DataMember]
        public int? Level2or3SpecifyBlackoutPeriod2Compare { get; set; }
        /// <summary>
        /// returns the value of Level2or3SpecifyBlackoutPeriod3Exposure
        /// </summary>
        [DataMember]
        public int? Level2or3SpecifyBlackoutPeriod3Exposure { get; set; }
        /// <summary>
        /// returns the value of Level2or3SpecifyBlackoutPeriod3Compare
        /// </summary>
        [DataMember]
        public int? Level2or3SpecifyBlackoutPeriod3Compare { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup11
        /// </summary>


        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup11 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup12
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup12 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup13
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup13 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup11
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup11 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup12
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup12 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup13
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup13 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup21
        /// </summary>

        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup21 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup22
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup22 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup23
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup23 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup21
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup21 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup22
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup22 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup23
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup23 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup31
        /// </summary>
        
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup31 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup32
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup32 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup33
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup33 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup31
        /// </summary>
    
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup31 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup32
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup32 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup33
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup33 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup41
        /// </summary>

        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup41 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup42
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup42 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup43
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup43 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup41
        /// </summary>

        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup41 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup42
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup42 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup43
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup43 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup51
        /// </summary>

        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup51 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup5
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup52 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup53
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup53 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup51
        /// </summary>

        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup51 { get; set; }
        /// <summary>
        ///  Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup52
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup52 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup53
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup53 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup61
        /// </summary>

        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup61 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup62
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup62 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup63
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup63 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup61
        /// </summary>

        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup61 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup62
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup62 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup63
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup63 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup11
        /// </summary>


        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup11 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup12
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup12 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup13
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup13 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup11
        /// </summary>
        
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup11 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup12
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup12 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup13
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup13 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup21
        /// </summary>

        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup21 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup22
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup22 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup23
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup23 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup21
        /// </summary>

        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup21 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup22
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup22 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup23
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup23 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup31
        /// </summary>

        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup31 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup32
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup32 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup33
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup33 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup31
        /// </summary>

        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup31 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup32 
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup32 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup33
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup33 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup41
        /// </summary>

        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup41 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup42
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup42 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup43
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup43 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup41
        /// </summary>

        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup41 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup42
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup42 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup43
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup43 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup51
        /// </summary>

        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup51 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup52
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup52 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup53
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup53 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup51
        /// </summary>

        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup51 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup52
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup52 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup53
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup53 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup61
        /// </summary>

        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup61 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup62
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup62 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup63
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup63 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup61
        /// </summary>

        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup61 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup62
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup62 { get; set; }
        /// <summary>
        /// Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup63
        /// </summary>
        [DataMember]
        public string Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup63 { get; set; }
        /// <summary>
        /// returns the value of Level2or3VaryLengthOfWashoutPeriod1Exposure
        /// </summary>

        [DataMember]
        public int? Level2or3VaryLengthOfWashoutPeriod1Exposure { get; set; }
        /// <summary>
        /// returns the value of Level2or3VaryLengthOfWashoutPeriod1Compare
        /// </summary>
        [DataMember]
        public int? Level2or3VaryLengthOfWashoutPeriod1Compare { get; set; }
        /// <summary>
        /// returns the value of Level2or3VaryLengthOfWashoutPeriod2Exposure
        /// </summary>
        [DataMember]
        public int? Level2or3VaryLengthOfWashoutPeriod2Exposure { get; set; }
        /// <summary>
        /// returns the value of Level2or3VaryLengthOfWashoutPeriod2Compare
        /// </summary>
        [DataMember]
        public int? Level2or3VaryLengthOfWashoutPeriod2Compare { get; set; }
        /// <summary>
        /// returns the value of Level2or3VaryLengthOfWashoutPeriod3Exposure
        /// </summary>
        [DataMember]
        public int? Level2or3VaryLengthOfWashoutPeriod3Exposure { get; set; }
        /// <summary>
        /// returns the value of Level2or3VaryLengthOfWashoutPeriod3Compare
        /// </summary>
        [DataMember]
        public int? Level2or3VaryLengthOfWashoutPeriod3Compare { get; set; }
        /// <summary>
        /// gets or sets the Level2or3VaryUserExposedTime1Exposure
        /// </summary>

        [DataMember]
        public bool Level2or3VaryUserExposedTime1Exposure { get; set; }
        /// <summary>
        /// gets or sets the Level2or3VaryUserExposedTime1Compare
        /// </summary>
        [DataMember]
        public bool Level2or3VaryUserExposedTime1Compare { get; set; }
        /// <summary>
        /// gets or sets the Level2or3VaryUserExposedTime2Exposure
        /// </summary>
        [DataMember]
        public bool Level2or3VaryUserExposedTime2Exposure { get; set; }
        /// <summary>
        /// gets or sets the Level2or3VaryUserExposedTime2Compare
        /// </summary>
        [DataMember]
        public bool Level2or3VaryUserExposedTime2Compare { get; set; }
        /// <summary>
        /// gets or sets the Level2or3VaryUserExposedTime3Exposure
        /// </summary>
        [DataMember]
        public bool Level2or3VaryUserExposedTime3Exposure { get; set; }
        /// <summary>
        /// gets or sets the Level2or3VaryUserExposedTime3Compare
        /// </summary>
        [DataMember]
        public bool Level2or3VaryUserExposedTime3Compare { get; set; }
        /// <summary>
        /// returns the value of Level2or3VaryBlackoutPeriodPeriod1Exposure
        /// </summary>

        [DataMember]
        public int? Level2or3VaryBlackoutPeriodPeriod1Exposure { get; set; }
        /// <summary>
        ///  returns the value for Level2or3VaryBlackoutPeriodPeriod1Compare
        /// </summary>
        [DataMember]
        public int? Level2or3VaryBlackoutPeriodPeriod1Compare { get; set; }
        /// <summary>
        /// returns the value for Level2or3VaryBlackoutPeriodPeriod2Exposure
        /// </summary>
        [DataMember]
        public int? Level2or3VaryBlackoutPeriodPeriod2Exposure { get; set; }
        /// <summary>
        /// returns the value for Level2or3VaryBlackoutPeriodPeriod2Compare
        /// </summary>
        [DataMember]
        public int? Level2or3VaryBlackoutPeriodPeriod2Compare { get; set; }
        /// <summary>
        /// returns the value for Level2or3VaryBlackoutPeriodPeriod3Exposure
        /// </summary>
        [DataMember]
        public int? Level2or3VaryBlackoutPeriodPeriod3Exposure { get; set; }
        /// <summary>
        /// returns the value for Level2or3VaryBlackoutPeriodPeriod3Compare
        /// </summary>
        [DataMember]
        public int? Level2or3VaryBlackoutPeriodPeriod3Compare { get; set; }
        
        

        #endregion
        #endregion


        #region Section4
        //Section 4
        /// <summary>
        /// Available outcome list
        /// </summary>
        [DataMember]
        public IEnumerable<OutcomeItemDTO> OutcomeList { get; set; }
        #endregion

          #region Section5
        //Section 5
        /// <summary>
        /// AgeCovariate
        /// </summary>
        [DataMember]
        public string AgeCovariate { get; set; }
        /// <summary>
        /// SexCovariate
        /// </summary>
        [DataMember]
        public string SexCovariate { get; set; }
        /// <summary>
        /// TimeCovariate
        /// </summary>
        [DataMember]
        public string TimeCovariate { get; set; }
        /// <summary>
        /// YearCovariate
        /// </summary>
        [DataMember]
        public string YearCovariate { get; set; }
        /// <summary>
        /// ComorbidityCovariate
        /// </summary>
        [DataMember]
        public string ComorbidityCovariate { get; set; }
        /// <summary>
        /// HealthCovariate
        /// </summary>
        [DataMember]
        public string HealthCovariate { get; set; }
        /// <summary>
        /// DrugCovariate
        /// </summary>
        [DataMember]
        public string DrugCovariate { get; set; }
        /// <summary>
        /// available covariate list
        /// </summary>
        [DataMember]
        public IEnumerable<CovariateItemDTO> CovariateList { get; set; }
        

        #endregion

        #region Section6

        //Section 6
        /// <summary>
        /// hdPSAnalysis
        /// </summary>
        [DataMember]
        public string hdPSAnalysis { get; set; }
        /// <summary>
        /// returns the value for InclusionCovariates
        /// </summary>
        [DataMember]
        public int InclusionCovariates { get; set; }
        /// <summary>
        /// returns the value for PoolCovariates
        /// </summary>
        [DataMember]
        public int PoolCovariates { get; set; }
        /// <summary>
        /// SelectionCovariates
        /// </summary>
        [DataMember]
        public string SelectionCovariates { get; set; }
        /// <summary>
        /// ZeroCellCorrection
        /// </summary>
        [DataMember]
        public string ZeroCellCorrection { get; set; }
        /// <summary>
        /// MatchingRatio
        /// </summary>
        [DataMember]
        public string MatchingRatio { get; set; }
        /// <summary>
        /// MatchingCalipers
        /// </summary>
        [DataMember]
        public string MatchingCalipers { get; set; }
        /// <summary>
        /// VaryMatchingRatio
        /// </summary>
        [DataMember]
        public string VaryMatchingRatio { get; set; }
        /// <summary>
        /// VaryMatchingCalipers
        /// </summary>
        [DataMember]
        public string VaryMatchingCalipers { get; set; }
       

        #endregion

    }
    /// <summary>
    /// Outcome item
    /// </summary>
    [DataContract]
    public class OutcomeItemDTO
    {
        /// <summary>
        /// Common name
        /// </summary>
        [DataMember]
        public string CommonName { get; set; }
        /// <summary>
        /// Outcome
        /// </summary>
        [DataMember]
        public string Outcome { get; set; }
        /// <summary>
        /// Washout period
        /// </summary>
        [DataMember]
        public string WashoutPeriod { get; set; }
        /// <summary>
        /// Vary washout period
        /// </summary>
        [DataMember]
        public string VaryWashoutPeriod { get; set; }
        

    }
    /// <summary>
    /// Covariate item DTO
    /// </summary>
    [DataContract]
    public class CovariateItemDTO
    {
        /// <summary>
        /// Grouping indicator
        /// </summary>
        [DataMember]
        public string GroupingIndicator { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Code type
        /// </summary>
        [DataMember]
        public string CodeType { get; set; }
        /// <summary>
        /// Ingredients
        /// </summary>
        [DataMember]
        public string Ingredients { get; set; }
        /// <summary>
        /// Sub Group Analysis
        /// </summary>
        [DataMember]
        public string SubGroupAnalysis { get; set; }
    }

}
