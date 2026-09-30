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
        private string? _persistentWmiError = null;
        private double _lastUserWindowsSizeGb = -1;

        public MainForm()
        {
            Localization.Initialize(); // Init earlier to get system language
            InitializeComponent();
            _diskManager = new DiskManager();
            _diskManager.OnOutputReceived += AppendOutput;
            _toolTip = new ToolTip();

            // Relax Minimums to support smooth typing (enforced at Format time)
            numWindowsSize.Minimum = 1;
            numRecoverySize.Minimum = 1;
            numBootSize.Minimum = 1;

            // Set Icon
            try { this.Icon = new Icon("ico.ico"); } catch { }
            this.Text = "Diskpart GUI v4 | made by Abdullah ERTÜRK";

            // Init Languages Dynamically
            var langs = Localization.GetAvailableLanguages();
            foreach (var lang in langs) cmbLang.Items.Add(lang);
            
            // Set current selection
            for (int i = 0; i < cmbLang.Items.Count; i++)
            {
                if (((LanguageInfo)cmbLang.Items[i]).Code == Localization.CurrentLanguage)
                {
                    cmbLang.SelectedIndex = i;
                    break;
                }
            }
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

            // Remember current selection to restore it after refresh
            int selectedIndexToRestore = -1;
            if (cmbDisks.SelectedItem is DiskItem currentDisk)
            {
                selectedIndexToRestore = currentDisk.Index;
            }

            System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    var disks = _diskManager.GetDisks();
                    
                    // Handle WMI Error Reporting (Only once)
                    if (_diskManager.WmiErrorDetected && string.IsNullOrEmpty(_persistentWmiError))
                    {
                        string msg = Localization.Get("ERR_WmiFailure") + Environment.NewLine + "(Details: " + _diskManager.WmiErrorDetails + ")";
                        AppendOutput(msg);
                    }

                    this.BeginInvoke(new Action(() =>
                    {
                        if (!cmbDisks.IsHandleCreated) return;
                        
                        cmbDisks.Items.Clear();
                        int matchIdx = 0; // Default to first disk if no match
                        
                        for (int i = 0; i < disks.Count; i++)
                        {
                            cmbDisks.Items.Add(disks[i]);
                            if (disks[i].Index == selectedIndexToRestore)
                            {
                                matchIdx = i;
                            }
                        }

                        if (cmbDisks.Items.Count > 0)
                        {
                            cmbDisks.SelectedIndex = matchIdx;
                        }
                    }));
                }
                catch { }
            });
        }

        private void UpdateDiskMap()
        {
            if (_selectedDiskSizeGB <= 0)
            {
                pnlPartBoot.Width = pnlPartWin.Width = pnlPartData.Width = pnlPartRec.Width = 0;
                return;
            }

            double totalW = pnlDiskMap.Width;
            double bootGb = GetCurrentValue(numBootSize) / 1024.0;
            double winGb = GetCurrentValue(numWindowsSize);
            double recGb = chkRecovery.Checked ? GetCurrentValue(numRecoverySize) / 1024.0 : 0;
            
            double remainingGb = _selectedDiskSizeGB - bootGb - winGb - recGb;
            if (remainingGb < 0) remainingGb = 0;

            // --- NEW SMART SCALING LOGIC ---
            // 1. Determine which partitions are active
            bool hasBoot = bootGb > 0;
            bool hasWin = winGb > 0 && (_selectedDiskSizeGB - bootGb > 0.001);
            bool hasData = Math.Round(remainingGb, 1) > 0;
            bool hasRec = chkRecovery.Checked && recGb > 0;
            
            // Adjust hasRec safety: if disk is tiny and boot+win already take 100%, rec can't exist
            if (bootGb + winGb >= _selectedDiskSizeGB - 0.001) hasRec = false;

            // 2. Reserve 25px for each active part
            int minB = hasBoot ? 25 : 0;
            int minW = hasWin ? 25 : 0;
            int minR = hasRec ? 25 : 0;
            int minD = hasData ? 5 : 0;
            
            int reservedAll = minB + minW + minR + minD;
            double expandablePixels = totalW - reservedAll;
            if (expandablePixels < 0) expandablePixels = 0;

            // 3. Proportional Distribution
            double totalGbActivelyUsed = bootGb + winGb + (hasRec ? recGb : 0) + (hasData ? remainingGb : 0);
            if (totalGbActivelyUsed <= 0) totalGbActivelyUsed = 1; // Prevent div by zero

            int calcBootW = minB + (hasBoot ? (int)((bootGb / totalGbActivelyUsed) * expandablePixels) : 0);
            int calcWinW = minW + (hasWin ? (int)((winGb / totalGbActivelyUsed) * expandablePixels) : 0);
            int calcRecW = minR + (hasRec ? (int)((recGb / totalGbActivelyUsed) * expandablePixels) : 0);
            int calcDataW = (int)totalW - calcBootW - calcWinW - calcRecW;
            if (calcDataW < 0) calcDataW = 0;

            // Apply widths and visibility
            pnlPartBoot.Visible = hasBoot;
            pnlPartWin.Visible = hasWin;
            pnlPartData.Visible = hasData; 
            pnlPartRec.Visible = hasRec;

            pnlPartBoot.Width = calcBootW;
            pnlPartWin.Width = calcWinW;
            pnlPartData.Width = calcDataW;
            pnlPartRec.Width = calcRecW;

            // Sequential Stacking
            int currentX = 0;
            if (hasBoot) { pnlPartBoot.Left = currentX; currentX += calcBootW; }
            if (hasWin)  { pnlPartWin.Left = currentX;  currentX += calcWinW;  }
            
            if (hasData) { pnlPartData.Left = currentX; currentX += calcDataW; }
            else { pnlPartData.Width = 0; }

            if (hasRec)  { 
                pnlPartRec.Left = currentX; 
                pnlPartRec.Width = (int)totalW - currentX; 
            }

            pnlPartBoot.BringToFront();
            pnlPartWin.BringToFront();
            pnlPartData.BringToFront();
            pnlPartRec.BringToFront();

            pnlDiskMap.Invalidate(); 
            pnlDiskMap.Update();

            // Tooltips
            double curBoot = GetCurrentValue(numBootSize);
            double curWin = GetCurrentValue(numWindowsSize);
            double curRec = hasRec ? GetCurrentValue(numRecoverySize) : 0;

            _toolTip.SetToolTip(pnlPartBoot, $"{Localization.Get("UI_BootSize").Replace("(MB)", "").Replace(":", "").Trim()}: {curBoot} MB");
            _toolTip.SetToolTip(pnlPartWin, $"{Localization.Get("UI_WindowsSize").Replace("(GB)", "").Replace(":", "").Trim()}: {curWin} GB");
            
            if (hasData)
                _toolTip.SetToolTip(pnlPartData, $"DATA: {remainingGb:F1} GB");
            else
                _toolTip.SetToolTip(pnlPartData, null);

            _toolTip.SetToolTip(pnlPartRec, $"{Localization.Get("UI_CreateRecovery").Replace("(MB)", "").Replace(":", "").Trim()}: {curRec} MB");
        }

        private double GetCurrentValue(NumericUpDown num)
        {
            // Return the raw typed value to prevent WinForms from snapping to Minimum while typing
            if (num != null && !string.IsNullOrEmpty(num.Text))
            {
                if (decimal.TryParse(num.Text, out decimal val))
                {
                    return (double)val;
                }
            }
            return (double)num.Value;
        }

        private void FinalizeMinimums(object sender, EventArgs e)
        {
            if (sender is NumericUpDown num)
            {
                if (num == numBootSize && num.Value < 200) num.Value = 200;
                if (num == numRecoverySize && num.Value < 800) num.Value = 800;
            }
            CalculatePartitionSizes();
        }

        private void CalculatePartitionSizes()
        {
            if (_isUpdatingSizes || _selectedDiskSizeGB <= 0) return;

            try
            {
                _isUpdatingSizes = true;

                double bootMb = GetCurrentValue(numBootSize);
                double winGb = GetCurrentValue(numWindowsSize);
                double recMb = chkRecovery.Checked ? GetCurrentValue(numRecoverySize) : 0;

                double bootGb = bootMb / 1024.0;
                double recGb = recMb / 1024.0;

                // --- Scenario 2: Priority Balancing ---
                // If Recovery is checked, it MUST have space. 
                // We reduce Windows size to accommodate Boot + Recovery.
                double maxAvailableForWin = _selectedDiskSizeGB - bootGb - recGb;
                if (maxAvailableForWin < 0) maxAvailableForWin = 0;

                if (winGb > maxAvailableForWin)
                {
                    winGb = maxAvailableForWin;
                    numWindowsSize.Value = (decimal)winGb;
                    numWindowsSize.Text = winGb.ToString("F1"); // Force visual sync
                }

                double totalRequired = bootGb + winGb + recGb;
                if (totalRequired > _selectedDiskSizeGB)
                {
                    double excess = totalRequired - _selectedDiskSizeGB;
                    double newWin = winGb - excess;
                    
                    if (newWin < (double)numWindowsSize.Minimum) newWin = (double)numWindowsSize.Minimum;
                    
                    numWindowsSize.Value = (decimal)newWin;
                    winGb = newWin;
                }

                // --- Dynamic Units ---
                lblBootSize.Text = bootMb >= 1024 
                    ? Localization.Get("UI_BootSize").Replace("(MB)", "(GB)") 
                    : Localization.Get("UI_BootSize").Replace("(GB)", "(MB)");

                string recLabel = Localization.Get("UI_CreateRecovery");
                chkRecovery.Text = recMb >= 1024 
                    ? recLabel.Replace("(MB)", "(GB)") 
                    : recLabel.Replace("(GB)", "(MB)");

                double remaining = _selectedDiskSizeGB - bootGb - winGb - recGb;
                if (remaining < 0.1) remaining = 0;

                if (remaining < 0 && (bootGb + winGb + recGb) > _selectedDiskSizeGB + 0.01)
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

                UpdateDiskMap();
            }
            finally
            {
                _isUpdatingSizes = false;
            }
        }

        private void TriggerSizeUpdate(object sender, EventArgs e)
        {
            // Scenario 1: Record user's manual input if not currently programmatically updating
            if (!_isUpdatingSizes && sender == numWindowsSize)
            {
                _lastUserWindowsSizeGb = GetCurrentValue(numWindowsSize);
            }
            CalculatePartitionSizes();
        }

        private void chkRecovery_CheckedChanged(object sender, EventArgs e)
        {
            numRecoverySize.Enabled = chkRecovery.Checked;
            
            _isUpdatingSizes = true; // Block triggers during value bump
            if (chkRecovery.Checked)
            {
                if (numRecoverySize.Value < 800) numRecoverySize.Value = 800;
            }
            else
            {
                // Scenario 1: Restore last user-defined size if available, otherwise expand
                double bootGb = GetCurrentValue(numBootSize) / 1024.0;
                double maxPossible = _selectedDiskSizeGB - bootGb;

                if (_lastUserWindowsSizeGb > 0 && _lastUserWindowsSizeGb <= maxPossible)
                {
                    numWindowsSize.Value = (decimal)_lastUserWindowsSizeGb;
                }
                else
                {
                    if (maxPossible > 0) numWindowsSize.Value = (decimal)maxPossible;
                }
            }
            _isUpdatingSizes = false;

            CalculatePartitionSizes();
        }

        private void cmbDisks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDisks.SelectedItem is DiskItem selectedDisk)
            {
                _selectedDiskSizeGB = selectedDisk.SizeGB;
                
                _isUpdatingSizes = true; // Block triggers during bounds adjustment
                
                // Attempt to reset to a healthy default (100 GB) first
                numWindowsSize.Maximum = 100000; 
                numWindowsSize.Value = Math.Min(100, (decimal)selectedDisk.SizeGB);

                // Now enforce the disk-specific maximum
                decimal newMax = Math.Max(1, (decimal)selectedDisk.SizeGB);
                if (numWindowsSize.Value > newMax)
                    numWindowsSize.Value = newMax;
                
                numWindowsSize.Maximum = Math.Max(numWindowsSize.Minimum, newMax);

                // Update Health and Type labels
                lblDiskInfo.Text = $"{Localization.Get("UI_Status")}: {selectedDisk.HealthStatus} | {Localization.Get("UI_Type")}: {selectedDisk.DiskType}";
                lblDiskInfo.ForeColor = selectedDisk.HealthStatus.Equals("OK", StringComparison.OrdinalIgnoreCase) ? Color.Green : Color.Red;
                
                _isUpdatingSizes = false;

                CalculatePartitionSizes();
            }
        }

        private void ClearFocus_Click(object sender, EventArgs e)
        {
            // Forces focus out of NumericUpDown to trigger snap-back and validation
            this.ActiveControl = null;
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

            // Store WMI error for persistence and fixed placement at top
            if (text.Contains(Localization.Get("ERR_WmiFailure")))
            {
                _persistentWmiError = text;
                // Move/Place it to the very top
                string logWithoutWmi = txtOutput.Text.Replace(_persistentWmiError + Environment.NewLine, "");
                txtOutput.Text = _persistentWmiError + Environment.NewLine + logWithoutWmi;
            }
            else
            {
                txtOutput.AppendText(text + Environment.NewLine);
            }

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
            if (!string.IsNullOrEmpty(_persistentWmiError)) 
                txtOutput.AppendText(_persistentWmiError + Environment.NewLine);
            
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
            if (!(cmbDisks.SelectedItem is DiskItem selectedDisk)) return;

            // Final Validation of Minimums (since controls allow 1 during typing)
            double bVal = (double)numBootSize.Value;
            double rVal = chkRecovery.Checked ? (double)numRecoverySize.Value : 0;

            if (bVal < 200)
            {
                MessageBox.Show(this, Localization.Get("UI_Error") + ": Boot size must be >= 200 MB", Localization.Get("UI_Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (chkRecovery.Checked && rVal < 800)
            {
                MessageBox.Show(this, Localization.Get("UI_Error") + ": Recovery size must be >= 800 MB", Localization.Get("UI_Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedDisk.IsSystem)
            {
                MessageBox.Show(this, Localization.Get("ERR_SystemDiskFormat"), Localization.Get("UI_Warning"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            double bootMb = GetCurrentValue(numBootSize);
            double winGb = GetCurrentValue(numWindowsSize);
            double recMb = chkRecovery.Checked ? GetCurrentValue(numRecoverySize) : 0;

            double bootGb = bootMb / 1024.0;
            double recGb = recMb / 1024.0;
            double remainingGb = _selectedDiskSizeGB - bootGb - winGb - recGb;
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

            if (MessageBox.Show(this, sb.ToString(), Localization.Get("UI_Confirm"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            btnFormat.Enabled = false;
            txtOutput.Clear();

            int bootSizeInt = (int)numBootSize.Value;
            int windowsSizeGbInt = (int)numWindowsSize.Value;
            btnRefresh.Enabled = false;
            txtOutput.Clear();
            if (!string.IsNullOrEmpty(_persistentWmiError)) 
            {
                txtOutput.AppendText(_persistentWmiError + Environment.NewLine);
            }

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
                        MessageBox.Show(this, Localization.Get("MSG_Success"), Localization.Get("UI_Info"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(this, Localization.Get("MSG_Error"), Localization.Get("UI_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            MessageBox.Show(this, Localization.Get("MSG_VHDCreated"), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(this, Localization.Get("MSG_Error"), Localization.Get("UI_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        RefreshDiskList();
                    }));
                });
            }
        }

        private void lnkWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://erturk-dev.netlify.app");
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
            if (cmbLang.SelectedItem is LanguageInfo selected)
            {
                Localization.LoadLanguage(selected.Code);
                Localization.SaveLanguagePreference(selected.Code);
            }
            
            txtOutput.Clear();
            if (!string.IsNullOrEmpty(_persistentWmiError)) 
                txtOutput.AppendText(_persistentWmiError + Environment.NewLine);

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
            this.Size = new Size(370, 200);
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
