using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities.WebSites.Controllers;
using Lpp.Utilities;
using Lpp.Dns.DTO.Enums;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Lpp.Dns.Api.Comments
{
    /// <summary>
    /// Controller that services Comments' action.
    /// </summary>
    public class CommentsController : LppApiDataController<Comment, CommentDTO, DataContext, PermissionDefinition>
    {
        /// <summary>
        /// Gets all comments associated to the specified Request.
        /// </summary>
        /// <param name="requestID">The ID of the request.</param>
        /// <param name="workflowActivityID">The optional ID of the specific workflow activity to get the comments for.</param>
        /// <returns>WFCommentDTOs</returns>
        [HttpGet]
        public IEnumerable<WFCommentDTO> ByRequestID(Guid requestID, Guid? workflowActivityID = null)
        {
            //if the comment is associated to a specific task, the user needs to have permission to view comments for that task
            //if the comment is an overall request comment, the user needs to have permission to view the overview for the request
            //the ItemID on the comment will always be the request ID
            //overall comments will not have a task associated with it

            var commentAcls = DataContext.ProjectRequestTypeWorkflowActivities.FilterAcl(Identity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ViewComments);
            var overviewAcls = DataContext.ProjectRequestTypeWorkflowActivities.FilterAcl(Identity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ViewRequestOverview);

            var comments = from c in DataContext.Comments
                           join request in DataContext.Requests on c.ItemID equals request.ID
                           from taskRefs in c.References.Where(r => r.Type == CommentItemTypes.Task).DefaultIfEmpty()
                           from task in DataContext.Actions.Where(t => t.ID == taskRefs.ItemID).DefaultIfEmpty()
                           //for a task comment, permission is based on the activity associated to the task
                           let acls = commentAcls.Where(a => a.RequestTypeID == request.RequestTypeID && a.ProjectID == request.ProjectID && a.WorkflowActivityID == task.WorkflowActivityID)
                           //for general comment, permission is based on the current activity of the request if they can view the overview
                           let oAcls = overviewAcls.Where(a => a.RequestTypeID == request.RequestTypeID && a.ProjectID == request.ProjectID && a.WorkflowActivityID == request.WorkFlowActivityID)
                           where request.ID == requestID
                           && (
                                (taskRefs == null && oAcls.Any() && oAcls.All(a => a.Allowed))
                                ||
                                (taskRefs != null && acls.Any() && acls.All(a => a.Allowed))
                           )
                           //if a workflow activity is specified limit to the associated tasks
                           && ((workflowActivityID.HasValue && task != null && task.WorkflowActivityID == workflowActivityID) || workflowActivityID.Value == null)
                           orderby c.CreatedOn descending
                           select new WFCommentDTO
                           {
                               ID = c.ID,
                               Timestamp = c.Timestamp,
                               CreatedBy = c.CreatedBy.UserName,
                               CreatedByID = c.CreatedByID,
                               CreatedOn = c.CreatedOn,
                               Comment = c.Text,
                               RequestID = c.ItemID,
                               TaskID = task.ID,
                               WorkflowActivity = task.WorkflowActivity.Name,
                               WorkflowActivityID = task.WorkflowActivityID
                           };

            return comments;


            //if (workflowActivityID == null)
            //{
            //    //No workflow, so global.
            //    var overviewAcls = DataContext.ProjectRequestTypeWorkflowActivities.FilterAcl(Identity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ViewRequestOverview);

            //    //var comments = from c in DataContext.Comments
            //    //               join request in DataContext.Requests on c.ItemID equals request.ID
            //    //               let oAcls = overviewAcls.Where(a => a.RequestTypeID == request.RequestTypeID && a.ProjectID == request.ProjectID && a.WorkflowActivityID == request.WorkFlowActivityID)
            //    //               where request.ID == requestID && oAcls.Any() && oAcls.All(a => a.Allowed)
            //    //               orderby c.CreatedOn descending
            //    //               select new WFCommentDTO
            //    //               {
            //    //                   ID = c.ID,
            //    //                   Timestamp = c.Timestamp,
            //    //                   CreatedBy = c.CreatedBy.UserName,
            //    //                   CreatedByID = c.CreatedByID,
            //    //                   CreatedOn = c.CreatedOn,
            //    //                   Comment = c.Text,
            //    //                   RequestID = c.ItemID,
            //    //                   TaskID = null,
            //    //                   WorkflowActivity = request.WorkflowActivity.Name,
            //    //                   WorkflowActivityID = request.WorkFlowActivityID
            //    //               };

            //    var baseAcls = DataContext.ProjectRequestTypeWorkflowActivities.FilterAcl(Identity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ViewComments);

            //    var comments = from c in DataContext.Comments
            //                   from taskRefs in c.References.Where(r => r.Type == CommentItemTypes.Task).DefaultIfEmpty()
            //                   from task in DataContext.Actions.Where(t => t.ID == taskRefs.ItemID).DefaultIfEmpty()
            //                   join request in DataContext.Requests on c.ItemID equals request.ID
            //                   let acls = baseAcls.Where(a => a.RequestTypeID == request.RequestTypeID && a.ProjectID == request.ProjectID)
            //                   where c.ItemID == requestID
            //                   orderby c.CreatedOn descending
            //                   select new WFCommentDTO
            //                   {
            //                       ID = c.ID,
            //                       Timestamp = c.Timestamp,
            //                       CreatedBy = c.CreatedBy.UserName,
            //                       CreatedByID = c.CreatedByID,
            //                       CreatedOn = c.CreatedOn,
            //                       Comment = c.Text,
            //                       RequestID = c.ItemID,
            //                       TaskID = (Guid?)taskRefs.ItemID,
            //                       WorkflowActivity = task.WorkflowActivity.Name,
            //                       WorkflowActivityID = task.WorkflowActivityID
            //                   };

            //    return comments;

            //}
            //else
            //{
            //    var baseAcls = DataContext.ProjectRequestTypeWorkflowActivities.FilterAcl(Identity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ViewComments);

            //    var comments = from c in DataContext.Comments
            //                   join cr in DataContext.CommentReferences.Where(r => r.Type == CommentItemTypes.Task) on c.ID equals cr.CommentID
            //                   join task in DataContext.Actions on cr.ItemID equals task.ID
            //                   join request in DataContext.Requests on c.ItemID equals request.ID
            //                   let acls = baseAcls.Where(a => a.RequestTypeID == request.RequestTypeID && a.ProjectID == request.ProjectID && a.WorkflowActivityID == task.WorkflowActivityID)
            //                   where request.ID == requestID && acls.Any() && acls.All(a => a.Allowed)
            //                   orderby c.CreatedOn descending
            //                   select new WFCommentDTO
            //                   {
            //                       ID = c.ID,
            //                       Timestamp = c.Timestamp,
            //                       CreatedBy = c.CreatedBy.UserName,
            //                       CreatedByID = c.CreatedByID,
            //                       CreatedOn = c.CreatedOn,
            //                       Comment = c.Text,
            //                       RequestID = c.ItemID,
            //                       TaskID = task.ID,
            //                       WorkflowActivity = task.WorkflowActivity.Name,
            //                       WorkflowActivityID = task.WorkflowActivityID
            //                   };

            //    return comments;
            //}
        }

        /// <summary>
        /// Gets WFComments based on the document ID specified.
        /// </summary>
        /// <param name="documentID">The ID of the document.</param>
        /// <returns>WFCommentDTOs</returns>
        [HttpGet]
        public IEnumerable<WFCommentDTO> ByDocumentID(Guid documentID)
        {
            var comments = from c in DataContext.Comments
                           from taskRefs in c.References.Where(r => r.Type == CommentItemTypes.Task).DefaultIfEmpty()
                           from task in DataContext.Actions.Where(t => t.ID == taskRefs.ItemID).DefaultIfEmpty()
                           where c.References.Any(r => r.ItemID == documentID && r.Type == CommentItemTypes.Document)
                           orderby c.CreatedOn descending
                           select new WFCommentDTO
                           {
                               ID = c.ID,
                               Timestamp = c.Timestamp,
                               CreatedBy = c.CreatedBy.UserName,
                               CreatedByID = c.CreatedByID,
                               CreatedOn = c.CreatedOn,
                               Comment = c.Text,
                               RequestID = c.ItemID,
                               TaskID = taskRefs.ItemID,
                               WorkflowActivity = task.WorkflowActivity.Name,
                               WorkflowActivityID = task.WorkflowActivityID
                           };

            return comments;
        }

        /// <summary>
        /// Gets document details for all comments associated with the specified request and optionally limited to comments for a specific workflow activity.
        /// </summary>
        /// <param name="requestID">The ID of the request.</param>
        /// <param name="workflowActivityID">The optional ID of the workflow activity.</param>
        /// <returns>CommentDocumentReferenceDTOs</returns>
        [HttpGet]
        public IEnumerable<CommentDocumentReferenceDTO> GetDocumentReferencesByRequest(Guid requestID, Guid? workflowActivityID = null)
        {
            var query = from commentReference in DataContext.CommentReferences
                        from document in DataContext.Documents.Where(d => d.ID == commentReference.ItemID).DefaultIfEmpty()
                        where commentReference.Comment.ItemID == requestID && commentReference.Type == CommentItemTypes.Document                        
                        select new { commentReference, document };            

            if (workflowActivityID.HasValue)
            {
                //make sure that the comment has a reference to a task that belongs to the request and has the specified workflow activity id
                query = query.Where(x => DataContext.Actions.Where(a => 
                            a.WorkflowActivityID == workflowActivityID.Value
                            //make sure the comment has a refernence to the task
                            && x.commentReference.Comment.References.Any(c => c.Type == CommentItemTypes.Task && c.ItemID == a.ID)
                            //make sure the task has a reference to the request
                            && a.References.Any(b => b.ItemID == requestID && b.Type == TaskItemTypes.Request)).Any()
                        );
            }

            var result = query.Select(s => new CommentDocumentReferenceDTO {
                                                    CommentID = s.commentReference.CommentID,
                                                    FileName = s.commentReference.ItemTitle,
                                                    //using the itemtitle from the reference incase the document has been deleted.
                                                    //That way the user can still see that a document had been uploaded, but is not available for download.
                                                    DocumentID = s.document.ID,
                                                    DocumentName = s.document.Name,                                                    
                                                    RevisionSetID = s.document.RevisionSetID.Value
                                                });

            return result;
        }

        /// <summary>
        /// Adds a comment for the specified request and workflow activity.
        /// </summary>
        /// <param name="value">The details of the new comment.</param>
        /// <returns>A WFCommentDTO containing the details of the new comment.</returns>
        [HttpPost]
        public async Task<WFCommentDTO> AddWorkflowComment(DTO.AddWFCommentDTO value)
        {
            if (string.IsNullOrWhiteSpace(value.Comment))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "A comment is required."));
            }

            var comment = DataContext.Comments.Add(new Comment {
                CreatedByID = Identity.ID,
                ItemID = value.RequestID,
                Text = value.Comment
            });

            Guid workflowActivityID = value.WorkflowActivityID.HasValue ? value.WorkflowActivityID.Value : Guid.Empty;
            var task = await DataContext.ActionReferences.Where(tr => tr.ItemID == value.RequestID && tr.Type == TaskItemTypes.Request && tr.Task.WorkflowActivityID == workflowActivityID).Select(tr => new { TaskID = tr.TaskID, WorkflowActivityName = tr.Task.WorkflowActivity.Name }).FirstOrDefaultAsync();
            
            if (task != null)
            {
                DataContext.CommentReferences.Add(new CommentReference
                {
                    CommentID = comment.ID,
                    Type = CommentItemTypes.Task,
                    ItemID = task.TaskID,
                    ItemTitle = task.WorkflowActivityName
                });
            }

            await DataContext.SaveChangesAsync();

            var result = new WFCommentDTO
            {
                Comment = comment.Text,
                CreatedByID = comment.CreatedByID,
                CreatedBy = Identity.UserName,
                CreatedOn = comment.CreatedOn,
                ID = comment.ID,
                RequestID = comment.ItemID,
                Timestamp = comment.Timestamp                
            };

            if (task != null)
            {
                result.TaskID = task.TaskID;
                result.WorkflowActivity = task.WorkflowActivityName;
                result.WorkflowActivityID = value.WorkflowActivityID;
            }

            return result;
        }

        //[HttpPost]
        //public override async Task<IEnumerable<CommentDTO>> InsertOrUpdate(IEnumerable<CommentDTO> values)
        //{
        //    var user = DataContext.Users.Where(u => u.ID == Identity.ID).FirstOrDefault();
        //    var value = values.FirstOrDefault();
        //    var comment = new Comment
        //        {
        //            CreatedBy = user,
        //            CreatedOn = DateTime.UtcNow,
        //            ItemID = value.ItemID,
        //            Text = value.Comment
        //        };

        //    DataContext.Comments.Add(comment);

        //    await DataContext.SaveChangesAsync();

        //    IList<CommentDTO> commentDtos = new List<CommentDTO>();
        //    commentDtos.Add(new CommentDTO
        //    {
        //        Comment = comment.Text,
        //        CreatedByID = comment.CreatedByID,
        //        ID = comment.ID,
        //        ItemID = comment.ItemID,
        //        ItemTitle = DataContext.Actions.Where(t => t.ID == comment.ItemID).Select(t => t.Subject).FirstOrDefault(),
        //        CreatedOn = comment.CreatedOn
        //    });

        //    return commentDtos.AsEnumerable();
        //    //return await base.InsertOrUpdate(values);
        //}
    }
}