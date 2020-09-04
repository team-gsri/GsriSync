using GsriSync.WpfApp.Models;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace GsriSync.WpfApp.Services
{
    internal class ManifestService
    {
        private readonly RegistryService _registry = new RegistryService();

        private string LocalManifestPath => $@"{_registry.CurrentPath}\manifest.json";

        public async Task DownloadRemoteManifestAsync()
        {
            using (var client = new WebClient())
            {
                await client.DownloadFileTaskAsync(_registry.ManifestUrl, LocalManifestPath);
            }
        }

        public async Task<Manifest> ReadLocalManifestAsync()
        {
            if (!File.Exists(LocalManifestPath)) { return new Manifest(); }
            return JsonConvert.DeserializeObject<Manifest>(
                await File.ReadAllTextAsync(LocalManifestPath));
        }

        public async Task<Manifest> ReadRemoteManifestAsync()
        {
            using (var client = new WebClient())
            {
                return JsonConvert.DeserializeObject<Manifest>(
                    await client.DownloadStringTaskAsync(_registry.ManifestUrl));
            }
        }
    }
}
