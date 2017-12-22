using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RequestCriteria.Models
{
    [DataContract]
    public class CriteriaData
    {
        [DataMember]
        //[Required, MaxLength(50)]
        public string Name {get; set;}
        [DataMember]
        public bool IsExclusion {get; set;}
        [DataMember]
        public bool IsPrimary {get; set;}
        [DataMember]
        [Required]
        public IEnumerable<ITermData> Terms { get; set; }
    }
}
