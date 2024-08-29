using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Project DataMart within Request Type
    /// </summary>
    [DataContract]
    public class ProjectDataMartWithRequestTypesDTO : ProjectDataMartDTO
    {
        /// <summary>
        /// Available Request types
        /// </summary>
        [DataMember]
        public IEnumerable<RequestTypeDTO> RequestTypes { get; set; }
    }
}
