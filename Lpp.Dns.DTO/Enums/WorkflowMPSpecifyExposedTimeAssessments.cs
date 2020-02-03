using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// Workflow MP specify Exposed Time Assessments
    /// </summary>
    [DataContract]
    public enum WorkflowMPSpecifyExposedTimeAssessments
    {
        /// <summary>
        /// Create Treatment Episodes
        /// </summary>
        [EnumMember, Description("Create Treatment Episodes")]
        CreateTreatmentEpisodes = 1,
        /// <summary>
        /// Define Number of Days
        /// </summary>
        [EnumMember, Description("Define Number of Days")]
        DefineNumberOfDays = 2
    }
}
