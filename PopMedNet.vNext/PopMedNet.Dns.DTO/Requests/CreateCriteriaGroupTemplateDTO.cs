using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// DTO for initiating a save of a new criteria group.
    /// </summary>
    [DataContract]
    public class CreateCriteriaGroupTemplateDTO
    {
        /// <summary>
        /// Gets or sets the name of the template.
        /// </summary>
        [DataMember, PopMedNet.Objects.ValidationAttributes.Required]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the description of the template.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the criteria group json to save.
        /// </summary>
        [DataMember, PopMedNet.Objects.ValidationAttributes.Required]
        public string Json { get; set; }
        /// <summary>
        /// Gets or sets the adapter detail that is applicable to the template.
        /// </summary>
        [DataMember]
        public Enums.QueryComposerQueryTypes? AdapterDetail { get; set; }
        ///// <summary>
        ///// Gets or sets the template ID of the parent template the criteria group is being created from.
        ///// </summary>
        //[DataMember]
        //public Guid? TemplateID { get; set; }
        ///// <summary>
        ///// Gets or sets the request type of the parent template the criteria group is being created from.
        ///// </summary>
        //[DataMember]
        //public Guid? RequestTypeID { get; set; }
        ///// <summary>
        ///// Gets or sets the request to determine the parent template the criteria group is being created from.
        ///// </summary>
        //[DataMember]
        //public Guid? RequestID { get; set; }
    }
}
