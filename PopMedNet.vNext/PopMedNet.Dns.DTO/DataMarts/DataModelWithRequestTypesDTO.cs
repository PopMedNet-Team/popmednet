using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Data Model with Request Types
    /// </summary>
    [DataContract]
    public class DataModelWithRequestTypesDTO : DataModelDTO
    {
        /// <summary>
        /// Available Request Types
        /// </summary>
        [DataMember]
        public IEnumerable<RequestTypeDTO> RequestTypes { get; set; }
    }
}
