using GsriSync.WpfApp.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GsriSync.WpfApp.Repositories.ManifestProviders
{
    internal class LocalStrategyProvider : IAsyncManifestProvider
    {
        private readonly LocalContentReaderProvider _content;

        private readonly LocalFileManifestProvider _file;

        public LocalStrategyProvider(
            LocalFileManifestProvider file,
            LocalContentReaderProvider content)
        {
            _file = file;
            _content = content;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException">You don't have permission to access the contents of the current folder</exception>
        /// <exception cref="IOException">The file could not be opened or retrieved as a stream</exception>
        public async Task<Manifest> ProvideManifestAsync()
        {
            return await _file.ProvideManifestAsync()
                ?? await _content.ProvideManifestAsync();
        }
    }
}
