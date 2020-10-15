using GsriSync.WpfApp.Services;
using GsriSync.WpfApp.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GsriSync.WpfApp.ViewModels
{
    internal class InstallVM : NotifyPropertyChangedBase
    {
        private readonly ManifestService _manifest;

        private readonly NavigationService _navigation;

        private Task running;

        public ICommand InstallCommand => new DelegateCommand(InstallAction);

        public ICommand UninstallCommand => new DelegateAsyncCommand(UninstallAsync);

        public Visibility UninstallVisibility { get; set; } = Visibility.Hidden;

        public InstallVM(
            NavigationService navigation,
            ManifestService manifest)
        {
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));
            ScheduleVerify();
        }

        internal void ScheduleVerify()
        {
            running ??= Task.Factory.StartNew(VerifyIsInstalled, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void InstallAction(object obj)
        {
            _navigation.NavigateTo(NavigationService.Pages.Download);
        }

        private async Task UninstallAsync(object parameter)
        {
            await _manifest.UninstallAsync();
            UninstallVisibility = Visibility.Hidden;
            NotifyPropertyChanged(nameof(UninstallVisibility));
        }

        private void VerifyIsInstalled()
        {
            UninstallVisibility = _manifest.IsInstalled ? Visibility.Visible : Visibility.Hidden;
            NotifyPropertyChanged(nameof(UninstallVisibility));
        }
    }
}
