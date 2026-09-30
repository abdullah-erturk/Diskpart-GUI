using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Globalization;

namespace DiskpartGUI
{
    public static class Localization
    {
        private static Dictionary<string, string> _texts = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public static string CurrentLanguage { get; private set; } = "en";

        public static void Initialize()
        {
            string culture = "";
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lang", "config.txt");
            try
            {
                if (File.Exists(configPath))
                    culture = File.ReadAllText(configPath).Trim().ToLower();
            }
            catch { }

            if (string.IsNullOrEmpty(culture))
            {
                // Detect system language
                culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
            }
            
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lang", culture + ".ini");
            
            if (File.Exists(path))
            {
                LoadLanguage(culture);
            }
            else
            {
                // Fallback to English if system language is not supported
                LoadLanguage("en");
            }
        }

        public static void LoadLanguage(string langCode)
        {
            CurrentLanguage = langCode;
            _texts.Clear();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lang", langCode + ".ini");
            if (!File.Exists(path)) return;

            try
            {
                var lines = File.ReadAllLines(path);
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";") || line.StartsWith("#")) continue;
                    var parts = line.Split(new[] { '=' }, 2);
                    if (parts.Length == 2)
                    {
                        _texts[parts[0].Trim()] = parts[1].Trim();
                    }
                }
            }
            catch { }
        }

        public static void SaveLanguagePreference(string langCode)
        {
            try
            {
                string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lang", "config.txt");
                File.WriteAllText(configPath, langCode);
            }
            catch { }
        }

        public static string Get(string key)
        {
            if (_texts.TryGetValue(key, out string? value) && value != null) 
                return value.Replace("\\n", Environment.NewLine);
            return key;
        }

        public static string Get(string key, params object[] args)
        {
            string fmt = Get(key);
            try { return string.Format(fmt, args); } catch { return fmt; }
        }

        public static List<LanguageInfo> GetAvailableLanguages()
        {
            var list = new List<LanguageInfo>();
            string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lang");
            if (!Directory.Exists(folder)) return list;

            foreach (var file in Directory.GetFiles(folder, "*.ini"))
            {
                string code = Path.GetFileNameWithoutExtension(file).ToLower();
                string name = GetLangNameFromFile(file) ?? code.ToUpper();
                list.Add(new LanguageInfo { Code = code, Name = name });
            }
            return list.OrderBy(l => l.Name).ToList();
        }

        private static string? GetLangNameFromFile(string path)
        {
            try
            {
                var lines = File.ReadAllLines(path);
                foreach (var line in lines)
                {
                    if (line.StartsWith("UI_LANG_NAME", StringComparison.OrdinalIgnoreCase))
                    {
                        var parts = line.Split(new[] { '=' }, 2);
                        if (parts.Length == 2) return parts[1].Trim();
                    }
                }
            }
            catch { }
            return null;
        }
    }

    public class LanguageInfo
    {
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public override string ToString() => Name;
        public override bool Equals(object? obj) => obj is LanguageInfo other && other.Code == this.Code;
        public override int GetHashCode() => Code.GetHashCode();
    }
}
