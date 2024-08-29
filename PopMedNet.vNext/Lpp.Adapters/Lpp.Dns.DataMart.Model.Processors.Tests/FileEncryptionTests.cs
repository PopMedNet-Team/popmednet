using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Lpp.Utilities;
using Newtonsoft.Json.Linq;

namespace Lpp.Dns.DataMart.Model.Processors.Tests
{
    [TestClass]
    public class FileEncryptionTests
    {
        static readonly int NetworkID = 1;
        static readonly Guid DataMartID = new Guid("86050001-5C87-43AC-B977-A8D2012F544A");
        static readonly Guid RequestID = new Guid("8B380001-07B2-4299-A862-A8D2012F5B47");

        [TestMethod]
        public void EncryptDecryptFile()
        {
            var document = new { Document = new Document(Guid.NewGuid(), "application/json", "results.json", false, 12000, null) };
            bool encrypt = true;
            string metaDataPath = Path.Combine("cache", document.Document.DocumentID + ".e.meta");

            //encrypt the document
            using (var fileStream = File.OpenWrite(metaDataPath))
            using (StreamWriter writer = encrypt ? new StreamWriter(CreateEncryptionStream(fileStream)) : new StreamWriter(fileStream))
            using (var json = new Newtonsoft.Json.JsonTextWriter(writer))
            {
                var serializer = new Newtonsoft.Json.JsonSerializer();
                serializer.Serialize(json, document.Document);
                json.Flush();

                //if (encrypt)
                //{
                //    ((CryptoStream)writer.BaseStream).FlushFinalBlock();
                //}
            }

            Document decryptedDocument = null;
            // decrypt the document
            using (var fs = File.OpenRead(metaDataPath))
            using (StreamReader reader = encrypt ? new StreamReader(CreateDecryptionStream(fs)) : new StreamReader(fs))
            using (var jsonReader = new Newtonsoft.Json.JsonTextReader(reader))
            {

                var serializer = new Newtonsoft.Json.JsonSerializer();
                serializer.Converters.Add(new DocumentCreationConverter());

                decryptedDocument = serializer.Deserialize<Document>(jsonReader);
            }

            Assert.IsNotNull(decryptedDocument);

        }

        [TestMethod]
        public void EncryptDecryptJsonData()
        {
            string sourceFile = @"..\Resources\92aea0be-710b-4294-81e2-c1d06ca5e9c5.data";

            using (var sourceFS = File.OpenRead(sourceFile))
            using (var destFS = File.OpenWrite(Path.Combine("cache", "92aea0be-710b-4294-81e2-c1d06ca5e9c5.e.data")))
            using (Stream stream = CreateEncryptionStream(destFS))
            {
                sourceFS.CopyTo(stream);
                stream.Flush();
                sourceFS.Flush();
            }

            using (var source = File.OpenRead(Path.Combine("cache", "92aea0be-710b-4294-81e2-c1d06ca5e9c5.e.data")))
            using (var crypto = CreateDecryptionStream(source))
            //using(var dest = new FileStream(Path.Combine("cache", "decrypted-test.json"), FileMode.Create))
            //{
            //    reader.CopyTo(dest);
            //}
            using(var reader = new StreamReader(crypto))
            {
                string json = reader.ReadToEnd();

                reader.Close();
                crypto.Close();
            }

            //using(var rawJson = new StreamReader(Path.Combine("cache", "decrypted-test.json")))
            //{
            //    string json = rawJson.ReadToEnd();
            //    var response = JObject.Parse(json);
            //}

        }

        CryptoStream CreateEncryptionStream(Stream outputStream)
        {
            using (RijndaelManaged aesAlg = new RijndaelManaged())
            {

                using (Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(string.Format("{0}-{1:D}-{2:D}", NetworkID, DataMartID, RequestID), RequestID.ToByteArray()))
                {
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                }

                //prepend the IV
                outputStream.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                outputStream.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                CryptoStream encryptor = new CryptoStream(outputStream, aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV), CryptoStreamMode.Write);
                return encryptor;
            }
        }

        CryptoStream CreateDecryptionStream(Stream input)
        {
            using (RijndaelManaged aesAlg = new RijndaelManaged())
            {
                using (Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(string.Format("{0}-{1:D}-{2:D}", NetworkID, DataMartID, RequestID), RequestID.ToByteArray()))
                {
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                }
                aesAlg.IV = ReadByteArray(input);

                CryptoStream decryptor = new CryptoStream(input, aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV), CryptoStreamMode.Read);
                return decryptor;
            }
        }

        static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new System.Security.SecurityException("Stream did not contain properly formatted byte array.");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new InvalidOperationException("Could not read byte array properly.");
            }

            return buffer;
        }
    }

    public class DocumentCreationConverter : Newtonsoft.Json.Converters.CustomCreationConverter<Document>
    {
        public override Document Create(Type objectType)
        {
            return new Document("", "", "");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var document = new Document("", "", "");
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName && (reader.Value.ToStringEx() == "DocumentID"))
                {
                    reader.Read();
                    document.DocumentID = reader.Value.ToStringEx();
                }
                else if (reader.TokenType == JsonToken.PropertyName && (reader.Value.ToStringEx() == "MimeType"))
                {
                    reader.Read();
                    document.MimeType = reader.Value.ToStringEx();
                }
                else if (reader.TokenType == JsonToken.PropertyName && (reader.Value.ToStringEx() == "Size"))
                {
                    reader.Read();
                    document.Size = reader.Value.ToInt32();
                }
                else if (reader.TokenType == JsonToken.PropertyName && (reader.Value.ToStringEx() == "IsViewable"))
                {
                    reader.Read();
                    document.IsViewable = reader.Value.ToBool();
                }
                else if (reader.TokenType == JsonToken.PropertyName && (reader.Value.ToStringEx() == "Filename"))
                {
                    reader.Read();
                    document.Filename = reader.Value.ToStringEx();
                }
                else if (reader.TokenType == JsonToken.PropertyName && (reader.Value.ToStringEx() == "Kind"))
                {
                    reader.Read();
                    document.Kind = reader.Value.ToStringEx();
                }
            }

            return document;
        }
    }
}
