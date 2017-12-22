using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Task Summary in Home page
    /// </summary>
    [DataContract]
    public class HomepageTaskSummaryDTO
    {
        /// <summary>
        /// Gets or set the ID of task.
        /// </summary>
        [DataMember]
        public Guid TaskID { get; set; }
        /// <summary>
        /// Gets or sets the name of the task.
        /// </summary>
        [DataMember]
        public string TaskName { get; set; }
        /// <summary>
        /// Gets or set the status of the task.
        /// </summary>
        [DataMember]
        public Enums.TaskStatuses TaskStatus { get; set; }
        /// <summary>
        /// Gets or sets the status of the task as text.
        /// </summary>
        [DataMember]
        public string TaskStatusText { get; set; }
        /// <summary>
        /// Gets or sets the date the task was created on.
        /// </summary>
        [DataMember]
        public DateTimeOffset? CreatedOn { get; set; }
        /// <summary>
        /// Gets or sets the date the task was started on.
        /// </summary>
        [DataMember]
        public DateTimeOffset? StartOn { get; set; }
        /// <summary>
        /// Gets or sets the date the task ended on.
        /// </summary>
        [DataMember]
        public DateTimeOffset? EndOn { get; set; }
        /// <summary>
        /// Gets or sets the name of the workflow associated with the task.
        /// </summary>
        [DataMember]
        public string Type { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if should go directly to request details instead of task details.
        /// </summary>
        [DataMember]
        public bool DirectToRequest { get; set; }

        /// <summary>
        /// The name of the request or new user.
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Gets or set the request identifier if available, else the new user's username.
        /// </summary>
        [DataMember]
        public string Identifier { get; set; }
        /// <summary>
        /// Gets or sets the ID of the request if applicable.
        /// </summary>
        [DataMember]
        public Guid? RequestID { get; set; }
        /// <summary>
        /// Gets or sets the MS Request ID of the request if applicable.
        /// </summary>
        [DataMember]
        public string MSRequestID { get; set; }
        /// <summary>
        /// Gets or set the status of the request if applicable.
        /// </summary>
        [DataMember]
        public Enums.RequestStatuses? RequestStatus { get; set; }
        /// <summary>
        /// Gets or sets the request status description.
        /// </summary>
        [DataMember]
        public string RequestStatusText { get; set; }
        /// <summary>
        /// Gets or sets the ID of the new user.
        /// </summary>
        [DataMember]
        public Guid? NewUserID { get; set; }
        /// <summary>
        /// Gets or sets a the description of the resources assigned to the task. (From the server this will always be string.Empty).
        /// </summary>
        [DataMember]
        public string AssignedResources { get; set; }
    }
}
