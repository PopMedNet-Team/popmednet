using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using Lpp.Dns.Data.Documents;
using Lpp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities.Objects;
using Lpp.Utilities.Security;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities.Logging;
using Lpp.Dns.DTO.Events;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.Data
{
    [Table("Documents")]
    public class Document : EntityWithID
    {
        public Document()
        {
            this.CreatedOn = DateTime.UtcNow;
            this.MajorVersion = 1;
            this.MinorVersion = 0;
            this.BuildVersion = 0;
            this.RevisionVersion = 0;
        }

        /// <summary>
        /// Gets or sets the name of the document.
        /// </summary>
        [MaxLength(255), Required, Index]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the filename of the document.
        /// </summary>
        [MaxLength(255), Required, Index]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets if the document is viewable (has a visualizer).
        /// </summary>
        [Column("isViewable")]
        public bool Viewable { get; set; }

        /// <summary>
        /// Gets or sets the mime type of the document.
        /// </summary>
        [Required, MaxLength(100)]
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the document kind.
        /// </summary>
        [MaxLength(50)]
        public string Kind { get; set; }

        /// <summary>
        /// Gets or sets the created on date.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the last time the document content was modified.
        /// </summary>
        public DateTime? ContentModifiedOn { get; set; }

        /// <summary>
        /// Gets or set the length of the document in bytes.
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// Gets or set the ID of the item the document is associated with, ie. Request, Response, Task, etc...
        /// </summary>
        public Guid ItemID { get; set; }

        /// <summary>
        /// Gets or set the description of the document.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the ID of the document the current document is a revision of.
        /// </summary>
        public Guid? ParentDocumentID { get; set; }
        public virtual Document ParentDocument { get; set; }

        /// <summary>
        /// Gets or set the ID of the user who uploaded the document.
        /// </summary>
        public Guid? UploadedByID { get; set; }
        public virtual User UploadedBy {get; set;}

        /// <summary>
        /// Gets or sets an identifier that groups a set of revisions of a specific document.
        /// </summary>
        public Guid? RevisionSetID { get; set; }
        /// <summary>
        /// Gets or set a description of the revision.
        /// </summary>
        public string RevisionDescription { get; set; }  
        /// <summary>
        /// Gets or sets the major version number. Version format: {major}.{minor}.{build}.{revision}
        /// </summary>
        public int MajorVersion { get; set; }
        /// <summary>
        /// Gets or sets the minor version number. Version format: {major}.{minor}.{build}.{revision}
        /// </summary>
        public int MinorVersion { get; set; }
        /// <summary>
        /// Gets or sets the build version number. Version format: {major}.{minor}.{build}.{revision}
        /// </summary>
        public int BuildVersion { get; set; }
        /// <summary>
        /// Gets or sets the revision version number. Version format: {major}.{minor}.{build}.{revision}
        /// </summary>
        public int RevisionVersion { get; set; }

        public virtual ICollection<Document> Documents { get; set; }

        public virtual ICollection<Audit.DocumentChangeLog> DocumentChangeLogs { get; set; }

        /// <summary>
        /// Gets the Data of the document
        /// </summary>
        /// <param name="db">The data context that the document is attached to</param>
        /// <returns></returns>
        public byte[] GetData(DataContext db) 
        {
            using (var stream = new DocumentStream(db, this.ID))
            {
                var buffer = new byte[stream.Length];
                if (this.Length != buffer.Length)
                    this.Length = buffer.Length;

                stream.Read(buffer, 0, buffer.Length);

                return buffer;
            }
        }

        /// <summary>
        /// Returns a stream to read and write to and from
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public DocumentStream GetStream(DataContext db)
        {
            return new DocumentStream(db, this.ID);
        }

        /// <summary>
        /// Returns the string representation of the document
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public string ReadStreamAsString(DataContext db)
        {
            return System.Text.UTF8Encoding.UTF8.GetString(this.GetData(db));
        }

        /// <summary>
        /// Sets the data for the document
        /// </summary>
        /// <param name="db">The Data context that the document is attached to</param>
        /// <param name="data">The Data to write</param>
        public void SetData(DataContext db, byte[] data)
        {
            using (var stream = new DocumentStream(db, this.ID))
            {
                //this.Length = data.Length;
                stream.Write(data, 0, data.Length);
            }

            db.Database.ExecuteSqlCommand($"UPDATE Documents SET ContentModifiedOn = GETUTCDATE() WHERE ID = '{ this.ID.ToString("D") }'");
        }

        public void CopyData(DataContext db, Guid sourceDocumentID)
        {
            using (var sourceStream = new DocumentStream(db, sourceDocumentID))
            {
                using (var destinationStream = new DocumentStream(db, this.ID))
                {
                    sourceStream.CopyTo(destinationStream);
                    this.Length = sourceStream.Length;
                    destinationStream.Flush();
                }
                sourceStream.Flush();
            }

            db.Database.ExecuteSqlCommand($"UPDATE Documents SET ContentModifiedOn = GETUTCDATE() WHERE ID = '{ this.ID.ToString("D") }'");
        }        
    }

    public class FileStreamRowData
    {
        public FileStreamRowData() {}

        public string Path { get; set; }
        public byte[] Transaction { get; set; }
    }

    internal class DocumentConfiguration : EntityTypeConfiguration<Document>
    {
        public DocumentConfiguration()
        {
            Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            HasMany(d => d.Documents).WithOptional(d => d.ParentDocument).HasForeignKey(d => d.ParentDocumentID).WillCascadeOnDelete(false);
            HasMany(d => d.DocumentChangeLogs).WithRequired(d => d.Document).HasForeignKey(d => d.DocumentID).WillCascadeOnDelete(true);
        }
    }

    internal class DocumentSecurityConfiguration : DnsEntitySecurityConfiguration<Document>
    {

        public override IQueryable<Document> SecureList(DataContext db, IQueryable<Document> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Request.ViewResults,
                    PermissionIdentifiers.Request.ViewIndividualResults
                };
            
            return query.Where(d =>
                db.Filter(db.Requests, identity, permissions).Any(r => r.ID == d.ItemID)
                //Add other ors here.
            );
        }

        //These permissions should all be extended to take into account the ItemID and what it's parent is.
        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params Document[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Request.Edit);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Request.Delete); 
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Request.Edit);
        }
    }

    internal class DocumentLogConfiguration : EntityLoggingConfiguration<DataContext, Document>
    {

        public override IEnumerable<AuditLog> ProcessEvents(System.Data.Entity.Infrastructure.DbEntityEntry obj, DataContext db, ApiIdentity identity, bool read)
        {
            var document = obj.Entity as Document;
            if (document == null)
                throw new InvalidCastException("The entity being logged is not a Document.");

            var logs = new List<AuditLog>();

            if ((document.Kind == DTO.Enums.DocumentKind.SystemGeneratedNoLog) || (document.Kind == DTO.Enums.DocumentKind.Request))
            {
                return logs;
            }

            var orgUser = db.Users.Where(u => u.ID == identity.ID).Select(u => new { u.UserName, u.Organization.Acronym }).FirstOrDefault();
            var logItem = new Audit.DocumentChangeLog
            {
                Description = string.Format("{0} '{1}' has been {2} by {3}", document.ParentDocumentID.HasValue ? "Revision of document": "Document", document.Name, obj.State, (orgUser.Acronym + @"\" + orgUser.UserName)),
                Reason = obj.State,
                UserID = identity == null ? Guid.Empty : identity.ID,
                DocumentID = document.ID,
                Document = document
            };

            db.LogsDocumentChange.Add(logItem);
            logs.Add(logItem);

            //check if associated with a request, if so create a document added for request notification
            //Overall request document will have request.ID == document.ItemID, task document will have task.ID == document.ItemID

            PmnTask workflowActivityTask = null;
            var request = db.Requests.Find(document.ItemID);
            if (request == null)
            {
                var taskReference = db.ActionReferences.Include(tr => tr.Task).Where(tr => tr.TaskID == document.ItemID && tr.Type == TaskItemTypes.Request).FirstOrDefault();
                if (taskReference != null)
                {
                    workflowActivityTask = taskReference.Task;
                    request = db.Requests.Find(taskReference.ItemID);
                }
            }
            if (request != null && obj.State == EntityState.Added)
            {                
                logs.Add(
                    db.LogsRequestDocumentChange.Add(
                        new Audit.RequestDocumentChangeLog {
                           Description = string.Format("{0} '{1}' for Request: {2} been has added by {3}", (document.ParentDocumentID.HasValue ? "Revision of document" : "Document"), document.Name, request.Name, (orgUser.Acronym + @"\" + orgUser.UserName)),
                           UserID = identity == null ? Guid.Empty : identity.ID,
                           Reason = obj.State,
                           DocumentID = document.ID,
                           RequestID = request.ID,
                           TaskID = workflowActivityTask == null ? null : (Guid?)workflowActivityTask.ID
                        }
                    )     
                );

            }

            return logs;
        }

        public override IEnumerable<Notification> CreateNotifications<T>(T logItem, DataContext db, bool immediate)
        {
            if (typeof(T) == typeof(Audit.RequestDocumentChangeLog))
            {
                var log = logItem as Audit.RequestDocumentChangeLog;

                db.Entry(log).Reference(l => l.Document).Load();

                if (log.TaskID.HasValue)
                {
                    db.Entry(log).Reference(l => l.Task).Load();

                    if (log.Task.WorkflowActivityID.HasValue)
                    {
                        db.Entry(log.Task).Reference(t => t.WorkflowActivity).Load();
                    }
                }

                var actingUser = db.Users.Where(u => u.ID == log.UserID).FirstOrDefault();

                var details = (from r in db.Requests
                               where r.ID == log.RequestID
                               select new
                               {
                                   ProjectID = r.ProjectID,
                                   ProjectName = r.Project.Name,
                                   OrganizationID = r.OrganizationID,
                                   RequestTypeName = r.RequestType.Name,
                                   RequestTypeID = r.RequestTypeID,
                                   RequestName = r.Name,
                                   CreatedByID = r.CreatedByID
                               }).Single();

                var networkName = db.Networks.Select(n => n.Name).FirstOrDefault();

                var body = GenerateTimestampText(log) +
                           "<p>Here are your most recent <b>New Document Attached</b> notifications from <b>" + networkName + "</b>.</p>" +
                           "<p><b>" + actingUser.FullName + "</b> attached a new document to " +
                           details.RequestTypeName + "</b> request <b>" + details.RequestName + "</b> in project <b>" + details.ProjectName + "</b>.</p>" +
                           "<p><b>Document</b><br/>" +
                           log.Document.Name + "</p>";
                var myBody = GenerateTimestampText(log) +
                           "<p>Here are your most recent <b>New Document Attached</b> to My Request notifications from <b>" + networkName + "</b>.</p>" +
                           "<p><b>" + actingUser.FullName + "</b> attached a new document to your " +
                           details.RequestTypeName + "</b> request <b>" + details.RequestName + "</b> in project <b>" + details.ProjectName + "</b>.</p>" +
                           "<p><b>Document</b><br/>" +
                           log.Document.Name + "</p>";

                //user is not a requestUser and has subscribed to the general notification
                var recipientsQuery = from s in db.UserEventSubscriptions
                                 where s.EventID == EventIdentifiers.Document.Change.ID && !s.User.Deleted && s.User.Active && s.Frequency != null &&
                                 (
                                    (
                                        db.ProjectEvents.Any(a => a.EventID == EventIdentifiers.Document.Change.ID && a.Project.Requests.Any(r => r.ID == log.RequestID) && a.ProjectID == details.ProjectID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID))
                                     || db.OrganizationEvents.Any(a => a.EventID == EventIdentifiers.Document.Change.ID && a.Organization.Requests.Any(r => r.ID == log.RequestID) && a.OrganizationID == details.OrganizationID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID))
                                     || db.ProjectDataMartEvents.Any(a => a.EventID == EventIdentifiers.Document.Change.ID && a.Project.Requests.Any(r => r.ID == log.RequestID) && a.ProjectID == details.ProjectID && a.DataMart.Requests.Any(r => r.RequestID == log.RequestID) && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID))
                                    )
                                    &&
                                    (
                                        db.ProjectEvents.Where(a => a.EventID == EventIdentifiers.Document.Change.ID && a.Project.Requests.Any(r => r.ID == log.RequestID) && a.ProjectID == details.ProjectID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID)).All(a => a.Allowed)
                                     && db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.Document.Change.ID && a.Organization.Requests.Any(r => r.ID == log.RequestID) && a.OrganizationID == details.OrganizationID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID)).All(a => a.Allowed)
                                     && db.ProjectDataMartEvents.Where(a => a.EventID == EventIdentifiers.Document.Change.ID && a.Project.Requests.Any(r => r.ID == log.RequestID) && a.ProjectID == details.ProjectID && a.DataMart.Requests.Any(r => r.RequestID == log.RequestID) && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID)).All(a => a.Allowed)
                                    )
                                 )
                                 &&
                                 (
                                     (
                                        from r in db.Requests.Where(r => r.ID == log.RequestID)
                                        let globalAcls = db.GlobalAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewRequest.ID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID))
                                        let projectAcls = db.ProjectAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewRequest.ID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID) && a.Project.Requests.Any(rq => rq.ID == log.RequestID) && a.ProjectID == details.ProjectID)
                                        let organizationAcls = db.OrganizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewRequest.ID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID) && a.Organization.Requests.Any(rq => rq.ID == log.RequestID) && a.OrganizationID == details.OrganizationID)
                                        where
                                        (
                                            globalAcls.Any() || projectAcls.Any() || organizationAcls.Any()
                                        )
                                        &&
                                        (
                                            globalAcls.All(a => a.Allowed) && projectAcls.All(a => a.Allowed) && organizationAcls.All(a => a.Allowed)
                                        )
                                        select r
                                     ).Any()
                                     ||
                                     db.Requests.Any(r => r.ID == log.RequestID && (r.CreatedByID == s.UserID || r.SubmittedByID == s.UserID))
                                 )
                                 //user is not a request user OR user is a requestUser but has not subscribed to the "My" Notification
                                 && (!db.RequestUsers.Any(ru => ru.RequestID == log.RequestID && ru.UserID == s.UserID)
                                    ||
                                    db.RequestUsers.Any(ru => ru.RequestID == log.RequestID && ru.UserID == s.UserID && s.FrequencyForMy == null && s.Frequency != null)
                                 )
                                 && ((!immediate && s.NextDueTime <= DateTime.UtcNow) || s.Frequency == Frequencies.Immediately)
                                 select s;

                //user is a request user and has subscribed to the "My" notification
                var requestUsersQuery = from s in db.UserEventSubscriptions
                                   where s.EventID == EventIdentifiers.Document.Change.ID && !s.User.Deleted && s.User.Active && s.FrequencyForMy != null &&
                                   (
                                      (
                                          db.ProjectEvents.Any(a => a.EventID == EventIdentifiers.Document.Change.ID && a.Project.Requests.Any(r => r.ID == log.RequestID) && a.ProjectID == details.ProjectID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID))
                                       || db.OrganizationEvents.Any(a => a.EventID == EventIdentifiers.Document.Change.ID && a.Organization.Requests.Any(r => r.ID == log.RequestID) && a.OrganizationID == details.OrganizationID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID))
                                       || db.ProjectDataMartEvents.Any(a => a.EventID == EventIdentifiers.Document.Change.ID && a.Project.Requests.Any(r => r.ID == log.RequestID) && a.ProjectID == details.ProjectID && a.DataMart.Requests.Any(r => r.RequestID == log.RequestID) && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID))
                                      )
                                      &&
                                      (
                                          db.ProjectEvents.Where(a => a.EventID == EventIdentifiers.Document.Change.ID && a.Project.Requests.Any(r => r.ID == log.RequestID) && a.ProjectID == details.ProjectID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID)).All(a => a.Allowed)
                                       && db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.Document.Change.ID && a.Organization.Requests.Any(r => r.ID == log.RequestID) && a.OrganizationID == details.OrganizationID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID)).All(a => a.Allowed)
                                       && db.ProjectDataMartEvents.Where(a => a.EventID == EventIdentifiers.Document.Change.ID && a.Project.Requests.Any(r => r.ID == log.RequestID) && a.ProjectID == details.ProjectID && a.DataMart.Requests.Any(r => r.RequestID == log.RequestID) && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID)).All(a => a.Allowed)
                                      )
                                   )
                                   &&
                                   (
                                       (
                                        from r in db.Requests.Where(r => r.ID == log.RequestID)
                                        let globalAcls = db.GlobalAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewRequest.ID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID))
                                        let projectAcls = db.ProjectAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewRequest.ID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID) && a.Project.Requests.Any(rq => rq.ID == log.RequestID) && a.ProjectID == details.ProjectID)
                                        let organizationAcls = db.OrganizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewRequest.ID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID) && a.Organization.Requests.Any(rq => rq.ID == log.RequestID) && a.OrganizationID == details.OrganizationID)
                                        where
                                        (
                                            globalAcls.Any() || projectAcls.Any() || organizationAcls.Any()
                                        )
                                        &&
                                        (
                                            globalAcls.All(a => a.Allowed) && projectAcls.All(a => a.Allowed) && organizationAcls.All(a => a.Allowed)
                                        )
                                        select r
                                     ).Any()
                                     ||
                                     db.Requests.Any(r => r.ID == log.RequestID && (r.CreatedByID == s.UserID || r.SubmittedByID == s.UserID))
                                   )
                                   && db.RequestUsers.Any(ru => ru.RequestID == log.RequestID && ru.UserID == s.UserID)
                                   && ((!immediate && s.NextDueTimeForMy <= DateTime.UtcNow) || s.FrequencyForMy == Frequencies.Immediately)
                                   select s;
                
                if (log.TaskID.HasValue)
                {
                    //make sure the recipient has permission to view task documents for the specified workflow activity the task is for
                    recipientsQuery = from s in recipientsQuery
                                 let acl = db.ProjectRequestTypeWorkflowActivities.Where(p => p.PermissionID == PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ViewDocuments && 
                                                                                              p.SecurityGroup.Users.Any(u => u.UserID == s.UserID) && 
                                                                                              p.ProjectID == details.ProjectID && 
                                                                                              p.RequestTypeID == details.RequestTypeID && 
                                                                                              p.WorkflowActivityID == db.Actions.Where(t => t.ID == log.TaskID.Value).Select(t => t.WorkflowActivityID).FirstOrDefault()
                                                                                         )
                                 where acl.Any() && acl.All(a => a.Allowed)
                                 select s;

                    requestUsersQuery = from s in requestUsersQuery
                                   let acl = db.ProjectRequestTypeWorkflowActivities.Where(p => p.PermissionID == PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ViewDocuments &&
                                                                                                p.SecurityGroup.Users.Any(u => u.UserID == s.UserID) &&
                                                                                                p.ProjectID == details.ProjectID &&
                                                                                                p.RequestTypeID == details.RequestTypeID &&
                                                                                                p.WorkflowActivityID == db.Actions.Where(t => t.ID == log.TaskID.Value).Select(t => t.WorkflowActivityID).FirstOrDefault()
                                                                                           )
                                   where acl.Any() && acl.All(a => a.Allowed)
                                   select s;
                }

                IList<Notification> notifies = new List<Notification>();

                var recipients = recipientsQuery.Select(s => new Recipient
                                      {
                                          Email = s.User.Email,
                                          Phone = s.User.Phone,
                                          Name = s.User.FirstName + " " + s.User.LastName,
                                          UserID = s.UserID
                                      }).ToArray();

                var userObservers = (from u in db.Users
                                     let requestObservers = db.RequestObservers.Where(obs => obs.RequestID == log.RequestID && obs.EventSubscriptions.Any(e => e.EventID == EventIdentifiers.Document.Change.ID) && (obs.UserID == u.ID || (obs.SecurityGroupID.HasValue && obs.SecurityGroup.Users.Any(sgu => sgu.UserID == u.ID))))
                                     where requestObservers.Any()
                                     select new Recipient
                                     {
                                         Email = u.Email,
                                         Phone = u.Phone,
                                         Name = ((u.FirstName + " " + u.MiddleName).Trim() + " " + u.LastName).Trim(),
                                         UserID = u.ID
                                     }).ToArray();

                var emailObservers = (from o in db.RequestObservers
                                      where o.EventSubscriptions.Any(e => e.EventID == EventIdentifiers.Document.Change.ID)
                                      && o.UserID.HasValue == false && o.SecurityGroupID.HasValue == false
                                      && o.Email != "" && o.RequestID == log.RequestID
                                      select new Recipient
                                      {
                                          Email = o.Email,
                                          Phone = "",
                                          Name = o.DisplayName,
                                          UserID = null
                                      }).ToArray();

                recipients = recipients.Union(userObservers).Union(emailObservers).ToArray();

                if (recipients.Any())
                {
                    notifies.Add(
                            new Notification
                            {
                                Subject = "New Document Attached to Request Notification",
                                Body = body,
                                Recipients =  recipients
                            }
                        );
                }

                var requestUserRecipients = requestUsersQuery.Select(s => new Recipient
                    {
                        Email = s.User.Email,
                        Phone = s.User.Phone,
                        Name = s.User.FirstName + " " + s.User.LastName,
                        UserID = s.UserID
                    }).ToArray();

                if (requestUserRecipients.Any())
                {
                    notifies.Add(
                        new Notification
                        {
                            Subject = "New Document Attached to Your Request Notification",
                            Body = myBody,
                            Recipients = requestUserRecipients
                        }
                    );
                }

                return notifies;
            }

            return null;
        }

        public async override Task<IEnumerable<Notification>> GenerateNotificationsFromLogs(DataContext db)
        {
            var logs = await FilterAuditLog(from l in db.LogsDocumentChange.Include(x => x.Document) select l, db.UserEventSubscriptions, Lpp.Dns.DTO.Events.EventIdentifiers.Document.Change.ID).GroupBy(g => new { g.DocumentID, g.UserID }).ToArrayAsync();

            var notifications = new List<Notification>();
            foreach (var log in logs)
            {
                var notification = CreateNotifications(log.First(), db, false);
                if (notification != null && notification.Any())
                    notifications.AddRange(notification);
            }

            return notifications.AsEnumerable();
        }
    }

    internal class ExtendedDocumentDTOMapping : EntityMappingConfiguration<Document, Lpp.Dns.DTO.ExtendedDocumentDTO>
    {
        public override System.Linq.Expressions.Expression<Func<Document, DTO.ExtendedDocumentDTO>> MapExpression
        {
            get
            {
                return (d) => new Lpp.Dns.DTO.ExtendedDocumentDTO
                {
                    ID = d.ID,
                    Name = d.Name,
                    FileName = d.FileName,
                    MimeType = d.MimeType,
                    Description = d.Description,
                    Viewable = d.Viewable,
                    ItemID = d.ItemID,
                    ItemTitle = null,
                    Kind = d.Kind,
                    Length = d.Length,
                    CreatedOn = d.CreatedOn,
                    ParentDocumentID = d.ParentDocumentID,
                    RevisionDescription = d.RevisionDescription,
                    RevisionSetID = d.RevisionSetID,
                    MajorVersion = d.MajorVersion,
                    MinorVersion = d.MinorVersion,
                    BuildVersion = d.BuildVersion,
                    RevisionVersion = d.RevisionVersion,
                    Timestamp = d.Timestamp,
                    UploadedByID = d.UploadedByID,
                    UploadedBy = d.UploadedBy.UserName
                };
            }
        }
    }
}
