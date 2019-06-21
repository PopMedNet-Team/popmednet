using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using Lpp.Dns.DataMart.Model;
using Newtonsoft.Json;
using Lpp.Utilities;

namespace Lpp.Dns.DataMart.Client.Lib.Caching
{
    public class DocumentCacheManager
    {
        public readonly int NetworkID;
        public readonly Guid DataMartID;
        public readonly Guid RequestID;
        public readonly string RequestIdentifier;
        public readonly string BaseCachePath;

        public DocumentCacheManager(int networkID, Guid datamartID, Guid requestID, Guid? responseID)
        {
            NetworkID = networkID;
            DataMartID = datamartID;
            RequestID = requestID;

            string rootCachePath = Lpp.Dns.DataMart.Client.Properties.Settings.Default.CacheRootFolder;
            if (string.IsNullOrEmpty(rootCachePath))
            {
                rootCachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Properties.Settings.Default.AppDataFolderName, "cache");
            }

            if (responseID.HasValue)
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(Path.Combine(rootCachePath, NetworkID.ToString(), DataMartID.ToString("D"), RequestID.ToString()));

                if (di.Exists)
                {
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    var dir = di.GetDirectories();
                    foreach (DirectoryInfo sub in dir.Where(x => !string.Equals(x.Name, responseID.Value.ToString("D"), StringComparison.OrdinalIgnoreCase)))
                    {
                        sub.Delete(true);
                    }
                }                

                BaseCachePath = Path.Combine(rootCachePath, NetworkID.ToString(), DataMartID.ToString("D"), RequestID.ToString(), responseID.Value.ToString("D"));
            }                
            else
                BaseCachePath = Path.Combine(rootCachePath, NetworkID.ToString(), DataMartID.ToString("D"), RequestID.ToString());

            if (!Directory.Exists(BaseCachePath))
                Directory.CreateDirectory(BaseCachePath);
        }

        DataMart.Lib.DataMartDescription Configuration
        {
            get
            {
                return Client.Utils.Configuration.Instance.GetDataMartDescription(NetworkID, DataMartID);
            }
        }

        public bool Enabled
        {
            get
            {
                return Configuration.EnableResponseCaching && Configuration.DaysToRetainCacheItems != 0m;
            }
        }

        public bool HasResponseDocuments
        {
            get
            {
                return Configuration.EnableResponseCaching && Directory.GetFiles(BaseCachePath, "*.meta").Any();
            }
        }

        public bool CanClearRequestSpecificCache
        {
            get
            {
                return Configuration.EnableExplictCacheRemoval && HasResponseDocuments;
            }
        }

        public IEnumerable<Document> GetResponseDocuments()
        {
            if (Enabled == false)
            {
                return Array.Empty<Document>();
            }
            
            var files = Directory.GetFiles(BaseCachePath, "*.meta");
            List<Document> documents = new List<Document>(files.Length * 2);

            foreach (var path in files)
            {
                bool encrypted = path.EndsWith(".e.meta");

                //deserialize the document meta data
                using (var fs = File.OpenRead(path))
                using (StreamReader reader = encrypted ? new StreamReader(CreateDecryptionStream(fs)) : new StreamReader(fs))
                using (var json = new Newtonsoft.Json.JsonTextReader(reader))
                {
                    var serializer = new Newtonsoft.Json.JsonSerializer();
                    serializer.Converters.Add(new DocumentCreationConverter());

                    var doc = serializer.Deserialize<Document>(json);
                    documents.Add(doc);
                }
            }

            return documents;            
        }

        public Stream GetDocumentStream(Guid documentID)
        {
            string path = Path.Combine(BaseCachePath, documentID.ToString("D") + ".data");
            if (File.Exists(path))
            {
                return File.OpenRead(path);
            }

            path = Path.Combine(BaseCachePath, documentID.ToString("D") + ".e.data");
            if (File.Exists(path))
            {                
                return CreateDecryptionStream(File.OpenRead(path));
            }

            throw new FileNotFoundException("Cached data file not found for document ID: " + documentID.ToString("D"));
        }

        public void Add(IEnumerable<DocumentWithStream> documents)
        {
            if(Enabled == false)
            {
                return;
            }

            bool encrypt = Configuration.EncryptCacheItems;

            foreach(var document in documents)
            {
                string metaDataPath = Path.Combine(BaseCachePath, document.ID.ToString("D") + (encrypt ? ".e" : "") + ".meta");
                string contentPath = Path.Combine(BaseCachePath, document.ID.ToString("D") + (encrypt ? ".e" : "") + ".data");

                using (var fileStream = File.OpenWrite(metaDataPath))
                using (StreamWriter writer = encrypt ? new StreamWriter(CreateEncryptionStream(fileStream)) : new StreamWriter(fileStream))
                using (var json = new Newtonsoft.Json.JsonTextWriter(writer))
                {
                    var serializer = new Newtonsoft.Json.JsonSerializer();
                    serializer.Serialize(json, document.Document);
                    json.Flush();
                }

                using (var fileStream = new FileStream(contentPath, FileMode.Create, FileAccess.Write))
                using (Stream stream = encrypt ? (Stream)CreateEncryptionStream(fileStream) : fileStream)
                {
                    document.Stream.CopyTo(stream);
                    stream.Flush();
                    document.Stream.Flush();
                }
            }
        }

        public void ClearCache()
        {
            foreach(var file in Directory.GetFiles(BaseCachePath))
            {
                File.Delete(file);
            }
        }

        public void Remove(IEnumerable<Document> documents)
        {
            foreach(var doc in documents)
            {
                var paths = Directory.GetFiles(BaseCachePath, Guid.Parse(doc.DocumentID) + ".*");
                foreach(var file in paths)
                {
                    File.Delete(file);
                }
            }
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
            using(RijndaelManaged aesAlg = new RijndaelManaged())
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
                if(reader.TokenType == JsonToken.PropertyName && (reader.Value.ToStringEx() == "DocumentID"))
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
                    document.Size =  reader.Value.ToInt32();
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
