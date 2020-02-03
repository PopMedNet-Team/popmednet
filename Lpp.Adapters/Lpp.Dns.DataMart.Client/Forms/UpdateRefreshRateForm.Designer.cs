namespace Lpp.Dns.DataMart.Client.Forms
{
    partial class UpdateRefreshRateForm
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
            this.lblRefreshRate = new System.Windows.Forms.Label();
            this.numBoxRefreshRate = new System.Windows.Forms.NumericUpDown();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numBoxRefreshRate)).BeginInit();
            this.SuspendLayout();
            // 
            // lblRefreshRate
            // 
            this.lblRefreshRate.AutoSize = true;
            this.lblRefreshRate.Location = new System.Drawing.Point(13, 88);
            this.lblRefreshRate.Name = "lblRefreshRate";
            this.lblRefreshRate.Size = new System.Drawing.Size(73, 13);
            this.lblRefreshRate.TabIndex = 0;
            this.lblRefreshRate.Text = "Refresh Rate:\r\n(In seconds)";
            // 
            // numBoxRefreshRate
            // 
            this.numBoxRefreshRate.Location = new System.Drawing.Point(92, 86);
            this.numBoxRefreshRate.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numBoxRefreshRate.Name = "numBoxRefreshRate";
            this.numBoxRefreshRate.Size = new System.Drawing.Size(162, 20);
            this.numBoxRefreshRate.TabIndex = 1;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(10, 226);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(152, 226);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // UpdateRefreshRateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.numBoxRefreshRate);
            this.Controls.Add(this.lblRefreshRate);
            this.Name = "UpdateRefreshRateForm";
            this.ShowIcon = false;
            this.Text = "Refresh Rate";
            ((System.ComponentModel.ISupportInitialize)(this.numBoxRefreshRate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRefreshRate;
        private System.Windows.Forms.NumericUpDown numBoxRefreshRate;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCancel;
    }
}