using GsriSync.WpfApp.Services;
using GsriSync.WpfApp.Utils;
using System;
using System.Windows.Input;

namespace GsriSync.WpfApp.ViewModels
{
    internal class ConfigurationVM : NotifyPropertyChangedBase
    {
        private readonly MainWindowsVM _parent;

        private readonly RegistryService _registry = new RegistryService();

        public string CurrentPath
        {
            get => _registry.CurrentPath;
            set => _registry.CurrentPath = value;
        }

        public string ManifestUrl
        {
            get => _registry.ManifestUrl;
            set => _registry.ManifestUrl = value;
        }

        public ICommand NavigateBackCommand => new DelegateCommand(parameter => _parent.NavigateToVerify());

        public bool StartWithConfig
        {
            get => string.Equals(_registry.StartWithConfig ?? "true", "true", StringComparison.InvariantCultureIgnoreCase);
            set => _registry.StartWithConfig = value ? "true" : "false";
        }

        public ConfigurationVM(MainWindowsVM parent)
        {
            this._parent = parent;
        }
    }
}
