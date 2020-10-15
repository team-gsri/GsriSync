using GsriSync.WpfApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;

namespace GsriSync.WpfApp.Services.ManifestProviders
{
    internal class LocalContentReaderProvider : IAsyncManifestProvider
    {
        private const string MANIFEST_FILE = "manifest.json";

        public async Task<Manifest> ProvideManifestAsync()
        {
            var files = await ProvideFiles().ToListAsync();
            var manifest = new Manifest { Addons = files, LastModification = DateTimeOffset.Now };
            var file = await ApplicationData.Current.LocalCacheFolder.CreateFileAsync(MANIFEST_FILE, CreationCollisionOption.FailIfExists);
            await FileIO.WriteTextAsync(file, JsonSerializer.Serialize(manifest));
            return manifest;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "SCS0006:Weak hashing function", Justification = "md5 is faster on large files, and used for change detection rather than integrity control")]
        private async Task<string> GetFileHash(StorageFile file)
        {
            await using var stream = await file.OpenStreamForReadAsync();
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }

        private async IAsyncEnumerable<Addon> ProvideFiles()
        {
            foreach (var file in await ApplicationData.Current.LocalCacheFolder.GetFilesAsync(CommonFileQuery.OrderByName))
            {
                if (MANIFEST_FILE.Equals(file.Name, StringComparison.CurrentCulture)) { continue; }
                var relative = Path.GetRelativePath(ApplicationData.Current.LocalCacheFolder.Path, file.Path);
                var properties = await file.GetBasicPropertiesAsync();
                var hash = await Task.Run(() => GetFileHash(file));
                yield return new Addon { Name = relative, Size = properties.Size, Hash = hash };
            }
        }
    }
}
