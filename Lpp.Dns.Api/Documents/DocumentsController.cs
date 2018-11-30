using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities;
using Lpp.Utilities.Security;
using Lpp.Utilities.WebSites.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Lpp.Dns.Api.Documents
{
    /// <summary>
    /// Document specific endpoint.
    /// </summary>
    public class DocumentsController : LppApiController<Lpp.Dns.Data.DataContext>
    {
        static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Gets documents for the specified tasks.
        /// </summary>
        /// <param name="tasks">The tasks.</param>
        /// <param name="filterByTaskItemType"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Lpp.Dns.DTO.ExtendedDocumentDTO> ByTask([FromUri]IEnumerable<Guid> tasks, [FromUri]IEnumerable<DTO.Enums.TaskItemTypes> filterByTaskItemType = null)
        {
            var baseAcls = DataContext.ProjectRequestTypeWorkflowActivities.FilterAcl(Identity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ViewDocuments);
            IEnumerable<Guid> baseA = baseAcls.Where(a => a.Allowed).Select(a => a.WorkflowActivityID);

           var acls = from reference in DataContext.ActionReferences
                       join request in DataContext.Requests.Where(r => r.WorkFlowActivityID.HasValue).Select(r => new { r.ID, WorkflowActivityID = r.WorkFlowActivityID.Value, r.ProjectID, r.RequestTypeID }) on reference.ItemID equals request.ID
                       join acl in baseAcls on new { request.ProjectID, request.RequestTypeID, request.WorkflowActivityID } equals new { acl.ProjectID, acl.RequestTypeID, WorkflowActivityID = acl.WorkflowActivityID }

                       where reference.Type == DTO.Enums.TaskItemTypes.Request 
                       && tasks.Contains(reference.TaskID)
                        && baseA.Contains(reference.Task.WorkflowActivityID.Value)
                       select new { TaskID = reference.Task.ID, request.ID, acl.PermissionID, acl.SecurityGroupID, acl.Allowed };


            var docs = (from d in DataContext.Documents
                        let taskReference = DataContext.ActionReferences.Where(tr => tr.ItemID == d.ID).DefaultIfEmpty()
                        let security = acls.Where(a => d.ItemID == a.TaskID)
                        where security.Any() && security.All(a => a.Allowed)
                        orderby d.ItemID descending, d.RevisionSetID descending, d.CreatedOn descending
                        select new ExtendedDocumentDTO
                        {
                            ID = d.ID,
                            Name = d.Name,
                            FileName = d.FileName,
                            MimeType = d.MimeType,
                            Description = d.Description,
                            Viewable = d.Viewable,
                            ItemID = d.ItemID,
                            ItemTitle = DataContext.Actions.Where(t => t.ID == d.ItemID).Select(t => t.WorkflowActivityID.HasValue ? t.WorkflowActivity.Name : t.Subject).FirstOrDefault(),
                            Kind = d.Kind,
                            Length = d.Length,
                            CreatedOn = d.CreatedOn,
                            ContentCreatedOn = d.ContentCreatedOn,
                            ContentModifiedOn = d.ContentModifiedOn,
                            ParentDocumentID = d.ParentDocumentID,
                            RevisionDescription = d.RevisionDescription,
                            RevisionSetID = d.RevisionSetID,
                            MajorVersion = d.MajorVersion,
                            MinorVersion = d.MinorVersion,
                            BuildVersion = d.BuildVersion,
                            RevisionVersion = d.RevisionVersion,
                            Timestamp = d.Timestamp,
                            UploadedByID = d.UploadedByID,
                            UploadedBy = DataContext.Users.Where(u => u.ID == d.UploadedByID).Select(u => u.UserName).FirstOrDefault(),
                            TaskItemType = taskReference.Select(tr => (DTO.Enums.TaskItemTypes?)tr.Type).FirstOrDefault()
                        }).Distinct();

            if (filterByTaskItemType != null && filterByTaskItemType.Any())
            {
                docs = docs.Where(d => d.TaskItemType.HasValue && filterByTaskItemType.Contains(d.TaskItemType.Value));
            }

            return docs;
        }

        /// <summary>
        /// Returns the most current document for each specified revision set.
        /// </summary>
        /// <param name="revisionSets">The collection of revision set IDs to get the current documents for.</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Lpp.Dns.DTO.ExtendedDocumentDTO> ByRevisionID([FromUri]IEnumerable<Guid> revisionSets)
        {
            if (revisionSets == null)
                revisionSets = Enumerable.Empty<Guid>();

            var docs = from d in DataContext.Documents.AsNoTracking()
                       join x in (
                           DataContext.Documents.Where(dd => revisionSets.Contains(dd.RevisionSetID.Value))
                           .GroupBy(k => k.RevisionSetID)
                           .Select(k => k.OrderByDescending(d => d.MajorVersion).ThenByDescending(d => d.MinorVersion).ThenByDescending(d => d.BuildVersion).ThenByDescending(d => d.RevisionVersion).Select(y => y.ID).FirstOrDefault())
                       ) on d.ID equals x
                       orderby d.ItemID descending, d.RevisionSetID descending, d.CreatedOn descending
                       select new ExtendedDocumentDTO
                       {
                           ID = d.ID,
                           Name = d.Name,
                           FileName = d.FileName,
                           MimeType = d.MimeType,
                           Description = d.Description,
                           Viewable = d.Viewable,
                           ItemID = d.ItemID,
                           ItemTitle = DataContext.Actions.Where(t => t.ID == d.ItemID).Select(t => t.WorkflowActivityID.HasValue ? t.WorkflowActivity.Name : t.Subject).FirstOrDefault(),
                           Kind = d.Kind,
                           Length = d.Length,
                           CreatedOn = d.CreatedOn,
                           ContentCreatedOn = d.ContentCreatedOn,
                           ContentModifiedOn = d.ContentModifiedOn,
                           ParentDocumentID = d.ParentDocumentID,
                           RevisionDescription = d.RevisionDescription,
                           RevisionSetID = d.RevisionSetID,
                           MajorVersion = d.MajorVersion,
                           MinorVersion = d.MinorVersion,
                           BuildVersion = d.BuildVersion,
                           RevisionVersion = d.RevisionVersion,
                           Timestamp = d.Timestamp,
                           UploadedByID = d.UploadedByID,
                           UploadedBy = DataContext.Users.Where(u => u.ID == d.UploadedByID).Select(u => u.UserName).FirstOrDefault()
                       };

            return docs;
        }        

        /// <summary>
        /// Gets the documents for the specified response.
        /// </summary>
        /// <param name="ID">The ID of the response.</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Dns.DTO.ExtendedDocumentDTO> ByResponse([FromUri] IEnumerable<Guid> ID)
        {
            var docs = DataContext.Documents
                .Where(d => ID.Contains(d.ItemID))
                .OrderByDescending(d => d.ItemID).ThenByDescending(d => d.RevisionSetID).ThenByDescending(d => d.CreatedOn)
                .Select(d => new ExtendedDocumentDTO
                {
                    ID = d.ID,
                    Name = d.Name,
                    FileName = d.FileName,
                    MimeType = d.MimeType,
                    Description = d.Description,
                    Viewable = d.Viewable,
                    ItemID = d.ItemID,
                    ItemTitle = DataContext.Actions.Where(t => t.ID == d.ItemID).Select(t => t.WorkflowActivityID.HasValue ? t.WorkflowActivity.Name : t.Subject).FirstOrDefault(),
                    Kind = d.Kind,
                    Length = d.Length,
                    CreatedOn = d.CreatedOn,
                    ContentCreatedOn = d.ContentCreatedOn,
                    ContentModifiedOn = d.ContentModifiedOn,
                    ParentDocumentID = d.ParentDocumentID,
                    RevisionDescription = d.RevisionDescription,
                    RevisionSetID = d.RevisionSetID,
                    MajorVersion = d.MajorVersion,
                    MinorVersion = d.MinorVersion,
                    BuildVersion = d.BuildVersion,
                    RevisionVersion = d.RevisionVersion,
                    Timestamp = d.Timestamp,
                    UploadedByID = d.UploadedByID,
                    UploadedBy = DataContext.Users.Where(u => u.ID == d.UploadedByID).Select(u => u.UserName).FirstOrDefault()
                });

            return docs;
        }

        /// <summary>
        /// Gets documents for a request that are not specific to a task.
        /// </summary>
        /// <param name="requestID">The ID of the request.</param>
        /// <returns></returns>
        [HttpGet, ActionName("GeneralRequestDocuments")]
        public IQueryable<Lpp.Dns.DTO.ExtendedDocumentDTO> GeneralRequestDocuments(Guid requestID)
        {
            var docs = (from d in DataContext.Documents where d.ItemID == requestID orderby d.RevisionSetID descending, d.CreatedOn descending select d).Map<Document, ExtendedDocumentDTO>();

            return docs;
        }

        /// <summary>
        /// Streams the content of the specified document, content is not specified as an attachment.
        /// </summary>
        /// <param name="id">The ID of the document.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> Read(Guid id)
        {
            //TODO:implement security for viewing the content
            Document document = await DataContext.Documents.AsNoTracking().SingleOrDefaultAsync(d => d.ID == id);

            if (document == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Document not found.");
            }

            var content = new StreamContent(document.GetStream(DataContext));
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(document.MimeType);
            content.Headers.ContentLength = document.Length;

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = content
            };
        }

        /// <summary>
        /// Downloads a specific document.
        /// </summary>
        /// <param name="id">The ID of the document.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> Download(Guid id)
        {
            //TODO: implement security for downloading document, use Secure for the select
            Document document = await DataContext.Documents.AsNoTracking().SingleOrDefaultAsync(d => d.ID == id);

            if (document == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Document not found.");
            }

            var content = new StreamContent(document.GetStream(DataContext));
            //using application/octet-stream forces the browser to download rather than try to open if supports document mime type.
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");            
            content.Headers.ContentLength = document.Length;
            content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = document.FileName,
                Size = document.Length
            };

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = content
            };
        }

        /// <summary>
        /// Upload a document.
        /// </summary>
        /// <returns></returns>
        [HttpPost, Obsolete]
        public async Task<HttpResponseMessage> Upload()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Content must be mime multipart.");
            }  

            Guid taskID = Guid.Empty;
            Guid requestID = Guid.Empty;
            string documentName = string.Empty;
            string description = string.Empty;
            string filename = string.Empty;
            string comments = string.Empty;
            Guid? parentDocumentID = null;
            DTO.Enums.TaskItemTypes? taskItemType = null;
            Guid responseID = Guid.Empty;
            string documentKind = string.Empty;

            string uploadPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Uploads/");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var provider = new MultipartFormDataStreamProvider(uploadPath);
            var o = await Request.Content.ReadAsMultipartAsync(provider);

            foreach (var c in o.Contents)
            {
                string parameterName = c.Headers.ContentDisposition.Name.Replace("\"", "");
                switch (parameterName)
                {
                    case "requestID":
                        Guid.TryParse(await c.ReadAsStringAsync(), out requestID);
                        break;
                    case "taskID":
                        Guid.TryParse(await c.ReadAsStringAsync(), out taskID);
                        break;
                    case "taskItemType":
                        DTO.Enums.TaskItemTypes itemType;
                        if (Enum.TryParse<DTO.Enums.TaskItemTypes>(await c.ReadAsStringAsync(), out itemType))
                        {
                            taskItemType = itemType;
                        }
                        break;
                    case "comments":
                        comments = await c.ReadAsStringAsync();
                        break;
                    case "files":
                        filename = Path.GetFileName(c.Headers.ContentDisposition.FileName.Replace("\"", ""));
                        break;
                    case "parentDocumentID":
                        Guid parentID;
                        if (Guid.TryParse(await c.ReadAsStringAsync(), out parentID))
                        {
                            parentDocumentID = parentID;
                        }
                        break;
                    case "description":
                        description = await c.ReadAsStringAsync();
                        break;
                    case "documentName":
                        documentName = await c.ReadAsStringAsync();
                        break;
                    case "responseID":
                        Guid.TryParse(await c.ReadAsStringAsync(), out responseID);
                        break;
                    case "documentKind":
                        documentKind = await c.ReadAsStringAsync();
                        break;
                }
            }

            if (taskID == Guid.Empty && requestID == Guid.Empty && responseID == Guid.Empty)
            {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to determine the documents owning object ID.");
            }

            if (string.IsNullOrEmpty(filename))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Filename is missing.");

            if (requestID == Guid.Empty && taskID != Guid.Empty)
            {
                //Get the requestID based on the task.
                requestID = await DataContext.ActionReferences.Where(tr => tr.TaskID == taskID && tr.Type == DTO.Enums.TaskItemTypes.Request).Select(tr => tr.ItemID).FirstOrDefaultAsync();
            }

            //TODO: check for upload permission
            var stream = new FileStream(o.FileData.First().LocalFileName, FileMode.Open);

            var document = new Document { 
                Description = description,
                Name = string.IsNullOrEmpty(documentName) ? filename : documentName,
                FileName = filename,
                MimeType = Lpp.Utilities.FileEx.GetMimeTypeByExtension(filename),
                ItemID = responseID != Guid.Empty ? responseID : taskID == Guid.Empty ? requestID : taskID,
                Length = stream.Length,               
                RevisionDescription = comments,
                UploadedByID = Identity.ID,
                ParentDocumentID = parentDocumentID
            };
            if(documentKind != string.Empty)
            {
                if (documentKind == "OutputManifest")
                    document.Kind = "DistributedRegression.FileList";
                else if (documentKind == "TrackingTable")
                    document.Kind = "DistributedRegression.TrackingTable";
                else
                    document.Kind = null;
            }

            if (document.ParentDocumentID.HasValue)
            {
                var versionQuery = from d in DataContext.Documents
                               let revisionID = DataContext.Documents.Where(p => p.ID == parentDocumentID).Select(p => p.RevisionSetID).FirstOrDefault()
                               let taskReference = DataContext.ActionReferences.Where(tr => tr.ItemID == d.ID).DefaultIfEmpty()
                               where d.RevisionSetID == revisionID
                               orderby d.MajorVersion descending, d.MinorVersion descending, d.BuildVersion descending, d.RevisionVersion descending
                               select new
                               {
                                   d.ItemID,
                                   d.RevisionSetID,
                                   d.MajorVersion,
                                   d.MinorVersion,
                                   d.BuildVersion,
                                   d.RevisionVersion,
                                   TaskItemType = taskReference.Select(tr => (DTO.Enums.TaskItemTypes?)tr.Type).FirstOrDefault()
                               };
                var version = taskID != Guid.Empty ? await versionQuery.Where(d => d.ItemID == taskID).FirstOrDefaultAsync() : await versionQuery.Where(d => d.ItemID == requestID).FirstOrDefaultAsync();

                if (version != null)
                {
                    document.RevisionSetID = version.RevisionSetID;
                    document.MajorVersion = version.MajorVersion;
                    document.MinorVersion = version.MinorVersion;
                    document.BuildVersion = version.BuildVersion;
                    document.RevisionVersion = version.RevisionVersion + 1;

                    //if the task item type has not been specified for the upload but the parent document had it specified, inhertit the type.
                    if (taskItemType == null && version.TaskItemType.HasValue)
                    {
                        taskItemType = version.TaskItemType.Value;
                    }
                }

            }

            if(!document.RevisionSetID.HasValue)
                document.RevisionSetID = document.ID;

            DataContext.Documents.Add(document);

            if(responseID != Guid.Empty)
            {
                var requestDoc = new RequestDocument
                {
                    ResponseID = responseID,
                    RevisionSetID = document.ID,
                    DocumentType = DTO.Enums.RequestDocumentType.Output
                };

                DataContext.RequestDocuments.Add(requestDoc);
            }

            if (taskItemType.HasValue && taskID != Guid.Empty)
            {
                DataContext.ActionReferences.Add(new TaskReference
                {
                    TaskID = taskID,
                    ItemID = document.ID,
                    Type = taskItemType.Value
                });
            }
            
            await DataContext.SaveChangesAsync();

            DateTime contentCreatedOn = DateTime.UtcNow;
            try
            {
                using (var dbStream = new Dns.Data.Documents.DocumentStream(DataContext, document.ID))
                {
                    await stream.CopyToAsync(dbStream, 50000000);
                    //await dbStream.CopyFromStreamAsync(stream, 50000000);
                }
            }
            finally
            {
                stream.Close();
                stream.Dispose();
                stream = null;

                System.IO.File.Delete(o.FileData.First().LocalFileName);
            }

            await DataContext.Database.ExecuteSqlCommandAsync("UPDATE Documents SET ContentModifiedOn = GETUTCDATE(), ContentCreatedOn = @ContentCreatedOn WHERE ID = @ID", new System.Data.SqlClient.SqlParameter("@ID", document.ID), new System.Data.SqlClient.SqlParameter("@ContentCreatedOn", contentCreatedOn));

            if (!string.IsNullOrWhiteSpace(comments))
            {
                //create a comment associated to the request
                var comment = DataContext.Comments.Add(new Comment
                {
                    CreatedByID = Identity.ID,
                    CreatedOn = DateTime.UtcNow,
                    Text = comments,
                    ItemID = requestID
                });

                //create the comment reference to the document
                DataContext.CommentReferences.Add(new CommentReference
                {
                    ItemID = document.ID,
                    ItemTitle = document.FileName,
                    CommentID = comment.ID,
                    Type = DTO.Enums.CommentItemTypes.Document
                });

                if (taskID != Guid.Empty)
                {
                    //create the comment reference to the task
                    DataContext.CommentReferences.Add(new CommentReference
                    {
                        ItemID = taskID,
                        CommentID = comment.ID,
                        Type = DTO.Enums.CommentItemTypes.Task
                    });
                }

                await DataContext.SaveChangesAsync();
            }

            return Request.CreateResponse(HttpStatusCode.Created, new DTO.ExtendedDocumentDTO {
                ID = document.ID,
                Name = document.Name,
                FileName = document.FileName,
                MimeType = document.MimeType,
                Description = document.Description,
                Viewable = document.Viewable,
                ItemID = document.ItemID,
                ItemTitle = DataContext.Actions.Where(t => t.ID == taskID).Select(t => t.WorkflowActivityID.HasValue ? t.WorkflowActivity.Name : t.Subject).FirstOrDefault(),
                Kind = document.Kind,
                Length = document.Length,
                CreatedOn = document.CreatedOn,
                ContentCreatedOn = document.ContentCreatedOn,
                ContentModifiedOn = document.ContentModifiedOn,
                ParentDocumentID = document.ParentDocumentID,
                RevisionDescription = document.RevisionDescription,
                RevisionSetID = document.RevisionSetID,
                MajorVersion = document.MajorVersion,
                MinorVersion = document.MinorVersion,
                BuildVersion = document.BuildVersion,
                RevisionVersion = document.RevisionVersion,
                Timestamp = document.Timestamp,
                UploadedByID = document.UploadedByID,
                UploadedBy = Identity.UserName   
            });
        }

        /// <summary>
        /// Upload a document.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UploadChunked()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Content must be mime multipart.");
            }

            string uploadPath = System.Web.Configuration.WebConfigurationManager.AppSettings["DocumentsUploadFolder"] ?? string.Empty;
            if(string.IsNullOrEmpty(uploadPath))
                uploadPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Uploads/");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var provider = new ChunkedMultipartFormDataStreamDbProvider<ExtendedDocumentDTO>(uploadPath, HttpContext.Current.Request, DataContext, Identity);

            var o = await Request.Content.ReadAsMultipartAsync(provider);

            var result = o.GetResult();
            if (result.uploaded == false)
            {
                return Request.CreateResponse(HttpStatusCode.Created, result);
            }

            if (o.TaskID == Guid.Empty && o.RequestID == Guid.Empty && o.ResponseID == Guid.Empty)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to determine RequestID, ResponseID, and TaskID");
            }

            if (string.IsNullOrEmpty(o.FileName))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Filename is missing.");

            if (o.RequestID == Guid.Empty && o.TaskID != Guid.Empty)
            {
                //Get the requestID based on the task.
                o.RequestID = await DataContext.ActionReferences.Where(tr => tr.TaskID == o.TaskID && tr.Type == DTO.Enums.TaskItemTypes.Request).Select(tr => tr.ItemID).FirstOrDefaultAsync();
            }
            await o.SetUpDocumentInDatabase();
            
            result.Result = new DTO.ExtendedDocumentDTO
            {
                ID = o.Doc.ID,
                Name = o.Doc.Name,
                FileName = o.Doc.FileName,
                MimeType = o.Doc.MimeType,
                Description = o.Doc.Description,
                Viewable = o.Doc.Viewable,
                ItemID = o.Doc.ItemID,
                ItemTitle = DataContext.Actions.Where(t => t.ID == o.TaskID).Select(t => t.WorkflowActivityID.HasValue ? t.WorkflowActivity.Name : t.Subject).FirstOrDefault(),
                Kind = o.Doc.Kind,
                Length = o.Doc.Length,
                CreatedOn = o.Doc.CreatedOn,
                ContentCreatedOn = o.Doc.ContentCreatedOn,
                ContentModifiedOn = o.Doc.ContentModifiedOn,
                ParentDocumentID = o.Doc.ParentDocumentID,
                RevisionDescription = o.Doc.RevisionDescription,
                RevisionSetID = o.Doc.RevisionSetID,
                MajorVersion = o.Doc.MajorVersion,
                MinorVersion = o.Doc.MinorVersion,
                BuildVersion = o.Doc.BuildVersion,
                RevisionVersion = o.Doc.RevisionVersion,
                Timestamp = o.Doc.Timestamp,
                UploadedByID = o.Doc.UploadedByID,
                UploadedBy = Identity.UserName
            };

            await o.StreamDocumentToDatabase();

            return Request.CreateResponse(HttpStatusCode.Created, result);
        }


        /// <summary>
        /// Delete the specified documents.
        /// </summary>
        /// <param name="id">The ID's of the documents to delete.</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task Delete([FromUri] IEnumerable<Guid> id)
        {
            try
            {
                

                //TODO: check for delete permission

                //if (!await DataContext.CanDelete<DataContext, Document, PermissionDefinition>(Identity, id.ToArray()))
                //    throw new SecurityException("We're sorry but you do not have permission to delete one or more of these items.");


                var taskReferences = (from tr in DataContext.ActionReferences where id.Contains(tr.ItemID) select tr);
                DataContext.Set<TaskReference>().RemoveRange(taskReferences);

                var commentReferences = (from cr in DataContext.CommentReferences where id.Contains(cr.ItemID) select cr);
                DataContext.Set<CommentReference>().RemoveRange(commentReferences);

                var dbSet = DataContext.Set<Document>();
                var objs = (from o in dbSet where id.Contains(o.ID) select o);

                foreach (var obj in objs)
                {
                    dbSet.Remove(obj);
                }

                

                await DataContext.SaveChangesAsync();
            }
            catch (System.Security.SecurityException se)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, se));
            }
            catch (DbUpdateException dbe)
            {
                Exception exception = dbe;
                while (exception.InnerException != null)
                    exception = exception.InnerException;

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, exception));
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        private void AppendToFile(string fullPath, Stream content)
        {
            try
            {
                using (FileStream stream = new FileStream(fullPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (content)
                    {
                        content.CopyTo(stream);
                    }
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

    }
}
