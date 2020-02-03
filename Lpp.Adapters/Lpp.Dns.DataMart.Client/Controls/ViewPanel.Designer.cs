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
            if (disposing && (components != null))
            {
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
            this.PLAIN = new System.Windows.Forms.RichTextBox();
            this.DATASET = new System.Windows.Forms.DataGridView();
            this.HTML = new System.Windows.Forms.WebBrowser();
            this.URL = new System.Windows.Forms.WebBrowser();
            this.JSON = new System.Windows.Forms.DataGridView();
            this.XSLXML = new System.Windows.Forms.WebBrowser();
            this.FILELIST = new System.Windows.Forms.DataGridView();
            this.ReulstFileName = new System.Windows.Forms.DataGridViewLinkColumn();
            this.XML = new System.Windows.Forms.TreeView();
            this.lblNoResults = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DATASET)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JSON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FILELIST)).BeginInit();
            this.SuspendLayout();
            // 
            // PLAIN
            // 
            this.PLAIN.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PLAIN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PLAIN.Location = new System.Drawing.Point(0, 0);
            this.PLAIN.Name = "PLAIN";
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
            this.HTML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HTML.Location = new System.Drawing.Point(0, 0);
            this.HTML.MinimumSize = new System.Drawing.Size(20, 20);
            this.HTML.Name = "HTML";
            this.HTML.Size = new System.Drawing.Size(782, 333);
            this.HTML.TabIndex = 11;
            this.HTML.Visible = false;
            // 
            // URL
            // 
            this.URL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.URL.Location = new System.Drawing.Point(0, 0);
            this.URL.MinimumSize = new System.Drawing.Size(20, 20);
            this.URL.Name = "URL";
            this.URL.Size = new System.Drawing.Size(782, 333);
            this.URL.TabIndex = 11;
            this.URL.Visible = false;
            // 
            // JSON
            // 
            this.JSON.AllowUserToAddRows = false;
            this.JSON.AllowUserToDeleteRows = false;
            this.JSON.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.JSON.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.JSON.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.JSON.Dock = System.Windows.Forms.DockStyle.Fill;
            this.JSON.Location = new System.Drawing.Point(0, 0);
            this.JSON.Margin = new System.Windows.Forms.Padding(2);
            this.JSON.Name = "JSON";
            this.JSON.ReadOnly = true;
            this.JSON.RowHeadersVisible = false;
            this.JSON.RowTemplate.Height = 24;
            this.JSON.Size = new System.Drawing.Size(782, 333);
            this.JSON.TabIndex = 10;
            this.JSON.Visible = false;
            this.JSON.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.JSON_CellFormatting);
            // 
            // XSLXML
            // 
            this.XSLXML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.XSLXML.Location = new System.Drawing.Point(0, 0);
            this.XSLXML.Name = "XSLXML";
            this.XSLXML.Size = new System.Drawing.Size(782, 333);
            this.XSLXML.TabIndex = 53;
            this.XSLXML.Visible = false;
            // 
            // FILELIST
            // 
            this.FILELIST.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.FILELIST.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FILELIST.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FILELIST.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ReulstFileName});
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
            // ReulstFileName
            // 
            this.ReulstFileName.DataPropertyName = "FileName";
            this.ReulstFileName.HeaderText = "File Name";
            this.ReulstFileName.Name = "ReulstFileName";
            // 
            // XML
            // 
            this.XML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.XML.Location = new System.Drawing.Point(0, 0);
            this.XML.MinimumSize = new System.Drawing.Size(20, 20);
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
            this.lblNoResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoResults.Location = new System.Drawing.Point(237, 72);
            this.lblNoResults.Name = "lblNoResults";
            this.lblNoResults.Size = new System.Drawing.Size(323, 20);
            this.lblNoResults.TabIndex = 54;
            this.lblNoResults.Text = "No Results";
            this.lblNoResults.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ViewPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.lblNoResults);
            this.Controls.Add(this.FILELIST);
            this.Controls.Add(this.HTML);
            this.Controls.Add(this.URL);
            this.Controls.Add(this.JSON);
            this.Controls.Add(this.XSLXML);
            this.Controls.Add(this.DATASET);
            this.Controls.Add(this.PLAIN);
            this.Controls.Add(this.XML);
            this.Name = "ViewPanel";
            this.Size = new System.Drawing.Size(782, 333);
            ((System.ComponentModel.ISupportInitialize)(this.DATASET)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JSON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FILELIST)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.RichTextBox PLAIN;
        public System.Windows.Forms.DataGridView DATASET;
        private System.Windows.Forms.WebBrowser HTML;
        private System.Windows.Forms.WebBrowser URL;
        public System.Windows.Forms.DataGridView JSON;
        private System.Windows.Forms.TreeView XML;
        private System.Windows.Forms.WebBrowser XSLXML;
        public System.Windows.Forms.DataGridView FILELIST;
        private System.Windows.Forms.Label lblNoResults;
        private System.Windows.Forms.DataGridViewLinkColumn ReulstFileName;
    }
}
