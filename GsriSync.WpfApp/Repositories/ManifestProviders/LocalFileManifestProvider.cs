using GsriSync.WpfApp.Models;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace GsriSync.WpfApp.Repositories.ManifestProviders
{
    internal class LocalFileManifestProvider : IAsyncManifestProvider
    {
        public async Task<Manifest> ProvideManifestAsync()
        {
            try
            {
                var file = await ApplicationData.Current.LocalCacheFolder.GetFileAsync("manifest.json");
                var stream = await file.OpenStreamForReadAsync();
                return await JsonSerializer.DeserializeAsync<Manifest>(stream);
            }
            catch (Exception ex) when (ex is IOException || ex is JsonException)
            {
                return null;
            }
        }
    }
}
