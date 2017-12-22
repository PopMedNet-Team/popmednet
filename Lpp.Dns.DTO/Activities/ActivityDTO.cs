using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Activities
    /// </summary>
    [DataContract]
    public class ActivityDTO
    {
        /// <summary>
        /// Identifier of Activity
        /// </summary>
        [DataMember]
        public Guid ID { get; set; }
        /// <summary>
        /// Name of the Activity
        /// </summary>
        [DataMember]
        public string Name { get; set; } 
        /// <summary>
        /// Available Activities
        /// </summary>
        [DataMember]
        public IEnumerable<ActivityDTO> Activities { get; set; }
        /// <summary>
        /// Description of the Activity
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Project the Activity is tied to
        /// </summary>
        [DataMember]
        public Guid? ProjectID { get; set; }
        /// <summary>
        /// Display order of the Activity
        /// </summary>
        [DataMember]
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Task level of Activity
        /// </summary>
        [DataMember]
        public int TaskLevel { get; set; }
        /// <summary>
        /// Activity of the Parent Activity
        /// </summary>
        [DataMember]
        public Guid? ParentActivityID { get; set; }
        /// <summary>
        /// Gets or set the acronym for the activity.
        /// </summary>
        [DataMember]
        public string Acronym { get; set; }
        /// <summary>
        /// Gets or sets if the activity has been soft deleted.
        /// </summary>
        [DataMember]
        public bool Deleted { get; set; }
    }
}
