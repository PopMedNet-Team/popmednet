using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Request Type Models
    /// </summary>
    [DataContract]
    public class RequestTypeModelDTO
    {
        /// <summary>
        /// The ID of the Request Type
        /// </summary>
        [DataMember]
        public Guid RequestTypeID { get; set; }
        /// <summary>
        /// The ID of the Model
        /// </summary>
        [DataMember]
        public Guid DataModelID { get; set; }

    }
}
