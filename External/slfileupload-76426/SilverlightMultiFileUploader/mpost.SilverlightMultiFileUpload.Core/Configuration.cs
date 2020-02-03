using System.Collections.Generic;
using System.Windows.Browser;
using mpost.SilverlightMultiFileUpload.Utils;

/*
* Copyright Michiel Post
* http://www.michielpost.nl
* contact@michielpost.nl
*
* http://www.codeplex.com/SLFileUpload/
*
* */

namespace mpost.SilverlightMultiFileUpload.Core
{
    /// <summary>
    /// Singleton configuration class
    /// </summary>
    public class Configuration
    {

        public string CustomParams { get; set; }

        [ScriptableMember()]
        public string FileFilter { get; set; }

        public string UploadHandlerName { get; set; }

        public int MaxUploads { get; set; }
        public long MaxFileSize { get; set; }

        public long ChunkSize { get; set; }
        public long WcfChunkSize { get; set; }

        private int _testInt;
        private long _testLong;

        private const string CustomParamsParam = "CustomParams";
        private const string MaxUploadsParam = "MaxUploads";
        private const string MaxFileSizeKBParam = "MaxFileSizeKB";
        private const string ChunkSizeParam = "ChunkSize";
        private const string FileFilterParam = "FileFilter";
        private const string UploadHandlerNameParam = "UploadHandlerName";

        private static Configuration instance;

        private Configuration() { }

        public static Configuration Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Configuration();
                }
                return instance;
            }
        }


        /// <summary>
        /// Load configuration first from initParams, then from .Config file
        /// </summary>
        /// <param name="initParams"></param>
        public void Initialize(IDictionary<string, string> initParams)
        {
            //Defaults:
            MaxUploads = 2;
            ChunkSize = 1024 * 4096;
            WcfChunkSize = 16 * 1024;
            MaxFileSize = int.MaxValue;

            //Load settings from Init Params (if available)
            LoadFromInitParams(initParams);


            //Overwrite initParams using settings from .config file
            LoadFromConfigFile();

        }

        /// <summary>
        ///  Load settings from Init Params (if available)
        /// </summary>
        /// <param name="initParams"></param>
        private void LoadFromInitParams(IDictionary<string, string> initParams)
        {
            //Load Custom Config String
            if (initParams.ContainsKey(CustomParamsParam) && !string.IsNullOrEmpty(initParams[CustomParamsParam]))
                CustomParams = initParams[CustomParamsParam];

            if (initParams.ContainsKey(MaxUploadsParam) && !string.IsNullOrEmpty(initParams[MaxUploadsParam]))
            {
                if (int.TryParse(initParams[MaxUploadsParam], out _testInt))
                    MaxUploads = int.Parse(initParams[MaxUploadsParam]);
            }

            if (initParams.ContainsKey(MaxFileSizeKBParam) && !string.IsNullOrEmpty(initParams[MaxFileSizeKBParam]))
            {
                if (long.TryParse(initParams[MaxFileSizeKBParam], out _testLong))
                    MaxFileSize = long.Parse(initParams[MaxFileSizeKBParam]) * 1024;
            }

            if (initParams.ContainsKey(ChunkSizeParam) && !string.IsNullOrEmpty(initParams[ChunkSizeParam]))
            {
                if (long.TryParse(initParams[ChunkSizeParam], out _testLong))
                {
                    //Minimum Chunksize is 4096 bytes
                    if(_testLong >= 4096)
                        ChunkSize = int.Parse(initParams[ChunkSizeParam]);
                }
            }

            if (initParams.ContainsKey(FileFilterParam) && !string.IsNullOrEmpty(initParams[FileFilterParam]))
                FileFilter = initParams[FileFilterParam];

            if (initParams.ContainsKey(UploadHandlerNameParam) && !string.IsNullOrEmpty(initParams[UploadHandlerNameParam]))
                UploadHandlerName = initParams[UploadHandlerNameParam];
        }

        /// <summary>
        /// Load settings from .config file
        /// </summary>
        private void LoadFromConfigFile()
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[MaxFileSizeKBParam]))
            {
                if (long.TryParse(ConfigurationManager.AppSettings[MaxFileSizeKBParam], out _testLong))
                {
                    MaxFileSize = long.Parse(ConfigurationManager.AppSettings[MaxFileSizeKBParam]) * 1024;
                }
            }

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[MaxUploadsParam]))
            {
                if (int.TryParse(ConfigurationManager.AppSettings[MaxUploadsParam], out _testInt))
                {
                    MaxUploads = int.Parse(ConfigurationManager.AppSettings[MaxUploadsParam]);
                }
            }

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[FileFilterParam]))
                FileFilter = ConfigurationManager.AppSettings[FileFilterParam];
        }

    }
}
