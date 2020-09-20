using GsriSync.WpfApp.Models;
using GsriSync.WpfApp.Utils;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace GsriSync.WpfApp.Services
{
    internal class ManifestService
    {
        private readonly HttpClient _http;

        private readonly SettingsService _settings;

        public AsyncLazy<Manifest> LocalManifest { get; private set; }

        public AsyncLazy<Manifest> RemoteManifest { get; }

        public ManifestService(
            HttpClient http,
            SettingsService settings)
        {
            _http = http;
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            LocalManifest = new AsyncLazy<Manifest>(ReadLocalManifestAsync);
            RemoteManifest = new AsyncLazy<Manifest>(ReadRemoteManifestAsync);
        }

        public async Task DownloadRemoteManifestAsync()
        {
            using (var client = new WebClient())
            {
                var file = await ApplicationData.Current.LocalCacheFolder.CreateFileAsync("manifest.json", CreationCollisionOption.ReplaceExisting);
                await client.DownloadFileTaskAsync(_settings.ManifestUrl, file.Path);
            }
            LocalManifest = new AsyncLazy<Manifest>(ReadLocalManifestAsync);
        }

        public async Task UninstallAsync()
        {
            var manifest = await LocalManifest;
            if (!manifest.IsInstalled) { return; }
            await manifest.UninstallAsync();
            LocalManifest = new AsyncLazy<Manifest>(ReadLocalManifestAsync);
        }

        private async Task<Manifest> ReadLocalManifestAsync()
        {
            try
            {
                var file = await ApplicationData.Current.LocalCacheFolder.GetFileAsync("manifest.json");
                var stream = await file.OpenStreamForReadAsync();
                return await JsonSerializer.DeserializeAsync<Manifest>(stream);
            }
            catch (FileNotFoundException)
            {
                return default;
            }
        }

        private async Task<Manifest> ReadRemoteManifestAsync()
        {
            var stream = await _http.GetStreamAsync(_settings.ManifestUrl);
            return await JsonSerializer.DeserializeAsync<Manifest>(stream);
        }
    }
}
