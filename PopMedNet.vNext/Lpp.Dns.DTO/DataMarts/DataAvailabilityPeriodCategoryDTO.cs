using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Data Availabile Period Category
    /// </summary>
    [DataContract]
    public class DataAvailabilityPeriodCategoryDTO
    {
        /// <summary>
        /// Category Type
        /// </summary>
        [DataMember]
        public string CategoryType { get; set; }
        /// <summary>
        /// Category Description
        /// </summary>
        [DataMember]
        public string CategoryDescription { get; set; }
        /// <summary>
        /// Determines that the Data Available period category is published or not
        /// </summary>
        [DataMember]
        public bool Published { get; set; }
        /// <summary>
        /// The DataMart Description
        /// </summary>
        [DataMember]
        public string DataMartDescription { get; set; }
    }
}
