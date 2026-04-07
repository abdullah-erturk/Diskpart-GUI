using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DiskpartGUI
{
    public partial class MainForm : Form
    {
        private DiskManager _diskManager;
        private double _selectedDiskSizeGB = 0;
        private ToolTip _toolTip;
        private bool _isUpdatingSizes = false;

        public MainForm()
        {
            Localization.Initialize(); // Init earlier to get system language
            InitializeComponent();
            _diskManager = new DiskManager();
            _diskManager.OnOutputReceived += AppendOutput;
            _toolTip = new ToolTip();

            // Fix Minimums to support small disks
            numWindowsSize.Minimum = 1;
            numRecoverySize.Minimum = 100;
            numBootSize.Minimum = 50;

            // Set Icon
            try { this.Icon = new Icon("ico.ico"); } catch { }
            this.Text = "Diskpart GUI v2 | made by Abdullah ERTÜRK";

            // Init Languages
            cmbLang.Items.Add("Türkçe");
            cmbLang.Items.Add("English");
            cmbLang.SelectedIndex = Localization.CurrentLanguage == "tr" ? 0 : 1;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ApplyLocalization();
            RefreshDiskList();
        }

        private void ApplyLocalization()
        {
            lblDisks.Text = Localization.Get("UI_SelectDisk");
            rbGpt.Text = Localization.Get("UI_GPT");
            rbMbr.Text = Localization.Get("UI_MBR");
            chkRecovery.Text = Localization.Get("UI_CreateRecovery");
            lblBootSize.Text = Localization.Get("UI_BootSize");
            lblWindowsSize.Text = Localization.Get("UI_WindowsSize");
            lblVhdSize.Text = Localization.Get("UI_VHD_SIZE");
            lblLang.Text = Localization.Get("UI_LANG_LABEL");
            btnFormat.Text = Localization.Get("UI_FORMAT");
            btnCreateVhd.Text = Localization.Get("UI_CREATE_VHD");
            lnkWebsite.Text = Localization.Get("UI_WEBSITE");
            lnkAbout.Text = Localization.Get("UI_About");
            lnkGithub.Text = Localization.Get("UI_GITHUB");

            // Improve Refresh Button Icon and Tooltip
            btnRefresh.Text = "⟳"; // Aria anticlockwise arrow
            btnRefresh.Font = new Font("Arial", 12F, FontStyle.Bold);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Size = new Size(34, 28);
            btnRefresh.TextAlign = ContentAlignment.MiddleCenter;
            btnRefresh.Padding = new Padding(0);
            _toolTip.SetToolTip(btnRefresh, Localization.Get("MSG_RefreshTooltip"));
            _toolTip.SetToolTip(lblDataSize, Localization.Get("MSG_DataTooltip"));

            CalculatePartitionSizes();
        }

        private void RefreshDiskList()
        {
            if (!this.IsHandleCreated) return;

            System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    var disks = _diskManager.GetDisks();
                    this.BeginInvoke(new Action(() =>
                    {
                        if (!cmbDisks.IsHandleCreated) return;
                        cmbDisks.Items.Clear();
                        foreach (var disk in disks)
                        {
                            cmbDisks.Items.Add(disk);
                        }
                        if (cmbDisks.Items.Count > 0) cmbDisks.SelectedIndex = 0;
                    }));
                }
                catch { }
            });
        }

        private void cmbDisks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDisks.SelectedItem is DiskItem selectedDisk)
            {
                _selectedDiskSizeGB = selectedDisk.SizeGB;
                
                // Safety: Limit Windows size input based on disk capacity
                // Fix: Adjust Value BEFORE lowering Maximum to avoid WinForms exception
                decimal newMax = Math.Max(1, (decimal)selectedDisk.SizeGB);
                
                _isUpdatingSizes = true; // Block triggers during bounds adjustment
                if (numWindowsSize.Value > newMax)
                    numWindowsSize.Value = newMax;
                
                numWindowsSize.Maximum = Math.Max(numWindowsSize.Minimum, newMax);
                _isUpdatingSizes = false;

                CalculatePartitionSizes();
            }
        }

        private void CalculatePartitionSizes()
        {
            if (_isUpdatingSizes) return;
            if (_selectedDiskSizeGB <= 0)
            {
                lblDataSize.Text = string.Format(Localization.Get("UI_DataSize"), 0);
                return;
            }

            double bootMb = (double)numBootSize.Value;
            double winGb = (double)numWindowsSize.Value;
            double recMb = chkRecovery.Checked ? (double)numRecoverySize.Value : 0;

            double bootGb = bootMb / 1024.0;
            double recGb = recMb / 1024.0;

            // --- Smart Size Balancing ---
            // If total partitions exceed disk size, automatically shrink Windows partition
            double totalRequired = bootGb + winGb + recGb;
            _isUpdatingSizes = true;
            if (totalRequired > _selectedDiskSizeGB)
            {
                double excess = totalRequired - _selectedDiskSizeGB;
                double newWin = winGb - excess;
                
                // Ensure Windows doesn't go below its minimum if possible
                if (newWin < (double)numWindowsSize.Minimum) newWin = (double)numWindowsSize.Minimum;
                
                numWindowsSize.Value = (decimal)newWin;
                winGb = newWin;
            }
            _isUpdatingSizes = false;

            // --- Dynamic Units ---
            lblBootSize.Text = bootMb >= 1024 
                ? Localization.Get("UI_BootSize").Replace("(MB)", "(GB)") 
                : Localization.Get("UI_BootSize").Replace("(GB)", "(MB)");

            string recLabel = Localization.Get("UI_CreateRecovery");
            chkRecovery.Text = recMb >= 1024 
                ? recLabel.Replace("(MB)", "(GB)") 
                : recLabel.Replace("(GB)", "(MB)");

            double remaining = _selectedDiskSizeGB - bootGb - winGb - recGb;
            // If remaining is less than 1GB, DiskManager won't create it, so we show 0.0
            bool isError = remaining < -0.1;
            if (remaining < 1.0) remaining = 0;

            if (isError)
            {
                lblDataSize.Text = Localization.Get("ERR_InsufficientSpace");
                lblDataSize.ForeColor = Color.Red;
                btnFormat.Enabled = false;
            }
            else
            {
                lblDataSize.Text = string.Format(Localization.Get("UI_DataSize"), remaining.ToString("F1"));
                lblDataSize.ForeColor = Color.LimeGreen;
                btnFormat.Enabled = true;
            }
        }

        private void TriggerSizeUpdate(object sender, EventArgs e)
        {
            CalculatePartitionSizes();
        }

        private void chkRecovery_CheckedChanged(object sender, EventArgs e)
        {
            numRecoverySize.Enabled = chkRecovery.Checked;
            CalculatePartitionSizes();
        }

        private void AppendOutput(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            if (txtOutput.InvokeRequired)
            {
                if (txtOutput.IsHandleCreated)
                    txtOutput.BeginInvoke(new Action<string>(AppendOutput), text);
                return;
            }

            if (!txtOutput.IsHandleCreated) return;

            txtOutput.AppendText(text + Environment.NewLine);
            txtOutput.SelectionStart = txtOutput.Text.Length;
            txtOutput.ScrollToCaret();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ResetUI();
        }

        private void ResetUI()
        {
            _isUpdatingSizes = true;
            txtOutput.Clear();
            
            rbGpt.Checked = true;
            numBootSize.Value = 200;
            
            // Fix: Increase Maximum before setting Value
            numWindowsSize.Maximum = 100000; 
            numWindowsSize.Value = 100;
            
            chkRecovery.Checked = false;
            numRecoverySize.Value = 800;
            numVhdSize.Value = 70;
            _isUpdatingSizes = false;

            RefreshDiskList(); 
        }

        private void btnFormat_Click(object sender, EventArgs e)
        {
            if (cmbDisks.SelectedItem is not DiskItem selectedDisk) return;

            if (selectedDisk.IsSystem)
            {
                MessageBox.Show(Localization.Get("ERR_SystemDiskFormat"), Localization.Get("UI_Warning"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            double bootMb = (double)numBootSize.Value;
            double winGb = (double)numWindowsSize.Value;
            double recMb = chkRecovery.Checked ? (double)numRecoverySize.Value : 0;
            double remainingGb = _selectedDiskSizeGB - (bootMb / 1024.0) - winGb - (recMb / 1024.0);
            if (remainingGb < 0) remainingGb = 0;

            // Build detailed confirmation message
            var sb = new System.Text.StringBuilder();
            sb.AppendLine(Localization.Get("CONFIRM_DISK", selectedDisk.Index, selectedDisk.Model));
            sb.AppendLine(Localization.Get("CONFIRM_STYLE", rbGpt.Checked ? "GPT (UEFI)" : "MBR (Legacy)"));
            sb.AppendLine();
            sb.AppendLine(Localization.Get("CONFIRM_BOOT", bootMb >= 1024 ? (bootMb/1024).ToString("F1") + " GB" : bootMb + " MB"));
            sb.AppendLine(Localization.Get("CONFIRM_WINDOWS", winGb + " GB"));
            
            if (chkRecovery.Checked)
                sb.AppendLine(Localization.Get("CONFIRM_RECOVERY", recMb >= 1024 ? (recMb/1024).ToString("F1") + " GB" : recMb + " MB"));
            
            if (remainingGb > 0.1)
                sb.AppendLine(Localization.Get("CONFIRM_DATA", remainingGb.ToString("F1") + " GB"));

            sb.AppendLine();
            sb.AppendLine(Localization.Get("CONFIRM_PROMPT"));

            if (MessageBox.Show(sb.ToString(), Localization.Get("UI_Confirm"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            btnFormat.Enabled = false;
            txtOutput.Clear();

            int bootSizeInt = (int)numBootSize.Value;
            int windowsSizeGbInt = (int)numWindowsSize.Value;
            btnRefresh.Enabled = false;
            txtOutput.Clear();

            System.Threading.Tasks.Task.Run(() =>
            {
                string script = _diskManager.BuildFormatScript(selectedDisk.Index, rbGpt.Checked, (int)numBootSize.Value, (int)numWindowsSize.Value, chkRecovery.Checked, (int)numRecoverySize.Value, _selectedDiskSizeGB);
                bool success = _diskManager.RunDiskpart(script, "FORMAT DISK");
                
                this.Invoke(new Action(() =>
                {
                    btnFormat.Enabled = true;
                    btnRefresh.Enabled = true;
                    if (success)
                    {
                        MessageBox.Show(Localization.Get("MSG_Success"), Localization.Get("UI_Info"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(Localization.Get("MSG_Error"), Localization.Get("UI_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    RefreshDiskList();
                }));
            });
        }

        private void btnCreateVhd_Click(object sender, EventArgs e)
        {
            using SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "VHD Files (*.vhd)|*.vhd|VHDX Files (*.vhdx)|*.vhdx";
            sfd.Title = Localization.Get("UI_SelectVHDPath");

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                long sizeMb = (long)numVhdSize.Value * 1024;
                bool isVhdx = sfd.FileName.EndsWith(".vhdx", StringComparison.OrdinalIgnoreCase);

                System.Threading.Tasks.Task.Run(() =>
                {
                    // If file exists, delete it first because Diskpart 'create vdisk' fails if file exists.
                    // SaveFileDialog already asked for overwrite confirmation.
                    try { if (File.Exists(sfd.FileName)) File.Delete(sfd.FileName); } catch { }

                    string script = _diskManager.BuildVhdScript(sfd.FileName, sizeMb, false, isVhdx);
                    bool success = _diskManager.RunDiskpart(script, "VHD CREATION");
                    
                    this.Invoke(new Action(() =>
                    {
                        if (success)
                        {
                            MessageBox.Show(Localization.Get("MSG_VHDCreated"), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(Localization.Get("MSG_Error"), Localization.Get("UI_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        RefreshDiskList();
                    }));
                });
            }
        }

        private void lnkWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://erturk.netlify.app");
        }

        private void lnkAbout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (AboutDialog dlg = new AboutDialog())
            {
                dlg.ShowDialog();
            }
        }

        private void lnkGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://github.com/abdullah-erturk");
        }

        private void cmbLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            string lang = cmbLang.Text == "Türkçe" ? "tr" : "en";
            Localization.LoadLanguage(lang);
            txtOutput.Clear();
            ApplyLocalization();
            RefreshDiskList();
        }

        private void OpenUrl(string url)
        {
            try { Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }); } catch { }
        }

        private void lblDataSize_Click(object sender, EventArgs e)
        {

        }
    }

    public class AboutDialog : Form
    {
        public AboutDialog()
        {
            this.Text = Localization.Get("UI_About");
            this.Size = new Size(350, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Label lbl = new Label();
            lbl.Text = Localization.Get("MSG_AboutText");
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Dock = DockStyle.Top;
            lbl.Height = 60;

            LinkLabel lnk = new LinkLabel();
            lnk.Text = Localization.Get("UI_BuyMeACoffee");
            lnk.TextAlign = ContentAlignment.MiddleCenter;
            lnk.Dock = DockStyle.Top;
            lnk.Height = 40;
            lnk.LinkClicked += (s, e) => {
                try { Process.Start(new ProcessStartInfo("https://buymeacoffee.com/abdullaherturk") { UseShellExecute = true }); } catch { }
            };

            Button btn = new Button();
            btn.Text = "OK";
            btn.Size = new Size(100, 35);
            btn.Location = new Point((this.ClientSize.Width - btn.Width) / 2, 110);
            btn.Click += (s, e) => { this.Close(); };

            this.Controls.Add(btn);
            this.Controls.Add(lnk);
            this.Controls.Add(lbl);
        }
    }
}
