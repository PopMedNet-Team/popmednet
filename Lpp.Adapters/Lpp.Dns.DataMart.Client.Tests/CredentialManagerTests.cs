using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lpp.Dns.DataMart.Client.Utils;

namespace Lpp.Dns.DataMart.Client.Tests
{
    [TestClass]
    public class CredentialManagerTests
    {
        private const string KEY = "BF36159B-8406-4257-8EA2-9352F8CF5F87";
        private const string USERNAME = "TestUser";
        private const string PASSWORD = "TestPassword";

        //[TestMethod]
        public void TestCreateCredential()
        {
            try
            {
                CredentialManager.SaveCredential(KEY, USERNAME, PASSWORD);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        //[TestMethod]
        public void TestGetCredential()
        {
            string un, pw;
            CredentialManager.GetCredential(KEY, out un, out pw);
            Assert.AreEqual(USERNAME, un);
            Assert.AreEqual(PASSWORD, pw);
        }

        //[TestMethod]
        public void TestDeleteCredential()
        {
            string un, pw;
            CredentialManager.GetCredential(KEY, out un, out pw);
            Assert.IsFalse(string.IsNullOrEmpty(un));
            Assert.IsFalse(string.IsNullOrEmpty(pw));
            CredentialManager.DeleteCredential(KEY);
            CredentialManager.GetCredential(KEY, out un, out pw);
            Assert.IsNull(un);
            Assert.IsNull(pw);
        }

        //[TestMethod]
        public void TestDeleteHubWebCredential()
        {
            string un, pw;
            CredentialManager.GetCredential("http://dnsquerytool.lincolnpeak.com/api/soap/dmclient", out un, out pw);
            Assert.IsFalse(string.IsNullOrEmpty(un));
            Assert.IsFalse(string.IsNullOrEmpty(pw));
            CredentialManager.DeleteCredential("http://dnsquerytool.lincolnpeak.com/api/soap/dmclient");
            CredentialManager.GetCredential("http://dnsquerytool.lincolnpeak.com/api/soap/dmclient", out un, out pw);
            Assert.IsNull(un);
            Assert.IsNull(pw);
        }
    }
}
