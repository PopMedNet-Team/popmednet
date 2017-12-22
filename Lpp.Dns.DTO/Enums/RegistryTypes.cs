using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// Types of Registry
    /// </summary>
    [DataContract]
    public enum RegistryTypes : byte { 
        /// <summary>
        ///Indicates RegistryType is Registry
        /// </summary>
        [EnumMember]
        Registry = 0,
        /// <summary>
        ///Indicates RegistryType is ResearchDataSet
        /// </summary>
        [EnumMember, Description("Research Data Set")]
        ResearchDataSet = 1 
    }
}
