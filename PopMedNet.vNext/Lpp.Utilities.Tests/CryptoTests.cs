using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lpp.Utilities.Tests
{
    [TestClass]
    public class CryptoTests
    {
        [TestMethod]
        public void EncryptDecryptAES()
        {
            string plaintext = "Hello World";
            string sharedSecret = "Password1!";
            string salt = "abcd1234";
            var encrypted = Crypto.EncryptStringAES(plaintext, sharedSecret, salt);
            var decrypted = Crypto.DecryptStringAES(encrypted, sharedSecret, salt);
            Assert.AreEqual(plaintext, decrypted);
        }

        [TestMethod]
        public void GenerateTokens()
        {
            string[] plaintexts = { "Hello World", "Humpty Dumpty has a great fall", "Jack be nimble, Jack be quick", "To be or not to be, that is the question" };
            string sharedSecret = "Password1!";
            string salt = "abcd1234";

            foreach(string plaintext in plaintexts)
            {
                var encrypted = Crypto.EncryptStringAES(plaintext, sharedSecret, salt);
                System.Diagnostics.Debug.WriteLine("\"" + encrypted + "\", ");
            }
        }
    }
}
