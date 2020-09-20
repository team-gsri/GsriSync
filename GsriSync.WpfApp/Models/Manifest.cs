using GsriSync.WpfApp.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace GsriSync.WpfApp.Models
{
    internal class Manifest : INotifyInstallProgressChanged
    {
        public IEnumerable<Addon> Addons { get; set; } = new Addon[0];

        public bool IsInstalled => LastModification != default;

        public DateTimeOffset LastModification { get; set; }

        public Server Server { get; set; }

        public Teamspeak Teamspeak { get; set; }

        public event InstallProgressChangedEventHandler InstallProgressChanged;

        public async Task InstallAsync()
        {
            foreach (var addon in Addons.OfType<Expandable>().Append(Teamspeak))
            {
                addon.InstallProgressChanged += OnAddonInstallProgress;
                await addon.InstallAsync();
            }
        }

        public async Task UninstallAsync()
        {
            foreach (var addon in Addons)
            {
                await addon.UninstallAsync();
            }

            if (!IsInstalled) { return; }

            var manifest = await ApplicationData.Current.LocalCacheFolder.GetFileAsync("manifest.json");
            await manifest.DeleteAsync();
        }

        private void OnAddonInstallProgress(object sender, InstallProgressChangedEventArgs e)
        {
            InstallProgressChanged?.Invoke(this, e);
        }
    }
}
