using System;
using Windows.Storage;

namespace GsriSync.WpfApp.Services
{
    internal class SettingsService
    {
        private const string DEFAULT_PATH = @"%LOCALAPPDATA%\Gsri\GsriSync";

        private const string DEFAULT_URL = "https://mods.gsri.team/manifest.json";

        public string CustomCliArgs
        {
            get => ApplicationData.Current.RoamingSettings.Values[nameof(CustomCliArgs)]?.ToString()
                ?? string.Empty;

            set => ApplicationData.Current.RoamingSettings.Values[nameof(CustomCliArgs)] = value;
        }

        public string LocalDataPath
        {
            get => ApplicationData.Current.LocalSettings.Values[nameof(LocalDataPath)]?.ToString()
                ?? Environment.ExpandEnvironmentVariables(DEFAULT_PATH);

            set => ApplicationData.Current.LocalSettings.Values[nameof(LocalDataPath)] = value;
        }

        public string ManifestUrl
        {
            get => ApplicationData.Current.RoamingSettings.Values[nameof(ManifestUrl)]?.ToString()
                ?? DEFAULT_URL;

            set => ApplicationData.Current.RoamingSettings.Values[nameof(ManifestUrl)] = value;
        }

        public string StartWithConfig
        {
            get => ApplicationData.Current.RoamingSettings.Values[nameof(StartWithConfig)]?.ToString()
                ?? "true";

            set => ApplicationData.Current.RoamingSettings.Values[nameof(StartWithConfig)] = value;
        }
    }
}
