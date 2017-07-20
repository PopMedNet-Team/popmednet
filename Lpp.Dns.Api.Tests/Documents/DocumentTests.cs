using System;
using System.Diagnostics;
using System.IO;
using Lpp.Dns.Data;
using Lpp.Dns.Data.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lpp.Dns.Api.Tests.Documents
{
    [TestClass]
    public class DocumentTests
    {
        /// <summary>
        /// Tests if the document stream function is working.
        /// </summary>
        [TestMethod]
        public void DocumentsTestReadingStream()
        {
            const string fileString = "The quick brown fox jumps over the lazy dog.";

            using (var db = new DataContext())
            {
                //Create the document
                var document = new Document
                {
                    FileName = "test.txt",
                    Name = "test",
                    MimeType = "text/plain",
                    Viewable = true,
                    ItemID =  Guid.Empty
                };

                db.Documents.Add(document);

                db.SaveChanges();

                try
                {
                    //Our Test file
                    var textBytes = System.Text.Encoding.UTF8.GetBytes(fileString);


                    //Write the file
                    using (var stream = new DocumentStream(db, document.ID))
                    {
                        stream.Write(textBytes, 0, textBytes.Length);
                        stream.Flush();
                    }

                    db.SaveChanges();

                    //Read the file
                    using (var stream = new DocumentStream(db, document.ID))
                    {
                        var bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, bytes.Length);

                        var txt = System.Text.Encoding.UTF8.GetString(bytes);

                        Debug.Assert(txt == fileString);
                    }
                }
                finally
                {
                    db.Documents.Remove(document);
                    db.SaveChanges();
                }
            }
        }
    }
}
