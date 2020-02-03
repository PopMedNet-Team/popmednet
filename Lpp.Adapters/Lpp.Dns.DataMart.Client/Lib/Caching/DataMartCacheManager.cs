using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lpp.Dns.DataMart.Client.Lib.Caching
{
    public class DataMartCacheManager
    {
        public readonly int NetworkID;
        public readonly Guid DataMartID;
        public readonly string BaseCachePath;

        public DataMartCacheManager(int networkID, Guid dataMartID)
        {
            NetworkID = networkID;
            DataMartID = dataMartID;

            string rootCachePath = Lpp.Dns.DataMart.Client.Properties.Settings.Default.CacheRootFolder;
            if (string.IsNullOrEmpty(rootCachePath))
            {
                rootCachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Properties.Settings.Default.AppDataFolderName, "cache");
            }

            BaseCachePath = Path.Combine(rootCachePath, NetworkID.ToString(), DataMartID.ToString("D"));

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

        public void ClearCache()
        {
            var files = Directory.GetFiles(BaseCachePath);
            foreach(var file in files)
            {
                File.Delete(file);
            }

            var directories = Directory.GetDirectories(BaseCachePath);
            foreach(var directory in directories)
            {
                Directory.Delete(directory, true);
            }
        }


    }
}
