using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security;
using System.Threading.Tasks;
using CodeFirstStoreFunctions;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities;
using Lpp.Utilities.Security;
using LinqKit;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;
using Lpp.Utilities.Logging;
using System.Web;
using System.Threading;
using Lpp.Utilities.Objects;
using System.Data.Entity.Core.Metadata.Edm;
using Lpp.Dns.Data.Audit;
using Lpp.Workflow.Engine.Interfaces;
using Lpp.Workflow.Engine.Database;
using Microsoft.AspNet.SignalR;
using System.Data.Entity.Validation;

namespace Lpp.Dns.Data
{
    public class DataContext : DbContext, ISecurityContextProvider<PermissionDefinition>, IWorkflowDataContext
    {
        private static INotifier _Notifier;

        public DbSet<ProjectDataMartEvent> ProjectDataMartEvents { get; set; }
        public DbSet<ProjectOrganizationEvent> ProjectOrganizationEvents { get; set; }
        public DbSet<GlobalEvent> GlobalEvents { get; set; }
        public DbSet<OrganizationEvent> OrganizationEvents { get; set; }
        public DbSet<GroupEvent> GroupEvents { get; set; }
        public DbSet<ProjectEvent> ProjectEvents { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
        public DbSet<DataMartEvent> DataMartEvents { get; set; }
        public DbSet<RegistryEvent> RegistryEvents { get; set; }

        public DbSet<Template> Templates { get; set; }
        public DbSet<TemplateTerm> TemplateTerms { get; set; }
        public DbSet<Term> Terms { get; set; }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionLocation> PermissionLocations { get; set; }

        public DbSet<AclProjectRequestTypeWorkflowActivity> ProjectRequestTypeWorkflowActivities { get; set; }
        public DbSet<AclGlobal> GlobalAcls { get; set; }
        public DbSet<AclDataMart> DataMartAcls { get; set; }

        [DbFunction("DataContext", "AclDataMartsFiltered")]
        public IQueryable<FilteredAcl> FilteredDataMartAcls(Guid userID, Guid permissionID, Guid dataMartID)
        {
            return
                ((IObjectContextAdapter) this).ObjectContext.CreateQuery<FilteredAcl>(
                    "dbo.AclDataMartsFiltered(@UserID, @PermissionID, @DataMartID)", new ObjectParameter("UserID", userID), new ObjectParameter("PermissionID", permissionID), new ObjectParameter("DataMartID", dataMartID));
        }

        [DbFunction("DataContext", "AclGlobalFiltered")]
        public IQueryable<FilteredAcl> FilteredGlobalAcls(Guid userID, Guid permissionID)
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FilteredAcl>("dbo.AclGlobalFiltered(@UserID, @PermissionID)", new ObjectParameter("UserID", userID), new ObjectParameter("PermissionID", permissionID));
        }

        [DbFunction("DataContext", "AclOrganizationsFiltered")]
        public IQueryable<FilteredAcl> FilteredOrganizationAcls(Guid userID, Guid permissionID, Guid organizationID)
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FilteredAcl>("dbo.AclOrganizationsFiltered(@UserID, @PermissionID, @OrganizationID)", new ObjectParameter("UserID", userID), new ObjectParameter("PermissionID", permissionID), new ObjectParameter("OriganizationID", organizationID));
        }

        [DbFunction("DataContext", "AclProjectDataMartsFiltered")]
        public IQueryable<FilteredAcl> FilteredProjectDataMartAcls(Guid userID, Guid permissionID, Guid projectID, Guid datamartID)
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FilteredAcl>("dbo.AclProjectDataMartsFiltered(@UserID, @PermissionID, @ProjectID, @DataMartID)", new ObjectParameter("UserID", userID), new ObjectParameter("PermissionID", permissionID), new ObjectParameter("ProjectID", projectID), new ObjectParameter("DataMartID", datamartID));
        }

        [DbFunction("DataContext", "AclProjectOrganizationsFiltered")]
        public IQueryable<FilteredAcl> FilteredProjectOrganizationsAcls(Guid userID, Guid permissionID, Guid projectID, Guid organizationID)
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FilteredAcl>("dbo.AclProjectOrganizationsFiltered(@UserID, @PermissionID, @ProjectID, @OrganizationID)", new ObjectParameter("UserID", userID), new ObjectParameter("PermissionID", permissionID), new ObjectParameter("ProjectID", projectID), new ObjectParameter("OrganizationID", organizationID));
        }

        [DbFunction("DataContext", "AclProjectsFiltered")]
        public IQueryable<FilteredAcl> FilteredProjectAcls(Guid userID, Guid permissionID, Guid projectID)
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FilteredAcl>("dbo.AclProjectsFiltered(@UserID, @PermissionID, @ProjectID)", new ObjectParameter("UserID", userID), new ObjectParameter("PermissionID", permissionID), new ObjectParameter("ProjectID", projectID));
        }

        [DbFunction("DataContext", "AclRequestSharedFoldersFiltered")]
        public IQueryable<FilteredAcl> FilteredRequestSharedFoldersAcls(Guid userID, Guid permissionID, Guid sharedFolderID)
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FilteredAcl>("dbo.AclRequestSharedFoldersFiltered(@UserID, @PermissionID, @RequestSharedFolderID)", new ObjectParameter("UserID", userID), new ObjectParameter("PermissionID", permissionID), new ObjectParameter("RequestSharedFolderID", sharedFolderID));
        }

        [DbFunction("DataContext", "AclUsersFiltered")]
        public IQueryable<FilteredAcl> FilteredUsersAcls(Guid userID, Guid permissionID, Guid targetUserID)
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FilteredAcl>("dbo.AclUsersFiltered(@UserID, @PermissionID, @TargetUserID)", new ObjectParameter("UserID", userID), new ObjectParameter("PermissionID", permissionID), new ObjectParameter("TargetUserID", targetUserID));
        }

        [DbFunction("DataContext", "FilteredRequestList")]
        public IQueryable<Request> FilteredRequestList(Guid userID)
        {
            return
                ((IObjectContextAdapter)this).ObjectContext.CreateQuery<Request>(
                    "DataContext.FilteredRequestList(@UserID)", new ObjectParameter("UserID", userID));
        }

        [DbFunction("DataContext", "FilteredRequestListForEvent")]
        public IQueryable<Request> FilteredRequestListForEvent(Guid userID, Guid? dataMartID)
        {
            return
                ((IObjectContextAdapter)this).ObjectContext.CreateQuery<Request>(
                    "DataContext.FilteredRequestListForEvent(@UserID, @DataMartID)", new ObjectParameter("UserID", userID), new ObjectParameter("DataMartID", dataMartID));
        }

        [DbFunction("DataContext", "RequestRelatedNotificationRecipients")]
        public IQueryable<User> RequestRelatedNotificationRecipients(Guid eventID, Guid requestID)
        {
            return
                ((IObjectContextAdapter)this).ObjectContext.CreateQuery<User>(
                    "DataContext.RequestRelatedNotificationRecipients(@EventID, @RequestID)", new ObjectParameter("EventID", eventID), new ObjectParameter("RequestID", requestID));
        }

        [DbFunction("DataContext", "ResponseRelatedNotificationRecipients")]
        public IQueryable<User> ResponseRelatedNotificationRecipients(Guid eventID, Guid requestDataMartID)
        {
            return
                ((IObjectContextAdapter)this).ObjectContext.CreateQuery<User>(
                    "DataContext.ResponseRelatedNotificationRecipients(@EventID, @RequestDataMartID)", new ObjectParameter("EventID", eventID), new ObjectParameter("RequestDataMartID", requestDataMartID));
        }

        [DbFunction("DataContext", "FilteredResponseList")]
        public IQueryable<Response> FilteredResponseList(Guid userID)
        {
            return
                ((IObjectContextAdapter)this).ObjectContext.CreateQuery<Response>(
                    "DataContext.FilteredResponseList(@UserID)", new ObjectParameter("UserID", userID));
        }

        [DbFunction("DataContext", "AclDataMartAllowUnattendedProcessing")]
        public IQueryable<FilteredRequestAcl> DataMartAllowUnattendedProcessing(Guid userID, Guid requestTypeID, Guid projectID, Guid dataMartID)
        {
            return
                ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FilteredRequestAcl>(
                    "DataContext.AclDataMartAllowUnattendedProcessing(@UserID, @RequestTypeID, @ProjectID, @DataMartID)",
                    new ObjectParameter("UserID", userID),
                    new ObjectParameter("RequestTypeID", requestTypeID),
                    new ObjectParameter("ProjectID", projectID),
                    new ObjectParameter("DataMartID", dataMartID)
                    );
        }

        [DbFunction("DataContext", "AclDataMartRights")]
        public IQueryable<ShortPermission> DataMartRights(Guid userID, Guid projectID, Guid dataMartID)
        {
            return
                ((IObjectContextAdapter)this).ObjectContext.CreateQuery<ShortPermission>(
                    "DataContext.AclDataMartRights(@UserID, @ProjectID, @DataMartID)",
                    new ObjectParameter("UserID", userID),
                    new ObjectParameter("ProjectID", projectID),
                    new ObjectParameter("DataMartID", dataMartID)
                    );
        }

        [DbFunction("DataContext", "GetAssignedUserNotifications")]
        public IQueryable<DTO.AssignedUserNotificationDTO> GetAssignedUserNotifications(Guid userID)
        {
            return
                ((IObjectContextAdapter)this).ObjectContext.CreateQuery<DTO.AssignedUserNotificationDTO>(
                    "DataContext.GetAssignedUserNotifications(@UserID)", new ObjectParameter("UserID", userID));
        }

        [DbFunction("DataContext", "UsersAbleToViewRequest")]
        public IQueryable<UserList> UsersAbleToViewRequest(Guid requestID)
        {
            return
                ((IObjectContextAdapter)this).ObjectContext.CreateQuery<UserList>(
                    "DataContext.UsersAbleToViewRequest(@RequestID)", new ObjectParameter("RequestID", requestID));
        }

        [DbFunction("DataContext", "ReturnUserEventSubscriptionsByRequest")]
        public IQueryable<DTO.Users.UserEventSubscriptionDetail> ReturnUserEventSubscriptionsByRequest(Guid requestID, Guid eventID, bool immediate)
        {
            return
                ((IObjectContextAdapter)this).ObjectContext.CreateQuery<DTO.Users.UserEventSubscriptionDetail>(
                    "DataContext.ReturnUserEventSubscriptionsByRequest(@RequestID, @EventID, @Immediate)", new ObjectParameter("RequestID", requestID), new ObjectParameter("EventID", eventID), new ObjectParameter("Immediate", immediate));
        }

        [DbFunction("DataContext", "ReturnUserEventSubscriptionsByResponse")]
        public IQueryable<DTO.Users.UserEventSubscriptionDetail> ReturnUserEventSubscriptionsByResponse(Guid responseID, Guid eventID, bool immediate)
        {
            return
                ((IObjectContextAdapter)this).ObjectContext.CreateQuery<DTO.Users.UserEventSubscriptionDetail>(
                    "DataContext.ReturnUserEventSubscriptionsByResponse(@ResponseID, @EventID, @Immediate)", new ObjectParameter("ResponseID", responseID), new ObjectParameter("EventID", eventID), new ObjectParameter("Immediate", immediate));
        }

        [DbFunction("DataContext", "FilteredSecurityGroups")]
        public IQueryable<SecurityGroup> FilteredSecurityGroups(Guid userID)
        {
            return
                ((IObjectContextAdapter)this).ObjectContext.CreateQuery<SecurityGroup>(
                    "DataContext.FilteredSecurityGroups(@UserID)", new ObjectParameter("UserID", userID));
        }

        [DbFunction("DataContext", "GetNotifications")]
        public IQueryable<NotificationDTO> GetNotifications(Guid userID)
        {
            return
                ((IObjectContextAdapter)this).ObjectContext.CreateQuery<NotificationDTO>(
                    "DataContext.GetNotifications(@UserID)", new ObjectParameter("UserID", userID));
        }

        [DbFunction("DataContext", "GetWorkflowHistory")]
        public IQueryable<WorkflowHistoryItemDTO> GetWorkflowHistory(Guid requestID, Guid userID)
        {
            return ((IObjectContextAdapter)this).ObjectContext
                .CreateQuery<WorkflowHistoryItemDTO>("DataContext.GetWorkflowHistory(@RequestID, @UserID)", new ObjectParameter("RequestID", requestID), new ObjectParameter("UserID", userID));
        }

        [DbFunction("CodeFirstDatabaseSchema", "GetRequestAssigneesForTask")]
        public string GetRequestAssigneesForTask(Guid taskID, string delimiter)
        {
            throw new NotSupportedException();
        }

        [DbFunction("CodeFirstDatabaseSchema", "GetRequestStatusDisplayText")]
        public string GetRequestStatusDisplayText(Guid requestID)
        {
            throw new NotSupportedException();
        }

        public DbSet<AclDataMartRequestType> DataMartRequestTypeAcls { get; set; }
        public DbSet<AclGroup> GroupAcls { get; set; }
        public DbSet<AclOrganization> OrganizationAcls { get; set; }
        public DbSet<AclOrganizationDataMart> OrganizationDataMartAcls { get; set; }
        public DbSet<AclOrganizationUser> OrganizationUserAcls { get; set; }
        public DbSet<AclProject> ProjectAcls { get; set; }
        public DbSet<AclProjectRequestType> ProjectRequestTypeAcls { get; set; }
        public DbSet<AclProjectDataMart> ProjectDataMartAcls { get; set; }
        public DbSet<AclGlobalFieldOption> GlobalFieldOptionAcls { get; set; }
        public DbSet<AclProjectFieldOption> ProjectFieldOptionAcls { get; set; }
        
        public DbSet<AclProjectOrganization> ProjectOrganizationAcls { get; set; }        
        public DbSet<AclProjectDataMartRequestType> ProjectDataMartRequestTypeAcls { get; set; }
        public DbSet<AclRegistry> RegistryAcls { get; set; }
        public DbSet<AclRequest> RequestAcls { get; set; }
        public DbSet<AclRequestSharedFolder> RequestSharedFolderAcls { get; set; }
        public DbSet<AclRequestType> RequestTypeAcls { get; set; }
        public DbSet<AclUser> UserAcls { get; set; }
        public DbSet<AclTemplate> TemplateAcls { get; set; }


        public DbSet<ACLEntry> AclEntries { get; set; }
        public DbSet<SecurityTuple1> SecurityTuple1s { get; set; }
        public DbSet<SecurityTuple2> SecurityTuple2s { get; set; }
        public DbSet<SecurityTuple3> SecurityTuple3s { get; set; }
        public DbSet<SecurityTuple4> SecurityTuple4s { get; set; }
        public DbSet<SecurityInheritanceClosure> SecurityInheritance { get; set; }
        public DbSet<SecurityTarget> SecurityTargets { get; set; }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<AuditEvent> AuditEvents { get; set; }
        public DbSet<AuditPropertyValue> AuditEventProperties { get; set; }

        //public DbSet<DatMartAvailabilityPeriods> DataMartsAvailabilityPeriods { get; set; }
        public DbSet<Audit.RegistryChangeLog> LogsRegistryChange { get; set; }
        public DbSet<Audit.UserChangeLog> LogsUserChange { get; set; }
        public DbSet<Audit.DataMartChangeLog> LogsDataMartChange { get; set; }
        public DbSet<Audit.ExternalCommunicationLog> LogsExternalCommunication { get; set; }
        public DbSet<Audit.ProfileUpdatedLog> LogsProfileChange { get; set; }
        public DbSet<Audit.PasswordExpirationLog> LogsPasswordExpiration { get; set; }
        public DbSet<Audit.UserRegistrationSubmittedLog> LogsUserRegistrationSubmitted { get; set; }
        public DbSet<Audit.UserRegistrationChangedLog> LogsUserRegistrationChanged { get; set; }
        public DbSet<Audit.GroupChangeLog> LogsGroupChange { get; set; }
        public DbSet<Audit.OrganizationChangedLog> LogsOrganizationChange { get; set; }
        public DbSet<Audit.ProjectChangeLog> LogsProjectChange { get; set; }
        public DbSet<Audit.SubmittedRequestNeedsApprovalLog> LogsSubmittedrequestNeedsApproval { get; set; }
        public DbSet<Audit.SubmittedRequestAwaitsResponseLog> LogsSubmittedRequestAwaitsResponse { get; set; }
        public DbSet<Audit.NewRequestSubmittedLog> LogsNewRequestSubmitted { get; set; }
        public DbSet<Audit.RequestStatusChangedLog> LogsRequestStatusChanged { get; set; }
        public DbSet<Audit.UploadedResultNeedsApprovalLog> LogsUploadedResultNeedsApproval { get; set; }
        public DbSet<Audit.RoutingStatusChangeLog> LogsRoutingStatusChange { get; set; }
        public DbSet<Audit.ResponseViewedLog> LogsResponseViewed { get; set; }
        public DbSet<Audit.ResultsReminderLog> LogsResultsReminder { get; set; }
        public DbSet<Audit.NewDataMartClientLog> LogsNewDataMartClient { get; set; }
        public DbSet<Audit.PmnTaskChangeLog> LogsTaskChange { get; set; }
        public DbSet<Audit.PmnTaskReminderLog> LogsTaskReminder { get; set; }
        public DbSet<Audit.DocumentChangeLog> LogsDocumentChange { get; set; }
        public DbSet<Audit.RequestAssignmentChangeLog> LogsRequestAssignmentChange { get; set; }
        public DbSet<Audit.RequestDocumentChangeLog> LogsRequestDocumentChange { get; set; }
        public DbSet<Audit.RequestCommentChangeLog> LogsRequestCommentChange { get; set; }
        public DbSet<Audit.RequestMetadataChangeLog> LogsRequestMetadataChange { get; set; }
        public DbSet<Audit.RequestDataMartMetadataChangeLog> LogsRequestDataMartMetadataChange { get; set; }
        public DbSet<Audit.NewRequestDraftSubmittedLog> LogsNewRequestDraftSubmitted { get; set; }
        public DbSet<Audit.RequestDataMartAddedRemovedLog> LogsRequestDataMartAddedRemoved { get; set; }
        public DbSet<Audit.UserAuthenticationLogs> LogsUserAuthentication { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<UserEventSubscription> UserEventSubscriptions { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<SsoEndpoint> SsoEndpoints { get; set; }

        public DbSet<SecurityGroup> SecurityGroups { get; set; }
        public DbSet<SecurityGroupUser> SecurityGroupUsers { get; set; }

        public DbSet<Network> Networks { get; set; }
        public DbSet<NetworkMessage> NetworkMessages { get; set; }
        public DbSet<NetworkMessageUser> NetworkMessageUsers { get; set; }
        public DbSet<Group> Groups { get; set; }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationRegistry> OrganizationRegistries { get; set; }
        public DbSet<OrganizationEHRS> OrganizationEHRSes { get; set; }
        public DbSet<OrganizationGroup> OrganizationGroups { get; set; }

        public DbSet<DataMart> DataMarts { get; set; }
        public DbSet<DataMartType> DataMartTypes { get; set; }
        public DbSet<DataMartInstalledModel> DataMartModels { get; set; }
        public DbSet<DataMartAvailabilityPeriod> DataMartAvailabilityPeriods { get; set; }
        public DbSet<DataAvailabilityPeriodCategory> DataAvailabilityPeriodCategory { get; set; }

        public DbSet<LookupList> LookupLists { get; set; }
        public DbSet<LookupListCategory> LookupListCategories { get; set; }
        public DbSet<LookupListValue> LookupListValues { get; set; }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventLocation> EventLocations { get; set; }

        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestUser> RequestUsers { get; set; }
        public DbSet<RequestDataMart> RequestDataMarts { get; set; }
        public DbSet<RequestSharedFolder> RequestSharedFolders { get; set; }
        public DbSet<RequestType> RequestTypes { get; set; }
        public DbSet<RequestTypeTerm> RequestTypeTerms { get; set; }
        public DbSet<RequestSearchTerm> RequestSearchTerms { get; set; }
        public DbSet<RequestObserver> RequestObservers { get; set; }
        public DbSet<RequestObserverEventSubscription> RequestObserverEventSubscriptions { get; set; }
        public DbSet<DataModel> DataModels { get; set; }
        public DbSet<RequestTypeModel> RequestTypeDataModels { get; set; }
        public DbSet<DataModelSupportedTerm> DataModelSupportedTerms { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<ResponseSearchResult> ResponseSearchResults { get; set;}
        public DbSet<RequesterCenter> RequesterCenters { get; set; }
        public DbSet<ResponseGroup> ResponseGroups { get; set; }
        public DbSet<WorkplanType> WorkplanTypes { get; set; }
        public DbSet<ReportAggregationLevel> ReportAggregationLevels { get; set; }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectDataMart> ProjectDataMarts { get; set; }
        public DbSet<ProjectOrganization> ProjectOrganizations { get; set; }
        public DbSet<ProjectRequestType> ProjectRequestTypes { get; set; }
        

        public DbSet<Document> Documents { get; set; }
        public DbSet<RequestDocument> RequestDocuments { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentReference> CommentReferences { get; set; }


        public DbSet<Demographic> Demographics { get; set; }
        public DbSet<GeographicLocation> GeographicLocations { get; set; }
        public DbSet<ZCTADemographic> DemographicsByZCTA { get; set; }

        public DbSet<Registry> Registries { get; set; }
        public DbSet<RegistryItemDefinition> RegistryItemDefinitions { get; set; }

        public DbSet<PmnTask> Actions { get; set; }
        public DbSet<PmnTaskUser> ActionUsers { get; set; }
        public DbSet<TaskReference> ActionReferences { get; set; }

        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<WorkflowRole> WorkflowRoles { get; set; }
        public DbSet<WorkflowActivity> WorkflowActivities { get; set; }
        public DbSet<WorkflowActivityCompletionMap> WorkflowActivityCompletionMaps { get; set; }

        public DbSet<vNewRequestSubmittedLog> viewNewRequestSubmittedLogs { get; set; }
        public DbSet<vRoutingStatusChangedLog> viewRoutingStatusChangedLogs { get; set; }
        public DbSet<RequestStatistics> RequestStatistics { get; set; }

        public DbSet<DataAdapterDetailTerm> DataAdapterDetailTerms { get; set; }

        private static readonly bool Initialized = false;
        private static List<object> Security { get; set; }
        private static List<object> Logging { get; set; }

        public static Dictionary<Type, string> Hubs { get; set; }
        static DataContext()
        {
            if (!Initialized)
            {
                Initialized = true;
                Security = new List<object>();
                Logging = new List<object>();
                Hubs = new Dictionary<Type, string>();
                //System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Migrations.Configuration>());
            }
        }

        public static void RegisteryNotifier(INotifier notifier) {
            DataContext._Notifier = notifier;
        }

        public DataContext()
        {
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = true;
            this.DisableExtendedValidationAndSave = false;
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            objectContext.CommandTimeout = 999;
        }        

        public DataContext(string connectionString)
        {
            Database.Connection.ConnectionString = connectionString;
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = true;
            this.DisableExtendedValidationAndSave = false;
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            objectContext.CommandTimeout = 999;
        }

        public DataContext(bool enableLazyLoading) : this()
        {
            if (enableLazyLoading)
            {
                this.Configuration.ProxyCreationEnabled = true;
                this.Configuration.LazyLoadingEnabled = true;
            }
        }

        public bool DisableExtendedValidationAndSave { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.ComplexType<FilteredAcl>();
            modelBuilder.ComplexType<FilteredRequestAcl>();
            modelBuilder.ComplexType<UserList>();
            modelBuilder.ComplexType<DTO.Users.UserEventSubscriptionDetail>();
            modelBuilder.ComplexType<AssignedUserNotificationDTO>();
            modelBuilder.ComplexType<NotificationDTO>();
            modelBuilder.ComplexType<WorkflowHistoryItemDTO>();
            modelBuilder.ComplexType<ShortPermission>();
            modelBuilder.Conventions.Add(new FunctionsConvention<DataContext>("dbo"));

            //Loads all of the configuration classes to define the joins. These are stored in the same file as the originating class
            //Fluent should only be used for relationships that are not easily describable using attributes
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes().Where(
                type => type.BaseType != null &&
                    type.BaseType.IsGenericType &&
                    type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

            foreach (object configurationInstance in typesToRegister.Select(Activator.CreateInstance))
            {
                modelBuilder.Configurations.Add((dynamic)configurationInstance);
            }

            //Register the security management system
            var securityTypesToRegister = Assembly.GetExecutingAssembly().GetTypes().Where(
        type => type.BaseType != null && !type.IsAbstract &&
        type.BaseType.IsGenericType &&
        (type.BaseType.GetGenericTypeDefinition() == typeof(EntitySecurityConfiguration<,,>) || type.BaseType.GetGenericTypeDefinition() == typeof(DnsEntitySecurityConfiguration<>)));

            foreach (object configurationInstance in securityTypesToRegister.Select(Activator.CreateInstance))
            {
                Security.Add(configurationInstance);
            }

            //Register the logging management system
            var logTypesToRegister = Assembly.GetExecutingAssembly().GetTypes().Where(
        type => type.BaseType != null && !type.IsAbstract &&
        type.BaseType.IsGenericType &&
        (type.BaseType.GetGenericTypeDefinition() == typeof(EntityLoggingConfiguration<,>)));
            foreach (object configurationInstance in logTypesToRegister.Select(Activator.CreateInstance))
                Logging.Add(configurationInstance);

        }

        public override int SaveChanges()
        {
            //Get all of the Logging configurations if not already loaded

            Dictionary<DbEntityEntry, IEnumerable<AuditLog>> logs = new Dictionary<DbEntityEntry, IEnumerable<AuditLog>>();

            foreach (var entry in ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged && e.State != EntityState.Detached && e.State != EntityState.Deleted))
            {
                var newLogs = LogEntry(entry);
                if (newLogs.Any())
                    logs.Add(entry, newLogs);
            }

            var hubEntries = ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged && e.State != EntityState.Detached).ToArray();

            int count = 0;
            int result;
        retry:
            
            try
            {
                result =  base.SaveChanges();
            }
            catch (CommitFailedException ce)
            {
                count++;
                if (count <= 5)
                {
                    System.Threading.Thread.Sleep(300 * count);
                    goto retry;
                }

                throw ce;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //If notifications, pass them to the new thread to process and send
            //Note that we do this after save because some notifications are dependant upon the changed values.
            List<Notification> notifications = new List<Notification>();
            foreach (var logDetails in logs)
            {
                var conf = Logging.FirstOrDefault(l => l.GetType().BaseType.GetGenericArguments()[1] == logDetails.Key.Entity.GetType());
                foreach (var log in logDetails.Value)
                {
                    var notification = conf.GetType().GetMethod("CreateNotifications").MakeGenericMethod(log.GetType()).Invoke(conf, new object[] { log, this, true }) as IEnumerable<Notification>;

                    if (notification != null && notification.Any())
                        notifications.AddRange(notification);
                }
            }                

            if (notifications.Any())
                Notify(notifications);

            //Notify Crud to Hubs
            NotifyHubs(hubEntries);
            
            return result;
        }

        public override async Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            Dictionary<DbEntityEntry, IEnumerable<AuditLog>> logs = new Dictionary<DbEntityEntry, IEnumerable<AuditLog>>();

            foreach (var entry in ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged && e.State != EntityState.Detached && e.State != EntityState.Deleted))
            {
                var newLogs = LogEntry(entry);
                if (newLogs.Any())
                    logs.Add(entry, newLogs);
            }

            var hubEntries = ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged && e.State != EntityState.Detached).ToArray();


            int result;
            int count = 0;
        retry:

            try
            {
                result = await base.SaveChangesAsync(cancellationToken);
            }
            catch (CommitFailedException ce)
            {
                count++;
                if (count <= 5)
                {
                    System.Threading.Thread.Sleep(300 * count);
                    goto retry;
                }

                throw ce;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //If notifications, pass them to the new thread to process and send
            //Note that we do this after save because some notifications are dependant upon the changed values.
            List<Notification> notifications = new List<Notification>();
            foreach (var logDetails in logs)
            {
                var conf = Logging.FirstOrDefault(l => l.GetType().BaseType.GetGenericArguments()[1] == logDetails.Key.Entity.GetType());

                foreach (var log in logDetails.Value)
                {
                    var notification = conf.GetType().GetMethod("CreateNotifications").MakeGenericMethod(log.GetType()).Invoke(conf, new object[] { log, this, true }) as IEnumerable<Notification>;

                    if (notification != null && notification.Any())
                        notifications.AddRange(notification);
                }
            }

            if (notifications.Any())
                Notify(notifications);

            //Notify Crud to Hubs
            NotifyHubs(hubEntries);

            return result;
        }

        /// <summary>
        /// Sends communications to the Hubs using the CrudNotify endpoint
        /// </summary>
        /// <param name="entries"></param>
        private void NotifyHubs(IEnumerable<DbEntityEntry> entries)
        {
            var maps = from e in entries join h in Hubs on e.Entity.GetType() equals h.Key select new { Entry = e.Entity, Hub = h.Value, State = (int) e.State };

            foreach (var map in maps)
            {
                var hub = GlobalHost.ConnectionManager.GetHubContext(map.Hub);

                var ID = ((EntityWithID) map.Entry).ID;
                List<string> userIDs;
                //Do security here and then send
                if (map.Entry is Request)
                {
                    userIDs = UsersAbleToViewRequest(ID).ToArray().Where(u => u.UserID != null).Select(u => u.UserID.ToString()).ToList();
                }
                else
                {
                    throw new NotSupportedException("This type of entry is not yet supported. Please ensure that you add the logic for getting the users that can see the entity here.");
                }

                hub.Clients.Users(userIDs).NotifyCrud(new NotificationCrudDTO
                {
                    ObjectID = ID,
                    State = (ObjectStates)map.State
                });
            }
        }

        /// <summary>
        /// Sends the live notifications as necessary after save in a separate thread.
        /// </summary>
        /// <param name="notifications"></param>
        private void Notify(IEnumerable<Notification> notifications)
        {
            if (notifications.Any() && _Notifier != null)
            {
                var thread = new Thread(() => _Notifier.Notify(notifications));                
                thread.Start();
            }
        }

        private IEnumerable<AuditLog> LogEntry(DbEntityEntry entry, bool read = false)            
        {
            var logs = new List<AuditLog>();
            //If entry has logging configuration, and process, then add those notifications to the notifications list.
            var conf =  Logging.FirstOrDefault(l => l.GetType().BaseType.GetGenericArguments()[1] == entry.Entity.GetType());

            if (conf != null)
            {
                var method = conf.GetType().GetMethod("ProcessEvents");
                logs.AddRange((IEnumerable<AuditLog>) method.Invoke(conf, new object[] { entry, this, Identity, read}));
            }
            return logs.AsEnumerable();
        }

        public Task LogRead(object entity)
        {
            return LogRead(new[] { entity });
        }

        public async Task LogRead(IEnumerable<object> entities)
        {
            List<Notification> notifications = new List<Notification>();

            foreach (var entity in entities)
            {
                var entry = this.ChangeTracker.Entries().FirstOrDefault(e => e.Entity == entity);
                if (entry == null)
                    throw new ArgumentOutOfRangeException("The entity could not be found in the change trackker to log the read request.");

                var logs = LogEntry(entry, true);

                foreach (var log in logs)
                {
                    var conf = Logging.FirstOrDefault(l => l.GetType().BaseType.GetGenericArguments()[1] == entry.Entity.GetType());

                    var notification = conf.GetType().GetMethod("CreateNotifications").MakeGenericMethod(log.GetType()).Invoke(conf, new object[] { log, this, true }) as IEnumerable<Notification>;

                    if (notification != null && notification.Any())
                        notifications.AddRange(notification);
                }
            }

            Notify(notifications);
        }

        public void ForceLog(object entity)
        {
            ForceLog(new[] { entity });
        }

        public void ForceLog(IEnumerable<object> entities)
        {
            List<Notification> notifications = new List<Notification>();

            foreach (var entity in entities)
            {
                var entry = this.ChangeTracker.Entries().FirstOrDefault(e => e.Entity == entity);
                if (entry == null)
                    throw new ArgumentOutOfRangeException("The entity could not be found in the change trackker to log the read request.");

                var logs = LogEntry(entry, true);

                foreach (var log in logs)
                {
                    var conf = Logging.FirstOrDefault(l => l.GetType().BaseType.GetGenericArguments()[1] == entry.Entity.GetType());

                    var notification = conf.GetType().GetMethod("CreateNotifications").MakeGenericMethod(log.GetType()).Invoke(conf, new object[] { log, this, false }) as IEnumerable<Notification>;


                    if (notification != null && notifications.Any())
                        notifications.AddRange(notification);
                }
            }

            Notify(notifications);
        }

        /// <summary>
        /// Returns the workflow activity given the completion information
        /// </summary>
        /// <param name="workflowID">The ID of the workflow.</param>
        /// <param name="sourceWorkflowActivityID">The ID of the source workflow activity.</param>
        /// <param name="ResultID">The activity result ID.</param>
        /// <returns></returns>
        public async Task<IDbWorkflowActivity> GetWorkflowActivityFromCompletion(Guid workflowID, Guid sourceWorkflowActivityID, Guid ResultID)
        {
            var result = await (from cm in WorkflowActivityCompletionMaps where cm.WorkflowID == workflowID && cm.SourceWorkflowActivityID == sourceWorkflowActivityID && cm.WorkflowActivityResultID == ResultID select cm.DestinationWorkflowActivity).FirstOrDefaultAsync(); //No longer returns null if end point

            return (IDbWorkflowActivity)result;
        }

        /// <summary>
        /// Returns a specified workflow activity from the ID.
        /// </summary>
        /// <param name="workflowActivityID"></param>
        /// <returns></returns>
        public IDbWorkflowActivity GetWorkflowActivityByID(Guid workflowActivityID)
        {
            return WorkflowActivities.Find(workflowActivityID);
        }

        private ApiIdentity Identity
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    //HttpContext.Current will be null when the datacontext is run outside of iis.
                    if (System.Threading.Thread.CurrentPrincipal == null)
                        return null;

                    var identity = System.Threading.Thread.CurrentPrincipal.Identity as ApiIdentity;
                    if (identity == null)
                        return null;

                    return identity;
                }

                if (HttpContext.Current.User == null)
                    return null;

                var apiIdentity = HttpContext.Current.User.Identity as ApiIdentity;
                if (apiIdentity == null)
                    return null;

                return apiIdentity;
            }
        }

        List<object> ISecurityContextProvider<PermissionDefinition>.Security
        {
            get { return DataContext.Security; }
        }

        /// <summary>
        /// Returns the validated user, or throws an error if the credentials fail.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<IUser> ValidateUser(string userName, string password)
        {
            var user = await (from c in this.Users.Include(x => x.Organization).Include(x => x.RejectedBy).Include(x => x.DeactivatedBy)
                                 where c.UserName == userName && !c.Deleted && c.Active
                                 select c).FirstOrDefaultAsync();

            if (user == null)
                throw new SecurityException("The Login or Password is invalid.");
               
            if (password.ComputeHash() != user.PasswordHash)
            {
                throw new SecurityException("The Login or Password is invalid.");
            }

            return user;
        }


        public bool ValidateUser2(string userName, string password, out IUser user)
        {
            user = (from c in this.Users.Include(x => x.Organization).Include(x => x.RejectedBy).Include(x => x.DeactivatedBy)
                              where c.UserName == userName && !c.Deleted
                              select c).FirstOrDefault();

            if (user == null || ((User)user).PasswordHash != password.ComputeHash())
                return false;
            return true;

        }


        /// <summary>
        /// Returns permission on a specific security permission against the Global Acls
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="PermissionIdentifiers"></param>
        /// <returns></returns>
        public async Task<bool> HasPermission(ApiIdentity identity, PermissionDefinition permissions)
        {
            return (await HasGrantedPermissions(identity, permissions)).Contains(permissions);
        }
        
        /// <summary>
        /// Returns the granted permissions for the specified object based on an array of permissions passed to it. (i.e. it filters the demanded permissions providing the actual allowed permissions
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="identity"></param>
        /// <param name="objID"></param>
        /// <param name="PermissionIdentifierss"></param>
        /// <returns></returns>
        public Task<IEnumerable<PermissionDefinition>> HasGrantedPermissions<TEntity>(ApiIdentity identity, Guid objID, params PermissionDefinition[] PermissionIdentifierss)
            where TEntity : class
        {
            return HasGrantedPermissions<TEntity>(identity, new Guid[] { objID }, PermissionIdentifierss);
        }

        public Task<IEnumerable<PermissionDefinition>> HasGrantedPermissions<TEntity>(ApiIdentity identity, Guid objID, ExtendedQuery filters, params PermissionDefinition[] PermissionIdentifierss)
    where TEntity : class
        {
            return HasGrantedPermissions<TEntity>(identity, new Guid[] { objID }, filters, PermissionIdentifierss);
        }

        public Task<IEnumerable<PermissionDefinition>> HasGrantedPermissions<TEntity>(ApiIdentity identity, Guid[] objIDs, params PermissionDefinition[] permissions)
    where TEntity : class
        {
            var sec = (DnsEntitySecurityConfiguration<TEntity>)Security.FirstOrDefault(s => s is DnsEntitySecurityConfiguration<TEntity>);

            if (sec == null)
                throw new ArgumentNullException("sec", "The entity does not have a security configuration");

            return sec.HasGrantedPermissions(this, identity, objIDs, permissions);
        }

        public Task<IEnumerable<PermissionDefinition>> HasGrantedPermissions<TEntity>(ApiIdentity identity, Guid[] objIDs, ExtendedQuery filters, params PermissionDefinition[] permissions)
where TEntity : class
        {
            var sec = (DnsEntitySecurityConfiguration<TEntity>)Security.FirstOrDefault(s => s is DnsEntitySecurityConfiguration<TEntity>);

            if (sec == null)
                throw new ArgumentNullException("sec", "The entity does not have a security configuration");

            return sec.HasGrantedPermissions(this, identity, objIDs, filters, permissions);
        }

        public async Task<IEnumerable<PermissionDefinition>> HasGrantedPermissions(ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            var result = await HasGrantedPermissions(identity, null, permissions);
            return result;
        }

        /// <summary>
        /// Returns the granted global permissions filtering the demanded permissions to the actual granted permissions.
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="PermissionIdentifierss"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PermissionDefinition>> HasGrantedPermissions(ApiIdentity identity, ExtendedQuery filters, params PermissionDefinition[] permissions)
        {
            var predicateAll = PredicateBuilder.True<Permission>();
            var predicateAny = PredicateBuilder.False<Permission>();

            if (filters == null)
                filters = new ExtendedQuery();

            foreach (var permission in permissions)
            {

                foreach (var type in permission.Locations)
                {
                    switch (type)
                    {
                        case PermissionAclTypes.Global:
                            var gAcls = GlobalAcls.AsQueryable().FilterAcl(identity, permission);

                            if (filters.Global != null)
                                gAcls = gAcls.Where(filters.Global);

                            predicateAny = predicateAny.Or(p => gAcls.Where(a => a.PermissionID == p.ID).Any());
                            predicateAll = predicateAll.And(p => gAcls.Where(a => a.PermissionID == p.ID).All(a => a.Allowed));

                            break;
                        case PermissionAclTypes.DataMarts:
                            var gDataMartAcls = DataMartAcls.AsQueryable().FilterAcl(identity, permission);

                            if (filters.DataMarts != null)
                                gDataMartAcls = gDataMartAcls.Where(filters.DataMarts);

                            predicateAny = predicateAny.Or(p => gDataMartAcls.Where(a => a.PermissionID == p.ID).Any());
                            predicateAll = predicateAll.And(p => gDataMartAcls.Where(a => a.PermissionID == p.ID).All(a => a.Allowed));

                            break;
                        case PermissionAclTypes.Groups:
                            var gGroupAcls = GroupAcls.AsQueryable().FilterAcl(identity, permission);
                            if (filters.Groups != null)
                                gGroupAcls = gGroupAcls.Where(filters.Groups);
                            
                            predicateAny = predicateAny.Or(p => gGroupAcls.Where(a => a.PermissionID == p.ID).Any());
                            predicateAll = predicateAll.And(p => gGroupAcls.Where(a => a.PermissionID == p.ID).All(a => a.Allowed));
                            break;
                        case PermissionAclTypes.OrganizationDataMarts:
                            var gOrganizationDataMartAcls = OrganizationDataMartAcls.AsQueryable().FilterAcl(identity, permission);
                            if (filters.OrganizationDataMarts != null)
                                gOrganizationDataMartAcls = gOrganizationDataMartAcls.Where(filters.OrganizationDataMarts);
                            
                            predicateAny = predicateAny.Or(p => gOrganizationDataMartAcls.Where(a => a.PermissionID == p.ID).Any());
                            predicateAll = predicateAll.And(p => gOrganizationDataMartAcls.Where(a => a.PermissionID == p.ID).All(a => a.Allowed));
                            break;
                        case PermissionAclTypes.Organizations:
                            var gOrganizationAcls = OrganizationAcls.AsQueryable().FilterAcl(identity,permission);
                            if (filters.Organizations != null)
                                gOrganizationAcls = gOrganizationAcls.Where(filters.Organizations);

                            predicateAny = predicateAny.Or(p => gOrganizationAcls.Where(a => a.PermissionID == p.ID).Any());
                            predicateAll = predicateAll.And(p => gOrganizationAcls.Where(a => a.PermissionID == p.ID).All(a => a.Allowed));
                            break;
                        case PermissionAclTypes.OrganizationUsers:
                            var gOrganizationUserAcls = OrganizationUserAcls.AsQueryable().FilterAcl(identity,permission);
                            if (filters.OrganizationUsers != null)
                                gOrganizationUserAcls = gOrganizationUserAcls.Where(filters.OrganizationUsers);

                            predicateAny = predicateAny.Or(p => gOrganizationUserAcls.Where(a => a.PermissionID == p.ID).Any());
                            predicateAll = predicateAll.And(p => gOrganizationUserAcls.Where(a => a.PermissionID == p.ID).All(a => a.Allowed));
                            break;
                        case PermissionAclTypes.ProjectDataMarts:
                            var gProjectDataMartAcls = ProjectDataMartAcls.AsQueryable().FilterAcl(identity, permission);
                            if (filters.ProjectDataMarts != null)
                                gProjectDataMartAcls = gProjectDataMartAcls.Where(filters.ProjectDataMarts);

                            predicateAny = predicateAny.Or(p => gProjectDataMartAcls.Where(a => a.PermissionID == p.ID).Any());
                            predicateAll = predicateAll.And(p => gProjectDataMartAcls.Where(a => a.PermissionID == p.ID).All(a => a.Allowed));
                            break;
                        case PermissionAclTypes.ProjectOrganizations:
                            var gProjectOrganizationAcls = ProjectOrganizationAcls.AsQueryable().FilterAcl(identity,permission);
                            if (filters.ProjectOrganizations != null)
                                gProjectOrganizationAcls = gProjectOrganizationAcls.Where(filters.ProjectOrganizations);

                            predicateAny = predicateAny.Or(p => gProjectOrganizationAcls.Where(a => a.PermissionID == p.ID).Any());
                            predicateAll = predicateAll.And(p => gProjectOrganizationAcls.Where(a => a.PermissionID == p.ID).All(a => a.Allowed));
                            break;
                        case PermissionAclTypes.Projects:
                            var gProjectAcls = ProjectAcls.AsQueryable().FilterAcl(identity, permission);
                            if (filters.Projects != null)
                                gProjectAcls = gProjectAcls.Where(filters.Projects);

                            predicateAny = predicateAny.Or(p => gProjectAcls.Where(a => a.PermissionID == p.ID).Any());
                            predicateAll = predicateAll.And(p => gProjectAcls.Where(a => a.PermissionID == p.ID).All(a => a.Allowed));
                            break;
                        case PermissionAclTypes.Registries:
                            var gRegistries = RegistryAcls.AsQueryable().FilterAcl(identity,permission);
                            if (filters.Registries != null)
                                gRegistries = gRegistries.Where(filters.Registries);

                            predicateAny = predicateAny.Or(p => gRegistries.Where(a => a.PermissionID == p.ID).Any());
                            predicateAll = predicateAll.And(p => gRegistries.Where(a => a.PermissionID == p.ID).All(a => a.Allowed));
                            break;
                        case PermissionAclTypes.Templates:
                            var gTemplates = TemplateAcls.AsQueryable().FilterAcl(identity, permission);
                            if (filters.Templates != null)
                                gTemplates = gTemplates.Where(filters.Templates);

                            predicateAny = predicateAny.Or(p => gTemplates.Where(a => a.PermissionID == p.ID).Any());
                            predicateAll = predicateAll.And(p => gTemplates.Where(a => a.PermissionID == p.ID).All(a => a.Allowed));
                            break;
                        case PermissionAclTypes.Requests:
                            var gRequestAcls = RequestAcls.AsQueryable().FilterAcl(identity,permission);
                            if (filters.Requests != null)
                                gRequestAcls = gRequestAcls.Where(filters.Requests);

                            predicateAny = predicateAny.Or(p => gRequestAcls.Where(a => a.PermissionID == p.ID).Any());
                            predicateAll = predicateAll.And(p => gRequestAcls.Where(a => a.PermissionID == p.ID).All(a => a.Allowed));
                            break;
                        case PermissionAclTypes.RequestSharedFolders:
                            var gRequestSharedFolderAcls = RequestSharedFolderAcls.AsQueryable().FilterAcl(identity,permission);
                            if (filters.SharedFolders != null)
                                gRequestSharedFolderAcls = gRequestSharedFolderAcls.Where(filters.SharedFolders);

                            predicateAny = predicateAny.Or(p => gRequestSharedFolderAcls.Where(a => a.PermissionID == p.ID).Any());
                            predicateAll = predicateAll.And(p => gRequestSharedFolderAcls.Where(a => a.PermissionID == p.ID).All(a => a.Allowed));
                            break;
                        case PermissionAclTypes.Users:
                            var gUserAcls = UserAcls.AsQueryable().FilterAcl(identity, permission);
                            if (filters.Users != null)
                                gUserAcls = gUserAcls.Where(filters.Users);

                            predicateAny = predicateAny.Or(p => gUserAcls.Where(a => a.PermissionID == p.ID).Any());
                            predicateAll = predicateAll.And(p => gUserAcls.Where(a => a.PermissionID == p.ID).All(a => a.Allowed));
                            break;
                        case PermissionAclTypes.ProjectRequestTypeWorkflowActivity:
                            var gProjectRequestTypeWorkflowActivityAcls = ProjectRequestTypeWorkflowActivities.AsQueryable().FilterAcl(identity, permission);
                            if (filters.ProjectRequestTypeWorkflowActivity != null)
                                gProjectRequestTypeWorkflowActivityAcls = gProjectRequestTypeWorkflowActivityAcls.Where(filters.ProjectRequestTypeWorkflowActivity);

                            predicateAny = predicateAny.Or(p => gProjectRequestTypeWorkflowActivityAcls.Where(a => a.PermissionID == p.ID).Any());
                            predicateAll = predicateAll.And(p => gProjectRequestTypeWorkflowActivityAcls.Where(a => a.PermissionID == p.ID).All(a => a.Allowed));
                            break;
                        default:
                            throw new NotSupportedException("The security location is not supported.");
                    }
                }
                
            }

            return (from p in await Permissions.AsExpandable().Where(predicateAny.Expand()).Where(predicateAll.Expand()).Select(p => p.ID).ToArrayAsync() join rp in permissions on p equals rp.ID select rp);
        }

        public IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> query, ApiIdentity identity, params PermissionDefinition[] requestedPermissions)
            where TEntity : EntityWithID
        {
            return Filter(query, identity, null, requestedPermissions);
        }

        public IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> query, ApiIdentity identity, ExtendedQuery filters, params PermissionDefinition[] requestedPermissions)
            where TEntity : EntityWithID
        {
            return Filter(query, identity.ID, filters, requestedPermissions);
        }

        public IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> query, Guid userID, ExtendedQuery filters, params PermissionDefinition[] requestedPermissions)
            where TEntity : EntityWithID
        {
            var predicateAll = PredicateBuilder.True<TEntity>();
            var predicateAny = PredicateBuilder.False<TEntity>();

            if (filters == null)
                filters = new ExtendedQuery();

            var locations = requestedPermissions.SelectMany(p => p.Locations).Distinct();
            foreach (var location in locations)
            {
                var permissions = requestedPermissions.Where(p => p.Locations.Any(l => l == location)).ToArray();

                switch (location)
                {
                    case PermissionAclTypes.RequestTypes:
                        var rtAcls = RequestTypeAcls.FilterAcl(userID, permissions);
                        if (filters.RequestTypes != null)
                            rtAcls.Where(filters.RequestTypes);
                        if (typeof(TEntity) == typeof(RequestType))
                        {
                            predicateAll = predicateAll.And(p => rtAcls.Where(a => a.RequestTypeID == p.ID).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => rtAcls.Where(a => a.RequestTypeID == p.ID).Any());
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("A permission type of Request Type requires that the entity type of Filter must be Request Type");
                        }

                        break;
                    case PermissionAclTypes.DataMarts:
                        var dmAcls = DataMartAcls.FilterAcl(userID, permissions);

                        if (filters.DataMarts != null)
                        {
                            dmAcls = dmAcls.Where(filters.DataMarts);
                        }

                        if (typeof(TEntity) == typeof(DataMart))
                        {
                            predicateAll = predicateAll.And(p => dmAcls.Where(a => a.DataMartID == p.ID).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => dmAcls.Where(a => a.DataMartID == p.ID).Any());
                        }
                        else if (typeof(TEntity) == typeof(Request))
                        {
                            predicateAll = predicateAll.And(p => dmAcls.Where(a => a.DataMart.Requests.Any(r => r.RequestID == p.ID)).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => dmAcls.Where(a => a.DataMart.Requests.Any(r => r.RequestID == p.ID)).Any());
                        }
                        else if (typeof(TEntity) == typeof(Project))
                        {
                            predicateAll = predicateAll.And(p => dmAcls.Where(a => a.DataMart.Projects.Any(r => r.ProjectID == p.ID)).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => dmAcls.Where(a => a.DataMart.Projects.Any(r => r.ProjectID == p.ID)).Any());
                        }  
                        else if (typeof(TEntity) == typeof(Organization))
                        {
                            predicateAll = predicateAll.And(p => dmAcls.Where(a => a.DataMart.OrganizationID == p.ID).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => dmAcls.Where(a => a.DataMart.OrganizationID == p.ID).Any());
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("A permission type of DataMarts requires that the entity type of Filter must be DataMart or Request");
                        }
                        break;
                    case PermissionAclTypes.Global:
                        var gAcls = GlobalAcls.FilterAcl(userID, permissions);

                        if (filters.Global != null)
                        {
                            gAcls = gAcls.Where(filters.Global);
                        }

                        predicateAll = predicateAll.And(p => gAcls.All(a => a.Allowed));
                        predicateAny = predicateAny.Or(p => gAcls.Any());
                        break;
                    case PermissionAclTypes.Groups:
                        var groupAcls = GroupAcls.FilterAcl(userID, permissions);

                        if (filters.Groups != null)
                            groupAcls = groupAcls.Where(filters.Groups);

                        if (typeof(TEntity) == typeof(Group))
                        {
                            predicateAll = predicateAll.And(p => groupAcls.Where(g => g.GroupID == p.ID).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => groupAcls.Where(g => g.GroupID == p.ID).Any());
                        }
                        else if (typeof(TEntity) == typeof(Organization))
                        {
                            predicateAll = predicateAll.And(p => groupAcls.Where(g => g.Group.Organizations.Any(o => o.OrganizationID == p.ID)).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => groupAcls.Where(g => g.Group.Organizations.Any(o => o.OrganizationID == p.ID)).Any());
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("A permission type of Groups requires that the entity type of Filter must be Group or organization");
                        }
                        break;
                    case PermissionAclTypes.Requests:
                        var requestAcls = RequestAcls.FilterAcl(userID, permissions);

                        if (filters.Requests != null)
                            requestAcls = requestAcls.Where(filters.Requests);

                        if (typeof(TEntity) == typeof(Request))
                        {
                            predicateAll = predicateAll.And(p => requestAcls.Where(g => g.RequestID == p.ID).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => requestAcls.Where(r => r.RequestID == p.ID).Any());
                        }
                        else if (typeof(TEntity) == typeof(Project))
                        {
                            predicateAll = predicateAll.And(p => requestAcls.Where(g => g.Request.ProjectID == p.ID).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => requestAcls.Where(r => r.Request.ProjectID == p.ID).Any());
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("A permission type of Request requires that the entity type of Filter must be Request or project");
                        }
                        break;
                    case PermissionAclTypes.Organizations:
                        var organizationAcls = OrganizationAcls.FilterAcl(userID, permissions);

                        if (filters.Organizations != null)
                        {
                            organizationAcls = organizationAcls.Where(filters.Organizations);
                        }

                        if (typeof(TEntity) == typeof(Organization))
                        {
                            predicateAll = predicateAll.And(p => organizationAcls.Where(a => a.OrganizationID == p.ID).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => organizationAcls.Where(a => a.OrganizationID == p.ID).Any());
                        }
                        else if (typeof(TEntity) == typeof(Request))
                        {
                            predicateAll = predicateAll.And(p => organizationAcls.Where(a => a.Organization.Requests.Any(r => r.ID == p.ID)).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => organizationAcls.Where(a => a.Organization.Requests.Any(r => r.ID == p.ID)).Any());
                        }
                        else if (typeof(TEntity) == typeof(DataMart))
                        {
                            predicateAll = predicateAll.And(p => organizationAcls.Where(a => a.Organization.DataMarts.Any(r => r.ID == p.ID)).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => organizationAcls.Where(a => a.Organization.DataMarts.Any(r => r.ID == p.ID)).Any());
                        }
                        else if (typeof(TEntity) == typeof(Project))
                        {
                            predicateAll = predicateAll.And(p => organizationAcls.Where(a => a.Organization.Projects.Any(r => r.ProjectID == p.ID)).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => organizationAcls.Where(a => a.Organization.Projects.Any(r => r.ProjectID == p.ID)).Any());
                        }
                        else if (typeof(TEntity) == typeof(User))
                        {
                            predicateAll = predicateAll.And(p => organizationAcls.Where(a => a.Organization.Users.Any(u => u.ID == p.ID)).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => organizationAcls.Where(a => a.Organization.Users.Any(u => u.ID == p.ID)).Any());
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("A permission type of Organization requires that the entity type of Filter must be Organization or Request");
                        }
                        break;
                    case PermissionAclTypes.OrganizationUsers:
                        var ouAcls = OrganizationUserAcls.FilterAcl(userID, permissions);

                        if (filters.OrganizationUsers != null)
                        {
                            ouAcls = ouAcls.Where(filters.OrganizationUsers);
                        }

                        if (typeof(TEntity) == typeof(Organization))
                        {
                            predicateAll = predicateAll.And(p => ouAcls.Where(a => a.OrganizationID == p.ID).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => ouAcls.Where(a => a.OrganizationID == p.ID).Any());
                        }
                        else if (typeof(TEntity) == typeof(User))
                        {
                            predicateAll = predicateAll.And(p => ouAcls.Where(a => a.UserID == p.ID).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => ouAcls.Where(a => a.UserID == p.ID).Any());
                        }
                        else if (typeof(TEntity) == typeof(Request))
                        {
                            predicateAll = predicateAll.And(p => ouAcls.Where(a => a.Organization.Requests.Any(r => r.ID == p.ID) && a.User.CreatedRequests.Any(r => r.ID == p.ID)).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => ouAcls.Where(a => a.Organization.Requests.Any(r => r.ID == p.ID) && a.User.CreatedRequests.Any(r => r.ID == p.ID)).Any());
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("A permission type of Organization Users requires that the entity type of Filter must be Organization or Users");
                        }
                        break;
                    case PermissionAclTypes.ProjectDataMarts:
                        var pdAcls = ProjectDataMartAcls.FilterAcl(userID, permissions);

                        if (filters.ProjectDataMarts != null)
                        {
                            pdAcls = pdAcls.Where(filters.ProjectDataMarts);
                        }

                        if (typeof(TEntity) == typeof(Project))
                        {
                            predicateAll = predicateAll.And(p => pdAcls.Where(a => a.ProjectID == p.ID).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => pdAcls.Where(a => a.ProjectID == p.ID).Any());
                        }
                        else if (typeof(TEntity) == typeof(DataMart))
                        {
                            predicateAll = predicateAll.And(p => pdAcls.Where(a => a.DataMartID == p.ID).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => pdAcls.Where(a => a.DataMartID == p.ID).Any());
                        }
                        else if (typeof(TEntity) == typeof(Request))
                        {
                            predicateAll = predicateAll.And(p => pdAcls.Where(a => a.DataMart.Requests.Any(r => r.RequestID == p.ID) && a.Project.Requests.Any(r => r.ID == p.ID)).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => pdAcls.Where(a => a.DataMart.Requests.Any(r => r.RequestID == p.ID) && a.Project.Requests.Any(r => r.ID == p.ID)).Any());
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("A permission type of Project DataMarts requires that the entity type of Filter must be Project, Request or DataMart");
                        }
                        break;
                    case PermissionAclTypes.ProjectOrganizations:
                        var poAcls = ProjectOrganizationAcls.FilterAcl(userID, permissions);

                        if (filters.ProjectOrganizations != null)
                        {
                            poAcls = poAcls.Where(filters.ProjectOrganizations);
                        }

                        if (typeof(TEntity) == typeof(Organization))
                        {
                            predicateAll = predicateAll.And(p => poAcls.Where(a => a.OrganizationID == p.ID).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => poAcls.Where(a => a.OrganizationID == p.ID).Any());
                        }
                        else if (typeof(TEntity) == typeof(Project))
                        {
                            predicateAll = predicateAll.And(p => poAcls.Where(a => a.ProjectID == p.ID).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => poAcls.Where(a => a.ProjectID == p.ID).Any());
                        }
                        else if (typeof(TEntity) == typeof(Request))
                        {
                            predicateAll = predicateAll.And(p => poAcls.Where(a => a.Project.Requests.Any(r => r.ID == p.ID) && a.Organization.Requests.Any(r => r.ID == p.ID)).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => poAcls.Where(a => a.Project.Requests.Any(r => r.ID == p.ID) && a.Organization.Requests.Any(r => r.ID == p.ID)).Any());
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("A permission type of Project Organizations requires that the entity type of Filter must be Organization or Project");
                        }
                        break;
                    case PermissionAclTypes.Projects:
                        var projectAcls = ProjectAcls.FilterAcl(userID, permissions);

                        if (filters.Projects != null)
                        {
                            projectAcls = projectAcls.Where(filters.Projects);
                        }

                        if (typeof(TEntity) == typeof(Project))
                        {
                            predicateAll = predicateAll.And(p => projectAcls.Where(g => g.ProjectID == p.ID).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => projectAcls.Where(r => r.ProjectID == p.ID).Any());
                        }
                        else if (typeof(TEntity) == typeof(Request))
                        {
                            predicateAll = predicateAll.And(p => projectAcls.Where(a => a.Project.Requests.Any(r => r.ID == p.ID)).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => projectAcls.Where(a => a.Project.Requests.Any(r => r.ID == p.ID)).Any());
                        }
                        else if (typeof(TEntity) == typeof(DataMart))
                        {
                            predicateAll = predicateAll.And(p => projectAcls.Where(a => a.Project.DataMarts.Any(dm => dm.DataMartID == p.ID)).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => projectAcls.Where(a => a.Project.DataMarts.Any(dm => dm.DataMartID == p.ID)).Any());
                        }
                        else if(typeof(TEntity) == typeof(Organization))
                        {
                            predicateAll = predicateAll.And(p => projectAcls.Where(a => a.Project.Organizations.Any(dm => dm.OrganizationID == p.ID)).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => projectAcls.Where(a => a.Project.Organizations.Any(dm => dm.OrganizationID == p.ID)).Any());
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("A permission type of Projects requires that the entity type of Filter must be Project, DataMart or Request");
                        }
                        break;
                    case PermissionAclTypes.Registries:
                        var registriesAcls = RegistryAcls.FilterAcl(userID, permissions);

                        if (filters.Registries != null)
                        {
                            registriesAcls = registriesAcls.Where(filters.Registries);
                        }

                        if (typeof(TEntity) == typeof(Registry))
                        {
                            predicateAll = predicateAll.And(p => registriesAcls.Where(g => g.RegistryID == p.ID).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => registriesAcls.Where(r => r.RegistryID == p.ID).Any());
                        }
                        else if (typeof(TEntity) == typeof(Organization))
                        {
                            predicateAll = predicateAll.And(p => registriesAcls.Where(r => r.Registry.Organizations.Any(o => o.OrganizationID == p.ID)).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => registriesAcls.Where(r => r.Registry.Organizations.Any(o => o.OrganizationID == p.ID)).Any());
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("A permission type of Registries requires that the entity type of Filter must be Registry or Organization.");
                        }
                        break;
                    case PermissionAclTypes.Templates:
                        var templateAcls = TemplateAcls.FilterAcl(userID, permissions);
                        if (filters.Templates != null)
                        {
                            templateAcls = templateAcls.Where(filters.Templates);
                        }
                        if (typeof(TEntity) == typeof(Template))
                        {
                            predicateAll = predicateAll.And(p => templateAcls.Where(a => a.TemplateID == p.ID).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => templateAcls.Where(a => a.TemplateID == p.ID).Any());
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("A permission type of Templates requires that the entity type of Filter must be Template.");
                        }
                        break;
                    case PermissionAclTypes.RequestSharedFolders:
                        var rsfAcls = this.RequestSharedFolderAcls.FilterAcl(userID, permissions);

                        if (filters.SharedFolders != null)
                        {
                            rsfAcls = rsfAcls.Where(filters.SharedFolders);
                        }

                        if (typeof(TEntity) == typeof(RequestSharedFolder))
                        {
                            predicateAll = predicateAll.And(p => rsfAcls.Where(g => g.RequestSharedFolderID == p.ID).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => rsfAcls.Where(r => r.RequestSharedFolderID == p.ID).Any());
                        }
                        else if (typeof(TEntity) == typeof(Request))
                        {
                            predicateAll = predicateAll.And(p => rsfAcls.Where(a => a.RequestSharedFolder.Requests.Any(r => r.RequestID == p.ID)).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => rsfAcls.Where(a => a.RequestSharedFolder.Requests.Any(r => r.RequestID == p.ID)).Any());
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("A permission type of Request Shared Folders requires that the entity type of Filter must be RequestSharedFolder or Request");
                        }
                        break;
                    case PermissionAclTypes.UserProfile:
                    case PermissionAclTypes.Users:
                        var userAcls = this.UserAcls.FilterAcl(userID, permissions);

                        if (filters.Users != null)
                        {
                            userAcls = userAcls.Where(filters.Users);
                        }

                        if (typeof(TEntity) == typeof(User))
                        {
                            predicateAll = predicateAll.And(p => userAcls.Where(g => g.UserID == p.ID).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => userAcls.Where(r => r.UserID == p.ID).Any());
                        }
                        else if (typeof(TEntity) == typeof(Request))
                        {
                            predicateAll = predicateAll.And(p => userAcls.Where(a => a.User.CreatedRequests.Any(r => r.ID == p.ID) || a.User.SubmittedRequests.Any(r => r.ID == p.ID)).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => userAcls.Where(a => a.User.CreatedRequests.Any(r => r.ID == p.ID) || a.User.SubmittedRequests.Any(r => r.ID == p.ID)).Any());
                        }
                        else if (typeof(TEntity) == typeof(Project))
                        {
                            predicateAll = predicateAll.And(p => userAcls.Where(a => a.UserID == userID && a.User.Organization.Projects.Any(r => r.ProjectID == p.ID)).All(a => a.Allowed));
                            predicateAny = predicateAny.Or(p => userAcls.Where(a => a.UserID == userID && a.User.Organization.Projects.Any(r => r.ProjectID == p.ID)).Any());
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("A permission type of Users/UserProfile requires that the entity type of Filter must be User or Request");
                        }
                        break;
                }

            }

            //Combine the two predicates and and them so that we know that we have at least one allowed, and all are allowed
            //var predicate = PredicateBuilder.True<TEntity>();
            //predicate = predicate.And(predicateAll.Expand());
            //predicate = predicate.And(predicateAny.Expand());

            return query.AsExpandable().Where(predicateAny.Expand()).Where(predicateAll.Expand());
        }


        public async Task<IEnumerable<Guid>> HasGrantedPermissions<TEntity>(ApiIdentity identity, Guid[] objIDs, params Guid[] permissionIDs) where TEntity : class
        {
            //Convert the permissions to permissiondefinitions using reflection
            //Call the other function
            var permissions = PermissionIdentifiers.Definitions.Where(d => d != null && permissionIDs.Contains(d.ID));
            return (await this.HasGrantedPermissions<TEntity>(identity, objIDs, permissions.ToArray())).Select(p => p.ID);
        }        
    }
}
