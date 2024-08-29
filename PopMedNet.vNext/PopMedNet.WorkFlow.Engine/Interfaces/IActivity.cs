using PopMedNet.Objects;
using PopMedNet.Utilities.Security;
using PopMedNet.Workflow.Engine;
using PopMedNet.Workflow.Engine.Database;
using PopMedNet.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Workflow.Engine.Interfaces
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
