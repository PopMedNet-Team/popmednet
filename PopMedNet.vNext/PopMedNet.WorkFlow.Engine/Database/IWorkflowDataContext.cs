using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Workflow.Engine.Database
{
    public interface IWorkflowDataContext
    {
        Task<IDbWorkflowActivity> GetWorkflowActivityFromCompletion(Guid workflowID, Guid sourceWorkflowActivityID, Guid resultID);

        IDbWorkflowActivity GetWorkflowActivityByID(Guid workflowActivityID);
    }
}
