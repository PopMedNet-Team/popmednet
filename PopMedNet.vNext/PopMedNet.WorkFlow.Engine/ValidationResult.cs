using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Workflow.Engine
{
    [DataContract]
    public class ValidationResult
    {
        [DataMember]
        public bool Success { get; set; }
        [DataMember]
        public string Errors { get; set; }

        //[DataMember]
        //public IEnumerable<Error>
    }
}
