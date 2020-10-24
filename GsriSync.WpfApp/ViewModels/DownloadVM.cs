using GsriSync.WpfApp.Events;
using GsriSync.WpfApp.Repositories.Errors;
using GsriSync.WpfApp.Services;
using GsriSync.WpfApp.Utils;
using GsriSync.WpfApp.ViewModels.Errors;
using System;
using System.Threading;
using System.Threading.Tasks;
using static GsriSync.WpfApp.Events.NavigationChangedEventArgs;

namespace GsriSync.WpfApp.ViewModels
{
    internal class DownloadVM : NotifyPropertyChangedBase
    {
        private readonly ManifestService _manifest;

        private readonly NavigationService _navigation;

        private Task download;

        public string Filename { get; set; }

        public string Message { get; set; }

        public string Operation { get; set; }

        public int Progress { get; set; }

        public DownloadVM(
            NavigationService navigation,
            ManifestService manifest)
        {
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));
            _manifest.InstallProgressChanged += OnDownloadProgress;
        }

        public async Task InstallAsync()
        {
            try
            {
                await _manifest.InstallAsync();
                _navigation.NavigateTo(Pages.Play);
            }
            catch (RepositoryException<ManifestErrors> ex)
            when (ex.Error == ManifestErrors.CannotReadLocalCacheDir)
            {
                SetErrorMessage(ErrorMessages.MSG_CANNOT_IO);
            }
            catch (RepositoryException<ManifestErrors> ex)
            when (ex.Error == ManifestErrors.CannotReadLocalCacheFile)
            {
                SetErrorMessage(ErrorMessages.MSG_CANNOT_IO);
            }
            catch (RepositoryException<ManifestErrors> ex)
            when (ex.Error == ManifestErrors.InvalidRemoteManifest)
            {
                SetErrorMessage(ErrorMessages.MSG_CANNOT_PARSE);
            }
            catch (RepositoryException<ManifestErrors> ex)
            when (ex.Error == ManifestErrors.NetworkErrorDownloadingManifest)
            {
                SetErrorMessage(ErrorMessages.MSG_CANNOT_DOWNLOAD);
            }
            catch (RepositoryException<AddonErrors> ex)
            when (ex.Error == AddonErrors.ErrorDownload)
            {
                SetErrorMessage(ErrorMessages.MSG_CANNOT_DOWNLOAD);
            }
            catch (RepositoryException<AddonErrors> ex)
            when (ex.Error == AddonErrors.InvalidFilename)
            {
                SetErrorMessage(ErrorMessages.MSG_CANNOT_PARSE);
            }
            catch (RepositoryException<AddonErrors> ex)
            when (ex.Error == AddonErrors.Threading)
            {
                SetErrorMessage(ErrorMessages.MSG_THREADING);
            }
            catch (RepositoryException<AddonErrors> ex)
            when (ex.Error == AddonErrors.WritePermissionDenied)
            {
                SetErrorMessage(ErrorMessages.MSG_CANNOT_IO);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"{ErrorMessages.MSG_UNKNOWN} : {ex.Message}");
            }
            finally
            {
                download = null;
            }
        }

        internal void ScheduleDownload()
        {
            download ??= Task.Factory.StartNew(InstallAsync, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void NotifyDownloadProgress()
        {
            foreach (var name in new[] { nameof(Filename), nameof(Operation), nameof(Progress) })
            {
                NotifyPropertyChanged(name);
            }
        }

        private void OnDownloadProgress(object sender, InstallProgressChangedEventArgs e)
        {
            Operation = e.Stage;
            Filename = e.Filename;
            Progress = e.Progress;
            NotifyDownloadProgress();
        }

        private void SetErrorMessage(string message)
        {
            Message = message;
            NotifyPropertyChanged(nameof(Message));
        }
    }
}
