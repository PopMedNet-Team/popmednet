using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// A DTO for returning Errors when we dont want for an AJAX Fail event to fire
    /// </summary>
    [DataContract]
    public class HttpResponseErrors
    {
        /// <summary>
        /// A String Collection of Errors
        /// </summary>
        [DataMember]
        public IEnumerable<string>? Errors { get; set; }
    }
}
