using Lpp.Objects;
using Lpp.Utilities.Security;
using Lpp.Workflow.Engine;
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
    public interface IActivity<TDataContext, TEntity>
        where TEntity : class, IWorkflowEntity, IEntityWithID
        where TDataContext: DbContext, IWorkflowDataContext
    {
        void Initialize(Workflow<TDataContext, TEntity> workflow);

        Guid ID { get; }
        string Uri { get; }

        Task<ValidationResult> Validate(Guid? activityResultID);

        Task<CompletionResult> Complete(string data, Guid? activityResultID);

        Task Start(string comment);
    }
}
