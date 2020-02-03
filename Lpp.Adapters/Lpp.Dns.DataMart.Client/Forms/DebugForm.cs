using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Lpp.Dns.DataMart.Client.Forms
{
    public partial class DebugForm : Form
    {
        public DebugForm()
        {
            InitializeComponent();
        }

        public string LogFilePath
        {
            get { return tbLogFilePath.Text;  }
            set { tbLogFilePath.Text = value; }
        }

        public string LogLevel
        {
            get { return cboLogLevel.Text; }
            set { cboLogLevel.Text = value; }
        }

        public string DataMartClientId
        {
            get { return tbDataMartClientId.Text; }
            set { tbDataMartClientId.Text = value; }
        }
        private string GetFolderDialog()
        {
            string FolderName = string.Empty;
            FolderBrowserDialog FolderDialog = new FolderBrowserDialog();
            FolderDialog.SelectedPath = LogFilePath;
            FolderDialog.Description = "Log file path";
            FolderDialog.ShowNewFolderButton = true;
            FolderDialog.ShowDialog();
            FolderName = FolderDialog.SelectedPath;
            return FolderName;
        }

        private void btnLogFilePath_Click(object sender, EventArgs e)
        {
            tbLogFilePath.Text = GetFolderDialog();
            LogFilePath = tbLogFilePath.Text;
        }

    }
}
