using GsriSync.WpfApp.Models;
using System.Threading.Tasks;

namespace GsriSync.WpfApp.Services.ManifestProviders
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

        public async Task<Manifest> ProvideManifestAsync()
        {
            return await _file.ProvideManifestAsync()
                ?? await _content.ProvideManifestAsync();
        }
    }
}
