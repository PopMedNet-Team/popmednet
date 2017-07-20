using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Lpp.Dns.DataMart.Client
{
    public partial class RejectReasonForm : Form
    {
        #region Constructors
        public RejectReasonForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Reason text entered by the user
        /// </summary>
        public string RejectReason
        {
            get {
                string strRejectReason = txtRejectReason.Text;
                if (IsLowCellCountSetToZero)
                    strRejectReason = txtLowCellCountIndicator.Text + ".\r\n" + strRejectReason;
                return strRejectReason; 
            }
            set { txtRejectReason.Text = value; }
        }
        public string FormText
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }
        public string ButtonText
        {
            get { return btnReject.Text; }
            set { btnReject.Text = value; }
        }
        public bool _IsLowCellCountSetToZero = false;
        public bool IsLowCellCountSetToZero
        {
            get { return _IsLowCellCountSetToZero; }
            set { 
                _IsLowCellCountSetToZero = value;
                txtLowCellCountIndicator.Visible = value;
            }
        }

        #endregion

        #region Event Handlers

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try{
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            }
            catch
            {
                // TODO Log
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        #endregion
    }
}