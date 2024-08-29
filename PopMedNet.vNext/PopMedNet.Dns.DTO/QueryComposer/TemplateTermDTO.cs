using PopMedNet.Dns.DTO.Enums;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.QueryComposer
{
    /// <summary>
    /// A DTO for a term that may not be allowed within a template
    /// </summary>
    [DataContract]
    public class TemplateTermDTO 
    {
        /// <summary>
        /// Gets or Sets The ID of the template the term belongs to
        /// </summary>
        [DataMember]
        public Guid TemplateID { get; set; }

        /// <summary>
        /// Gets or Sets The template the term belongs to
        /// </summary>
        [DataMember]
        public virtual TemplateDTO Template { get; set; }

        /// <summary>
        /// Gets or Sets The ID of the Term
        /// </summary>
        [DataMember]
        public Guid TermID { get; set; }

        /// <summary>
        /// Gets or Sets The DTO of the term 
        /// </summary>
        [DataMember]
        public virtual TermDTO Term { get; set; }

        /// <summary>
        /// Gets or Sets Whether the term is allowed under the rules of the template
        /// </summary>
        [DataMember]
        public bool Allowed { get; set; }

        /// <summary>
        /// Gets or Sets Whether the term belongs to the criteria (0) or stratification (1) section
        /// </summary>
        [DataMember]
        public QueryComposerSections Section { get; set; }
    }
}
