using PopMedNet.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Workflow.Engine.Database
{
    public interface IDbWorkflowActivityResult : IWorkflowActivityResult
    {
        string Name { get; set; }
        string Description { get; set; }
    }
}
