using Lpp.Dns.DataMart.Lib.Classes;
namespace Lpp.Dns.DataMart.Client
{
    partial class RequestDetailForm
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
                if(components != null)
                    components.Dispose();

                if (_domainManager != null)
                {
                    Processor = null;
                    _domainManager.Dispose();
                    _domainManager = null;
                }

                if(_patIDprogressForm != null)
                {
                    _patIDprogressForm.Dispose();
                    _patIDprogressForm = null;
                }
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Button btn_Close;
            System.Windows.Forms.Label lblSubmittedByNote;
            System.Windows.Forms.Label lblNetwork;
            System.Windows.Forms.Label lblSubmittedTo;
            System.Windows.Forms.Label queryCreatedByUserNameLabel;
            System.Windows.Forms.Label queryActivityPriorityLabel;
            System.Windows.Forms.Label activityLabel;
            System.Windows.Forms.Label lblDataMart;
            System.Windows.Forms.Label queryNameLabel;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label requestTimeLabel;
            System.Windows.Forms.Label activityDueDateLabel;
            System.Windows.Forms.Label queryIdLabel;
            System.Windows.Forms.Label queryStatusTypeIdLabel;
            System.Windows.Forms.Label requestorEmailLabel;
            System.Windows.Forms.Label lblAdditionalInstructions;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RequestDetailForm));
            this.queryDataMartBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.FileDialog = new System.Windows.Forms.OpenFileDialog();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.lblStatusBarProgress = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageOverview = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.txtAdditionalInstructions = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.vpAttachments = new Lpp.Dns.DataMart.Client.Controls.DataMartViewPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtRequestTimeLocal = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.txtRequestorCenter = new System.Windows.Forms.TextBox();
            this.txtSourceTaskOrder = new System.Windows.Forms.TextBox();
            this.lblRequesterCenter = new System.Windows.Forms.Label();
            this.lblSourceTaskOrder = new System.Windows.Forms.Label();
            this.txtTaskOrder = new System.Windows.Forms.TextBox();
            this.queryCreatedByUserNameTextBox = new System.Windows.Forms.TextBox();
            this.lblTaskOrder = new System.Windows.Forms.Label();
            this.txtPurposeOfUse = new System.Windows.Forms.TextBox();
            this.lblPurposeOfUse = new System.Windows.Forms.Label();
            this.txtNetwork = new System.Windows.Forms.TextBox();
            this.activityTextbox = new System.Windows.Forms.TextBox();
            this.txtDataMart = new System.Windows.Forms.TextBox();
            this.txtWorkplanType = new System.Windows.Forms.TextBox();
            this.lblWorkplanType = new System.Windows.Forms.Label();
            this.txtActivity = new System.Windows.Forms.TextBox();
            this.lblActivity = new System.Windows.Forms.Label();
            this.txtSourceActivity = new System.Windows.Forms.TextBox();
            this.lblSourceActivity = new System.Windows.Forms.Label();
            this.txtPhiDisclosureLevel = new System.Windows.Forms.TextBox();
            this.lblPhiDisclosureLevel = new System.Windows.Forms.Label();
            this.txtActivityProject = new System.Windows.Forms.TextBox();
            this.queryIdTextBox = new System.Windows.Forms.TextBox();
            this.lblSourceActivityProject = new System.Windows.Forms.Label();
            this.lblActivityProject = new System.Windows.Forms.Label();
            this.txtSourceActivityProject = new System.Windows.Forms.TextBox();
            this.requestorEmailTextBox = new System.Windows.Forms.TextBox();
            this.lblReportAggregationLevel = new System.Windows.Forms.Label();
            this.txtReportAggregationLevel = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tabPageDescription = new System.Windows.Forms.TabPage();
            this.descriptionBrowser = new System.Windows.Forms.WebBrowser();
            this.tabPageRequestDetails = new System.Windows.Forms.TabPage();
            this.tlpRequestDetails = new System.Windows.Forms.TableLayoutPanel();
            this.flowPanelRequestDetailsButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnHold = new System.Windows.Forms.Button();
            this.btnRejectQuery = new System.Windows.Forms.Button();
            this.btnViewSQL = new System.Windows.Forms.Button();
            this.cbRequestFileList = new System.Windows.Forms.CheckBox();
            this.vpRequest = new Lpp.Dns.DataMart.Client.Controls.DataMartViewPanel();
            this.chkGeneratePATIDList = new System.Windows.Forms.CheckBox();
            this.tabPageResponseDetails = new System.Windows.Forms.TabPage();
            this.tlpResponseDetails = new System.Windows.Forms.TableLayoutPanel();
            this.vpResponse = new Lpp.Dns.DataMart.Client.Controls.DataMartViewPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddFile = new System.Windows.Forms.Button();
            this.btnDeleteFile = new System.Windows.Forms.Button();
            this.btnPostProcess = new System.Windows.Forms.Button();
            this.btnClearCache = new System.Windows.Forms.Button();
            this.btnExportResults = new System.Windows.Forms.Button();
            this.btnUploadResults = new System.Windows.Forms.Button();
            this.cbResponseFileList = new System.Windows.Forms.CheckBox();
            this.queryActivityPriorityTextbox = new System.Windows.Forms.TextBox();
            this.queryNameTextBox = new System.Windows.Forms.TextBox();
            this.activityDueDateTextbox = new System.Windows.Forms.TextBox();
            this.queryStatusTypeIdTextBox = new System.Windows.Forms.TextBox();
            this.queryMSRequestIDTextBox = new System.Windows.Forms.TextBox();
            this.queryMSRequestIDLabel = new System.Windows.Forms.Label();
            this.tlpHeaderDetails = new System.Windows.Forms.TableLayoutPanel();
            this.panelHeaderFiller = new System.Windows.Forms.Panel();
            this.lblInputAttachments = new System.Windows.Forms.Label();
            btn_Close = new System.Windows.Forms.Button();
            lblSubmittedByNote = new System.Windows.Forms.Label();
            lblNetwork = new System.Windows.Forms.Label();
            lblSubmittedTo = new System.Windows.Forms.Label();
            queryCreatedByUserNameLabel = new System.Windows.Forms.Label();
            queryActivityPriorityLabel = new System.Windows.Forms.Label();
            activityLabel = new System.Windows.Forms.Label();
            lblDataMart = new System.Windows.Forms.Label();
            queryNameLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            requestTimeLabel = new System.Windows.Forms.Label();
            activityDueDateLabel = new System.Windows.Forms.Label();
            queryIdLabel = new System.Windows.Forms.Label();
            queryStatusTypeIdLabel = new System.Windows.Forms.Label();
            requestorEmailLabel = new System.Windows.Forms.Label();
            lblAdditionalInstructions = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.queryDataMartBindingSource)).BeginInit();
            this.pnlFooter.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageOverview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabPageDescription.SuspendLayout();
            this.tabPageRequestDetails.SuspendLayout();
            this.tlpRequestDetails.SuspendLayout();
            this.flowPanelRequestDetailsButtons.SuspendLayout();
            this.tabPageResponseDetails.SuspendLayout();
            this.tlpResponseDetails.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tlpHeaderDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Close
            // 
            btn_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            btn_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btn_Close.Location = new System.Drawing.Point(911, 6);
            btn_Close.Margin = new System.Windows.Forms.Padding(2);
            btn_Close.Name = "btn_Close";
            btn_Close.Size = new System.Drawing.Size(80, 24);
            btn_Close.TabIndex = 0;
            btn_Close.Text = "Close";
            btn_Close.UseVisualStyleBackColor = true;
            btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // lblSubmittedByNote
            // 
            lblSubmittedByNote.AutoSize = true;
            lblSubmittedByNote.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "CreatedByMessage", true));
            lblSubmittedByNote.Location = new System.Drawing.Point(90, 208);
            lblSubmittedByNote.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblSubmittedByNote.Name = "lblSubmittedByNote";
            lblSubmittedByNote.Size = new System.Drawing.Size(0, 13);
            lblSubmittedByNote.TabIndex = 62;
            lblSubmittedByNote.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // queryDataMartBindingSource
            // 
            this.queryDataMartBindingSource.DataSource = typeof(Lpp.Dns.DataMart.Lib.Classes.HubRequest);
            // 
            // lblNetwork
            // 
            lblNetwork.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblNetwork.AutoSize = true;
            lblNetwork.Location = new System.Drawing.Point(68, 7);
            lblNetwork.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblNetwork.Name = "lblNetwork";
            lblNetwork.Size = new System.Drawing.Size(50, 13);
            lblNetwork.TabIndex = 40;
            lblNetwork.Text = "Network:";
            // 
            // lblSubmittedTo
            // 
            lblSubmittedTo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblSubmittedTo.AutoSize = true;
            lblSubmittedTo.Location = new System.Drawing.Point(45, 197);
            lblSubmittedTo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblSubmittedTo.Name = "lblSubmittedTo";
            lblSubmittedTo.Size = new System.Drawing.Size(73, 13);
            lblSubmittedTo.TabIndex = 65;
            lblSubmittedTo.Text = "Submitted To:";
            // 
            // queryCreatedByUserNameLabel
            // 
            queryCreatedByUserNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            queryCreatedByUserNameLabel.AutoSize = true;
            queryCreatedByUserNameLabel.Location = new System.Drawing.Point(59, 61);
            queryCreatedByUserNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            queryCreatedByUserNameLabel.Name = "queryCreatedByUserNameLabel";
            queryCreatedByUserNameLabel.Size = new System.Drawing.Size(59, 13);
            queryCreatedByUserNameLabel.TabIndex = 38;
            queryCreatedByUserNameLabel.Text = "Requestor:";
            // 
            // queryActivityPriorityLabel
            // 
            queryActivityPriorityLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            queryActivityPriorityLabel.AutoSize = true;
            queryActivityPriorityLabel.Location = new System.Drawing.Point(77, 30);
            queryActivityPriorityLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            queryActivityPriorityLabel.Name = "queryActivityPriorityLabel";
            queryActivityPriorityLabel.Size = new System.Drawing.Size(41, 13);
            queryActivityPriorityLabel.TabIndex = 68;
            queryActivityPriorityLabel.Text = "Priority:";
            // 
            // activityLabel
            // 
            activityLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            activityLabel.AutoSize = true;
            activityLabel.Location = new System.Drawing.Point(378, 7);
            activityLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            activityLabel.Name = "activityLabel";
            activityLabel.Size = new System.Drawing.Size(43, 13);
            activityLabel.TabIndex = 70;
            activityLabel.Text = "Project:";
            // 
            // lblDataMart
            // 
            lblDataMart.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblDataMart.AutoSize = true;
            lblDataMart.Location = new System.Drawing.Point(730, 7);
            lblDataMart.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblDataMart.Name = "lblDataMart";
            lblDataMart.Size = new System.Drawing.Size(54, 13);
            lblDataMart.TabIndex = 40;
            lblDataMart.Text = "DataMart:";
            // 
            // queryNameLabel
            // 
            queryNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            queryNameLabel.AutoSize = true;
            queryNameLabel.Location = new System.Drawing.Point(37, 5);
            queryNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            queryNameLabel.Name = "queryNameLabel";
            queryNameLabel.Size = new System.Drawing.Size(81, 13);
            queryNameLabel.TabIndex = 37;
            queryNameLabel.Text = "Request Name:";
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(41, 34);
            label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(77, 13);
            label1.TabIndex = 80;
            label1.Text = "Request Type:";
            // 
            // requestTimeLabel
            // 
            requestTimeLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            requestTimeLabel.AutoSize = true;
            requestTimeLabel.Location = new System.Drawing.Point(345, 61);
            requestTimeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            requestTimeLabel.Name = "requestTimeLabel";
            requestTimeLabel.Size = new System.Drawing.Size(76, 13);
            requestTimeLabel.TabIndex = 39;
            requestTimeLabel.Text = "Request Time:";
            // 
            // activityDueDateLabel
            // 
            activityDueDateLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            activityDueDateLabel.AutoSize = true;
            activityDueDateLabel.Location = new System.Drawing.Point(370, 30);
            activityDueDateLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            activityDueDateLabel.Name = "activityDueDateLabel";
            activityDueDateLabel.Size = new System.Drawing.Size(56, 13);
            activityDueDateLabel.TabIndex = 72;
            activityDueDateLabel.Text = "Due Date:";
            // 
            // queryIdLabel
            // 
            queryIdLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            queryIdLabel.AutoSize = true;
            queryIdLabel.Location = new System.Drawing.Point(700, 34);
            queryIdLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            queryIdLabel.Name = "queryIdLabel";
            queryIdLabel.Size = new System.Drawing.Size(84, 13);
            queryIdLabel.TabIndex = 40;
            queryIdLabel.Text = "System Number:";
            // 
            // queryStatusTypeIdLabel
            // 
            queryStatusTypeIdLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            queryStatusTypeIdLabel.AutoSize = true;
            queryStatusTypeIdLabel.Location = new System.Drawing.Point(754, 30);
            queryStatusTypeIdLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            queryStatusTypeIdLabel.Name = "queryStatusTypeIdLabel";
            queryStatusTypeIdLabel.Size = new System.Drawing.Size(40, 13);
            queryStatusTypeIdLabel.TabIndex = 40;
            queryStatusTypeIdLabel.Text = "Status:";
            // 
            // requestorEmailLabel
            // 
            requestorEmailLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            requestorEmailLabel.AutoSize = true;
            requestorEmailLabel.Location = new System.Drawing.Point(749, 61);
            requestorEmailLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            requestorEmailLabel.Name = "requestorEmailLabel";
            requestorEmailLabel.Size = new System.Drawing.Size(35, 13);
            requestorEmailLabel.TabIndex = 40;
            requestorEmailLabel.Text = "Email:";
            // 
            // lblAdditionalInstructions
            // 
            lblAdditionalInstructions.Anchor = System.Windows.Forms.AnchorStyles.Left;
            lblAdditionalInstructions.AutoSize = true;
            lblAdditionalInstructions.Location = new System.Drawing.Point(4, 3);
            lblAdditionalInstructions.Margin = new System.Windows.Forms.Padding(4, 0, 3, 0);
            lblAdditionalInstructions.Name = "lblAdditionalInstructions";
            lblAdditionalInstructions.Size = new System.Drawing.Size(113, 13);
            lblAdditionalInstructions.TabIndex = 61;
            lblAdditionalInstructions.Text = "Additional Instructions:";
            // 
            // FileDialog
            // 
            this.FileDialog.FileName = "openFileDialog1";
            // 
            // buttonPanel
            // 
            this.buttonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPanel.AutoScroll = true;
            this.buttonPanel.AutoScrollMinSize = new System.Drawing.Size(1200, 0);
            this.buttonPanel.Location = new System.Drawing.Point(0, 1200);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(936, 67);
            this.buttonPanel.TabIndex = 116;
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.lblStatusBarProgress);
            this.pnlFooter.Controls.Add(btn_Close);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 687);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(1004, 34);
            this.pnlFooter.TabIndex = 2;
            // 
            // lblStatusBarProgress
            // 
            this.lblStatusBarProgress.AutoSize = true;
            this.lblStatusBarProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusBarProgress.ForeColor = System.Drawing.Color.Green;
            this.lblStatusBarProgress.Location = new System.Drawing.Point(11, 12);
            this.lblStatusBarProgress.Name = "lblStatusBarProgress";
            this.lblStatusBarProgress.Size = new System.Drawing.Size(65, 13);
            this.lblStatusBarProgress.TabIndex = 37;
            this.lblStatusBarProgress.Text = "Pending...";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageOverview);
            this.tabControl1.Controls.Add(this.tabPageDescription);
            this.tabControl1.Controls.Add(this.tabPageRequestDetails);
            this.tabControl1.Controls.Add(this.tabPageResponseDetails);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.ItemSize = new System.Drawing.Size(57, 24);
            this.tabControl1.Location = new System.Drawing.Point(0, 58);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(24, 3);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1004, 629);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPageOverview
            // 
            this.tabPageOverview.Controls.Add(this.splitContainer2);
            this.tabPageOverview.Controls.Add(this.tableLayoutPanel1);
            this.tabPageOverview.Location = new System.Drawing.Point(4, 28);
            this.tabPageOverview.Name = "tabPageOverview";
            this.tabPageOverview.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOverview.Size = new System.Drawing.Size(996, 597);
            this.tabPageOverview.TabIndex = 0;
            this.tabPageOverview.Text = "Overview";
            this.tabPageOverview.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 221);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tableLayoutPanel4);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutPanel5);
            this.splitContainer2.Size = new System.Drawing.Size(990, 373);
            this.splitContainer2.SplitterDistance = 146;
            this.splitContainer2.TabIndex = 116;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(lblAdditionalInstructions, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.txtAdditionalInstructions, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(990, 146);
            this.tableLayoutPanel4.TabIndex = 63;
            // 
            // txtAdditionalInstructions
            // 
            this.tableLayoutPanel4.SetColumnSpan(this.txtAdditionalInstructions, 2);
            this.txtAdditionalInstructions.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.AdditionalInstructions", true));
            this.txtAdditionalInstructions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAdditionalInstructions.Location = new System.Drawing.Point(3, 23);
            this.txtAdditionalInstructions.Multiline = true;
            this.txtAdditionalInstructions.Name = "txtAdditionalInstructions";
            this.txtAdditionalInstructions.ReadOnly = true;
            this.txtAdditionalInstructions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAdditionalInstructions.Size = new System.Drawing.Size(984, 120);
            this.txtAdditionalInstructions.TabIndex = 0;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.vpAttachments, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.lblInputAttachments, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Size = new System.Drawing.Size(990, 223);
            this.tableLayoutPanel5.TabIndex = 115;
            // 
            // vpAttachments
            // 
            this.tableLayoutPanel5.SetColumnSpan(this.vpAttachments, 2);
            this.vpAttachments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vpAttachments.LastView = null;
            this.vpAttachments.Location = new System.Drawing.Point(3, 23);
            this.vpAttachments.Name = "vpAttachments";
            this.vpAttachments.ShowView = Lpp.Dns.DataMart.Client.Controls.ViewPanel.DisplayType.FILELIST;
            this.vpAttachments.Size = new System.Drawing.Size(984, 197);
            this.vpAttachments.TabIndex = 0;
            this.vpAttachments.ViewText = "";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.txtRequestTimeLocal, 3, 2);
            this.tableLayoutPanel1.Controls.Add(lblNetwork, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 1, 7);
            this.tableLayoutPanel1.Controls.Add(lblSubmittedTo, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.txtRequestorCenter, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtSourceTaskOrder, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblRequesterCenter, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblSourceTaskOrder, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtTaskOrder, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.queryCreatedByUserNameTextBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblTaskOrder, 0, 5);
            this.tableLayoutPanel1.Controls.Add(queryCreatedByUserNameLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtPurposeOfUse, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblPurposeOfUse, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtNetwork, 1, 0);
            this.tableLayoutPanel1.Controls.Add(activityLabel, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.activityTextbox, 3, 0);
            this.tableLayoutPanel1.Controls.Add(lblDataMart, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtDataMart, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtWorkplanType, 3, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblWorkplanType, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtActivity, 3, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblActivity, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtSourceActivity, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblSourceActivity, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtPhiDisclosureLevel, 3, 3);
            this.tableLayoutPanel1.Controls.Add(requestTimeLabel, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblPhiDisclosureLevel, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtActivityProject, 5, 5);
            this.tableLayoutPanel1.Controls.Add(queryIdLabel, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.queryIdTextBox, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblSourceActivityProject, 4, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblActivityProject, 4, 5);
            this.tableLayoutPanel1.Controls.Add(requestorEmailLabel, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtSourceActivityProject, 5, 4);
            this.tableLayoutPanel1.Controls.Add(this.requestorEmailTextBox, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblReportAggregationLevel, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtReportAggregationLevel, 5, 3);
            this.tableLayoutPanel1.Controls.Add(label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBox2, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(990, 218);
            this.tableLayoutPanel1.TabIndex = 115;
            // 
            // txtRequestTimeLocal
            // 
            this.txtRequestTimeLocal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRequestTimeLocal.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.CreatedOnLocal", true, System.Windows.Forms.DataSourceUpdateMode.Never, null, "g"));
            this.txtRequestTimeLocal.Location = new System.Drawing.Point(426, 57);
            this.txtRequestTimeLocal.Name = "txtRequestTimeLocal";
            this.txtRequestTimeLocal.ReadOnly = true;
            this.txtRequestTimeLocal.Size = new System.Drawing.Size(197, 20);
            this.txtRequestTimeLocal.TabIndex = 6;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.textBox1, 5);
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "SubmittedDataMarts", true));
            this.textBox1.Location = new System.Drawing.Point(123, 193);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(864, 20);
            this.textBox1.TabIndex = 19;
            // 
            // txtRequestorCenter
            // 
            this.txtRequestorCenter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRequestorCenter.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.RequestorCenter", true));
            this.txtRequestorCenter.Location = new System.Drawing.Point(123, 165);
            this.txtRequestorCenter.Name = "txtRequestorCenter";
            this.txtRequestorCenter.ReadOnly = true;
            this.txtRequestorCenter.Size = new System.Drawing.Size(197, 20);
            this.txtRequestorCenter.TabIndex = 17;
            // 
            // txtSourceTaskOrder
            // 
            this.txtSourceTaskOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSourceTaskOrder.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.SourceTaskOrder", true));
            this.txtSourceTaskOrder.Location = new System.Drawing.Point(123, 111);
            this.txtSourceTaskOrder.Name = "txtSourceTaskOrder";
            this.txtSourceTaskOrder.ReadOnly = true;
            this.txtSourceTaskOrder.Size = new System.Drawing.Size(197, 20);
            this.txtSourceTaskOrder.TabIndex = 11;
            // 
            // lblRequesterCenter
            // 
            this.lblRequesterCenter.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblRequesterCenter.AutoSize = true;
            this.lblRequesterCenter.Location = new System.Drawing.Point(23, 169);
            this.lblRequesterCenter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRequesterCenter.Name = "lblRequesterCenter";
            this.lblRequesterCenter.Size = new System.Drawing.Size(93, 13);
            this.lblRequesterCenter.TabIndex = 97;
            this.lblRequesterCenter.Text = "Requester Center:";
            // 
            // lblSourceTaskOrder
            // 
            this.lblSourceTaskOrder.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblSourceTaskOrder.AutoSize = true;
            this.lblSourceTaskOrder.Location = new System.Drawing.Point(16, 115);
            this.lblSourceTaskOrder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSourceTaskOrder.Name = "lblSourceTaskOrder";
            this.lblSourceTaskOrder.Size = new System.Drawing.Size(100, 13);
            this.lblSourceTaskOrder.TabIndex = 107;
            this.lblSourceTaskOrder.Text = "Source Task Order:";
            // 
            // txtTaskOrder
            // 
            this.txtTaskOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTaskOrder.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.TaskOrder", true));
            this.txtTaskOrder.Location = new System.Drawing.Point(123, 138);
            this.txtTaskOrder.Name = "txtTaskOrder";
            this.txtTaskOrder.ReadOnly = true;
            this.txtTaskOrder.Size = new System.Drawing.Size(197, 20);
            this.txtTaskOrder.TabIndex = 14;
            // 
            // queryCreatedByUserNameTextBox
            // 
            this.queryCreatedByUserNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.queryCreatedByUserNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.Author.Username", true));
            this.queryCreatedByUserNameTextBox.Location = new System.Drawing.Point(123, 57);
            this.queryCreatedByUserNameTextBox.Name = "queryCreatedByUserNameTextBox";
            this.queryCreatedByUserNameTextBox.ReadOnly = true;
            this.queryCreatedByUserNameTextBox.Size = new System.Drawing.Size(197, 20);
            this.queryCreatedByUserNameTextBox.TabIndex = 5;
            // 
            // lblTaskOrder
            // 
            this.lblTaskOrder.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTaskOrder.AutoSize = true;
            this.lblTaskOrder.Location = new System.Drawing.Point(16, 142);
            this.lblTaskOrder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTaskOrder.Name = "lblTaskOrder";
            this.lblTaskOrder.Size = new System.Drawing.Size(100, 13);
            this.lblTaskOrder.TabIndex = 90;
            this.lblTaskOrder.Text = "Budget Task Order:";
            // 
            // txtPurposeOfUse
            // 
            this.txtPurposeOfUse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPurposeOfUse.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.PurposeOfUse", true));
            this.txtPurposeOfUse.Location = new System.Drawing.Point(123, 84);
            this.txtPurposeOfUse.Name = "txtPurposeOfUse";
            this.txtPurposeOfUse.ReadOnly = true;
            this.txtPurposeOfUse.Size = new System.Drawing.Size(197, 20);
            this.txtPurposeOfUse.TabIndex = 8;
            // 
            // lblPurposeOfUse
            // 
            this.lblPurposeOfUse.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPurposeOfUse.AutoSize = true;
            this.lblPurposeOfUse.Location = new System.Drawing.Point(33, 88);
            this.lblPurposeOfUse.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPurposeOfUse.Name = "lblPurposeOfUse";
            this.lblPurposeOfUse.Size = new System.Drawing.Size(83, 13);
            this.lblPurposeOfUse.TabIndex = 88;
            this.lblPurposeOfUse.Text = "Purpose of Use:";
            // 
            // txtNetwork
            // 
            this.txtNetwork.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNetwork.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "NetworkName", true));
            this.txtNetwork.Location = new System.Drawing.Point(123, 3);
            this.txtNetwork.Name = "txtNetwork";
            this.txtNetwork.ReadOnly = true;
            this.txtNetwork.Size = new System.Drawing.Size(197, 20);
            this.txtNetwork.TabIndex = 0;
            // 
            // activityTextbox
            // 
            this.activityTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.activityTextbox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "ProjectName", true));
            this.activityTextbox.Location = new System.Drawing.Point(426, 3);
            this.activityTextbox.Name = "activityTextbox";
            this.activityTextbox.ReadOnly = true;
            this.activityTextbox.Size = new System.Drawing.Size(197, 20);
            this.activityTextbox.TabIndex = 1;
            // 
            // txtDataMart
            // 
            this.txtDataMart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDataMart.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "DataMartName", true));
            this.txtDataMart.Location = new System.Drawing.Point(789, 3);
            this.txtDataMart.Name = "txtDataMart";
            this.txtDataMart.ReadOnly = true;
            this.txtDataMart.Size = new System.Drawing.Size(198, 20);
            this.txtDataMart.TabIndex = 2;
            // 
            // txtWorkplanType
            // 
            this.txtWorkplanType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWorkplanType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.WorkplanType", true));
            this.txtWorkplanType.Location = new System.Drawing.Point(426, 165);
            this.txtWorkplanType.Name = "txtWorkplanType";
            this.txtWorkplanType.ReadOnly = true;
            this.txtWorkplanType.Size = new System.Drawing.Size(197, 20);
            this.txtWorkplanType.TabIndex = 18;
            // 
            // lblWorkplanType
            // 
            this.lblWorkplanType.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblWorkplanType.AutoSize = true;
            this.lblWorkplanType.Location = new System.Drawing.Point(336, 169);
            this.lblWorkplanType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWorkplanType.Name = "lblWorkplanType";
            this.lblWorkplanType.Size = new System.Drawing.Size(83, 13);
            this.lblWorkplanType.TabIndex = 100;
            this.lblWorkplanType.Text = "Workplan Type:";
            // 
            // txtActivity
            // 
            this.txtActivity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtActivity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.Activity", true));
            this.txtActivity.Location = new System.Drawing.Point(426, 138);
            this.txtActivity.Name = "txtActivity";
            this.txtActivity.ReadOnly = true;
            this.txtActivity.Size = new System.Drawing.Size(197, 20);
            this.txtActivity.TabIndex = 15;
            // 
            // lblActivity
            // 
            this.lblActivity.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblActivity.AutoSize = true;
            this.lblActivity.Location = new System.Drawing.Point(338, 142);
            this.lblActivity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblActivity.Name = "lblActivity";
            this.lblActivity.Size = new System.Drawing.Size(81, 13);
            this.lblActivity.TabIndex = 91;
            this.lblActivity.Text = "Budget Activity:";
            // 
            // txtSourceActivity
            // 
            this.txtSourceActivity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSourceActivity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.SourceActivity", true));
            this.txtSourceActivity.Location = new System.Drawing.Point(426, 111);
            this.txtSourceActivity.Name = "txtSourceActivity";
            this.txtSourceActivity.ReadOnly = true;
            this.txtSourceActivity.Size = new System.Drawing.Size(197, 20);
            this.txtSourceActivity.TabIndex = 12;
            // 
            // lblSourceActivity
            // 
            this.lblSourceActivity.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblSourceActivity.AutoSize = true;
            this.lblSourceActivity.Location = new System.Drawing.Point(338, 115);
            this.lblSourceActivity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSourceActivity.Name = "lblSourceActivity";
            this.lblSourceActivity.Size = new System.Drawing.Size(81, 13);
            this.lblSourceActivity.TabIndex = 108;
            this.lblSourceActivity.Text = "Source Activity:";
            // 
            // txtPhiDisclosureLevel
            // 
            this.txtPhiDisclosureLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPhiDisclosureLevel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.PhiDisclosureLevel", true));
            this.txtPhiDisclosureLevel.Location = new System.Drawing.Point(426, 84);
            this.txtPhiDisclosureLevel.Name = "txtPhiDisclosureLevel";
            this.txtPhiDisclosureLevel.ReadOnly = true;
            this.txtPhiDisclosureLevel.Size = new System.Drawing.Size(197, 20);
            this.txtPhiDisclosureLevel.TabIndex = 9;
            // 
            // lblPhiDisclosureLevel
            // 
            this.lblPhiDisclosureLevel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPhiDisclosureLevel.AutoSize = true;
            this.lblPhiDisclosureLevel.Location = new System.Drawing.Point(350, 88);
            this.lblPhiDisclosureLevel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPhiDisclosureLevel.Name = "lblPhiDisclosureLevel";
            this.lblPhiDisclosureLevel.Size = new System.Drawing.Size(69, 13);
            this.lblPhiDisclosureLevel.TabIndex = 89;
            this.lblPhiDisclosureLevel.Text = "Level of PHI:";
            // 
            // txtActivityProject
            // 
            this.txtActivityProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtActivityProject.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.ActivityProject", true));
            this.txtActivityProject.Location = new System.Drawing.Point(789, 138);
            this.txtActivityProject.Name = "txtActivityProject";
            this.txtActivityProject.ReadOnly = true;
            this.txtActivityProject.Size = new System.Drawing.Size(198, 20);
            this.txtActivityProject.TabIndex = 16;
            // 
            // queryIdTextBox
            // 
            this.queryIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.queryIdTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.Identifier", true));
            this.queryIdTextBox.Location = new System.Drawing.Point(789, 30);
            this.queryIdTextBox.Name = "queryIdTextBox";
            this.queryIdTextBox.ReadOnly = true;
            this.queryIdTextBox.Size = new System.Drawing.Size(198, 20);
            this.queryIdTextBox.TabIndex = 4;
            // 
            // lblSourceActivityProject
            // 
            this.lblSourceActivityProject.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblSourceActivityProject.AutoSize = true;
            this.lblSourceActivityProject.Location = new System.Drawing.Point(665, 115);
            this.lblSourceActivityProject.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSourceActivityProject.Name = "lblSourceActivityProject";
            this.lblSourceActivityProject.Size = new System.Drawing.Size(117, 13);
            this.lblSourceActivityProject.TabIndex = 109;
            this.lblSourceActivityProject.Text = "Source Activity Project:";
            // 
            // lblActivityProject
            // 
            this.lblActivityProject.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblActivityProject.AutoSize = true;
            this.lblActivityProject.Location = new System.Drawing.Point(665, 142);
            this.lblActivityProject.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblActivityProject.Name = "lblActivityProject";
            this.lblActivityProject.Size = new System.Drawing.Size(117, 13);
            this.lblActivityProject.TabIndex = 92;
            this.lblActivityProject.Text = "Budget Activity Project:";
            // 
            // txtSourceActivityProject
            // 
            this.txtSourceActivityProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSourceActivityProject.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.SourceActivityProject", true));
            this.txtSourceActivityProject.Location = new System.Drawing.Point(789, 111);
            this.txtSourceActivityProject.Name = "txtSourceActivityProject";
            this.txtSourceActivityProject.ReadOnly = true;
            this.txtSourceActivityProject.Size = new System.Drawing.Size(198, 20);
            this.txtSourceActivityProject.TabIndex = 13;
            // 
            // requestorEmailTextBox
            // 
            this.requestorEmailTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.requestorEmailTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.Author.Email", true));
            this.requestorEmailTextBox.Location = new System.Drawing.Point(789, 57);
            this.requestorEmailTextBox.Name = "requestorEmailTextBox";
            this.requestorEmailTextBox.ReadOnly = true;
            this.requestorEmailTextBox.Size = new System.Drawing.Size(198, 20);
            this.requestorEmailTextBox.TabIndex = 7;
            // 
            // lblReportAggregationLevel
            // 
            this.lblReportAggregationLevel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblReportAggregationLevel.AutoSize = true;
            this.lblReportAggregationLevel.Location = new System.Drawing.Point(639, 88);
            this.lblReportAggregationLevel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReportAggregationLevel.Name = "lblReportAggregationLevel";
            this.lblReportAggregationLevel.Size = new System.Drawing.Size(143, 13);
            this.lblReportAggregationLevel.TabIndex = 112;
            this.lblReportAggregationLevel.Text = "Level of Report Aggregation:";
            // 
            // txtReportAggregationLevel
            // 
            this.txtReportAggregationLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReportAggregationLevel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.ReportAggregationLevel", true));
            this.txtReportAggregationLevel.Location = new System.Drawing.Point(789, 84);
            this.txtReportAggregationLevel.Name = "txtReportAggregationLevel";
            this.txtReportAggregationLevel.ReadOnly = true;
            this.txtReportAggregationLevel.Size = new System.Drawing.Size(198, 20);
            this.txtReportAggregationLevel.TabIndex = 10;
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.textBox2, 3);
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.RequestTypeName", true));
            this.textBox2.Location = new System.Drawing.Point(123, 30);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(500, 20);
            this.textBox2.TabIndex = 3;
            // 
            // tabPageDescription
            // 
            this.tabPageDescription.Controls.Add(this.descriptionBrowser);
            this.tabPageDescription.Location = new System.Drawing.Point(4, 28);
            this.tabPageDescription.Name = "tabPageDescription";
            this.tabPageDescription.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDescription.Size = new System.Drawing.Size(996, 597);
            this.tabPageDescription.TabIndex = 3;
            this.tabPageDescription.Text = "Description";
            this.tabPageDescription.UseVisualStyleBackColor = true;
            // 
            // descriptionBrowser
            // 
            this.descriptionBrowser.AllowNavigation = false;
            this.descriptionBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descriptionBrowser.IsWebBrowserContextMenuEnabled = false;
            this.descriptionBrowser.Location = new System.Drawing.Point(3, 3);
            this.descriptionBrowser.Margin = new System.Windows.Forms.Padding(4);
            this.descriptionBrowser.Name = "descriptionBrowser";
            this.descriptionBrowser.Size = new System.Drawing.Size(990, 591);
            this.descriptionBrowser.TabIndex = 0;
            this.descriptionBrowser.WebBrowserShortcutsEnabled = false;
            // 
            // tabPageRequestDetails
            // 
            this.tabPageRequestDetails.Controls.Add(this.tlpRequestDetails);
            this.tabPageRequestDetails.Location = new System.Drawing.Point(4, 28);
            this.tabPageRequestDetails.Name = "tabPageRequestDetails";
            this.tabPageRequestDetails.Size = new System.Drawing.Size(996, 597);
            this.tabPageRequestDetails.TabIndex = 1;
            this.tabPageRequestDetails.Text = "Request Details";
            this.tabPageRequestDetails.UseVisualStyleBackColor = true;
            // 
            // tlpRequestDetails
            // 
            this.tlpRequestDetails.ColumnCount = 2;
            this.tlpRequestDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpRequestDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpRequestDetails.Controls.Add(this.flowPanelRequestDetailsButtons, 0, 2);
            this.tlpRequestDetails.Controls.Add(this.cbRequestFileList, 1, 0);
            this.tlpRequestDetails.Controls.Add(this.vpRequest, 0, 1);
            this.tlpRequestDetails.Controls.Add(this.chkGeneratePATIDList, 0, 0);
            this.tlpRequestDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRequestDetails.Location = new System.Drawing.Point(0, 0);
            this.tlpRequestDetails.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.tlpRequestDetails.Name = "tlpRequestDetails";
            this.tlpRequestDetails.RowCount = 3;
            this.tlpRequestDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpRequestDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRequestDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tlpRequestDetails.Size = new System.Drawing.Size(996, 597);
            this.tlpRequestDetails.TabIndex = 1;
            // 
            // flowPanelRequestDetailsButtons
            // 
            this.tlpRequestDetails.SetColumnSpan(this.flowPanelRequestDetailsButtons, 2);
            this.flowPanelRequestDetailsButtons.Controls.Add(this.btnRun);
            this.flowPanelRequestDetailsButtons.Controls.Add(this.btnHold);
            this.flowPanelRequestDetailsButtons.Controls.Add(this.btnRejectQuery);
            this.flowPanelRequestDetailsButtons.Controls.Add(this.btnViewSQL);
            this.flowPanelRequestDetailsButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPanelRequestDetailsButtons.Location = new System.Drawing.Point(3, 562);
            this.flowPanelRequestDetailsButtons.Name = "flowPanelRequestDetailsButtons";
            this.flowPanelRequestDetailsButtons.Padding = new System.Windows.Forms.Padding(4);
            this.flowPanelRequestDetailsButtons.Size = new System.Drawing.Size(990, 32);
            this.flowPanelRequestDetailsButtons.TabIndex = 4;
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRun.Location = new System.Drawing.Point(7, 7);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(80, 24);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnHold
            // 
            this.btnHold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHold.Location = new System.Drawing.Point(93, 7);
            this.btnHold.Name = "btnHold";
            this.btnHold.Size = new System.Drawing.Size(80, 24);
            this.btnHold.TabIndex = 1;
            this.btnHold.Text = "Hold";
            this.btnHold.UseVisualStyleBackColor = true;
            this.btnHold.Click += new System.EventHandler(this.btnHold_Click);
            // 
            // btnRejectQuery
            // 
            this.btnRejectQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRejectQuery.Location = new System.Drawing.Point(179, 7);
            this.btnRejectQuery.Name = "btnRejectQuery";
            this.btnRejectQuery.Size = new System.Drawing.Size(80, 24);
            this.btnRejectQuery.TabIndex = 2;
            this.btnRejectQuery.Text = "Reject";
            this.btnRejectQuery.UseVisualStyleBackColor = true;
            this.btnRejectQuery.Click += new System.EventHandler(this.btnRejectQuery_Click);
            // 
            // btnViewSQL
            // 
            this.btnViewSQL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnViewSQL.Location = new System.Drawing.Point(265, 7);
            this.btnViewSQL.Name = "btnViewSQL";
            this.btnViewSQL.Size = new System.Drawing.Size(80, 24);
            this.btnViewSQL.TabIndex = 3;
            this.btnViewSQL.Text = "View SQL";
            this.btnViewSQL.UseVisualStyleBackColor = true;
            this.btnViewSQL.Click += new System.EventHandler(this.btnViewSQL_Click);
            // 
            // cbRequestFileList
            // 
            this.cbRequestFileList.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cbRequestFileList.AutoSize = true;
            this.cbRequestFileList.Location = new System.Drawing.Point(888, 6);
            this.cbRequestFileList.Margin = new System.Windows.Forms.Padding(4);
            this.cbRequestFileList.Name = "cbRequestFileList";
            this.cbRequestFileList.Size = new System.Drawing.Size(104, 17);
            this.cbRequestFileList.TabIndex = 1;
            this.cbRequestFileList.Text = "View Raw File(s)";
            this.cbRequestFileList.UseVisualStyleBackColor = true;
            // 
            // vpRequest
            // 
            this.tlpRequestDetails.SetColumnSpan(this.vpRequest, 2);
            this.vpRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vpRequest.LastView = null;
            this.vpRequest.Location = new System.Drawing.Point(3, 33);
            this.vpRequest.Name = "vpRequest";
            this.vpRequest.ShowView = Lpp.Dns.DataMart.Client.Controls.ViewPanel.DisplayType.PLAIN;
            this.vpRequest.Size = new System.Drawing.Size(990, 523);
            this.vpRequest.TabIndex = 2;
            this.vpRequest.View = null;
            this.vpRequest.ViewText = "";
            // 
            // chkGeneratePATIDList
            // 
            this.chkGeneratePATIDList.AutoSize = true;
            this.chkGeneratePATIDList.Location = new System.Drawing.Point(3, 3);
            this.chkGeneratePATIDList.Name = "chkGeneratePATIDList";
            this.chkGeneratePATIDList.Size = new System.Drawing.Size(124, 17);
            this.chkGeneratePATIDList.TabIndex = 0;
            this.chkGeneratePATIDList.Text = "Generate PATID List";
            this.chkGeneratePATIDList.UseVisualStyleBackColor = true;
            // 
            // tabPageResponseDetails
            // 
            this.tabPageResponseDetails.Controls.Add(this.tlpResponseDetails);
            this.tabPageResponseDetails.Location = new System.Drawing.Point(4, 28);
            this.tabPageResponseDetails.Name = "tabPageResponseDetails";
            this.tabPageResponseDetails.Size = new System.Drawing.Size(996, 597);
            this.tabPageResponseDetails.TabIndex = 2;
            this.tabPageResponseDetails.Text = "Response Details";
            this.tabPageResponseDetails.UseVisualStyleBackColor = true;
            // 
            // tlpResponseDetails
            // 
            this.tlpResponseDetails.ColumnCount = 2;
            this.tlpResponseDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpResponseDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpResponseDetails.Controls.Add(this.vpResponse, 0, 1);
            this.tlpResponseDetails.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tlpResponseDetails.Controls.Add(this.cbResponseFileList, 1, 0);
            this.tlpResponseDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpResponseDetails.Location = new System.Drawing.Point(0, 0);
            this.tlpResponseDetails.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.tlpResponseDetails.Name = "tlpResponseDetails";
            this.tlpResponseDetails.RowCount = 3;
            this.tlpResponseDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpResponseDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpResponseDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tlpResponseDetails.Size = new System.Drawing.Size(996, 597);
            this.tlpResponseDetails.TabIndex = 0;
            // 
            // vpResponse
            // 
            this.tlpResponseDetails.SetColumnSpan(this.vpResponse, 2);
            this.vpResponse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vpResponse.LastView = null;
            this.vpResponse.Location = new System.Drawing.Point(3, 33);
            this.vpResponse.Name = "vpResponse";
            this.vpResponse.ShowView = Lpp.Dns.DataMart.Client.Controls.ViewPanel.DisplayType.PLAIN;
            this.vpResponse.Size = new System.Drawing.Size(990, 523);
            this.vpResponse.TabIndex = 1;
            this.vpResponse.View = null;
            this.vpResponse.ViewText = "";
            // 
            // flowLayoutPanel1
            // 
            this.tlpResponseDetails.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.btnAddFile);
            this.flowLayoutPanel1.Controls.Add(this.btnDeleteFile);
            this.flowLayoutPanel1.Controls.Add(this.btnPostProcess);
            this.flowLayoutPanel1.Controls.Add(this.btnClearCache);
            this.flowLayoutPanel1.Controls.Add(this.btnExportResults);
            this.flowLayoutPanel1.Controls.Add(this.btnUploadResults);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 562);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(990, 32);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // btnAddFile
            // 
            this.btnAddFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddFile.Location = new System.Drawing.Point(7, 7);
            this.btnAddFile.Name = "btnAddFile";
            this.btnAddFile.Size = new System.Drawing.Size(80, 24);
            this.btnAddFile.TabIndex = 0;
            this.btnAddFile.Text = "Add File";
            this.btnAddFile.UseVisualStyleBackColor = true;
            this.btnAddFile.Click += new System.EventHandler(this.btnAddFile_Click);
            // 
            // btnDeleteFile
            // 
            this.btnDeleteFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteFile.Location = new System.Drawing.Point(93, 7);
            this.btnDeleteFile.Name = "btnDeleteFile";
            this.btnDeleteFile.Size = new System.Drawing.Size(80, 24);
            this.btnDeleteFile.TabIndex = 1;
            this.btnDeleteFile.Text = "Delete File";
            this.btnDeleteFile.UseVisualStyleBackColor = true;
            this.btnDeleteFile.Click += new System.EventHandler(this.btnDeleteFile_Click);
            // 
            // btnPostProcess
            // 
            this.btnPostProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPostProcess.Enabled = false;
            this.btnPostProcess.Location = new System.Drawing.Point(179, 7);
            this.btnPostProcess.Name = "btnPostProcess";
            this.btnPostProcess.Size = new System.Drawing.Size(112, 24);
            this.btnPostProcess.TabIndex = 2;
            this.btnPostProcess.Text = "Suppress Low Cells";
            this.btnPostProcess.UseVisualStyleBackColor = true;
            this.btnPostProcess.Click += new System.EventHandler(this.btnPostProcess_Click);
            // 
            // btnClearCache
            // 
            this.btnClearCache.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearCache.Enabled = false;
            this.btnClearCache.Location = new System.Drawing.Point(297, 7);
            this.btnClearCache.Name = "btnClearCache";
            this.btnClearCache.Size = new System.Drawing.Size(112, 24);
            this.btnClearCache.TabIndex = 3;
            this.btnClearCache.Text = "Clear Cache";
            this.btnClearCache.UseVisualStyleBackColor = true;
            this.btnClearCache.Click += new System.EventHandler(this.btnClearCache_Click);
            // 
            // btnExportResults
            // 
            this.btnExportResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExportResults.Location = new System.Drawing.Point(415, 7);
            this.btnExportResults.Name = "btnExportResults";
            this.btnExportResults.Size = new System.Drawing.Size(102, 24);
            this.btnExportResults.TabIndex = 4;
            this.btnExportResults.Text = "Export Results..";
            this.btnExportResults.UseVisualStyleBackColor = true;
            this.btnExportResults.Click += new System.EventHandler(this.btnExportResults_Click);
            // 
            // btnUploadResults
            // 
            this.btnUploadResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUploadResults.Location = new System.Drawing.Point(523, 7);
            this.btnUploadResults.Name = "btnUploadResults";
            this.btnUploadResults.Size = new System.Drawing.Size(102, 24);
            this.btnUploadResults.TabIndex = 5;
            this.btnUploadResults.Text = "Upload Results";
            this.btnUploadResults.UseVisualStyleBackColor = true;
            this.btnUploadResults.Click += new System.EventHandler(this.btnUploadResults_Click);
            // 
            // cbResponseFileList
            // 
            this.cbResponseFileList.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cbResponseFileList.AutoSize = true;
            this.cbResponseFileList.Location = new System.Drawing.Point(888, 6);
            this.cbResponseFileList.Margin = new System.Windows.Forms.Padding(4);
            this.cbResponseFileList.Name = "cbResponseFileList";
            this.cbResponseFileList.Size = new System.Drawing.Size(104, 17);
            this.cbResponseFileList.TabIndex = 0;
            this.cbResponseFileList.Text = "View Raw File(s)";
            this.cbResponseFileList.UseVisualStyleBackColor = true;
            // 
            // queryActivityPriorityTextbox
            // 
            this.queryActivityPriorityTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.queryActivityPriorityTextbox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.Priority", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.queryActivityPriorityTextbox.Location = new System.Drawing.Point(123, 27);
            this.queryActivityPriorityTextbox.Name = "queryActivityPriorityTextbox";
            this.queryActivityPriorityTextbox.ReadOnly = true;
            this.queryActivityPriorityTextbox.Size = new System.Drawing.Size(202, 20);
            this.queryActivityPriorityTextbox.TabIndex = 2;
            // 
            // queryNameTextBox
            // 
            this.queryNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpHeaderDetails.SetColumnSpan(this.queryNameTextBox, 3);
            this.queryNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.Name", true));
            this.queryNameTextBox.Location = new System.Drawing.Point(123, 3);
            this.queryNameTextBox.Name = "queryNameTextBox";
            this.queryNameTextBox.ReadOnly = true;
            this.queryNameTextBox.Size = new System.Drawing.Size(510, 20);
            this.queryNameTextBox.TabIndex = 0;
            // 
            // activityDueDateTextbox
            // 
            this.activityDueDateTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.activityDueDateTextbox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.DueDateNoTime", true));
            this.activityDueDateTextbox.Location = new System.Drawing.Point(431, 27);
            this.activityDueDateTextbox.Name = "activityDueDateTextbox";
            this.activityDueDateTextbox.ReadOnly = true;
            this.activityDueDateTextbox.Size = new System.Drawing.Size(202, 20);
            this.activityDueDateTextbox.TabIndex = 3;
            // 
            // queryStatusTypeIdTextBox
            // 
            this.queryStatusTypeIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.queryStatusTypeIdTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "RoutingStatus", true));
            this.queryStatusTypeIdTextBox.Location = new System.Drawing.Point(799, 27);
            this.queryStatusTypeIdTextBox.Name = "queryStatusTypeIdTextBox";
            this.queryStatusTypeIdTextBox.ReadOnly = true;
            this.queryStatusTypeIdTextBox.Size = new System.Drawing.Size(202, 20);
            this.queryStatusTypeIdTextBox.TabIndex = 4;
            // 
            // queryMSRequestIDTextBox
            // 
            this.queryMSRequestIDTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.queryMSRequestIDTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.queryMSRequestIDTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.MSRequestID", true));
            this.queryMSRequestIDTextBox.Location = new System.Drawing.Point(799, 3);
            this.queryMSRequestIDTextBox.Name = "queryMSRequestIDTextBox";
            this.queryMSRequestIDTextBox.ReadOnly = true;
            this.queryMSRequestIDTextBox.Size = new System.Drawing.Size(202, 20);
            this.queryMSRequestIDTextBox.TabIndex = 1;
            // 
            // queryMSRequestIDLabel
            // 
            this.queryMSRequestIDLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.queryMSRequestIDLabel.AutoSize = true;
            this.queryMSRequestIDLabel.Location = new System.Drawing.Point(728, 5);
            this.queryMSRequestIDLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.queryMSRequestIDLabel.Name = "queryMSRequestIDLabel";
            this.queryMSRequestIDLabel.Size = new System.Drawing.Size(64, 13);
            this.queryMSRequestIDLabel.TabIndex = 103;
            this.queryMSRequestIDLabel.Text = "Request ID:";
            this.queryMSRequestIDLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tlpHeaderDetails
            // 
            this.tlpHeaderDetails.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tlpHeaderDetails.ColumnCount = 6;
            this.tlpHeaderDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpHeaderDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpHeaderDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpHeaderDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpHeaderDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tlpHeaderDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpHeaderDetails.Controls.Add(queryNameLabel, 0, 0);
            this.tlpHeaderDetails.Controls.Add(this.queryNameTextBox, 1, 0);
            this.tlpHeaderDetails.Controls.Add(this.queryMSRequestIDLabel, 4, 0);
            this.tlpHeaderDetails.Controls.Add(this.queryMSRequestIDTextBox, 5, 0);
            this.tlpHeaderDetails.Controls.Add(queryActivityPriorityLabel, 0, 1);
            this.tlpHeaderDetails.Controls.Add(this.queryActivityPriorityTextbox, 1, 1);
            this.tlpHeaderDetails.Controls.Add(activityDueDateLabel, 2, 1);
            this.tlpHeaderDetails.Controls.Add(this.activityDueDateTextbox, 3, 1);
            this.tlpHeaderDetails.Controls.Add(queryStatusTypeIdLabel, 4, 1);
            this.tlpHeaderDetails.Controls.Add(this.queryStatusTypeIdTextBox, 5, 1);
            this.tlpHeaderDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpHeaderDetails.Location = new System.Drawing.Point(0, 3);
            this.tlpHeaderDetails.Name = "tlpHeaderDetails";
            this.tlpHeaderDetails.RowCount = 2;
            this.tlpHeaderDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpHeaderDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpHeaderDetails.Size = new System.Drawing.Size(1004, 49);
            this.tlpHeaderDetails.TabIndex = 0;
            // 
            // panelHeaderFiller
            // 
            this.panelHeaderFiller.BackColor = System.Drawing.Color.Transparent;
            this.panelHeaderFiller.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeaderFiller.Location = new System.Drawing.Point(0, 52);
            this.panelHeaderFiller.Name = "panelHeaderFiller";
            this.panelHeaderFiller.Size = new System.Drawing.Size(1004, 6);
            this.panelHeaderFiller.TabIndex = 121;
            // 
            // lblInputAttachments
            // 
            this.lblInputAttachments.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblInputAttachments.AutoSize = true;
            this.lblInputAttachments.Location = new System.Drawing.Point(3, 3);
            this.lblInputAttachments.Name = "lblInputAttachments";
            this.lblInputAttachments.Size = new System.Drawing.Size(69, 13);
            this.lblInputAttachments.TabIndex = 1;
            this.lblInputAttachments.Text = "Attachments:";
            // 
            // RequestDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CancelButton = btn_Close;
            this.ClientSize = new System.Drawing.Size(1004, 721);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panelHeaderFiller);
            this.Controls.Add(this.tlpHeaderDetails);
            this.Controls.Add(this.buttonPanel);
            this.Controls.Add(lblSubmittedByNote);
            this.Controls.Add(this.pnlFooter);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(680, 570);
            this.Name = "RequestDetailForm";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DataMart Client - Request Detail";
            this.Load += new System.EventHandler(this.RequestDetailForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.queryDataMartBindingSource)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageOverview.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabPageDescription.ResumeLayout(false);
            this.tabPageRequestDetails.ResumeLayout(false);
            this.tlpRequestDetails.ResumeLayout(false);
            this.tlpRequestDetails.PerformLayout();
            this.flowPanelRequestDetailsButtons.ResumeLayout(false);
            this.tabPageResponseDetails.ResumeLayout(false);
            this.tlpResponseDetails.ResumeLayout(false);
            this.tlpResponseDetails.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tlpHeaderDetails.ResumeLayout(false);
            this.tlpHeaderDetails.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource queryDataMartBindingSource;
        private System.Windows.Forms.OpenFileDialog FileDialog;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageOverview;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtRequestTimeLocal;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox txtRequestorCenter;
        private System.Windows.Forms.TextBox txtSourceTaskOrder;
        private System.Windows.Forms.Label lblRequesterCenter;
        private System.Windows.Forms.Label lblSourceTaskOrder;
        private System.Windows.Forms.TextBox txtTaskOrder;
        private System.Windows.Forms.TextBox queryCreatedByUserNameTextBox;
        private System.Windows.Forms.Label lblTaskOrder;
        private System.Windows.Forms.TextBox txtPurposeOfUse;
        private System.Windows.Forms.Label lblPurposeOfUse;
        private System.Windows.Forms.TextBox queryActivityPriorityTextbox;
        private System.Windows.Forms.TextBox txtNetwork;
        private System.Windows.Forms.TextBox queryNameTextBox;
        private System.Windows.Forms.TextBox activityTextbox;
        private System.Windows.Forms.TextBox txtDataMart;
        private System.Windows.Forms.TextBox txtWorkplanType;
        private System.Windows.Forms.Label lblWorkplanType;
        private System.Windows.Forms.TextBox txtActivity;
        private System.Windows.Forms.Label lblActivity;
        private System.Windows.Forms.TextBox txtSourceActivity;
        private System.Windows.Forms.Label lblSourceActivity;
        private System.Windows.Forms.TextBox txtPhiDisclosureLevel;
        private System.Windows.Forms.Label lblPhiDisclosureLevel;
        private System.Windows.Forms.TextBox activityDueDateTextbox;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox txtActivityProject;
        private System.Windows.Forms.TextBox queryIdTextBox;
        private System.Windows.Forms.Label lblSourceActivityProject;
        private System.Windows.Forms.Label lblActivityProject;
        private System.Windows.Forms.TextBox queryStatusTypeIdTextBox;
        private System.Windows.Forms.TextBox queryMSRequestIDTextBox;
        private System.Windows.Forms.Label queryMSRequestIDLabel;
        private System.Windows.Forms.TextBox txtSourceActivityProject;
        private System.Windows.Forms.TextBox requestorEmailTextBox;
        private System.Windows.Forms.Label lblReportAggregationLevel;
        private System.Windows.Forms.TextBox txtReportAggregationLevel;
        private System.Windows.Forms.TabPage tabPageRequestDetails;
        private System.Windows.Forms.TableLayoutPanel tlpRequestDetails;
        private System.Windows.Forms.CheckBox cbRequestFileList;
        private Controls.DataMartViewPanel vpRequest;
        private System.Windows.Forms.CheckBox chkGeneratePATIDList;
        private System.Windows.Forms.TabPage tabPageResponseDetails;
        private System.Windows.Forms.TableLayoutPanel tlpResponseDetails;
        public Controls.DataMartViewPanel vpResponse;
        private System.Windows.Forms.CheckBox cbResponseFileList;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TextBox txtAdditionalInstructions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnHold;
        private System.Windows.Forms.Button btnViewSQL;
        private System.Windows.Forms.Button btnRejectQuery;
        private System.Windows.Forms.Button btnClearCache;
        private System.Windows.Forms.Button btnExportResults;
        private System.Windows.Forms.Button btnUploadResults;
        private System.Windows.Forms.Button btnPostProcess;
        private System.Windows.Forms.TableLayoutPanel tlpHeaderDetails;
        private System.Windows.Forms.Button btnAddFile;
        private System.Windows.Forms.Button btnDeleteFile;
        private System.Windows.Forms.TabPage tabPageDescription;
        private System.Windows.Forms.WebBrowser descriptionBrowser;
        private System.Windows.Forms.FlowLayoutPanel flowPanelRequestDetailsButtons;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label lblStatusBarProgress;
        private Controls.DataMartViewPanel vpAttachments;
        private System.Windows.Forms.Panel panelHeaderFiller;
        private System.Windows.Forms.Label lblInputAttachments;
    }
}