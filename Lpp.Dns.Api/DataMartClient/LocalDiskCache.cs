using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Lpp.Dns.Api.DataMartClient
{
    public class LocalDiskCache
    {
        /// <summary>
        /// An instance of a LocalDiskCache.
        /// </summary>
        public static readonly LocalDiskCache Instance;

        static LocalDiskCache()
        {
            string uploadPath = System.Web.Configuration.WebConfigurationManager.AppSettings["DocumentsUploadFolder"] ?? string.Empty;

            if (string.IsNullOrEmpty(uploadPath))
            {
                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Server != null)
                {
                    uploadPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Uploads/");
                }
                else
                {
                    uploadPath = "App_Data\\Uploads";
                }
            }

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            Instance = new LocalDiskCache(uploadPath);
        }

        readonly string _uploadPath;
        static readonly object _lock = new object();

        private LocalDiskCache(string uploadPath)
        {
            _uploadPath = uploadPath;
        }

        /// <summary>
        /// Reads bytes for a specific document from the database.
        /// </summary>
        /// <param name="dataContext">The DataContext for the database.</param>
        /// <param name="documentID">The ID of the document to read the content for.</param>
        /// <param name="offset">The starting offset to read the content bytes from.</param>
        /// <param name="length">The length of content bytes to read.</param>
        /// <returns>A byte array containing the document content for the specified byte range.</returns>
        public async Task<byte[]> ReadChunk(Data.DataContext dataContext, Guid documentID, int offset, int length)
        {
            string filename = Path.Combine(_uploadPath, documentID.ToString("D") + ".part");

            if (!File.Exists(filename))
            {
                lock (_lock)
                {
                    if (!File.Exists(filename))
                    {
                        string tempFileName = Path.Combine(_uploadPath, "temp_" + documentID.ToString("D") + ".part");
                        using (var fs = new FileStream(tempFileName, FileMode.Append, FileAccess.Write, FileShare.Write))
                        using (var dbStream = new Data.Documents.DocumentStream(dataContext, documentID))
                        {
                            dbStream.CopyTo(fs);
                            fs.Flush();
                            fs.Close();
                        }

                        File.Move(tempFileName, filename);
                    }
                }
            }


            byte[] buffer = new byte[length];
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                stream.Seek(offset, SeekOrigin.Begin);
                await stream.ReadAsync(buffer, 0, buffer.Length);
            }

            return buffer;
        }
    }
}