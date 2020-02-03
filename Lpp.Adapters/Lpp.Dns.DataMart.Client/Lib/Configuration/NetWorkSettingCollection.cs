using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using Lpp.Dns.DataMart.Lib.Classes;
using Lpp.Dns.DataMart.Lib;
using log4net;

namespace Lpp.Dns.DataMart.Lib
{
    /// <summary>
    /// Helper class to read/ write Network settings. 
    /// </summary>
    public class NetWorkSettingCollection
    {
        #region Constructor

        public NetWorkSettingCollection()
        {
        }

        #endregion

        #region Private Fields
        private ArrayList _NetWorkSettingCollection = new ArrayList();
        private List<HubRequest> _NetWorkQueryDataMartList = null;
        private bool _HasCollectionChanged = false;
        private string _QDMSortColumnName = "NETWORKNAME";
        private bool _IsAscending = false;
        static readonly ILog Log = LogManager.GetLogger( "NetworkSettingCollection" );
        #endregion

        #region Public Properties

        [XmlArrayItem(ElementName = "NetworkSetting", Type = typeof(NetWorkSetting))]
        [XmlArray(ElementName = "NetworkSettingCollection")]
        public ArrayList NetWorkSettings
        {
            get { return _NetWorkSettingCollection; }
            set { _NetWorkSettingCollection = value; }
        }

        [XmlIgnore]
        public List<HubRequest> QueryDataMartList
        {
            get { return _NetWorkQueryDataMartList; }
            set { _NetWorkQueryDataMartList = value; }
        }

        [XmlIgnore]
        public bool HasCollectionChanged
        {
            get { return _HasCollectionChanged; }
            set { _HasCollectionChanged = value; }
        }

        [XmlIgnore]
        public string QDMSortColumnName
        {
            get { return _QDMSortColumnName; }
            set { _QDMSortColumnName = value; }
        }

        [XmlIgnore]
        public bool IsAscending
        {
            get { return _IsAscending; }
            set { _IsAscending = value; }
        }
        
        #endregion

        #region Public Instance methods

        public void Serialize( string filePath )
        {
            try
            {
                _HasCollectionChanged = false;
                using ( var s = File.Create( filePath ) )
                {
                    _serializer.Serialize( new StreamWriter( s, Encoding.UTF8 ), this );
                }
            }
            catch (Exception ex)
            {
                Log.Error( ex );
            }
        }

        public DataTable GetNetworkDataMarts(int NetWorkId)
        {
            DataTable NetworkDataMartsCollection = null;
            if (_NetWorkSettingCollection != null && _NetWorkSettingCollection.Count > 0)
            {
                foreach (NetWorkSetting ns in _NetWorkSettingCollection)
                {
                    if (ns != null && (ns.NetworkStatus == Util.LoggedInStatus || ns.NetworkStatus == Util.ConnectionOKStatus))
                    {         
                        if (NetWorkId > 0 && ns.NetworkId == NetWorkId) 
                        {
                            return GetNetworkDataMarts(ns, true, false);                               
                        }
                        else if (NetWorkId == 0)
                        {
                            DataTable dt = GetNetworkDataMarts(ns, (NetworkDataMartsCollection == null) ? true : false, true);

                            if (null == NetworkDataMartsCollection)
                                NetworkDataMartsCollection = dt;
                            else
                            {
                                if (dt != null)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        NetworkDataMartsCollection.ImportRow(dr);
                                    }                                        
                                }
                            }
                        }                            
                    }
                }
            }

            return NetworkDataMartsCollection;
        }
        #endregion

        #region Public Static methods

        static readonly XmlSerializer _serializer = new XmlSerializer( typeof( NetWorkSettingCollection ) );
        public static NetWorkSettingCollection Deserialize( string settingsFileName )
        {
            using ( var fs = File.OpenRead( settingsFileName ) )
            {
                return (NetWorkSettingCollection)_serializer.Deserialize( new StreamReader( fs, Encoding.UTF8 ) );
            }
        }

        public static DataTable GetNetworkDataMarts(NetWorkSetting networkSetting,bool shouldAddHeaderRow , bool isPrefixedDataMartName)
        {
                                   
            DataRow dr  = null;
            DataTable NetworkDataMartCollection = new DataTable();

            DataColumn dcNetworkId = new DataColumn("NetworkId");
            DataColumn dcDataMartId = new DataColumn("DataMartId");
            DataColumn dcDataMartDisplayName = new DataColumn("DataMartName");

            NetworkDataMartCollection.Columns.Add(dcNetworkId);
            NetworkDataMartCollection.Columns.Add(dcDataMartId);
            NetworkDataMartCollection.Columns.Add(dcDataMartDisplayName);

            if (shouldAddHeaderRow)
            {
                
                dr = NetworkDataMartCollection.NewRow();
                dr["NetworkId"] = 0;
                dr["DataMartId"] = 0;
                dr["DataMartName"] = "All";

                NetworkDataMartCollection.Rows.Add(dr);
            }            

            try
            {

                if (null != networkSetting.DataMartList && networkSetting.DataMartList.Count > 0)
                {
                    foreach (DataMartDescription dd in networkSetting.DataMartList)
                    {
                        if (null != dd)
                        {
                            dr = NetworkDataMartCollection.NewRow();
                            dr["NetworkId"] = networkSetting.NetworkId;
                            dr["DataMartId"] = dd.DataMartId;
                            dr["DataMartName"] = (isPrefixedDataMartName) ? string.Format("{0} : {1} ", networkSetting.NetworkName, dd.DataMartName)
                                : dd.DataMartName;

                            NetworkDataMartCollection.Rows.Add(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return NetworkDataMartCollection;
        }
        #endregion

        #region private Methods
        private String UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }
        #endregion

        public IEnumerable<NetWorkSetting> AsEnumerable()
        {
            return this._NetWorkSettingCollection.Cast<NetWorkSetting>();
        }
    }
}