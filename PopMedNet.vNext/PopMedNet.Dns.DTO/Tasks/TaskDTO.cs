using PopMedNet.Dns.DTO.Enums;
using PopMedNet.Objects;
using PopMedNet.Objects.ValidationAttributes;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
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
        public string Subject { get; set; } = string.Empty;
        /// <summary>
        /// Task Location
        /// </summary>
        [DataMember]
        [MaxLength(255)]
        public string? Location { get; set; }
        /// <summary>
        /// Body
        /// </summary>
        [DataMember]
        public string? Body { get; set; }
        /// <summary>
        /// Gets or sets the due date
        /// </summary>
        [DataMember]
        public DateTimeOffset? DueDate { get; set; }
        /// <summary>
        /// Gets or sets the date the task was created on
        /// </summary>
        [DataMember]
        public DateTimeOffset CreatedOn { get; set; } = DateTime.UtcNow;
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
        public Priorities Priority { get; set; } = Priorities.Medium;
        /// <summary>
        /// Gets or sets the Task status
        /// </summary>
        [DataMember]
        public TaskStatuses Status { get; set; } = TaskStatuses.NotStarted;
        /// <summary>
        /// Gets or sets the type of task
        /// </summary>
        [DataMember]
        public TaskTypes Type { get; set; } = TaskTypes.Task;
        /// <summary>
        /// complete precentage
        /// </summary>
        [DataMember]
        public double PercentComplete { get; set; } = 0d;
        /// <summary>
        /// Gets or sets the id of workflow activity
        /// </summary>
        [DataMember]
        public Guid? WorkflowActivityID { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if should go directly to request detail
        /// </summary>
        [DataMember]
        public bool DirectToRequest { get; set; } = false;

    }
}
