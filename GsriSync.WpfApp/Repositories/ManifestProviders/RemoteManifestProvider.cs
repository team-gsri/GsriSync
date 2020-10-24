using GsriSync.WpfApp.Models;
using GsriSync.WpfApp.Services;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GsriSync.WpfApp.Repositories.ManifestProviders
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

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <exception cref="JsonException">The Json is invalid</exception>
        public async Task<Manifest> ProvideManifestAsync()
        {
            var stream = await _http.GetStreamAsync(_settings.ManifestUrl);
            return await JsonSerializer.DeserializeAsync<Manifest>(stream);
        }
    }
}
