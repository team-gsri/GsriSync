using GsriSync.WpfApp.Events;
using GsriSync.WpfApp.Models;
using GsriSync.WpfApp.Repositories;
using GsriSync.WpfApp.Repositories.Errors;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GsriSync.WpfApp.Services
{
    internal class ManifestService : INotifyInstallProgressChanged
    {
        private readonly AddonRepository _addons;

        private readonly ManifestRepository _manifest;

        private readonly RegistryService _registry;

        private readonly SettingsService _settings;

        private Modpack modpack;

        public bool IsInstalled => modpack?.Local?.IsInstalled ?? false;

        public bool IsSync => modpack?.IsSync ?? false;

        public event InstallProgressChangedEventHandler InstallProgressChanged;

        public ManifestService(
            RegistryService registry,
            SettingsService settings,
            ManifestRepository manifest,
            AddonRepository addons)
        {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));
            _addons = addons ?? throw new ArgumentNullException(nameof(addons));
            _addons.InstallProgressChanged += OnInstallProgress;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <exception cref="RepositoryException{ManifestErros}"></exception>
        /// <exception cref="RepositoryException{AddonErrors}"></exception>
        public async Task InstallAsync()
        {
            if (modpack == null) { modpack = await GetModpackAsync(); }

            foreach (var addon in modpack.GetDeleteAddons())
            {
                await _addons.DeleteAsync(addon);
                await modpack.Delete(addon);
            }

            foreach (var addon in modpack.GetDownloadAddons())
            {
                await _addons.DownloadAsync(addon);
                await modpack.Download(addon);
            }

            await _addons.DownloadManifestAsync();
            modpack.Install();
        }

        public void Play()
        {
            var args = $"{modpack.Local.Server.GetArgs()} {_settings.CustomCliArgs}";
            var exe = $@"{_registry.Arma3Path}\arma3_x64.exe";
            Process.Start(exe, args);
        }

        public void Talk()
        {
            Process.Start(new ProcessStartInfo(modpack.Local.Server.TeamspeakUrl) { UseShellExecute = true, Verb = "open" });
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <exception cref="RepositoryException{ManifestErros}"></exception>
        /// <exception cref="RepositoryException{AddonErrors}"></exception>
        public async Task UninstallAsync()
        {
            if (modpack == null) { modpack = await GetModpackAsync(); }

            foreach (var addon in modpack.Local.Addons)
            {
                await _addons.DeleteAsync(addon);
            }

            await _addons.DeleteManifestAsync();
            modpack = null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <exception cref="RepositoryException{ManifestErros}"></exception>
        public async Task VerifyAsync()
        {
            modpack = await GetModpackAsync();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <exception cref="RepositoryException{ManifestErrors}"></exception>
        private async Task<Modpack> GetModpackAsync()
        {
            return await _manifest.GetModpackAsync();
        }

        private void OnInstallProgress(object sender, InstallProgressChangedEventArgs e)
        {
            InstallProgressChanged?.Invoke(this, e);
        }
    }
}
