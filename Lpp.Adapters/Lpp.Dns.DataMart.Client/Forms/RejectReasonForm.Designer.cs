namespace Lpp.Dns.DataMart.Client
{
    partial class RejectReasonForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RejectReasonForm));
            this.label1 = new System.Windows.Forms.Label();
            this.txtRejectReason = new System.Windows.Forms.TextBox();
            this.btnReject = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtLowCellCountIndicator = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(588, 155);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // txtRejectReason
            // 
            this.txtRejectReason.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRejectReason.Location = new System.Drawing.Point(12, 71);
            this.txtRejectReason.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtRejectReason.Multiline = true;
            this.txtRejectReason.Name = "txtRejectReason";
            this.txtRejectReason.Size = new System.Drawing.Size(583, 185);
            this.txtRejectReason.TabIndex = 1;
            // 
            // btnReject
            // 
            this.btnReject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReject.Location = new System.Drawing.Point(404, 262);
            this.btnReject.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(95, 46);
            this.btnReject.TabIndex = 2;
            this.btnReject.Text = "Reject";
            this.btnReject.UseVisualStyleBackColor = true;
            this.btnReject.Click += new System.EventHandler(this.btnReject_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(504, 262);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 46);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtLowCellCountIndicator
            // 
            this.txtLowCellCountIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLowCellCountIndicator.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLowCellCountIndicator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLowCellCountIndicator.ForeColor = System.Drawing.Color.Red;
            this.txtLowCellCountIndicator.Location = new System.Drawing.Point(12, 270);
            this.txtLowCellCountIndicator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtLowCellCountIndicator.Multiline = true;
            this.txtLowCellCountIndicator.Name = "txtLowCellCountIndicator";
            this.txtLowCellCountIndicator.ReadOnly = true;
            this.txtLowCellCountIndicator.Size = new System.Drawing.Size(387, 27);
            this.txtLowCellCountIndicator.TabIndex = 4;
            this.txtLowCellCountIndicator.TabStop = false;
            this.txtLowCellCountIndicator.Text = "Low cell counts have been zeroed in these results.";
            this.txtLowCellCountIndicator.Visible = false;
            // 
            // RejectReasonForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(608, 310);
            this.ControlBox = false;
            this.Controls.Add(this.txtLowCellCountIndicator);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnReject);
            this.Controls.Add(this.txtRejectReason);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RejectReasonForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DataMart Client - Reject Reason";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRejectReason;
        private System.Windows.Forms.Button btnReject;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtLowCellCountIndicator;
    }
}