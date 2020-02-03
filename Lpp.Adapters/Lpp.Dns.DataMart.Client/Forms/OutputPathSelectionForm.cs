using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lpp.Dns.DataMart.Client
{
    public partial class OutputPathSelectionForm : Form
    {
        List<OutputPathSelectionItem> _outputPaths = new List<OutputPathSelectionItem>();

        public IDictionary<Guid, string> OutputPaths
        {
            get
            {
                return _outputPaths.ToDictionary(item => item.Identifier, item => item.OutputPath);
            }
        }
        public OutputPathSelectionForm()
        {
            InitializeComponent();
        }

        public OutputPathSelectionForm(IDictionary<Guid, string> queryIdentifiers)
        {
            var qi = queryIdentifiers ?? new Dictionary<Guid, string>();
            foreach(var identifier in qi)
            {
                _outputPaths.Add(new OutputPathSelectionItem { Descriptor = identifier.Value, Identifier = identifier.Key });
            }

            InitializeComponent();
            
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = _outputPaths;
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 2)
            {
                OutputPathSelectionItem item = _outputPaths[e.RowIndex];

                var dialog = new SaveFileDialog() { AddExtension = true, Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*", DefaultExt="csv", RestoreDirectory=true };

                if (!string.IsNullOrEmpty(item.OutputPath))
                {
                    dialog.FileName = System.IO.Path.GetFileName(item.OutputPath);
                    dialog.InitialDirectory = System.IO.Path.GetDirectoryName(item.OutputPath);
                }

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    item.OutputPath = dialog.FileName;
                    dataGridView1.InvalidateRow(e.RowIndex);
                }
            }
        }
    }

    internal class OutputPathSelectionItem
    {
        public Guid Identifier { get; set; }
        public string Descriptor { get; set; }

        public string OutputPath { get; set; }
    }
}
