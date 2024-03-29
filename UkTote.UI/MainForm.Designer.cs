﻿namespace UkTote.UI
{
    partial class MainForm
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.listBoxLog = new System.Windows.Forms.ListBox();
            this.btnCopyLog = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.balanceLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.batchProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.versionLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnGetRacecard = new System.Windows.Forms.Button();
            this.racecardTreeView = new System.Windows.Forms.TreeView();
            this.btnExportRacecard = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.checkBoxHideRawComms = new System.Windows.Forms.CheckBox();
            this.btnGetBalance = new System.Windows.Forms.Button();
            this.txtBetFolder = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnChangeBetFolder = new System.Windows.Forms.Button();
            this.numLastBetId = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.btnExportBets = new System.Windows.Forms.Button();
            this.btnChangeFeedFolder = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txtFeedFolder = new System.Windows.Forms.TextBox();
            this.btnMsnRequest = new System.Windows.Forms.Button();
            this.btnPayEnquiry = new System.Windows.Forms.Button();
            this.btnChangeBetOutputFolder = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtBetOutputFolder = new System.Windows.Forms.TextBox();
            this.btnArchiveFeed = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.btnOpenBetInputFolder = new System.Windows.Forms.Button();
            this.btnOpenBetOutputFolder = new System.Windows.Forms.Button();
            this.btnOpenFeedOutputFolder = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSaveLogin = new System.Windows.Forms.Button();
            this.numHostPort = new System.Windows.Forms.NumericUpDown();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtHostIpAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLastBetId)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHostPort)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(22, 388);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(156, 34);
            this.btnConnect.TabIndex = 9;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // listBoxLog
            // 
            this.listBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxLog.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxLog.FormattingEnabled = true;
            this.listBoxLog.ItemHeight = 16;
            this.listBoxLog.Location = new System.Drawing.Point(189, 680);
            this.listBoxLog.Name = "listBoxLog";
            this.listBoxLog.Size = new System.Drawing.Size(921, 148);
            this.listBoxLog.TabIndex = 10;
            // 
            // btnCopyLog
            // 
            this.btnCopyLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyLog.Location = new System.Drawing.Point(832, 845);
            this.btnCopyLog.Name = "btnCopyLog";
            this.btnCopyLog.Size = new System.Drawing.Size(280, 38);
            this.btnCopyLog.TabIndex = 11;
            this.btnCopyLog.Text = "Export Log";
            this.btnCopyLog.UseVisualStyleBackColor = true;
            this.btnCopyLog.Click += new System.EventHandler(this.btnCopyLog_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.balanceLabel,
            this.batchProgressBar,
            this.versionLabel});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 885);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 14, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1121, 32);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 25);
            // 
            // balanceLabel
            // 
            this.balanceLabel.Name = "balanceLabel";
            this.balanceLabel.Size = new System.Drawing.Size(0, 25);
            // 
            // batchProgressBar
            // 
            this.batchProgressBar.Name = "batchProgressBar";
            this.batchProgressBar.Size = new System.Drawing.Size(100, 24);
            // 
            // versionLabel
            // 
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(110, 25);
            this.versionLabel.Text = "versionLabel";
            // 
            // btnGetRacecard
            // 
            this.btnGetRacecard.Location = new System.Drawing.Point(22, 428);
            this.btnGetRacecard.Name = "btnGetRacecard";
            this.btnGetRacecard.Size = new System.Drawing.Size(156, 34);
            this.btnGetRacecard.TabIndex = 13;
            this.btnGetRacecard.Text = "Get Racecard";
            this.btnGetRacecard.UseVisualStyleBackColor = true;
            this.btnGetRacecard.Click += new System.EventHandler(this.btnGetRacecard_Click);
            // 
            // racecardTreeView
            // 
            this.racecardTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.racecardTreeView.Location = new System.Drawing.Point(189, 43);
            this.racecardTreeView.Name = "racecardTreeView";
            this.racecardTreeView.Size = new System.Drawing.Size(921, 193);
            this.racecardTreeView.TabIndex = 14;
            // 
            // btnExportRacecard
            // 
            this.btnExportRacecard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportRacecard.Location = new System.Drawing.Point(829, 242);
            this.btnExportRacecard.Name = "btnExportRacecard";
            this.btnExportRacecard.Size = new System.Drawing.Size(280, 38);
            this.btnExportRacecard.TabIndex = 15;
            this.btnExportRacecard.Text = "Export Racecard";
            this.btnExportRacecard.UseVisualStyleBackColor = true;
            this.btnExportRacecard.Click += new System.EventHandler(this.btnExportRacecard_Click);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader10,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13});
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(189, 388);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(921, 243);
            this.listView1.TabIndex = 16;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Raw";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Date";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Meeting#";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Race#";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "UnitStake";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "TotalStake";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "BetType";
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "BetOption";
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Selections";
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Status";
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "BetId";
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "TSN";
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Pay Enquiry Result";
            this.columnHeader13.Width = 158;
            // 
            // checkBoxHideRawComms
            // 
            this.checkBoxHideRawComms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxHideRawComms.AutoSize = true;
            this.checkBoxHideRawComms.Checked = true;
            this.checkBoxHideRawComms.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxHideRawComms.Location = new System.Drawing.Point(16, 681);
            this.checkBoxHideRawComms.Name = "checkBoxHideRawComms";
            this.checkBoxHideRawComms.Size = new System.Drawing.Size(162, 24);
            this.checkBoxHideRawComms.TabIndex = 17;
            this.checkBoxHideRawComms.Text = "Hide Raw Comms";
            this.checkBoxHideRawComms.UseVisualStyleBackColor = true;
            // 
            // btnGetBalance
            // 
            this.btnGetBalance.Location = new System.Drawing.Point(22, 468);
            this.btnGetBalance.Name = "btnGetBalance";
            this.btnGetBalance.Size = new System.Drawing.Size(156, 34);
            this.btnGetBalance.TabIndex = 18;
            this.btnGetBalance.Text = "Get Balance";
            this.btnGetBalance.UseVisualStyleBackColor = true;
            this.btnGetBalance.Click += new System.EventHandler(this.btnGetBalance_Click);
            // 
            // txtBetFolder
            // 
            this.txtBetFolder.Location = new System.Drawing.Point(339, 285);
            this.txtBetFolder.Name = "txtBetFolder";
            this.txtBetFolder.Size = new System.Drawing.Size(382, 26);
            this.txtBetFolder.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(184, 288);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(128, 20);
            this.label5.TabIndex = 20;
            this.label5.Text = "Bet Input Folder:";
            // 
            // btnChangeBetFolder
            // 
            this.btnChangeBetFolder.Location = new System.Drawing.Point(728, 285);
            this.btnChangeBetFolder.Name = "btnChangeBetFolder";
            this.btnChangeBetFolder.Size = new System.Drawing.Size(86, 32);
            this.btnChangeBetFolder.TabIndex = 21;
            this.btnChangeBetFolder.Text = "Change";
            this.btnChangeBetFolder.UseVisualStyleBackColor = true;
            this.btnChangeBetFolder.Click += new System.EventHandler(this.btnChangeBetFolder_Click);
            // 
            // numLastBetId
            // 
            this.numLastBetId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numLastBetId.Enabled = false;
            this.numLastBetId.Location = new System.Drawing.Point(281, 643);
            this.numLastBetId.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numLastBetId.Name = "numLastBetId";
            this.numLastBetId.Size = new System.Drawing.Size(156, 26);
            this.numLastBetId.TabIndex = 22;
            this.numLastBetId.ValueChanged += new System.EventHandler(this.numLastBetId_ValueChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(185, 645);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 20);
            this.label6.TabIndex = 23;
            this.label6.Text = "Last Bet ID";
            // 
            // btnExportBets
            // 
            this.btnExportBets.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportBets.Location = new System.Drawing.Point(905, 637);
            this.btnExportBets.Name = "btnExportBets";
            this.btnExportBets.Size = new System.Drawing.Size(202, 38);
            this.btnExportBets.TabIndex = 24;
            this.btnExportBets.Text = "Export Bets";
            this.btnExportBets.UseVisualStyleBackColor = true;
            this.btnExportBets.Click += new System.EventHandler(this.btnExportBets_Click);
            // 
            // btnChangeFeedFolder
            // 
            this.btnChangeFeedFolder.Location = new System.Drawing.Point(728, 349);
            this.btnChangeFeedFolder.Name = "btnChangeFeedFolder";
            this.btnChangeFeedFolder.Size = new System.Drawing.Size(86, 32);
            this.btnChangeFeedFolder.TabIndex = 27;
            this.btnChangeFeedFolder.Text = "Change";
            this.btnChangeFeedFolder.UseVisualStyleBackColor = true;
            this.btnChangeFeedFolder.Click += new System.EventHandler(this.btnChangeFeedFolder_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(184, 352);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(152, 20);
            this.label7.TabIndex = 26;
            this.label7.Text = "Feed Output Folder:";
            // 
            // txtFeedFolder
            // 
            this.txtFeedFolder.Location = new System.Drawing.Point(339, 349);
            this.txtFeedFolder.Name = "txtFeedFolder";
            this.txtFeedFolder.Size = new System.Drawing.Size(382, 26);
            this.txtFeedFolder.TabIndex = 25;
            // 
            // btnMsnRequest
            // 
            this.btnMsnRequest.Location = new System.Drawing.Point(22, 508);
            this.btnMsnRequest.Name = "btnMsnRequest";
            this.btnMsnRequest.Size = new System.Drawing.Size(156, 34);
            this.btnMsnRequest.TabIndex = 28;
            this.btnMsnRequest.Text = "MSN Request";
            this.btnMsnRequest.UseVisualStyleBackColor = true;
            this.btnMsnRequest.Click += new System.EventHandler(this.btnMsnRequest_Click);
            // 
            // btnPayEnquiry
            // 
            this.btnPayEnquiry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPayEnquiry.Location = new System.Drawing.Point(695, 637);
            this.btnPayEnquiry.Name = "btnPayEnquiry";
            this.btnPayEnquiry.Size = new System.Drawing.Size(206, 38);
            this.btnPayEnquiry.TabIndex = 29;
            this.btnPayEnquiry.Text = "Pay Enquiry";
            this.btnPayEnquiry.UseVisualStyleBackColor = true;
            this.btnPayEnquiry.Click += new System.EventHandler(this.btnPayEnquiry_Click);
            // 
            // btnChangeBetOutputFolder
            // 
            this.btnChangeBetOutputFolder.Location = new System.Drawing.Point(728, 317);
            this.btnChangeBetOutputFolder.Name = "btnChangeBetOutputFolder";
            this.btnChangeBetOutputFolder.Size = new System.Drawing.Size(86, 32);
            this.btnChangeBetOutputFolder.TabIndex = 32;
            this.btnChangeBetOutputFolder.Text = "Change";
            this.btnChangeBetOutputFolder.UseVisualStyleBackColor = true;
            this.btnChangeBetOutputFolder.Click += new System.EventHandler(this.btnChangeBetOutputFolder_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(184, 320);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(140, 20);
            this.label8.TabIndex = 31;
            this.label8.Text = "Bet Output Folder:";
            // 
            // txtBetOutputFolder
            // 
            this.txtBetOutputFolder.Location = new System.Drawing.Point(339, 317);
            this.txtBetOutputFolder.Name = "txtBetOutputFolder";
            this.txtBetOutputFolder.Size = new System.Drawing.Size(382, 26);
            this.txtBetOutputFolder.TabIndex = 30;
            // 
            // btnArchiveFeed
            // 
            this.btnArchiveFeed.Location = new System.Drawing.Point(22, 548);
            this.btnArchiveFeed.Name = "btnArchiveFeed";
            this.btnArchiveFeed.Size = new System.Drawing.Size(156, 34);
            this.btnArchiveFeed.TabIndex = 33;
            this.btnArchiveFeed.Text = "Archive Feed";
            this.btnArchiveFeed.UseVisualStyleBackColor = true;
            this.btnArchiveFeed.Click += new System.EventHandler(this.btnArchiveFeed_Click);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.Info;
            this.label9.Dock = System.Windows.Forms.DockStyle.Top;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(0, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(1121, 20);
            this.label9.TabIndex = 34;
            this.label9.Text = "*** Provided on an AS IS basis. Use of this test tool for live betting is at the " +
    "USERS own risk ***";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOpenBetInputFolder
            // 
            this.btnOpenBetInputFolder.Location = new System.Drawing.Point(820, 285);
            this.btnOpenBetInputFolder.Name = "btnOpenBetInputFolder";
            this.btnOpenBetInputFolder.Size = new System.Drawing.Size(86, 32);
            this.btnOpenBetInputFolder.TabIndex = 35;
            this.btnOpenBetInputFolder.Text = "Open";
            this.btnOpenBetInputFolder.UseVisualStyleBackColor = true;
            this.btnOpenBetInputFolder.Click += new System.EventHandler(this.btnOpenBetInputFolder_Click);
            // 
            // btnOpenBetOutputFolder
            // 
            this.btnOpenBetOutputFolder.Location = new System.Drawing.Point(820, 317);
            this.btnOpenBetOutputFolder.Name = "btnOpenBetOutputFolder";
            this.btnOpenBetOutputFolder.Size = new System.Drawing.Size(86, 32);
            this.btnOpenBetOutputFolder.TabIndex = 36;
            this.btnOpenBetOutputFolder.Text = "Open";
            this.btnOpenBetOutputFolder.UseVisualStyleBackColor = true;
            this.btnOpenBetOutputFolder.Click += new System.EventHandler(this.btnOpenBetOutputFolder_Click);
            // 
            // btnOpenFeedOutputFolder
            // 
            this.btnOpenFeedOutputFolder.Location = new System.Drawing.Point(820, 349);
            this.btnOpenFeedOutputFolder.Name = "btnOpenFeedOutputFolder";
            this.btnOpenFeedOutputFolder.Size = new System.Drawing.Size(86, 32);
            this.btnOpenFeedOutputFolder.TabIndex = 37;
            this.btnOpenFeedOutputFolder.Text = "Open";
            this.btnOpenFeedOutputFolder.UseVisualStyleBackColor = true;
            this.btnOpenFeedOutputFolder.Click += new System.EventHandler(this.btnOpenFeedOutputFolder_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSaveLogin);
            this.groupBox1.Controls.Add(this.numHostPort);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtUsername);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtHostIpAddress);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(171, 343);
            this.groupBox1.TabIndex = 38;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Host Config";
            // 
            // btnSaveLogin
            // 
            this.btnSaveLogin.Location = new System.Drawing.Point(5, 290);
            this.btnSaveLogin.Name = "btnSaveLogin";
            this.btnSaveLogin.Size = new System.Drawing.Size(156, 34);
            this.btnSaveLogin.TabIndex = 47;
            this.btnSaveLogin.Text = "Save";
            this.btnSaveLogin.UseVisualStyleBackColor = true;
            this.btnSaveLogin.Click += new System.EventHandler(this.btnSaveLogin_Click);
            // 
            // numHostPort
            // 
            this.numHostPort.Location = new System.Drawing.Point(5, 121);
            this.numHostPort.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numHostPort.Minimum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.numHostPort.Name = "numHostPort";
            this.numHostPort.Size = new System.Drawing.Size(156, 26);
            this.numHostPort.TabIndex = 46;
            this.numHostPort.Value = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(5, 258);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(156, 26);
            this.txtPassword.TabIndex = 45;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 225);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 20);
            this.label4.TabIndex = 44;
            this.label4.Text = "Password";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(5, 188);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(156, 26);
            this.txtUsername.TabIndex = 43;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 20);
            this.label3.TabIndex = 42;
            this.label3.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 20);
            this.label2.TabIndex = 41;
            this.label2.Text = "Port";
            // 
            // txtHostIpAddress
            // 
            this.txtHostIpAddress.Location = new System.Drawing.Point(5, 53);
            this.txtHostIpAddress.Name = "txtHostIpAddress";
            this.txtHostIpAddress.Size = new System.Drawing.Size(156, 26);
            this.txtHostIpAddress.TabIndex = 40;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 20);
            this.label1.TabIndex = 39;
            this.label1.Text = "IP Address";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1121, 917);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOpenFeedOutputFolder);
            this.Controls.Add(this.btnOpenBetOutputFolder);
            this.Controls.Add(this.btnOpenBetInputFolder);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnArchiveFeed);
            this.Controls.Add(this.btnChangeBetOutputFolder);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtBetOutputFolder);
            this.Controls.Add(this.btnPayEnquiry);
            this.Controls.Add(this.btnMsnRequest);
            this.Controls.Add(this.btnChangeFeedFolder);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtFeedFolder);
            this.Controls.Add(this.btnExportBets);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numLastBetId);
            this.Controls.Add(this.btnChangeBetFolder);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBetFolder);
            this.Controls.Add(this.btnGetBalance);
            this.Controls.Add(this.checkBoxHideRawComms);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.btnExportRacecard);
            this.Controls.Add(this.racecardTreeView);
            this.Controls.Add(this.btnGetRacecard);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnCopyLog);
            this.Controls.Add(this.listBoxLog);
            this.Controls.Add(this.btnConnect);
            this.Name = "MainForm";
            this.Text = "UkTote UI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLastBetId)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHostPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ListBox listBoxLog;
        private System.Windows.Forms.Button btnCopyLog;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripStatusLabel balanceLabel;
        private System.Windows.Forms.Button btnGetRacecard;
        private System.Windows.Forms.TreeView racecardTreeView;
        private System.Windows.Forms.Button btnExportRacecard;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.CheckBox checkBoxHideRawComms;
        private System.Windows.Forms.Button btnGetBalance;
        private System.Windows.Forms.TextBox txtBetFolder;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnChangeBetFolder;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.NumericUpDown numLastBetId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnExportBets;
        private System.Windows.Forms.Button btnChangeFeedFolder;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtFeedFolder;
        private System.Windows.Forms.Button btnMsnRequest;
        private System.Windows.Forms.Button btnPayEnquiry;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.Button btnChangeBetOutputFolder;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtBetOutputFolder;
        private System.Windows.Forms.Button btnArchiveFeed;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ToolStripProgressBar batchProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel versionLabel;
        private System.Windows.Forms.Button btnOpenBetInputFolder;
        private System.Windows.Forms.Button btnOpenBetOutputFolder;
        private System.Windows.Forms.Button btnOpenFeedOutputFolder;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSaveLogin;
        private System.Windows.Forms.NumericUpDown numHostPort;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtHostIpAddress;
        private System.Windows.Forms.Label label1;
    }
}

