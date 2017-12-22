using System.Runtime.Serialization;

namespace RequestCriteria.Models
{
    [DataContract]
    public class ProjectData : TermData
    {
        [DataMember]
        public string Project { get; set; }
    }
}
