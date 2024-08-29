using System.ComponentModel;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Project DataMarts
    /// </summary>
    [DataContract]
    public class ProjectDataMartDTO
    {
        /// <summary>
        /// Gets or sets the id of project
        /// </summary>
        [DataMember]
        public Guid ProjectID { get; set; }
        /// <summary>
        /// Gets the Name of the Project
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string Project { get; set; }
        /// <summary>
        /// Gets the Acronym of the Project
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string ProjectAcronym { get; set; }
        /// <summary>
        /// Gets or sets the id of DataMart
        /// </summary>
         [DataMember]
        public Guid DataMartID { get; set; }
        /// <summary>
         /// Gets the DataMart
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string DataMart { get; set; }
        /// <summary>
        /// Gets the Organization
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string Organization { get; set; }
    }
}
