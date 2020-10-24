using GsriSync.WpfApp.Models;
using GsriSync.WpfApp.Repositories.Errors;
using GsriSync.WpfApp.Repositories.ManifestProviders;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GsriSync.WpfApp.Repositories
{
    internal class ManifestRepository
    {
        private readonly LocalStrategyProvider _local;

        private readonly RemoteManifestProvider _remote;

        public ManifestRepository(
            LocalStrategyProvider local,
            RemoteManifestProvider remote)
        {
            _local = local;
            _remote = remote;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <exception cref="RepositoryException{ManifestErros}"></exception>
        public async Task<Modpack> GetModpackAsync()
        {
            try
            {
                var local = await _local.ProvideManifestAsync();
                var remote = await _remote.ProvideManifestAsync();
                return new Modpack(local, remote);
            }
            catch (UnauthorizedAccessException)
            {
                throw new RepositoryException<ManifestErrors>(ManifestErrors.CannotReadLocalCacheDir);
            }
            catch (IOException)
            {
                throw new RepositoryException<ManifestErrors>(ManifestErrors.CannotReadLocalCacheFile);
            }
            catch (HttpRequestException)
            {
                throw new RepositoryException<ManifestErrors>(ManifestErrors.NetworkErrorDownloadingManifest);
            }
            catch (JsonException)
            {
                throw new RepositoryException<ManifestErrors>(ManifestErrors.InvalidRemoteManifest);
            }
        }
    }
}
