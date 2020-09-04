using Microsoft.Win32;
using System;

namespace GsriSync.WpfApp.Services
{
    internal class RegistryService
    {
        public const string GSRI_PATH = @"Software\GSRI\GsriSync\";

        public string Arma3Path => Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 107410")?.GetValue("InstallLocation")?.ToString();

        public string CurrentPath
        {
            get => this[nameof(CurrentPath)] ?? Environment.ExpandEnvironmentVariables(@"%LOCALAPPDATA%\Gsri\GsriSync");
            set => this[nameof(CurrentPath)] = value;
        }

        public string ManifestUrl
        {
            get => this[nameof(ManifestUrl)] ?? @"https://mods.gsri.team/manifest.json";
            set => this[nameof(ManifestUrl)] = value;
        }

        public string StartWithConfig
        {
            get => this[nameof(StartWithConfig)] ?? "true";
            set => this[nameof(StartWithConfig)] = value;
        }

        private RegistryKey Key
        {
            get
            {
                if (Registry.CurrentUser.OpenSubKey(GSRI_PATH) == null) { Registry.CurrentUser.CreateSubKey(GSRI_PATH); }
                return Registry.CurrentUser.OpenSubKey(GSRI_PATH, true);
            }
        }

        public string this[string index]
        {
            get => Key.GetValue(index)?.ToString();
            set => Key.SetValue(index, value);
        }
    }
}
