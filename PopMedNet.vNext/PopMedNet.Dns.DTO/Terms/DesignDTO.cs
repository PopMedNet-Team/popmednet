using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Holds key design elements of a term
    /// </summary>
    [DataContract]
    public class DesignDTO
    {
        /// <summary>
        /// Whether the term can be removed from a request. Set in the template/request type edit page
        /// </summary>
        [DataMember]
        public bool Locked { get; set; }
    }
}