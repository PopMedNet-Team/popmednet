using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Project DataMart Update
    /// </summary>
    [DataContract]
    public class ProjectDataMartUpdateDTO
    {
        /// <summary>
        /// Gets or sets the id of the project
        /// </summary>
        [DataMember]
        public Guid ProjectID { get; set; }
        /// <summary>
        /// Available DataMarts in project
        /// </summary>
        [DataMember]
        public IEnumerable<ProjectDataMartDTO> DataMarts { get; set; }
    }
}
