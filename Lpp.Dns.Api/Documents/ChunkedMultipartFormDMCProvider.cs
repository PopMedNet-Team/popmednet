using Lpp.Dns.Data;
using Lpp.Utilities.Security;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using dmc = Lpp.Dns.DTO.DataMartClient;

namespace Lpp.Dns.Api.DataMartClient
{
    /// <summary>
    /// Custom Provider to handle Chunked File uploads specifically for the DataMart Client
    /// </summary>
    public class ChunkedMultipartFormDMCProvider : MultipartFormDataStreamProvider
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="ChunkedMultipartFormDMCProvider"/> class
        /// </summary>
        /// <param name="rootDir">The Root Directory the file chunk is to be written to</param>
        /// <param name="request">The Request Context sent by the Upload Control</param>
        /// <param name="context">The DbContext</param>
        /// <param name="identity">The Api Identity of the User</param>
        public ChunkedMultipartFormDMCProvider(string rootDir, HttpRequest request, DataContext context, ApiIdentity identity) : base(rootDir)
        {
            DocumentMetadata = Newtonsoft.Json.JsonConvert.DeserializeObject<dmc.Criteria.DocumentMetadata>(request.Params["metadata"]);

            if (DocumentMetadata.ID == Guid.Empty) { DocumentMetadata.ID = Utilities.DatabaseEx.NewGuid(); }

            _dataContext = context;

            _identity = identity;
        }

        /// <summary>
        /// Property for the Metadata about the Document sent by the DMC
        /// </summary>
        public dmc.Criteria.DocumentMetadata DocumentMetadata { get; private set; }

        /// <summary>
        /// Read-only Property that determines if we are on the final chunk.
        /// </summary>
        public bool IsFinalChunk
        {
            get
            {
                var size = new DirectoryInfo(this.RootPath).GetFiles(string.Format("{0:D}*.part", DocumentMetadata.ID)).Sum(x => x.Length);
                return size >= DocumentMetadata.Size;
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
        /// The Document stored in the Database or To be stored in the DB.
        /// </summary>
        public Document Doc;

        /// <summary>
        /// Indicates if the document with the ID specified in the metadata exists in the database.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DocumentExists()
        {
            return (await _dataContext.Documents.Where(d => d.ID == DocumentMetadata.ID).CountAsync()) > 0;
        }


        /// <summary>
        /// Gets the FileName to be Used on the FileSystem
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            return GetChunkDocumentFileName(DocumentMetadata.CurrentChunkIndex);
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
                if (string.Equals(UnquoteToken(headers.ContentDisposition.Name), "files", StringComparison.OrdinalIgnoreCase))
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
            Response response = await _dataContext.Responses
                                                 .Where(rsp => rsp.RequestDataMart.DataMartID == DocumentMetadata.DataMartID && rsp.RequestDataMart.RequestID == DocumentMetadata.RequestID)
                                                 .OrderByDescending(rsp => rsp.Count)
                                                 .FirstOrDefaultAsync();

            if (response == null)
            {
                RequestDataMart datamart = await _dataContext.RequestDataMarts.FirstOrDefaultAsync(dm => dm.DataMartID == DocumentMetadata.DataMartID && dm.RequestID == DocumentMetadata.RequestID);

                if (datamart == null)
                {
                    throw new Exception($"Unable to determine the routing for the specified request and datamart. RequestID: { DocumentMetadata.RequestID }, DataMartID: {DocumentMetadata.DataMartID}");
                }

                //create the response
                response = datamart.AddResponse(_identity.ID);
                response.SubmittedOn = DateTime.UtcNow;
            }

            if (_dataContext.Documents.Any(d => d.ItemID == response.ID && d.FileName == DocumentMetadata.Name))
            {
                //Get existing documents with revision 
                var existingDocuments = await _dataContext.Documents.AsNoTracking().Where(d => d.ItemID == response.ID && d.FileName == DocumentMetadata.Name).Select(d => new { d.FileName, d.MajorVersion, d.MinorVersion, d.BuildVersion, d.RevisionVersion, d.ID, d.RevisionSetID }).ToArrayAsync();
                var existingRequestDocuments = await _dataContext.RequestDocuments.AsNoTracking().Where(rd => rd.ResponseID == response.ID).ToListAsync();

                //get the most recent parent document
                var pDoc = existingDocuments.Where(ed => ed.FileName == DocumentMetadata.Name).OrderByDescending(ed => ed.MajorVersion).ThenByDescending(ed => ed.MinorVersion).ThenByDescending(ed => ed.BuildVersion).ThenByDescending(ed => ed.RevisionVersion).First();

                Doc = new Document
                {
                    ID = DocumentMetadata.ID,
                    ItemID = response.ID,
                    FileName = DocumentMetadata.Name,
                    Name = DocumentMetadata.Name,
                    Description = string.Empty,
                    UploadedByID = _identity.ID,
                    MimeType = DocumentMetadata.MimeType,
                    Length = DocumentMetadata.Size,
                    Viewable = DocumentMetadata.IsViewable,
                    Kind = DocumentMetadata.Kind,
                    RevisionSetID = pDoc.RevisionSetID,
                    ParentDocumentID = pDoc.ID,
                    MajorVersion = pDoc.MajorVersion,
                    MinorVersion = pDoc.MinorVersion,
                    BuildVersion = pDoc.BuildVersion,
                    RevisionVersion = pDoc.RevisionVersion + 1
                };
            }
            else
            {
                Doc = new Document
                {
                    ID = DocumentMetadata.ID,
                    ItemID = response.ID,
                    FileName = DocumentMetadata.Name,
                    Name = DocumentMetadata.Name,
                    ParentDocumentID = null,
                    MimeType = DocumentMetadata.MimeType,
                    Length = DocumentMetadata.Size,
                    Viewable = DocumentMetadata.IsViewable,
                    Kind = DocumentMetadata.Kind,
                    Description = string.Empty,
                    UploadedByID = _identity.ID,
                    RevisionSetID = DocumentMetadata.ID
                };
            }

            _dataContext.Documents.Add(Doc);

            //If workflow add to RequestDocuments
            var requestWorkflowActivityID = await _dataContext.Requests.Where(r => r.ID == DocumentMetadata.RequestID).Select(x => x.WorkFlowActivityID).FirstOrDefaultAsync();

            if (requestWorkflowActivityID.HasValue)
            {
                if (!_dataContext.RequestDocuments.Any(rd => rd.ResponseID == Doc.ItemID && rd.RevisionSetID == Doc.RevisionSetID && rd.DocumentType == DTO.Enums.RequestDocumentType.Output))
                {
                    _dataContext.RequestDocuments.Add(new RequestDocument { ResponseID = Doc.ItemID, RevisionSetID = Doc.RevisionSetID.Value, DocumentType = DTO.Enums.RequestDocumentType.Output });
                }
            }

            await _dataContext.SaveChangesAsync();
        }

        /// <summary>
        /// Combines all the uploaded chunks into one File.
        /// </summary>
        /// <returns></returns>
        public async Task CombineChunks()
        {
            using (var combinedFile = new FileStream(CombindedTempDocumentFileName, FileMode.Create, FileAccess.Write))
            {
                for (int i = 0; i <= DocumentMetadata.CurrentChunkIndex; i++)
                {
                    using (var chunkedFile = new FileStream(GetChunkDocumentFileName(i), FileMode.Open, FileAccess.Read))
                    {
                        await chunkedFile.CopyToAsync(combinedFile);
                    }
                }

                await combinedFile.FlushAsync();
            }

            try
            {
                //only cleanup the parts if the combined file was successfuly created
                for (int i = 0; i <= DocumentMetadata.CurrentChunkIndex; i++)
                {
                    File.Delete(GetChunkDocumentFileName(i));
                }
            }
            catch { }
        }

        /// <summary>
        /// Sends the stream directly to the database
        /// </summary>
        /// <returns></returns>
        public async Task StreamDocumentToDatabase()
        {
            if (_dataContext.Database.Connection.State != ConnectionState.Open)
                _dataContext.Database.Connection.Open();

            using (var fs = new FileStream(CombindedTempDocumentFileName, FileMode.Open, FileAccess.Read))
            {

                using (var conn = (SqlConnection)_dataContext.Database.Connection)
                {
                    int chunkSize = 524288000;
                    int bytesRead;
                    byte[] buffer = fs.Length < chunkSize ? new byte[fs.Length] : new byte[chunkSize];
                    while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        using (var cmd = new SqlCommand("UPDATE Documents SET [Data] = CASE WHEN [Data] IS NULL THEN @newData ELSE [Data] + @newData END, [Length] = DATALENGTH(CASE WHEN Data IS NULL THEN @newData ELSE Data + @newData END), ContentModifiedOn = GETUTCDATE(), ContentCreatedOn = CASE WHEN ContentCreatedOn IS NULL THEN GETUTCDATE() ELSE ContentCreatedOn END WHERE ID = @ID", conn))
                        {
                            cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = Doc.ID;
                            cmd.Parameters.Add("@newData", SqlDbType.VarBinary, bytesRead).Value = buffer;

                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = 900;
                            await cmd.ExecuteNonQueryAsync();
                            bytesRead -= chunkSize;
                        }

                    }
                    conn.Close();
                    conn.Dispose();
                }
                fs.Close();
                fs.Dispose();
            }
        }

        /// <summary>
        /// Gets the name of the documents combined parts after all chunks have been uploaded. Format: {DocumentID}_Finished.part
        /// </summary>
        public string CombindedTempDocumentFileName
        {
            get
            {
                return Path.Combine(this.RootPath, string.Format("{0:D}_Finished.part", DocumentMetadata.ID));
            }
        }

        /// <summary>
        /// Gets the name of a document chunk. Format: {DocumentID}_{chunk index}.part
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetChunkDocumentFileName(int index)
        {
            return Path.Combine(this.RootPath, string.Format("{0:D}_{1}.part", DocumentMetadata.ID, index));
        }

        /// <summary>
        /// Performs any cleanup actions required.
        /// </summary>
        public void Cleanup()
        {
            if (File.Exists(CombindedTempDocumentFileName))
            {
                File.Delete(CombindedTempDocumentFileName);
            }
        }
    }
}