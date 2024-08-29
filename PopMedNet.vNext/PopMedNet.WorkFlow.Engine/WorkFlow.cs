using PopMedNet.Objects;
using PopMedNet.Utilities;
using PopMedNet.Utilities.Security;
using PopMedNet.Workflow.Engine.Database;
using PopMedNet.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Workflow.Engine
{
    public class Workflow<TDataContext, TEntity>
        where TEntity : class, IWorkflowEntity, IEntityWithID
        where TDataContext: DbContext, IWorkflowDataContext

    {
        IActivity<TDataContext, TEntity> currentActivity;
        bool _isNew = false;
        public readonly Guid ID;
        public readonly TDataContext DataContext;
        public readonly TEntity Entity;
        public readonly ApiIdentity Identity;

        static readonly IDictionary<Guid, IWorkflowConfiguration> WorkflowConfigurations = null;

        static Workflow()
        {
            var assemblies = ObjectEx.GetNonSystemAssemblies();
            WorkflowConfigurations = assemblies.SelectMany(a => a.GetTypes().Where(type => typeof(IWorkflowConfiguration).IsAssignableFrom(type) && type != typeof(IWorkflowConfiguration)).Select(t => (IWorkflowConfiguration)Activator.CreateInstance(t))).ToDictionary<IWorkflowConfiguration, Guid>(k => k.ID);
        }

        public readonly IWorkflowConfiguration CurrentWorkflowConfiguration;

        public Workflow(TDataContext dbContext, TEntity entity, ApiIdentity identity, Func<WorkflowActivityContext> getEntryActivity)
        {
            this.DataContext = dbContext;
            this.Entity = entity;
            this.Identity = identity;

            
            //Determine current activity and load it in.
            if (entity.WorkFlowActivityID == null)
            {
                _isNew = true;
                //New, we don't have one, load the entry point.
                var dbActivityContext = getEntryActivity();
                entity.WorkFlowActivityID = dbActivityContext.Activity.ID;
                entity.WorkflowID = dbActivityContext.WorkflowID;

                dbContext.SaveChanges();

                this.ID = dbActivityContext.WorkflowID;
            }
            else
            {
                this.ID = entity.WorkflowID.Value;
            }

            WorkflowConfigurations.TryGetValue(this.ID, out CurrentWorkflowConfiguration);          

            if (!CurrentWorkflowConfiguration.Activities.ContainsKey(entity.WorkFlowActivityID.Value))
                throw new ArgumentOutOfRangeException("The Workflow activity referenced by the entity could not be found.");
        }

        
        public async Task InitializeActivityAsync()
        {
            this.currentActivity = (IActivity<TDataContext, TEntity>)Activator.CreateInstance(CurrentWorkflowConfiguration.Activities[Entity.WorkFlowActivityID.Value]);
            this.currentActivity.Initialize(this);

            if (_isNew)
                await this.currentActivity.Start(string.Empty);
        }

        public Task<ValidationResult> Validate(Guid? demandActivityResultID)
        {
            return this.currentActivity.Validate(demandActivityResultID);
        }

        public async Task<string> Complete(string data, Guid? demandActivityResultID, string comment) {
            var result = await this.currentActivity.Complete(data, demandActivityResultID);

            if (result == null)
            {
                //can't do this, need to be able to end a workflow, and if you specify the complete activity it will error if it tries to load that activity
                //Entity.WorkFlowActivityID = null;
                //await DataContext.SaveChangesAsync();

                return null;
            }
            else
            {
                //Find the activity that should be used based on the current activity and the result
                var activity = await DataContext.GetWorkflowActivityFromCompletion(this.ID, this.currentActivity.ID, result.ResultID);

                //Load activity
                if(!CurrentWorkflowConfiguration.Activities.ContainsKey(activity.ID))
                    throw new ArgumentOutOfRangeException("The Workflow activity referenced by the entity could not be found.");

                await DataContext.Entry(Entity).ReloadAsync();
                Entity.WorkFlowActivityID = activity.ID;
                await DataContext.SaveChangesAsync();

                this.currentActivity = (IActivity<TDataContext, TEntity>)Activator.CreateInstance(CurrentWorkflowConfiguration.Activities[Entity.WorkFlowActivityID.Value]);
                this.currentActivity.Initialize(this);

                await this.currentActivity.Start(comment);
                
                //Return routing info
                return this.currentActivity.Uri;
            }
        }
    }

    //Have to create a IRouter for mvc side that takes workflow step, mapped to activity and displays the activity's screen
}
