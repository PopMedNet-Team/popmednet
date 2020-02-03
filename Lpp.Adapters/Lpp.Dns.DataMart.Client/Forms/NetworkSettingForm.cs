using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Lpp.Dns.DataMart.Model;
using Lpp.Dns.DataMart.Lib;
using Lpp.Dns.DataMart.Client.Utils;
using System.Security;
using log4net;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using Lpp;

namespace Lpp.Dns.DataMart.Client
{
    public partial class NetworkSettingForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public delegate void NetworkSettingChangedEventHandler(object sender, NetWorkSetting ns);
        public event NetworkSettingChangedEventHandler NetworkSettingChanged;

        #region private fields

        private NetWorkSetting _NetworkSetting = null;
        private bool _isNewNetworksetting = false;
        private bool _HasNetworkCredentialsChanged = false;
        #endregion

        #region Properties
        public NetWorkSetting NetworkSettingObject
        {
            get { return _NetworkSetting; }
            set { _NetworkSetting = value != null ? value.Clone() as NetWorkSetting : value; }
        }

        #endregion

        #region Constructors

        public NetworkSettingForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void NetworkSettingForm_Load(object sender, EventArgs e)
        {
            try
            {
                lblStatusMessage.Text = string.Empty;
                btnApply.Enabled = btnOk.Enabled = btnRefreshDMlist.Enabled = false;

                var certs = LoadCerts();
                cbCertificates.DataSource = certs;

                if (_NetworkSetting != null)
                {
                    txtNetWorkId.Text = _NetworkSetting.NetworkId.ToString();
                    txtNetworkname.Text = _NetworkSetting.NetworkName;
                    txtServiceUrl.Text = _NetworkSetting.HubWebServiceUrl;
                    txtUsername.Text = _NetworkSetting.Username;
                    txtPassword.Text = _NetworkSetting.DecryptedPassword;
                    numBoxRefreshRate.Value = _NetworkSetting.RefreshRate;
                    cbCertificates.SelectedItem = certs.FirstOrDefault( c => c.Thumbprint == _NetworkSetting.X509CertThumbprint );
                    chkAutoLogin.Checked = _NetworkSetting.IsAutoLogin;
                    _isNewNetworksetting = false;
                    tbHost.Text = _NetworkSetting.Host;
                    tbPort.Text = _NetworkSetting.Port.ToString();
                    tbReceiveTimeout.Text = _NetworkSetting.WcfReceiveTimeout;


                    if (_NetworkSetting.NetworkStatus == Util.ConnectionFailedStatus)
                        lblStatusMessage.Text = _NetworkSetting.NetworkMessage;

                    //if (_NetworkSetting.IsAuthenticated)
                    if(_NetworkSetting.DataMartList != null)
                    {
                        dgvDataMarts.DataSource = _NetworkSetting.DataMartList;
                        btnRefreshDMlist.Enabled = true;
                    }
                    else
                    {
                        dgvDataMarts.DataSource = null;
                        btnRefreshDMlist.Enabled = false;
                    }
					EnableDMControls(_NetworkSetting.IsAuthenticated);
                }
                else
                {
                    dgvDataMarts.DataSource = null;
                    txtNetWorkId.Text = string.Empty;
                    txtNetworkname.Text = string.Empty;
                    txtServiceUrl.Text = string.Empty;
                    txtUsername.Text = string.Empty;
                    txtPassword.Text = string.Empty;
                    chkAutoLogin.Checked = true;
                    numBoxRefreshRate.Value = 300;
                    _isNewNetworksetting = true;
                }

                if (dgvDataMarts.DataSource == null || dgvDataMarts.RowCount == 0)
                {
                    dgvDataMarts.Enabled = btnEdit.Enabled = false;
                }

                // Act on any unsaved changes by the user.
                chkAutoLogin.CheckedChanged += new EventHandler(OnNetworkSettingChanged);
                txtNetworkname.TextChanged += new EventHandler(OnNetworkSettingChanged);
                txtServiceUrl.TextChanged += new EventHandler(OnNetworkSettingChanged);
                txtUsername.TextChanged += new EventHandler(OnNetworkSettingChanged);
                txtPassword.TextChanged += new EventHandler(OnNetworkSettingChanged);
                tbHost.TextChanged += new EventHandler(OnNetworkSettingChanged);
                tbPort.TextChanged += new EventHandler(OnNetworkSettingChanged);
                tbReceiveTimeout.TextChanged += new EventHandler(OnNetworkSettingChanged);
                numBoxRefreshRate.ValueChanged += new EventHandler(OnNetworkSettingChanged);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                MessageBox.Show(ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnableDMControls(bool enable)
        {
            btnEdit.Enabled = enable;
            dgvDataMarts.Enabled = enable;
            dgvDataMarts.Visible = enable;
            btnRefreshDMlist.Enabled = enable;
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                lblStatusMessage.Text = string.Empty;

                if (!IsValidNetworkSetting())
                    return;

                // Test the network configuration on the UI (may or may not be saved).
                //NetWorkSetting _NetworkSetting = new NetWorkSetting();
                if (_NetworkSetting == null)
                    _NetworkSetting = new NetWorkSetting();
                SaveFields( _NetworkSetting );

                if (DnsServiceManager.LogIn(_NetworkSetting))
                    MessageBox.Show("Connection Successful", Util.ConnectionOKStatus, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show(_NetworkSetting.NetworkMessage, Util.ConnectionFailedStatus, MessageBoxButtons.OK, MessageBoxIcon.Information);

                PopulateDataMarts(ref _NetworkSetting);
				EnableDMControls(_NetworkSetting.IsAuthenticated);
                return;

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                MessageBox.Show(ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }

        private void btnRefreshDMlist_Click(object sender, EventArgs e)
        {
            try
            {
                PopulateDataMarts(ref _NetworkSetting);                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                MessageBox.Show(ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                ShowDataMartDetail();                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                MessageBox.Show(ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            lblStatusMessage.Text = string.Empty;

            try
            {
                if (!IsValidNetworkSetting())
                    return;

                if (_HasNetworkCredentialsChanged && _NetworkSetting != null && !_NetworkSetting.IsAuthenticated)
                    dgvDataMarts.DataSource = null;

                if (SaveNetworkSetting())
                {
                    _isNewNetworksetting = false;

                    PopulateDataMarts(ref _NetworkSetting);

                    txtNetWorkId.Text = _NetworkSetting.NetworkId.ToString();
                    MessageBox.Show("Settings saved successfully");

                    btnApply.Enabled = btnOk.Enabled = false;

                    _NetworkSetting = (NetWorkSetting)Configuration.Instance.GetNetworkSetting(_NetworkSetting.NetworkId);

                    NetworkSettingChanged(sender, _NetworkSetting);

                    if (_NetworkSetting.NetworkStatus == Util.ConnectionFailedStatus)
                        lblStatusMessage.Text = _NetworkSetting.NetworkMessage;
                }

                chkAutoLogin.CheckedChanged += new EventHandler(OnDefaultChanged);
                txtNetworkname.TextChanged += new EventHandler(OnDefaultChanged);
                txtServiceUrl.TextChanged += new EventHandler(OnDefaultChanged);
                txtUsername.TextChanged += new EventHandler(OnDefaultChanged);
                txtPassword.TextChanged += new EventHandler(OnDefaultChanged);
                tbReceiveTimeout.TextChanged += new EventHandler(OnDefaultChanged);
                numBoxRefreshRate.ValueChanged += new EventHandler(OnDefaultChanged);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                MessageBox.Show(ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateAndSave())
                    this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                MessageBox.Show(ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private Boolean ValidateAndSave()
        {
            if (string.IsNullOrEmpty(txtNetworkname.Text))
            {
                MessageBox.Show("Please enter a Network name");
                return false;
            }

            if (SaveNetworkSetting())
            {
                _isNewNetworksetting = false;
                NetworkSettingChanged(null, _NetworkSetting);
                return true;
            }

            return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            try
            {
                this.Close();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                MessageBox.Show(ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvDataMarts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ShowDataMartDetail();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                MessageBox.Show(ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnNetworkSettingChanged(object sender, EventArgs e)
        {
            try
            {
                SetOKApplyButtonStatus();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                MessageBox.Show(ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnDefaultChanged(object sender, EventArgs e)
        {
            // Required to by pass onchanged event on the controls when values get assigned.
        }
        #endregion
        
        #region private methods

        private bool SaveNetworkSetting()
        {
            if (_isNewNetworksetting)
            {
                _NetworkSetting = _NetworkSetting ?? new NetWorkSetting();
                Configuration.Instance.AddNetworkSetting(_NetworkSetting);
            }
            else
            {
                //if (_HasNetworkCredentialsChanged && _NetworkSetting != null && !_NetworkSetting.IsAuthenticated)
                //{
                //      BMS: Dont' believe this is a problem anymore now that I've enhanced how the network setting form works with a clone of the network connection.
                //    _NetworkSetting.DataMartList = null; // TODO This is where the datamart list is cleared. Should match and update instead of clearing.
                //}

                int totalNetworkSettingCount = Configuration.Instance.NetworkSettingCollection.NetWorkSettings.Count;
                for (int i = 0; i < totalNetworkSettingCount; i++)
                {
                    if (((NetWorkSetting)Configuration.Instance.NetworkSettingCollection.NetWorkSettings[i]).NetworkId == _NetworkSetting.NetworkId)
                    {
                        // BMS: TODO - Is there a possible memory leak with just stepping on the old network setting?
                        Configuration.Instance.NetworkSettingCollection.NetWorkSettings[i] = _NetworkSetting;
                        break;
                    }
                }
            }

            SaveFields( _NetworkSetting );
            CredentialManager.SaveCredential( _NetworkSetting.CredentialKey, _NetworkSetting.Username, _NetworkSetting.Password );
            Configuration.SaveNetworkSettings();
            return true;
        }

        private void SaveFields( NetWorkSetting ns )
        {
            ns.NetworkName = txtNetworkname.Text;
            ns.IsAutoLogin = chkAutoLogin.Checked;
            ns.HubWebServiceUrl = txtServiceUrl.Text;
            ns.Username = txtUsername.Text;
            ns.EncryptedPassword = txtPassword.Text;
            ns.RefreshRate = Convert.ToInt32(numBoxRefreshRate.Value);
            ns.Host = tbHost.Text;
            int port;
            if (!int.TryParse(tbPort.Text, out port))
                port = 0;
            ns.Port = port;
            ns.WcfReceiveTimeout = tbReceiveTimeout.Text;
            var selectedCert = cbCertificates.SelectedItem as Cert;
            ns.X509CertThumbprint = selectedCert == null ? null : selectedCert.Thumbprint;
        }
                               
        private void PopulateDataMarts(ref NetWorkSetting ns)
        {
            try
            {           
                if (!DnsServiceManager.LogIn(ns))
                {
                    return;
                }

                // Find matching existing datamarts and update their info. Append new datamarts and remove deleted ones.
                var newDMs = ns.GetDataMartsByUser().EmptyIfNull();
                if ( (dgvDataMarts.DataSource as IEnumerable<DataMartDescription>).NullOrEmpty() )
                    dgvDataMarts.DataSource = newDMs;
                else
                {
                    (from oldDataMarts in dgvDataMarts.DataSource as IEnumerable<DataMartDescription>
                     join newDataMarts in newDMs on oldDataMarts.DataMartId equals newDataMarts.DataMartId
                     select new { oldDM = oldDataMarts, newDM = newDataMarts }).ForEach(d =>
                     {
                         // Append new datamarts.
                         // For each existing datamart, find matching existing model and update their info. 
                         d.oldDM.DataMartName = d.newDM.DataMartName;
                         d.oldDM.OrganizationId = d.newDM.OrganizationId;
                         d.oldDM.OrganizationName = d.newDM.OrganizationName;

                         if (d.oldDM.ModelList != null && d.newDM.ModelList != null)
                             (from oldModel in d.oldDM.ModelList.Cast<ModelDescription>()
                              join newModel in d.newDM.ModelList.Cast<ModelDescription>() on oldModel.ModelId equals newModel.ModelId
                              select new { oldModel = oldModel, newModel = newModel }).ForEach(m =>
                              {
                                  m.oldModel.ModelName = m.newModel.ModelName;
                                  m.oldModel.ModelDisplayName = m.newModel.ModelDisplayName;                                  
                              });
                     });

                    var ds = new List<DataMartDescription>();
                    ds.AddRange(dgvDataMarts.DataSource as IEnumerable<DataMartDescription>);

                    ds.AddRange(from newDataMart in newDMs
                                where !ds.Any(es => (es.DataMartId == newDataMart.DataMartId))
                                select newDataMart);

                    ds.RemoveAll(r => !newDMs.Any(es => es.DataMartId == r.DataMartId));

                    dgvDataMarts.DataSource = ds;
                }

                dgvDataMarts.Refresh();
                if (ns.DataMartList != null && ns.DataMartList.Count > 0)
                {
                    Configuration.Instance.LoadModels(ns);

                    btnEdit.Enabled = btnRefreshDMlist.Enabled = true;
                    btnApply.Enabled = btnOk.Enabled = true;
                    dgvDataMarts.Enabled = true;
                }
                else
                {

                    dgvDataMarts.Enabled = false;
                    btnEdit.Enabled = false;
                    MessageBox.Show("No network DataMart(s) found", Application.ProductName);                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsDataSourceSetForDataMarts()
        {
            bool _IsDatasourceAvailable = true; ;
            try
            {
                if (_NetworkSetting != null && _NetworkSetting.DataMartList != null && _NetworkSetting.DataMartList.Count > 0)
                {
                    foreach (DataMartDescription dd in _NetworkSetting.DataMartList)
                    {
                        if (null == dd || (null != dd && string.IsNullOrEmpty(dd.DataSourceName)))
                        {
                            _IsDatasourceAvailable = false;
                            break;
                        }
                    }
                }
                else
                {
                    _IsDatasourceAvailable = false;
                }

                return _IsDatasourceAvailable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
                
        private void ShowDataMartDetail()
        {
            try
            {
                if (dgvDataMarts == null || dgvDataMarts.SelectedRows.Count == 0)
                {
                    MessageBox.Show("You don't have a DataMart to edit settings", Application.ProductName);
                    return;
                }

                DataMartSettingsForm f = new DataMartSettingsForm();
                f.NetworkSetting = NetworkSettingObject;
                f.DataMartDescription = dgvDataMarts.SelectedRows[0].DataBoundItem as DataMartDescription;

                 if (_HasNetworkCredentialsChanged)
                {
                    if (!DoesDataMartExists(f.DataMartDescription))
                        return;
                }

                f.ShowDialog();
                DialogResult dr = f.DialogResult;
                if (dr == DialogResult.OK )
                {
                    btnApply.Enabled = btnOk.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool DoesDataMartExists(DataMartDescription dmd)
        {
            if (dgvDataMarts.SelectedRows[0].DataBoundItem as DataMartDescription == null)
                return false;

            var dr = (from DataGridViewRow d in dgvDataMarts.Rows
                      where (d.DataBoundItem as DataMartDescription).DataMartId == dmd.DataMartId
                      select d).FirstOrDefault();

            if (dr == null)
            {
                MessageBox.Show("Selected DataMart no longer exists in the new Network Settings.", "Network Settings Changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            dr.Selected = true;
            return true;
        }

        private void NetworkLogin()
        {
            try
            {
                if (_NetworkSetting != null && _NetworkSetting.IsAuthenticated)
                {
                    //_NetworkSetting.LogOut();
                    DnsServiceManager.LogOut(_NetworkSetting);
                    //txtServiceUrl.ReadOnly = txtUsername.ReadOnly = txtPassword.ReadOnly = false;
                    //btnLogOut.Visible = false;
                    //btnTestConnection.Visible = true;
                    btnRefreshDMlist.Enabled = false;

                    //RefreshNetworlListDelegate();
                }
            }
            catch (Exception ex)
            {
                //if (_NetworkSetting != null && !_NetworkSetting.IsAuthenticated)
                //    txtServiceUrl.ReadOnly = txtUsername.ReadOnly = txtPassword.ReadOnly = true;

                throw ex;
            }
        }
        /// <summary>
        /// BMS: Really?  This is way too complicated.  Just turn on apply/ok buttons on a change and check to see if there was a credential change.
        /// </summary>
        private void SetOKApplyButtonStatus()
        {
            try
            {
                int port;
                if (!int.TryParse(tbPort.Text, out port))
                    port = 0;
                if (!_isNewNetworksetting && null != _NetworkSetting)
                {
                    if (txtPassword.Text != _NetworkSetting.DecryptedPassword ||
                        txtServiceUrl.Text != _NetworkSetting.HubWebServiceUrl ||
                        txtUsername.Text != _NetworkSetting.Username ||
                        port != _NetworkSetting.Port ||
                        tbHost.Text != _NetworkSetting.Host)
                    {
                        _HasNetworkCredentialsChanged = true;
                        btnOk.Enabled = btnApply.Enabled = true;
                    }
                    else
                    {
                        _HasNetworkCredentialsChanged = false;

                        if (txtNetworkname.Text != _NetworkSetting.NetworkName ||
                            chkAutoLogin.Checked != _NetworkSetting.IsAutoLogin ||
                            numBoxRefreshRate.Value != _NetworkSetting.RefreshRate ||
                            port != _NetworkSetting.Port ||
                            tbHost.Text != _NetworkSetting.Host)
                            btnOk.Enabled = btnApply.Enabled = true;
                        else
                            btnOk.Enabled = btnApply.Enabled = false;
                    }
                }
                else
                {
                    _HasNetworkCredentialsChanged = true;
                    btnOk.Enabled = btnApply.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsValidNetworkSetting()
        {
            bool isValid = true;

            try
            {
                if (string.IsNullOrEmpty(txtNetworkname.Text))
                {
                    MessageBox.Show("Please enter a Network name");
                    isValid = false;
                }

                if (isValid && string.IsNullOrEmpty(txtServiceUrl.Text))
                {
                    MessageBox.Show("Please enter a Service Url");
                    isValid = false;
                }

                if (isValid && string.IsNullOrEmpty(txtUsername.Text))
                {
                    MessageBox.Show("Please enter a Username");
                    isValid = false;
                }

                if (isValid && string.IsNullOrEmpty(txtPassword.Text))
                {
                    MessageBox.Show("Please enter a password");
                    isValid = false;
                }

                if(isValid && numBoxRefreshRate.Value <= 0)
                {
                    MessageBox.Show("Please enter a valid Refresh Rate");
                    isValid = false;
                }

                return isValid;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void txtServiceUrl_TextChanged( object sender, EventArgs e )
        {
            cbCertificates.Enabled = IsHttpsUrl();
        }

        private bool IsHttpsUrl()
        {
            return txtServiceUrl.Text.Trim().StartsWith( "https://", StringComparison.InvariantCultureIgnoreCase );
        }

        #endregion

        class Cert
        {
            public string Name { get; set; }
            public string Thumbprint { get; set; }

            public static readonly Cert None = new Cert { Name = "<None>", Thumbprint = null };
        }

        private IList<Cert> LoadCerts()
        {
            var store = new X509Store( StoreName.My, StoreLocation.CurrentUser );
            try
            {
                store.Open( OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly | OpenFlags.MaxAllowed );
                return store.Certificates.Cast<X509Certificate2>().Select( c => new Cert
                {
                    Name = c.FriendlyName.NullOrSpace() ? c.SubjectName.Name : c.FriendlyName,
                    Thumbprint = c.Thumbprint
                } )
                .StartWith( Cert.None )
                .ToList();
            }
            catch
            {
                return new[] { Cert.None };
            }
            finally
            {
                store.Close();
            }
        }

        private void txtServiceUrl_Leave(object sender, EventArgs e)
        {
            if (!IsHttpsUrl() && Properties.Settings.Default.ForceHTTPS)
            {
                txtServiceUrl.Text.Trim().StartsWith("http://", StringComparison.InvariantCultureIgnoreCase);
                txtServiceUrl.Text = txtServiceUrl.Text.Trim().Replace("http://", "https://");
                cbCertificates.Enabled = true;
            }
        }

        private void NetworkSettingForm_Closing(object sender, FormClosingEventArgs e)
        {
            chkAutoLogin.CheckedChanged -= new EventHandler(OnNetworkSettingChanged);
            txtNetworkname.TextChanged -= new EventHandler(OnNetworkSettingChanged);
            txtServiceUrl.TextChanged -= new EventHandler(OnNetworkSettingChanged);
            txtUsername.TextChanged -= new EventHandler(OnNetworkSettingChanged);
            txtPassword.TextChanged -= new EventHandler(OnNetworkSettingChanged);
            tbHost.TextChanged -= new EventHandler(OnNetworkSettingChanged);
            tbPort.TextChanged -= new EventHandler(OnNetworkSettingChanged);
            tbReceiveTimeout.TextChanged -= new EventHandler(OnNetworkSettingChanged);
            numBoxRefreshRate.ValueChanged -= new EventHandler(OnNetworkSettingChanged);
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
