using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DiskpartGUI
{
    public class DiskItem
    {
        public int Index { get; set; }
        public string Model { get; set; } = "";
        public long SizeBytes { get; set; }
        public double SizeGB { get; set; }
        public bool IsSystem { get; set; }
        public string HealthStatus { get; set; } = "OK";
        public string DiskType { get; set; } = "HDD"; // Default

        public string SizeDisplay => SizeGB.ToString("F1") + " GB";
        public override string ToString() => $"Disk {Index}: {Model} ({DiskType}) - {SizeDisplay} {(IsSystem ? "[SYSTEM]" : "")}";
    }

    public class DiskManager
    {
        public event Action<string>? OnOutputReceived;
        public bool WmiErrorDetected { get; private set; }
        public string? WmiErrorDetails { get; private set; }

        public List<DiskItem> GetDisks()
        {
            try
            {
                // Try WMI first (more detailed)
                var disks = GetDisksViaWmi();
                WmiErrorDetected = false; // Reset if it succeeds now
                return disks;
            }
            catch (Exception ex)
            {
                WmiErrorDetected = true;
                WmiErrorDetails = ex.Message;
                return GetDisksViaDiskpart();
            }
        }

        private List<DiskItem> GetDisksViaWmi()
        {
            var disks = new List<DiskItem>();
            int systemDiskIndex = GetSystemDiskIndex();
            
            // Get physical disk info for Type (SSD/HDD)
            var typeMap = new Dictionary<int, string>();
            try
            {
                using var storageSearcher = new ManagementObjectSearcher(@"Root\Microsoft\Windows\Storage", "SELECT DeviceId, MediaType, BusType FROM MSFT_PhysicalDisk");
                foreach (ManagementObject obj in storageSearcher.Get())
                {
                    int devId = Convert.ToInt32(obj["DeviceId"]);
                    int mediaType = Convert.ToInt32(obj["MediaType"]);
                    int busType = Convert.ToInt32(obj["BusType"]);
                    
                    string type = "HDD";
                    if (busType == 17) type = "NVMe";
                    else if (mediaType == 4) type = "SSD";
                    
                    typeMap[devId] = type;
                }
            } catch { }

            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            foreach (ManagementObject drive in searcher.Get())
            {
                int index = Convert.ToInt32(drive["Index"]);
                long sizeBytes = Convert.ToInt64(drive["Size"]);
                string model = drive["Model"]?.ToString() ?? "Unknown";
                string status = drive["Status"]?.ToString() ?? "OK";

                // Infer type if storage query failed
                string diskType = typeMap.ContainsKey(index) ? typeMap[index] : "HDD";
                if (diskType == "HDD")
                {
                    if (model.IndexOf("NVMe", StringComparison.OrdinalIgnoreCase) >= 0) diskType = "NVMe";
                    else if (model.IndexOf("SSD", StringComparison.OrdinalIgnoreCase) >= 0) diskType = "SSD";
                }

                disks.Add(new DiskItem
                {
                    Index = index,
                    Model = model,
                    SizeBytes = sizeBytes,
                    SizeGB = sizeBytes / (1024.0 * 1024.0 * 1024.0),
                    IsSystem = (index == systemDiskIndex),
                    HealthStatus = status,
                    DiskType = diskType
                });
            }
            return disks.OrderBy(d => d.Index).ToList();
        }

        private List<DiskItem> GetDisksViaDiskpart()
        {
            var disks = new List<DiskItem>();
            string output = RunDiskpartCommand("list disk");
            int systemDiskIndex = GetSystemDiskIndexViaDiskpart();

            // Parse lines like: Disk 0    Online          119 GB      0 B
            var lines = output.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                // Support decimals like 0.9
                var match = Regex.Match(line, @"Disk\s+(\d+)\s+.*\s+([\d\.]+)\s+(GB|MB|TB)");
                if (match.Success)
                {
                    int index = int.Parse(match.Groups[1].Value);
                    double size = double.Parse(match.Groups[2].Value, System.Globalization.CultureInfo.InvariantCulture);
                    string unit = match.Groups[3].Value;

                    if (unit == "MB") size /= 1024.0;
                    else if (unit == "TB") size *= 1024.0;

                    disks.Add(new DiskItem
                    {
                        Index = index,
                        Model = $"Disk {index} (Fallback)",
                        SizeGB = size,
                        SizeBytes = (long)(size * 1024.0 * 1024.0 * 1024.0),
                        IsSystem = (index == systemDiskIndex),
                        HealthStatus = "OK",
                        DiskType = "Unknown"
                    });
                }
            }
            return disks.OrderBy(d => d.Index).ToList();
        }

        public int GetSystemDiskIndex()
        {
            try { return GetSystemDiskIndexViaWmi(); }
            catch { return GetSystemDiskIndexViaDiskpart(); }
        }

        private int GetSystemDiskIndexViaWmi()
        {
            string systemDrive = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)) ?? "C:\\";
            systemDrive = systemDrive.TrimEnd('\\');

            using var partitionSearcher = new ManagementObjectSearcher($"ASSOCIATORS OF {{Win32_LogicalDisk.DeviceID='{systemDrive}'}} WHERE AssocClass = Win32_LogicalDiskToPartition");
            foreach (ManagementObject partition in partitionSearcher.Get())
            {
                using var driveSearcher = new ManagementObjectSearcher($"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partition["DeviceID"]}'}} WHERE AssocClass = Win32_DiskDriveToDiskPartition");
                foreach (ManagementObject drive in driveSearcher.Get())
                {
                    return Convert.ToInt32(drive["Index"]);
                }
            }
            return -1;
        }

        private int GetSystemDiskIndexViaDiskpart()
        {
            try
            {
                string systemDrive = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)) ?? "C:";
                char systemLetter = systemDrive.ToUpper()[0];

                // Check each disk to see if it contains the volume with the system letter
                string listDiskOut = RunDiskpartCommand("list disk");
                var diskMatches = Regex.Matches(listDiskOut, @"Disk\s+(\d+)");
                
                foreach (Match m in diskMatches)
                {
                    int idx = int.Parse(m.Groups[1].Value);
                    string detail = RunDiskpartCommand($"select disk {idx}\nlist volume");
                    if (detail.Contains($"  {systemLetter}  ")) return idx;
                }
            }
            catch { }
            return -1;
        }

        private string RunDiskpartCommand(string command)
        {
            string scriptPath = Path.Combine(Path.GetTempPath(), "dp_temp.txt");
            File.WriteAllText(scriptPath, command);

            Process process = new Process();
            process.StartInfo.FileName = "diskpart.exe";
            process.StartInfo.Arguments = $"/s \"{scriptPath}\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            
            if (File.Exists(scriptPath)) File.Delete(scriptPath);
            return output;
        }

        public bool RunDiskpart(string script, string label)
        {
            OnOutputReceived?.Invoke($"--- {label} ---");
            string scriptPath = Path.Combine(Path.GetTempPath(), "dp_script.txt");
            File.WriteAllText(scriptPath, script);

            Process process = new Process();
            process.StartInfo.FileName = "diskpart.exe";
            process.StartInfo.Arguments = $"/s \"{scriptPath}\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.OutputDataReceived += (s, e) => { if (e.Data != null) OnOutputReceived?.Invoke(e.Data); };
            process.ErrorDataReceived += (s, e) => { if (e.Data != null) OnOutputReceived?.Invoke("ERROR: " + e.Data); };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            if (File.Exists(scriptPath)) File.Delete(scriptPath);
            bool success = process.ExitCode == 0;
            OnOutputReceived?.Invoke($"--- {label} {(success ? "SUCCESS" : "FAILED")} ---");
            return success;
        }

        public List<char> GetDiskDriveLetters(int diskIndex)
        {
            var letters = new List<char>();
            try
            {
                using var partitionSearcher = new ManagementObjectSearcher($"ASSOCIATORS OF {{Win32_DiskDrive.DeviceID='\\\\.\\PHYSICALDRIVE{diskIndex}'}} WHERE AssocClass = Win32_DiskDriveToDiskPartition");
                foreach (ManagementObject partition in partitionSearcher.Get())
                {
                    using var logicalSearcher = new ManagementObjectSearcher($"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partition["DeviceID"]}'}} WHERE AssocClass = Win32_LogicalDiskToPartition");
                    foreach (ManagementObject logical in logicalSearcher.Get())
                    {
                        string driveLetter = logical["DeviceID"]?.ToString() ?? "";
                        if (!string.IsNullOrEmpty(driveLetter))
                        {
                            letters.Add(char.ToUpper(driveLetter[0]));
                        }
                    }
                }
            }
            catch { }
            return letters;
        }

        public char? GetAvailableLetter(char preferred = ' ', List<char>? exemptLetters = null)
        {
            try
            {
                var usedLetters = DriveInfo.GetDrives().Select(d => char.ToUpper(d.Name[0])).ToList();
                
                // If preferred letter is in exempt list (it belongs to the current disk), consider it free!
                if (preferred != ' ' && exemptLetters != null && exemptLetters.Contains(char.ToUpper(preferred)))
                    return char.ToUpper(preferred);

                if (preferred != ' ' && !usedLetters.Contains(char.ToUpper(preferred))) 
                    return char.ToUpper(preferred);

                for (char c = 'C'; c <= 'Z'; c++)
                {
                    if (!usedLetters.Contains(c)) return c;
                }
            }
            catch { }
            return null;
        }

        public string BuildFormatScript(int diskIndex, bool isGpt, int bootSize, int windowsSizeGb, bool createRecovery, int recoverySizeMb, double totalDiskSizeGB)
        {
            var sw = new StringWriter();
            sw.WriteLine($"select disk {diskIndex}");
            sw.WriteLine("clean");

            double totalMb = totalDiskSizeGB * 1024.0;
            double bootMb = bootSize;
            double winMb = windowsSizeGb * 1024.0;
            double recMb = createRecovery ? recoverySizeMb : 0;
            
            // Check if we have enough space for a DATA partition (at least 1GB)
            double usedMbExceptData = bootMb + winMb + recMb;
            bool spaceForData = (totalMb - usedMbExceptData) >= 1024;
            
            // Layout Order: 1. Boot, 2. OS, 3. DATA (if exists), 4. Recovery (if exists)

            if (isGpt) sw.WriteLine("convert gpt");
            else sw.WriteLine("convert mbr");

            // --- 1. BOOT SECTION (Always Fixed) ---
            sw.WriteLine($"create partition primary size={bootSize}");
            if (isGpt)
            {
                sw.WriteLine("format quick fs=fat32 label=\"EFI_BOOT\"");
                sw.WriteLine("set id=\"c12a7328-f81f-11d2-ba4b-00a0c93ec93b\" override");
                sw.WriteLine("gpt attributes=0x8000000000000001");
            }
            else
            {
                sw.WriteLine("format quick fs=ntfs label=\"MBR_BOOT\"");
                sw.WriteLine("active");
            }

            // --- 2. WINDOWS SECTION ---
            // If it's the LAST partition, don't specify size
            bool isWindowsLast = !spaceForData && !createRecovery;
            if (isWindowsLast) sw.WriteLine("create partition primary");
            else sw.WriteLine($"create partition primary size={(int)winMb}");
            
            sw.WriteLine("format quick fs=ntfs label=\"Windows\"");
            var currentDiskLetters = GetDiskDriveLetters(diskIndex);
            char? winLetter = GetAvailableLetter('C', currentDiskLetters);
            if (winLetter != null) sw.WriteLine($"assign letter={winLetter}");
            else sw.WriteLine("assign");

            // --- 3. DATA SECTION ---
            if (spaceForData)
            {
                bool isDataLast = !createRecovery;
                if (isDataLast)
                {
                    sw.WriteLine("create partition primary");
                }
                else
                {
                    // Fixed size DATA. Subtract padding (10MB) to ensure room for Recovery at the end
                    double dataMb = totalMb - usedMbExceptData - 10;
                    sw.WriteLine($"create partition primary size={(int)dataMb}");
                }
                sw.WriteLine("format quick fs=ntfs label=\"DATA\"");
                sw.WriteLine("assign");
            }

            // --- 4. RECOVERY SECTION ---
            if (createRecovery)
            {
                // Recovery is ALWAYS last in this new layout
                sw.WriteLine("create partition primary");
                sw.WriteLine("format quick fs=ntfs label=\"Recovery\"");
                if (isGpt)
                {
                    sw.WriteLine("set id=\"de94bba4-06d1-4d40-a16a-bfd50179d6ac\" override");
                    sw.WriteLine("gpt attributes=0x8000000000000001");
                }
                else
                {
                    sw.WriteLine("set id=27 override");
                }
            }


            sw.WriteLine("rescan");
            return sw.ToString();
        }

        public string BuildVhdScript(string vhdPath, long sizeMb, bool isFixed, bool isVhdx)
        {
            // Dynamic (expandable) is preferred as per user request. 
            string type = isFixed ? "fixed" : "expandable";
            return $"create vdisk file=\"{vhdPath}\" maximum={sizeMb} type={type}\nattach vdisk";
        }
    }
}
