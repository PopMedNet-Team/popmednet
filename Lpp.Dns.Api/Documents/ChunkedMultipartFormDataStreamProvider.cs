using Lpp.Utilities.WebSites.Attributes;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Web;

namespace Lpp.Dns.Api
{
    public class ChunkedMultipartFormDataStreamProvider<T> : MultipartFormDataStreamProvider
    {
        public ChunkedMultipartFormDataStreamProvider(string rootDir, HttpRequest request) : base(rootDir)
        {
            MetaData = Newtonsoft.Json.JsonConvert.DeserializeObject<ChunkMetaData>(request.Params["metadata"]);

            _filename = MetaData.FileName;
        }

        /// <summary>
        /// The Filename of the file sent by the upload control
        /// </summary>
        private string _filename = string.Empty;

        /// <summary>
        /// The Filename of the file sent by the upload control
        /// </summary>
        public string FileName { get { return _filename; } }

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
        /// Gets The Result that should be returned to the Upload Control
        /// </summary>
        /// <returns></returns>
        public UploadChuckResult<T> GetResult()
        {
            return new UploadChuckResult<T>
            {
                uploaded = MetaData.TotalChunks - 1 <= MetaData.ChunkIndex,
                fileUid = MetaData.UploadUid
            };
        }
    }

    /// <summary>
    /// The Result that should be sent back to the Upload Control
    /// </summary>
    [DataContract, SkipJsonResult]
    public class UploadChuckResult<T>
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
        public T Result { get; set; }
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
}