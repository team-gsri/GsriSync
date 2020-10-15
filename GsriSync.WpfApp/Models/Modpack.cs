using GsriSync.WpfApp.Events;
using GsriSync.WpfApp.Models.Comparers;
using GsriSync.WpfApp.Services.ManifestProviders;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.Storage;

namespace GsriSync.WpfApp.Models
{
    internal class Modpack : INotifyInstallProgressChanged
    {
        private readonly string _manifest;

        public Manifest Local { get; private set; }

        public Manifest Remote { get; private set; }

        public bool Sync { get; private set; }

        public event InstallProgressChangedEventHandler InstallProgressChanged;

        public Modpack(Manifest local, Manifest remote, string manifest)
        {
            _manifest = manifest;
            Local = local;
            Remote = remote;
            Sync = new ManifestComparer().Equals(local, remote);
        }

        public async Task InstallAsync()
        {
            if (Sync) { return; }

            foreach (var addon in Local.Addons.Except(Remote.Addons, new AddonNameComparer()))
            {
                await addon.DeleteAsync();
            }

            foreach (var addon in Remote.Addons.Except(Local.Addons, new AddonHashComparer()))
            {
                addon.InstallProgressChanged += OnAddonInstallProgress;
                await addon.DownloadAsync();
                addon.InstallProgressChanged -= OnAddonInstallProgress;
            }

            await DownloadManifestAsync();
            Local = Remote;
            Sync = true;
        }

        public async Task UninstallAsync()
        {
            foreach (var addon in Local.Addons)
            {
                await addon.DeleteAsync();
            }
            Local = await new LocalContentReaderProvider().ProvideManifestAsync();
            Sync = false;
        }

        private async Task DownloadManifestAsync()
        {
            var manifest = await ApplicationData.Current.LocalCacheFolder.CreateFileAsync("manifest.json", CreationCollisionOption.ReplaceExisting);
            using var client = new WebClient();
            client.DownloadFile(_manifest, manifest.Path);
        }

        private void OnAddonInstallProgress(object sender, InstallProgressChangedEventArgs e)
        {
            InstallProgressChanged?.Invoke(this, e);
        }
    }
}
