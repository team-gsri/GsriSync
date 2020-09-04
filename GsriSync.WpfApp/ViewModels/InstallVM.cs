using GsriSync.WpfApp.Services;
using GsriSync.WpfApp.Utils;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GsriSync.WpfApp.ViewModels
{
    internal class InstallVM : NotifyPropertyChangedBase
    {
        private readonly MainWindowsVM _parent;

        private readonly SynchronizeService _sync = new SynchronizeService();

        public ICommand InstallCommand => new DelegateCommand(InstallAction);

        public ICommand UninstallCommand => new DelegateAsyncCommand(UninstallAsync);

        public Visibility UninstallVisibility { get; set; } = Visibility.Hidden;
        private Task running;
        public InstallVM(MainWindowsVM parent)
        {
            _parent = parent;
            ScheduleVerify();
        }

        private void InstallAction(object obj)
        {
            _parent.NavigateToDownload();
        }

        private async Task UninstallAsync(object parameter)
        {
            await _sync.RemoveAsync();
            UninstallVisibility = Visibility.Hidden;
            NotifyPropertyChanged(nameof(UninstallVisibility));
        }

        private async Task VerifyIsInstalledAsync()
        {
            UninstallVisibility = await _sync.IsInstalledAsync() ? Visibility.Visible : Visibility.Hidden;
            NotifyPropertyChanged(nameof(UninstallVisibility));
        }

        internal void ScheduleVerify()
        {
            running = running ?? Task.Factory.StartNew(VerifyIsInstalledAsync, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
