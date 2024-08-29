using System;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// DTO for Viewing the availablity periods of DataMarts.
    /// </summary>
    [DataContract]
    public class DataMartAvailabilityPeriodV2DTO
    {
        /// <summary>
        /// Gets or sets the Identifier of the DataMart.
        /// </summary>
        [DataMember]
        public Guid DataMartID { get; set; }
        /// <summary>
        /// Gets or sets the Name of the DataMart.
        /// </summary>
        [DataMember]
        public string DataMart { get; set; }
        /// <summary>
        /// Gets or sets the DataTable the Period Came from.
        /// </summary>
        [DataMember]
        public string DataTable { get; set; }
        /// <summary>
        /// Gets or sets the type of Period.  
        /// Y = Yearly.
        /// Q = Quarterly.
        /// </summary>
        [DataMember]
        public string PeriodCategory { get; set; }
        /// <summary>
        /// Gets or sets the raw data outputed from the DataTable.
        /// </summary>
        [DataMember]
        public string Period { get; set; }
        /// <summary>
        /// Gets or sets the Year of the Period.
        /// </summary>
        [DataMember]
        public int Year { get; set; }
        /// <summary>
        /// Gets or sets the Quarter of the Period if PeriodCategory = Q
        /// </summary>
        [DataMember]
        public int? Quarter { get; set; }
    }
}
