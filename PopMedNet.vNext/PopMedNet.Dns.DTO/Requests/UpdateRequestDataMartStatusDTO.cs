using PopMedNet.Dns.DTO.Enums;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{

    /// <summary>
    /// Update Request DataMart Status DTO
    /// </summary>
    [DataContract]
    public class UpdateRequestDataMartStatusDTO
    {
        ///<summary>
        ///Request DataMart ID
        ///</summary>
        [DataMember]
        public Guid RequestDataMartID { get; set;}

        ///<summary>
        ///DataMart ID
        ///</summary>
        [DataMember]
        public Guid DataMartID { get; set; }

        ///<summary>
        ///New Routing Status
        ///</summary>
        [DataMember]
        public RoutingStatus NewStatus { get; set; }

        ///<summary>
        ///Message
        ///</summary>
        [DataMember]
        public string Message { get; set; }
    }
}
