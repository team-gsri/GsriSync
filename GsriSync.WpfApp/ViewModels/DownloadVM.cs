using GsriSync.WpfApp.Events;
using GsriSync.WpfApp.Services;
using GsriSync.WpfApp.Utils;
using System.Threading;
using System.Threading.Tasks;

namespace GsriSync.WpfApp.ViewModels
{
    internal class DownloadVM : NotifyPropertyChangedBase
    {
        private readonly MainWindowsVM _parent;

        private readonly SynchronizeService _sync = new SynchronizeService();

        private Task running;

        public string Filename { get; set; }

        public string Operation { get; set; }

        public int Progress { get; set; }

        public DownloadVM(MainWindowsVM parent)
        {
            _parent = parent;
            _sync.InstallProgressChanged += OnDownloadProgress;
        }

        public async Task InstallAsync()
        {
            await _sync.DownloadAsync();
            _parent.NavigateToPlay();
            running = null;
        }

        internal void ScheduleDownload()
        {
            running = running ?? Task.Factory.StartNew(InstallAsync, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void NotifyPropertiesChanged()
        {
            foreach (var name in new[] { nameof(Filename), nameof(Operation), nameof(Progress) })
            {
                NotifyPropertyChanged(name);
            }
        }

        private void OnDownloadProgress(object sender, InstallProgressChangedEventArgs e)
        {
            Operation = e.Stage;
            Filename = e.Filename;
            Progress = e.Progress;
            NotifyPropertiesChanged();
        }
    }
}
