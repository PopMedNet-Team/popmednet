namespace Lpp.Dns.DataMart.Client
{
    partial class DataMartSettingsForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataMartSettingsForm));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblNote = new System.Windows.Forms.Label();
            this.rbSemiAutomatic = new System.Windows.Forms.RadioButton();
            this.rbModeProcess = new System.Windows.Forms.RadioButton();
            this.rbModeNotify = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.chkAllowUnattendedOperation = new System.Windows.Forms.CheckBox();
            this.ofdialog = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.gbModels = new System.Windows.Forms.GroupBox();
            this.dgvModels = new System.Windows.Forms.DataGridView();
            this.btnEdit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnClearCache = new System.Windows.Forms.Button();
            this.chkExplicitCacheRemoval = new System.Windows.Forms.CheckBox();
            this.chkEncryptCacheItems = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nmCacheRetentionPeriod = new System.Windows.Forms.NumericUpDown();
            this.chkEnableCache = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.gbModels.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvModels)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmCacheRetentionPeriod)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(548, 537);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(65, 40);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "OK";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(619, 537);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 40);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.lblNote);
            this.groupBox2.Controls.Add(this.rbSemiAutomatic);
            this.groupBox2.Controls.Add(this.rbModeProcess);
            this.groupBox2.Controls.Add(this.rbModeNotify);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.chkAllowUnattendedOperation);
            this.groupBox2.Location = new System.Drawing.Point(10, 185);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(674, 182);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Unattended Operation";
            // 
            // lblNote
            // 
            this.lblNote.Location = new System.Drawing.Point(26, 147);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(309, 31);
            this.lblNote.TabIndex = 9;
            this.lblNote.Text = "Note:This option uploads results by automatically setting low cell counts to zero" +
    ".";
            // 
            // rbSemiAutomatic
            // 
            this.rbSemiAutomatic.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rbSemiAutomatic.AutoSize = true;
            this.rbSemiAutomatic.Location = new System.Drawing.Point(9, 95);
            this.rbSemiAutomatic.Name = "rbSemiAutomatic";
            this.rbSemiAutomatic.Size = new System.Drawing.Size(306, 17);
            this.rbSemiAutomatic.TabIndex = 8;
            this.rbSemiAutomatic.TabStop = true;
            this.rbSemiAutomatic.Text = "Process new queries automatically but do not upload results";
            this.rbSemiAutomatic.UseVisualStyleBackColor = true;
            // 
            // rbModeProcess
            // 
            this.rbModeProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rbModeProcess.AutoSize = true;
            this.rbModeProcess.Location = new System.Drawing.Point(9, 123);
            this.rbModeProcess.Margin = new System.Windows.Forms.Padding(2);
            this.rbModeProcess.Name = "rbModeProcess";
            this.rbModeProcess.Size = new System.Drawing.Size(276, 17);
            this.rbModeProcess.TabIndex = 5;
            this.rbModeProcess.TabStop = true;
            this.rbModeProcess.Text = "Process new queries automatically and upload results";
            this.rbModeProcess.UseVisualStyleBackColor = true;
            // 
            // rbModeNotify
            // 
            this.rbModeNotify.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rbModeNotify.AutoSize = true;
            this.rbModeNotify.Location = new System.Drawing.Point(9, 67);
            this.rbModeNotify.Margin = new System.Windows.Forms.Padding(2);
            this.rbModeNotify.Name = "rbModeNotify";
            this.rbModeNotify.Size = new System.Drawing.Size(325, 17);
            this.rbModeNotify.TabIndex = 4;
            this.rbModeNotify.TabStop = true;
            this.rbModeNotify.Text = "Notify me of new queries, but do not process them automatically";
            this.rbModeNotify.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 48);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 1;
            // 
            // chkAllowUnattendedOperation
            // 
            this.chkAllowUnattendedOperation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAllowUnattendedOperation.AutoSize = true;
            this.chkAllowUnattendedOperation.Location = new System.Drawing.Point(7, 25);
            this.chkAllowUnattendedOperation.Margin = new System.Windows.Forms.Padding(2);
            this.chkAllowUnattendedOperation.Name = "chkAllowUnattendedOperation";
            this.chkAllowUnattendedOperation.Size = new System.Drawing.Size(155, 17);
            this.chkAllowUnattendedOperation.TabIndex = 0;
            this.chkAllowUnattendedOperation.Text = "Allow unattended operation";
            this.chkAllowUnattendedOperation.UseVisualStyleBackColor = true;
            this.chkAllowUnattendedOperation.CheckedChanged += new System.EventHandler(this.chkAllowUnattendedOperation_CheckedChanged);
            // 
            // ofdialog
            // 
            this.ofdialog.FileName = "openFileDialog1";
            // 
            // gbModels
            // 
            this.gbModels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbModels.Controls.Add(this.dgvModels);
            this.gbModels.Location = new System.Drawing.Point(10, 12);
            this.gbModels.Name = "gbModels";
            this.gbModels.Size = new System.Drawing.Size(673, 142);
            this.gbModels.TabIndex = 9;
            this.gbModels.TabStop = false;
            this.gbModels.Text = "Models";
            // 
            // dgvModels
            // 
            this.dgvModels.AllowUserToAddRows = false;
            this.dgvModels.AllowUserToDeleteRows = false;
            this.dgvModels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvModels.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvModels.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvModels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvModels.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvModels.Location = new System.Drawing.Point(7, 20);
            this.dgvModels.MultiSelect = false;
            this.dgvModels.Name = "dgvModels";
            this.dgvModels.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvModels.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvModels.RowHeadersVisible = false;
            this.dgvModels.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvModels.Size = new System.Drawing.Size(660, 116);
            this.dgvModels.TabIndex = 0;
            this.dgvModels.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvModels_CellDoubleClick);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Location = new System.Drawing.Point(616, 160);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(61, 25);
            this.btnEdit.TabIndex = 10;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnClearCache);
            this.groupBox1.Controls.Add(this.chkExplicitCacheRemoval);
            this.groupBox1.Controls.Add(this.chkEncryptCacheItems);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.nmCacheRetentionPeriod);
            this.groupBox1.Controls.Add(this.chkEnableCache);
            this.groupBox1.Location = new System.Drawing.Point(10, 372);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(674, 159);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cache Settings";
            // 
            // btnClearCache
            // 
            this.btnClearCache.Location = new System.Drawing.Point(10, 127);
            this.btnClearCache.Name = "btnClearCache";
            this.btnClearCache.Size = new System.Drawing.Size(75, 23);
            this.btnClearCache.TabIndex = 5;
            this.btnClearCache.Text = "Clear Cache";
            this.btnClearCache.UseVisualStyleBackColor = true;
            this.btnClearCache.Click += new System.EventHandler(this.OnClearCache_Clicked);
            // 
            // chkExplicitCacheRemoval
            // 
            this.chkExplicitCacheRemoval.AutoSize = true;
            this.chkExplicitCacheRemoval.Location = new System.Drawing.Point(10, 95);
            this.chkExplicitCacheRemoval.Name = "chkExplicitCacheRemoval";
            this.chkExplicitCacheRemoval.Size = new System.Drawing.Size(167, 17);
            this.chkExplicitCacheRemoval.TabIndex = 4;
            this.chkExplicitCacheRemoval.Text = "Enable explicit cache removal";
            this.chkExplicitCacheRemoval.UseVisualStyleBackColor = true;
            // 
            // chkEncryptCacheItems
            // 
            this.chkEncryptCacheItems.AutoSize = true;
            this.chkEncryptCacheItems.Location = new System.Drawing.Point(10, 71);
            this.chkEncryptCacheItems.Name = "chkEncryptCacheItems";
            this.chkEncryptCacheItems.Size = new System.Drawing.Size(128, 17);
            this.chkEncryptCacheItems.TabIndex = 3;
            this.chkEncryptCacheItems.Text = "Encrypt cached items";
            this.chkEncryptCacheItems.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(74, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(211, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Days to retain responses (-1 for no removal)";
            // 
            // nmCacheRetentionPeriod
            // 
            this.nmCacheRetentionPeriod.DecimalPlaces = 1;
            this.nmCacheRetentionPeriod.Location = new System.Drawing.Point(9, 44);
            this.nmCacheRetentionPeriod.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nmCacheRetentionPeriod.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nmCacheRetentionPeriod.Name = "nmCacheRetentionPeriod";
            this.nmCacheRetentionPeriod.Size = new System.Drawing.Size(61, 20);
            this.nmCacheRetentionPeriod.TabIndex = 1;
            this.nmCacheRetentionPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nmCacheRetentionPeriod.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // chkEnableCache
            // 
            this.chkEnableCache.AutoSize = true;
            this.chkEnableCache.Location = new System.Drawing.Point(10, 20);
            this.chkEnableCache.Name = "chkEnableCache";
            this.chkEnableCache.Size = new System.Drawing.Size(152, 17);
            this.chkEnableCache.TabIndex = 0;
            this.chkEnableCache.Text = "Enable Response Caching";
            this.chkEnableCache.UseVisualStyleBackColor = true;
            // 
            // DataMartSettingsForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(694, 580);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.gbModels);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataMartSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DataMart Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.gbModels.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvModels)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmCacheRetentionPeriod)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbModeProcess;
        private System.Windows.Forms.RadioButton rbModeNotify;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkAllowUnattendedOperation;
        private System.Windows.Forms.RadioButton rbSemiAutomatic;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.OpenFileDialog ofdialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.GroupBox gbModels;
        private System.Windows.Forms.DataGridView dgvModels;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClearCache;
        private System.Windows.Forms.CheckBox chkExplicitCacheRemoval;
        private System.Windows.Forms.CheckBox chkEncryptCacheItems;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nmCacheRetentionPeriod;
        private System.Windows.Forms.CheckBox chkEnableCache;
    }
}