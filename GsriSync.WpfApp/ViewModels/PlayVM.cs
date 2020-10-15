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

        public ICommand LaunchCommand => new DelegateCommand(Launch);

        public ICommand UninstallCommand => new DelegateAsyncCommand(UninstallAsync);

        public ICommand VocalCommand => new DelegateCommand(Vocal);

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

        private void Launch(object parameter)
        {
            _manifest.Play(
                _registry.Arma3Path,
                _settings.CustomCliArgs);
        }

        private async Task UninstallAsync(object parameter)
        {
            await _manifest.UninstallAsync();
            _navigation.NavigateTo(NavigationService.Pages.Verify);
        }

        private void Vocal(object arg)
        {
            _manifest.Talk();
        }
    }
}
