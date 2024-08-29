using Lpp.Dns.DataMart.Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Client.Tests
{
    [TestClass]
    public class DocumentConcurrentUploadTests
    {
        static readonly log4net.ILog logger;

        static DocumentConcurrentUploadTests()
        {
            log4net.Config.XmlConfigurator.Configure();
            logger = log4net.LogManager.GetLogger(typeof(DocumentConcurrentUploadTests));
        }

        readonly DataMart.Lib.NetWorkSetting _networkSetting;

        public DocumentConcurrentUploadTests()
        {
            //string username = "dean";
            //string password = "Password1!";

            //logger.Debug($"Creating network setting: Url=\"https://api-pmndruat.popmednet.org\", Username=\"{username}\", Password=\"{password}\"");
            //_networkSetting = new DataMart.Lib.NetWorkSetting
            //{
            //    NetworkName = "PMN-DR UAT",
            //    HubWebServiceUrl = "https://api-pmndruat.popmednet.org",
            //    Username = username,
            //    EncryptedPassword = password
            //};

            string username = "SystemAdministrator";
            string password = "Password1!";

            logger.Debug($"Creating network setting: Url=\"http://localhost:14586\", Username=\"{username}\", Password=\"{password}\"");
            _networkSetting = new DataMart.Lib.NetWorkSetting
            {
                NetworkName = "Local Dev",
                HubWebServiceUrl = "http://localhost:14586",
                Username = username,
                EncryptedPassword = password
            };
        }

        [TestMethod]
        public void UploadMultipleSmallFiles()
        {
            //Guid requestID = new Guid("63f074ee-cf4c-4838-ae27-aa4700d7ca87");
            //Guid dataMartID = new Guid("84C9440A-0B1F-4EC0-AF75-A85500CC8F16");
            //string sourceFolderPath = @"C:\work\DR-UAT\DP2\v_psu_01_346\msoc";
            string sourceFolderPath = @"C:\work\DR-UAT\DP2\v_psu_01_353\TEST";

            Guid requestID = new Guid("2D35C33F-11D2-47E3-A2D8-AA4700BCCBBA");
            Guid dataMartID = new Guid("6D55D9BA-9787-40DC-B9B7-A49F00C83946");

            List<DTO.DataMartClient.Criteria.DocumentMetadata> documents = new List<DTO.DataMartClient.Criteria.DocumentMetadata>();
            foreach (var file in System.IO.Directory.GetFiles(sourceFolderPath))
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(file);
                Guid documentID = Lpp.Utilities.DatabaseEx.NewGuid();
                documents.Add(
                    new DTO.DataMartClient.Criteria.DocumentMetadata
                    {
                        CurrentChunkIndex = 0,
                        DataMartID = dataMartID,
                        ID = documentID,
                        IsViewable = false,
                        Kind = string.Empty,
                        MimeType = Lpp.Utilities.FileEx.GetMimeTypeByExtension(fi.Name),
                        Name = fi.Name,
                        RequestID = requestID
                    }
                    );

                logger.Debug($"Adding document: {fi.Name} (ID: {documentID})");

            }

            //documents = documents.Skip(12).Take(4).ToList();

            Parallel.ForEach(documents,
                new ParallelOptions { MaxDegreeOfParallelism = 4 },
                (doc) =>
                {
                    string uploadIdentifier = ("[" + Utilities.Crypto.Hash(Guid.NewGuid()) + "]").PadRight(16);

                    System.IO.Stream stream = null;
                    try
                    {
                        //if (cache.Enabled)
                        //{
                        //    stream = cache.GetDocumentStream(Guid.Parse(doc.DocumentID));
                        //}
                        //else
                        //{
                        //    request.Processor.ResponseDocument(requestId, doc.DocumentID, out stream, doc.Size);
                        //}

                        stream = new System.IO.FileStream(System.IO.Path.Combine(sourceFolderPath, doc.Name), System.IO.FileMode.Open);

                        var dto = new DTO.DataMartClient.Criteria.DocumentMetadata
                        {
                            ID = doc.ID,
                            DataMartID = dataMartID,
                            RequestID = requestID,
                            IsViewable = doc.IsViewable,
                            Size = doc.Size,
                            MimeType = doc.MimeType,
                            Kind = doc.Kind,
                            Name = doc.Name,
                            CurrentChunkIndex = 0
                        };

                        DnsServiceManager.PostDocumentChunk(uploadIdentifier, dto, stream, _networkSetting);
                    }
                    finally
                    {
                        stream.Dispose();
                    }

                });


        }
    }
}
