using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DMClient = Lpp.Dns.DataMart.Client;
using Lpp.Dns.DataMart.Lib;
using Lpp.Dns.DataMart.Lib.Utils;
using log4net;
using Lpp.Dns.DataMart.Lib.Classes;
using Lpp.Dns.DataMart.Model;
using System.Diagnostics.Contracts;
using System.Collections;

namespace Lpp.Dns.DataMart.Client.Utils
{
    /// <summary>
    /// Singleton instance that manages NetWorkCollection, NetWorkSettings, DataMartDescriptions, ModelDescriptions loaded from NetworkSettings.xml file.
    /// </summary>
    public class Configuration
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Constants

        private const string NetworkSettingsFileName = "NetWorkSettings.xml";

        public const int NO_NETWORK_ID = -1;
        public const int ALL_NETWORKS_ID = 0;
        //public const int ALL_DATAMARTS_ID = 0;
        public static readonly Guid ALL_DATAMARTS_ID = Guid.Empty;
        public const int ALL_STATUS_ID = 0;
        public const int DEFAULT_DATES_FILTER_ID = 0;

        public const string NO_NETWORK_NAME = "None";
        public const string ALL_NETWORKS_NAME = "All";
        public const string ALL_DATAMARTS_NAME = "All";
        public const string ALL_STATUS_NAME = "All";

        #endregion

        #region Properties

        public NetWorkSettingCollection NetworkSettingCollection
        {
            get;
            set;
        }

        public static string AppFolderPath
        {
            get
            {
                return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DMClient.Properties.Settings.Default.AppDataFolderName);
            }
        }

        public static string PackagesFolderPath
        {
            get
            {
                return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DMClient.Properties.Settings.Default.AppDataFolderName, "packages"); 
            }
        }

        #endregion

        #region Singleton Instance

        private static Configuration configuration;
        private static readonly object lockObj = new object();

        public static Configuration Instance
        {
            get
            {
                if (configuration != null)
                    return configuration;

                lock (lockObj)
                {
                    configuration = new Configuration();
                }
                return configuration;
            }
        }

        private Configuration()
        {
            try
            {
                log.Info("Loading configuration.");
                string NetworkSettingsFilePath = GetNetworkSettingsFilePath();

                if (string.IsNullOrEmpty(NetworkSettingsFilePath))
                {
                    CreateNewNetworkSettingsFile();
                    NetworkSettingsFilePath = GetNetworkSettingsFilePath();
                }

                NetworkSettingCollection = NetWorkSettingCollection.Deserialize(NetworkSettingsFilePath);
                foreach (NetWorkSetting networkSetting in NetworkSettingCollection.NetWorkSettings)
                {
                    string username, password;
                    CredentialManager.GetCredential(networkSetting.CredentialKey, out username, out password);
                    networkSetting.Username = username;
                    networkSetting.Password = password;
                    // Get model passwords that were auto-saved in creential manager
                    GetModelPasswords(networkSetting.CredentialKey, networkSetting.DataMartList);
                }


                if (!Directory.Exists(PackagesFolderPath))
                {
                    Directory.CreateDirectory(PackagesFolderPath);
                }

                //copy the base plugin dependencies to the root packages folder, this is where the app domains set their base directory
                //doing this at startup regardless of if the assemblies exist so that the most current are always there.
                CopyToPluginBase("ICSharpCode.SharpZipLib.dll", AppDomain.CurrentDomain.BaseDirectory, PackagesFolderPath);
                CopyToPluginBase("log4net.dll", AppDomain.CurrentDomain.BaseDirectory, PackagesFolderPath);
                CopyToPluginBase("Lpp.Dns.DataMart.Client.DomainManger.dll", AppDomain.CurrentDomain.BaseDirectory, PackagesFolderPath);
                CopyToPluginBase("Lpp.Dns.DataMart.Model.Interface.dll", AppDomain.CurrentDomain.BaseDirectory, PackagesFolderPath);
                CopyToPluginBase("Lpp.Dns.DataMart.Client.exe", AppDomain.CurrentDomain.BaseDirectory, PackagesFolderPath);

                //delete previous dependencies from the package folder so that there is no conflict with versions in adapter packages
                DeleteFromPluginBase("Npgsql.dll", PackagesFolderPath);
                DeleteFromPluginBase("MySql.Data.dll", PackagesFolderPath);
                DeleteFromPluginBase("Oracle.ManagedDataAccess.dll", PackagesFolderPath);

            }
            catch (ArgumentException ex)
            {
                log.Debug(ex.Message, ex);
                throw new NetworkSettingsFileNotFound("No Network configured. Add new Network from File > Network Settings", ex);
            }
            catch (BadImageFormatException ex)
            {
                log.Error(ex.Message, ex);
                // Credential manager is built for a different architecture. This is a build error and should not occur.
                // The system can still run but username and password cannot be saved.
            }
            catch (Exception ex)
            {
                log.Error("Cannot fully instantiate Configuration singleton instance.", ex);
                throw;
            }
        }

        static void CopyToPluginBase(string filename, string currentBase, string pluginBase)
        {
            string destination = Path.Combine(pluginBase, filename);
            if (File.Exists(destination))
            {
                File.Delete(destination);
            }
            File.Copy(Path.Combine(currentBase, filename), destination);
        }

        static void DeleteFromPluginBase(string filename, string pluginBase)
        {
            string filepath = Path.Combine(pluginBase, filename);
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
        }

        #endregion

        #region Network Configuration

        public NetWorkSetting GetNetworkSetting(int networkId)
        {
            if (NetworkSettingCollection.NetWorkSettings == null)
                return null;

            return (from x in NetworkSettingCollection.NetWorkSettings.ToArray(typeof(NetWorkSetting)) as NetWorkSetting[]
                    where x != null && x.NetworkId == networkId
                    select x).FirstOrDefault<NetWorkSetting>();
        }

        public DataMartDescription GetDataMartDescription(int networkId, Guid dataMartId)
        {
            var networkSetting = GetNetworkSetting(networkId);

            if (networkSetting == null || networkSetting.DataMartList == null) 
                return null;

            return networkSetting.DataMartList.FirstOrDefault( x => x.DataMartId == dataMartId );
        }

        public ModelDescription GetModelDescription(int networkId, Guid dataMartId, Guid modelId)
        {
            var dataMartDescription = GetDataMartDescription(networkId, dataMartId);

            if (dataMartDescription == null)
                return null;

            if (dataMartDescription.ModelList == null)
                LoadModels(GetNetworkSetting(networkId));

            return dataMartDescription.ModelList.Cast<ModelDescription>().FirstOrDefault( m => m.ModelId == modelId );
        }

        public void LoadModels( NetWorkSetting ns )
        {
            foreach ( var d in ns.DataMartList )
            {
                d.ModelList = (from m in DnsServiceManager.GetModels( d.DataMartId, ns ).EmptyIfNull()
                               join existing in d.ModelList.EmptyIfNull() on m.Id equals existing.ModelId into exts
                               from existing in exts.DefaultIfEmpty()
                               select existing ?? new ModelDescription { ModelId = m.Id, ModelName = m.Name, ProcessorId = m.ModelProcessorId }
                              ).ToList();
            }
        }

        #endregion

        #region Network Configuration Lists for ComboBoxes

        public IList<NetWorkSetting> GetNetworkSelections()
        {
            NetWorkSetting[] networkSettings = NetworkSettingCollection.NetWorkSettings.ToArray(typeof(NetWorkSetting)) as NetWorkSetting[];
            IList<NetWorkSetting> networkSettingsList = networkSettings.ToList<NetWorkSetting>();
            NetWorkSetting allNetworksSetting = new NetWorkSetting() { NetworkId = ALL_NETWORKS_ID, NetworkName = ALL_NETWORKS_NAME };
            networkSettingsList.Insert(0, allNetworksSetting);
            return networkSettingsList;
        }

        public IList<DataMartDescription> GetDataMartSelections(NetWorkSetting networkSetting)
        {
            List<DataMartDescription> dataMartDescriptionList = null;

            if (networkSetting.NetworkId == ALL_NETWORKS_ID) // All
            {
                dataMartDescriptionList = new List<DataMartDescription>();
                NetWorkSetting[] networkSettings = NetworkSettingCollection.NetWorkSettings.ToArray(typeof(NetWorkSetting)) as NetWorkSetting[];
                foreach (NetWorkSetting network in networkSettings)
                    if(network.DataMartList != null && network.DataMartList.Count > 0)
                        dataMartDescriptionList.AddRange(network.DataMartList);

                DataMartDescription allDataMartDescription = new DataMartDescription() { DataMartId = ALL_DATAMARTS_ID, DataMartName = ALL_DATAMARTS_NAME };
                dataMartDescriptionList.Insert(0, allDataMartDescription);
                return dataMartDescriptionList;
            }
            else
            {
                if (networkSetting.DataMartList != null && networkSetting.DataMartList.Count > 0)
                {
                    dataMartDescriptionList = networkSetting.DataMartList.ToList<DataMartDescription>();
                }
                else
                    dataMartDescriptionList = new List<DataMartDescription>();
                DataMartDescription allDataMartDescription = new DataMartDescription() { DataMartId = ALL_DATAMARTS_ID, DataMartName = ALL_DATAMARTS_NAME };
                dataMartDescriptionList.Insert(0, allDataMartDescription);
                return dataMartDescriptionList;
            }
        }

        public static void LogNetworkSettingsFile()
        {
            try
            {
                using (var file = new StreamReader(GetNetworkSettingsFilePath()))
                {
                    log.Debug("Network Settings: " + file.ReadToEnd());
                }
            }
            catch (Exception e)
            {
            }
        }
        #endregion

        #region Private Methods

        private static string GetNetworkSettingsFilePath()
        {
            string appFilepath = string.Empty;
            try
            {
                appFilepath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DMClient.Properties.Settings.Default.AppDataFolderName, NetworkSettingsFileName);
                if (!File.Exists(appFilepath))
                    appFilepath = string.Empty;
            }
            catch (Exception )
            {
                appFilepath = string.Empty;
            }

            return appFilepath;
        }

        private static int FindUnusedNetworkId()
        {
            int i = 1;
            while(true)
            {
                if (Configuration.Instance.GetNetworkSetting(i) != null)
                    i++;
                else
                    return i;
            }
        }

        #endregion

        public static void DeleteModelPasswords(NetWorkSetting network)
        {
            foreach (var dataMart in network.DataMartList)
            {
                foreach (var model in dataMart.ModelList)
                {
                    foreach (var property in model.Properties)
                    {
                        if (property.Name.ToLower() == "password")
                        {
                            CredentialManager.DeleteCred(network.CredentialKey + "_" + model.ProcessorId);
                        }
                    }
                }
            }
        }

        
        private static void GetModelPasswords(string path, IList<DataMartDescription> dataMarts)
        {
            string username, password;
            if (dataMarts != null)
            {
                foreach (var dataMart in dataMarts)
                {
                    if (dataMart.ModelList != null)
                    {
                        foreach (var model in dataMart.ModelList)
                        {
                            if (model.Properties != null)
                            {
                                foreach (var property in model.Properties)
                                {
                                    if (property.Name.ToLower() == "password")
                                    {
                                        CredentialManager.GetCredential(path + "_" + dataMart.DataMartName + "_" + model.ProcessorId + "_" + model.ModelId, out username, out password);
                                        property.Value = password;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void SaveModelPasswords(NetWorkSettingCollection networks)
        {
            foreach (var network in networks.NetWorkSettings)
            {
                if (((NetWorkSetting)network).DataMartList != null)
                {
                    // Get model passwords that were auto-saved in creential manager
                    foreach (var dataMart in ((NetWorkSetting)network).DataMartList)
                    {
                        if (dataMart.ModelList != null)
                        {
                            foreach (var model in dataMart.ModelList)
                            {
                                if (model.Properties != null)
                                {
                                    foreach (var property in model.Properties)
                                    {
                                        if (property.Name.ToLower() == "password")
                                        {
                                            CredentialManager.SaveCredential(((NetWorkSetting)network).CredentialKey + "_" + dataMart.DataMartName + "_" + model.ProcessorId + "_" + model.ModelId, model.ModelName, property.Value);
                                            property.Value = string.Empty;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void GetModelPasswords(NetWorkSettingCollection networks)
        {
            foreach (var network in networks.NetWorkSettings)
            {
                GetModelPasswords(((NetWorkSetting)network).CredentialKey, ((NetWorkSetting)network).DataMartList);
            }
        }

        public static void CreateNewNetworkSettingsFile()
        {

            NetWorkSettingCollection nwc = new NetWorkSettingCollection();
            CreateNetworkSettingsFile(nwc.Serialize);
        }

        public static bool SaveNetworkSettings()
        {
            bool success = false;
            // Save passwords in credential manager
            SaveModelPasswords(Instance.NetworkSettingCollection);
            success = CreateNetworkSettingsFile(Instance.NetworkSettingCollection.Serialize);
            GetModelPasswords(Instance.NetworkSettingCollection);
            
            return success;
        }

        /// <summary>
        /// create new settings file
        /// </summary>
        /// <param name="fileContent">Xml file content</param>
        /// <returns>boolean specifying the existence of network setting file name</returns>
        public static bool CreateNetworkSettingsFile( Action<string> writeFile )
        {
            bool isAppSettingsFolderAvailable = false;
            try
            {
                if (!Directory.Exists(AppFolderPath)) Directory.CreateDirectory(AppFolderPath);

                writeFile( string.Format("{0}\\{1}", AppFolderPath, NetworkSettingsFileName) );
                isAppSettingsFolderAvailable = true;
            }
            catch (Exception )
            {
                isAppSettingsFolderAvailable = false;
            }
            return isAppSettingsFolderAvailable;
        }

        public void AddNetworkSetting(NetWorkSetting ns)
        {
            ns.NetworkId = FindUnusedNetworkId();
            NetworkSettingCollection.NetWorkSettings.Add(ns);
        }

    }
}
