using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PopMedNet.DMCS.Code.Files;
using PopMedNet.DMCS.Data.Model;
using PopMedNet.DMCS.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.DMCS.Code
{
    public class CacheManager
    {
        readonly IOptions<DMCSConfiguration> config;
        readonly ILogger logger;
        readonly DataMart dataMart;
        /// <summary>
        /// The ID of the request the cache manager is managing documents for.
        /// </summary>
        public readonly Guid RequestID;
        /// <summary>
        /// The ID of the response the cache manager is managing the documents for.
        /// </summary>
        public readonly Guid ResponseID;  
        /// <summary>
        /// The file path for documents associated with the response.
        /// </summary>
        public readonly string BaseCacheForResponse;
        /// <summary>
        /// The file path for the cached documents associated with the datamart/request
        /// </summary>
        public readonly string BaseCache;

        /**
         * Cache folder format:
         * {root cache folder path}\{DMCS Identifier}\{DataMart ID}\{Request ID}\{Response ID}
         */

        public CacheManager(IOptions<DMCSConfiguration> config, DataMart dataMart, Guid requestID, Guid responseID, ILogger logger)
        {
            this.config = config;
            this.dataMart = dataMart;
            this.RequestID = requestID;
            this.ResponseID = responseID;
            this.logger = logger.ForContext("SourceContext", typeof(CacheManager).FullName);

            BaseCache = Path.Combine(config.Value.Settings.CacheFolder, config.Value.Settings.DMCSIdentifier.ToString("D"), dataMart.ID.ToString("D"), RequestID.ToString("D"));

            BaseCacheForResponse = Path.Combine(BaseCache, ResponseID.ToString("D"));

            if (!Directory.Exists(BaseCacheForResponse))
                Directory.CreateDirectory(BaseCacheForResponse);

            this.logger.Debug($"Cache manager initialized with properties: {{ enabled: {dataMart.CacheDays != 0}, encrypt: { dataMart.EncryptCache }, base path: {BaseCache} }}");
        }

        public bool Enabled
        {
            get
            {
                return this.dataMart.CacheDays != 0;
            }
        }

        public bool CanClearRequestSpecificCache
        {
            get
            {
                return this.dataMart.EnableExplictCacheRemoval;
            }
        }

        public void AddTempFile(DocumentUploadDTO dto)
        {
            var id = dto.UploadID;
            var chunkIndex = dto.CurrentChunk;

            this.logger.AddResponse(this.ResponseID).Information($"Starting uploading chunk {chunkIndex} for '{dto.FileName}', temp document ID '{id.ToString("D")}'");
            string contentPath = Path.Combine(BaseCacheForResponse, GetTempFileName(id, chunkIndex));

            using (var fileStream = new FileStream(contentPath, FileMode.Create, FileAccess.Write))
            {
                dto.File.OpenReadStream().CopyTo(fileStream);
                fileStream.Flush();
            }

            this.logger.AddResponse(this.ResponseID).Information($"Finished uploading chunk {chunkIndex} for '{dto.FileName}', temp document ID '{id.ToString("D")}'");
        }

        public void FinalizeChunks(Guid origFileID, Document document, int TotalChunks)
        {
            bool encrypt = this.dataMart.EncryptCache;

            this.logger.AddResponse(this.ResponseID).Information($"Finalizing all uploaded {TotalChunks} chunks for '{document.Name}', temp document ID '{origFileID.ToString("D")}' that will have a final document ID of '{ document.ID.ToString("D")}'");
            for (int i = 1; i <= TotalChunks; i++)
            {
                var tmpFileName = Path.Combine(BaseCacheForResponse, GetTempFileName(origFileID, i));
                var finalFileName = Path.Combine(BaseCacheForResponse, GetFileName(document.ID, i));

                if (encrypt)
                {
                    using (var tmpFileStream = new FileStream(tmpFileName, FileMode.Open, FileAccess.Read))
                    using (var fileStream = new FileStream(finalFileName, FileMode.Create, FileAccess.Write))
                    using (Stream stream = encrypt ? (Stream)CreateEncryptionStream(fileStream) : fileStream)
                    {
                        tmpFileStream.CopyTo(stream);
                        stream.Flush();
                    }
                    return;
                }

                File.Move(tmpFileName, finalFileName);
            }
        }

        /// <summary>
        /// Returns the content of the document as the IActionResult of the current HttpRequest.
        /// </summary>
        /// <param name="request">The HttpRequest to respond to</param>
        /// <param name="id">The ID of the document to send the contents of</param>
        /// <param name="db">The current database context</param>
        /// <returns></returns>
        public async Task<IActionResult> ReturnDownloadActionResultAsync(HttpRequest request, Guid id, ModelContext db)
        {
            var doc = await (from d in db.Documents
                             join rdoc in db.RequestDocuments on d.RevisionSetID equals rdoc.RevisionSetID
                             where d.ID == id
                             orderby d.Version descending
                             select new { 
                                 Document = d,
                                 DocumentType = rdoc.DocumentType
                             }).FirstOrDefaultAsync();

            if (doc == null)
            {
                return new NotFoundResult();
            }

            List<Stream> streams = new List<Stream>();

            var localDocs = GetDocuments(id);

            if (doc.Document.DocumentState == Data.Enums.DocumentStates.Local && localDocs.Length == 0)
            {
                //check the cache, if does not exist throw error. Since it is local cannot pull from API
                return new NotFoundResult();
            }

            //check if the document exists in the cache, if not download it from the API
            if(localDocs.Length == 0)
            {
                var cookie = request.Cookies.Where(x => x.Key == "DMCS-User").Select(x => x.Value).FirstOrDefault();
                var unEncryptedStringArray = Crypto.DecryptStringAES(cookie, config.Value.Settings.Hash, config.Value.Settings.Key).Split(':');

                using (var web = new HttpClient())
                {
                    var creds = Convert.ToBase64String(Encoding.UTF8.GetBytes(unEncryptedStringArray[0] + ":" + unEncryptedStringArray[1]));
                    web.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", creds);
                    var pmnResponse = await web.GetAsync(config.Value.PopMedNet.ApiServiceURL + "/documents/download?id=" + id.ToString("D"), HttpCompletionOption.ResponseHeadersRead);
                    if (pmnResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return new NotFoundResult();
                    }

                    pmnResponse.EnsureSuccessStatusCode();
                    var resStream = await pmnResponse.Content.ReadAsStreamAsync();

                    this.logger.AddResponse(this.ResponseID).Information($"Starting to download the Request Document from PMN with the document ID of '{id}'");

                    using (var fs = new FileStream(Path.Combine(BaseCacheForResponse, $"{id.ToString("D")}.TMP"), FileMode.CreateNew, FileAccess.ReadWrite))
                    {
                        resStream.CopyTo(fs);
                        fs.Flush();
                        fs.Position = 0;

                        int chunkSize = 25000000;
                        int bytesRead;
                        int chunkIndex = 0;
                        byte[] buffer = fs.Length < chunkSize ? new byte[fs.Length] : new byte[chunkSize];
                        while ((bytesRead = fs.Read(buffer, 0, fs.Length < chunkSize ? Convert.ToInt32(fs.Length) : chunkSize)) > 0)
                        {
                            chunkIndex++;
                            string contentPath = Path.Combine(BaseCacheForResponse, GetTempFileName(id, chunkIndex));

                            var resizedBuffer = new byte[fs.Length < chunkSize ? Convert.ToInt32(fs.Length) : chunkSize];
                            resizedBuffer = buffer;

                            if (fs.Length > chunkSize)
                                Array.Resize(ref resizedBuffer, (chunkSize * chunkIndex) > fs.Length ? Convert.ToInt32(chunkSize - ((chunkSize * chunkIndex) - fs.Length)) : chunkSize);

                            using (var fileStream = new FileStream(contentPath, FileMode.Create, FileAccess.Write))
                            {
                                fileStream.Write(resizedBuffer);
                                fileStream.Flush();
                            }

                            bytesRead -= chunkSize;
                        }
                        FinalizeChunks(id, doc.Document, chunkIndex);
                    }

                    File.Delete(Path.Combine(BaseCacheForResponse, $"{id.ToString("D")}.TMP"));
                }
                localDocs = GetDocuments(id);
            }

            if(localDocs.Length == 0)
            {
                throw new Exception($"Could not download document content from PopMedNet API for document ID: '{ doc.Document.ID }', name: '{ doc.Document.Name }', type: { doc.DocumentType }");
            }

            for (int i = 1; i <= localDocs.Count(); i++)
            {
                var file = new FileStream(Path.Combine(BaseCacheForResponse, GetFileName(id, i)), FileMode.Open, FileAccess.Read);
                streams.Add(file);
            }

            return new FileStreamResult(new CombinationStream(streams), doc.Document.MimeType)
            {
                FileDownloadName = doc.Document.Name,
            };
        }

        public void ClearCache()
        {
            this.logger.Information($"Starting to clear the cache of the folder {BaseCache}");
            foreach (var file in Directory.GetFiles(BaseCache))
            {
                File.Delete(file);
            }
        }

        public void Remove(Document document)
        {
            Remove(document, null);
        }

        public void Remove(Document document, Guid? responseID)
        {
            this.logger.AddResponse(responseID).Information($"Removing file '{document.Name}'(ID: '{document.ID}') from the local file cache");

            var paths = GetDocuments(document.ID);
            foreach (var file in paths)
            {
                File.Delete(file);
            }

            this.logger.AddResponse(responseID).Debug($"Finished removing file '{document.Name}'(ID: '{document.ID}') from the local file cache");
        }

        CryptoStream CreateEncryptionStream(Stream outputStream)
        {
            using (RijndaelManaged aesAlg = new RijndaelManaged())
            {

                using (Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(string.Format("{0}-{1:D}-{2:D}", config.Value.Settings.DMCSIdentifier, this.dataMart.ID, RequestID), RequestID.ToByteArray()))
                {
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                }

                //prepend the IV
                outputStream.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                outputStream.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                CryptoStream encryptor = new CryptoStream(outputStream, aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV), CryptoStreamMode.Write);
                return encryptor;
            }
        }

        CryptoStream CreateDecryptionStream(Stream input)
        {
            using (RijndaelManaged aesAlg = new RijndaelManaged())
            {
                using (Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(string.Format("{0}-{1:D}-{2:D}", config.Value.Settings.DMCSIdentifier, this.dataMart.ID, RequestID), RequestID.ToByteArray()))
                {
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                }
                aesAlg.IV = ReadByteArray(input);

                CryptoStream decryptor = new CryptoStream(input, aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV), CryptoStreamMode.Read);
                return decryptor;
            }
        }

        static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new System.Security.SecurityException("Stream did not contain properly formatted byte array.");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new InvalidOperationException("Could not read byte array properly.");
            }

            return buffer;
        }

        string GetTempFileName(Guid id, int chunkIndex)
        {
            return $"{id.ToString("D")}_{chunkIndex}.TMP";
        }

        string GetFileName(Guid id, int chunkIndex)
        {
            bool encrypt = this.dataMart.EncryptCache;
            return $"{id.ToString("D")}_{chunkIndex}{(encrypt ? ".e" : "")}.part";
        }

        public Stream GetChunkStream(Guid id, int chunkIndex)
        {
            return new FileStream(Path.Combine(BaseCacheForResponse, GetFileName(id, chunkIndex)), FileMode.Open, FileAccess.Read);
        }

        public string[] GetDocuments(Guid id)
        {
            return Directory.GetFiles(this.BaseCacheForResponse, $"{ id.ToString("D") }_*.part");
        }

    }
}
