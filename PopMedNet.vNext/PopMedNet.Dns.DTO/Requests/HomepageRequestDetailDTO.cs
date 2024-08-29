using PopMedNet.Objects;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// A condensed version of Request information.
    /// </summary>
    [DataContract]
    public class HomepageRequestDetailDTO : EntityDtoWithID
    {
        /// <summary>
        /// Gets or sets the name of the Request.
        /// </summary>
        [DataMember]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the Identifier (System Number) of the Request.
        /// </summary>
        [DataMember, ReadOnly(true)]
        public long Identifier { get; set; }
        /// <summary>
        /// Gets or sets the date the Request was submitted on.
        /// </summary>
        [DataMember]
        public DateTimeOffset? SubmittedOn { get; set; }
        /// <summary>
        /// Gets or sets the username of the user that submitted the Request.
        /// </summary>
        [DataMember]
        public string? SubmittedByName { get; set; }
        /// <summary>
        /// Gets or sets the Username of the submitter.
        /// </summary>
        [DataMember]
        public string? SubmittedBy { get; set; }
        /// <summary>
        /// Gets or sets the ID of the user that submitted the Request.
        /// </summary>
        [DataMember]
        public Guid? SubmittedByID { get; set; }
        /// <summary>
        /// Gets or sets the display text for the status of the Request.
        /// </summary>
        [DataMember]
        public string StatusText { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the status of the Request.
        /// </summary>
        [DataMember]
        public DTO.Enums.RequestStatuses Status { get; set; }
        /// <summary>
        /// Gets or sets the name of the RequestType.
        /// </summary>
        [DataMember]
        public string RequestType { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the name of the Project.
        /// </summary>
        [DataMember]
        public string Project { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the Priority of the Request.
        /// </summary>
        [DataMember]
        public DTO.Enums.Priorities Priority { get; set; }
        /// <summary>
        /// Gets or sets the Due Date of the Request.
        /// </summary>
        [DataMember]
        public DateTimeOffset? DueDate { get; set; }
        /// <summary>
        /// Gets or sets the Mini-Sentiel Request ID for the Request.
        /// </summary>
        [DataMember]
        public string? MSRequestID { get; set; }
        /// <summary>
        /// Gets or sets if the Request uses workflow.
        /// </summary>
        [DataMember]
        public bool IsWorkflowRequest { get; set; }
        /// <summary>
        /// Gets or sets if the user can edit the request's metadata.
        /// </summary>
        [DataMember]
        public bool CanEditMetadata { get; set; }
    }
}
