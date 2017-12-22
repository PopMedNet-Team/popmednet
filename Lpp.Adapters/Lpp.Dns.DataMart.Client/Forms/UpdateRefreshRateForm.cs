using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lpp.Dns.DataMart.Client.Forms
{
    public partial class UpdateRefreshRateForm : Form
    {
        public UpdateRefreshRateForm()
        {
            InitializeComponent();
            numBoxRefreshRate.Value = (Properties.Settings.Default.RefreshRate / 1000);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.RefreshRate = (int)(numBoxRefreshRate.Value * 1000);
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Upgrade();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
