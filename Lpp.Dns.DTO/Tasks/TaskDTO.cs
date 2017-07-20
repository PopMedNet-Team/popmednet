using Lpp.Dns.DTO.Enums;
using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects.ValidationAttributes;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Task
    /// </summary>
    [DataContract]
    public class TaskDTO : EntityDtoWithID
    {
        /// <summary>
        /// Task Subject
        /// </summary>
        [DataMember]
        [Required, MaxLength(255)]
        public string Subject { get; set; }
        /// <summary>
        /// Task Location
        /// </summary>
        [DataMember]
        [MaxLength(255)]
        public string Location { get; set; }
        /// <summary>
        /// Body
        /// </summary>
        [DataMember]
        public string Body { get; set; }
        /// <summary>
        /// Gets or sets the due date
        /// </summary>
        [DataMember]
        public DateTimeOffset? DueDate { get; set; }
        /// <summary>
        /// Gets or sets the date the task was created on
        /// </summary>
        [DataMember]
        public DateTimeOffset CreatedOn { get; set; }
        /// <summary>
        /// Gets or sets the date the task was started on
        /// </summary>
        [DataMember]
        public DateTimeOffset? StartOn { get; set; }
        /// <summary>
        /// Gets or sets the date the task was ended on
        /// </summary>
        [DataMember]
        public DateTimeOffset? EndOn { get; set; }
        /// <summary>
        /// Gets or sets the estimated completed date
        /// </summary>
        [DataMember]
        public DateTimeOffset? EstimatedCompletedOn { get; set; }
        /// <summary>
        /// Gets or sets the priorities
        /// </summary>
        [DataMember]
        public Priorities Priority { get; set; }
        /// <summary>
        /// Gets or sets the Task status
        /// </summary>
        [DataMember]
        public TaskStatuses Status { get; set; }
        /// <summary>
        /// Gets or sets the type of task
        /// </summary>
        [DataMember]
        public TaskTypes Type { get; set; }
        /// <summary>
        /// complete precentage
        /// </summary>
        [DataMember]
        public double PercentComplete { get; set; }
        /// <summary>
        /// Gets or sets the id of workflow activity
        /// </summary>
        [DataMember]
        public Guid? WorkflowActivityID { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if should go directly to request detail
        /// </summary>
        [DataMember]
        public bool DirectToRequest { get; set; }

    }
}
