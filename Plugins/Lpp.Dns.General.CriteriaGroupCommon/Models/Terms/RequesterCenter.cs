using System;
using System.Runtime.Serialization;

namespace RequestCriteria.Models
{
    [DataContract]
    public class RequesterCenterData : TermData
    {
        [DataMember]
        public Guid RequesterCenterID { get; set; }
    }
}
