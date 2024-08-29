using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RequestCriteria.Models
{
    [DataContract]
    public class MetaDataTableData : TermData
    {
        [DataMember]
        public MetaDataTableTypes[] Tables { get; set; }
    }

    [DataContract]
    public enum MetaDataTableTypes
    {
        [EnumMember]
        Diagnosis = 0,
        [EnumMember]
        Dispensing = 1,
        [EnumMember]
        Encounter = 2,
        [EnumMember]
        Enrollment = 3,
        [EnumMember]
        Procedure = 4,
    }
}
