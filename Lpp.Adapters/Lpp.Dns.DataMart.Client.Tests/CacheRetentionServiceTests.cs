using Lpp.Dns.DataMart.Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Client.Tests
{
    [TestClass]
    public class CacheRetentionServiceTests
    {
        static readonly log4net.ILog logger;
        readonly string BaseCacheFolderPath;

        static CacheRetentionServiceTests()
        {
            log4net.Config.XmlConfigurator.Configure();
            logger = log4net.LogManager.GetLogger(typeof(CacheRetentionServiceTests));
        }

        public CacheRetentionServiceTests()
        {
            BaseCacheFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cache");
            //setup a local test cache root folder
            if (!Directory.Exists(BaseCacheFolderPath))
            {
                Directory.CreateDirectory(BaseCacheFolderPath);
            }

            logger.Info("Base test cache folder path: " + BaseCacheFolderPath);
        }

        [TestMethod]
        public void ShouldDeleteExpiredCachedFiles()
        {
            //initialize a fake datamart setting
            var networkSetting = new DataMart.Lib.NetWorkSetting {
                NetworkId = 99,
                NetworkName = "Cache Retention Test",
                DataMartList = new List<DataMart.Lib.DataMartDescription> {
                    new DataMart.Lib.DataMartDescription
                    {
                        DataMartId = Guid.Parse("6C793581-BC1D-4B3B-8D0B-DA527D9CB00B"),
                        DataMartName = "Cache Test DataMart",
                        OrganizationId = "F2076C32-A86C-4271-AB19-E076FF0808A5",
                        EnableResponseCaching = true,
                        DaysToRetainCacheItems = 1                        
                    }
                }
            };

            DeleteExistingCachedRequests(Path.Combine(BaseCacheFolderPath, networkSetting.NetworkId.ToString(), networkSetting.DataMartList[0].DataMartId.ToString("D")));

            List<string> createdTestFiles = CreateTestFiles(BaseCacheFolderPath, networkSetting);

            //initialize cache retention service
            var retentionService = new Lib.Caching.CacheRetentionService(BaseCacheFolderPath, 1);
            retentionService.RegisterNetwork(networkSetting);

            //wait longer than the cache timer 
            System.Threading.Thread.Sleep(Convert.ToInt32(TimeSpan.FromMinutes(1.25f).TotalMilliseconds));

            //confirm that the cache removed the files on initialization of the network.
            foreach(string path in createdTestFiles)
            {
                Assert.IsTrue(File.Exists(path) == false, "File was not deleted on cache retention serice network initialization: " + path);
            }

            //create some new test files and make sure they get cleaned up by timer
            System.Threading.Thread.Sleep(Convert.ToInt32(TimeSpan.FromMinutes(1.25f).TotalMilliseconds));
            createdTestFiles = CreateTestFiles(BaseCacheFolderPath, networkSetting);
            System.Threading.Thread.Sleep(Convert.ToInt32(TimeSpan.FromMinutes(1.25f).TotalMilliseconds));
            
            //confirm that the cache removed the files on initialization of the network.
            foreach (string path in createdTestFiles)
            {
                Assert.IsTrue(File.Exists(path) == false, "File was not deleted on cache retention serice network timer elapse: " + path);
            }
        }

        static void DeleteExistingCachedRequests(string datamartFolder)
        {
            if (!Directory.Exists(datamartFolder))
            {
                return;
            }

            var directories = Directory.GetDirectories(datamartFolder);
            foreach(var dir in directories)
            {
                Directory.Delete(dir, true);
            }
        }

        static List<string> CreateTestFiles(string baseCacheFolderPath, DataMart.Lib.NetWorkSetting networkSetting)
        {
            if (!Directory.Exists(baseCacheFolderPath))
            {
                Directory.CreateDirectory(baseCacheFolderPath);
            }

            if(!Directory.Exists(Path.Combine(baseCacheFolderPath, networkSetting.NetworkId.ToString())))
            {
                Directory.CreateDirectory(Path.Combine(baseCacheFolderPath, networkSetting.NetworkId.ToString()));
            }

            if (!Directory.Exists(Path.Combine(baseCacheFolderPath, networkSetting.NetworkId.ToString(), networkSetting.DataMartList[0].DataMartId.ToString("D"))))
            {
                Directory.CreateDirectory(Path.Combine(baseCacheFolderPath, networkSetting.NetworkId.ToString(), networkSetting.DataMartList[0].DataMartId.ToString("D")));
            }

            List<string> createdTestFiles = new List<string>(20);
            //create some fake cache files            
            for (int i = 0; i < 3; i++)
            {
                Guid requestID = Guid.NewGuid();
                string requestCacheFolder = Path.Combine(baseCacheFolderPath, networkSetting.NetworkId.ToString(), networkSetting.DataMartList[0].DataMartId.ToString("D"), requestID.ToString("D"));
                if (!Directory.Exists(requestCacheFolder))
                {
                    Directory.CreateDirectory(requestCacheFolder);
                }

                Guid fileID = Guid.NewGuid();
                string filePath = Path.Combine(requestCacheFolder, string.Format("{0:D}.meta", fileID));
                using (var fs = File.CreateText(filePath))
                {
                    fs.Close();
                }
                createdTestFiles.Add(filePath);

                //alter the file time to be older than the cache duration
                File.SetCreationTimeUtc(filePath, DateTime.UtcNow.AddDays(-2));
                File.SetLastWriteTime(filePath, DateTime.UtcNow.AddDays(-2));

                filePath = Path.Combine(requestCacheFolder, string.Format("{0:D}.data", fileID));
                using (var fs = File.CreateText(filePath))
                {
                    fs.Close();
                }

                createdTestFiles.Add(filePath);

                File.SetCreationTimeUtc(filePath, DateTime.UtcNow.AddDays(-2));
                File.SetLastWriteTime(filePath, DateTime.UtcNow.AddDays(-2));
            }

            return createdTestFiles;
        }

    }    
}
