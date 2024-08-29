using System.Runtime.Serialization;

namespace RequestCriteria.Models
{
    [DataContract]
    public class ActivityData : TermData
    {
        [DataMember]
        public string Activity { get; set; }
    }
}
