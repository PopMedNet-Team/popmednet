using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
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