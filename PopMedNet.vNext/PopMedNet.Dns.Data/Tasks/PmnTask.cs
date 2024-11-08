﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Utilities.Objects;
using PopMedNet.Dns.DTO.Enums;
using PopMedNet.Utilities.Security;
using PopMedNet.Dns.DTO.Security;

namespace PopMedNet.Dns.Data
{
    [Table("Tasks")]
    public class PmnTask : EntityWithID
    {
        public PmnTask()
        {
            CreatedOn = DateTime.UtcNow;
            Priority = Priorities.Medium;
            Status = TaskStatuses.NotStarted;
            DirectToRequest = false;
        }

        [Required, MaxLength(255)]
        public string Subject { get; set; } = string.Empty;
        [MaxLength(255)]
        public string? Location { get; set; }
        public string? Body { get; set; }

        public DateTime? DueDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? StartOn { get; set; }
        public DateTime? EndOn { get; set; }
        public DateTime? EstimatedCompletedOn { get; set; }
        public Priorities Priority { get; set; }
        public TaskStatuses Status { get; set; }
        public TaskTypes Type { get; set; }
        public double PercentComplete { get; set; }
        public bool DirectToRequest { get; set; }

        public Guid? WorkflowActivityID { get; set; }
        public virtual WorkflowActivity? WorkflowActivity { get; set; }

        public virtual ICollection<PmnTaskUser> Users { get; set; } = new HashSet<PmnTaskUser>();
        public virtual ICollection<TaskReference> References { get; set; } = new HashSet<TaskReference>();
        public virtual ICollection<Audit.PmnTaskChangeLog> TaskChangedLogs { get; set; } = new HashSet<Audit.PmnTaskChangeLog>();

        /// <summary>
        /// Adds a log item to TaskChangedLogs indicating the task was modified.
        /// </summary>
        /// <param name="identity">The identity to associate with the log item.</param>
        /// <param name="db">The datacontext.</param>
        /// <param name="optionalDescription">An optional description to use instead of the description generated by the ActionLogConfiguration.</param>
        /// <returns></returns>
        public async Task LogAsModifiedAsync(ApiIdentity identity, DataContext db, string optionalDescription = null)
        {
            throw new NotImplementedException();
            //var logger = new ActionLogConfiguration();
            //var logItem = await logger.CreateLogItemAsync(this, EntityState.Modified, identity, db);
            //if (!string.IsNullOrWhiteSpace(optionalDescription))
            //{
            //    logItem.Description = optionalDescription;
            //}

            //this.TaskChangedLogs.Add(logItem);
        }

        /// <summary>
        /// Confirms if there is an active task open for the specified request/workflow activity.
        /// </summary>
        /// <param name="requestID">The ID of the request.</param>
        /// <param name="workflowActivityID">The ID of the workflow activity.</param>
        /// <param name="db">The datacontext.</param>
        /// <returns>True if there is an task that has not been canceled or completed for the request/workflow activity, else false.</returns>
        public static bool HasActiveTaskForRequestActivity(Guid requestID, Guid workflowActivityID, DataContext db)
        {
            return db.ActionReferences.Where(tr => tr.ItemID == requestID
                                                && tr.Type == TaskItemTypes.Request
                                                && tr.Task.WorkflowActivityID == workflowActivityID
                                                && tr.Task.Type == TaskTypes.Task
                                                && tr.Task.Status != TaskStatuses.Cancelled
                                                && tr.Task.Status != TaskStatuses.Complete
                                                && tr.Task.EndOn == null
                                                ).Any();
        }

        /// <summary>
        /// Confirms if there is an active task open for the specified request/workflow activity.
        /// </summary>
        /// <param name="requestID">The ID of the request.</param>
        /// <param name="workflowActivityID">The ID of the workflow activity.</param>
        /// <param name="db">The datacontext.</param>
        /// <returns>True if there is an task that has not been canceled or completed for the request/workflow activity, else false.</returns>
        public static async Task<bool> HasActiveTaskForRequestActivityAsync(Guid requestID, Guid workflowActivityID, DataContext db)
        {
            return await db.ActionReferences.Where(tr => tr.ItemID == requestID
                                                && tr.Type == DTO.Enums.TaskItemTypes.Request
                                                && tr.Task.WorkflowActivityID == workflowActivityID
                                                && tr.Task.Type == DTO.Enums.TaskTypes.Task
                                                && tr.Task.Status != TaskStatuses.Cancelled
                                                && tr.Task.Status != TaskStatuses.Complete
                                                && tr.Task.EndOn == null
                                                ).AnyAsync();
        }

        /// <summary>
        /// Gets the first active task for the specified request/workflow activity.
        /// </summary>
        /// <param name="requestID">The ID of the request.</param>
        /// <param name="workflowActivityID">The ID of the workflow activity.</param>
        /// <param name="db">The datacontext.</param>
        /// <returns>The first task found that is not canceled or completed for the specified request/workflow activity, else null if none found.</returns>
        public static PmnTask GetActiveTaskForRequestActivity(Guid requestID, Guid workflowActivityID, DataContext db)
        {
            var task = db.ActionReferences.Where(tr => tr.ItemID == requestID
                                                    && tr.Type == DTO.Enums.TaskItemTypes.Request
                                                    && tr.Task.WorkflowActivityID == workflowActivityID
                                                    && tr.Task.Type == DTO.Enums.TaskTypes.Task
                                                    && tr.Task.Status != TaskStatuses.Cancelled
                                                    && tr.Task.Status != TaskStatuses.Complete
                                                    && tr.Task.EndOn == null
                                                    )
                                                    .Select(tr => tr.Task).FirstOrDefault();
            return task;
        }

        /// <summary>
        /// Gets the ID of the first active task for the specified request/workflow activity.
        /// </summary>
        /// <param name="requestID">The ID of the request.</param>
        /// <param name="workflowActivityID">The ID of the workflow activity.</param>
        /// <param name="db">The datacontext.</param>
        /// <returns></returns>
        public static Guid? GetActiveTaskIDForRequestActivity(Guid requestID, Guid? workflowActivityID, DataContext db)
        {
            if (!workflowActivityID.HasValue)
            {
                return null;
            }

            var taskID = db.ActionReferences.Where(tr => tr.ItemID == requestID
                                                    && tr.Type == DTO.Enums.TaskItemTypes.Request
                                                    && tr.Task.WorkflowActivityID == workflowActivityID
                                                    && tr.Task.Type == DTO.Enums.TaskTypes.Task
                                                    && tr.Task.Status != TaskStatuses.Cancelled
                                                    && tr.Task.Status != TaskStatuses.Complete
                                                    && tr.Task.EndOn == null
                                                    )
                                                    .Select(tr => tr.TaskID).FirstOrDefault();

            if (taskID == Guid.Empty)
                return null;

            return taskID;
        }

        /// <summary>
        /// Gets the first active task for the specified request/workflow activity.
        /// </summary>
        /// <param name="requestID">The ID of the request.</param>
        /// <param name="workflowActivityID">The ID of the workflow activity.</param>
        /// <param name="db">The datacontext.</param>
        /// <returns>The first task found that is not canceled or completed for the specified request/workflow activity, else null if none found.</returns>
        public static async Task<PmnTask> GetActiveTaskForRequestActivityAsync(Guid requestID, Guid workflowActivityID, DataContext db)
        {
            var task = await db.ActionReferences.Where(tr => tr.ItemID == requestID
                                                    && tr.Type == DTO.Enums.TaskItemTypes.Request
                                                    && tr.Task.WorkflowActivityID == workflowActivityID
                                                    && tr.Task.Type == DTO.Enums.TaskTypes.Task
                                                    && tr.Task.Status != TaskStatuses.Cancelled
                                                    && tr.Task.Status != TaskStatuses.Complete
                                                    && tr.Task.EndOn == null
                                                    )
                                                    .Select(tr => tr.Task).FirstOrDefaultAsync();
            return task;
        }

        /// <summary>
        /// Creates a new task for the specified request/workflow activity/workflow. The Request.Users that have view permissions for the task will automatically be added to the Task.Users.
        /// </summary>
        /// <remarks>Note: the new task is not added to the datacontext.</remarks>
        /// <param name="requestID">The ID of the request.</param>
        /// <param name="workflowActivityID">The ID of the workflow activity.</param>
        /// <param name="workflowID">The ID of the workflow.</param>
        /// <param name="db">The datacontext.</param>
        /// <returns></returns>
        public static PmnTask CreateForWorkflowActivity(Guid requestID, Guid workflowActivityID, Guid workflowID, DataContext db)
        {
            return CreateForWorkflowActivity(requestID, workflowActivityID, workflowID, db, string.Empty);
        }

        /// <summary>
        /// Creates a new task for the specified request/workflow activity/workflow. The Request.Users that have view permissions for the task will automatically be added to the Task.Users.
        /// </summary>
        /// <remarks>Note: the new task is not added to the datacontext.</remarks>
        /// <param name="requestID">The ID of the request.</param>
        /// <param name="workflowActivityID">The ID of the workflow activity.</param>
        /// <param name="workflowID">The ID of the workflow.</param>
        /// <param name="db">The datacontext.</param>
        /// <param name="subject">The subject for the new task.</param>
        /// <returns></returns>
        public static PmnTask CreateForWorkflowActivity(Guid requestID, Guid workflowActivityID, Guid workflowID, DataContext db, string subject)
        {
            var task = new PopMedNet.Dns.Data.PmnTask();
            task.Type = DTO.Enums.TaskTypes.Task;
            task.StartOn = task.CreatedOn;
            task.DirectToRequest = true;
            task.Status = DTO.Enums.TaskStatuses.InProgress;
            task.Subject = string.IsNullOrEmpty(subject) ? db.WorkflowActivities.Where(a => a.ID == workflowActivityID).Select(a => a.Name).FirstOrDefault() : subject;
            task.WorkflowActivityID = workflowActivityID;

            task.References.Add(new TaskReference { TaskID = task.ID, ItemID = requestID, Type = DTO.Enums.TaskItemTypes.Request });

            //get the users based on the request.users that have permission to view the task for the workflow activity
            var query = from ru in db.RequestUsers
                        let pq = db.ProjectRequestTypeWorkflowActivities.Where(a => a.SecurityGroup.Users.Any(u => u.UserID == ru.UserID) &&
                                                                                    a.PermissionID == PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ViewTask &&
                                                                                    a.ProjectID == ru.Request.ProjectID &&
                                                                                    a.RequestTypeID == ru.Request.RequestTypeID &&
                                                                                    a.WorkflowActivityID == workflowActivityID)
                        where ru.RequestID == requestID &&
                        pq.Any(a => a.PermissionID == PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ViewTask) &&
                        pq.All(a => a.Allowed)
                        select ru;


            foreach (var userID in query.Select(r => r.UserID).Distinct())
            {
                //NOTE: by default the users will be added to task as worker.
                task.Users.Add(new PmnTaskUser { Role = DTO.Enums.TaskRoles.Worker, UserID = userID });
            }

            return task;
        }

        public static async Task ConfirmUsersToTaskForWorkflowRequest(PmnTask task, Request request, DataContext db)
        {
            //Get all the users that have view task permission but have not been added to the specified task.
            var query = from ru in db.RequestUsers
                        let pq = db.ProjectRequestTypeWorkflowActivities.Where(a => a.SecurityGroup.Users.Any(u => u.UserID == ru.UserID) &&
                                                                                    a.PermissionID == PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ViewTask &&
                                                                                    a.ProjectID == ru.Request.ProjectID &&
                                                                                    a.RequestTypeID == ru.Request.RequestTypeID &&
                                                                                    a.WorkflowActivityID == ru.Request.WorkFlowActivityID)
                        where ru.RequestID == request.ID &&
                        pq.Any(a => a.PermissionID == PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ViewTask) &&
                        pq.All(a => a.Allowed)
                        && !db.ActionUsers.Any(tu => tu.TaskID == task.ID && tu.UserID == ru.UserID)
                        select ru;

            IEnumerable<Guid> usersToAdd = query.Select(r => r.UserID).Distinct();
            foreach (var userID in usersToAdd)
            {
                //NOTE: by default the users will be added to task as worker.
                task.Users.Add(new PmnTaskUser { Role = DTO.Enums.TaskRoles.Worker, UserID = userID });
            }

            if (usersToAdd.Any())
                await db.SaveChangesAsync();
        }


    }

    internal class PmnTaskConfiguration : IEntityTypeConfiguration<PmnTask>
    {
        public void Configure(EntityTypeBuilder<PmnTask> builder)
        {
            builder.HasMany(t => t.Users)
                .WithOne(t => t.Task)
                .IsRequired(true)
                .HasForeignKey(t => t.TaskID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.References)
                .WithOne(t => t.Task)
                .IsRequired(true)
                .HasForeignKey(t => t.TaskID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.TaskChangedLogs)
                .WithOne(t => t.Task)
                .IsRequired(true)
                .HasForeignKey(t => t.TaskID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.Priority).HasConversion<byte>();
            builder.Property(e => e.Status).HasConversion<int>();
            builder.Property(e => e.Type).HasConversion<int>();
        }
    }

    internal class ActionSecurityConfiguration : DnsEntitySecurityConfiguration<Data.PmnTask>
    {
        public override IQueryable<PmnTask> SecureList(DataContext db, IQueryable<PmnTask> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Portal.ListTasks
                };

            return db.Filter(query, identity, permissions);
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params PmnTask[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Portal.ListTasks); //Once this becomes free form, there will need to be new permissions for this.
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Portal.ListTasks);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Portal.ListTasks);
        }
    }

    public class TaskMappingProfile : AutoMapper.Profile
    {
        public TaskMappingProfile()
        {
            CreateMap<PmnTask, DTO.TaskDTO>()
                .ForMember(d => d.CreatedOn, opt => opt.ConvertUsing<AutoMapperHelpers.DateTimeToDateTimeOffsetConverter, DateTime>(src => src.CreatedOn))
                .ForMember(d => d.StartOn, opt => opt.ConvertUsing<AutoMapperHelpers.NullableDateTimeToNullableDateOffsetTimeConverter, DateTime?>(src => src.StartOn))
                .ForMember(d => d.EndOn, opt => opt.ConvertUsing<AutoMapperHelpers.NullableDateTimeToNullableDateOffsetTimeConverter, DateTime?>(src => src.EndOn))
                .ForMember(d => d.EstimatedCompletedOn, opt => opt.ConvertUsing<AutoMapperHelpers.NullableDateTimeToNullableDateOffsetTimeConverter, DateTime?>(src => src.EstimatedCompletedOn))
                .ForMember(d => d.DueDate, opt => opt.ConvertUsing<AutoMapperHelpers.NullableDateTimeToNullableDateOffsetTimeConverter, DateTime?>(src => src.DueDate));

            CreateMap<DTO.TaskDTO, PmnTask>()
                .ForMember(u => u.Timestamp, opt => opt.Ignore())
                .ForMember(d => d.CreatedOn, opt => opt.ConvertUsing<AutoMapperHelpers.DateTimeOffsetToDateTimeConverter, DateTimeOffset>(src => src.CreatedOn))
                .ForMember(d => d.StartOn, opt => opt.ConvertUsing<AutoMapperHelpers.NullableDateTimeOffsetToNullableDateTimeConverter, DateTimeOffset?>(src => src.StartOn))
                .ForMember(d => d.EndOn, opt => opt.ConvertUsing<AutoMapperHelpers.NullableDateTimeOffsetToNullableDateTimeConverter, DateTimeOffset?>(src => src.EndOn))
                .ForMember(d => d.EstimatedCompletedOn, opt => opt.ConvertUsing<AutoMapperHelpers.NullableDateTimeOffsetToNullableDateTimeConverter, DateTimeOffset?>(src => src.EstimatedCompletedOn))
                .ForMember(d => d.DueDate, opt => opt.ConvertUsing<AutoMapperHelpers.NullableDateTimeOffsetToNullableDateTimeConverter, DateTimeOffset?>(src => src.DueDate));
        }
    }
}
