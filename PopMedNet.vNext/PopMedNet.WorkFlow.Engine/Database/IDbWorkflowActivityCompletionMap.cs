using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Workflow.Engine.Database
{
    public interface IDbWorkflowActivityCompletionMap
    {
        Guid WorkflowActivityResultID { get; set; }
        Guid SourceWorkflowActivityID { get; set; }
        Guid DestinationWorkflowActivityID { get; set; }
    }
}
