using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RequestCriteria.Models
{
    [DataContract]
    public class AgeRangeData : TermData
    {
        [DataMember, Range(0, 120)]
        public int? MinAge { get; set; }
        [DataMember, Range(0, 120)]
        public int? MaxAge { get; set; }
    }
}
