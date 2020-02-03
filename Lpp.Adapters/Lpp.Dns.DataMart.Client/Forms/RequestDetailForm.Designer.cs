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

                _domainManager.Dispose();
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
            System.Windows.Forms.Label queryIdLabel;
            System.Windows.Forms.Label requestTimeLabel;
            System.Windows.Forms.Label queryCreatedByUserNameLabel;
            System.Windows.Forms.Label queryStatusTypeIdLabel;
            System.Windows.Forms.Label queryNameLabel;
            System.Windows.Forms.Label queryDescriptionLabel;
            System.Windows.Forms.Label requestorEmailLabel;
            System.Windows.Forms.Label lblNetwork;
            System.Windows.Forms.Label lblDataMart;
            System.Windows.Forms.Button btn_Close;
            System.Windows.Forms.Label lblAdditionalInstructions;
            System.Windows.Forms.Label lblSubmittedByNote;
            System.Windows.Forms.Label lblSubmittedTo;
            System.Windows.Forms.Label activityDueDateLabel;
            System.Windows.Forms.Label activityLabel;
            System.Windows.Forms.Label queryActivityPriorityLabel;
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RequestDetailForm));
            this.queryDataMartBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.queryIdTextBox = new System.Windows.Forms.TextBox();
            this.requestTimeDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.queryCreatedByUserNameTextBox = new System.Windows.Forms.TextBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnUploadResults = new System.Windows.Forms.Button();
            this.btnRejectQuery = new System.Windows.Forms.Button();
            this.queryStatusTypeIdTextBox = new System.Windows.Forms.TextBox();
            this.queryNameTextBox = new System.Windows.Forms.TextBox();
            this.requestorEmailTextBox = new System.Windows.Forms.TextBox();
            this.btnHold = new System.Windows.Forms.Button();
            this.txtNetwork = new System.Windows.Forms.TextBox();
            this.txtDataMart = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnExportResults = new System.Windows.Forms.Button();
            this.activityDueDateTextbox = new System.Windows.Forms.TextBox();
            this.activityTextbox = new System.Windows.Forms.TextBox();
            this.queryActivityPriorityTextbox = new System.Windows.Forms.TextBox();
            this.btnDeleteFile = new System.Windows.Forms.Button();
            this.btnAddFile = new System.Windows.Forms.Button();
            this.FileDialog = new System.Windows.Forms.OpenFileDialog();
            this.processRequestWorker = new System.ComponentModel.BackgroundWorker();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.lblRequest = new System.Windows.Forms.Label();
            this.lblResponse = new System.Windows.Forms.Label();
            this.cbRequestFileList = new System.Windows.Forms.CheckBox();
            this.cbResponseFileList = new System.Windows.Forms.CheckBox();
            this.btnPostProcess = new System.Windows.Forms.Button();
            this.txtPurposeOfUse = new System.Windows.Forms.TextBox();
            this.lblPurposeOfUse = new System.Windows.Forms.Label();
            this.lblPhiDisclosureLevel = new System.Windows.Forms.Label();
            this.lblTaskOrder = new System.Windows.Forms.Label();
            this.lblActivity = new System.Windows.Forms.Label();
            this.lblActivityProject = new System.Windows.Forms.Label();
            this.txtPhiDisclosureLevel = new System.Windows.Forms.TextBox();
            this.txtTaskOrder = new System.Windows.Forms.TextBox();
            this.txtActivityProject = new System.Windows.Forms.TextBox();
            this.txtSourceActivity = new System.Windows.Forms.TextBox();
            this.lblRequesterCenter = new System.Windows.Forms.Label();
            this.txtRequestorCenter = new System.Windows.Forms.TextBox();
            this.txtWorkplanType = new System.Windows.Forms.TextBox();
            this.lblWorkplanType = new System.Windows.Forms.Label();
            this.txtAdditionalInstructions = new System.Windows.Forms.TextBox();
            this.btnViewSQL = new System.Windows.Forms.Button();
            this.queryMSRequestIDTextBox = new System.Windows.Forms.TextBox();
            this.queryMSRequestIDLabel = new System.Windows.Forms.Label();
            this.txtSourceTaskOrder = new System.Windows.Forms.TextBox();
            this.txtActivity = new System.Windows.Forms.TextBox();
            this.txtSourceActivityProject = new System.Windows.Forms.TextBox();
            this.lblSourceTaskOrder = new System.Windows.Forms.Label();
            this.lblSourceActivity = new System.Windows.Forms.Label();
            this.lblSourceActivityProject = new System.Windows.Forms.Label();
            this.descriptionBrowser = new System.Windows.Forms.WebBrowser();
            this.txtReportAggregationLevel = new System.Windows.Forms.TextBox();
            this.lblReportAggregationLevel = new System.Windows.Forms.Label();
            this.browserPanel = new System.Windows.Forms.Panel();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.vpRequest = new Lpp.Dns.DataMart.Client.Controls.DataMartViewPanel();
            this.vpResponse = new Lpp.Dns.DataMart.Client.Controls.DataMartViewPanel();
            queryIdLabel = new System.Windows.Forms.Label();
            requestTimeLabel = new System.Windows.Forms.Label();
            queryCreatedByUserNameLabel = new System.Windows.Forms.Label();
            queryStatusTypeIdLabel = new System.Windows.Forms.Label();
            queryNameLabel = new System.Windows.Forms.Label();
            queryDescriptionLabel = new System.Windows.Forms.Label();
            requestorEmailLabel = new System.Windows.Forms.Label();
            lblNetwork = new System.Windows.Forms.Label();
            lblDataMart = new System.Windows.Forms.Label();
            btn_Close = new System.Windows.Forms.Button();
            lblAdditionalInstructions = new System.Windows.Forms.Label();
            lblSubmittedByNote = new System.Windows.Forms.Label();
            lblSubmittedTo = new System.Windows.Forms.Label();
            activityDueDateLabel = new System.Windows.Forms.Label();
            activityLabel = new System.Windows.Forms.Label();
            queryActivityPriorityLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.queryDataMartBindingSource)).BeginInit();
            this.browserPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // queryIdLabel
            // 
            queryIdLabel.AutoSize = true;
            queryIdLabel.Location = new System.Drawing.Point(777, 44);
            queryIdLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            queryIdLabel.Name = "queryIdLabel";
            queryIdLabel.Size = new System.Drawing.Size(84, 13);
            queryIdLabel.TabIndex = 40;
            queryIdLabel.Text = "System Number:";
            // 
            // requestTimeLabel
            // 
            requestTimeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            requestTimeLabel.AutoSize = true;
            requestTimeLabel.Location = new System.Drawing.Point(425, 104);
            requestTimeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            requestTimeLabel.Name = "requestTimeLabel";
            requestTimeLabel.Size = new System.Drawing.Size(76, 13);
            requestTimeLabel.TabIndex = 39;
            requestTimeLabel.Text = "Request Time:";
            // 
            // queryCreatedByUserNameLabel
            // 
            queryCreatedByUserNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            queryCreatedByUserNameLabel.AutoSize = true;
            queryCreatedByUserNameLabel.Location = new System.Drawing.Point(82, 104);
            queryCreatedByUserNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            queryCreatedByUserNameLabel.Name = "queryCreatedByUserNameLabel";
            queryCreatedByUserNameLabel.Size = new System.Drawing.Size(59, 13);
            queryCreatedByUserNameLabel.TabIndex = 38;
            queryCreatedByUserNameLabel.Text = "Requestor:";
            // 
            // queryStatusTypeIdLabel
            // 
            queryStatusTypeIdLabel.AutoSize = true;
            queryStatusTypeIdLabel.Location = new System.Drawing.Point(821, 74);
            queryStatusTypeIdLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            queryStatusTypeIdLabel.Name = "queryStatusTypeIdLabel";
            queryStatusTypeIdLabel.Size = new System.Drawing.Size(40, 13);
            queryStatusTypeIdLabel.TabIndex = 40;
            queryStatusTypeIdLabel.Text = "Status:";
            // 
            // queryNameLabel
            // 
            queryNameLabel.AutoSize = true;
            queryNameLabel.Location = new System.Drawing.Point(58, 44);
            queryNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            queryNameLabel.Name = "queryNameLabel";
            queryNameLabel.Size = new System.Drawing.Size(81, 13);
            queryNameLabel.TabIndex = 37;
            queryNameLabel.Text = "Request Name:";
            // 
            // queryDescriptionLabel
            // 
            queryDescriptionLabel.AutoSize = true;
            queryDescriptionLabel.Location = new System.Drawing.Point(19, 367);
            queryDescriptionLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            queryDescriptionLabel.Name = "queryDescriptionLabel";
            queryDescriptionLabel.Size = new System.Drawing.Size(63, 13);
            queryDescriptionLabel.TabIndex = 41;
            queryDescriptionLabel.Text = "Description:";
            queryDescriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // requestorEmailLabel
            // 
            requestorEmailLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            requestorEmailLabel.AutoSize = true;
            requestorEmailLabel.Location = new System.Drawing.Point(826, 104);
            requestorEmailLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            requestorEmailLabel.Name = "requestorEmailLabel";
            requestorEmailLabel.Size = new System.Drawing.Size(35, 13);
            requestorEmailLabel.TabIndex = 40;
            requestorEmailLabel.Text = "Email:";
            // 
            // lblNetwork
            // 
            lblNetwork.AutoSize = true;
            lblNetwork.Location = new System.Drawing.Point(91, 14);
            lblNetwork.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblNetwork.Name = "lblNetwork";
            lblNetwork.Size = new System.Drawing.Size(50, 13);
            lblNetwork.TabIndex = 40;
            lblNetwork.Text = "Network:";
            // 
            // lblDataMart
            // 
            lblDataMart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            lblDataMart.AutoSize = true;
            lblDataMart.Location = new System.Drawing.Point(807, 14);
            lblDataMart.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblDataMart.Name = "lblDataMart";
            lblDataMart.Size = new System.Drawing.Size(54, 13);
            lblDataMart.TabIndex = 40;
            lblDataMart.Text = "DataMart:";
            // 
            // btn_Close
            // 
            btn_Close.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            btn_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btn_Close.Location = new System.Drawing.Point(952, 614);
            btn_Close.Margin = new System.Windows.Forms.Padding(2);
            btn_Close.Name = "btn_Close";
            btn_Close.Size = new System.Drawing.Size(80, 24);
            btn_Close.TabIndex = 36;
            btn_Close.Text = "Close";
            btn_Close.UseVisualStyleBackColor = true;
            btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // lblAdditionalInstructions
            // 
            lblAdditionalInstructions.AutoSize = true;
            lblAdditionalInstructions.Location = new System.Drawing.Point(21, 280);
            lblAdditionalInstructions.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblAdditionalInstructions.Name = "lblAdditionalInstructions";
            lblAdditionalInstructions.Size = new System.Drawing.Size(113, 13);
            lblAdditionalInstructions.TabIndex = 61;
            lblAdditionalInstructions.Text = "Additional Instructions:";
            lblAdditionalInstructions.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSubmittedByNote
            // 
            lblSubmittedByNote.AutoSize = true;
            lblSubmittedByNote.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "CreatedByMessage", true));
            lblSubmittedByNote.Location = new System.Drawing.Point(90, 205);
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
            // lblSubmittedTo
            // 
            lblSubmittedTo.AutoSize = true;
            lblSubmittedTo.Location = new System.Drawing.Point(68, 254);
            lblSubmittedTo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblSubmittedTo.Name = "lblSubmittedTo";
            lblSubmittedTo.Size = new System.Drawing.Size(73, 13);
            lblSubmittedTo.TabIndex = 65;
            lblSubmittedTo.Text = "Submitted To:";
            // 
            // activityDueDateLabel
            // 
            activityDueDateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            activityDueDateLabel.AutoSize = true;
            activityDueDateLabel.Location = new System.Drawing.Point(445, 74);
            activityDueDateLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            activityDueDateLabel.Name = "activityDueDateLabel";
            activityDueDateLabel.Size = new System.Drawing.Size(56, 13);
            activityDueDateLabel.TabIndex = 72;
            activityDueDateLabel.Text = "Due Date:";
            // 
            // activityLabel
            // 
            activityLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            activityLabel.AutoSize = true;
            activityLabel.Location = new System.Drawing.Point(458, 14);
            activityLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            activityLabel.Name = "activityLabel";
            activityLabel.Size = new System.Drawing.Size(43, 13);
            activityLabel.TabIndex = 70;
            activityLabel.Text = "Project:";
            // 
            // queryActivityPriorityLabel
            // 
            queryActivityPriorityLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            queryActivityPriorityLabel.AutoSize = true;
            queryActivityPriorityLabel.Location = new System.Drawing.Point(100, 74);
            queryActivityPriorityLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            queryActivityPriorityLabel.Name = "queryActivityPriorityLabel";
            queryActivityPriorityLabel.Size = new System.Drawing.Size(41, 13);
            queryActivityPriorityLabel.TabIndex = 68;
            queryActivityPriorityLabel.Text = "Priority:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(424, 44);
            label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(77, 13);
            label1.TabIndex = 80;
            label1.Text = "Request Type:";
            // 
            // queryIdTextBox
            // 
            this.queryIdTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.Identifier", true));
            this.queryIdTextBox.Location = new System.Drawing.Point(863, 41);
            this.queryIdTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.queryIdTextBox.Name = "queryIdTextBox";
            this.queryIdTextBox.ReadOnly = true;
            this.queryIdTextBox.Size = new System.Drawing.Size(178, 20);
            this.queryIdTextBox.TabIndex = 5;
            // 
            // requestTimeDateTimePicker
            // 
            this.requestTimeDateTimePicker.CustomFormat = "MM/dd/yyyy hh:mm tt";
            this.requestTimeDateTimePicker.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.queryDataMartBindingSource, "Source.CreatedOnLocal", true));
            this.requestTimeDateTimePicker.Enabled = false;
            this.requestTimeDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.requestTimeDateTimePicker.Location = new System.Drawing.Point(503, 101);
            this.requestTimeDateTimePicker.Margin = new System.Windows.Forms.Padding(2);
            this.requestTimeDateTimePicker.Name = "requestTimeDateTimePicker";
            this.requestTimeDateTimePicker.Size = new System.Drawing.Size(178, 20);
            this.requestTimeDateTimePicker.TabIndex = 10;
            // 
            // queryCreatedByUserNameTextBox
            // 
            this.queryCreatedByUserNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.Author.Username", true));
            this.queryCreatedByUserNameTextBox.Location = new System.Drawing.Point(143, 101);
            this.queryCreatedByUserNameTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.queryCreatedByUserNameTextBox.Name = "queryCreatedByUserNameTextBox";
            this.queryCreatedByUserNameTextBox.ReadOnly = true;
            this.queryCreatedByUserNameTextBox.Size = new System.Drawing.Size(178, 20);
            this.queryCreatedByUserNameTextBox.TabIndex = 9;
            // 
            // btnRun
            // 
            this.btnRun.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRun.Location = new System.Drawing.Point(71, 614);
            this.btnRun.Margin = new System.Windows.Forms.Padding(2);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(80, 24);
            this.btnRun.TabIndex = 27;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnUploadResults
            // 
            this.btnUploadResults.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnUploadResults.Location = new System.Drawing.Point(842, 614);
            this.btnUploadResults.Margin = new System.Windows.Forms.Padding(2);
            this.btnUploadResults.Name = "btnUploadResults";
            this.btnUploadResults.Size = new System.Drawing.Size(100, 24);
            this.btnUploadResults.TabIndex = 35;
            this.btnUploadResults.Text = "Upload Results";
            this.btnUploadResults.UseVisualStyleBackColor = true;
            this.btnUploadResults.Click += new System.EventHandler(this.btnUploadResults_Click);
            // 
            // btnRejectQuery
            // 
            this.btnRejectQuery.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRejectQuery.Location = new System.Drawing.Point(251, 614);
            this.btnRejectQuery.Margin = new System.Windows.Forms.Padding(2);
            this.btnRejectQuery.Name = "btnRejectQuery";
            this.btnRejectQuery.Size = new System.Drawing.Size(80, 24);
            this.btnRejectQuery.TabIndex = 29;
            this.btnRejectQuery.Text = "Reject";
            this.btnRejectQuery.UseVisualStyleBackColor = true;
            this.btnRejectQuery.Click += new System.EventHandler(this.btnRejectQuery_Click);
            // 
            // queryStatusTypeIdTextBox
            // 
            this.queryStatusTypeIdTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "RoutingStatus", true));
            this.queryStatusTypeIdTextBox.Location = new System.Drawing.Point(863, 71);
            this.queryStatusTypeIdTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.queryStatusTypeIdTextBox.Name = "queryStatusTypeIdTextBox";
            this.queryStatusTypeIdTextBox.ReadOnly = true;
            this.queryStatusTypeIdTextBox.Size = new System.Drawing.Size(178, 20);
            this.queryStatusTypeIdTextBox.TabIndex = 8;
            // 
            // queryNameTextBox
            // 
            this.queryNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.Name", true));
            this.queryNameTextBox.Location = new System.Drawing.Point(143, 41);
            this.queryNameTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.queryNameTextBox.Name = "queryNameTextBox";
            this.queryNameTextBox.ReadOnly = true;
            this.queryNameTextBox.Size = new System.Drawing.Size(178, 20);
            this.queryNameTextBox.TabIndex = 3;
            // 
            // requestorEmailTextBox
            // 
            this.requestorEmailTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.Author.Email", true));
            this.requestorEmailTextBox.Location = new System.Drawing.Point(863, 101);
            this.requestorEmailTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.requestorEmailTextBox.Name = "requestorEmailTextBox";
            this.requestorEmailTextBox.ReadOnly = true;
            this.requestorEmailTextBox.Size = new System.Drawing.Size(178, 20);
            this.requestorEmailTextBox.TabIndex = 11;
            // 
            // btnHold
            // 
            this.btnHold.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnHold.Location = new System.Drawing.Point(161, 614);
            this.btnHold.Margin = new System.Windows.Forms.Padding(2);
            this.btnHold.Name = "btnHold";
            this.btnHold.Size = new System.Drawing.Size(80, 24);
            this.btnHold.TabIndex = 28;
            this.btnHold.Text = "Hold";
            this.btnHold.UseVisualStyleBackColor = true;
            this.btnHold.Click += new System.EventHandler(this.btnHold_Click);
            // 
            // txtNetwork
            // 
            this.txtNetwork.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "NetworkName", true));
            this.txtNetwork.Location = new System.Drawing.Point(143, 11);
            this.txtNetwork.Margin = new System.Windows.Forms.Padding(2);
            this.txtNetwork.Name = "txtNetwork";
            this.txtNetwork.ReadOnly = true;
            this.txtNetwork.Size = new System.Drawing.Size(178, 20);
            this.txtNetwork.TabIndex = 0;
            // 
            // txtDataMart
            // 
            this.txtDataMart.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "DataMartName", true));
            this.txtDataMart.Location = new System.Drawing.Point(863, 11);
            this.txtDataMart.Margin = new System.Windows.Forms.Padding(2);
            this.txtDataMart.Name = "txtDataMart";
            this.txtDataMart.ReadOnly = true;
            this.txtDataMart.Size = new System.Drawing.Size(178, 20);
            this.txtDataMart.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "SubmittedDataMarts", true));
            this.textBox1.Location = new System.Drawing.Point(143, 251);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(927, 20);
            this.textBox1.TabIndex = 20;
            // 
            // btnExportResults
            // 
            this.btnExportResults.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnExportResults.Location = new System.Drawing.Point(731, 614);
            this.btnExportResults.Margin = new System.Windows.Forms.Padding(2);
            this.btnExportResults.Name = "btnExportResults";
            this.btnExportResults.Size = new System.Drawing.Size(100, 24);
            this.btnExportResults.TabIndex = 34;
            this.btnExportResults.Text = "Export Results..";
            this.btnExportResults.UseVisualStyleBackColor = true;
            this.btnExportResults.Click += new System.EventHandler(this.btnExportResults_Click);
            // 
            // activityDueDateTextbox
            // 
            this.activityDueDateTextbox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.DueDateNoTime", true));
            this.activityDueDateTextbox.Location = new System.Drawing.Point(503, 71);
            this.activityDueDateTextbox.Margin = new System.Windows.Forms.Padding(2);
            this.activityDueDateTextbox.Name = "activityDueDateTextbox";
            this.activityDueDateTextbox.ReadOnly = true;
            this.activityDueDateTextbox.Size = new System.Drawing.Size(178, 20);
            this.activityDueDateTextbox.TabIndex = 7;
            // 
            // activityTextbox
            // 
            this.activityTextbox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "ProjectName", true));
            this.activityTextbox.Location = new System.Drawing.Point(503, 11);
            this.activityTextbox.Margin = new System.Windows.Forms.Padding(2);
            this.activityTextbox.Name = "activityTextbox";
            this.activityTextbox.ReadOnly = true;
            this.activityTextbox.Size = new System.Drawing.Size(178, 20);
            this.activityTextbox.TabIndex = 1;
            // 
            // queryActivityPriorityTextbox
            // 
            this.queryActivityPriorityTextbox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.Priority", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.queryActivityPriorityTextbox.Location = new System.Drawing.Point(143, 71);
            this.queryActivityPriorityTextbox.Margin = new System.Windows.Forms.Padding(2);
            this.queryActivityPriorityTextbox.Name = "queryActivityPriorityTextbox";
            this.queryActivityPriorityTextbox.ReadOnly = true;
            this.queryActivityPriorityTextbox.Size = new System.Drawing.Size(178, 20);
            this.queryActivityPriorityTextbox.TabIndex = 6;
            // 
            // btnDeleteFile
            // 
            this.btnDeleteFile.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDeleteFile.Location = new System.Drawing.Point(521, 614);
            this.btnDeleteFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeleteFile.Name = "btnDeleteFile";
            this.btnDeleteFile.Size = new System.Drawing.Size(80, 24);
            this.btnDeleteFile.TabIndex = 32;
            this.btnDeleteFile.Text = "Delete File";
            this.btnDeleteFile.UseVisualStyleBackColor = true;
            this.btnDeleteFile.Click += new System.EventHandler(this.btnDeleteFile_Click);
            // 
            // btnAddFile
            // 
            this.btnAddFile.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAddFile.Location = new System.Drawing.Point(431, 614);
            this.btnAddFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddFile.Name = "btnAddFile";
            this.btnAddFile.Size = new System.Drawing.Size(80, 24);
            this.btnAddFile.TabIndex = 31;
            this.btnAddFile.Text = "Add File";
            this.btnAddFile.UseVisualStyleBackColor = true;
            this.btnAddFile.Click += new System.EventHandler(this.btnAddFile_Click);
            // 
            // FileDialog
            // 
            this.FileDialog.FileName = "openFileDialog1";
            // 
            // processRequestWorker
            // 
            this.processRequestWorker.WorkerReportsProgress = true;
            this.processRequestWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ProcessRequest);
            this.processRequestWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.processRequestWorker_ProgressChanged);
            this.processRequestWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.processRequestWorker_RunWorkerCompleted);
            // 
            // textBox2
            // 
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.RequestTypeName", true));
            this.textBox2.Location = new System.Drawing.Point(503, 41);
            this.textBox2.Margin = new System.Windows.Forms.Padding(2);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(178, 20);
            this.textBox2.TabIndex = 4;
            // 
            // lblRequest
            // 
            this.lblRequest.AutoSize = true;
            this.lblRequest.Location = new System.Drawing.Point(17, 454);
            this.lblRequest.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRequest.Name = "lblRequest";
            this.lblRequest.Size = new System.Drawing.Size(50, 13);
            this.lblRequest.TabIndex = 82;
            this.lblRequest.Tag = "";
            this.lblRequest.Text = "Request:";
            // 
            // lblResponse
            // 
            this.lblResponse.AutoSize = true;
            this.lblResponse.Location = new System.Drawing.Point(18, 685);
            this.lblResponse.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblResponse.Name = "lblResponse";
            this.lblResponse.Size = new System.Drawing.Size(58, 13);
            this.lblResponse.TabIndex = 83;
            this.lblResponse.Tag = "Request";
            this.lblResponse.Text = "Response:";
            // 
            // cbRequestFileList
            // 
            this.cbRequestFileList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRequestFileList.AutoSize = true;
            this.cbRequestFileList.Location = new System.Drawing.Point(964, 457);
            this.cbRequestFileList.Margin = new System.Windows.Forms.Padding(4);
            this.cbRequestFileList.Name = "cbRequestFileList";
            this.cbRequestFileList.Size = new System.Drawing.Size(104, 17);
            this.cbRequestFileList.TabIndex = 24;
            this.cbRequestFileList.Text = "View Raw File(s)";
            this.cbRequestFileList.UseVisualStyleBackColor = true;
            // 
            // cbResponseFileList
            // 
            this.cbResponseFileList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbResponseFileList.AutoSize = true;
            this.cbResponseFileList.Location = new System.Drawing.Point(956, 686);
            this.cbResponseFileList.Margin = new System.Windows.Forms.Padding(4);
            this.cbResponseFileList.Name = "cbResponseFileList";
            this.cbResponseFileList.Size = new System.Drawing.Size(104, 17);
            this.cbResponseFileList.TabIndex = 26;
            this.cbResponseFileList.Text = "View Raw File(s)";
            this.cbResponseFileList.UseVisualStyleBackColor = true;
            // 
            // btnPostProcess
            // 
            this.btnPostProcess.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnPostProcess.Enabled = false;
            this.btnPostProcess.Location = new System.Drawing.Point(612, 614);
            this.btnPostProcess.Margin = new System.Windows.Forms.Padding(4);
            this.btnPostProcess.Name = "btnPostProcess";
            this.btnPostProcess.Size = new System.Drawing.Size(110, 24);
            this.btnPostProcess.TabIndex = 33;
            this.btnPostProcess.Text = "Suppress Low Cells";
            this.btnPostProcess.UseVisualStyleBackColor = true;
            this.btnPostProcess.Click += new System.EventHandler(this.btnPostProcess_Click);
            // 
            // txtPurposeOfUse
            // 
            this.txtPurposeOfUse.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.PurposeOfUse", true));
            this.txtPurposeOfUse.Location = new System.Drawing.Point(143, 131);
            this.txtPurposeOfUse.Margin = new System.Windows.Forms.Padding(4);
            this.txtPurposeOfUse.Name = "txtPurposeOfUse";
            this.txtPurposeOfUse.ReadOnly = true;
            this.txtPurposeOfUse.Size = new System.Drawing.Size(178, 20);
            this.txtPurposeOfUse.TabIndex = 12;
            // 
            // lblPurposeOfUse
            // 
            this.lblPurposeOfUse.AutoSize = true;
            this.lblPurposeOfUse.Location = new System.Drawing.Point(58, 134);
            this.lblPurposeOfUse.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPurposeOfUse.Name = "lblPurposeOfUse";
            this.lblPurposeOfUse.Size = new System.Drawing.Size(83, 13);
            this.lblPurposeOfUse.TabIndex = 88;
            this.lblPurposeOfUse.Text = "Purpose of Use:";
            // 
            // lblPhiDisclosureLevel
            // 
            this.lblPhiDisclosureLevel.AutoSize = true;
            this.lblPhiDisclosureLevel.Location = new System.Drawing.Point(432, 134);
            this.lblPhiDisclosureLevel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPhiDisclosureLevel.Name = "lblPhiDisclosureLevel";
            this.lblPhiDisclosureLevel.Size = new System.Drawing.Size(69, 13);
            this.lblPhiDisclosureLevel.TabIndex = 89;
            this.lblPhiDisclosureLevel.Text = "Level of PHI:";
            // 
            // lblTaskOrder
            // 
            this.lblTaskOrder.AutoSize = true;
            this.lblTaskOrder.Location = new System.Drawing.Point(41, 194);
            this.lblTaskOrder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTaskOrder.Name = "lblTaskOrder";
            this.lblTaskOrder.Size = new System.Drawing.Size(100, 13);
            this.lblTaskOrder.TabIndex = 90;
            this.lblTaskOrder.Text = "Budget Task Order:";
            // 
            // lblActivity
            // 
            this.lblActivity.AutoSize = true;
            this.lblActivity.Location = new System.Drawing.Point(420, 194);
            this.lblActivity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblActivity.Name = "lblActivity";
            this.lblActivity.Size = new System.Drawing.Size(81, 13);
            this.lblActivity.TabIndex = 91;
            this.lblActivity.Text = "Budget Activity:";
            // 
            // lblActivityProject
            // 
            this.lblActivityProject.AutoSize = true;
            this.lblActivityProject.Location = new System.Drawing.Point(744, 194);
            this.lblActivityProject.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblActivityProject.Name = "lblActivityProject";
            this.lblActivityProject.Size = new System.Drawing.Size(117, 13);
            this.lblActivityProject.TabIndex = 92;
            this.lblActivityProject.Text = "Budget Activity Project:";
            // 
            // txtPhiDisclosureLevel
            // 
            this.txtPhiDisclosureLevel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.PhiDisclosureLevel", true));
            this.txtPhiDisclosureLevel.Location = new System.Drawing.Point(503, 131);
            this.txtPhiDisclosureLevel.Margin = new System.Windows.Forms.Padding(4);
            this.txtPhiDisclosureLevel.Name = "txtPhiDisclosureLevel";
            this.txtPhiDisclosureLevel.ReadOnly = true;
            this.txtPhiDisclosureLevel.Size = new System.Drawing.Size(178, 20);
            this.txtPhiDisclosureLevel.TabIndex = 13;
            // 
            // txtTaskOrder
            // 
            this.txtTaskOrder.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.TaskOrder", true));
            this.txtTaskOrder.Location = new System.Drawing.Point(143, 191);
            this.txtTaskOrder.Margin = new System.Windows.Forms.Padding(4);
            this.txtTaskOrder.Name = "txtTaskOrder";
            this.txtTaskOrder.ReadOnly = true;
            this.txtTaskOrder.Size = new System.Drawing.Size(178, 20);
            this.txtTaskOrder.TabIndex = 15;
            // 
            // txtActivityProject
            // 
            this.txtActivityProject.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.ActivityProject", true));
            this.txtActivityProject.Location = new System.Drawing.Point(863, 191);
            this.txtActivityProject.Margin = new System.Windows.Forms.Padding(4);
            this.txtActivityProject.Name = "txtActivityProject";
            this.txtActivityProject.ReadOnly = true;
            this.txtActivityProject.Size = new System.Drawing.Size(178, 20);
            this.txtActivityProject.TabIndex = 17;
            // 
            // txtSourceActivity
            // 
            this.txtSourceActivity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.SourceActivity", true));
            this.txtSourceActivity.Location = new System.Drawing.Point(503, 161);
            this.txtSourceActivity.Name = "txtSourceActivity";
            this.txtSourceActivity.ReadOnly = true;
            this.txtSourceActivity.Size = new System.Drawing.Size(178, 20);
            this.txtSourceActivity.TabIndex = 16;
            // 
            // lblRequesterCenter
            // 
            this.lblRequesterCenter.AutoSize = true;
            this.lblRequesterCenter.Location = new System.Drawing.Point(48, 224);
            this.lblRequesterCenter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRequesterCenter.Name = "lblRequesterCenter";
            this.lblRequesterCenter.Size = new System.Drawing.Size(93, 13);
            this.lblRequesterCenter.TabIndex = 97;
            this.lblRequesterCenter.Text = "Requester Center:";
            // 
            // txtRequestorCenter
            // 
            this.txtRequestorCenter.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.RequestorCenter", true));
            this.txtRequestorCenter.Location = new System.Drawing.Point(143, 221);
            this.txtRequestorCenter.Margin = new System.Windows.Forms.Padding(4);
            this.txtRequestorCenter.Name = "txtRequestorCenter";
            this.txtRequestorCenter.ReadOnly = true;
            this.txtRequestorCenter.Size = new System.Drawing.Size(178, 20);
            this.txtRequestorCenter.TabIndex = 18;
            // 
            // txtWorkplanType
            // 
            this.txtWorkplanType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.WorkplanType", true));
            this.txtWorkplanType.Location = new System.Drawing.Point(503, 221);
            this.txtWorkplanType.Margin = new System.Windows.Forms.Padding(4);
            this.txtWorkplanType.Name = "txtWorkplanType";
            this.txtWorkplanType.ReadOnly = true;
            this.txtWorkplanType.Size = new System.Drawing.Size(178, 20);
            this.txtWorkplanType.TabIndex = 19;
            // 
            // lblWorkplanType
            // 
            this.lblWorkplanType.AutoSize = true;
            this.lblWorkplanType.Location = new System.Drawing.Point(418, 224);
            this.lblWorkplanType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWorkplanType.Name = "lblWorkplanType";
            this.lblWorkplanType.Size = new System.Drawing.Size(83, 13);
            this.lblWorkplanType.TabIndex = 100;
            this.lblWorkplanType.Text = "Workplan Type:";
            // 
            // txtAdditionalInstructions
            // 
            this.txtAdditionalInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAdditionalInstructions.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.AdditionalInstructions", true));
            this.txtAdditionalInstructions.Location = new System.Drawing.Point(12, 299);
            this.txtAdditionalInstructions.Margin = new System.Windows.Forms.Padding(2);
            this.txtAdditionalInstructions.Multiline = true;
            this.txtAdditionalInstructions.Name = "txtAdditionalInstructions";
            this.txtAdditionalInstructions.ReadOnly = true;
            this.txtAdditionalInstructions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAdditionalInstructions.Size = new System.Drawing.Size(1058, 60);
            this.txtAdditionalInstructions.TabIndex = 22;
            // 
            // btnViewSQL
            // 
            this.btnViewSQL.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnViewSQL.Location = new System.Drawing.Point(341, 614);
            this.btnViewSQL.Margin = new System.Windows.Forms.Padding(2);
            this.btnViewSQL.Name = "btnViewSQL";
            this.btnViewSQL.Size = new System.Drawing.Size(80, 24);
            this.btnViewSQL.TabIndex = 30;
            this.btnViewSQL.Text = "View SQL";
            this.btnViewSQL.UseVisualStyleBackColor = true;
            this.btnViewSQL.Click += new System.EventHandler(this.btnViewSQL_Click);
            // 
            // queryMSRequestIDTextBox
            // 
            this.queryMSRequestIDTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.queryMSRequestIDTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.MSRequestID", true));
            this.queryMSRequestIDTextBox.Location = new System.Drawing.Point(863, 221);
            this.queryMSRequestIDTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.queryMSRequestIDTextBox.Name = "queryMSRequestIDTextBox";
            this.queryMSRequestIDTextBox.ReadOnly = true;
            this.queryMSRequestIDTextBox.Size = new System.Drawing.Size(178, 20);
            this.queryMSRequestIDTextBox.TabIndex = 14;
            // 
            // queryMSRequestIDLabel
            // 
            this.queryMSRequestIDLabel.AutoSize = true;
            this.queryMSRequestIDLabel.Location = new System.Drawing.Point(797, 224);
            this.queryMSRequestIDLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.queryMSRequestIDLabel.Name = "queryMSRequestIDLabel";
            this.queryMSRequestIDLabel.Size = new System.Drawing.Size(64, 13);
            this.queryMSRequestIDLabel.TabIndex = 103;
            this.queryMSRequestIDLabel.Text = "Request ID:";
            // 
            // txtSourceTaskOrder
            // 
            this.txtSourceTaskOrder.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.SourceTaskOrder", true));
            this.txtSourceTaskOrder.Location = new System.Drawing.Point(143, 161);
            this.txtSourceTaskOrder.Margin = new System.Windows.Forms.Padding(4);
            this.txtSourceTaskOrder.Name = "txtSourceTaskOrder";
            this.txtSourceTaskOrder.ReadOnly = true;
            this.txtSourceTaskOrder.Size = new System.Drawing.Size(178, 20);
            this.txtSourceTaskOrder.TabIndex = 104;
            // 
            // txtActivity
            // 
            this.txtActivity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.Activity", true));
            this.txtActivity.Location = new System.Drawing.Point(503, 191);
            this.txtActivity.Margin = new System.Windows.Forms.Padding(4);
            this.txtActivity.Name = "txtActivity";
            this.txtActivity.ReadOnly = true;
            this.txtActivity.Size = new System.Drawing.Size(178, 20);
            this.txtActivity.TabIndex = 105;
            // 
            // txtSourceActivityProject
            // 
            this.txtSourceActivityProject.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.SourceActivityProject", true));
            this.txtSourceActivityProject.Location = new System.Drawing.Point(863, 161);
            this.txtSourceActivityProject.Margin = new System.Windows.Forms.Padding(4);
            this.txtSourceActivityProject.Name = "txtSourceActivityProject";
            this.txtSourceActivityProject.ReadOnly = true;
            this.txtSourceActivityProject.Size = new System.Drawing.Size(178, 20);
            this.txtSourceActivityProject.TabIndex = 106;
            // 
            // lblSourceTaskOrder
            // 
            this.lblSourceTaskOrder.AutoSize = true;
            this.lblSourceTaskOrder.Location = new System.Drawing.Point(41, 164);
            this.lblSourceTaskOrder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSourceTaskOrder.Name = "lblSourceTaskOrder";
            this.lblSourceTaskOrder.Size = new System.Drawing.Size(100, 13);
            this.lblSourceTaskOrder.TabIndex = 107;
            this.lblSourceTaskOrder.Text = "Source Task Order:";
            // 
            // lblSourceActivity
            // 
            this.lblSourceActivity.AutoSize = true;
            this.lblSourceActivity.Location = new System.Drawing.Point(420, 164);
            this.lblSourceActivity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSourceActivity.Name = "lblSourceActivity";
            this.lblSourceActivity.Size = new System.Drawing.Size(81, 13);
            this.lblSourceActivity.TabIndex = 108;
            this.lblSourceActivity.Text = "Source Activity:";
            // 
            // lblSourceActivityProject
            // 
            this.lblSourceActivityProject.AutoSize = true;
            this.lblSourceActivityProject.Location = new System.Drawing.Point(744, 164);
            this.lblSourceActivityProject.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSourceActivityProject.Name = "lblSourceActivityProject";
            this.lblSourceActivityProject.Size = new System.Drawing.Size(117, 13);
            this.lblSourceActivityProject.TabIndex = 109;
            this.lblSourceActivityProject.Text = "Source Activity Project:";
            // 
            // descriptionBrowser
            // 
            this.descriptionBrowser.AllowNavigation = false;
            this.descriptionBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionBrowser.IsWebBrowserContextMenuEnabled = false;
            this.descriptionBrowser.Location = new System.Drawing.Point(0, 0);
            this.descriptionBrowser.Margin = new System.Windows.Forms.Padding(4);
            this.descriptionBrowser.MinimumSize = new System.Drawing.Size(25, 25);
            this.descriptionBrowser.Name = "descriptionBrowser";
            this.descriptionBrowser.Size = new System.Drawing.Size(1053, 61);
            this.descriptionBrowser.TabIndex = 115;
            this.descriptionBrowser.WebBrowserShortcutsEnabled = false;
            // 
            // txtReportAggregationLevel
            // 
            this.txtReportAggregationLevel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.ReportAggregationLevel", true));
            this.txtReportAggregationLevel.Location = new System.Drawing.Point(863, 131);
            this.txtReportAggregationLevel.Margin = new System.Windows.Forms.Padding(4);
            this.txtReportAggregationLevel.Name = "txtReportAggregationLevel";
            this.txtReportAggregationLevel.ReadOnly = true;
            this.txtReportAggregationLevel.Size = new System.Drawing.Size(178, 20);
            this.txtReportAggregationLevel.TabIndex = 111;
            // 
            // lblReportAggregationLevel
            // 
            this.lblReportAggregationLevel.AutoSize = true;
            this.lblReportAggregationLevel.Location = new System.Drawing.Point(779, 128);
            this.lblReportAggregationLevel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReportAggregationLevel.Name = "lblReportAggregationLevel";
            this.lblReportAggregationLevel.Size = new System.Drawing.Size(82, 26);
            this.lblReportAggregationLevel.TabIndex = 112;
            this.lblReportAggregationLevel.Text = "Level of Report\r\n     Aggregation:";
            // 
            // browserPanel
            // 
            this.browserPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.browserPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.browserPanel.Controls.Add(this.descriptionBrowser);
            this.browserPanel.Location = new System.Drawing.Point(12, 384);
            this.browserPanel.Margin = new System.Windows.Forms.Padding(4);
            this.browserPanel.Name = "browserPanel";
            this.browserPanel.Size = new System.Drawing.Size(1056, 63);
            this.browserPanel.TabIndex = 113;
            // 
            // buttonPanel
            // 
            this.buttonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPanel.AutoScroll = true;
            this.buttonPanel.AutoScrollMinSize = new System.Drawing.Size(1200, 0);
            this.buttonPanel.Location = new System.Drawing.Point(0, 1126);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(1036, 67);
            this.buttonPanel.TabIndex = 116;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.txtNetwork);
            this.panel1.Controls.Add(queryActivityPriorityLabel);
            this.panel1.Controls.Add(this.queryNameTextBox);
            this.panel1.Controls.Add(this.queryActivityPriorityTextbox);
            this.panel1.Controls.Add(queryCreatedByUserNameLabel);
            this.panel1.Controls.Add(this.queryCreatedByUserNameTextBox);
            this.panel1.Controls.Add(lblNetwork);
            this.panel1.Controls.Add(queryNameLabel);
            this.panel1.Controls.Add(this.lblSourceTaskOrder);
            this.panel1.Controls.Add(this.txtSourceTaskOrder);
            this.panel1.Controls.Add(this.lblPurposeOfUse);
            this.panel1.Controls.Add(this.txtPurposeOfUse);
            this.panel1.Controls.Add(this.txtRequestorCenter);
            this.panel1.Controls.Add(this.lblRequesterCenter);
            this.panel1.Controls.Add(this.txtTaskOrder);
            this.panel1.Controls.Add(this.lblTaskOrder);
            this.panel1.Controls.Add(label1);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(activityLabel);
            this.panel1.Controls.Add(this.activityTextbox);
            this.panel1.Controls.Add(activityDueDateLabel);
            this.panel1.Controls.Add(this.activityDueDateTextbox);
            this.panel1.Controls.Add(requestTimeLabel);
            this.panel1.Controls.Add(this.requestTimeDateTimePicker);
            this.panel1.Controls.Add(this.txtSourceActivity);
            this.panel1.Controls.Add(this.txtActivityProject);
            this.panel1.Controls.Add(this.txtPhiDisclosureLevel);
            this.panel1.Controls.Add(this.lblPhiDisclosureLevel);
            this.panel1.Controls.Add(this.lblSourceActivityProject);
            this.panel1.Controls.Add(this.lblSourceActivity);
            this.panel1.Controls.Add(this.lblActivityProject);
            this.panel1.Controls.Add(this.lblActivity);
            this.panel1.Controls.Add(this.txtSourceActivityProject);
            this.panel1.Controls.Add(this.txtActivity);
            this.panel1.Controls.Add(this.lblWorkplanType);
            this.panel1.Controls.Add(this.txtWorkplanType);
            this.panel1.Controls.Add(lblDataMart);
            this.panel1.Controls.Add(this.txtDataMart);
            this.panel1.Controls.Add(queryIdLabel);
            this.panel1.Controls.Add(this.queryIdTextBox);
            this.panel1.Controls.Add(requestorEmailLabel);
            this.panel1.Controls.Add(this.requestorEmailTextBox);
            this.panel1.Controls.Add(queryStatusTypeIdLabel);
            this.panel1.Controls.Add(this.queryStatusTypeIdTextBox);
            this.panel1.Controls.Add(this.lblReportAggregationLevel);
            this.panel1.Controls.Add(this.txtReportAggregationLevel);
            this.panel1.Controls.Add(this.queryMSRequestIDTextBox);
            this.panel1.Controls.Add(this.queryMSRequestIDLabel);
            this.panel1.Controls.Add(lblSubmittedTo);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(lblAdditionalInstructions);
            this.panel1.Controls.Add(this.txtAdditionalInstructions);
            this.panel1.Controls.Add(queryDescriptionLabel);
            this.panel1.Controls.Add(this.lblRequest);
            this.panel1.Controls.Add(this.cbRequestFileList);
            this.panel1.Controls.Add(this.vpRequest);
            this.panel1.Controls.Add(this.browserPanel);
            this.panel1.Controls.Add(this.lblResponse);
            this.panel1.Controls.Add(this.cbResponseFileList);
            this.panel1.Controls.Add(this.vpResponse);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1104, 602);
            this.panel1.TabIndex = 117;
            // 
            // vpRequest
            // 
            this.vpRequest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vpRequest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.vpRequest.LastView = null;
            this.vpRequest.Location = new System.Drawing.Point(11, 476);
            this.vpRequest.Margin = new System.Windows.Forms.Padding(4);
            this.vpRequest.Name = "vpRequest";
            this.vpRequest.ShowView = Lpp.Dns.DataMart.Client.Controls.ViewPanel.DisplayType.PLAIN;
            this.vpRequest.Size = new System.Drawing.Size(1056, 198);
            this.vpRequest.TabIndex = 23;
            this.vpRequest.ViewText = "";
            // 
            // vpResponse
            // 
            this.vpResponse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vpResponse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.vpResponse.LastView = null;
            this.vpResponse.Location = new System.Drawing.Point(11, 705);
            this.vpResponse.Margin = new System.Windows.Forms.Padding(4);
            this.vpResponse.Name = "vpResponse";
            this.vpResponse.ShowView = Lpp.Dns.DataMart.Client.Controls.ViewPanel.DisplayType.PLAIN;
            this.vpResponse.Size = new System.Drawing.Size(1050, 241);
            this.vpResponse.TabIndex = 25;
            this.vpResponse.ViewText = "";
            // 
            // RequestDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CancelButton = btn_Close;
            this.ClientSize = new System.Drawing.Size(1104, 647);
            this.Controls.Add(btn_Close);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnUploadResults);
            this.Controls.Add(this.btnViewSQL);
            this.Controls.Add(this.btnRejectQuery);
            this.Controls.Add(this.btnPostProcess);
            this.Controls.Add(this.btnHold);
            this.Controls.Add(this.btnDeleteFile);
            this.Controls.Add(this.btnExportResults);
            this.Controls.Add(this.btnAddFile);
            this.Controls.Add(this.buttonPanel);
            this.Controls.Add(lblSubmittedByNote);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1020, 398);
            this.Name = "RequestDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DataMart Client - Request Detail";
            this.Load += new System.EventHandler(this.RequestDetailForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.queryDataMartBindingSource)).EndInit();
            this.browserPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource queryDataMartBindingSource;
        private System.Windows.Forms.TextBox queryIdTextBox;
        private System.Windows.Forms.DateTimePicker requestTimeDateTimePicker;
        private System.Windows.Forms.TextBox queryCreatedByUserNameTextBox;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnUploadResults;
        private System.Windows.Forms.Button btnRejectQuery;
        private System.Windows.Forms.TextBox queryStatusTypeIdTextBox;
        private System.Windows.Forms.TextBox queryNameTextBox;
        private System.Windows.Forms.TextBox requestorEmailTextBox;
        private System.Windows.Forms.Button btnHold;
        private System.Windows.Forms.TextBox txtNetwork;
        private System.Windows.Forms.TextBox txtDataMart;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnExportResults;
        private System.Windows.Forms.TextBox activityDueDateTextbox;
        private System.Windows.Forms.TextBox activityTextbox;
        private System.Windows.Forms.TextBox queryActivityPriorityTextbox;
        private Controls.DataMartViewPanel vpRequest;
        public Controls.DataMartViewPanel vpResponse;
        private System.Windows.Forms.Button btnDeleteFile;
        private System.Windows.Forms.Button btnAddFile;
        private System.Windows.Forms.OpenFileDialog FileDialog;
        private System.ComponentModel.BackgroundWorker processRequestWorker;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label lblRequest;
        private System.Windows.Forms.Label lblResponse;
        private System.Windows.Forms.CheckBox cbRequestFileList;
        private System.Windows.Forms.CheckBox cbResponseFileList;
        private System.Windows.Forms.Button btnPostProcess;
        private System.Windows.Forms.TextBox txtPurposeOfUse;
        private System.Windows.Forms.Label lblPurposeOfUse;
        private System.Windows.Forms.Label lblPhiDisclosureLevel;
        private System.Windows.Forms.Label lblTaskOrder;
        private System.Windows.Forms.Label lblActivity;
        private System.Windows.Forms.Label lblActivityProject;
        private System.Windows.Forms.TextBox txtPhiDisclosureLevel;
        private System.Windows.Forms.TextBox txtTaskOrder;
        private System.Windows.Forms.TextBox txtActivityProject;
        private System.Windows.Forms.TextBox txtSourceActivity;
        private System.Windows.Forms.Label lblRequesterCenter;
        private System.Windows.Forms.TextBox txtRequestorCenter;
        private System.Windows.Forms.TextBox txtWorkplanType;
        private System.Windows.Forms.Label lblWorkplanType;
        private System.Windows.Forms.TextBox txtAdditionalInstructions;
        private System.Windows.Forms.Button btnViewSQL;
        private System.Windows.Forms.TextBox queryMSRequestIDTextBox;
        private System.Windows.Forms.Label queryMSRequestIDLabel;
        private System.Windows.Forms.TextBox txtSourceTaskOrder;
        private System.Windows.Forms.TextBox txtActivity;
        private System.Windows.Forms.TextBox txtSourceActivityProject;
        private System.Windows.Forms.Label lblSourceTaskOrder;
        private System.Windows.Forms.Label lblSourceActivity;
        private System.Windows.Forms.Label lblSourceActivityProject;
        private System.Windows.Forms.WebBrowser descriptionBrowser;
        private System.Windows.Forms.TextBox txtReportAggregationLevel;
        private System.Windows.Forms.Label lblReportAggregationLevel;
        private System.Windows.Forms.Panel browserPanel;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Panel panel1;
    }
}