using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Collections;

namespace Lpp.Dns.DataMart.Lib
{
    /// <summary>
    /// Class that holds datamart specific settings
    /// </summary>
    [XmlRootAttribute("DataMartDescription", Namespace = "", IsNullable = false)]
    [Serializable]
    public class DataMartDescription
    {
        private Guid _dataMartId = Guid.Empty;
        private string _dataMartName = string.Empty;
        private string _dataSourceName = string.Empty;
        private bool _allowUnattendedOperation = false;
        private int _pollingInterval = 0;
        private bool _processQueriesAndUploadAutomatically = false;
        private bool _processQueriesAndNotUpload = false;
        private bool _notifyOfNewQueries = false;
        private string _threshHoldCellCount = string.Empty;
        private bool _enableResponseCaching = true;
        private decimal _daysToRetainCacheItems = 30m;
        private bool _encryptCacheItems = false;
        private bool _enableExplictCacheRemoval = false;

        public DataMartDescription()
        {
        }

        [XmlAttribute]
        public Guid DataMartId
        {
            get { return _dataMartId; }
            set { _dataMartId = value; }
        }
        [XmlAttribute]
        public string DataMartName
        {
            get { return _dataMartName; }
            set { _dataMartName = value; }
        }

        public string OrganizationId { get; set; }
        public string OrganizationName { get; set; }

        public string DataSourceName
        {
            get { return _dataSourceName; }
            set { _dataSourceName = value; }
        }

        public bool AllowUnattendedOperation
        {
            get { return _allowUnattendedOperation; }
            set { _allowUnattendedOperation = value; }
        }

        public int PollingInterval
        {
            get { return _pollingInterval; }
            set { _pollingInterval = value; }
        }

        public bool ProcessQueriesAndNotUpload
        {
            get { return _processQueriesAndNotUpload; }
            set { _processQueriesAndNotUpload = value; }
        }

        public bool ProcessQueriesAndUploadAutomatically
        {
            get { return _processQueriesAndUploadAutomatically; }
            set { _processQueriesAndUploadAutomatically = value; }
        }

        public bool NotifyOfNewQueries
        {
            get { return _notifyOfNewQueries; }
            set { _notifyOfNewQueries = value; }
        }

        public string ThreshHoldCellCount
        {
            get { return _threshHoldCellCount; }
            set { _threshHoldCellCount = value; }

        }
        public bool EnableResponseCaching
        {
            get { return _enableResponseCaching; }
            set { _enableResponseCaching = value; }
        }

        public decimal DaysToRetainCacheItems
        {
            get { return _daysToRetainCacheItems; }
            set { _daysToRetainCacheItems = value; }
        }

        public bool EncryptCacheItems
        {
            get { return _encryptCacheItems; }
            set { _encryptCacheItems = value; }
        }

        public bool EnableExplictCacheRemoval
        {
            get { return _enableExplictCacheRemoval; }
            set { _enableExplictCacheRemoval = value; }
        }

        [XmlArrayItem(ElementName = "ModelDescription", Type = typeof(ModelDescription))]
        [XmlArray(ElementName = "ModelDescriptions")]
        public List<ModelDescription> ModelList
        {
            get;
            set;
        }

    }
}
