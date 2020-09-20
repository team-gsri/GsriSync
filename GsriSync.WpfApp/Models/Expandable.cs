using GsriSync.WpfApp.Events;
using GsriSync.WpfApp.Services;
using System;
using System.Net;
using System.Threading.Tasks;
using Windows.Storage;

namespace GsriSync.WpfApp.Models
{
    internal abstract class Expandable : INotifyInstallProgressChanged
    {
        public const string STAGE_DOWNLOAD = "Downloading";

        public const string STAGE_EXPAND = "Unzip";

        public string Name { get; set; }

        public string RemoteUrl { get; set; }

        protected virtual string Archive => $"{Name}.zip";

        protected virtual string ExpandPath => $"{Name}";

        public event InstallProgressChangedEventHandler InstallProgressChanged;

        public virtual async Task InstallAsync()
        {
            await DownloadAsync();
            await ExpandAsync();
            var archive = await ApplicationData.Current.TemporaryFolder.GetFileAsync(Archive);
            await archive.DeleteAsync();
        }

        public virtual async Task UninstallAsync()
        {
            var directory = await GetOrMakeExpandFolderAsync(ExpandPath);
            await directory.DeleteAsync();
        }

        protected async virtual Task<StorageFolder> GetOrMakeExpandFolderAsync(string expandPath)
        {
            return await ApplicationData.Current.LocalCacheFolder.CreateFolderAsync($"{Name}", CreationCollisionOption.ReplaceExisting);
        }

        private async Task DownloadAsync()
        {
            NotifyInstallProgressChanged(STAGE_DOWNLOAD, 0);
            using (var client = new WebClient())
            {
                client.DownloadProgressChanged += (sender, e) => NotifyInstallProgressChanged(STAGE_DOWNLOAD, e.ProgressPercentage);
                var archive = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(Archive, CreationCollisionOption.ReplaceExisting);
                await client.DownloadFileTaskAsync(this.RemoteUrl, archive.Path);
            }
        }

        private async Task ExpandAsync()
        {
            NotifyInstallProgressChanged(STAGE_EXPAND, 0);
            using (var client = new ZipClient())
            {
                client.UnzipProgressChanged += (sender, e) => NotifyInstallProgressChanged(STAGE_EXPAND, e.ProgressPercentage);
                var archive = await ApplicationData.Current.TemporaryFolder.GetFileAsync(Archive);
                var expand = await GetOrMakeExpandFolderAsync(ExpandPath);
                await client.UnzipArchiveTaskAsync(archive.Path, expand.Path);
            }
        }

        private void NotifyInstallProgressChanged(string stage, int progress)
        {
            InstallProgressChanged?.Invoke(this, new InstallProgressChangedEventArgs(Name, stage, progress));
        }
    }
}
