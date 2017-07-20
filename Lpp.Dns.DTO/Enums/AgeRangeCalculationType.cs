using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// Defines how the age range should be used in the query.
    /// </summary>
    [DataContract]
    public enum AgeRangeCalculationType
    {
        /// <summary>
        /// At first encounter that meets the criteria in this criteria group 
        /// </summary>
        [EnumMember, Description("At first encounter that meets the criteria in this criteria group ")]
        AtFirstMatchingEncounterWithinCriteriaGroup = 1,
        /// <summary>
        /// At last encounter that meets the criteria in this criteria group 
        /// </summary>
        [EnumMember, Description("At last encounter that meets the criteria in this criteria group ")]
        AtLastMatchingEncounterWithinCriteriaGroup = 2,
        /// <summary>
        /// At last encounter of any kind in the health system
        /// </summary>
        [EnumMember, Description("At last encounter of any kind in the health system")]
        AtLastEncounterWithinHealthSystem = 3,
        /// <summary>
        /// As of observation period start date for this criteria group 
        /// </summary>
        [EnumMember, Description("As of observation period start date for this criteria group ")]
        AsOfObservationPeriodStartDateWithinCriteriaGroup = 4,
        /// <summary>
        /// As of observation period end date for this criteria group 
        /// </summary>
        [EnumMember, Description("As of observation period end date for this criteria group ")]
        AsOfObservationPeriodEndDateWithinCriteriaGroup = 5,
        /// <summary>
        /// As of the date of the request submission, ie current time when query is being run.
        /// </summary>
        [EnumMember, Description("As of the date of the request submission")]
        AsOfDateOfRequestSubmission = 6,
        /// <summary>
        /// As of [select date]* 
        /// </summary>
        [EnumMember, Description("As of [select date]* ")]
        AsOfSpecifiedDate = 7
    }
}
