using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Workflow.Engine
{
    [DataContract]
    public class CompletionResult
    {
        [DataMember]
        public Guid ResultID { get; set; }
    }
}
