using GsriSync.WpfApp.Services;
using GsriSync.WpfApp.Utils;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GsriSync.WpfApp.ViewModels
{
    internal class PlayVM : NotifyPropertyChangedBase
    {
        private readonly ManifestService _manifest;

        private readonly NavigationService _navigation;

        private readonly RegistryService _registry;

        private readonly SettingsService _settings;

        private readonly NavigationService navigation;

        public ICommand LaunchCommand => new DelegateAsyncCommand(LaunchAsync);

        public ICommand UninstallCommand => new DelegateAsyncCommand(UninstallAsync);

        public ICommand VocalCommand => new DelegateAsyncCommand(VocalAsync);

        public PlayVM(
            NavigationService navigation,
            ManifestService manifest,
            RegistryService registry,
            SettingsService settings)
        {
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        private async Task LaunchAsync(object parameter)
        {
            var manifest = await _manifest.LocalManifest;
            manifest.Server.ConnectGame(
                _registry.Arma3Path,
                _settings.CustomCliArgs);
        }

        private async Task UninstallAsync(object parameter)
        {
            await _manifest.UninstallAsync();
            _navigation.NavigateTo(NavigationService.Pages.Verify);
        }

        private async Task VocalAsync(object arg)
        {
            var manifest = await _manifest.LocalManifest;
            manifest.Server.ConnectVocal();
        }
    }
}
