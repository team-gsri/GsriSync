using GsriSync.WpfApp.Events;
using System.Threading.Tasks;

namespace GsriSync.WpfApp.Services
{
    internal class SynchronizeService : INotifyInstallProgressChanged
    {
        private readonly ManifestService _manifest = new ManifestService();

        public event InstallProgressChangedEventHandler InstallProgressChanged;

        public async Task DownloadAsync()
        {
            var remote = await _manifest.ReadRemoteManifestAsync();
            remote.InstallProgressChanged += this.ManifestInstallProgressChanged;
            await remote.InstallAsync();
            await _manifest.DownloadRemoteManifestAsync();
            remote.InstallProgressChanged -= this.ManifestInstallProgressChanged;
        }

        public async Task RemoveAsync()
        {
            var manifest = await _manifest.ReadLocalManifestAsync();
            manifest.Delete();
        }

        internal async Task<bool> IsInstalledAsync()
        {
            var local = await _manifest.ReadLocalManifestAsync();
            return local.LastModification != default;
        }

        internal async Task<bool> IsSynchronizedAsync()
        {
            var local = await _manifest.ReadLocalManifestAsync();
            var remote = await _manifest.ReadRemoteManifestAsync();
            return Equals(local.LastModification, remote.LastModification);
        }

        private void ManifestInstallProgressChanged(object sender, InstallProgressChangedEventArgs e)
        {
            InstallProgressChanged?.Invoke(this, e);
        }
    }
}
