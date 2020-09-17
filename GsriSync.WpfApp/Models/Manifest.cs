using GsriSync.WpfApp.Events;
using GsriSync.WpfApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GsriSync.WpfApp.Models
{
    internal class Manifest : INotifyInstallProgressChanged
    {
        private readonly RegistryService _registry = new RegistryService();

        public IEnumerable<Addon> Addons { get; set; } = new Addon[0];

        public DateTimeOffset LastModification { get; set; }

        public Server Server { get; set; }

        public Teamspeak Teamspeak { get; set; }

        public event InstallProgressChangedEventHandler InstallProgressChanged;

        public void Delete()
        {
            Directory.Delete(_registry.CurrentPath, true);
        }

        public async Task InstallAsync()
        {
            foreach (var addon in Addons)
            {
                addon.InstallProgressChanged += InstallProgressChangedHandler;
                await addon.InstallAsync();
                addon.InstallProgressChanged -= InstallProgressChangedHandler;
            }
            Teamspeak.InstallProgressChanged += InstallProgressChangedHandler;
            await Teamspeak.InstallAsync();
            Teamspeak.InstallProgressChanged -= InstallProgressChangedHandler;
        }

        public void Launch()
        {
            var mods = string.Join(";", Server.Addons.Select(name => Addons.First(addon => addon.Name == name).LocalExpandPath));
            var args = $"-connect={Server.Hostname} -port={Server.Port} \"-mod={mods}\" {_registry.CustomCliArgs}";
            var exe = $@"{_registry.Arma3Path}\arma3_x64.exe";
            Process.Start(exe, args);
        }

        internal void ConnectVocal()
        {
            Process.Start(new ProcessStartInfo(Server.TeamspeakUrl) { UseShellExecute = true, Verb = "open" });
        }

        private void InstallProgressChangedHandler(object sender, InstallProgressChangedEventArgs e)
        {
            InstallProgressChanged?.Invoke(this, e);
        }
    }
}
