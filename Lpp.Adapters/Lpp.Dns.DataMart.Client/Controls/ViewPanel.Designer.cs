namespace Lpp.Dns.DataMart.Client.Controls
{
    partial class ViewPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(_datasourceStream != null)
                {
                    _datasourceStream.Dispose();
                    _datasourceStream = null;
                }

                if(components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

       

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.PLAIN = new System.Windows.Forms.RichTextBox();
            this.DATASET = new System.Windows.Forms.DataGridView();
            this.HTML = new System.Windows.Forms.WebBrowser();
            this.URL = new System.Windows.Forms.WebBrowser();
            this.JSON = new Lpp.Dns.DataMart.Client.Controls.JSON();
            this.JSON_OLD = new System.Windows.Forms.DataGridView();
            this.XSLXML = new System.Windows.Forms.WebBrowser();
            this.FILELIST = new System.Windows.Forms.DataGridView();
            this.colDocumentSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colFilename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDocumentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMimeType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsDocumentList = new System.Windows.Forms.BindingSource(this.components);
            this.XML = new System.Windows.Forms.TreeView();
            this.lblNoResults = new Lpp.Dns.DataMart.Client.Controls.TransparentLabel();
            ((System.ComponentModel.ISupportInitialize)(this.DATASET)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JSON_OLD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FILELIST)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDocumentList)).BeginInit();
            this.SuspendLayout();
            // 
            // PLAIN
            // 
            this.PLAIN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PLAIN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PLAIN.Location = new System.Drawing.Point(0, 0);
            this.PLAIN.Name = "PLAIN";
            this.PLAIN.ReadOnly = true;
            this.PLAIN.Size = new System.Drawing.Size(782, 333);
            this.PLAIN.TabIndex = 0;
            this.PLAIN.Text = "";
            // 
            // DATASET
            // 
            this.DATASET.AllowUserToAddRows = false;
            this.DATASET.AllowUserToDeleteRows = false;
            this.DATASET.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DATASET.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DATASET.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DATASET.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DATASET.Location = new System.Drawing.Point(0, 0);
            this.DATASET.Margin = new System.Windows.Forms.Padding(2);
            this.DATASET.Name = "DATASET";
            this.DATASET.ReadOnly = true;
            this.DATASET.RowHeadersVisible = false;
            this.DATASET.RowTemplate.Height = 24;
            this.DATASET.Size = new System.Drawing.Size(782, 333);
            this.DATASET.TabIndex = 10;
            this.DATASET.Visible = false;
            // 
            // HTML
            // 
            this.HTML.AllowWebBrowserDrop = false;
            this.HTML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HTML.Location = new System.Drawing.Point(0, 0);
            this.HTML.Name = "HTML";
            this.HTML.Size = new System.Drawing.Size(782, 333);
            this.HTML.TabIndex = 11;
            this.HTML.Visible = false;
            // 
            // URL
            // 
            this.URL.AllowWebBrowserDrop = false;
            this.URL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.URL.Location = new System.Drawing.Point(0, 0);
            this.URL.Name = "URL";
            this.URL.Size = new System.Drawing.Size(782, 333);
            this.URL.TabIndex = 11;
            this.URL.Visible = false;
            // 
            // JSON
            // 
            this.JSON.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AutoScroll = true;
            this.JSON.Location = new System.Drawing.Point(0, 0);
            this.JSON.Name = "JSON";
            this.JSON.Size = new System.Drawing.Size(782, 333);
            this.JSON.TabIndex = 10;
            this.JSON.Visible = false;
            // 
            // JSON_OLD
            // 
            this.JSON_OLD.AllowUserToAddRows = false;
            this.JSON_OLD.AllowUserToDeleteRows = false;
            this.JSON_OLD.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.JSON_OLD.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.JSON_OLD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.JSON_OLD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.JSON_OLD.Location = new System.Drawing.Point(0, 0);
            this.JSON_OLD.Margin = new System.Windows.Forms.Padding(2);
            this.JSON_OLD.Name = "JSON_OLD";
            this.JSON_OLD.ReadOnly = true;
            this.JSON_OLD.RowHeadersVisible = false;
            this.JSON_OLD.RowTemplate.Height = 24;
            this.JSON_OLD.Size = new System.Drawing.Size(782, 333);
            this.JSON_OLD.TabIndex = 10;
            this.JSON_OLD.Visible = false;
            this.JSON_OLD.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.JSON_CellFormatting);
            // 
            // XSLXML
            // 
            this.XSLXML.AllowWebBrowserDrop = false;
            this.XSLXML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.XSLXML.Location = new System.Drawing.Point(0, 0);
            this.XSLXML.Name = "XSLXML";
            this.XSLXML.Size = new System.Drawing.Size(782, 333);
            this.XSLXML.TabIndex = 53;
            this.XSLXML.Visible = false;
            // 
            // FILELIST
            // 
            this.FILELIST.AutoGenerateColumns = false;
            this.FILELIST.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.FILELIST.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FILELIST.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FILELIST.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDocumentSelected,
            this.colFilename,
            this.colDocumentID,
            this.colMimeType,
            this.colSize});
            this.FILELIST.DataSource = this.bsDocumentList;
            this.FILELIST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FILELIST.Location = new System.Drawing.Point(0, 0);
            this.FILELIST.Name = "FILELIST";
            this.FILELIST.RowHeadersVisible = false;
            this.FILELIST.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.White;
            this.FILELIST.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.FILELIST.RowTemplate.Height = 24;
            this.FILELIST.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.FILELIST.Size = new System.Drawing.Size(782, 333);
            this.FILELIST.TabIndex = 52;
            this.FILELIST.Visible = false;
            // 
            // colDocumentSelected
            // 
            this.colDocumentSelected.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colDocumentSelected.HeaderText = "Select";
            this.colDocumentSelected.Name = "colDocumentSelected";
            this.colDocumentSelected.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colDocumentSelected.Width = 55;
            // 
            // colFilename
            // 
            this.colFilename.DataPropertyName = "Filename";
            this.colFilename.HeaderText = "File Name";
            this.colFilename.Name = "colFilename";
            this.colFilename.ReadOnly = true;
            // 
            // colDocumentID
            // 
            this.colDocumentID.DataPropertyName = "DocumentID";
            this.colDocumentID.HeaderText = "Document ID";
            this.colDocumentID.Name = "colDocumentID";
            this.colDocumentID.ReadOnly = true;
            // 
            // colMimeType
            // 
            this.colMimeType.DataPropertyName = "MimeType";
            this.colMimeType.HeaderText = "File Type";
            this.colMimeType.Name = "colMimeType";
            this.colMimeType.ReadOnly = true;
            // 
            // colSize
            // 
            this.colSize.DataPropertyName = "Size";
            this.colSize.HeaderText = "Size";
            this.colSize.Name = "colSize";
            this.colSize.ReadOnly = true;
            // 
            // bsDocumentList
            // 
            this.bsDocumentList.DataSource = typeof(Lpp.Dns.DataMart.Model.Document);
            // 
            // XML
            // 
            this.XML.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.XML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.XML.Location = new System.Drawing.Point(0, 0);
            this.XML.Name = "XML";
            this.XML.Size = new System.Drawing.Size(782, 333);
            this.XML.TabIndex = 55;
            this.XML.Visible = false;
            // 
            // lblNoResults
            // 
            this.lblNoResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNoResults.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.lblNoResults.CausesValidation = false;
            this.lblNoResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoResults.Location = new System.Drawing.Point(237, 72);
            this.lblNoResults.Name = "lblNoResults";
            this.lblNoResults.Size = new System.Drawing.Size(323, 20);
            this.lblNoResults.TabIndex = 54;
            this.lblNoResults.TabStop = false;
            this.lblNoResults.Text = "No Results";
            this.lblNoResults.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblNoResults.Visible = false;
            // 
            // ViewPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.lblNoResults);
            this.Controls.Add(this.JSON);
            this.Controls.Add(this.JSON_OLD);
            this.Controls.Add(this.XSLXML);
            this.Controls.Add(this.DATASET);
            this.Controls.Add(this.PLAIN);
            this.Controls.Add(this.XML);
            this.Controls.Add(this.FILELIST);
            this.Controls.Add(this.HTML);
            this.Controls.Add(this.URL);
            this.Name = "ViewPanel";
            this.Size = new System.Drawing.Size(782, 333);
            ((System.ComponentModel.ISupportInitialize)(this.DATASET)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JSON_OLD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FILELIST)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDocumentList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.RichTextBox PLAIN;
        public System.Windows.Forms.DataGridView DATASET;
        private System.Windows.Forms.WebBrowser HTML;
        private System.Windows.Forms.WebBrowser URL;
        public System.Windows.Forms.DataGridView JSON_OLD;
        public JSON JSON;
        private System.Windows.Forms.TreeView XML;
        private System.Windows.Forms.WebBrowser XSLXML;
        public System.Windows.Forms.DataGridView FILELIST;
        private System.Windows.Forms.BindingSource bsDocumentList;
        protected TransparentLabel lblNoResults;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colDocumentSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFilename;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDocumentID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMimeType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSize;
    }
}
