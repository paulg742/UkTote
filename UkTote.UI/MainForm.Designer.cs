namespace UkTote.UI
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtHostIpAddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numHostPort = new System.Windows.Forms.NumericUpDown();
            this.btnConnect = new System.Windows.Forms.Button();
            this.listBoxLog = new System.Windows.Forms.ListBox();
            this.btnCopyLog = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.balanceLabel = new System.Windows.Forms.ToolStripStatusLabel();
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
            this.checkBoxHideRawComms = new System.Windows.Forms.CheckBox();
            this.btnGetBalance = new System.Windows.Forms.Button();
            this.txtBetFolder = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnChangeBetFolder = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numHostPort)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Host IP Address";
            // 
            // txtHostIpAddress
            // 
            this.txtHostIpAddress.Location = new System.Drawing.Point(23, 43);
            this.txtHostIpAddress.Name = "txtHostIpAddress";
            this.txtHostIpAddress.Size = new System.Drawing.Size(156, 26);
            this.txtHostIpAddress.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Host Port";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(23, 179);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(156, 26);
            this.txtUsername.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Username";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(23, 247);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(156, 26);
            this.txtPassword.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 216);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Password";
            // 
            // numHostPort
            // 
            this.numHostPort.Location = new System.Drawing.Point(23, 111);
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
            this.numHostPort.TabIndex = 8;
            this.numHostPort.Value = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(23, 284);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(156, 34);
            this.btnConnect.TabIndex = 9;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // listBoxLog
            // 
            this.listBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxLog.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxLog.FormattingEnabled = true;
            this.listBoxLog.ItemHeight = 16;
            this.listBoxLog.Location = new System.Drawing.Point(189, 284);
            this.listBoxLog.Name = "listBoxLog";
            this.listBoxLog.Size = new System.Drawing.Size(1663, 404);
            this.listBoxLog.TabIndex = 10;
            // 
            // btnCopyLog
            // 
            this.btnCopyLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyLog.Location = new System.Drawing.Point(1623, 707);
            this.btnCopyLog.Name = "btnCopyLog";
            this.btnCopyLog.Size = new System.Drawing.Size(229, 30);
            this.btnCopyLog.TabIndex = 11;
            this.btnCopyLog.Text = "Export Log";
            this.btnCopyLog.UseVisualStyleBackColor = true;
            this.btnCopyLog.Click += new System.EventHandler(this.btnCopyLog_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.balanceLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1151);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1864, 22);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // balanceLabel
            // 
            this.balanceLabel.Name = "balanceLabel";
            this.balanceLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // btnGetRacecard
            // 
            this.btnGetRacecard.Location = new System.Drawing.Point(23, 324);
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
            this.racecardTreeView.Location = new System.Drawing.Point(189, 12);
            this.racecardTreeView.Name = "racecardTreeView";
            this.racecardTreeView.Size = new System.Drawing.Size(1663, 224);
            this.racecardTreeView.TabIndex = 14;
            // 
            // btnExportRacecard
            // 
            this.btnExportRacecard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportRacecard.Location = new System.Drawing.Point(1623, 245);
            this.btnExportRacecard.Name = "btnExportRacecard";
            this.btnExportRacecard.Size = new System.Drawing.Size(229, 30);
            this.btnExportRacecard.TabIndex = 15;
            this.btnExportRacecard.Text = "Export Racecard";
            this.btnExportRacecard.UseVisualStyleBackColor = true;
            this.btnExportRacecard.Click += new System.EventHandler(this.btnExportRacecard_Click);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
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
            this.columnHeader12});
            this.listView1.Location = new System.Drawing.Point(189, 781);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1663, 359);
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
            // checkBoxHideRawComms
            // 
            this.checkBoxHideRawComms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxHideRawComms.AutoSize = true;
            this.checkBoxHideRawComms.Location = new System.Drawing.Point(189, 713);
            this.checkBoxHideRawComms.Name = "checkBoxHideRawComms";
            this.checkBoxHideRawComms.Size = new System.Drawing.Size(162, 24);
            this.checkBoxHideRawComms.TabIndex = 17;
            this.checkBoxHideRawComms.Text = "Hide Raw Comms";
            this.checkBoxHideRawComms.UseVisualStyleBackColor = true;
            // 
            // btnGetBalance
            // 
            this.btnGetBalance.Location = new System.Drawing.Point(23, 364);
            this.btnGetBalance.Name = "btnGetBalance";
            this.btnGetBalance.Size = new System.Drawing.Size(156, 34);
            this.btnGetBalance.TabIndex = 18;
            this.btnGetBalance.Text = "Get Balance";
            this.btnGetBalance.UseVisualStyleBackColor = true;
            this.btnGetBalance.Click += new System.EventHandler(this.btnGetBalance_Click);
            // 
            // txtBetFolder
            // 
            this.txtBetFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtBetFolder.Location = new System.Drawing.Point(278, 746);
            this.txtBetFolder.Name = "txtBetFolder";
            this.txtBetFolder.Size = new System.Drawing.Size(443, 26);
            this.txtBetFolder.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(185, 749);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 20);
            this.label5.TabIndex = 20;
            this.label5.Text = "Bet Folder:";
            // 
            // btnChangeBetFolder
            // 
            this.btnChangeBetFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnChangeBetFolder.Location = new System.Drawing.Point(727, 746);
            this.btnChangeBetFolder.Name = "btnChangeBetFolder";
            this.btnChangeBetFolder.Size = new System.Drawing.Size(33, 32);
            this.btnChangeBetFolder.TabIndex = 21;
            this.btnChangeBetFolder.Text = "...";
            this.btnChangeBetFolder.UseVisualStyleBackColor = true;
            this.btnChangeBetFolder.Click += new System.EventHandler(this.btnChangeBetFolder_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1864, 1173);
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
            this.Controls.Add(this.numHostPort);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtHostIpAddress);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "UkTote.UI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numHostPort)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtHostIpAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numHostPort;
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
    }
}

