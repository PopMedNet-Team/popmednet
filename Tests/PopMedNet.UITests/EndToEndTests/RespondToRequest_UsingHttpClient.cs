using Lpp.Dns.DTO.DataMartClient;
using Lpp.Dns.DTO.DataMartClient.Criteria;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using PopMedNet.UITests.Enums;
using PopMedNet.UITests.Models;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Lpp.Dns.DTO.DataMartClient.Enums;

namespace PopMedNet.UITests.EndToEndTests
{
    public class RespondToRequest_UsingHttpClient
    {
        static readonly HttpClient client = new HttpClient();
        IPlaywright playwright;
        IBrowser browser;
        IBrowserContext context;
        IPage singlePage;

        string testUrl;

        public async Task<IPage> GetPage()
        {
            playwright = await Playwright.CreateAsync();
            browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = bool.Parse(ConfigurationManager.AppSettings["globalHeadless"]),
                SlowMo = int.Parse(ConfigurationManager.AppSettings["globalSloMo"])
            });
            context = await browser.NewContextAsync();
            return await context.NewPageAsync();
        }

        [SetUp]
        public void Setup()
        {
            
            testUrl = ConfigurationManager.AppSettings["baseUrl"];
            singlePage = GetPage().Result;
        }

        [TearDown]
        public void TearDown()
        {
            browser.CloseAsync();
        }

        [Test]
        [Category("PartialTestsDocumentUpload")]
        public async Task RespondToRequest_WithOneTextFileUsingHttpClient_VerifyStatusAndDocument()
        {
            // Create request for test
            
            await singlePage.SetViewportSizeAsync(1920, 1080);
            var loginPage = new LoginPage(singlePage);

            await loginPage.Goto();
            var portalUserName = ConfigurationManager.AppSettings["enhancedUser"];
            var portalPassword = ConfigurationManager.AppSettings["enhancedUserPwd"];
            var homePage = await loginPage.LoginAs(portalUserName, portalPassword);

            var requestName = $"Single Response: Respond with Text File - {DateTime.Now.ToString("s")}";
            var requestPage = await homePage.GoToPage(PageModels.Requests) as RequestsPage;
            var requestUrl = await requestPage.GenerateGenericRequest(requestName);
            var id = requestUrl.Split('=')[1];

            var fileList = new List<string>();

            #region API Calls
            await E2EUtils.WaitForRequestToProcess_HttpRequest(id);

            var requestId = new Guid(id);
            var dataMartId = new Guid(ConfigurationManager.AppSettings["dataMartId"]);

            var doc1 = new Document()
            {
                Name = "TestDoc",
                MimeType = "text/plain",
                Kind = "Text",
                Size = 3627,
                IsViewable = true,
            };
            var docs = new List<Document>()
            {
                doc1
            };
            fileList.Add(doc1.Name);


            var metaData = new DocumentMetadata()
            {
                RequestID = requestId,
                DataMartID = dataMartId,
                Name = doc1.Name,
                IsViewable = doc1.IsViewable,
                Size = doc1.Size,
                MimeType = doc1.MimeType,
                Kind = doc1.Kind,
                CurrentChunkIndex = 0,
            };

            var fileName = "TestDoc01.txt";
            var filePath = System.AppDomain.CurrentDomain.BaseDirectory
                + $"ResourceFiles\\{fileName}";

            var docList = new List<string>()
            {
                fileName
            };

            var uploadResult = await UploadDocument(metaData, filePath);
            Console.WriteLine($"Upload result: {uploadResult}");

            var props = new List<RoutingProperty>();
            var setRequestResult = await E2EUtils.SetRequestStatus(
                requestId,
                dataMartId,
                DMCRoutingStatus.AwaitingResponseApproval,
                "Test",
                props);

            Console.WriteLine($"Set Request result: {setRequestResult}");

            Assert.That(setRequestResult.IsSuccessStatusCode, Is.True);

            #endregion

            var requestDetails = await requestPage.GoToRequest(requestId);
            await requestDetails.VerifyEventLogUpdate("Submitted to Complete");
            await requestDetails.VerifyResultsStatusInOverviewResultsTable();
            await requestDetails.VerifyTaskStatusInRoutingsTable();

            await requestDetails.VerifyResponseDocuments(docList);

        }

        [Test]
        [Category("PipelineTest")]
        public async Task RespondToRequest_ReUploadResponse_HttpClient_ShowsResultsModified()
        {
            // Create request for test

            await singlePage.SetViewportSizeAsync(1920, 1080);
            var loginPage = new LoginPage(singlePage);

            await loginPage.Goto();
            var portalUserName = ConfigurationManager.AppSettings["enhancedUser"];
            var portalPassword = ConfigurationManager.AppSettings["enhancedUserPwd"];
            var homePage = await loginPage.LoginAs(portalUserName, portalPassword);

            var requestName = $"Re-Upload Response -- {DateTime.Now.ToString("s")}";
            var requestPage = await homePage.GoToPage(PageModels.Requests) as RequestsPage;
            var requestUrl = await requestPage.GenerateGenericRequest(requestName);
            var id = requestUrl.Split('=')[1];
            var docNames = new List<string>();

            #region First Response

            await E2EUtils.WaitForRequestToProcess_HttpRequest(id);
            var requestId = new Guid(id);
            var dataMartId = new Guid(ConfigurationManager.AppSettings["dataMartId"]);

            var doc1 = new Document()
            {
                Name = "TestDoc1",
                MimeType = "text/plain",
                Kind = "Text",
                Size = 3627,
                IsViewable = true,
            };

            docNames.Add(doc1.Name);

            var docs = new List<Document>()
            {
                doc1
            };

            Console.WriteLine($"Responding to request with document {doc1.Name}");

            var metaData = new DocumentMetadata()
            {
                //ID = docIds.Results[0],
                RequestID = requestId,
                DataMartID = dataMartId,
                Name = doc1.Name,
                IsViewable = doc1.IsViewable,
                Size = doc1.Size,
                MimeType = doc1.MimeType,
                Kind = doc1.Kind,
                CurrentChunkIndex = 0,
            };

            var fileName = "TestDoc01.txt";
            var filePath = System.AppDomain.CurrentDomain.BaseDirectory
                + $"ResourceFiles\\{fileName}";

            var uploadResult = await UploadDocument(metaData, filePath);
            Console.WriteLine($"Upload result: {uploadResult}");

            var props = new List<RoutingProperty>();
            var setRequestResult = await E2EUtils.SetRequestStatus(
                requestId,
                dataMartId,
                DMCRoutingStatus.AwaitingResponseApproval,
                "Test",
                props);

            Console.WriteLine($"Set Request result: {setRequestResult}");

            Assert.That(setRequestResult.IsSuccessStatusCode, Is.True);

            // Check that responses are set to Completed
            // Reload the page
            var requestDetails = await requestPage.GoToRequest(requestId);
            await requestDetails.VerifyEventLogUpdate("Submitted to Complete");
            await requestDetails.VerifyResultsStatusInOverviewResultsTable();
            await requestDetails.VerifyTaskStatusInRoutingsTable();
            Console.WriteLine($"Response {doc1.Name} verified...");
            #endregion

            #region Second Response
            var doc2 = new Document()
            {
                Name = "TestDoc2",
                MimeType = "text/plain",
                Kind = "Text",
                Size = 3627,
                IsViewable = true,
            };
            docNames.Add(doc2.Name);
            
            docs = new List<Document>()
            {
                doc2
            };
            Console.WriteLine($"Responding to request with document {doc2.Name}");
            //docIds = await PostResponseDocuments(requestId, dataMartId, docs);// should only be one

            metaData = new DocumentMetadata()
            {
                //ID = docIds.Results[0],
                RequestID = requestId,
                DataMartID = dataMartId,
                Name = doc2.Name,
                IsViewable = doc2.IsViewable,
                Size = doc2.Size,
                MimeType = doc2.MimeType,
                Kind = doc2.Kind,
                CurrentChunkIndex = 0,
            };

            fileName = "TestDoc02.txt";
            filePath = System.AppDomain.CurrentDomain.BaseDirectory
                + $"ResourceFiles\\{fileName}";

            uploadResult = await UploadDocument(metaData, filePath);
            Console.WriteLine($"Upload result: {uploadResult}");

            props = new List<RoutingProperty>();
            setRequestResult = await E2EUtils.SetRequestStatus(
                requestId,
                dataMartId,
                DMCRoutingStatus.AwaitingResponseApproval,
                "Test",
                props);

            Console.WriteLine($"Set Request result: {setRequestResult}");

            Assert.That(setRequestResult.IsSuccessStatusCode, Is.True);
            #endregion

            // Final assert
            // Check that responses are set to Completed
            // Reload the page
            requestDetails = await requestDetails.GoToRequest(requestId);
            
            // TODO: Wrap all these responses up into a method for easier verification...
            await requestDetails.VerifyEventLogUpdate("Completed to Results Modified");
            await requestDetails.VerifyResultsStatusInOverviewResultsTable("Results Modified");
            await requestDetails.VerifyTaskStatusInRoutingsTable("Results Modified");
            //await requestDetails.ExpandCompletedRouting();
            //await requestDetails.ViewResponse(); 
            
            await requestDetails.VerifyResponseDocuments(docNames);


        }

        [Test]
        [Category("PipelineTest")]
        public async Task ModularProgramRequest_RespondWithOneZipFileUsingMockApi_VerifyResults()
        {
            // Create request for test

            await singlePage.SetViewportSizeAsync(1920, 1080);
            var loginPage = new LoginPage(singlePage);

            await loginPage.Goto();
            var portalUserName = ConfigurationManager.AppSettings["enhancedUser"];
            var portalPassword = ConfigurationManager.AppSettings["enhancedUserPwd"];
            var homePage = await loginPage.LoginAs(portalUserName, portalPassword);

            var requestName = $"Request Roundtrip - Respond with Zip file - {DateTime.Now.ToString("s")}";
            var requestPage = await homePage.GoToPage(PageModels.Requests) as RequestsPage;
            var attachFile = $"{ConfigurationManager.AppSettings["testTextFile"]}";
            var requestType = $"{ConfigurationManager.AppSettings["modularProgramRequestType"]}";
            var requestUrl = await requestPage.GenerateGenericRequest(requestName, requestType, attachFile);
            var id = requestUrl.Split('=')[1];

            var fileList = new List<string>();
            fileList.Add(attachFile);

            #region API Calls
            await E2EUtils.WaitForRequestToProcess_HttpRequest(id);

            var requestId = new Guid(id);
            var dataMartId = new Guid(ConfigurationManager.AppSettings["dataMartId"]);
            var fileName = $"{ConfigurationManager.AppSettings["testZipFile"]}";
            var filePath = System.AppDomain.CurrentDomain.BaseDirectory
                + $"ResourceFiles\\{fileName}";

            var doc1 = new Document()
            {
                Name = fileName,
                MimeType = "application/octet-stream",
                Kind = "Zip",
                Size = 2260992,
                IsViewable = true,
            };
            var docNames = new List<string>()
            {
                doc1.Name
            };
            fileList.Add(doc1.Name);

            var metaData = new DocumentMetadata()
            {
                //ID = docIds.Results[0],
                RequestID = requestId,
                DataMartID = dataMartId,
                Name = doc1.Name,
                IsViewable = doc1.IsViewable,
                Size = doc1.Size,
                MimeType = doc1.MimeType,
                Kind = doc1.Kind,
                CurrentChunkIndex = 0,
            };


            var uploadResult = await UploadDocument(metaData, filePath);
            Console.WriteLine($"Upload result: {uploadResult}");

            var props = new List<RoutingProperty>();
            var setRequestResult = await E2EUtils.SetRequestStatus(
                requestId,
                dataMartId,
                DMCRoutingStatus.AwaitingResponseApproval,
                "Test",
                props);

            Console.WriteLine($"Set Request result: {setRequestResult}");

            Assert.That(setRequestResult.IsSuccessStatusCode, Is.True);

            #endregion

            // Check that responses are set to Completed
            // Reload the page
            var requestDetails = await requestPage.GoToRequest(requestId);
            await requestDetails.VerifyEventLogUpdate("Submitted to Complete");
            await requestDetails.VerifyResultsStatusInOverviewResultsTable();
            await requestDetails.VerifyTaskStatusInRoutingsTable();

            // TODO: Verify signature file information
            await requestDetails.VerifySignatureFileInformation();
            
            fileList.Add("ModularProgramRequest.xml");
            fileList.Add("ModularProgramRequest.html");
            await requestDetails.VerifyResponseDocuments(docNames);
        }

        /// <summary>
        /// Essentially creates a placeholder for a single document. The document is NOT actually uploaded
        /// at this point.
        /// </summary>
        /// <returns></returns>
        [Test]
        [Category("PartialTestsDocumentUpload")]
        public async Task PostSingleResponseDocument_ReturnsIdForDocument_UsingMockApi()
        {
            await singlePage.SetViewportSizeAsync(1920, 1080);
            var loginPage = new LoginPage(singlePage);

            await loginPage.Goto();
            var portalUserName = ConfigurationManager.AppSettings["enhancedUser"];
            var portalPassword = ConfigurationManager.AppSettings["enhancedUserPwd"];
            var homePage = await loginPage.LoginAs(portalUserName, portalPassword);

            var requestName = $"Partial Test: Single document posted, not uploaded {DateTime.Now.ToString("s")}";
            var requestPage = await homePage.GoToPage(PageModels.Requests) as RequestsPage;
            var requestUrl = await requestPage.GenerateGenericRequest(requestName);
            var id = requestUrl.Split('=')[1];

            await E2EUtils.WaitForRequestToProcess_HttpRequest(id);
            
            var requestId = new Guid(id);
            var dataMartId = new Guid(ConfigurationManager.AppSettings["dataMartId"]);

            var doc1 = new Document()
            {
                Name = "TestDoc",
                MimeType = "text/plain",
                Kind = "Text",
                Size = 128,
                IsViewable = true,
            };
            var docs = new List<Document>()
            {
                doc1
            };

            var docIds = await PostResponseDocuments(requestId, dataMartId, docs);
            foreach (var docId in docIds.Results)
                Console.WriteLine(docId);
            Assert.That(docIds.Results.Count, Is.EqualTo(docs.Count));

        }

        /// <summary>
        /// Creates placeholders for muliple documents. The documents are NOT uploaded at this point.
        /// </summary>
        /// <returns></returns>
        [Test]
        [Category("PartialTestsDocumentUpload")]
        public async Task PostMultipleResponseDocuments_ReturnsIdForEachDocument()
        {
            await singlePage.SetViewportSizeAsync(1920, 1080);
            var loginPage = new LoginPage(singlePage);
            // n.b. - This essentially creates placeholders in the Db for the documents.
            await loginPage.Goto();
            var portalUserName = ConfigurationManager.AppSettings["enhancedUser"];
            var portalPassword = ConfigurationManager.AppSettings["enhancedUserPwd"];
            var homePage = await loginPage.LoginAs(portalUserName, portalPassword);

            var requestName = $"Partial Test: Multiple Docs posted, not uploaded {DateTime.Now.ToString("s")}";
            var requestPage = await homePage.GoToPage(PageModels.Requests) as RequestsPage;
            var requestUrl = await requestPage.GenerateGenericRequest(requestName);
            var id = requestUrl.Split('=')[1];

            await E2EUtils.WaitForRequestToProcess_HttpRequest(id);

            var requestId = new Guid(id);
            var dataMartId = new Guid(ConfigurationManager.AppSettings["dataMartId"]);

            var doc1 = new Document()
            {
                Name = "TestDoc1",
                MimeType = "text/plain",
                Kind = "Text",
                Size = 128,
                IsViewable = true,
            };
            var doc2 = new Document()
            {
                Name = "TestDoc2",
                MimeType = "text/plain",
                Kind = "Text",
                Size = 128,
                IsViewable = true,
            };
            var doc3 = new Document()
            {
                Name = "TestDoc3",
                MimeType = "text/plain",
                Kind = "Text",
                Size = 128,
                IsViewable = true,
            };
            var docs = new List<Document>()
            {
                doc1,
                doc2,
                doc3
            };

            var docIds = await PostResponseDocuments(requestId, dataMartId, docs);
            foreach (var docId in docIds.Results)
                Console.WriteLine(docId);
            Assert.That(docIds.Results.Count, Is.EqualTo(docs.Count));

        }

        [Test]
        [Category("PartialTestsDocumentUpload")]
        public async Task UploadOneDocument_WithChunks_VerifyDocNoStatusChange()
        {
            var fileName = "TestDoc01.txt";
            var filePath = System.AppDomain.CurrentDomain.BaseDirectory
                + $"ResourceFiles\\{fileName}";
            await singlePage.SetViewportSizeAsync(1920, 1080);
            var loginPage = new LoginPage(singlePage);

            await loginPage.Goto();
            var portalUserName = ConfigurationManager.AppSettings["enhancedUser"];
            var portalPassword = ConfigurationManager.AppSettings["enhancedUserPwd"];
            var homePage = await loginPage.LoginAs(portalUserName, portalPassword);

            var requestName = $"Partial Test: Single Document Uploaded, No Status Change - {DateTime.Now.ToString("s")}";
            var requestPage = await homePage.GoToPage(PageModels.Requests) as RequestsPage;
            var requestUrl = await requestPage.GenerateGenericRequest(requestName);
            var id = requestUrl.Split('=')[1];

            await E2EUtils.WaitForRequestToProcess_HttpRequest(id);

            var requestId = new Guid(id);
            var dataMartId = new Guid(ConfigurationManager.AppSettings["dataMartId"]);

            var doc1 = new Document()
            {
                Name = "TestDoc",
                MimeType = "text/plain",
                Kind = "Text",
                Size = 3627,
                IsViewable = true,
            };
            var docs = new List<Document>()
            {
                doc1
            };

            var docIds = await PostResponseDocuments(requestId, dataMartId, docs);// should only be one

            var metaData = new DocumentMetadata()
            {
                ID = docIds.Results[0],
                RequestID = requestId,
                DataMartID = dataMartId,
                Name = doc1.Name,
                IsViewable = doc1.IsViewable,
                Size = doc1.Size,
                MimeType = doc1.MimeType,
                Kind = doc1.Kind,
                CurrentChunkIndex = 0,
            };

            var uploadResult = await UploadDocument(metaData, filePath);
            Console.WriteLine(uploadResult);
            Assert.That(uploadResult.IsSuccessStatusCode, Is.True);

            uploadResult.Dispose();
        }

        private async Task<HttpResponseMessage> UploadDocument(DocumentMetadata metaData, string filePath)
        {
            var response = new HttpResponseMessage();
            using (var stream = File.OpenRead(filePath))
            {
                byte[] buffer = new byte[0x400000];
                int index = 0;
                int bytesRead;
                metaData.Size = stream.Length;
                metaData.Kind = Path.GetExtension(filePath);
                metaData.MimeType = "text/plain";
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    byte[] data = buffer;
                    if (bytesRead < data.Length)
                    {
                        data = new byte[bytesRead];
                        Buffer.BlockCopy(buffer, 0, data, 0, bytesRead);
                    }
                    try
                    {
                        metaData.CurrentChunkIndex = index;

                        var temp = await PostDocumentChunk(metaData, data);
                        if (!temp.IsSuccessStatusCode || response == null)
                            response = temp;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Unable to post document {metaData.Name}");
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.InnerException);
                        throw ex;
                    }

                    index++;
                }
            }
            return response;
        }

        private async Task<HttpResponseMessage> PostDocumentChunk(DocumentMetadata documentMetadata, byte[] data)
        {
            Console.WriteLine($"Attempting to execute 'PostDocumentChunk' for request " +
                $"{documentMetadata.RequestID} from dataMart {documentMetadata.DataMartID}...");

            var resource = "DMC/PostDocumentChunk";

            using (var content = new MultipartFormDataContent())
            {

                var docContent = JsonConvert.SerializeObject(documentMetadata, E2EUtils.SerializerSettings());
                var sContent = new StringContent(docContent, Encoding.UTF8, "application/json");

                content.Add(sContent, "metadata");


                var docByteContent = new ByteArrayContent(data);
                docByteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                content.Add(docByteContent, "files", documentMetadata.Name);
                                
                var response = await E2EUtils.client.PostAsync(resource, content);
                if (response.IsSuccessStatusCode)
                    Console.WriteLine($"Success: {response.StatusCode}");
                else Console.WriteLine($"Response code: {response.StatusCode}");
                return response;
            }
        }

        private async Task<DocumentResult> PostResponseDocuments(Guid requestId, Guid dataMartId, List<Document> documents)
        {
            Console.WriteLine($"Attempting to execute 'PostResponseDocuments' for request {requestId} from dataMart {dataMartId}...");
            var data = new PostResponseDocumentsData()
            {
                RequestID = requestId,
                DataMartID = dataMartId,
                Documents = documents
            };

            var resource = "DMC/PostResponseDocuments";
            var request = new HttpRequestMessage(requestUri: resource, method: HttpMethod.Post);

            
            var content = JsonConvert.SerializeObject(data);

            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            request.Content = stringContent;

            
            using (var response = await E2EUtils.client.SendAsync(request))
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<DocumentResult>(result);
            }
        }

    }

    public class DocumentResult
    {
        public List<Guid> Results;
    }
}
