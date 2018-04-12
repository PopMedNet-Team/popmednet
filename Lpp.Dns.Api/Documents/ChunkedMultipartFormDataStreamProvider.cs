using Lpp.Dns.Data;
using Lpp.Utilities.Security;
using Lpp.Utilities.WebSites.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;

namespace Lpp.Dns.Api
{
    /// <summary>
    /// Custom Provider to handle Chunked File uploads
    /// </summary>
    public class ChunkedMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="ChunkedMultipartFormDataStreamProvider"/> class
        /// </summary>
        /// <param name="rootDir">The Root Directory the file chunk is to be written to</param>
        /// <param name="request">The Request Context sent by the Upload Control</param>
        /// <param name="context">The DbContext</param>
        /// <param name="identity">The Api Identity of the User</param>
        public ChunkedMultipartFormDataStreamProvider(string rootDir, HttpRequest request, DataContext context, ApiIdentity identity) : base(rootDir)
        {
            MetaData = Newtonsoft.Json.JsonConvert.DeserializeObject<ChunkMetaData>(request.Params["metadata"]);
            
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


                _filename = MetaData.FileName; 
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
        /// The Filename of the file sent by the upload control
        /// </summary>
        public string FileName { get { return _filename; } }

        /// <summary>
        /// The Document stored in the Database or To be stored in the DB.
        /// </summary>
        public Document Doc;

        /// <summary>
        /// The Deserialized Metadata sent by Kendo Upload
        /// </summary>
        public ChunkMetaData MetaData;

        /// <summary>
        /// Gets the FileName to be Used on the FileSystem
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            return MetaData.UploadUid + ".part";
        }

        /// <summary>
        /// Gets a Stream for all the file data
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            if (headers == null)
                throw new ArgumentNullException("headers");

            if (!string.IsNullOrWhiteSpace(headers.ContentDisposition.Name))
            {
                if(string.Equals(UnquoteToken(headers.ContentDisposition.Name), "files", StringComparison.OrdinalIgnoreCase))
                {
                    var fileName = this.GetLocalFileName(headers);

                    var file = Path.Combine(this.RootPath, fileName);

                    this.FileData.Add(new MultipartFileData(headers, file));

                    return new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.Write, this.BufferSize, true);
                }
                else
                {
                    return new MemoryStream();
                }
            }

            throw new InvalidOperationException("Did not find required 'Content-Disposition' header field in MIME multipart body part.");
        }

        /// <summary>
        /// Remove bounding quotes on a token if present
        /// </summary>
        /// <param name="token">Token to unquote.</param>
        /// <returns>Unquoted token.</returns>
        private static string UnquoteToken(string token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return token;
            }

            if (token.StartsWith("\"", StringComparison.Ordinal) && token.EndsWith("\"", StringComparison.Ordinal) && token.Length > 1)
            {
                return token.Substring(1, token.Length - 2);
            }

            return token;
        }

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
                else
                    Doc.Kind = null;
            }

            if (Doc.ParentDocumentID.HasValue)
            {
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
                                       TaskItemType = taskReference.Select(tr => (DTO.Enums.TaskItemTypes?)tr.Type).FirstOrDefault()
                                   };
                var version = TaskID != Guid.Empty ? await versionQuery.Where(d => d.ItemID == TaskID).FirstOrDefaultAsync() : await versionQuery.Where(d => d.ItemID == RequestID).FirstOrDefaultAsync();

                if (version != null)
                {
                    Doc.RevisionSetID = version.RevisionSetID;
                    Doc.MajorVersion = version.MajorVersion;
                    Doc.MinorVersion = version.MinorVersion;
                    Doc.BuildVersion = version.BuildVersion;
                    Doc.RevisionVersion = version.RevisionVersion + 1;

                    //if the task item type has not been specified for the upload but the parent document had it specified, inhertit the type.
                    if (_taskItemType == null && version.TaskItemType.HasValue)
                    {
                        _taskItemType = version.TaskItemType.Value;
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
                        using (var cmd = new SqlCommand("UPDATE Documents SET Data = CASE WHEN Data IS NULL THEN @newData ELSE Data + @newData END, ContentModifiedOn = GETUTCDATE() WHERE ID = @ID", conn))
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

        /// <summary>
        /// Gets The Result that should be returned to the Upload Control
        /// </summary>
        /// <returns></returns>
        public UploadChuckResult GetResult()
        {
            return new UploadChuckResult
            {
                uploaded = MetaData.TotalChunks - 1 <= MetaData.ChunkIndex,
                fileUid = MetaData.UploadUid
            }; ;
        }
    }

    /// <summary>
    /// The MetaData sent by the Upload Control
    /// </summary>
    [DataContract]
    public class ChunkMetaData
    {
        /// <summary>
        /// The File UID sent by the upload control
        /// </summary>
        [DataMember]
        public string UploadUid { get; set; }
        /// <summary>
        /// The File Name sent by the upload control
        /// </summary>
        [DataMember]
        public string FileName { get; set; }
        /// <summary>
        /// The File Name without the Extension sent by the upload control
        /// </summary>
        [DataMember]
        public string FileNameWithOutExtension { get { return Path.GetFileNameWithoutExtension(FileName); } }
        /// <summary>
        /// The File Extension sent by the upload control
        /// </summary>
        [DataMember]
        public string FileExtension { get { return Path.GetExtension(FileName); } }
        /// <summary>
        /// The Content Type sent by the upload control
        /// </summary>
        [DataMember]
        public string ContentType { get; set; }
        /// <summary>
        /// The Chunk Index sent by the upload control
        /// </summary>
        [DataMember]
        public long ChunkIndex { get; set; }
        /// <summary>
        /// The Total Chunks sent by the upload control
        /// </summary>
        [DataMember]
        public long TotalChunks { get; set; }
        /// <summary>
        /// The File size sent by the upload control
        /// </summary>
        [DataMember]
        public long TotalFileSize { get; set; }
    }

    /// <summary>
    /// The Result that should be sent back to the Upload Control
    /// </summary>
    [DataContract, SkipJsonResult]
    public class UploadChuckResult
    {
        /// <summary>
        /// Determines if all the chunks have been uploaded
        /// </summary>
        [DataMember]
        public bool uploaded { get; set; }
        /// <summary>
        /// The File UID sent by the upload control
        /// </summary>
        [DataMember]
        public string fileUid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DTO.ExtendedDocumentDTO Document { get; set; }
    }
}