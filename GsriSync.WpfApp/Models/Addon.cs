using GsriSync.WpfApp.Events;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Windows.Storage;

namespace GsriSync.WpfApp.Models
{
    public class Addon : INotifyInstallProgressChanged
    {
        private const string STAGE_DOWNLOAD = "Downloading";

        public string Hash { get; set; }

        public string Name { get; set; }

        public string RemoteUrl { get; set; }

        public ulong Size { get; set; }

        public event InstallProgressChangedEventHandler InstallProgressChanged;

        public async Task DeleteAsync()
        {
            try
            {
                var file = await ApplicationData.Current.LocalCacheFolder.GetFileAsync(Name);
                await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            catch (FileNotFoundException)
            {
                // File was already deleted, ignore exception
            }
        }

        public async Task DownloadAsync()
        {
            var file = await ApplicationData.Current.LocalCacheFolder.CreateFileAsync(Name, CreationCollisionOption.ReplaceExisting);
            NotifyInstallProgressChanged(STAGE_DOWNLOAD, 0);
            using var client = new WebClient();
            client.DownloadProgressChanged += (sender, e) => NotifyInstallProgressChanged(STAGE_DOWNLOAD, e.ProgressPercentage);
            await client.DownloadFileTaskAsync(RemoteUrl, file.Path);
        }

        private void NotifyInstallProgressChanged(string stage, int progress)
        {
            InstallProgressChanged?.Invoke(this, new InstallProgressChangedEventArgs(Name, stage, progress));
        }
    }
}
