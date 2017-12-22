using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using log4net;
using Lpp.Dns.DataMart.Model;
using Lpp.Dns.DataMart.Lib;
using Lpp.Dns.DataMart.Client.Utils;
using Lpp.Dns.DataMart.Lib.RequestQueue;

namespace Lpp.Dns.DataMart.Client
{
    public partial class NetworkListForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public delegate void ConfigurationChangedEventHandler(object sender, NetWorkSetting ns);
        public event ConfigurationChangedEventHandler ConfigurationChanged;

        #region Constructor
        public NetworkListForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers

        private void NetworkList_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DnsServiceManager.TestConnections(Configuration.Instance.NetworkSettingCollection.AsEnumerable());
                LoadPage();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                NetworkSettingForm f = new NetworkSettingForm();
                f.NetworkSettingObject = null;
                f.NetworkSettingChanged += NetworkSettingChanged_EventHandler;
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void NetworkSettingChanged_EventHandler(object sender, NetWorkSetting ns)
        {
            LoadPage();
            ConfigurationChanged(sender, ns);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                NetworkSettingForm f = new NetworkSettingForm();
                if (null != dgvNetworkList.SelectedRows && dgvNetworkList.SelectedRows.Count == 1)
                {
                    f.NetworkSettingObject = dgvNetworkList.SelectedRows[0].DataBoundItem as NetWorkSetting;
                    f.NetworkSettingChanged += NetworkSettingChanged_EventHandler;
                    f.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Please select one Network", Application.ProductName);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult ConfirmDelete = MessageBox.Show("Do you really want to delete the selected Network ?",
                "Please confirm",
                MessageBoxButtons.YesNo,MessageBoxIcon.Question);

                if (ConfirmDelete == DialogResult.No)
                    return;

                this.Cursor = Cursors.WaitCursor;
                DataGridViewSelectedRowCollection d = dgvNetworkList.SelectedRows;
                NetWorkSetting selectedNetworkSetting = d[0].DataBoundItem as NetWorkSetting;

                if (null != d && d.Count > 0)
                {
                    ArrayList UpdatedNetworkSettingCollection = new ArrayList();

                    int i = 1;
                    foreach (NetWorkSetting n in Configuration.Instance.NetworkSettingCollection.NetWorkSettings)
                    {
                        if (null != n && n != selectedNetworkSetting)
                        {
                            n.NetworkId = i;
                            UpdatedNetworkSettingCollection.Add(n);
                            i++;
                        }
                    }
                    Configuration.Instance.NetworkSettingCollection.NetWorkSettings = UpdatedNetworkSettingCollection;

                    Configuration.SaveNetworkSettings();
                    CredentialManager.DeleteCredential(selectedNetworkSetting.CredentialKey);
                    Configuration.DeleteModelPasswords(selectedNetworkSetting);
                    ConfigurationChanged(sender, selectedNetworkSetting);

                    this.Close();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
            
        private void dgvNetworkList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
         {
             try
             {
                 NetworkSettingForm f = new NetworkSettingForm();
                 if (null != dgvNetworkList.SelectedRows && dgvNetworkList.SelectedRows.Count > 0)
                 {
                     f.NetworkSettingObject = dgvNetworkList.SelectedRows[0].DataBoundItem as NetWorkSetting;
                     //f.RefreshNetworlListDelegate += new DMClient.RefreshNetworlListDelegate(RefreshQueryDataMartCallbackFn);
                     f.NetworkSettingChanged += NetworkSettingChanged_EventHandler;
                     f.ShowDialog();
                 }                     
             }
             catch (Exception ex)
             {
                 log.Error(ex);
             }
         }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        } 

        private void btnUpdate_Click(object sender, EventArgs e)
        {
             try
             {
                 if (null != dgvNetworkList.SelectedRows && dgvNetworkList.SelectedRows.Count > 0)
                 {
                     NetWorkSetting ns = dgvNetworkList.SelectedRows[0].DataBoundItem as NetWorkSetting;
                     Dictionary<string, string> dict = new Dictionary<string, string>();
                     dict.Add("serviceUrl", ns.HubWebServiceUrl);
                     dict.Add("networkName", ns.NetworkName);
                     dict.Add("username", ns.Username);
                     NetworkConnectForm f = new NetworkConnectForm(dict);
                     f.NetworkSettingChanged += NetworkSettingChanged_EventHandler;
                     f.ShowDialog();
                 }                     
             }
             catch (Exception ex)
             {
                 log.Error(ex);
             }
        }
     
        #endregion

        #region Private Methods

        private void LoadPage()
         {
             try
             {
                 if (Configuration.Instance.NetworkSettingCollection.NetWorkSettings != null && Configuration.Instance.NetworkSettingCollection.NetWorkSettings.Count > 0)
                 {
                     netWorkSettingCollectionBindingSource.DataSource = Configuration.Instance.NetworkSettingCollection.NetWorkSettings;
                     netWorkSettingCollectionBindingSource.ResetBindings(false);
                     btnEdit.Enabled = btnDelete.Enabled = btnAdd.Enabled = true;
                     dgvNetworkList.Refresh();
                 }
                 else
                 {
                     btnEdit.Enabled = btnDelete.Enabled = false;
                 }
             }
             catch (Exception ex)
             {
                 log.Error(ex);
                 throw ex;
             }
         }

        #endregion

        #region Networklist columns
        private enum NetworkListColumns
             {
                 NetworkId = 0,
                 NetworkName = 1,
                 Status = 2//,
                 //LoginCheckBox = 3,
                 //LogOutCheckBox = 4
             }
             #endregion    

                   
    }
}
