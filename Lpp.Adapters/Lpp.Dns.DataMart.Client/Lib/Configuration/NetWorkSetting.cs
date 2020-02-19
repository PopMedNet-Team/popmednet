using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using Lpp.Dns.DataMart.Lib.Classes;
using System.ServiceModel;
using Lpp.Dns.DataMart.Lib;
using Lpp.Dns.DataMart.Client.Utils;
using Lpp.Dns.DTO.DataMartClient;

namespace Lpp.Dns.DataMart.Lib
{
    [Serializable()]
    [XmlRootAttribute("NetWorkSetting", Namespace = "", IsNullable = false)]
    public class NetWorkSetting : System.ICloneable
    {
        #region Constructor
        public NetWorkSetting()
        {
            CredentialKey = string.Format("DMCNetwork_{0:D}", Guid.NewGuid());

            EncryptionSalt = Lpp.Dns.DataMart.Client.Properties.Settings.Default.EncryptionSalt;
        }
        #endregion

        #region Fields

        private int _NetworkId = Configuration.ALL_NETWORKS_ID;
        private string _NetworkName = string.Empty;
        private string _HubWebServiceUrl = string.Empty;
        private string _Username = string.Empty;
        private string _Password = string.Empty;
        private bool _isAutoLoginEnabled = false;
        private string _NetworkStatus = string.Empty;
        private string _NetworkMessage = string.Empty;
        private Guid _NetworkUserId = Guid.Empty;
        private bool _IsAuthenticated = false;
        private string _wcfReceiveTimeout = "120";
        private int _networkRefreshRate = 300;
        private List<DataMartDescription> _DataMartList = new List<DataMartDescription>();
        [NonSerialized]
        private HubRequest[] _QueriesDataMart = null;
        //private QueryDataMart[] _QueriesDataMart = null;
        private int[] _UserRights = null;
        private System.Net.CookieContainer _HttpCookieContainer = new System.Net.CookieContainer() ;
        [NonSerialized]
        private SFTPClient _sftpClient = null;

        #endregion

        #region Properties
        [XmlAttribute]
        public int NetworkId
        {
            get { return _NetworkId; }
            set { _NetworkId = value; }
        }
        [XmlAttribute]
        public string NetworkName
        {
            get { return _NetworkName; }
            set { _NetworkName = value; }
        }

        [XmlAttribute]
        public string CredentialKey
        {
            get;
            set;
        }

        public string HubWebServiceUrl
        {
            get { return _HubWebServiceUrl; }
            set { _HubWebServiceUrl = value; }
        }

        public string X509CertThumbprint { get; set; }

        [XmlAttribute]
        public string Host { get; set; }

        [XmlAttribute]
        public int Port { get; set; }

        /// <summary>
        /// The refresh rate in seconds.
        /// </summary>
        [XmlAttribute]
        public int RefreshRate
        {
            get { return _networkRefreshRate; }
            set { _networkRefreshRate = value; }
        }

        [XmlIgnore]
        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }
                
        //Always store encrypted password
        [XmlIgnore]
        public string EncryptedPassword
        {            
            get { return _Password; }
            set { _Password = (string.IsNullOrEmpty(value))? string.Empty : Util.Encrypt(value); }
        }

        [XmlIgnore]
        public string DecryptedPassword
        {
            get { return (string.IsNullOrEmpty(_Password))? string.Empty : Util.Decrypt(_Password); }            
        }

        [XmlIgnore]
        public string Password
        {
            set { _Password = value; }
            get { return _Password; }
        }

        public bool IsAutoLogin
        {
            set { _isAutoLoginEnabled = value; }
            get { return _isAutoLoginEnabled; }
        }
        
        public Guid NetworkUserId
        {
            get { return _NetworkUserId; }
            set { _NetworkUserId = value; }
        }

        public RequestFilter Filter { get; set; }

        private int _pageSize = 25;
        public int PageSize { get { return _pageSize; } set { _pageSize = Math.Max( 1, value ); } }

        public RequestSortColumn? Sort { get; set; }
        public bool? SortAscending { get; set; }

        [XmlIgnore]
        public bool IsAuthenticated
        {
            get { return _IsAuthenticated; }
            set { _IsAuthenticated = value; }
        }

        [XmlIgnore]
        public string NetworkStatus
        {
            get { return _NetworkStatus; }
            set { _NetworkStatus = value; }
        }

        [XmlIgnore]
        public string NetworkMessage
        {
            get { return _NetworkMessage; }
            set { _NetworkMessage = value; }
        }

        [XmlIgnore] 
        public Profile Profile { get; set; }

        [XmlArrayItem(ElementName = "DataMartDescription", Type = typeof(DataMartDescription))]
        [XmlArray(ElementName = "DataMartDescriptions")]
        public List<DataMartDescription> DataMartList
        {
            get { return _DataMartList; }
            set { _DataMartList = value; }
        }

        /// <summary>
        /// The list of Rights for the user.
        /// </summary>
        [XmlIgnore]
        public int[] UserRights
        {
            get { return _UserRights; }
            set { _UserRights = value; }
        }

        [XmlIgnore]
        public HubRequest[] DnsRequests
        {
            get { return _QueriesDataMart; }
            set { _QueriesDataMart = value; }
        }

        [XmlIgnore]
        public System.Net.CookieContainer HttpCookieContainer
        {
            get { return (null == _HttpCookieContainer)? new System.Net.CookieContainer() : _HttpCookieContainer; }
            set { _HttpCookieContainer = value; }
        }

        [XmlIgnore]
        public SFTPClient SftpClient
        {
            get { return _sftpClient; }
            set { _sftpClient = value; }
        }

        public String WcfReceiveTimeout
        {
            get { return _wcfReceiveTimeout; }
            set { _wcfReceiveTimeout = value; }
        }

        [XmlIgnore]
        public TimeSpan WcfReceiveTimoutTimeSpan
        {
            get
            {
                int wcfReceiveTimeout;
                if (!int.TryParse(_wcfReceiveTimeout, out wcfReceiveTimeout))
                    wcfReceiveTimeout = 120;
                return new TimeSpan(0,0,wcfReceiveTimeout);
            }
        }

        [XmlIgnore]
        public string EncryptionSalt
        {
            get;
            private set;
        }

        #endregion

        #region Public Instance Methods

        public object Clone()
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, this);
            ms.Position = 0;
            object obj = bf.Deserialize(ms);
            ms.Close();
            return obj;
        }
        /// <summary>
        /// Get the DataMart owned by the current user
        /// </summary>
        public List<DataMartDescription> GetDataMartsByUser()
        {
            try
            {
                // Call the method to return the DataMarts owned by the current user
                HubDataMart[] _dataMart = DnsServiceManager.GetDataMarts(this);

                if (_dataMart == null || _dataMart.Length == 0)
                {
                    NetworkMessage = "You are not listed as the Administrator for any DataMarts.  Please contact your system administrator.";
                    return null;
                }

                var TempDatamartList = new List<DataMartDescription>();

                foreach (HubDataMart dm in _dataMart)
                {
                    if (null != dm)
                    {
                        DataMartDescription dd = new DataMartDescription();
                        dd.DataMartId = dm.DataMartId;
                        dd.DataMartName = dm.DataMartName;
                        dd.OrganizationId = dm.OrganizationId;
                        dd.OrganizationName = dm.OrganizationName;
                        dd.PollingInterval = 60;
                        dd.AllowUnattendedOperation = false;
                        TempDatamartList.Add(dd);
                    }
                }

                if (_DataMartList != null)
                {
                    var models = DnsServiceManager.GetModels(this);

                    foreach (DataMartDescription dd in TempDatamartList)
                    {
                        foreach (DataMartDescription ddExisting in _DataMartList)
                        {
                            if (ddExisting.DataMartId == dd.DataMartId)
                            {
                                dd.DataSourceName = ddExisting.DataSourceName;
                                dd.AllowUnattendedOperation = ddExisting.AllowUnattendedOperation;
                                dd.PollingInterval = ddExisting.PollingInterval;
                                dd.ProcessQueriesAndNotUpload = ddExisting.ProcessQueriesAndNotUpload;
                                dd.ProcessQueriesAndUploadAutomatically = ddExisting.ProcessQueriesAndUploadAutomatically;
                                dd.ThreshHoldCellCount = ddExisting.ThreshHoldCellCount;
                                dd.NotifyOfNewQueries = ddExisting.NotifyOfNewQueries;
                                dd.EnableExplictCacheRemoval = ddExisting.EnableExplictCacheRemoval;
                                dd.EnableResponseCaching = ddExisting.EnableResponseCaching;
                                dd.EncryptCacheItems = ddExisting.EncryptCacheItems;
                                dd.DaysToRetainCacheItems = ddExisting.DaysToRetainCacheItems;

								dd.ModelList = (ddExisting.ModelList != null && !ddExisting.ModelList.IsEmpty()) ?
													 (from model in models[dd.DataMartId].EmptyIfNull()
													  join e in ddExisting.ModelList on model.Id equals e.ModelId into exts
													  from ex in exts.DefaultIfEmpty()
													  let e = ex ?? new ModelDescription { ModelId = model.Id, ModelName = model.Name, ModelDisplayName = model.Name, ProcessorId = model.ModelProcessorId }
													  select e).ToList() : new List<ModelDescription>();
								

                            }

                        }         
                    }
                }

                _DataMartList = TempDatamartList;
                return _DataMartList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get configured DataMart description for a network datamart
        /// </summary>
        /// <param name="DataMartId">DataMart Id</param>
        /// <returns>DataMart Description object</returns>
        public DataMartDescription GetDataMartDescription(Guid dataMartId)
        {
            DataMartDescription returnDescription = null;
            try
            {
                if (null != DataMartList && DataMartList.Count > 0)
                {
                    foreach (DataMartDescription dd in DataMartList)
                    {
                        if (dd != null && dd.DataMartId == dataMartId)
                        {
                            returnDescription = dd;
                            break;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnDescription;
        }

        #endregion

        public Model.NetworkConnectionMetadata CreateInterfaceMetadata()
        {
            return new Model.NetworkConnectionMetadata
            {
                UserLogin = Profile.Username,
                UserEmail = Profile.Email,
                UserFullName = Profile.FullName,
                OrganizationName = Profile.OrganizationName
            };
        }
    }

    public interface IRequestFilter
    {
        Guid[] DataMartIds { get; set; }
        Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus[] Statuses { get; set; }

        DateRangeKind DateRange { get; set; }
        int? RecentDaysToShow { get; set; }
        DateTime? FromDate { get; set; }
        DateTime? ToDate { get; set; }
    }

    public enum DateRangeKind 
    { 
        RecentDays,
        Exact
    }

    [Serializable()]
    public struct RequestFilter : IRequestFilter
    {
        public Guid[] DataMartIds { get; set; }

        public Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus[] Statuses { get; set; }

        public DateRangeKind DateRange { get; set; }

        public int? RecentDaysToShow { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public DateTime EffectiveFromDate 
        { get 
            { 
                return ( DateRange == DateRangeKind.Exact && FromDate != null ) ? FromDate.Value : DateTime.Today.ToUniversalTime().AddDays( - ( RecentDaysToShow ?? DefaultRecentDaysToShow ) ); 
            }
        }
        
        public DateTime EffectiveToDate 
        {
            get 
            { 
                return DateRange == DateRangeKind.Exact ? (ToDate ?? DateTime.UtcNow) : DateTime.UtcNow; 
            }
        }

        const int DefaultRecentDaysToShow = 30;
    }
}