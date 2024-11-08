namespace Lpp.Dns.DataMart.Client
{
    partial class ProgressForm
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
            System.Windows.Forms.Panel panel1;
            this.text = new System.Windows.Forms.Label();
            this.progress = new System.Windows.Forms.ProgressBar();
            panel1 = new System.Windows.Forms.Panel();
            panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel1.Controls.Add(this.text);
            panel1.Controls.Add(this.progress);
            panel1.Location = new System.Drawing.Point(4, 2);
            panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(516, 119);
            panel1.TabIndex = 3;
            // 
            // text
            // 
            this.text.AutoSize = true;
            this.text.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.text.Location = new System.Drawing.Point(8, 12);
            this.text.Name = "text";
            this.text.Size = new System.Drawing.Size(481, 29);
            this.text.TabIndex = 0;
            this.text.Text = "Loading full Request information from the Network...";
            // 
            // progress
            // 
            this.progress.Location = new System.Drawing.Point(12, 57);
            this.progress.MarqueeAnimationSpeed = 20;
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(490, 42);
            this.progress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progress.TabIndex = 1;
            // 
            // ProgressForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(525, 125);
            this.ControlBox = false;
            this.Controls.Add(panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Loading Request Information...";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label text;
        private System.Windows.Forms.ProgressBar progress;


    }
}