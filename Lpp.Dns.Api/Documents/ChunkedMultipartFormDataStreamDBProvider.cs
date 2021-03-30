using Lpp.Dns.Data;
using Lpp.Utilities.Security;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Lpp.Dns.Api
{
    /// <summary>
    /// Custom Provider to handle Chunked File uploads
    /// </summary>
    public class ChunkedMultipartFormDataStreamDbProvider<T> : ChunkedMultipartFormDataStreamProvider<T>
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="Lpp.Dns.Api.ChunkedMultipartFormDataStreamDbProvider<typeparamref name="T"/>"/> class
        /// </summary>
        /// <param name="rootDir">The Root Directory the file chunk is to be written to</param>
        /// <param name="request">The Request Context sent by the Upload Control</param>
        /// <param name="context">The DbContext</param>
        /// <param name="identity">The Api Identity of the User</param>
        public ChunkedMultipartFormDataStreamDbProvider(string rootDir, HttpRequest request, DataContext context, ApiIdentity identity) : base(rootDir, request)
        {
            MetaData = Newtonsoft.Json.JsonConvert.DeserializeObject<ChunkMetaData>(request.Params["metadata"]);

            _filename = MetaData.FileName;

            if (MetaData.TotalChunks - 1 <= MetaData.ChunkIndex)
            {
                _dataContext = context;

                _identity = identity;

                if (!string.IsNullOrWhiteSpace(request.Params["taskID"]))
                    Guid.TryParse(request.Params["taskID"], out _taskID);

                if (!string.IsNullOrWhiteSpace(request.Params["requestID"]))
                    Guid.TryParse(request.Params["requestID"], out _requestID);

                if (!string.IsNullOrWhiteSpace(request.Params["documentName"]))
                    _documentName = request.Params["documentName"];

                if (!string.IsNullOrWhiteSpace(request.Params["description"]))
                    _description = request.Params["description"];

                if (!string.IsNullOrWhiteSpace(request.Params["comments"]))
                    _comments = request.Params["comments"];


                if (!string.IsNullOrWhiteSpace(request.Params["parentDocumentID"]))
                {
                    Guid parentID;
                    if (Guid.TryParse(request.Params["parentDocumentID"], out parentID))
                    {
                        _parentDocumentID = parentID;
                    }
                }

                if (!string.IsNullOrWhiteSpace(request.Params["responseID"]))
                    Guid.TryParse(request.Params["responseID"], out _responseID);

                if (!string.IsNullOrWhiteSpace(request.Params["documentKind"]))
                    DocumentKind = request.Params["documentKind"];

                if (!string.IsNullOrWhiteSpace(request.Params["taskItemType"]))
                {
                    DTO.Enums.TaskItemTypes itemType;
                    if (Enum.TryParse<DTO.Enums.TaskItemTypes>(request.Params["taskItemType"], out itemType))
                    {
                        _taskItemType = itemType;
                    }
                }
            }
        }

        /// <summary>
        /// The Db Context 
        /// </summary>
        private DataContext _dataContext;

        /// <summary>
        /// The Identity of the User
        /// </summary>
        private ApiIdentity _identity;

        /// <summary>
        /// The TaskID sent by the upload control
        /// </summary>
        private Guid _taskID = Guid.Empty;

        /// <summary>
        /// The RequestID sent by the upload control
        /// </summary>
        private Guid _requestID = Guid.Empty;

        /// <summary>
        /// The document name sent by the upload control
        /// </summary>
        private string _documentName = string.Empty;

        /// <summary>
        /// The document name sent by the upload control
        /// </summary>
        private string _description = string.Empty;

        /// <summary>
        /// The Filename of the file sent by the upload control
        /// </summary>
        private string _filename = string.Empty;

        /// <summary>
        /// The Comments sent by the upload control
        /// </summary>
        private string _comments = string.Empty;

        /// <summary>
        /// The Parent DocumentID sent by the upload control
        /// </summary>
        private Guid? _parentDocumentID = null;

        /// <summary>
        /// The Task Item Type sent by the upload control
        /// </summary>
        private DTO.Enums.TaskItemTypes? _taskItemType = null;

        /// <summary>
        /// The ResponseID sent by the upload control
        /// </summary>
        private Guid _responseID = Guid.Empty;

        /// <summary>
        /// The DocumentKind sent by the upload control
        /// </summary>
        private string DocumentKind = string.Empty;

        /// <summary>
        /// The TaskID Sent by the Upload Control
        /// </summary>
        public Guid TaskID { get { return _taskID; } }

        /// <summary>
        /// The RequestID Sent by the Upload Control
        /// </summary>
        public Guid RequestID { get { return _requestID; } set { _requestID = value; } }

        /// <summary>
        /// The Response ID Sent by the Upload Control
        /// </summary>
        public Guid ResponseID { get { return _responseID; } }

        /// <summary>
        /// The Document stored in the Database or To be stored in the DB.
        /// </summary>
        public Document Doc;

        /// <summary>
        /// Initiates the Document object to be saved in the database
        /// </summary>
        /// <returns></returns>
        public async Task SetUpDocumentInDatabase()
        {
            Doc = new Document
            {
                Description = _description,
                Name = string.IsNullOrEmpty(_documentName) ? _filename : _documentName,
                FileName = _filename,
                MimeType = Lpp.Utilities.FileEx.GetMimeTypeByExtension(_filename),
                ItemID = _responseID != Guid.Empty ? _responseID : TaskID == Guid.Empty ? RequestID : TaskID,
                Length = MetaData.TotalFileSize,
                RevisionDescription = _comments,
                UploadedByID = _identity.ID,
                ParentDocumentID = _parentDocumentID
            };

            if (DocumentKind != string.Empty)
            {
                if (DocumentKind == "OutputManifest")
                    Doc.Kind = "DistributedRegression.FileList";
                else if (DocumentKind == "TrackingTable")
                    Doc.Kind = "DistributedRegression.TrackingTable";
                else if (DocumentKind == "AttachmentInput")
                    Doc.Kind = "Attachment.Input";
                else
                    Doc.Kind = null;
            }


            if (Doc.ParentDocumentID.HasValue)
            {
                //get all the documents belonging to the same revision set as the parent document, order by version numbers descending
                var versionQuery = from d in _dataContext.Documents
                                   let revisionID = _dataContext.Documents.Where(p => p.ID == _parentDocumentID).Select(p => p.RevisionSetID).FirstOrDefault()
                                   let taskReference = _dataContext.ActionReferences.Where(tr => tr.ItemID == d.ID).DefaultIfEmpty()
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
                                       d.Kind,
                                       TaskItemType = taskReference.Select(tr => (DTO.Enums.TaskItemTypes?)tr.Type).FirstOrDefault()
                                   };

                var currentDocument = await versionQuery.FirstOrDefaultAsync();
                if (currentDocument != null)
                {
                    //associate the revision to the same task as the parent document
                    Doc.ItemID = currentDocument.ItemID;

                    Doc.RevisionSetID = currentDocument.RevisionSetID;
                    Doc.MajorVersion = currentDocument.MajorVersion;
                    Doc.MinorVersion = currentDocument.MinorVersion;
                    Doc.BuildVersion = currentDocument.BuildVersion;
                    Doc.RevisionVersion = currentDocument.RevisionVersion + 1;


                    //if the task item type has not been specified for the upload but the parent document had it specified, inhertit the type.
                    if (_taskItemType == null && currentDocument.TaskItemType.HasValue)
                    {
                        _taskItemType = currentDocument.TaskItemType.Value;
                    }

                    if (string.IsNullOrEmpty(Doc.Kind))
                    {
                        Doc.Kind = currentDocument.Kind;
                    }
                }
            }

            if (!Doc.RevisionSetID.HasValue)
                Doc.RevisionSetID = Doc.ID;

            _dataContext.Documents.Add(Doc);

            if (_responseID != Guid.Empty)
            {
                var requestDoc = new RequestDocument
                {
                    ResponseID = _responseID,
                    RevisionSetID = Doc.ID,
                    DocumentType = DTO.Enums.RequestDocumentType.Output
                };

                _dataContext.RequestDocuments.Add(requestDoc);
            }

            if (_requestID != null && TaskID != Guid.Empty && Doc.Kind == "Attachment.Input" && !Doc.ParentDocumentID.HasValue)
            {
                var routes = await _dataContext.Responses.Where(x => x.RequestDataMart.RequestID == _requestID && x.RequestDataMart.Status != DTO.Enums.RoutingStatus.AwaitingRequestApproval && x.RequestDataMart.Status != DTO.Enums.RoutingStatus.Draft).Select(x => x.ID).ToArrayAsync();

                foreach (var dm in routes)
                {
                    _dataContext.RequestDocuments.Add(new RequestDocument { RevisionSetID = Doc.RevisionSetID.Value, ResponseID = dm, DocumentType = DTO.Enums.RequestDocumentType.AttachmentInput });
                }
            }

            if (_taskItemType.HasValue && TaskID != Guid.Empty)
            {
                _dataContext.ActionReferences.Add(new TaskReference
                {
                    TaskID = TaskID,
                    ItemID = Doc.ID,
                    Type = _taskItemType.Value
                });
            }

            await _dataContext.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(_comments))
            {
                //create a comment associated to the request
                var comment = _dataContext.Comments.Add(new Comment
                {
                    CreatedByID = _identity.ID,
                    CreatedOn = DateTime.UtcNow,
                    Text = _comments,
                    ItemID = _requestID
                });

                //create the comment reference to the document
                _dataContext.CommentReferences.Add(new CommentReference
                {
                    ItemID = Doc.ID,
                    ItemTitle = Doc.FileName,
                    CommentID = comment.ID,
                    Type = DTO.Enums.CommentItemTypes.Document
                });

                if (_taskID != Guid.Empty)
                {
                    //create the comment reference to the task
                    _dataContext.CommentReferences.Add(new CommentReference
                    {
                        ItemID = _taskID,
                        CommentID = comment.ID,
                        Type = DTO.Enums.CommentItemTypes.Task
                    });
                }

                await _dataContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Sends the stream directly to the database
        /// </summary>
        /// <returns></returns>
        public async Task StreamDocumentToDatabase()
        {
            if (_dataContext.Database.Connection.State != ConnectionState.Open)
                _dataContext.Database.Connection.Open();

            using (var fs = new FileStream(Path.Combine(this.RootPath, MetaData.UploadUid + ".part"), FileMode.Open, FileAccess.Read))
            {

                using (var conn = (SqlConnection)_dataContext.Database.Connection)
                {
                    int chunkSize = 524288000;
                    int bytesRead;
                    byte[] buffer = fs.Length < chunkSize ? new byte[fs.Length] : new byte[chunkSize];
                    while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        using (var cmd = new SqlCommand("UPDATE Documents SET Data = CASE WHEN Data IS NULL THEN @newData ELSE Data + @newData END, ContentModifiedOn = GETUTCDATE(), ContentCreatedOn = CASE WHEN ContentCreatedOn IS NULL THEN GETUTCDATE() ELSE ContentCreatedOn END WHERE ID = @ID", conn))
                        {
                            try
                            {
                                cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = Doc.ID;
                                cmd.Parameters.Add("@newData", SqlDbType.VarBinary, bytesRead).Value = buffer;

                                cmd.CommandType = CommandType.Text;
                                cmd.CommandTimeout = 900;
                                await cmd.ExecuteNonQueryAsync();
                                bytesRead -= chunkSize;

                            }
                            catch (Exception ex)
                            {
                                throw;
                            }
                        }

                    }
                    conn.Close();
                    conn.Dispose();
                }
                fs.Close();
                fs.Dispose();
            }


            File.Delete(Path.Combine(this.RootPath, MetaData.UploadUid + ".part"));
        }
    }
}