using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Lpp.Dns.DataMart.Client.Properties;
using System.Text.RegularExpressions;

using Lpp.Dns.DataMart.Model;
using Lpp.Dns.DataMart.Lib.Classes;
using Lpp.Dns.DataMart.Lib;
using log4net;

namespace Lpp.Dns.DataMart.Client
{
    public partial class DataMartSettingsForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Private variables

        private DataMartDescription _dataMartDescription = null;
        ToolTip toolTip = new ToolTip();
        Lib.Caching.DataMartCacheManager _cache = null;

        #endregion

        #region Public Properties

        public DataMartDescription DataMartDescription
        {
            get { return _dataMartDescription; }
            set { _dataMartDescription = value; }
        }

        public NetWorkSetting NetworkSetting
        {
            get;
            set;
        }

        #endregion

        #region Constructors
        public DataMartSettingsForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult = DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkAllowUnattendedOperation.Checked)
                {
                    if (!rbModeProcess.Checked && !rbModeNotify.Checked && !rbSemiAutomatic.Checked)
                    {
                        MessageBox.Show("Please select one of the modes in which you wish to process queries from this DataMart", Application.ProductName);
                        return;
                    }
                }
                _dataMartDescription.AllowUnattendedOperation = chkAllowUnattendedOperation.Checked;
                _dataMartDescription.ProcessQueriesAndUploadAutomatically = rbModeProcess.Checked;
                _dataMartDescription.NotifyOfNewQueries = rbModeNotify.Checked;
                _dataMartDescription.ProcessQueriesAndNotUpload = rbSemiAutomatic.Checked;

                if (chkEnableCache.Checked)
                {
                    _dataMartDescription.EnableResponseCaching = true;
                    _dataMartDescription.DaysToRetainCacheItems = nmCacheRetentionPeriod.Value;
                    _dataMartDescription.EncryptCacheItems = chkEncryptCacheItems.Checked;
                    _dataMartDescription.EnableExplictCacheRemoval = chkExplicitCacheRemoval.Checked;
                }
                else
                {
                    _dataMartDescription.EnableResponseCaching = false;
                    _dataMartDescription.DaysToRetainCacheItems = nmCacheRetentionPeriod.Value;
                    _dataMartDescription.EncryptCacheItems = false;
                    _dataMartDescription.EnableExplictCacheRemoval = false;

                    //remove any cache items
                    _cache.ClearCache();
                    log.Info($"Caching disabled, clearing existing cached documents on save for DataMart: { DataMartDescription.DataMartName }, Path: { _cache.BaseCachePath }");
                }


                DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            try
            {
                chkAllowUnattendedOperation.Checked = _dataMartDescription.AllowUnattendedOperation;
                rbModeProcess.Checked = _dataMartDescription.ProcessQueriesAndUploadAutomatically;
                rbModeNotify.Checked = _dataMartDescription.NotifyOfNewQueries;
                rbSemiAutomatic.Checked = _dataMartDescription.ProcessQueriesAndNotUpload;
                update_processing_mode_controls();

                Guid dataMartId = DataMartDescription.DataMartId;

                var models = DnsServiceManager.GetModels(NetworkSetting);
                if (models.ContainsKey(dataMartId))
                {
                    dgvModels.DataSource = (models[dataMartId] ?? Array.Empty<HubModel>()).Where(m => m.ModelProcessorId != Guid.Empty).ToArray();
                }
                else
                {
                    dgvModels.DataSource = Array.Empty<HubModel>();
                }

                btnEdit.Enabled = dgvModels.DataSource != null && (dgvModels.DataSource as HubModel[]).Length > 0;

                _cache = new Lib.Caching.DataMartCacheManager(NetworkSetting.NetworkId, DataMartDescription.DataMartId);

                chkEncryptCacheItems.Checked = _dataMartDescription.EncryptCacheItems;
                chkExplicitCacheRemoval.Checked = _dataMartDescription.EnableExplictCacheRemoval;
                nmCacheRetentionPeriod.Value = _dataMartDescription.DaysToRetainCacheItems;
                chkEnableCache.Checked = _dataMartDescription.EnableResponseCaching;

                chkEnableCache_CheckedChanged(null, EventArgs.Empty);
                chkEnableCache.CheckedChanged += chkEnableCache_CheckedChanged;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        protected void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[0-9]|\b");
                e.Handled = !(regex.IsMatch(e.KeyChar.ToString()));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        private void chkAllowUnattendedOperation_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                update_processing_mode_controls();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        #endregion

        #region Private Methods

        void update_processing_mode_controls()
        {
            try
            {
                //txtPollingInterval.Enabled = chkAllowUnattendedOperation.Checked;
                rbModeProcess.Enabled = chkAllowUnattendedOperation.Checked;
                lblNote.Enabled = chkAllowUnattendedOperation.Checked;
                rbModeNotify.Enabled = chkAllowUnattendedOperation.Checked;
                rbSemiAutomatic.Enabled = chkAllowUnattendedOperation.Checked;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion



        private void dgvModels_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowModelSettingsForm();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            ShowModelSettingsForm();
        }

        private void ShowModelSettingsForm()
        {
            ModelSettingsForm f = new ModelSettingsForm();
            f.DataMartDescription = DataMartDescription;
            f.HubModel = dgvModels.SelectedRows[0].DataBoundItem as HubModel;
            f.NetworkSetting = NetworkSetting;
            f.ShowDialog();
        }

        private void chkEnableCache_CheckedChanged(object sender, EventArgs e)
        {
            //enable settings based on checkstate
            nmCacheRetentionPeriod.Enabled = chkEnableCache.Checked;
            chkEncryptCacheItems.Enabled = chkEnableCache.Checked;
            chkExplicitCacheRemoval.Enabled = chkEnableCache.Checked;
            btnClearCache.Enabled = chkEnableCache.Checked;
        }

        private void OnClearCache_Clicked(object sender, EventArgs e)
        {
            _cache.ClearCache();

            log.Info($"Cache cleared for DataMart: { DataMartDescription.DataMartName }, Path: { _cache.BaseCachePath }");
            MessageBox.Show(this, $"Cache has been successfully cleared for DataMart: { DataMartDescription.DataMartName }.", "Cache Cleared", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}