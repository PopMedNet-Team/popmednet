using Lpp.Dns.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Lookup list for Detailed Request
    /// </summary>
    [DataContract]
    public class LookupListDetailRequestDTO
    {
        /// <summary>
        /// Available codes
        /// </summary>
        [DataMember]
        public IEnumerable<string> Codes { get; set; }
        /// <summary>
        /// List of id's
        /// </summary>
        [DataMember]
        public Lists ListID { get; set; }
    }}
