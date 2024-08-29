using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// DataMart Available Period
    /// </summary>
    [DataContract]
    public class DataMartAvailabilityPeriodDTO
    {
        /// <summary>
        /// The Id of the DataMart
        /// </summary>
        [DataMember]
        public Guid DataMartID { get; set; }
        /// <summary>
        /// The id of the Request
        /// </summary>
        [DataMember]
        public Guid RequestID { get; set; }
        /// <summary>
        /// The ID of the Request type
        /// </summary>
        [DataMember]
        public Guid RequestTypeID { get; set; }
        /// <summary>
        /// Period
        /// </summary>
        [DataMember]
        public string Period { get; set; }
        /// <summary>
        /// Determines that the DataMart Available Period is Active or not
        /// </summary>
        [DataMember]
        public bool Active { get; set; }
    }
}
