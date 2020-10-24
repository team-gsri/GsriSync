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

namespace GsriSync.WpfApp.Repositories.ManifestProviders
{
    internal class LocalContentReaderProvider : IAsyncManifestProvider
    {
        private const string MANIFEST_FILE = "manifest.json";

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException">You don't have permission to access the contents of the current folder</exception>
        /// <exception cref="IOException">The file could not be opened or retrieved as a stream</exception>
        public async Task<Manifest> ProvideManifestAsync()
        {
            var files = await ProvideFiles().ToListAsync();
            var manifest = new Manifest { Addons = files, LastModification = DateTimeOffset.Now };
            var file = await ApplicationData.Current.LocalCacheFolder.CreateFileAsync(MANIFEST_FILE, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, JsonSerializer.Serialize(manifest));
            return manifest;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="IOException">The file could not be opened or retrieved as a stream</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "SCS0006:Weak hashing function", Justification = "md5 is faster on large files, and used for change detection rather than integrity control")]
        private async Task<string> GetFileHash(StorageFile file)
        {
            await using var stream = await file.OpenStreamForReadAsync();
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }

        /// <summary>
        /// Returns a list of addon files
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException">You don't have permission to access the contents of the current folder</exception>
        /// <exception cref="IOException">The file could not be opened or retrieved as a stream</exception>
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
