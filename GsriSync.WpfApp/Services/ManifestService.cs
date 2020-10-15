using GsriSync.WpfApp.Events;
using GsriSync.WpfApp.Models;
using GsriSync.WpfApp.Repositories;
using System;
using System.Threading.Tasks;

namespace GsriSync.WpfApp.Services
{
    internal class ManifestService : INotifyInstallProgressChanged
    {
        private readonly ManifestRepository _repository;

        private Modpack modpack;

        public bool IsInstalled => modpack?.Local.IsInstalled ?? false;

        public event InstallProgressChangedEventHandler InstallProgressChanged;

        public ManifestService(
            ManifestRepository repository)
        {
            _repository = repository;
        }

        public async Task InstallAsync()
        {
            if (modpack == null) { throw new InvalidOperationException("Verify modpack before installing"); }

            modpack.InstallProgressChanged += OnInstallProgress;
            await modpack.InstallAsync();
            modpack.InstallProgressChanged -= OnInstallProgress;
        }

        public async Task UninstallAsync()
        {
            if (modpack == null) { throw new InvalidOperationException("Verify modpack before uninstalling"); }

            await modpack.UninstallAsync();
        }

        public async Task<bool> VerifyAsync()
        {
            modpack = await _repository.GetModpackAsync();
            return modpack.Sync;
        }

        internal void Play(string arma3Path, string customCliArgs)
        {
            modpack.Local.Server.Play(arma3Path, customCliArgs);
        }

        internal void Talk()
        {
            modpack.Local.Server.Talk();
        }

        private void OnInstallProgress(object sender, InstallProgressChangedEventArgs e)
        {
            InstallProgressChanged?.Invoke(this, e);
        }
    }
}
