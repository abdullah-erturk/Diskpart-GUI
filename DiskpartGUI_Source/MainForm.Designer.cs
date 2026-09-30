namespace DiskpartGUI
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            cmbDisks = new ComboBox();
            lblDisks = new Label();
            rbGpt = new RadioButton();
            rbMbr = new RadioButton();
            chkRecovery = new CheckBox();
            numBootSize = new NumericUpDown();
            btnFormat = new Button();
            txtOutput = new RichTextBox();
            lblBootSize = new Label();
            lblWindowsSize = new Label();
            numWindowsSize = new NumericUpDown();
            pnlDiskMap = new Panel();
            pnlPartRec = new Panel();
            pnlPartData = new Panel();
            pnlPartWin = new Panel();
            pnlPartBoot = new Panel();
            lblDataSize = new Label();
            btnRefresh = new Button();
            lblRecoverySize = new Label();
            numRecoverySize = new NumericUpDown();
            pnlFooter = new Panel();
            lnkGithub = new LinkLabel();
            lnkAbout = new LinkLabel();
            lnkWebsite = new LinkLabel();
            btnCreateVhd = new Button();
            numVhdSize = new NumericUpDown();
            lblVhdSize = new Label();
            lblLang = new Label();
            cmbLang = new ComboBox();
            lblDiskInfo = new Label();
            ((System.ComponentModel.ISupportInitialize)numBootSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numWindowsSize).BeginInit();
            pnlDiskMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numRecoverySize).BeginInit();
            pnlFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numVhdSize).BeginInit();
            SuspendLayout();
            // 
            // cmbDisks
            // 
            cmbDisks.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDisks.FormattingEnabled = true;
            cmbDisks.Location = new Point(12, 32);
            cmbDisks.Name = "cmbDisks";
            cmbDisks.Size = new Size(448, 23);
            cmbDisks.TabIndex = 0;
            cmbDisks.SelectedIndexChanged += cmbDisks_SelectedIndexChanged;
            // 
            // lblDisks
            // 
            lblDisks.AutoSize = true;
            lblDisks.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDisks.Location = new Point(12, 14);
            lblDisks.Name = "lblDisks";
            lblDisks.Size = new Size(72, 15);
            lblDisks.TabIndex = 1;
            lblDisks.Text = "Select Disk:";
            // 
            // rbGpt
            // 
            rbGpt.AutoSize = true;
            rbGpt.Checked = true;
            rbGpt.Location = new Point(12, 85);
            rbGpt.Name = "rbGpt";
            rbGpt.Size = new Size(67, 19);
            rbGpt.TabIndex = 2;
            rbGpt.TabStop = true;
            rbGpt.Text = "GPT/EFI";
            rbGpt.UseVisualStyleBackColor = true;
            // 
            // rbMbr
            // 
            rbMbr.AutoSize = true;
            rbMbr.Location = new Point(100, 85);
            rbMbr.Name = "rbMbr";
            rbMbr.Size = new Size(80, 19);
            rbMbr.TabIndex = 3;
            rbMbr.Text = "MBR/BIOS";
            rbMbr.UseVisualStyleBackColor = true;
            // 
            // chkRecovery
            // 
            chkRecovery.AutoSize = true;
            chkRecovery.Location = new Point(12, 115);
            chkRecovery.Name = "chkRecovery";
            chkRecovery.Size = new Size(194, 19);
            chkRecovery.TabIndex = 4;
            chkRecovery.Text = "Create Recovery Partition (MB) :";
            chkRecovery.UseVisualStyleBackColor = true;
            chkRecovery.CheckedChanged += chkRecovery_CheckedChanged;
            // 
            // numBootSize
            // 
            numBootSize.Location = new Point(244, 85);
            numBootSize.Maximum = new decimal(new int[] { 2048, 0, 0, 0 });
            numBootSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numBootSize.Name = "numBootSize";
            numBootSize.Size = new Size(74, 23);
            numBootSize.TabIndex = 20;
            numBootSize.Value = new decimal(new int[] { 200, 0, 0, 0 });
            numBootSize.ValueChanged += TriggerSizeUpdate;
            numBootSize.KeyUp += TriggerSizeUpdate;
            numBootSize.Leave += FinalizeMinimums;
            // 
            // btnFormat
            // 
            btnFormat.BackColor = Color.Maroon;
            btnFormat.FlatStyle = FlatStyle.Flat;
            btnFormat.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnFormat.ForeColor = Color.White;
            btnFormat.Location = new Point(506, 15);
            btnFormat.Name = "btnFormat";
            btnFormat.Size = new Size(210, 54);
            btnFormat.TabIndex = 6;
            btnFormat.Text = "FORMAT DISK";
            btnFormat.UseVisualStyleBackColor = false;
            btnFormat.Click += btnFormat_Click;
            // 
            // txtOutput
            // 
            txtOutput.BackColor = Color.Black;
            txtOutput.BorderStyle = BorderStyle.None;
            txtOutput.Font = new Font("Consolas", 9.75F);
            txtOutput.ForeColor = Color.Lime;
            txtOutput.Location = new Point(12, 210);
            txtOutput.Name = "txtOutput";
            txtOutput.ReadOnly = true;
            txtOutput.Size = new Size(811, 254);
            txtOutput.TabIndex = 7;
            txtOutput.Text = "";
            // 
            // lblBootSize
            // 
            lblBootSize.AutoSize = true;
            lblBootSize.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblBootSize.Location = new Point(242, 65);
            lblBootSize.Name = "lblBootSize";
            lblBootSize.Size = new Size(70, 15);
            lblBootSize.TabIndex = 8;
            lblBootSize.Text = "Boot (MB) :";
            // 
            // lblWindowsSize
            // 
            lblWindowsSize.AutoSize = true;
            lblWindowsSize.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblWindowsSize.Location = new Point(341, 65);
            lblWindowsSize.Name = "lblWindowsSize";
            lblWindowsSize.Size = new Size(92, 15);
            lblWindowsSize.TabIndex = 9;
            lblWindowsSize.Text = "Windows (GB) :";
            // 
            // numWindowsSize
            // 
            numWindowsSize.Location = new Point(341, 85);
            numWindowsSize.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numWindowsSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numWindowsSize.Name = "numWindowsSize";
            numWindowsSize.Size = new Size(74, 23);
            numWindowsSize.TabIndex = 10;
            numWindowsSize.Value = new decimal(new int[] { 100, 0, 0, 0 });
            numWindowsSize.ValueChanged += TriggerSizeUpdate;
            numWindowsSize.KeyUp += TriggerSizeUpdate;
            // 
            // pnlDiskMap
            // 
            pnlDiskMap.BackColor = Color.FromArgb(45, 45, 45);
            pnlDiskMap.BorderStyle = BorderStyle.FixedSingle;
            pnlDiskMap.Controls.Add(pnlPartRec);
            pnlDiskMap.Controls.Add(pnlPartData);
            pnlDiskMap.Controls.Add(pnlPartWin);
            pnlDiskMap.Controls.Add(pnlPartBoot);
            pnlDiskMap.Location = new Point(12, 168);
            pnlDiskMap.Name = "pnlDiskMap";
            pnlDiskMap.Size = new Size(811, 35);
            pnlDiskMap.TabIndex = 11;
            pnlDiskMap.Click += ClearFocus_Click;
            // 
            // pnlPartRec
            // 
            pnlPartRec.BackColor = Color.FromArgb(231, 76, 60);
            pnlPartRec.Location = new Point(0, 0);
            pnlPartRec.Name = "pnlPartRec";
            pnlPartRec.Size = new Size(25, 35);
            pnlPartRec.TabIndex = 0;
            // 
            // pnlPartData
            // 
            pnlPartData.BackColor = Color.FromArgb(46, 204, 113);
            pnlPartData.Location = new Point(0, 0);
            pnlPartData.Name = "pnlPartData";
            pnlPartData.Size = new Size(25, 35);
            pnlPartData.TabIndex = 1;
            // 
            // pnlPartWin
            // 
            pnlPartWin.BackColor = Color.FromArgb(52, 152, 219);
            pnlPartWin.Location = new Point(0, 0);
            pnlPartWin.Name = "pnlPartWin";
            pnlPartWin.Size = new Size(25, 35);
            pnlPartWin.TabIndex = 2;
            // 
            // pnlPartBoot
            // 
            pnlPartBoot.BackColor = Color.FromArgb(230, 126, 34);
            pnlPartBoot.Location = new Point(0, 0);
            pnlPartBoot.Name = "pnlPartBoot";
            pnlPartBoot.Size = new Size(25, 35);
            pnlPartBoot.TabIndex = 3;
            // 
            // lblDataSize
            // 
            lblDataSize.AutoSize = true;
            lblDataSize.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDataSize.ForeColor = Color.ForestGreen;
            lblDataSize.Location = new Point(241, 115);
            lblDataSize.Name = "lblDataSize";
            lblDataSize.Size = new Size(146, 15);
            lblDataSize.TabIndex = 12;
            lblDataSize.Text = "Remaining Data Partition";
            lblDataSize.Click += ClearFocus_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(466, 30);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(34, 23);
            btnRefresh.TabIndex = 13;
            btnRefresh.Text = "⟳";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // lblRecoverySize
            // 
            lblRecoverySize.AutoSize = true;
            lblRecoverySize.Location = new Point(124, 114);
            lblRecoverySize.Name = "lblRecoverySize";
            lblRecoverySize.Size = new Size(0, 15);
            lblRecoverySize.TabIndex = 14;
            // 
            // numRecoverySize
            // 
            numRecoverySize.Enabled = false;
            numRecoverySize.Location = new Point(12, 140);
            numRecoverySize.Maximum = new decimal(new int[] { 51200, 0, 0, 0 });
            numRecoverySize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numRecoverySize.Name = "numRecoverySize";
            numRecoverySize.Size = new Size(75, 23);
            numRecoverySize.TabIndex = 18;
            numRecoverySize.Value = new decimal(new int[] { 800, 0, 0, 0 });
            numRecoverySize.ValueChanged += TriggerSizeUpdate;
            numRecoverySize.KeyUp += TriggerSizeUpdate;
            numRecoverySize.Leave += FinalizeMinimums;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = SystemColors.ControlLight;
            pnlFooter.Controls.Add(lnkGithub);
            pnlFooter.Controls.Add(lnkAbout);
            pnlFooter.Controls.Add(lnkWebsite);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 470);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(833, 30);
            pnlFooter.TabIndex = 16;
            pnlFooter.Click += ClearFocus_Click;
            // 
            // lnkGithub
            // 
            lnkGithub.AutoSize = true;
            lnkGithub.Location = new Point(776, 8);
            lnkGithub.Name = "lnkGithub";
            lnkGithub.Size = new Size(45, 15);
            lnkGithub.TabIndex = 2;
            lnkGithub.TabStop = true;
            lnkGithub.Text = "GitHub";
            lnkGithub.LinkClicked += lnkGithub_LinkClicked;
            // 
            // lnkAbout
            // 
            lnkAbout.AutoSize = true;
            lnkAbout.Location = new Point(383, 8);
            lnkAbout.Name = "lnkAbout";
            lnkAbout.Size = new Size(40, 15);
            lnkAbout.TabIndex = 1;
            lnkAbout.TabStop = true;
            lnkAbout.Text = "About";
            lnkAbout.LinkClicked += lnkAbout_LinkClicked;
            // 
            // lnkWebsite
            // 
            lnkWebsite.AutoSize = true;
            lnkWebsite.Location = new Point(13, 8);
            lnkWebsite.Name = "lnkWebsite";
            lnkWebsite.Size = new Size(53, 15);
            lnkWebsite.TabIndex = 0;
            lnkWebsite.TabStop = true;
            lnkWebsite.Text = "Web Site";
            lnkWebsite.LinkClicked += lnkWebsite_LinkClicked;
            // 
            // btnCreateVhd
            // 
            btnCreateVhd.BackColor = Color.ForestGreen;
            btnCreateVhd.FlatStyle = FlatStyle.Flat;
            btnCreateVhd.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCreateVhd.ForeColor = Color.White;
            btnCreateVhd.Location = new Point(622, 121);
            btnCreateVhd.Name = "btnCreateVhd";
            btnCreateVhd.Size = new Size(201, 30);
            btnCreateVhd.TabIndex = 17;
            btnCreateVhd.Text = "CREATE VHD/VHDX";
            btnCreateVhd.UseVisualStyleBackColor = false;
            btnCreateVhd.Click += btnCreateVhd_Click;
            // 
            // numVhdSize
            // 
            numVhdSize.Location = new Point(749, 89);
            numVhdSize.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numVhdSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numVhdSize.Name = "numVhdSize";
            numVhdSize.Size = new Size(74, 23);
            numVhdSize.TabIndex = 18;
            numVhdSize.Value = new decimal(new int[] { 70, 0, 0, 0 });
            // 
            // lblVhdSize
            // 
            lblVhdSize.AutoSize = true;
            lblVhdSize.Location = new Point(622, 93);
            lblVhdSize.Name = "lblVhdSize";
            lblVhdSize.Size = new Size(98, 15);
            lblVhdSize.TabIndex = 19;
            lblVhdSize.Text = "VHD/X Size (GB) :";
            // 
            // lblLang
            // 
            lblLang.AutoSize = true;
            lblLang.Location = new Point(758, 18);
            lblLang.Name = "lblLang";
            lblLang.Size = new Size(36, 15);
            lblLang.TabIndex = 21;
            lblLang.Text = "Lang:";
            // 
            // cmbLang
            // 
            cmbLang.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLang.FormattingEnabled = true;
            cmbLang.Location = new Point(733, 40);
            cmbLang.Name = "cmbLang";
            cmbLang.Size = new Size(90, 23);
            cmbLang.TabIndex = 22;
            cmbLang.SelectedIndexChanged += cmbLang_SelectedIndexChanged;
            // 
            // lblDiskInfo
            // 
            lblDiskInfo.AutoSize = true;
            lblDiskInfo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDiskInfo.ForeColor = Color.ForestGreen;
            lblDiskInfo.Location = new Point(12, 65);
            lblDiskInfo.Name = "lblDiskInfo";
            lblDiskInfo.Size = new Size(130, 15);
            lblDiskInfo.TabIndex = 23;
            lblDiskInfo.Text = "Status: OK | Type: SSD";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(833, 500);
            Controls.Add(lblDiskInfo);
            Controls.Add(cmbLang);
            Controls.Add(lblLang);
            Controls.Add(lblVhdSize);
            Controls.Add(numVhdSize);
            Controls.Add(btnCreateVhd);
            Controls.Add(chkRecovery);
            Controls.Add(pnlFooter);
            Controls.Add(numRecoverySize);
            Controls.Add(lblRecoverySize);
            Controls.Add(btnRefresh);
            Controls.Add(lblDataSize);
            Controls.Add(pnlDiskMap);
            Controls.Add(numWindowsSize);
            Controls.Add(lblWindowsSize);
            Controls.Add(numBootSize);
            Controls.Add(lblBootSize);
            Controls.Add(txtOutput);
            Controls.Add(btnFormat);
            Controls.Add(rbMbr);
            Controls.Add(rbGpt);
            Controls.Add(lblDisks);
            Controls.Add(cmbDisks);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Diskpart GUI v4 | made by Abdullah ERTÜRK";
            Load += MainForm_Load;
            Click += ClearFocus_Click;
            ((System.ComponentModel.ISupportInitialize)numBootSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)numWindowsSize).EndInit();
            pnlDiskMap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numRecoverySize).EndInit();
            pnlFooter.ResumeLayout(false);
            pnlFooter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numVhdSize).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.ComboBox cmbDisks;
        private System.Windows.Forms.Label lblDisks;
        private System.Windows.Forms.RadioButton rbGpt;
        private System.Windows.Forms.RadioButton rbMbr;
        private System.Windows.Forms.CheckBox chkRecovery;
        private System.Windows.Forms.NumericUpDown numBootSize;
        private System.Windows.Forms.Button btnFormat;
        private System.Windows.Forms.RichTextBox txtOutput;
        private System.Windows.Forms.Label lblBootSize;
        private System.Windows.Forms.Label lblWindowsSize;
        private System.Windows.Forms.NumericUpDown numWindowsSize;
        private System.Windows.Forms.Panel pnlDiskMap;
        private System.Windows.Forms.Label lblDataSize;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblRecoverySize;
        private System.Windows.Forms.NumericUpDown numRecoverySize;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.LinkLabel lnkGithub;
        private System.Windows.Forms.LinkLabel lnkAbout;
        private System.Windows.Forms.LinkLabel lnkWebsite;
        private System.Windows.Forms.Button btnCreateVhd;
        private System.Windows.Forms.NumericUpDown numVhdSize;
        private System.Windows.Forms.Label lblVhdSize;
        private System.Windows.Forms.Label lblLang;
        private System.Windows.Forms.ComboBox cmbLang;
        private System.Windows.Forms.Panel pnlPartRec;
        private System.Windows.Forms.Panel pnlPartData;
        private System.Windows.Forms.Panel pnlPartWin;
        private System.Windows.Forms.Panel pnlPartBoot;
        private System.Windows.Forms.Label lblDiskInfo;
    }
}
