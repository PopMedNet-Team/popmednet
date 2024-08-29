using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Update DataMart Installed Model
    /// </summary>
    [DataContract]
    public class UpdateDataMartInstalledModelsDTO
    {
        /// <summary>
        /// ID of DataMart
        /// </summary>
        [DataMember]
        public Guid DataMartID { get; set; }
        /// <summary>
        /// Available Models
        /// </summary>
        [DataMember]
        public IEnumerable<DataMartInstalledModelDTO> Models { get; set; }
    }
}
