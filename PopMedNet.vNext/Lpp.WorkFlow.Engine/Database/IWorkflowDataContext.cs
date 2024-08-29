using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Workflow.Engine.Database
{
    public interface IWorkflowDataContext
    {
        Task<IDbWorkflowActivity> GetWorkflowActivityFromCompletion(Guid workflowID, Guid sourceWorkflowActivityID, Guid ResultID);

        IDbWorkflowActivity GetWorkflowActivityByID(Guid workflowActivityID);
    }
}
