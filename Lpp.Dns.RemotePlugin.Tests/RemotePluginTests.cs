using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel.Web;

namespace Lpp.Dns.RemotePlugin.Tests
{
    [TestClass]
    public class GetRequestTests
    {
        private const string REMOTE_SERVICE_URL = "http://localhost:55381/api/rest/remote";

        //[TestMethod]
        //public void TestStartSession()
        //{
        //    try
        //    {
        //        using (var factory = new WebChannelFactory<IRemotePluginService>(new Uri(REMOTE_SERVICE_URL)))
        //        {
        //            var channel = factory.CreateChannel();
        //            var sessionToken = channel.StartSession(new Credentials
        //                                 {
        //                                     Username = "Investigator",
        //                                     Password = "Password1!"
        //                                 }, "pmn", Guid.NewGuid());
        //            Assert.IsNotNull(sessionToken);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}

        //[TestMethod]
        //public void TestCloseSession()
        //{
        //    try
        //    {
        //        using (var factory = new WebChannelFactory<IRemotePluginService>(new Uri(REMOTE_SERVICE_URL)))
        //        {
        //            var channel = factory.CreateChannel();
        //            var sessionToken = channel.StartSession(new Credentials
        //                                 {
        //                                     Username = "Investigator",
        //                                     Password = "Password1!"
        //                                 });
        //            channel.CloseSession(sessionToken);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}

        //[TestMethod]
        //public void TestCreateRequest()
        //{
        //    try
        //    {
        //        using (var factory = new WebChannelFactory<IRemotePluginService>(new Uri(REMOTE_SERVICE_URL)))
        //        {
        //            var channel = factory.CreateChannel();
        //            var sessionToken = channel.StartSession(new Credentials
        //                                 {
        //                                     Username = "Investigator",
        //                                     Password = "Password1!"
        //                                 });
        //            var requestId = channel.CreateRequest(sessionToken, "{99A4F3A2-7C9E-4F42-8F07-4E654A05643F}");
        //            channel.CloseSession(sessionToken);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}

        //[TestMethod]
        //public void TestPostDocument()
        //{
        //    try
        //    {
        //        using (var factory = new WebChannelFactory<IRemotePluginService>(new Uri(REMOTE_SERVICE_URL)))
        //        {
        //            var channel = factory.CreateChannel();
        //            var sessionToken = channel.StartSession(new Credentials
        //                                 {
        //                                     Username = "Investigator",
        //                                     Password = "Password1!"
        //                                 });
        //            var requestId = channel.CreateRequest(sessionToken, "{6EB513D8-C916-4FEE-BA10-70E7ACE3AAB5}");
        //            channel.PostDocument(sessionToken, requestId, "TestDoc.txt", "text/plain", false, new byte[] { 1, 2, 3 });
        //            channel.CloseSession(sessionToken);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}

        //[TestMethod]
        //public void TestSubmitRequest()
        //{
        //    try
        //    {
        //        using (var factory = new WebChannelFactory<IRemotePluginService>(new Uri(REMOTE_SERVICE_URL)))
        //        {
        //            var channel = factory.CreateChannel();
        //            var sessionToken = channel.StartSession(new Credentials
        //                                 {
        //                                     Username = "Investigator",
        //                                     Password = "Password1!"
        //                                 });
        //            var requestId = channel.CreateRequest(sessionToken, "{6EB513D8-C916-4FEE-BA10-70E7ACE3AAB5}");
        //            channel.PostDocument(sessionToken, requestId, "TestDoc.txt", "text/plain", false, new byte[] { 1, 2, 3 });
        //            channel.SubmitRequest(sessionToken, requestId, new RequestHeader
        //                                                           {
        //                                                               Name = "TestRequest"
        //                                                           },
        //                                                           new int[0]);
        //            channel.CloseSession(sessionToken);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}

        //[TestMethod]
        //public void TestSubmitGetRequests()
        //{
        //    try
        //    {
        //        using (var factory = new WebChannelFactory<IRemotePluginService>(new Uri(REMOTE_SERVICE_URL)))
        //        {
        //            var channel = factory.CreateChannel();
        //            var sessionToken = channel.StartProjectSession(new Credentials
        //                                 {
        //                                     Username = "Investigator",
        //                                     Password = "Password1!"
        //                                 },
        //                                 "demo",
        //                                 "Demo");
        //            //var requestId = channel.CreateRequest(sessionToken, "{6EB513D8-C916-4FEE-BA10-70E7ACE3AAB5}");
        //            //channel.PostDocument(sessionToken, requestId, "TestDoc.txt", "text/plain", false, new byte[] { 1, 2, 3 });
        //            //channel.SubmitRequest(sessionToken, requestId, new RequestHeader
        //            //                                               {
        //            //                                                   Name = "TestRequest"
        //            //                                               },
        //            //                                               new int[0]);
        //            var requests = channel.GetRequests(sessionToken, null, null, null, null);
        //            channel.CloseSession(sessionToken);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}

        //[TestMethod]
        //public void TestGetRequests()
        //{
        //    try
        //    {
        //        using (var factory = new WebChannelFactory<IRemotePluginService>(new Uri(REMOTE_SERVICE_URL)))
        //        {
        //            var channel = factory.CreateChannel();
        //            var sessionToken = channel.StartSession(new Credentials
        //                                 {
        //                                     Username = "Investigator",
        //                                     Password = "Password1!"
        //                                 });                
        //            var requests = channel.GetRequests(sessionToken, null, null, null, null);
        //            channel.CloseSession(sessionToken);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}

        //[TestMethod]
        //public void TestSubmitGetRequest()
        //{
        //    try
        //    {
        //        using (var factory = new WebChannelFactory<IRemotePluginService>(new Uri(REMOTE_SERVICE_URL)))
        //        {
        //            var channel = factory.CreateChannel();
        //            var sessionToken = channel.StartSession(new Credentials
        //                                 {
        //                                     Username = "Investigator",
        //                                     Password = "Password1!"
        //                                 });
        //            var requestId = channel.CreateRequest(sessionToken, "{6EB513D8-C916-4FEE-BA10-70E7ACE3AAB5}");
        //            channel.PostDocument(sessionToken, requestId, "TestDoc.txt", "text/plain", false, new byte[] { 1, 2, 3 });
        //            channel.SubmitRequest(sessionToken, requestId, new RequestHeader
        //                                                           {
        //                                                               Name = "TestRequest"
        //                                                           },
        //                                                           new int[0]);
        //            var request = channel.GetRequest(sessionToken, requestId);
        //            channel.CloseSession(sessionToken);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}

        //[TestMethod]
        //public void TestGetRequestCountDocs()
        //{
        //    try
        //    {
        //        using (var factory = new WebChannelFactory<IRemotePluginService>(new Uri(REMOTE_SERVICE_URL)))
        //        {
        //            var channel = factory.CreateChannel();
        //            var sessionToken = channel.StartSession(new Credentials
        //                                 {
        //                                     Username = "Investigator",
        //                                     Password = "Password1!"
        //                                 });
        //            var requests = channel.GetRequests(sessionToken, null, null, null, null);
        //            var request = channel.GetRequest(sessionToken, "2");
        //            Assert.AreEqual(3, request.DataMartResponses.FirstOrDefault().Documents.Count());
        //            string content = System.Text.UTF8Encoding.UTF8.GetString(request.DataMartResponses.FirstOrDefault().Documents.FirstOrDefault().Content);
        //            channel.CloseSession(sessionToken);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}
    }
}
