using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PopMedNet.Dns.DTO.Enums
{
    /// <summary>
    /// Workflow MP Allow on or Multiple Exposure episodes
    /// </summary>
    [DataContract]
    public enum WorkflowMPAllowOnOrMultipleExposureEpisodes
    {
        /// <summary>
        /// One
        /// </summary>
        [EnumMember]
        One = 1,
        /// <summary>
        /// All
        /// </summary>
        [EnumMember]
        All = 2,
        /// <summary>
        /// All Until An Outcome is Observed
        /// </summary>
        [EnumMember, Description("All Until An Outcome is Observed")]
        AllUntilAnOutcomeObserved = 3
    }
}
