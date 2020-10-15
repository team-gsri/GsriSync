using GsriSync.WpfApp.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GsriSync.WpfApp.Services.ManifestProviders
{
    internal class RemoteManifestProvider : IAsyncManifestProvider
    {
        private readonly HttpClient _http;

        private readonly SettingsService _settings;

        public RemoteManifestProvider(HttpClient http, SettingsService settings)
        {
            _http = http;
            _settings = settings;
        }

        public async Task<Manifest> ProvideManifestAsync()
        {
            var stream = await _http.GetStreamAsync(_settings.ManifestUrl);
            return await JsonSerializer.DeserializeAsync<Manifest>(stream);
        }
    }
}
