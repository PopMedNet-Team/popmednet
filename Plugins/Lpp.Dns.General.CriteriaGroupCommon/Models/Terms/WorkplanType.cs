using System;
using System.Runtime.Serialization;

namespace RequestCriteria.Models
{
    [DataContract]
    public class WorkplanTypeData : TermData
    {
        [DataMember]
        public Guid WorkplanTypeID { get; set; }
    }
}
