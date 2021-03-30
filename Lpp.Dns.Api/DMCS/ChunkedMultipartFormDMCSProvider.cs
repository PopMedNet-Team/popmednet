using Lpp.Dns.Data;
using Lpp.Dns.DTO.DMCS;
using Lpp.Utilities.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Lpp.Dns.Api.DMCS
{
    public class ChunkedMultipartFormDMCSProvider : MultipartFormDataStreamProvider
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="ChunkedMultipartFormDMCSProvider"/> class
        /// </summary>
        /// <param name="rootDir">The Root Directory the file chunk is to be written to</param>
        /// <param name="request">The Request Context sent by the Upload Control</param>
        /// <param name="context">The DbContext</param>
        /// <param name="identity">The Api Identity of the User</param>
        public ChunkedMultipartFormDMCSProvider(string rootDir, HttpRequest request, DataContext context, ApiIdentity identity) : base(rootDir)
        {
            this.DocumentMetadata = Newtonsoft.Json.JsonConvert.DeserializeObject<DMCSResponseDocument>(request.Params["metadata"]);

            this.db = context;

            this.identity = identity;
        }

        /// <summary>
        /// Property for the Metadata about the Document sent by DMCS
        /// </summary>
        public DMCSResponseDocument DocumentMetadata { get; private set; }

        /// <summary>
        /// Read-only Property that determines if we are on the final chunk.
        /// </summary>
        public bool IsFinalChunk
        {
            get
            {
                return DocumentMetadata.CurrentChunk == DocumentMetadata.TotalChunks;
            }
        }

        /// <summary>
        /// The Db Context 
        /// </summary>
        private DataContext db;

        /// <summary>
        /// The Identity of the User
        /// </summary>
        private ApiIdentity identity;

        /// <summary>
        /// The Document stored in the Database or To be stored in the DB.
        /// </summary>
        public Document Doc;

        /// <summary>
        /// Gets the FileName to be Used on the FileSystem
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            return string.Format("{0:D}_{1}.part", DocumentMetadata.DocumentID, DocumentMetadata.CurrentChunk);
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
            Response response = await db.Responses
                                                 .Where(rsp => rsp.ID == DocumentMetadata.ResponseID)
                                                 .OrderByDescending(rsp => rsp.Count)
                                                 .FirstOrDefaultAsync();

            RequestDataMart datamart = await db.RequestDataMarts.FirstOrDefaultAsync(dm => dm.ID == response.RequestDataMartID);

            if (db.Documents.Any(d => d.ItemID == response.ID && d.FileName == DocumentMetadata.FileName))
            {
                //Get existing documents with revision 
                var existingDocuments = await db.Documents.AsNoTracking().Where(d => d.ItemID == response.ID && d.FileName == DocumentMetadata.FileName).Select(d => new { d.FileName, d.MajorVersion, d.MinorVersion, d.BuildVersion, d.RevisionVersion, d.ID, d.RevisionSetID }).ToArrayAsync();
                var existingRequestDocuments = await db.RequestDocuments.AsNoTracking().Where(rd => rd.ResponseID == response.ID).ToListAsync();

                //get the most recent parent document
                var pDoc = existingDocuments.Where(ed => ed.FileName == DocumentMetadata.FileName).OrderByDescending(ed => ed.MajorVersion).ThenByDescending(ed => ed.MinorVersion).ThenByDescending(ed => ed.BuildVersion).ThenByDescending(ed => ed.RevisionVersion).First();

                Doc = new Document
                {
                    ID = DocumentMetadata.DocumentID,
                    ItemID = response.ID,
                    FileName = DocumentMetadata.FileName,
                    Name = DocumentMetadata.FileName,
                    Description = string.Empty,
                    UploadedByID = identity.ID,
                    MimeType = DocumentMetadata.MimeType,
                    Length = DocumentMetadata.Length,
                    Viewable = true,
                    Kind = "",
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
                    ID = DocumentMetadata.DocumentID,
                    ItemID = response.ID,
                    FileName = DocumentMetadata.FileName,
                    Name = DocumentMetadata.FileName,
                    ParentDocumentID = null,
                    MimeType = DocumentMetadata.MimeType,
                    Length = DocumentMetadata.Length,
                    Viewable = true,
                    Kind = "",
                    Description = string.Empty,
                    UploadedByID = identity.ID,
                    RevisionSetID = DocumentMetadata.DocumentID
                };
            }

            db.Documents.Add(Doc);

            //If workflow add to RequestDocuments
            var requestWorkflowActivityID = await db.Requests.Where(r => r.ID == datamart.RequestID).Select(x => x.WorkFlowActivityID).FirstOrDefaultAsync();

            if (requestWorkflowActivityID.HasValue)
            {
                if (!db.RequestDocuments.Any(rd => rd.ResponseID == Doc.ItemID && rd.RevisionSetID == Doc.RevisionSetID && rd.DocumentType == DTO.Enums.RequestDocumentType.Output))
                {
                    db.RequestDocuments.Add(new RequestDocument { ResponseID = Doc.ItemID, RevisionSetID = Doc.RevisionSetID.Value, DocumentType = DTO.Enums.RequestDocumentType.Output });
                }
            }

            await db.SaveChangesAsync();
        }

        /// <summary>
        /// Combines all the uploaded chunks into one File.
        /// </summary>
        /// <returns></returns>
        public async Task CombineChunks()
        {
            using (var combinedFile = new FileStream(Path.Combine(this.RootPath, string.Format("{0:D}_Finished.part", DocumentMetadata.DocumentID)), FileMode.CreateNew, FileAccess.Write))
            {
                for (int i = 1; i <= DocumentMetadata.TotalChunks; i++)
                {
                    using (var chunkedFile = new FileStream(Path.Combine(this.RootPath, string.Format("{0:D}_{1}.part", DocumentMetadata.DocumentID, i)), FileMode.Open, FileAccess.Read))
                    {
                        await chunkedFile.CopyToAsync(combinedFile);
                    }
                }

                await combinedFile.FlushAsync();
            }

            try
            {
                //only cleanup the parts if the combined file was successfuly created
                for (int i = 1; i <= DocumentMetadata.TotalChunks; i++)
                {
                    File.Delete(Path.Combine(this.RootPath, string.Format("{0:D}_{1}.part", DocumentMetadata.DocumentID, i)));
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
            if (db.Database.Connection.State != ConnectionState.Open)
                db.Database.Connection.Open();

            using (var fs = new FileStream(Path.Combine(this.RootPath, string.Format("{0:D}_Finished.part", DocumentMetadata.DocumentID)), FileMode.Open, FileAccess.Read))
            {

                using (var conn = (SqlConnection)db.Database.Connection)
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

            try
            {
                //cleanup the uploaded file
                File.Delete(Path.Combine(this.RootPath, string.Format("{0:D}_Finished.part", DocumentMetadata.DocumentID)));
            }
            catch { }
        }
    }
}