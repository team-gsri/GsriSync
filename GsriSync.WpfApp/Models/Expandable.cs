using GsriSync.WpfApp.Events;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace GsriSync.WpfApp.Models
{
    internal abstract class Expandable : INotifyInstallProgressChanged
    {
        public const string STAGE_DOWNLOAD = "Downloading";

        public const string STAGE_EXPAND = "Unzip";

        public abstract string LocalExpandPath { get; }

        public string Name { get; set; }

        public string RemoteUrl { get; set; }

        protected abstract string LocalDownloadPath { get; }

        private string LocalArchiveFilename => $@"{Name}.zip";

        private string LocalArchivePath => $@"{LocalDownloadPath}\{LocalArchiveFilename}";

        public event InstallProgressChangedEventHandler InstallProgressChanged;

        public virtual void Delete()
        {
            if (!Directory.Exists(LocalExpandPath)) return;
            Directory.Delete(LocalExpandPath, true);
        }

        public virtual async Task InstallAsync()
        {
            await DownloadAsync();
            await ExpandAsync();
            File.Delete(LocalArchivePath);
        }

        private async Task DownloadAsync()
        {
            InstallProgressChanged?.Invoke(this, new InstallProgressChangedEventArgs(LocalArchiveFilename, STAGE_DOWNLOAD, 0));
            MakeDirectoryIfNotExists(LocalDownloadPath);
            using (var client = new WebClient())
            {
                client.DownloadProgressChanged += this.DownloadProgressChanged;
                await client.DownloadFileTaskAsync(this.RemoteUrl, LocalArchivePath);
            }
        }

        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var filename = Path.GetFileName(LocalArchivePath);
            var arguments = new InstallProgressChangedEventArgs(filename, STAGE_DOWNLOAD, e.ProgressPercentage);
            InstallProgressChanged?.Invoke(this, arguments);
        }

        private async Task ExpandAsync()
        {
            long current = 0;
            InstallProgressChanged?.Invoke(this, new InstallProgressChangedEventArgs(LocalArchiveFilename, STAGE_EXPAND, 0));
            MakeDirectoryIfNotExists(LocalExpandPath);
            using (var archive = ZipFile.OpenRead(LocalArchivePath))
            {
                var total = archive.Entries.Sum(entry => entry.CompressedLength);
                foreach (var entry in archive.Entries)
                {
                    current += entry.CompressedLength;
                    if (string.Empty.Equals(entry.Name)) { continue; }
                    var file_path = $@"{LocalExpandPath}\{entry.FullName}";
                    MakeDirectoryIfNotExists(Path.GetDirectoryName(file_path));
                    await Task.Factory.StartNew(() => entry.ExtractToFile(file_path, true));
                    var progress = 100 * current / total;
                    InstallProgressChanged?.Invoke(this, new InstallProgressChangedEventArgs(LocalArchiveFilename, STAGE_EXPAND, (int)progress));
                }
            }
        }

        private void MakeDirectoryIfNotExists(string directory)
        {
            if (Directory.Exists(directory)) return;
            Directory.CreateDirectory(directory);
        }
    }
}
