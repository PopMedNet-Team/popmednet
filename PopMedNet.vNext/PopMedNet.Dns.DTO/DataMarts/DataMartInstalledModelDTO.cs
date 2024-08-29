using System.ComponentModel;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Installed Models for DataMart
    /// </summary>
    [DataContract]
    public class DataMartInstalledModelDTO
    {
        /// <summary>
        /// Identifier of DataMart
        /// </summary>
        [DataMember]
        public Guid DataMartID { get; set; }
        /// <summary>
        /// Identifier of Model
        /// </summary>
        [DataMember]
        public Guid ModelID { get; set; }
        /// <summary>
        /// Model name
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string? Model { get; set; }
        /// <summary>
        /// Properties
        /// </summary>
        [DataMember]
        public string? Properties { get; set; }
    }
}
