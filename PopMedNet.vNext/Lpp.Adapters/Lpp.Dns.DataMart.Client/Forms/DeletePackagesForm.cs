using Lpp.Dns.DataMart.Client.Utils;
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
    public partial class DeletePackagesForm : Form
    {
        public DeletePackagesForm()
        {
            InitializeComponent();
        }

        private void DeletePackagesForm_Load(object sender, EventArgs e)
        {
            var packages = System.IO.Directory.GetFiles(Configuration.PackagesFolderPath, "*.zip", System.IO.SearchOption.AllDirectories).Select(s => System.IO.Path.GetFileName(s)).ToArray();
            checkedListBox1.SuspendLayout();
            checkedListBox1.Items.AddRange(packages);
            checkedListBox1.ResumeLayout();
        }

        int checkedItems = 0;

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.CurrentValue != e.NewValue)
            {
                if (e.NewValue == CheckState.Checked)
                {
                    checkedItems++;
                }
                else if(checkedItems > 0)
                {
                    checkedItems--;
                }
            }

            btnDelete.Enabled = checkedItems > 0;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (var item in checkedListBox1.CheckedItems)
            {
                if (checkedListBox1.GetItemCheckState(checkedListBox1.Items.IndexOf(item)) != CheckState.Checked)
                    continue;

                try
                {
                    System.IO.File.Delete(System.IO.Path.Combine(Configuration.PackagesFolderPath, item.ToString()));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.ToString(), "Error Deleting Packages", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        
    }
}
