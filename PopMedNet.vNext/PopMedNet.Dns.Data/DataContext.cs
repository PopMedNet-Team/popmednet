#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PopMedNet.Utilities.Security;
using PopMedNet.Utilities;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Dns.DTO.Enums;
using LinqKit;
using PopMedNet.Workflow.Engine.Database;
using System.Reflection;
using PopMedNet.Utilities.Objects;
using System.ComponentModel.DataAnnotations;
using PopMedNet.Utilities.Logging;
using PopMedNet.Dns.DTO;
using PopMedNet.Dns.Data.Requests;

namespace PopMedNet.Dns.Data
{
    public class DataContext : DbContext, ISecurityContextProvider<PermissionDefinition>, PopMedNet.Workflow.Engine.Database.IWorkflowDataContext, IPopMedNetAuthenticationManager
    {

        public DbSet<AclDataMart> DataMartAcls { get; set; }
        public DbSet<AclDataMartRequestType> DataMartRequestTypeAcls { get; set; }
        public DbSet<AclGlobal> GlobalAcls { get; set; }
        public DbSet<AclGlobalFieldOption> GlobalFieldOptionAcls { get; set; }
        public DbSet<AclGroup> GroupAcls { get; set; }
        public DbSet<AclOrganization> OrganizationAcls { get; set; }
        public DbSet<AclOrganizationDataMart> OrganizationDataMartAcls { get; set; }
        public DbSet<AclOrganizationUser> OrganizationUserAcls { get; set; }
        public DbSet<AclProject> ProjectAcls { get; set; }
        public DbSet<AclProjectDataMart> ProjectDataMartAcls { get; set; }
        public DbSet<AclProjectDataMartRequestType> ProjectDataMartRequestTypeAcls { get; set; }
        public DbSet<AclProjectFieldOption> ProjectFieldOptionAcls { get; set; }
        public DbSet<AclProjectOrganization> ProjectOrganizationAcls { get; set; }
        public DbSet<AclProjectRequestType> ProjectRequestTypeAcls { get; set; }
        public DbSet<AclProjectRequestTypeWorkflowActivity> ProjectRequestTypeWorkflowActivities { get; set; }
        public DbSet<AclRegistry> RegistryAcls { get; set; }
        public DbSet<AclRequest> RequestAcls { get; set; }
        public DbSet<AclRequestSharedFolder> RequestSharedFolderAcls { get; set; }
        public DbSet<AclRequestType> RequestTypeAcls { get; set; }
        public DbSet<AclTemplate> TemplateAcls { get; set; }
        public DbSet<AclUser> UserAcls { get; set; }
        public DbSet<TaskReference> ActionReferences { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentReference> CommentReferences { get; set; }
        public DbSet<DataAdapterDetailTerm> DataAdapterDetailTerms { get; set; }
        public DbSet<DataMart> DataMarts { get; set; }
        public DbSet<DataAvailabilityPeriodCategory> DataAvailabilityPeriodCategory { get; set; }
        public DbSet<DataMartAvailabilityPeriod> DataMartAvailabilityPeriods { get; set; }
        public DbSet<DataMartAvailabilityPeriod_v2> DataMartAvailabilityPeriodsV2 { get; set; }
        public DbSet<DataMartEvent> DataMartEvents { get; set; }
        public DbSet<DataMartInstalledModel> DataMartModels { get; set; }
        public DbSet<DataModelSupportedTerm> DataModelSupportedTerms { get; set; }
        public DbSet<DataMartType> DataMartTypes { get; set; }
        public DbSet<DataModel> DataModels { get; set; }
        public DbSet<Demographic> Demographics { get; set; }
        public DbSet<GeographicLocation> GeographicLocations { get; set; }
        public DbSet<ZCTADemographic> DemographicsByZCTA { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventLocation> EventLocations { get; set; }
        public DbSet<GlobalEvent> GlobalEvents { get; set; }
        public DbSet<GroupEvent> GroupEvents { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<LookupList> LookupLists { get; set; }
        public DbSet<LookupListCategory> LookupListCategories { get; set; }
        public DbSet<LookupListValue> LookupListValues { get; set; }
        public DbSet<Network> Networks { get; set; }
        public DbSet<NetworkMessage> NetworkMessages { get; set; }
        public DbSet<NetworkMessageUser> NetworkMessageUsers { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationEHRS> OrganizationEHRSes { get; set; }
        public DbSet<OrganizationEvent> OrganizationEvents { get; set; }
        public DbSet<OrganizationGroup> OrganizationGroups { get; set; }
        public DbSet<OrganizationRegistry> OrganizationRegistries { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionLocation> PermissionLocations { get; set; }
        public DbSet<ProjectEvent> ProjectEvents { get; set; }
        public DbSet<ProjectOrganizationEvent> ProjectOrganizationEvents { get; set; }
        public DbSet<PmnTask> Actions { get; set; }
        public DbSet<PmnTaskUser> ActionUsers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectDataMart> ProjectDataMarts { get; set; }
        public DbSet<ProjectDataMartEvent> ProjectDataMartEvents { get; set; }
        public DbSet<ProjectOrganization> ProjectOrganizations { get; set; }
        public DbSet<ProjectRequestType> ProjectRequestTypes { get; set; }
        public DbSet<Registry> Registries { get; set; }
        public DbSet<RegistryEvent> RegistryEvents { get; set; }
        public DbSet<RegistryItemDefinition> RegistryItemDefinitions { get; set; }
        public DbSet<ReportAggregationLevel> ReportAggregationLevels { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestDataMart> RequestDataMarts { get; set; }
        public DbSet<RequestDocument> RequestDocuments { get; set; }
        public DbSet<RequesterCenter> RequesterCenters { get; set; }
        public DbSet<RequestObserver> RequestObservers { get; set; }
        public DbSet<RequestObserverEventSubscription> RequestObserverEventSubscriptions { get; set; }
        public DbSet<RequestSchedule> RequestSchedules { get; set; }
        public DbSet<RequestSearchTerm> RequestSearchTerms { get; set; }
        public DbSet<RequestSharedFolder> RequestSharedFolders { get; set; }
        public DbSet<RequestStatistics> RequestStatistics { get; set; }
        public DbSet<RequestType> RequestTypes { get; set; }
        public DbSet<RequestTypeModel> RequestTypeDataModels { get; set; }
        public DbSet<RequestTypeTerm> RequestTypeTerms { get; set; }
        public DbSet<RequestUser> RequestUsers { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<ResponseGroup> ResponseGroups { get; set; }
        public DbSet<ResponseSearchResult> ResponseSearchResults { get; set; }
        public DbSet<SecurityGroup> SecurityGroups { get; set; }
        public DbSet<SecurityGroupUser> SecurityGroupUsers { get; set; }
        public DbSet<SsoEndpoint> SsoEndpoints { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<TemplateTerm> TemplateTerms { get; set; }
        public DbSet<Term> Terms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
        public DbSet<UserEventSubscription> UserEventSubscriptions { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<WorkflowRole> WorkflowRoles { get; set; }
        public DbSet<WorkflowActivity> WorkflowActivities { get; set; }
        public DbSet<WorkflowActivityCompletionMap> WorkflowActivityCompletionMaps { get; set; }
        public DbSet<WorkplanType> WorkplanTypes { get; set; }
        public DbSet<Audit.DataMartChangeLog> LogsDataMartChange { get; set; }
        public DbSet<Audit.DocumentChangeLog> LogsDocumentChange { get; set; }
        public DbSet<Audit.DocumentDeleteLog> LogsDeletedDocumentArchive { get; set; }
        public DbSet<Audit.ExternalCommunicationLog> LogsExternalCommunication { get; set; }
        public DbSet<Audit.GroupChangeLog> LogsGroupChange { get; set; }
        public DbSet<Audit.NewDataMartClientLog> LogsNewDataMartClient { get; set; }
        public DbSet<Audit.NewRequestDraftSubmittedLog> LogsNewRequestDraftSubmitted { get; set; }
        public DbSet<Audit.NewRequestSubmittedLog> LogsNewRequestSubmitted { get; set; }
        public DbSet<Audit.OrganizationChangedLog> LogsOrganizationChange { get; set; }
        public DbSet<Audit.PasswordExpirationLog> LogsPasswordExpiration { get; set; }
        public DbSet<Audit.PmnTaskChangeLog> LogsTaskChange { get; set; }
        public DbSet<Audit.PmnTaskReminderLog> LogsTaskReminder { get; set; }
        public DbSet<Audit.ProfileUpdatedLog> LogsProfileChange { get; set; }
        public DbSet<Audit.ProjectChangeLog> LogsProjectChange { get; set; }
        public DbSet<Audit.RegistryChangeLog> LogsRegistryChange { get; set; }
        public DbSet<Audit.RequestAssignmentChangeLog> LogsRequestAssignmentChange { get; set; }
        public DbSet<Audit.RequestCommentChangeLog> LogsRequestCommentChange { get; set; }
        public DbSet<Audit.RequestDataMartAddedRemovedLog> LogsRequestDataMartAddedRemoved { get; set; }
        public DbSet<Audit.RequestDataMartMetadataChangeLog> LogsRequestDataMartMetadataChange { get; set; }
        public DbSet<Audit.RequestDocumentChangeLog> LogsRequestDocumentChange { get; set; }
        public DbSet<Audit.RequestMetadataChangeLog> LogsRequestMetadataChange { get; set; }
        public DbSet<Audit.RequestStatusChangedLog> LogsRequestStatusChange { get; set; }
        public DbSet<Audit.ResponseViewedLog> LogsResponseViewed { get; set; }
        public DbSet<Audit.ResultsReminderLog> LogsResultsReminder { get; set; }
        public DbSet<Audit.RoutingStatusChangeLog> LogsRoutingStatusChange { get; set; }
        public DbSet<Audit.SubmittedRequestAwaitsResponseLog> LogsSubmittedRequestAwaitsResponse { get; set; }
        public DbSet<Audit.SubmittedRequestNeedsApprovalLog> LogsSubmittedRequestNeedsApproval { get; set; }
        public DbSet<Audit.UploadedResultNeedsApprovalLog> LogsUploadedResultNeedsApproval { get; set; }
        public DbSet<Audit.UserAuthenticationLogs> LogsUserAuthentication { get; set; }
        public DbSet<Audit.UserChangeLog> LogsUserChange { get; set; }
        public DbSet<Audit.UserPasswordChangeLog> LogsUserPasswordChange { get; set; }
        public DbSet<Audit.UserRegistrationChangedLog> LogsUserRegistrationChanged { get; set; }
        public DbSet<Audit.UserRegistrationSubmittedLog> LogsUserRegistrationSubmitted { get; set; }
        public DbSet<Audit.vNewRequestSubmittedLog> viewNewRequestSubmittedLogs { get; set; }
        public DbSet<Audit.vRoutingStatusChangedLog> viewRoutingStatusChangedLogs { get; set; }
        //public DbSet<UserNotificationDTO> viewGetNotifications { get; set; }

        //[DbFunction("DataContext", "FilteredSecurityGroups")]
        //public IQueryable<SecurityGroup> FilteredSecurityGroups(Guid userID)
        //{
        //    return
        //        ((IObjectContextAdapter)this).ObjectContext.CreateQuery<SecurityGroup>(
        //            "DataContext.FilteredSecurityGroups(@UserID)", new ObjectParameter("UserID", userID));
        //}

        public IQueryable<SecurityGroup> FilteredSecurityGroups(Guid userID)
        {
            return FromExpression(() => FilteredSecurityGroups(userID));
        }

        public IQueryable<NotificationDTO> GetNotifications(Guid userID)
        {
            return FromExpression(() => GetNotifications(userID));
        }

        public IQueryable<AssignedUserNotificationDTO> GetAssignedUserNotifications(Guid userID)
        {
            return FromExpression(() => GetAssignedUserNotifications(userID));
        }

        public string GetRequestAssigneesForTask(Guid taskID, string delimiter)
        {
            throw new NotSupportedException();
        }

        public string GetRequestStatusDisplayText(Guid requestID)
        {
            throw new NotSupportedException();
        }

        public IQueryable<FilteredRequestListItem> FilteredRequestList(Guid userID)
        {
            return FromExpression(() => FilteredRequestList(userID));
        }

        public IQueryable<AclGlobal> AclGlobalFiltered(Guid userID, Guid permissionID)
        {
            return FromExpression(() => AclGlobalFiltered(userID, permissionID));
        }


        static List<object> _securityConfigurations = new List<object>();
        public List<object> Security
        {
            get { return _securityConfigurations; }
        }

        static List<object> _loggingConfigurations = new List<object>();
        private List<object> Logging
        {
            get
            {
                return _loggingConfigurations;
            }
        }

        //ApiIdentity Identity
        //{
        //    get
        //    {
        //        if(_httpContextAccessor == null || _httpContextAccessor.HttpContext == null || _httpContextAccessor.HttpContext.User == null)
        //        {
        //            return null;
        //        }

        //        return _httpContextAccessor.HttpContext.User.Identity as ApiIdentity;
        //    }
        //}

        //readonly IHttpContextAccessor _httpContextAccessor;

        public DataContext() : base()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //    modelBuilder.ComplexType<DTO.Users.UserEventSubscriptionDetail>();
            //    modelBuilder.ComplexType<WorkflowHistoryItemDTO>();
            //    modelBuilder.ComplexType<DataMartAvailabilityPeriodV2DTO>();

            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<FilteredAcl>().HasNoKey();
            modelBuilder.Entity<FilteredRequestAcl>().HasNoKey();
            modelBuilder.Entity<ShortPermission>().HasNoKey();
            modelBuilder.Entity<UserList>();
            modelBuilder.Entity<NotificationDTO>().HasNoKey();
            modelBuilder.Entity<AssignedUserNotificationDTO>().HasNoKey();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            modelBuilder.HasDbFunction(typeof(DataContext).GetMethod(nameof(FilteredSecurityGroups), new[] { typeof(Guid) }));
            modelBuilder.HasDbFunction(typeof(DataContext).GetMethod(nameof(GetNotifications), new[] { typeof(Guid) }));
            modelBuilder.HasDbFunction(typeof(DataContext).GetMethod(nameof(GetAssignedUserNotifications), new[] { typeof(Guid) }));
            modelBuilder.HasDbFunction(typeof(DataContext).GetMethod(nameof(GetRequestAssigneesForTask), new[] { typeof(Guid), typeof(string) })).HasName("GetRequestAssigneesForTask");
            modelBuilder.HasDbFunction(typeof(DataContext).GetMethod(nameof(GetRequestStatusDisplayText), new[] { typeof(Guid) })).HasName("GetRequestStatusDisplayText");
            modelBuilder.HasDbFunction(typeof(DataContext).GetMethod(nameof(FilteredRequestList), new[] { typeof(Guid) })).HasName("FilteredRequestList");
            modelBuilder.HasDbFunction(typeof(DataContext).GetMethod(nameof(AclGlobalFiltered), new[] { typeof(Guid), typeof(Guid) })).HasName("AclGlobalFiltered");

            //modelBuilder.Entity<UserNotificationDTO>(b => {
            //    b.HasNoKey();
            //    b.ToView("vGetNotifications");
            //});

            //NOTE: OnModelCreating is only called once per application runtime. The security configurations and logging configurations need to be stored in a static variable. (Look at moving to an IoC type container)

            //Register the security management system
            var securityTypesToRegister = Assembly.GetExecutingAssembly().GetTypes().Where(
                    type => type.BaseType != null && !type.IsAbstract &&
                    type.BaseType.IsGenericType &&
                    (type.BaseType.GetGenericTypeDefinition() == typeof(EntitySecurityConfiguration<,,>) || type.BaseType.GetGenericTypeDefinition() == typeof(DnsEntitySecurityConfiguration<>)));

            foreach (object configurationInstance in securityTypesToRegister.Select(Activator.CreateInstance))
            {
                _securityConfigurations.Add(configurationInstance);
            }

            //Register the logging management system
            var logTypesToRegister = Assembly.GetExecutingAssembly().GetTypes().Where(
                    type => type.BaseType != null && !type.IsAbstract &&
                    type.BaseType.IsGenericType &&
                    (type.BaseType.GetGenericTypeDefinition() == typeof(Utilities.Logging.EntityLoggingConfiguration<,>)));
            foreach (object configurationInstance in logTypesToRegister.Select(Activator.CreateInstance))
                _loggingConfigurations.Add(configurationInstance);
        }

        public override int SaveChanges()
        {
            return SaveChanges(true);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            //ValidateEntities();

            //TODO: build change logs

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return SaveChangesAsync(true, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            //execute validation - used to be done automatically by EF, now must be explicitly done
            //ValidateEntities();

            //TODO: build change logs

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        void ValidateEntities()
        {
            var entities = from e in ChangeTracker.Entries()
                           where e.State == EntityState.Added
                               || e.State == EntityState.Modified
                           select e.Entity;
            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(
                    entity,
                    validationContext,
                    validateAllProperties: true);
            }
        }

        /// <summary>
        /// Returns the validated user, or throws an error if the credentials fail.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<IUser> ValidateUserAsync(string userName, string password)
        {
            var result = await (from c in Users
                                where c.UserName == userName && !c.Deleted
                                select new
                                {
                                    c.ID,
                                    c.FirstName,
                                    c.LastName,
                                    c.UserName,
                                    c.PasswordHash
                                }).FirstOrDefaultAsync();

            if (result == null)
                throw new System.Security.SecurityException("The Login or Password is invalid.");

            if (password.ComputeHash() != result.PasswordHash)
            {
                throw new System.Security.SecurityException("The Login or Password is invalid.");
            }

            return new AuthenticationResultModel(result.ID, result.FirstName, result.LastName, result.UserName);
        }

        public bool ValidateUser(string userName, string password, out IUser user)
        {
            var result = (from c in Users
                          where c.UserName == userName && !c.Deleted
                          select new
                          {
                              c.ID,
                              c.FirstName,
                              c.LastName,
                              c.UserName,
                              c.PasswordHash
                          }).FirstOrDefault();

            if (result == null || result.PasswordHash != password.ComputeHash())
            {
                user = null;
                return false;
            }

            user = new PopMedNet.Utilities.Security.AuthenticationResultModel(result.ID, result.FirstName, result.LastName, result.UserName);
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

        public async Task<IEnumerable<Guid>> HasGrantedPermissions<TEntity>(ApiIdentity identity, Guid[] objIDs, params Guid[] permissions) where TEntity : class
        {
            //Convert the permissions to permissiondefinitions using reflection
            //Call the other function
            var permissionIDs = PermissionIdentifiers.Definitions.Where(d => d != null && permissions.Contains(d.ID)).ToArray();
            return (await this.HasGrantedPermissions<TEntity>(identity, objIDs, permissionIDs)).Select(p => p.ID);
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
            var predicateAll = PredicateBuilder.New<Permission>(true);
            var predicateAny = PredicateBuilder.New<Permission>(false);

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
                            var gOrganizationAcls = OrganizationAcls.AsQueryable().FilterAcl(identity, permission);
                            if (filters.Organizations != null)
                                gOrganizationAcls = gOrganizationAcls.Where(filters.Organizations);

                            predicateAny = predicateAny.Or(p => gOrganizationAcls.Where(a => a.PermissionID == p.ID).Any());
                            predicateAll = predicateAll.And(p => gOrganizationAcls.Where(a => a.PermissionID == p.ID).All(a => a.Allowed));
                            break;
                        case PermissionAclTypes.OrganizationUsers:
                            var gOrganizationUserAcls = OrganizationUserAcls.AsQueryable().FilterAcl(identity, permission);
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
                            var gProjectOrganizationAcls = ProjectOrganizationAcls.AsQueryable().FilterAcl(identity, permission);
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
                            var gRegistries = RegistryAcls.AsQueryable().FilterAcl(identity, permission);
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
                            var gRequestAcls = RequestAcls.AsQueryable().FilterAcl(identity, permission);
                            if (filters.Requests != null)
                                gRequestAcls = gRequestAcls.Where(filters.Requests);

                            predicateAny = predicateAny.Or(p => gRequestAcls.Where(a => a.PermissionID == p.ID).Any());
                            predicateAll = predicateAll.And(p => gRequestAcls.Where(a => a.PermissionID == p.ID).All(a => a.Allowed));
                            break;
                        case PermissionAclTypes.RequestSharedFolders:
                            var gRequestSharedFolderAcls = RequestSharedFolderAcls.AsQueryable().FilterAcl(identity, permission);
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

            return (from p in await Permissions.AsExpandableEFCore().Where(predicateAny).Where(predicateAll).Select(p => p.ID).ToArrayAsync() join rp in permissions on p equals rp.ID select rp);
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
                        else if (typeof(TEntity) == typeof(Organization))
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

        /// <summary>
        /// Sends the live notifications as necessary after save in a separate thread.
        /// </summary>
        /// <param name="notifications"></param>
        private void Notify(IEnumerable<Notification> notifications)
        {
            if (notifications.Any())
            {                
                var thread = new Thread(() =>
                {
                    var notifier = new Utilities.Logging.Notifier(Serilog.Log.ForContext<Utilities.Logging.Notifier>());
                    notifier.Notify(notifications);
                });
                thread.Start();
            }
        }

        private IEnumerable<AuditLog> LogEntry(ApiIdentity identity, Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry, bool read = false)
        {
            var logs = new List<AuditLog>();
            //If entry has logging configuration, and process, then add those notifications to the notifications list.
            var conf = Logging.FirstOrDefault(l => l.GetType().BaseType.GetGenericArguments()[1] == entry.Entity.GetType());

            if (conf != null)
            {
                var method = conf.GetType().GetMethod("ProcessEvents");
                logs.AddRange((IEnumerable<AuditLog>)method.Invoke(conf, new object[] { entry, this, identity, read }));
            }
            return logs.AsEnumerable();
        }

        public void ForceLog(ApiIdentity identity, object entity)
        {
            ForceLog(identity, new[] { entity });
        }

        public void ForceLog(ApiIdentity identity, IEnumerable<object> entities)
        {
            List<Notification> notifications = new List<Notification>();

            foreach (var entity in entities)
            {
                var entry = this.ChangeTracker.Entries().FirstOrDefault(e => e.Entity == entity);
                if (entry == null)
                    throw new ArgumentOutOfRangeException("The entity could not be found in the change trackker to log the read request.");

                var logs = LogEntry(identity, entry, true);

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

        public async Task<IDbWorkflowActivity> GetWorkflowActivityFromCompletion(Guid workflowID, Guid sourceWorkflowActivityID, Guid resultID)
        {
            var result = await (from cm in WorkflowActivityCompletionMaps where cm.WorkflowID == workflowID && cm.SourceWorkflowActivityID == sourceWorkflowActivityID && cm.WorkflowActivityResultID == resultID select cm.DestinationWorkflowActivity).FirstOrDefaultAsync(); //No longer returns null if end point

            return result;
        }

        public IDbWorkflowActivity GetWorkflowActivityByID(Guid workflowActivityID)
        {
            return WorkflowActivities.Find(workflowActivityID);
        }

        public IQueryable<RequestType> GetProjectAvailableRequestTypes(Guid projectID, Guid identityID)
        {
            //For legacy request types, the user has to have the automatic or manual permission for the request type, which can be set at two levels within the Project (Project level or Project DataMart level)
            var projectDatamartAcls = this.ProjectDataMartRequestTypeAcls.FilterRequestTypeAcl(identityID);
            var projectAcls = this.ProjectRequestTypeAcls.FilterRequestTypeAcl(identityID);

            var results = from rt in this.ProjectRequestTypes
                          let projID = projectID
                          let userID = identityID
                          let editTaskPermissionID = PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditTask.ID
                          let wAcls = this.ProjectRequestTypeWorkflowActivities.Where(a => a.RequestTypeID == rt.RequestTypeID && a.WorkflowActivity.Start == true && a.SecurityGroup.Users.Any(u => u.UserID == userID) &&
                                                                                   a.ProjectID == projID &&
                                                                                   a.PermissionID == editTaskPermissionID).AsEnumerable()
                          let pAcls = projectAcls.Where(pa => pa.ProjectID == projID && pa.RequestTypeID == rt.RequestTypeID).AsEnumerable()
                          let pdmAcls = projectDatamartAcls.Where(pa => pa.ProjectID == projID && pa.RequestTypeID == rt.RequestTypeID).AsEnumerable()
                          where rt.ProjectID == projID
                          && (
                               (rt.RequestType.WorkflowID.HasValue && wAcls.Any(a => a.Allowed) && wAcls.All(a => a.Allowed))
                               ||
                               (rt.RequestType.WorkflowID.HasValue == false &&
                               (pAcls.Any(a => a.Permission > 0) || pdmAcls.Any(a => a.Permission > 0)) &&
                               (pAcls.All(a => a.Permission > 0) && pdmAcls.All(a => a.Permission > 0)))
                          )
                          select rt.RequestType;

            return results;
        }

    }
}
