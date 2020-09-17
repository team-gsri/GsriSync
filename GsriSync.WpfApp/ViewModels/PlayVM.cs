using GsriSync.WpfApp.Services;
using GsriSync.WpfApp.Utils;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GsriSync.WpfApp.ViewModels
{
    internal class PlayVM : NotifyPropertyChangedBase
    {
        private readonly ManifestService _manifest = new ManifestService();

        private readonly MainWindowsVM _parent;

        private readonly SynchronizeService _sync = new SynchronizeService();

        public ICommand LaunchCommand => new DelegateAsyncCommand(LaunchAsync);

        public ICommand UninstallCommand => new DelegateAsyncCommand(UninstallAsync);

        public ICommand VocalCommand => new DelegateAsyncCommand(VocalAsync);

        public PlayVM(MainWindowsVM parent)
        {
            _parent = parent;
        }

        private async Task LaunchAsync(object parameter)
        {
            var manifest = await _manifest.ReadLocalManifestAsync();
            manifest.Launch();
        }

        private async Task UninstallAsync(object parameter)
        {
            await _sync.RemoveAsync();
            _parent.NavigateToVerify();
        }

        private async Task VocalAsync(object arg)
        {
            var manifest = await _manifest.ReadLocalManifestAsync();
            manifest.ConnectVocal();
        }
    }
}
