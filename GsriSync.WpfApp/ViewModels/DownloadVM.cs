using GsriSync.WpfApp.Events;
using GsriSync.WpfApp.Services;
using GsriSync.WpfApp.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GsriSync.WpfApp.ViewModels
{
    internal class DownloadVM : NotifyPropertyChangedBase
    {
        private readonly ManifestService _manifest;

        private readonly NavigationService _navigation;

        private Task running;

        public string Filename { get; set; }

        public string Operation { get; set; }

        public int Progress { get; set; }

        public DownloadVM(
            NavigationService navigation,
            ManifestService manifest)
        {
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));
        }

        public async Task InstallAsync()
        {
            _manifest.InstallProgressChanged += OnDownloadProgress;
            await _manifest.InstallAsync();
            _manifest.InstallProgressChanged -= OnDownloadProgress;
            _navigation.NavigateTo(NavigationService.Pages.Play);
            running = null;
        }

        internal void ScheduleDownload()
        {
            running ??= Task.Factory.StartNew(InstallAsync, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
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
