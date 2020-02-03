using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Lpp.Objects;
using Lpp.Dns.DTO.Enums;
using Lpp.Objects.ValidationAttributes;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Request
    /// </summary>
    [DataContract]
    public class RequestMetadataDTO : EntityDtoWithID
    {
        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Due Date
        /// </summary>
        [DataMember]
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Priority
        /// </summary>
        [DataMember]
        public Priorities Priority { get; set; }

        /// <summary>
        /// Purpose of use
        /// </summary>
        [DataMember]
        public string PurposeOfUse { get; set; }

        /// <summary>
        /// PHI Disclosure level
        /// </summary>
        [DataMember]
        public string PhiDisclosureLevel { get; set; }
        
        /// <summary>
        /// Gets or sets the ID of Requester Center
        /// </summary>
        [DataMember]
        public Guid? RequesterCenterID { get; set; }

        /// <summary>
        /// Gets or sets the ID of Activity
        /// </summary>
        [DataMember]
        public Guid? ActivityID { get; set; }

        /// <summary>
        /// Gets or sets the ID of Activity Project
        /// </summary>
        [DataMember]
        public Guid? ActivityProjectID { get; set; }

        /// <summary>
        /// Gets or sets the ID of Task Order
        /// </summary>
        [DataMember]
        public Guid? TaskOrderID { get; set; }

        ///<summary>
        ///Gets or sets the ID of Source Activity
        /// </summary>
        [DataMember]
        public Guid? SourceActivityID { get; set; }

        ///<summary>
        ///Gets or sets the ID of Source Activity Project
        /// </summary>
        [DataMember]
        public Guid? SourceActivityProjectID { get; set; }

        ///<summary>
        ///Gets or sets the ID of Source Task Order
        /// </summary>
        [DataMember]
        public Guid? SourceTaskOrderID { get; set; }

        /// <summary>
        /// Gets or sets the ID of workplan type
        /// </summary>
        [DataMember]
        public Guid? WorkplanTypeID { get; set; }

        ///<summary>
        ///Gets or sets the MS Request ID
        ///</summary>
        [DataMember]
        public string MSRequestID { get; set; }

        ///<summary>
        ///Gets or sets the Report Aggregation Level ID
        /// </summary>
        [DataMember]
        public Guid? ReportAggregationLevelID { get; set; }

        /// <summary>
        /// Indicates if the applicable metadata changes should also be applied to the requests applicable routings.
        /// </summary>
        [DataMember]
        public bool? ApplyChangesToRoutings { get; set; }
    }
}
