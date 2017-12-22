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
    [Serializable()]
    public class DataMartDescription
    {

        #region Constructor
        public DataMartDescription()
        {
        }
        #endregion

        #region private Fields
        private Guid _DataMartId = Guid.Empty;
        private string _DataMartName = string.Empty;
        private string _DataSourceName = string.Empty;
        private bool _AllowUnattendedOperation = false;
        private int _PollingInterval = 0;
        private bool _ProcessQueriesAndUploadAutomatically = false;
        private bool _ProcessQueriesAndNotUpload = false;
        private bool _NotifyOfNewQueries = false;
        private string _ThreshHoldCellCount = string.Empty;
        #endregion

        #region Properties
        [XmlAttribute]
        public Guid DataMartId
        {
            get { return _DataMartId; }
            set { _DataMartId = value; }
        }
        [XmlAttribute]
        public string DataMartName
        {
            get { return _DataMartName; }
            set { _DataMartName = value; }
        }

        public string OrganizationId { get; set; }
        public string OrganizationName { get; set; }

        public string DataSourceName
        {
            get { return _DataSourceName; }
            set { _DataSourceName = value; }
        }

        public bool AllowUnattendedOperation
        {
            get { return _AllowUnattendedOperation; }
            set { _AllowUnattendedOperation = value; }
        }

        public int PollingInterval
        {
            get { return _PollingInterval; }
            set { _PollingInterval = value; }
        }

        public bool ProcessQueriesAndNotUpload
        {
            get { return _ProcessQueriesAndNotUpload; }
            set { _ProcessQueriesAndNotUpload = value; }
        }

        public bool ProcessQueriesAndUploadAutomatically
        {
            get { return _ProcessQueriesAndUploadAutomatically; }
            set { _ProcessQueriesAndUploadAutomatically = value; }
        }

        public bool NotifyOfNewQueries
        {
            get { return _NotifyOfNewQueries; }
            set { _NotifyOfNewQueries = value; }
        }

        public string ThreshHoldCellCount
        {
            get { return _ThreshHoldCellCount; }
            set { _ThreshHoldCellCount = value; }

        }

        [XmlArrayItem(ElementName = "ModelDescription", Type = typeof(ModelDescription))]
        [XmlArray(ElementName = "ModelDescriptions")]
        public List<ModelDescription> ModelList
        {
            get;
            set;
        }

        #endregion
    }
}
