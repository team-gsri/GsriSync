using GsriSync.WpfApp.Services;
using GsriSync.WpfApp.Utils;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static GsriSync.WpfApp.Events.NavigationChangedEventArgs;

namespace GsriSync.WpfApp.ViewModels
{
    internal class PlayVM : NotifyPropertyChangedBase
    {
        private readonly ManifestService _manifest;

        private readonly NavigationService _navigation;

        public ICommand InstallCommand => new DelegateCommand(Install);

        public Visibility InstallCommandVisibility { get; private set; }

        public ICommand PlayCommand => new DelegateCommand(Play);

        public Visibility PlayCommandVisibility { get; private set; }

        public ICommand UninstallCommand => new DelegateAsyncCommand(UninstallAsync);

        public Visibility UninstallCommandVisibility { get; private set; }

        public ICommand VocalCommand => new DelegateCommand(Vocal);

        public Visibility VocalCommandVisibility { get; private set; }

        public PlayVM(
            NavigationService navigation,
            ManifestService manifest)
        {
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));
        }

        public void RefreshCommandVisibility()
        {
            InstallCommandVisibility = _manifest.IsSync ? Visibility.Collapsed : Visibility.Visible;
            UninstallCommandVisibility = _manifest.IsInstalled ? Visibility.Visible : Visibility.Collapsed;
            PlayCommandVisibility = _manifest.IsSync ? Visibility.Visible : Visibility.Collapsed;
            VocalCommandVisibility = _manifest.IsSync ? Visibility.Visible : Visibility.Collapsed;
            foreach (var name in new[] { nameof(InstallCommandVisibility), nameof(UninstallCommandVisibility), nameof(PlayCommandVisibility), nameof(VocalCommandVisibility) })
            {
                NotifyPropertyChanged(name);
            }
        }

        private void Install(object obj)
        {
            if (_manifest.IsSync) { return; }
            _navigation.NavigateTo(Pages.Download);
        }

        private void Play(object parameter)
        {
            if (!_manifest.IsSync) { return; }
            _manifest.Play();
        }

        private async Task UninstallAsync(object parameter)
        {
            if (!_manifest.IsInstalled) { return; }
            try
            {
                await _manifest.UninstallAsync();
            }
            finally
            {
                RefreshCommandVisibility();
            }
        }

        private void Vocal(object arg)
        {
            if (!_manifest.IsSync) { return; }
            _manifest.Talk();
        }
    }
}
