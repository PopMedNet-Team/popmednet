using PopMedNet.Workflow.Engine.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Workflow.Engine
{
    public class WorkflowActivityContext
    {
        public Guid WorkflowID { get; set; }
        public IDbWorkflowActivity Activity { get; set; }
    }
}
