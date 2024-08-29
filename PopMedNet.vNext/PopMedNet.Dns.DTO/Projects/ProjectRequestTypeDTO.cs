using PopMedNet.Objects;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Request types in project
    /// </summary>
    [DataContract]
    public class ProjectRequestTypeDTO : EntityDto
    {
        /// <summary>
        /// Gets or set the ID of project
        /// </summary>
        [DataMember]
        public Guid ProjectID { get; set; }
        /// <summary>
        /// Gets or set the ID of Request type
        /// </summary>
        [DataMember]
        public Guid RequestTypeID { get; set; }
        /// <summary>
        /// Gets or sets the Request type
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string? RequestType { get; set; }
        /// <summary>
        /// Gets or set the ID of workflow
        /// </summary>
        [DataMember, ReadOnly(true)]
        public Guid? WorkflowID { get; set; }
        /// <summary>
        /// Gets or set the workflow
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string? Workflow { get; set; }
        ///// <summary>
        ///// Gets or set the template
        ///// </summary>
        //[DataMember, ReadOnly(true)]
        //public string Template { get; set; }
    }
}
