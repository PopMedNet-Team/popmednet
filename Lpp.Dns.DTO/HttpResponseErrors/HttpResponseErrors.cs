using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
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
        public IEnumerable<string> Errors { get; set; }
    }
}
