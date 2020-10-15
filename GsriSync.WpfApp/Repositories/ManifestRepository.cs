using GsriSync.WpfApp.Models;
using GsriSync.WpfApp.Services;
using GsriSync.WpfApp.Services.ManifestProviders;
using System.Threading.Tasks;

namespace GsriSync.WpfApp.Repositories
{
    internal class ManifestRepository
    {
        private readonly LocalStrategyProvider _local;

        private readonly RemoteManifestProvider _remote;

        private readonly SettingsService _settings;

        public ManifestRepository(
            SettingsService settings,
            LocalStrategyProvider local,
            RemoteManifestProvider remote)
        {
            _local = local;
            _remote = remote;
            _settings = settings;
        }

        public async Task<Modpack> GetModpackAsync()
        {
            var local = await _local.ProvideManifestAsync();
            var remote = await _remote.ProvideManifestAsync();
            return new Modpack(local, remote, _settings.ManifestUrl);
        }
    }
}
