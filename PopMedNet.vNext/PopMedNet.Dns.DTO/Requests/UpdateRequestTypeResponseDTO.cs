using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Container class containing the entities updated by a request type save.
    /// </summary>
    [DataContract]
    public class UpdateRequestTypeResponseDTO
    {
        /// <summary>
        /// Gets or set the request type saved.
        /// </summary>
        [DataMember]
        public RequestTypeDTO RequestType { get; set; }

        /// <summary>
        /// Gets or sets the template associated with the request type.
        /// </summary>
        [DataMember]
        public IEnumerable<TemplateDTO> Queries { get; set; }
    }
}
