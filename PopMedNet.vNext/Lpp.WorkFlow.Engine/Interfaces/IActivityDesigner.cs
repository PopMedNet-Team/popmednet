using Lpp.Workflow.Engine.Database;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Workflow.Engine.Interfaces
{
    public interface IActivityDesigner<TDataContext, T>
        where T : IWorkflowEntity
        where TDataContext : DbContext, IWorkflowDataContext
    {

        IEnumerable<IWorkflowActivityResult> Accepted { get; set; }

        IEnumerable<IWorkflowActivityResult> PossibleResults { get; set; }
    }
}
