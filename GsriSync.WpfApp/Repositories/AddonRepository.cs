using GsriSync.WpfApp.Events;
using GsriSync.WpfApp.Models;
using GsriSync.WpfApp.Repositories.Errors;
using GsriSync.WpfApp.Services;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Windows.Storage;

namespace GsriSync.WpfApp.Repositories
{
    internal class AddonRepository : INotifyInstallProgressChanged
    {
        private const string MANIFEST_FILENAME = "manifest.json";

        private const string STAGE_DOWNLOAD = "Downloading";

        private readonly SettingsService _settings;

        public event InstallProgressChangedEventHandler InstallProgressChanged;

        public AddonRepository(SettingsService settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="addon"></param>
        /// <returns></returns>
        /// <exception cref="RepositoryException{AddonErrors}"></exception>
        public async Task DeleteAsync(Addon addon)
        {
            try
            {
                var file = await ApplicationData.Current.LocalCacheFolder.GetFileAsync(addon.Name);
                await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            catch (FileNotFoundException)
            {
                // File was already deleted, ignore exception
            }
            catch (UnauthorizedAccessException)
            {
                throw new RepositoryException<AddonErrors>(AddonErrors.WritePermissionDenied);
            }
            catch (ArgumentException)
            {
                throw new RepositoryException<AddonErrors>(AddonErrors.InvalidFilename);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <exception cref="RepositoryException{AddonErrors}"></exception>
        public async Task DeleteManifestAsync()
        {
            try
            {
                var file = await ApplicationData.Current.LocalCacheFolder.GetFileAsync(MANIFEST_FILENAME);
                await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            catch (FileNotFoundException)
            {
                // File was already deleted, ignore exception
            }
            catch (UnauthorizedAccessException)
            {
                throw new RepositoryException<AddonErrors>(AddonErrors.WritePermissionDenied);
            }
            catch (ArgumentException)
            {
                throw new RepositoryException<AddonErrors>(AddonErrors.InvalidFilename);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="addon"></param>
        /// <returns></returns>
        /// <exception cref="RepositoryException{AddonErrors}"></exception>
        public async Task DownloadAsync(Addon addon)
        {
            try
            {
                NotifyInstallProgressChanged(addon, 0);
                var file = await ApplicationData.Current.LocalCacheFolder.CreateFileAsync(addon.Name, CreationCollisionOption.ReplaceExisting);
                using var client = new WebClient();
                client.DownloadProgressChanged += (sender, e) => NotifyInstallProgressChanged(addon, e.ProgressPercentage);
                await client.DownloadFileTaskAsync(addon.RemoteUrl, file.Path);
            }
            catch (FileNotFoundException)
            {
                throw new RepositoryException<AddonErrors>(AddonErrors.InvalidFilename);
            }
            catch (UnauthorizedAccessException)
            {
                throw new RepositoryException<AddonErrors>(AddonErrors.WritePermissionDenied);
            }
            catch (WebException)
            {
                throw new RepositoryException<AddonErrors>(AddonErrors.ErrorDownload);
            }
            catch (InvalidOperationException)
            {
                throw new RepositoryException<AddonErrors>(AddonErrors.Threading);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <exception cref="RepositoryException{AddonErrors}"></exception>
        public async Task DownloadManifestAsync()
        {
            try
            {
                var manifest = await ApplicationData.Current.LocalCacheFolder.CreateFileAsync(MANIFEST_FILENAME, CreationCollisionOption.ReplaceExisting);
                using var client = new WebClient();
                client.DownloadFile(_settings.ManifestUrl, manifest.Path);
            }
            catch (UnauthorizedAccessException)
            {
                throw new RepositoryException<AddonErrors>(AddonErrors.WritePermissionDenied);
            }
            catch (WebException)
            {
                throw new RepositoryException<AddonErrors>(AddonErrors.ErrorDownload);
            }
            catch (NotSupportedException)
            {
                throw new RepositoryException<AddonErrors>(AddonErrors.Threading);
            }
        }

        private void NotifyInstallProgressChanged(Addon addon, int progress)
        {
            InstallProgressChanged?.Invoke(this, new InstallProgressChangedEventArgs(addon.Name, STAGE_DOWNLOAD, progress));
        }
    }
}
