using ICSharpCode.SharpZipLib.Zip;
using Lpp.Dns.Data;
using Lpp.Dns.Data.Documents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;


namespace Lpp.Dns.General
{
    public class ZipHelper
    {
        // This will accumulate each of the files named in the zipFileList into a zip file,
        // and stream it to the browser.
        // This approach writes directly to the Response OutputStream.
        // The browser starts to receive data immediately which should avoid timeout problems.
        // This also avoids an intermediate memorystream, saving memory on large files.
        //
        public static void DownloadZipToBrowser(HttpContext httpContext, IEnumerable<KeyValuePair<string, Document>> zipFileList, string outputFileName)
        {
            httpContext.Response.ContentType = "application/zip";
            // If the browser is receiving a mangled zipfile, IIS Compression may cause this problem. Some members have found that
            //    Response.ContentType = "application/octet-stream"     has solved this. May be specific to Internet Explorer.

            httpContext.Response.AppendHeader("content-disposition", "attachment; filename=" + outputFileName);
            httpContext.Response.CacheControl = "Private";
            httpContext.Response.Cache.SetExpires(DateTime.Now.AddMinutes(3)); // or put a timestamp in the filename in the content-disposition

            //byte[] buffer = new byte[4096];

            ZipOutputStream zipOutputStream = new ZipOutputStream(HttpContext.Current.Response.OutputStream);
            zipOutputStream.SetLevel(3); //0-9, 9 being the highest level of compression
            using (var db = new DataContext())
            {
                foreach (var dmd in zipFileList)
                {

                    //string fileName = GetFileNameWithoutPath(dmd.Value.Name);
                    string fileName = Path.GetFileName(dmd.Value.Name);
                    ZipEntry entry = new ZipEntry((string.IsNullOrEmpty(dmd.Key) ? "" : dmd.Key + @"\") + fileName);

                    //// Setting the Size provides WinXP built-in extractor compatibility,
                    ////  but if not available, you can set zipOutputStream.UseZip64 = UseZip64.Off instead.
                    zipOutputStream.PutNextEntry(entry);
                    //zipOutputStream.Write(FileContent, 0, FileContent.Length);
                    int byteCount = 0;
                    Byte[] buffer = new byte[4096];

                    using (Stream inputStream = new DocumentStream(db, dmd.Value.ID))
                    {
                        byteCount = inputStream.Read(buffer, 0, buffer.Length);
                        while (byteCount > 0)
                        {
                            zipOutputStream.Write(buffer, 0, byteCount);
                            byteCount = inputStream.Read(buffer, 0, buffer.Length);
                        }
                    }
                    if (!httpContext.Response.IsClientConnected)
                    {
                        break;
                    }
                    httpContext.Response.Flush();
                }

                zipOutputStream.Close();

                httpContext.Response.Flush();
                httpContext.Response.End();
            }
        }

        //private static string GetFileNameWithoutPath(string FileNameWithPath)
        //{
        //    string fileName = FileNameWithPath;

        //    int pos = FileNameWithPath.LastIndexOf(@"\");
        //    if (pos <= 0) pos = FileNameWithPath.LastIndexOf(@"/");
        //    if (pos > 0) fileName = FileNameWithPath.Substring(pos + 1);

        //    return fileName;
        //}

    }
}
