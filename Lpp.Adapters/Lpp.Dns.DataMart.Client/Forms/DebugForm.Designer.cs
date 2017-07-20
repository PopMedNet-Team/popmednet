namespace Lpp.Dns.DataMart.Client.Forms
{
    partial class DebugForm
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
            this.cboLogLevel = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnLogFilePath = new System.Windows.Forms.Button();
            this.lblautoExec = new System.Windows.Forms.Label();
            this.tbLogFilePath = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbDataMartClientId = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cboLogLevel
            // 
            this.cboLogLevel.FormattingEnabled = true;
            this.cboLogLevel.Items.AddRange(new object[] {
            "Error",
            "Info",
            "Debug"});
            this.cboLogLevel.Location = new System.Drawing.Point(93, 57);
            this.cboLogLevel.Name = "cboLogLevel";
            this.cboLogLevel.Size = new System.Drawing.Size(86, 21);
            this.cboLogLevel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Log Level:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(369, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Please specify the location of the log file and the level of detail that is logge" +
    "d.";
            // 
            // btnLogFilePath
            // 
            this.btnLogFilePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogFilePath.Location = new System.Drawing.Point(344, 31);
            this.btnLogFilePath.Name = "btnLogFilePath";
            this.btnLogFilePath.Size = new System.Drawing.Size(28, 23);
            this.btnLogFilePath.TabIndex = 3;
            this.btnLogFilePath.Text = "...";
            this.btnLogFilePath.UseVisualStyleBackColor = true;
            this.btnLogFilePath.Click += new System.EventHandler(this.btnLogFilePath_Click);
            // 
            // lblautoExec
            // 
            this.lblautoExec.AutoSize = true;
            this.lblautoExec.Location = new System.Drawing.Point(15, 36);
            this.lblautoExec.Name = "lblautoExec";
            this.lblautoExec.Size = new System.Drawing.Size(72, 13);
            this.lblautoExec.TabIndex = 4;
            this.lblautoExec.Text = "Log File Path:";
            // 
            // tbLogFilePath
            // 
            this.tbLogFilePath.Location = new System.Drawing.Point(93, 31);
            this.tbLogFilePath.Name = "tbLogFilePath";
            this.tbLogFilePath.Size = new System.Drawing.Size(245, 20);
            this.tbLogFilePath.TabIndex = 5;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(297, 119);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(216, 119);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Client Id:";
            // 
            // tbDataMartClientId
            // 
            this.tbDataMartClientId.Enabled = false;
            this.tbDataMartClientId.Location = new System.Drawing.Point(93, 83);
            this.tbDataMartClientId.Name = "tbDataMartClientId";
            this.tbDataMartClientId.Size = new System.Drawing.Size(245, 20);
            this.tbDataMartClientId.TabIndex = 9;
            this.tbDataMartClientId.Text = "xxxxx-xxxxx-xxxxx";
            // 
            // DebugForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 153);
            this.ControlBox = false;
            this.Controls.Add(this.tbDataMartClientId);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tbLogFilePath);
            this.Controls.Add(this.lblautoExec);
            this.Controls.Add(this.btnLogFilePath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboLogLevel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DebugForm";
            this.Text = "Debug";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboLogLevel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnLogFilePath;
        private System.Windows.Forms.Label lblautoExec;
        private System.Windows.Forms.TextBox tbLogFilePath;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDataMartClientId;
    }
}