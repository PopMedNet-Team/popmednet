using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Client.Lib.Caching
{
    public class CacheRetentionService
    {
        public static readonly log4net.ILog log;

        public readonly System.Collections.Concurrent.ConcurrentBag<CacheDuration> DataMartCacheDurations = new System.Collections.Concurrent.ConcurrentBag<CacheDuration>();
        public readonly string BaseCachePath;
        public readonly System.Timers.Timer Timer;

        static CacheRetentionService()
        {
            log = log4net.LogManager.GetLogger(typeof(CacheRetentionService));
        }

        /// <summary>
        /// Initializes the cache retention service using settings specified in the app.config for the root folder and timer duration.
        /// </summary>
        public CacheRetentionService() {
            string rootCachePath = Lpp.Dns.DataMart.Client.Properties.Settings.Default.CacheRootFolder;
            if (string.IsNullOrEmpty(rootCachePath))
            {
                rootCachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Properties.Settings.Default.AppDataFolderName, "cache");
            }

            BaseCachePath = rootCachePath;

            log.Debug($"Initializing CacheRetentionManager to base cache path of: { BaseCachePath }");

            Timer = new System.Timers.Timer(TimeSpan.FromMinutes(Properties.Settings.Default.CacheRetentionTimerMinutes).TotalMilliseconds);
            Timer.AutoReset = false;
            Timer.Elapsed += Timer_Elapsed;
            Timer.Start();
        }

        /// <summary>
        /// Initializes the cache retention service using the specified root cache path, and timer duration.
        /// </summary>
        /// <param name="baseCachePath">The root path of the cache.</param>
        /// <param name="cacheRetentionTimerMinutes">The duration in minutes the retention service should check for files to evict.</param>
        public CacheRetentionService(string baseCachePath, int cacheRetentionTimerMinutes)
        {
            BaseCachePath = baseCachePath;

            log.Debug($"Initializing CacheRetentionManager to base cache path of: { BaseCachePath }");

            Timer = new System.Timers.Timer(TimeSpan.FromMinutes(cacheRetentionTimerMinutes).TotalMilliseconds);
            Timer.AutoReset = false;
            Timer.Elapsed += Timer_Elapsed;
            Timer.Start();
        }

        void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Parallel.ForEach(DataMartCacheDurations, duration => CheckCache(BaseCachePath, duration));
            Timer.Start();
        }

        public void RegisterNetwork(Lpp.Dns.DataMart.Lib.NetWorkSetting network)
        {
            Parallel.ForEach(network.DataMartList, dm =>
            {
                var cacheDuration = new CacheDuration(network.NetworkId, network.NetworkName, dm.DataMartId, dm.DataMartName, dm.EnableResponseCaching, dm.DaysToRetainCacheItems);
                CheckCache(BaseCachePath, cacheDuration);
                DataMartCacheDurations.Add(cacheDuration);
            });
        }

        static void CheckCache(string baseCachePath, CacheDuration cache)
        {
            if(cache.Enabled == false || cache.Duration == TimeSpan.Zero || cache.Duration == TimeSpan.MaxValue)
            {
                //do not remove files if caching is not enabled, or duration is for infinity
                //should this clear any existing files?
                return;
            }

            string cachePath = Path.Combine(baseCachePath, cache.NetworkID.ToString(), cache.DataMartID.ToString("D"));
            if (!Directory.Exists(cachePath))
            {
                //no cache files to check
                return;
            }

            log.Debug($"Checking cache for expired entries for Network:{ cache.Network }, DataMart:{ cache.DataMart }, path:{ cachePath }");

            string[] requestDirectories = Directory.GetDirectories(cachePath);
            if(requestDirectories.Length == 0)
            {
                log.Debug($"No cache entries for Network:{ cache.Network }, DataMart:{ cache.DataMart }, path:{ cachePath }");
                return;
            }

            Parallel.ForEach(requestDirectories, (directory) => {
                string[] filePaths = Directory.GetFiles(directory, "*.meta", SearchOption.AllDirectories);
                foreach(string path in filePaths)
                {
                    TimeSpan age = DateTime.UtcNow - File.GetLastWriteTimeUtc(path);
                    if (age > cache.Duration)
                    {
                        string dataPath = Path.ChangeExtension(path, "data");
                        try
                        {
                            log.Debug("Deleting expired cache document meta:" + path);
                            File.Delete(path);

                            log.Debug("Deleting expired cache document data:" + dataPath);
                            File.Delete(dataPath);
                        }catch(IOException ex)
                        {
                            //log and continue
                            log.Error($"Error deleting expired cache document:{ path }, data:{ dataPath }", ex);
                        }
                    }
                }

            });
        }

    }

    public struct CacheDuration
    {
        public readonly int NetworkID;
        public readonly string Network;
        public readonly Guid DataMartID;
        public readonly string DataMart;
        public readonly bool Enabled;
        public readonly TimeSpan Duration;

        public CacheDuration(int networkID, string networkName, Guid datamartID, string datamartName, bool enabled, decimal durationInDays)
        {
            NetworkID = networkID;
            Network = networkName;
            DataMartID = datamartID;
            DataMart = datamartName;
            Enabled = enabled;
            //DurationInDays < 0: do not delete cache items
            //DurationInDays == 0: caching not enabled
            Duration = (enabled == false || durationInDays == 0) ? TimeSpan.Zero : (durationInDays < 0 ? TimeSpan.MaxValue : TimeSpan.FromDays(Convert.ToDouble(durationInDays)));
        }

    }
}
