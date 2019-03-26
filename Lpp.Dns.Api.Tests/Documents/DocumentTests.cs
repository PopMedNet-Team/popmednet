using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Data;
using System.Linq;
using System.Data.Entity;
using Lpp.Dns.Data;
using Lpp.Dns.Data.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lpp.Dns.Api.Tests.Documents
{
    [TestClass]
    public class DocumentTests
    {

        static DocumentTests()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

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


        [TestMethod]
        public async Task ConcurrentDownloadOfDocument()
        {
            DocumentDetails document = null;
            using (var db = new DataContext()) {
                document = await db.Documents.OrderByDescending(d => d.Length).Select(d => new DocumentDetails { ID = d.ID, Length = d.Length }).FirstOrDefaultAsync();
            }

            if (!Directory.Exists("DownloadsTest"))
            {
                Directory.CreateDirectory("DownloadsTest");
            }

            foreach(var file in Directory.GetFiles("DownloadsTest"))
            {
                File.Delete(file);
            }

            System.Collections.Generic.List<Task> tasks = new System.Collections.Generic.List<Task>();

            for(int i = 0; i < 10; i++)
            {
                tasks.Add( DownloadDocument(document.ID, document.Length, i));
            }

            await Task.WhenAll(tasks.ToArray());
        }

        async Task DownloadDocument(Guid documentID, long documentLength, int index)
        {
            var controller = new Api.DataMartClient.DMCController();

            var response = await controller.GetDocumentChunk(documentID, 0, Convert.ToInt32(documentLength));

            using (var input = await response.Content.ReadAsStreamAsync())
            using (var output = new FileStream("DownloadsTest\\downloadtest-" + index + ".data", FileMode.Create))
            {
                await input.CopyToAsync(output);
                input.Flush();
                output.Flush();
            }
        }

        [TestMethod]
        public async Task DownloadSpecificDocument()
        {
            Guid documentID = new Guid("12D6B147-3FA6-400A-9F9A-A9C900F6F88C");

            using(var db = new DataContext())
            {
                string filename = db.Documents.Where(d => d.ID == documentID).Select(d => d.FileName).Single();

                using (var dbStream = new Data.Documents.DocumentStream(db, documentID))
                using (var fileStream = System.IO.File.Create(Path.Combine("Playpen", filename)))
                {
                    await dbStream.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                }
            }
            
        }

        [TestMethod]
        public async Task OverwriteDocumentContent()
        {
            Guid documentID = new Guid("B7024973-02F7-4477-AABB-A9C900F71CDE");
            string filename = "response.json";

            using (var db = new DataContext())
            {
                using (var dbStream = new Data.Documents.DocumentStream(db, documentID))
                using (var fileStream = System.IO.File.OpenRead(Path.Combine("Playpen", filename)))
                {
                    await dbStream.WriteStreamAsync(fileStream);
                }
            }

            Console.WriteLine("Finished updating document content.");
        }

        internal class DocumentDetails
        {
            public Guid ID { get; set; }

            public long Length { get; set; }
        }
    }
}
