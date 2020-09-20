using GsriSync.WpfApp.Services;
using GsriSync.WpfApp.Utils;
using System;
using System.Windows.Input;

namespace GsriSync.WpfApp.ViewModels
{
    internal class ConfigurationVM : NotifyPropertyChangedBase
    {
        private readonly NavigationService _navigation;

        private readonly SettingsService _settings;

        public string CustomCliArgs
        {
            get => _settings.CustomCliArgs;
            set => _settings.CustomCliArgs = value;
        }

        public string LocalDataPath
        {
            get => _settings.LocalDataPath;
            set => _settings.LocalDataPath = value;
        }

        public string ManifestUrl
        {
            get => _settings.ManifestUrl;
            set => _settings.ManifestUrl = value;
        }

        public ICommand NavigateBackCommand => new DelegateCommand(parameter => _navigation.NavigateBack());

        public bool StartWithConfig
        {
            get => string.Equals(_settings.StartWithConfig ?? "true", "true", StringComparison.InvariantCultureIgnoreCase);
            set => _settings.StartWithConfig = value ? "true" : "false";
        }

        public ConfigurationVM(
            NavigationService navigation,
            SettingsService settings)
        {
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }
    }
}
