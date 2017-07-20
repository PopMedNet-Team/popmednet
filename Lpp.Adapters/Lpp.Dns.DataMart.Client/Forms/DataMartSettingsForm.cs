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

        private DataMartDescription _DataMartDescription = null;
        ToolTip toolTip = new ToolTip();

        #endregion

        #region Public Properties

        public DataMartDescription DataMartDescription
        {
            get { return _DataMartDescription; }
            set { _DataMartDescription = value; }
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
                _DataMartDescription.AllowUnattendedOperation = chkAllowUnattendedOperation.Checked;
                _DataMartDescription.ProcessQueriesAndUploadAutomatically = rbModeProcess.Checked;
                _DataMartDescription.NotifyOfNewQueries = rbModeNotify.Checked;
                _DataMartDescription.ProcessQueriesAndNotUpload = rbSemiAutomatic.Checked;
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
                chkAllowUnattendedOperation.Checked = _DataMartDescription.AllowUnattendedOperation;
                rbModeProcess.Checked = _DataMartDescription.ProcessQueriesAndUploadAutomatically;
                rbModeNotify.Checked = _DataMartDescription.NotifyOfNewQueries;
                rbSemiAutomatic.Checked = _DataMartDescription.ProcessQueriesAndNotUpload;
                update_processing_mode_controls();

                Guid dataMartId = DataMartDescription.DataMartId;
                dgvModels.DataSource = DnsServiceManager.GetModels(dataMartId, NetworkSetting).Where(m => m.ModelProcessorId != Guid.Empty).ToArray();
                btnEdit.Enabled = dgvModels.DataSource != null && (dgvModels.DataSource as HubModel[]).Length > 0;
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
    }
}